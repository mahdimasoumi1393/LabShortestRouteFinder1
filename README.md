# Version1
I den här versionen har jag lagt en ny knapp under listan för att addera nya rutts. Rtterna sparas på json filen men jag hade problem i min Visual Studio och kunde se någon uppdatering, men När jag öpnade filen i file eXplorer med en text VS Code och NotePad, såg jag att rutterna har lagts till. 
Jag har lagt till en ny column i listan cost och ändrade properties så att de kan ta emot nya värden(set). Sedan en ny method har lagts till för att spara rutter(SaveRoutesToJson) och knapp funktion. 

## Kommentarer, Version 1
1. Bra gjort, Jag ser att du sparar dina json filer på kopiorna som finns under ..LabShortestRouteFinder\bin\Debug\net8.0-windows\Resources\

Filerna som finns under solution ..\LabShortestRouteFinder\Resources\ ändras inte. 
Det är Ok, så länge du är medveten om att nästa gång du bygger programmet, kommer orginal filerna att användas och kopieras till \net8.0-windows\Resources\.

2. Du har kvar hitta snabbaste vägen.  
Du kan göra det enkel, genom att lägga två textboxar, "Från" - "Till" och en kanpp för att starta algoritmen som vi har gått igenom tidigare.
/Alejandro
   
