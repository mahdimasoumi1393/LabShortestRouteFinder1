# Version 1
I den här versionen har jag lagt till en ny knapp under listan för att addera nya rutter. Rutterna sparas i JSON-filen, men jag hade problem i Visual Studio och kunde inte se några uppdateringar. När jag däremot öppnade filen i File Explorer med en texteditor som VS Code eller Notepad såg jag att rutterna hade lagts till.
Jag har också lagt till en ny kolumn i listan, Cost, och ändrat egenskaperna så att de kan ta emot nya värden (set). En ny metod har lagts till för att spara rutter (SaveRoutesToJson) och en knappfunktion har implementerats.

# Version 2
Jag lade till en ny flik (Tab Item) för att addera nya städer med namn samt longitud och latitud. I metoden SaveCityNodeToJson används metoden TransformToScreenPosition (Converters) för att räkna ut X- och Y-koordinater.

# Version 3
Jag ändrade klassen Route och lade till en nullable Waypoint. Två comboboxar skapades för nya RouteObjects: Fastest och NonFast. Dessa fungerade först inte i grafen, men när jag ändrade dem till ObservableCollection började de fungera.
Fastest Route visas med gröna linjer och NonFast Route visas med röda linjer. Här kan användaren hitta en rutt mellan två punkter.

# Slutversion
Jag finjusterade koden för att hantera flera rutter mellan Start och Destination via olika Waypoints och för att hitta den snabbaste rutten mellan dem.
Fastest Route har bara en rutt som visas med en grön linje, medan NonFast Route kan bestå av flera objekt som visas med röda linjer i grafen. Om användaren söker en rutt som inte finns i JSON-filen visas ett meddelande i en MessageBox som anger att det inte finns någon rutt mellan de valda städerna.

## Kommentarer, Version 1
1. Bra gjort, Jag ser att du sparar dina json filer på kopiorna som finns under ..LabShortestRouteFinder\bin\Debug\net8.0-windows\Resources\

Filerna som finns under solution ..\LabShortestRouteFinder\Resources\ ändras inte. 
Det är Ok, så länge du är medveten om att nästa gång du bygger programmet, kommer orginal filerna att användas och kopieras till \net8.0-windows\Resources\.

2. Du har kvar hitta snabbaste vägen.  
Du kan göra det enkel, genom att lägga två textboxar, "Från" - "Till" och en kanpp för att starta algoritmen som vi har gått igenom tidigare.
/Alejandro

## Kommentarer, Slutversion.
Bra jobbat, du har klarat fint kraven för Godkänt. Jag sätter betyget på Omniway.
Lycka till i fortsättning
/Alejandro
