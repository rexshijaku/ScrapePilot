# ScrapePilot

This library helps to scrape web content in a guided and user-friendly way. It aims to simplify repetitive and complex scraping tasks by combining and abstracting the functionality offered by HTMLAgilityPack and Selenium with the extended C# I/O tasks.

## How it works
Scrape Pilot uses a JSON file as an input, which contains a list of recipes that guides what Pilot should do, and each recipe has its own instructions. An Instruction represents a single unit of tasks and is made of Arguments. The recipe examples can be found in the <a href='https://github.com/rexshijaku/ScrapePilot/tree/master/ScrapePilot/Examples'>Examples</a> folder. Besides, this repo also provides a <a href='http://152.70.176.144/'>tool</a> (<a href='https://github.com/rexshijaku/ScrapePilot/tree/master/ScrapePilot.Client'>the ScrapePilot.Client Project</a>) which alleviates the recipe creation process.

### Usage
```csharp
 var app = new ScrapePilot.App(); // Create an instance of the ScrapePilot
 
 string recipeJSON = "<json string here>"; // Set the Recipe JSON here
 
 List<List<string>> table; // The Output is expected to be a list of lists (matrix | table)
 
 ProcessResponse result = app.ProcessRecipe(recipeJSON); // Process the Recipe 
 
 // Handle the output
 if (result.Type == ScrapePilot.Constants.OutputType.TABLE_DATA_JSON)
 {
     // All as expected now deserialize the result
     table = JsonSerializer.Deserialize<List<List<string>>>(result.Value);
 }
 else
 {
     // Something Went Wrong!
 }
```
### appsettings.json
The App Constructor of the Scrape Pilot may take an IConfigurationSection as a parameter. This Configuration is optional. Please refer to the <a href='https://github.com/rexshijaku/ScrapePilot/blob/master/ScrapePilot/AppConfiguration.cs'> AppConfiguration.cs </a> to see what properties you may use in your appsettings.json for the Scrape Pilot.

<br>

### The Main Recipe Structure

The Main Recipe is the recipe that contains subrecipes. It has a global store and variables.

#### The Store
The store is an in-memory storage that contains the results of the storable instructions. In technical terms, it is an implementation of a key-value C# Dictionary<String, String>, where the key corresponds to the 'name' property of the 'store' object. Once a value is stored, it can be used in subsequent steps throughout the script. However, certain instructions, such as basic tab changes, are void and may not necessitate storing their results.

