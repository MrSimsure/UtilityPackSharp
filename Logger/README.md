# Logger
---------------

Loggermette a disposizione una singola classe statica, con al suo interno diversi metodi a loro volta statici che si occupano di eseguire rapide scritture di file su disco in luoghi predefiniti.

## Enumeratori
**LogLocation**\
Definizioni per i luoghi dove impostare la cartella madre per i log.
```C#
public enum LogLocation
{   
        ROOT,
        CUSTOM,
        EXEPOS,
        PROGDATA,
        APPDATAROAM,
        APPDATALOCA
}
```
- ROOT = C:\
- CUSTOM = cartella libera
- EXEPOS = stessa cartella in cui si trova l'eseguibile
- PROGDATA = C:\ProgramData\
- APPDATAROAM = ..\AppData\Roaming
- APPDATALOCA = ..\AppData\Local



## Proprietà di Logger

**IsLogActive** : bool\
Valore booleano, solo se impostato a 'true' i metodi di Logger salveranno effettivamente i file su disco.

**LogDirSub** : string\
Stringa per definire sottocartelle su cui scrivere i file, la sottocartella è posizionata dentro la cartella principale, definita dal metodo SetLogLocation().



## Metodi di Logger

**SetLogLocation**\
Usata per definire il luogo dove è posizionata la cartella madre di logging, richiede un valore dell'enumeratore LogLocation.\
Il parametro 'customDir' è necessario unicamente se si sceglie il valore 'LogLocation.CUSTOM'.
```C#
	SetLogLocation(LogLocation location, string customDir = "")
```

**LogText**\
Scrive su disco un testo, come argomenti richiede il testo da scrivere e l'eventuale nome aggiuntivo del file.\
Formato del file .txt
```C#
	LogText(string text, string name = "")
```

**LogJson**\
Scrive su disco un oggetto json, come argomenti richiede un oggetto serializzabile come json e l'eventuale nome aggiuntivo del file.\
Formato del file .json
```C#
	LogJson(object obj, string name = "")
```

**LogJson**\
Scrive su disco un oggetto json, come argomenti richiede un testo in formato json e l'eventuale nome aggiuntivo del file.\
Formato del file .json
```C#
	LogJson(string text, string name = "")
```

**LogJsonList**\
Scrive su disco una lista di oggetti json, come argomenti richiede una lista di oggetti serializzabili come json e l'eventuale nome aggiuntivo del file.\
Formato del file .json
```C#
	LogJsonList<T>(List<T> list, string name = "")
```
