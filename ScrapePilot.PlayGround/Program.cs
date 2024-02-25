string json = "{\r\n  \"recipes\": [\r\n    {\r\n      \"use\": {\r\n        \"driver\": \"Selenium\",\r\n        \"configs\": {\r\n          \"Headless\": false\r\n        }\r\n      },\r\n      \"instructions\": [\r\n        {\r\n          \"type\": \"nav_to\",\r\n          \"arguments\": {\r\n            \"Url\": \"https://freetestdata.com/document-files/pdf/\"\r\n          }\r\n        },\r\n        {\r\n          \"type\": \"perform_click\",\r\n          \"arguments\": {\r\n            \"On\": \"(//a[contains(@class, \\u0022elementor-button\\u0022)])[1]\"\r\n          }\r\n        },\r\n        {\r\n          \"type\": \"wait_file_download\",\r\n          \"arguments\": {\r\n            \"Src\": \"Free_Test_Data_100KB_PDF.pdf\"\r\n          }\r\n        }\r\n      ]\r\n    }\r\n  ],\r\n  \"output\": {\r\n    \"value\": [ \"#output_path\", \"Free_Test_Data_100KB_PDF.pdf\" ],\r\n    \"type\": \"LOCAL_URL\"\r\n  }\r\n}";

var app = new ScrapePilot.App();
var theurl = app.ProcessRecipe(json);

Console.WriteLine(theurl);


Console.ReadKey();
