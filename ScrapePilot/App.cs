using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using HtmlAgilityPack;


using HtmlExtractAttr = ScrapePilot.Models.Instruction.Html.ExtractAttr;
using SeleniumExtractAttr = ScrapePilot.Models.Instruction.Selenium.ExtractAttr;
using ScrapePilot.Constants;
using ScrapePilot.Constants.InstructionType;
using ScrapePilot.Models.Recipe;
using ScrapePilot.Models.Instruction.AppDriver;
using ScrapePilot.Models.Instruction.Html;
using ScrapePilot.Models.Instruction.Selenium;
using Microsoft.Extensions.Configuration;
using ScrapePilot.Models;
using ScrapePilot.Helpers;
using ScrapePilot.Models.Configs;
using System.Text.Json;
using ScrapePilot.Attributes;

namespace ScrapePilot
{
    public class App
    {
        private AppConfiguration? _appConfiguration;

        private InstructionMethodsSelenium _seleniumActions;
        private InstructionMethodsHtml _htmlActions;
        private InstructionMethodsAppDriver _appDriverActions;

        public static ConsolePrinter _consolePrinter = new ConsolePrinter(); // TODO Remove this! But still gives warning

        public App(IConfigurationSection? configuration = null)
        {
            _appConfiguration = configuration?.Get<AppConfiguration>();
            _consolePrinter = new ConsolePrinter(_appConfiguration?.Verbose);

            _seleniumActions = new InstructionMethodsSelenium();
            _htmlActions = new InstructionMethodsHtml();
            _appDriverActions = new InstructionMethodsAppDriver();
        }


