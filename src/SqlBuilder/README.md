# SqlBuilder


## *Enum* SqlFactoryType
- MANUAL\
a totaly manual command, the only supported operation is 'SetParam()' that works like a string.replace()
- INSERT\
an insert command structured like 'INSERT INTO table_name (...) VALUES (...);'
- SELECT\
a select command structured like  'SELECT .. FROM table_name;'
- UPDATE\
an update command structured like 'UPDATE table_name SET ...;'
- DELETE\
a delete command structured like  'DELETE FROM table_name ...;'


## *Enum* SqlFactoryParam
- NULL\
when a parameter should be set exactly as the value passed (param = values)
- PLUS\
when a parameter should be set as an addition of the value passed (param += value)
- MINUS\
when a parameter should be set as a subtraction of the value passed (param -= value)


## *Enum* SqlFactoryJoin
- INNER   : in inner join to attach to a command


## *Class* Database

### Methods
- **CreateManual( *string* text )** : *SqlFactory*\
Create and return from zero a SqlFactory

- **CreateSelect( *string*  tableName)** : *SqlFactory*\
Create and return a SqlFactory with a SELECT template

- **CreateInsert( *string*  tableName)** : *SqlFactory*\
Create and return a SqlFactory with an INSERT template

- **CreateUpdate( *string*  tableName)** : *SqlFactory*\
Create and return a SqlFactory with an UPDATE template

- **CreateDelete( *string*  tableName)** : *SqlFactory*\
Create and return a SqlFactory with a DELETE template


- **GetCommand( )** : *string*\
Returns the command clean of all temporary constructs and ready to be inserted into a database


- **SetSelect(  *string* index )** : *SqlFactory*\
Set a select option inside the command, valid for SELECT type

- **SetParam( *string* index, *object* value, *SqlFactoryParam* type)** : *SqlFactory*\
Set a parameter inside the command, valid for MANUAL, INSERT and UPDATE type

- **SetWhere( *string* index, *object* value)** : *SqlFactory*\
Set where conditions inside the command, valid for SELECT, UPDATE and DELETE type

- **SetJoin( *SqlFactoryJoin* joinType, *string* tableName, *string* columnSX, *string* columnDX)** : *SqlFactory*\
Set a JOIN inside the command, valid for SELECT type


## Example 
```C#
    // Create a new update on table USERS
    SqlFactory sql = SqlFactory.CreateUpdate("users");

    // Set all the parameters "param = value,"
    sql.SetParam("nome",    "mario");
    sql.SetParam("cognome", "rossi");
    sql.SetParam("anni",    30);

    // Set a WHERE
    sql.SetWhere("id",      106);

    // Get the final command 
    // "UPDATE users SET nome='mario', cognome='rossi', anni=30 WHERE id=106;"
    string command = sql.GetCommand();
```