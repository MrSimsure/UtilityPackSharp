# Database

## *Enum* DbSystem
- SQL_SERVER
- FIREBIRD

## *Class* Database
An instance of this class equal to a connection to a Database.

### Property
- **CommandTimeout** : *public int*
Time for every command to run before calling an error (Default=0, unlimited time)

- **system** : *{get public; set private} DbSystem*
Type of this database


### Methods
- **ChangeDatabase( *string* name )** : *void*
Change the connected database

- **TestConnection( *bool*  print)** : *bool*
Return true if the connection succeed, false otherwise. 
If print is true, write the result on console.

- **ExecuteSqlQuery( *string* sql )** : *DataTable*
 Execute a query on the database and return the result in form of a DataTable

- **ExecuteSqlCommand( *string* sql )** : *int*
Execute a command on the database, return the number of affected rows


### Static Methods
- **ParseToNumber\<T\>(*object* value, *T* standard, *DbParsingOption* options)** : *T*
Parse a recived object from a query to a numeric value.
"T" is the Type of numeric value to return.
If something happen, return the value passed in "standard".
To change the parsing behaviour customize the values of the "options" object.

- **ParseToString(*Object* value, *string* standard, *DbParsingOption* options)** : *string*
Parse a recived object from a query to a string.
If something happen, return the value passed in "standard".
To change the parsing behaviour customize the values of the "options" object.


## *Class* DbParsingOption
Class used in static parse methods of Database class, to customize the parsing operation.

### Property
- **trim** : *bool*
If true execute a trim on the obtained string

- **returnNull** : *bool*
If true, allow the function to return null if database value is null

- **decimalDiv** : *string*
Specify the division simbol if the value is a float and must be returned as a string

- **decimalNumber** : *int*
Specify the number of decimal values