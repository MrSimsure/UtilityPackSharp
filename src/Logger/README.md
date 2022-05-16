# Logger

Logger provides a single static class, with several static methods that take care of performing rapid writing of files to disk in predefined places.

## *Enum* LogLocation
 - **ROOT**\
 *C:/*
- **CUSTOM**\
*Custom Path*
- **EXEDIR**\
*../exe_directory/* 
- **PROGDATA**\
*C:/ProgramData/*
- **APPDATAROAM**\
*../AppData/Roaming*
- **APPDATALOCA**\
*../AppData/Local* 

## *Class* Logger

### Property
- **IsLogActive** : bool\
Only if set to 'true' the Logger methods will actually save files to disk.
(Default true)

- **IsCatchErrorActive** : bool\
If true, when an error occur it will be thrown, otherwhise the functions will silently return false.
(Default false)

- **BasePath** : string\
Root directory where to save the logs.
(Default same directory as the exe) 

- **SubPath** : string\
Additional sub directories where save the logs, added at the end of BasePath. 
(Default "")

- **MaxFileSize** : long\
Max size of a log file, if the file exceed this value and "IsSizeLimitActive" is set to true, the file will be cleared.
(Default value = 5 MB = 1048576 Byte)

- **IsSizeLimitActive** : boolean\
If set to true, when a file exceed the "MaxFileSize" it will be cleared at the next log saving.

- **LogAppendSpace** : int\
Number of new line beetween a content append and the next one. 
(Default 3)


### Method
- **SetLocation(*LogLocation* location, *string?* customDir)**\
Set the log save location

- **SetSubLocation(*string* subLocation)**\
Set the log folder sub location 

- **SetMaxFileSize(*long* maxByteSize)**\
Activate the size limit control and set the "MaxFileSize" value to as pecific number


- **SaveText(*string* text, *string?* name , *bool?* append)**\
Save some text to file in the log directory as a file .txt 

- ****SaveJson(*string* text, *string?* name , *bool?* append)****\
Save some json text to file in the log directory as a file .json 

- ****SaveJson(*object* text, *string?* name , *bool?* append)****\
Save a json object to file in the log directory as a file .json 

- ****SaveJsonList< T >(*List< T >* list, *string?* name , *bool?* append)****\
Save a list of json objects to file in the log directory as a file .json 


- **ClearLogFolder(*string?* chosenPath)**\
 Delete every file inside the log folder, if another folder BasePath is specified, clear it instead


## Example 
```C#
class jsonObj
{
	public string Name;
	public int Age;
}

static void Main()
{
	// Set the main log folder location
	Logger.SetLocation(LogLocation.APPDATALOCA, "ProgramLogs");

	// Save a simple text message on a report.txt file located in the log folder
	Logger.SaveText("Report message", "report");

	// Create and append a json like object to a report_data.json file located in the log folder
	jsonObj json = new()
	{
		Name = "Burt",
		Age  = 20
	};

	Logger.SaveJson(json, "report_data", true);
}
```