# Settings
Settings expose a class to easly manage persistent data of a programa, but with  additional options, like crypting the final file, or choosing exactly where place it on disk relative to the program executible.

Every program can create a definition class and then create an instance of Setting passing the definition class as a generic parameter, this allow to work with specific types when reading or writing data using the instance.


## *Enum* SettLocation
 - **ROOT**\
 *C:/*
- **CUSTOM**\
*Custom Path*
- **EXEPOS**\
*../exe_directory/*
- **EXEDIR**\
*../exe_directory/custom_dir* 
- **PROGDATA**\
*C:/ProgramData/*
- **APPDATAROAM**\
*../AppData/Roaming*
- **APPDATALOCA**\
*../AppData/Local* 



## *Class* Settings< T >
### Property
- **data** : *public T*\
Property holding all the data, Loaded after calling "Load()" and saved on disk after calling "Save()"
- **path** : *public string*\
Path where save the setting file. (Default same directory as the exe)
- **name** : *public string*\
Name of the setting file, withotu extension.  (Default "settings")
- **crypt** : *public bool*\
If true, automatically use "SaveCrypt()" when calling "Save()" and "LoadCrypt()" when calling "Load()". (Default false)
- **prettyPrint** : *public bool*\
 If true pretty print the json file

### Methods
- **SetLocation(*SettLocation* location, *string* customDir)** : *void*\
Set the settings file save location.
When location == SettLocation.CUSTOM, the "customDir" parameter work as a full path. In every other cases "customDir" works as an additional final string to the base path.

- **Load()**
Load the settings from the file and parse them to the "data" property.
If the file is not present it will be created with the default parameters

- **Save()**
Save the settings data to file

- **LoadCrypt()**
Load the settings from the file, decrypt and parse them to the "data" property.
If the file is not present it will be created with the default parameters.
Automaticlay done when calling "Load()" if "crypt" is set to true

- **SaveCrypt()**
Save the settings data to file, but crypting them, making it unreadable to human.
Automaticlay done when calling "Save()" if "crypt" is set to true


## Example 
```C#
    // Definition class with default values
    public class ProgramData 
    {
        public bool   isActive { get; set;} = false;
        public string Name     { get; set;} = "Michael";
        public int    Uses     { get; set;} = 123;
    }

    class Program
    {
        // Create the Setting instance, passing the definition class as generic parameter
        // Making the instance static allow us to access data from every where in the program
        public static Settings<ProgramData> setting = new();

        static void Main()
        {
            // Choose setting configuration ad load the data
            setting.crypt = true;
            setting.SetLocation(SettLocation.PROGDATA, "ProgramName/");
            setting.Load();
          
            // Change a some values
            setting.data.Uses += 1;

            // Save the setting to file
            setting.Save();

            // Read a value from the setting data
            string name = setting.data.Name;
        }
    }
```