# Print

Print mette a disposizione una singola classe statica, con al suo interno diversi metodi a loro volta statici che astraggono le funzioni Console.WriteLine() e Console.Write(), per consentire di scrivere automaticamente messaggi di colori diversi e di disattivare tutti i comandi Print presenti nel programam usando un solo flag.

## Proprietà di Print

**IsVerbose** : bool\
Valore booleano, solo se impostato a 'true' tutti i metodi di Print scriveranno effettivamente su console.

**IsDebug** : bool\
Valore booleano, solo se impostato a 'true' il metodo Print.Debug() scriverà effettivamente su console.

**IsGUI** : bool\
Valore booleano, se fosse necessario sviluppare una'applicazione con interfaccia grafica, è possibile rimandare tutti i metodi di Print ad un qualche componente grafico invece che alla console.\
Per farlo è necessario settare questa proprietà a 'true' e poi valorizzare la proprietà 'GuiWriteFunction' di Print.

**GuiWriteFunction** : Action<Color, string, bool>\
Proprietà di tipo Action, per definire un qualche metodo a cui reinderizzare le chiamate di Print, il metodo impostato dovrà prendere in input 3 valori di tipo:
- Color, il colore del messaggio che sta arrivando
- string, il testo del messaggio che sta arrivando
- bool, se il messaggio ha l'ultimo carattere come nuova linea '\n'


## Metodi di Print

**Error**\
Scrive un messaggio rosso di errore
```C#
	Print.Error(string text)
```

**Warning**\
Scrive un messaggio giallo di attenzione
```C#
	Print.Warning(string text)
```

**Succes**\
Scrive un messaggio verde di successo
```C#
	Print.Succes(string text)
```

**Note**\
Scrive un messaggio grigio di nota
```C#
	Print.Note(string text)
```

**Message**\
Scrive un generico messaggio bianco
```C#
	Print.Message(string text)
```

**Debug**\
Scrive un messaggio blu di debug
```C#
	Print.Debug(string text)
```