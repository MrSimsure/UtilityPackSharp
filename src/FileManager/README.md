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

- **Write< T >(*string* path, *List< T >* prodotti)** : *string*\
Write a list of instance to a csv file

- **Write< T, M >(*string* path, *List< T >* prodotti)** : *string*\
 Write a list of instance to a csv file, with a specific class map to define specific behaviour

### Example

```C#
	// Define and create an instance to rappresent a csv row
	class Item
	{
		public string Name;
		public int Age;
	} 
	
    Lsit<Item> list = new();

	// Read the csv content from file and create a list
	list = CsvManager.Read<Item>("C:\example.csv");

	// Modify the list and save it back as csv
	list.RemoveAt(0);
	
	CsvManager.Write<Item>("C:\example.csv", list);
	
```

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

### Example

```C#

	
```

## *Class* RegistryManager

### Methods
- **Read(*RegistryHive* root, *string* key, *string* subKey)** : *string*\
Read a value from a specific section of the registry

### Example

```C#

	
```
