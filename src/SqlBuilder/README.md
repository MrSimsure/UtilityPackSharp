# SqlBuilder


## *Enum* SqlFactoryType
- MANUAL
- INSERT
- SELECT
- UPDATE
- DELETE


## *Enum* SqlFactoryParam
- NULL
- PLUS
- MINUS


## *Enum* SqlFactoryJoin
- INNER


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

- **SetParam( *string* index, *object* value, *SqlFactoryParam* type  )** : *SqlFactory*\
Set a parameter inside the command, valid for MANUAL and INSERT type

- **SetWhere( *string* index, *object* value)** : *SqlFactory*\
Set where conditions inside the command, valid for SELECT, DELETE and UPDATE type

- **SetJoin( *SqlFactoryJoin* joinType, *string* tableName, *string* columnSX, *string* columnDX)** : *SqlFactory*\
Set a JOIN inside the command, valid for SELECT type


## Example 
```C#
    // Create a new update on table USERS
    SqlFactory sql = SqlFactory.CreateUpdate("users");

	// Set all the parameters "SET (...) VALUES (...)"
    sql.SetParam("nome",    "mario");
    sql.SetParam("cognome", "rossi");
    sql.SetParam("anni",    30);

    // Set a WHERE
    sql.SetWhere("id",      106);

    // Get the final command 
    // "UPDATE users SET nome='mario', cognome='rossi', anni=30 WHERE id=106;"
    string command = sql.GetCommand();
```