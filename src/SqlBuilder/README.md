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
Returns the command clean of all temporary constructs and ready to be inserted into a database
- **SetParam( *string* index, *object* value, *SqlFactoryParam* type  )** : *SqlFactory*\
Returns the command clean of all temporary constructs and ready to be inserted into a database
- **SetWhere( *string* index, *object* value)** : *SqlFactory*\
Returns the command clean of all temporary constructs and ready to be inserted into a database
- **SetJoin( *SqlFactoryJoin* joinType, *string* tableName, *string* columnSX, *string* columnDX)** : *SqlFactory*\
Returns the command clean of all temporary constructs and ready to be inserted into a database