#### A Variable
An element that can be declared and used within the main JSON file is indicated by a preceding hash (#) string. Once a variable is introduced in a step, it can be used in subsequent steps. See below how '#a_variable_example' is used as part of an output string, it was introduced earlier when used to store an instruction result. Note that if a variable is used without the hash (#), it won't be stored in The Store and therefore won't be available when referred to.

#### The General Structure
The Main Recipe contains Multiple Recipe Items and gives a single Output at the end of the whole process. An example of the Main Recipe Structure is shown as follows:
```js
// An Example of the Main Recipe
{
  "recipes": [
    { // #Recipe1 
      "use": {
       ... // #Recipe1 Options 
      },
      "instructions": [
       ... //  #Recipe1 Instructions
       // Instruction 1.
       {
          // Instruction 1. Type
          // Instruction 1. Arguments
          "store": {
            "name": "#a_variable_example"
          }
        }
      ]
    }
    ... // The following Recipe (Should be Added Only If Driver Change is Needed)
  ],
  "output": {
      "type": "", // An Output type from Constants.OutputType
      "value":  [ "#a_variable_example" ] // The Specified Output
      // Note that #a_variable_example is Created/Stored during the execution of Instructions
  }
}
```

You can override appsettings.json properties inside the recipe file as below. When overridden, the values of the variables from the appsettings.json will be ignored.

```js
{
   "recipes": [],
   "configs": { // Override appsettings.json Configuration
      "OutputPath": "some_path",
      "verbose": false
   },
   "output": {}
}
```

#### A Single Recipe Structure
The Main Components of a single Recipe are:
- [Use] - the Options Specified for a Recipe
- [Instructions] - a List of Instructions or Steps that Recipe should follow

#### [Use]
This component contains the options where the Driver and its Configuration are specified. The possible Driver Types are located in <a href='https://github.com/rexshijaku/ScrapePilot/blob/master/ScrapePilot/Constants/RecipeDriverType.cs'>Constants.RecipeDriverType.cs</a>, and configuration properties for each Driver type are specified in individual files within <a href='https://github.com/rexshijaku/ScrapePilot/tree/master/ScrapePilot/Models/Configs'> Models.Configs </a>.

```js
"use": {
   "driver": "HTMLap", // The Driver Type Used in Recipe is HtmlAguilitiPack
   "configs": {  // The Driver Configuration
    "EncodingName": "utf-8"
   }
 }
```

#### [Instructions]
A List of Instructions or Steps that each Recipe should follow.
```js
// An Example of a single Instruction, 
// Its result is stored in a variable called 'extracted_file_url' 
{
  "type": "extract_attr",
  "arguments": {
    "From": "//a[text() = 'The element which downloads a file.']",
    "Attr": "href",
    "Constraints": [ "must_be_csv_file" ]
  },
  "store": {
    "name": "#extracted_file_url"
  }
}
```

Tip: In the code, each Instruction, is decorated with <b>InstructionDetails</b> attribute (see an example <a href='https://github.com/rexshijaku/ScrapePilot/blob/master/ScrapePilot/Models/Instruction/Html/ExtractAttr.cs'>here</a>), and this is where is documented whether the instruction should be stored or not. It also has a description which shows which field is stored. When using the tool introduced at the beginning, it will take care of such things and prevent the user from creating inappropriate parts of instructions.

#### [Instruction] Type
There are different Instruction Types for each Driver. These can be found  <a href='https://github.com/rexshijaku/ScrapePilot/tree/master/ScrapePilot/Constants/InstructionType'> here </a> for each driver.

#### [Instruction] Arguments
Each Instruction Type has different arguments, in the code you can see the Instruction Properties (which are the arguments) inside the Type of Instruction that you want to use. The previous example used the arguments for the ExtractAttr Instruction which resides in the  Models.Instruction.Selenium namespace.

Name | Description 
--- | --- 
From | The Element which contains the Attribute
Attr | The Attribute from which the value is extracted
Constraints | The Conditions that the resulting value must respect
Store-Name | The Name of the Variable which will contain the Instruction Result 

#### [Instruction] Value
Some Instruction Types may have Arguments that use predefined constant values, meaning they can't accept variables or hand-written values, these instruction types can be found in Constants.InstructionValue.{DriverName} folders. An example of this can be the Switch Tab Instruction Type which only navigates to specified browser tabs such as first, last, etc.

### Functions
Similar to variables, Functions can also be employed in constructing the result of an Instruction, the value of a specific type of Argument, or the Main Output value. The value of the function can be utilized in the mentioned points, and when that specific point is processed by the parser it will get what the function outputs. An example of the use of the Functions is shown below.

#### Independent Functions
These types of functions are not dependent on any other argument or variable inside the instruction or script. Examples of such functions are The Current DateTime or Blank Space Generator. The full list is available in Constants.IndependentFunctions namespace.

#### Dependent Functions
These types of functions are dependent on the value of some other property or variable. Note the relationship between the function name and the used argument is specified inside GetArgumentables() function of the specified Instruction. Currently, only one argument is supported. The full list of possible Dependent Functions is available in Constants.DependentFunctions namespace. 

````csharp
[InstructionArgumentDetails(attributeType: AttributeType.StoreKeyOrRawValue)]
[InstructionArgumentClientDetails(name: "From", description: "The path where the file is located.")]
public required string From { get; set; }

// The Argument that holds the path where a file should be stored and is constructed depending on the FROM argument of the same Instruction
// The TO parameter, as you can see can use an independent function or a dependent function that processes the value of the To

[InstructionArgumentDetails(attributeType: AttributeType.ListAsRawOrFromStore)]
[InstructionArgumentClientDetails(name: "To", description: "The new path where the file should be moved.")]
[CanUseDependentFunction(referToField: nameof(From))]
[CanUseInDependentFunction]
public List<string>? To { get; set; }

````
And in the recipe:
```js
 // An Example where Independent and Dependent functions are used
 {
   "type": "download_a_file",
   "arguments": {
     "From": "#extracted_file_url",
     "To": [ "Test", "fn_space", "fn_dateTime-yyyyMMdd", "fn_fileName-fromFullName" ]
   }
 }
 
 // Here fn_space and fn_dateTime-yyyyMMdd are Independent, they will output a static value without depending on some other parameter
 // However, fn_fileName-fromFullName will give the file name stored in the "From" property

 // let assume fn_space is  = ' ' and fn_dateTime-yyyyMMdd = '20231007' 
 // #extracted_file_url = 'some_remote_source/file_name.csv'
 // and fn_fileName-fromFullName = 'file_name.csv'
 // The To Property will be 'Test 20231007file_name.csv'
```


#### The Output
It contains a type, which shows what type the expected recipe result is. See the possible Output Types <a href='https://github.com/rexshijaku/ScrapePilot/blob/master/ScrapePilot/Constants/OutputType.cs'>here.</a> . The Output also has the value part, which contains a list of fragments that are concatenated to create the final result. This list can include handwritten values (raw), variables, or independent functions as well.

```js
{
  "recipes": [
    ...
  ],
  "output": { 
    "type": "table_data_json",
    "value": [ "#a_variable_name" ]
  } // The Specified Output
}
```

### Support
For general questions about ScrapePilot, tweet at @rexshijaku or write me an email at rexhepshijaku@gmail.com.

### Author
##### Rexhep Shijaku
 - Email : rexhepshijaku@gmail.com
 - Twitter : https://twitter.com/rexshijaku

### Acknowledgments
Special thanks to <a href='https://www.tm-tracking.org/'>Trygg Mat Tracking (TMT)</a> for supporting this project.

Contributing
We welcome contributions from everyone! Here are a few ways you can help improve this project:

Reporting Bugs: If you encounter any bugs or unexpected behavior, please open an issue on GitHub. Be sure to include as much detail as possible, including steps to reproduce the issue.

Suggesting Enhancements: Have an idea for a new feature or improvement? Feel free to open an issue to discuss it, or even better, submit a pull request with your proposed changes.

Submitting Pull Requests: Found a bug and know how to fix it? Want to add a new feature? Pull requests are welcome! Please follow the guidelines below:

Fork the repository and create your branch from the main.
Make your changes, ensuring they follow our coding conventions and style guide.
Write tests for any new functionality and ensure all tests pass.
Update the documentation to reflect your changes if necessary.
Open a pull request, describing the changes you've made.

Documentation: Improving the documentation is always appreciated. If you notice areas where the documentation could be clearer or more comprehensive, please let us know or submit a pull request with your proposed changes.

Spread the Word: If you find this project useful, consider sharing it with others who might benefit from it. You can also star the repository on GitHub to show your support.

By contributing to this project, you agree to abide by the Code of Conduct. Thank you for helping to make this project better!

### License
MIT License

Copyright (c) 2024 | Rexhep Shijaku & Trygg Mat Tracking (TMT) 

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
