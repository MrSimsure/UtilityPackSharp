# Print
Print provides a single static class, with  several static methods that abstract the functions Console.WriteLine() and Console.Write(), this allow to automatically write messages of different colors and disable all commands Print present in the program using a single flag.

## *Class* Print
### Property
- **IsVerbose** : *static public bool*\
Only if set to 'true' all Print methods calls will actually write to console.

- **IsDebug** : *static public bool*\
Only if set to 'true' the "Print.Debug()" calls will actually write to the console.

- **IsGUI** : *static public bool*\
If it is necessary to develop an application with a graphical interface, it is possible to send all the Print methods to some graphical component instead of the console.
To do this, it is necessary to set this property to 'true' and then to define the 'GuiWriteFunction' property of Print.

- **GuiWriteFunction** : *static public Action<Color, string, bool>*\
Define some method to which Print calls are re-routed, the set method must take 3 values as input:
-- Color, the color of the message that is coming
-- string, the text of the message that is coming
-- bool, if the message has the last character as a new line '\ n'


### Methods

- **Error**\
Write a red error message

- **Warning**\
Write a yellow warning message

- **Succes**\
Write a green succes message

- **Note**\
Write a gray note message

- **Message**\
Write a white message

- **Debug**\
Write a cyan debug message

- **Separator**\
Writes a separator line consisting of a defined number of dashes "-----"


## Example 
```C#
	try
	{
		...
		
		Print.Succes("Execution terminated");
		Print.Debug("Ending time: "+DateTime.Now());
	}
	catch()
	{
		Print.Error("Something has gone wrong");
	}
    
```