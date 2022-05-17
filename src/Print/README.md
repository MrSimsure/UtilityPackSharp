# Print
Print provides a single static class, with  several static methods that abstract the functions Console.WriteLine() and Console.Write(), this allow to automatically write messages of different colors and disable all commands Print present in the program using a single flag.

## *Class* Print
### Property
- **IsActive** : *static public bool*\
Only if set to 'true' all Print methods calls will actually write to console.

- **IsVerbose** : *static public bool*\
Only if set to 'true' all the Verbose methods will actually write to the console.

- **IsDebug** : *static public bool*\
Only if set to 'true' the "Print.Debug()" calls will actually write to the console.

- **IsGUI** : *static public bool*\
If it is necessary to develop an application with a graphical interface, it is possible to send all the Print methods to some graphical component instead of the console.
To do this, it is necessary to set this property to 'true' and then to define the 'GuiWriteFunction' property of Print.

- **GuiWriteFunction** : *static public Action<Color, string, bool>*\
Define some method to which Print calls are re-routed, the set method must take 3 values as input:\
-- Color, the color of the message that is coming\
-- string, the text of the message that is coming\
-- bool, if the message has the last character as a new line '\ n'


### Methods

- **Error(*Object* text, *bool* line)**\
Write a red error message

- **Warning(*Object* text, *bool* line)**\
Write a yellow warning message

- **Succes(*Object* text, *bool* line)**\
Write a green succes message

- **Note(*Object* text, *bool* line)**\
Write a gray note message

- **Message(*Object* text, *bool* line)**\
Write a white message

- **Debug(*Object* text, *bool* line)**\
Write a cyan debug message

- **Separator(*int* linsegmentse)**\
Writes a separator line consisting of a defined number of dashes "-----"

.

- **ErrorVerb(*Object* text, *bool* line)**\
Write a red error message, only when "IsVerbose" is true

- **WarningVerb(*Object* text, *bool* line)**\
Write a yellow warning message, only when "IsVerbose" is true

- **SuccesVerb(*Object* text, *bool* line)**\
Write a green succes message, only when "IsVerbose" is true

- **NoteVerb(*Object* text, *bool* line)**\
Write a gray note message, only when "IsVerbose" is true

- **MessageVerb(*Object* text, *bool* line)**\
Write a white message, only when "IsVerbose" is true

- **DebugVerb(*Object* text, *bool* line)**\
Write a cyan debug message, only when "IsVerbose" is true

- **SeparatorVerb(*int* linsegmentse)**\
Writes a separator line consisting of a defined number of dashes "-----", only when "IsVerbose" is true


## Example 
```C#
	Print.IsVerbose = true;
	Print.IsDebug = true;
	
	try
	{
		...
		
		Print.Succes("Execution terminated");
		Print.Debug("Ending time: "+DateTime.Now());
	}
	catch(Exception ex)
	{
		Print.Error("Something has gone wrong");
		Print.ErrorVerb(ex.ToString());
	}
    
```