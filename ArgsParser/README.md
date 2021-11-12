# ArgsParser
---------------
## Esempio
Esempio di programma da terminale: comando per rimpiazzare una sotto stringa di una stringa

**comando**   : replace\
**parametri** : stringa di base, stringa da rimpiazzare, stringa nuova\
**opzioni**   : help, all\
\
\
**Definizione degli argomenti**
```C#
static void Main(string[] args)
{
    //crea una nuova instanza del parser
    ArgsParser parser = new();

    //definisci un singolo o un array di comandi
    parser.AddCommand(new[]
    {
        new ArgsCommand("replace")
        {
            ValidParameters = {"replaceParam"},
            ValidOptions    = {"help", "all"}
        }
    });

    //definisci un set di parametri da attribuire al comando
    parser.AddParameterGroup("replaceParam", new[]
    {
        new ArgsParameter("base_string"),
        new ArgsParameter("to_replace"),
        new ArgsParameter("new_string"),
    });

    //definisci le opzioni
    parser.AddOption(new[]
    {
        new ArgsOption("help") { Alias = "h",  IsFlag = true},
        new ArgsOption("all")  { Alias = "a",  IsFlag = true}
    });

    //esegui il parsing partendo dagli argomenti ottenuti dal main
	//preferibilmente all'interno di un try-catch per arrestare il processo in caso di errore
    try
	{ 
		parser.Parse(args); 
	}
	catch (ArgsParseErrorException e) 
	{ 
		Console.WriteLine(e.StackTrace); 
	}
}
```



**Esecuzione della logica**
```C#
//controlla se il comando inserito è 'replace'
if(parser.GetCommand(0) == "replace")
{
    //recupera i parametri
    string baseString = parser.GetParameter("base_string");
    string toReplace  = parser.GetParameter("to_replace");
    string newString  = parser.GetParameter("new_string");

    //esecuzione logica del programma
    string result = baseString.Replace(toReplace, newString);
    Console.WriteLine(result);
}
```

\
\-----------------------
## Componenti
La libreria si compone di 4 elementi:
- **ArgsParser**: La classe principale per la definizione dei comandi e l'esecuzione del parsing
- **ArgsCommand**: Classe per definire un nuovo comando
- **ArgsOption**: Classe per definire una nuova opzione
- **ArgsParameterGroup**: Classe per definire un nuovo set di parametri

\
Primo passo è creare un'istanza di **ArgsParser** all'interno del Main.
```C#
ArgsParser parser = new();
```
Si procede definendo quali saranno i comandi.\
È possibile definire anche catene di comandi, in tal caso parametri e opzioni saranno presi per l'ultimo comando inserito nel terminale.\
Il metodo **parser.AddCommand()** accetta sia singole instanze di  **ArgsCommand** che array.
```C#
parser.AddCommand(new[]
{
    new ArgsCommand("replace")
    {
        ValidParameters = {"replaceParam"},
        ValidOptions    = {"help", "all"}
    }
});
```