        /*
         * Returns the File Name Which was Downloaded or Created depending on Main Recipe provided in file located in path variable.
         * or Returns the JSon String which was extracted depending on Main Recipe provided in file located in path variable.
         * 
         * Returns Empty if Something goes wrong.
         */
        public ProcessResponse ProcessRecipe(string recipeJson)
        {
            ProcessResponse theOutput = new ProcessResponse();



            RecipeMain? main_recipe;

            try
            {
                main_recipe = JsonSerializer.Deserialize<RecipeMain>(recipeJson);
            }
            catch
            {
                throw new Exception($"The Given Content found to be an Invalid JSON.");
            }

            if (main_recipe == null)
            {
                throw new Exception($"The Given Recipe Content of found to be null after Deserializion.");
            }

            // set output path based on what provided in configuration
            string? output_path = _appConfiguration?.OutputPath;

            if (string.IsNullOrEmpty(output_path)) // if no path provided use tmp
            {
                output_path = Path.GetTempPath();
                _consolePrinter.Print($"No Output path was found... Using: {output_path}", ConsoleColor.Cyan);
            }

            string outputPathVarName = AttrHelper.GetCustomAttribute<AppVariable>(typeof(AppConfiguration),
                nameof(AppConfiguration.OutputPath)).VariableNameInRecipe;

            if (main_recipe.configs != null && main_recipe.configs.ContainsKey(outputPathVarName))
            {
                output_path = main_recipe.configs[outputPathVarName];
            }

            if (!FileHelper.ValidDir(output_path))
            {
                throw new Exception($"The Specified Output path {output_path} does not exists!");
            }

            Store.Reset();

            Store.SetValue(outputPathVarName, output_path);

            HtmlDocument loadedDocument = new HtmlDocument();

            foreach (var recipe in main_recipe.recipes)
            {
                recipe.use.SayUsingDriver();

                if (recipe.use.driver == RecipeDriverType.SELENIUM)
                {
                    ChromeDriver driver = new ChromeDriver(_seleniumActions.GetDriverConfigs(
                        recipe.use.GetConfigs<SeleniumDriverConfigs>(),
                        output_path));

                    foreach (Instruction instruction in recipe.instructions)
                    {
                        instruction.SayPerforming();

                        switch (instruction.type)
                        {
                            case SeleniumInstuctionType.NAVIGATE_TO:
                                _seleniumActions.Nav_To(driver, instruction.GetArguments<NavTo>());
                                break;
                            case SeleniumInstuctionType.PERFORM_CLICK:
                                _seleniumActions.Perform_Click(driver, instruction.GetArguments<PerformClick>());
                                break;
                            case SeleniumInstuctionType.EXTRACT_MULTI_PAGE_TABLE_DATA:
                                if (instruction.resultIsStorable())
                                {
                                    List<List<string>> data = _seleniumActions.GetMultiPageHtmlTableData(driver, instruction.GetArguments<Models.Instruction.Selenium.ExtractMultiPageTableData>(), _htmlActions);
                                    Store.SetValue(instruction.store?.name, JsonSerializer.Serialize(data),
                                      $"{instruction.type}/{SeleniumInstuctionType.EXTRACT_MULTI_PAGE_TABLE_DATA}");
                                }
                                break;
                            case SeleniumInstuctionType.SWITCH_TAB:
                                _seleniumActions.Switch_Tab(driver, instruction.GetArguments<SwitchTab>());
                                break;
                            case SeleniumInstuctionType.EXTRACT_ATTR:
                                if (instruction.resultIsStorable())
                                {
                                    Store.SetValue(instruction.store?.name,
                                        _seleniumActions.ExtractAttr(driver, instruction.GetArguments<SeleniumExtractAttr>()),
                                        $"{instruction.type}/{SeleniumInstuctionType.EXTRACT_ATTR}");
                                }
                                break;
                            case SeleniumInstuctionType.WAIT_FILE_DOWNLOAD:
                                _seleniumActions.Wait_File_Download(driver, instruction.GetArguments<WaitForDownload>());
                                break;
                            default:
                                break;
                        }
                    }

                    if (driver != null)
                    {
                        driver.Dispose();
                    }
                }
                else if (recipe.use.driver == RecipeDriverType.HtmlAguilitiPack)
                {
                    _htmlActions.ApplyDriverConfigs(recipe.use.GetConfigs<HTMLDriverConfigs>());

                    foreach (Instruction instruction in recipe.instructions)
                    {
                        instruction.SayPerforming();

                        switch (instruction.type)
                        {
                            case HtmlInstuctionType.LOAD_PAGE:
                                LoadPage loadPage = instruction.GetArguments<LoadPage>();
                                loadedDocument = _htmlActions.Load(loadPage.Url);
                                break;
                            case HtmlInstuctionType.EXTRACT_ATTR:
                                if (instruction.resultIsStorable())
                                {
                                    Store.SetValue(instruction.store?.name, _htmlActions.GetAttr(loadedDocument, instruction.GetArguments<HtmlExtractAttr>()),
                                        $"{instruction.type}/{HtmlInstuctionType.EXTRACT_ATTR}");
                                }
                                break;
                            case HtmlInstuctionType.EXTRACT_TABLE_DATA:
                                if (instruction.resultIsStorable())
                                {
                                    List<List<string>> data = _htmlActions.GetHtmlTableData(loadedDocument, instruction.GetArguments<ExtractTableData>());
                                    Store.SetValue(instruction.store?.name, JsonSerializer.Serialize(data),
                                        $"{instruction.type}/{HtmlInstuctionType.EXTRACT_TABLE_DATA}");
                                }
                                break;
                            case HtmlInstuctionType.EXTRACT_MULTI_PAGE_TABLE_DATA:
                                if (instruction.resultIsStorable())
                                {
                                    List<List<string>> data = _htmlActions.GetMultiPageHtmlTableData(instruction.GetArguments<Models.Instruction.Html.ExtractMultiPageTableData>());
                                    Store.SetValue(instruction.store?.name, JsonSerializer.Serialize(data),
                                        $"{instruction.type}/{HtmlInstuctionType.EXTRACT_MULTI_PAGE_TABLE_DATA}");
                                }
                                break;
                            case HtmlInstuctionType.LOOP_A_TABLE:
                                LoopTable loopTableArgs = instruction.GetArguments<LoopTable>();
                                string table = _htmlActions.GetTableWithDetails(loadedDocument, loopTableArgs);
                                if (instruction.resultIsStorable())
                                {
                                    Store.SetValue(instruction.store?.name, table);
                                }
                                break;

                            default:
                                break;
                        }
                    }
                }
                else if (recipe.use.driver == RecipeDriverType.AppDriver)
                {
                    foreach (Instruction instruction in recipe.instructions)
                    {
                        instruction.SayPerforming();

                        switch (instruction.type)
                        {
                            case AppDriverInstructionType.DOWNLOAD_A_FILE:

                                DownloadAFile downloadAndSave = instruction.GetArguments<DownloadAFile>();

                                string savedToPath = _appDriverActions.DownloadAFile(downloadAndSave).Result;
                                if (instruction.resultIsStorable())
                                {
                                    Store.SetValue(instruction.store?.name, savedToPath, $"{instruction.type}/{AppDriverInstructionType.DOWNLOAD_A_FILE}");
                                }

                                break;
                            case AppDriverInstructionType.MOVE_FILE:
                                string movedTo = _appDriverActions.MoveFile(instruction.GetArguments<MoveFile>());
                                if (instruction.resultIsStorable())
                                {
                                    Store.SetValue(instruction.store?.name, movedTo);
                                }
                                break;
                            case AppDriverInstructionType.SAVE_TXT_FILE:
                                SaveTextFile saveTextFile = instruction.GetArguments<SaveTextFile>();
                                if (!Store.IsAvailable(saveTextFile.UseContentOf))
                                {
                                    throw new Exception($"Invalid Action Argument Declaration. {saveTextFile.UseContentOf} in {instruction.type}/{AppDriverInstructionType.SAVE_TXT_FILE} was not set.");
                                }
                                else
                                {
                                    string savedTo = _appDriverActions.CreateTxtFile(saveTextFile);
                                    if (instruction.resultIsStorable())
                                    {
                                        Store.SetValue(instruction.store?.name, savedTo, $"{instruction.type}/{AppDriverInstructionType.SAVE_TXT_FILE}");
                                    }
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            _consolePrinter.Print("Formulating the result...");

            // set the final result
            theOutput.Value = main_recipe.getOutputValue();
            theOutput.Type = main_recipe.output.type;

            _consolePrinter.Print("The process is done...");

            if (theOutput.Value.Length > 100)
            {
                _consolePrinter.Print($"The results first 100 characters: {theOutput.Value.Substring(0, 100)}...");
            }
            else
            {
                _consolePrinter.Print($"The result: {theOutput.Value}");
            }

            return theOutput;
        }
    }
}