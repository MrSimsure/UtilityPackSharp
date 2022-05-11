# FileManager
This package offer different static classes to manage data across file types, it currently support:
- Writing and Reading **CSV**
- Writing and Reading **INI**
- Reading **Windows Registry**


## *Class* CsvManager
### Property
- **delimiter** : *public string*\
Delimiter used to separate columns in the csv files. (Deafault ;)

- **culture** : *public CultureInfo*\
Culture to write csv files

- **createFolders** : *public bool*\
If true create every unexisting directory when passing a csv path. (Deafault true)

### Methods
- **Read< T >(*string* path)** : *List< T >*\
Create a list of values reading them from a csv file

- **Write< T >(*SettLocation* location, *string?* customDir)** : *string*\
Write a list of instance to a csv file

- **Write< T, M >(*SettLocation* location, *string?* customDir)** : *string*\
 Write a list of instance to a csv file, with a specific class map to define specific behaviour


## *Class* IniManager
### Property
- **printErrors** : *public bool*\
If true print errors, when a file is not found or a section or key are missing

- **forceWrite** : *public bool*\
If true create section and key when they are not found during writing


### Methods
- **Read< T >(*string* filePath, *string* section, *string* key, *T* defaul)** : *T*\
Read a value from a file ini, if some error occours return the default value passed instead

- **Write(*string* filePath, *string* section, *string* key, *object* value)** : *bool*\
Read a value from a file ini, if some error occours return the default value passed instead


## *Class* RegistryManager

### Methods
- **Read(*RegistryHive* root, *string* key, *string* subKey)** : *string*\
Read a value from a specific section of the registry



