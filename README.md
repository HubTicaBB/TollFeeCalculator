# TollFeeCalculator
## Andreea Nenciu och Tijana Ilić

### Laboration 2: Refaktorera tullavgifter 

#### Introduktion och syfte 
Målet är att öva på refaktorering, använda enhetstestning för att underlätta processen, samt 
att kontinuerligt lägga till enhetstester för att försäkra oss om att inte förstöra ny funktionalitet 
och inte senare återskapa buggar som rättats. 

Grundkoden hittas i ​https://github.com/davidsundelius/cleancodeiths 

#### Uppgift 
Programmets mål är att beräkna kostnaden för en given inputfil innehållande datum och tider 
som en bil åker genom vägtullarna under en dag. Programmet ska utifrån detta skriva ut i 
terminalen hur mycket den totala kostnaden är. Varje passage genom en betalstation kostar 
0, 8, 13 eller 18 kronor beroende på tidpunkt. Det maximala beloppet per dag är 60 kronor. 
 
**Tider**   | **Belopp**
------------|------------------
06:00–06:29 | 8 kr 
06:30–06:59 | 13 kr 
07:00–07:59 | 18 kr 
08:00–08:29 | 13 kr 
08:30–14:59 | 8 kr 
15:00–15:29 | 13 kr 
15:30–16:59 | 18 kr 
17:00–17:59 | 13 kr 
18:00–18:29 | 8 kr 
18:30–05:59 | 0 kr 

Vägtull tas ut för fordon som passerar en betalstation måndag till fredag mellan 06.00 och 
18.29. Tull tas inte ut lördagar och söndagar eller under juli månad. En bil som passerar flera 
betalstationer inom 60 minuter beskattas bara en gång. Det belopp som då ska betalas är 
det högsta beloppet av de passagerna. 
 
Uppgiften går ut på att hitta minst 10 stycken buggar i systemet för vägtullar. För var och en 
av buggarna ska ett unit-test implementerat på så sätt att om någon ändrar tillbaka 
koden så ska enhetstestet ge utslag och ge en korrekt beskrivning av vad som inte gått rätt 
till. Utöver detta ska principerna för Clean Code appliceras och koden ska refaktoreras på ett 
motiverat sätt för att skapa så läsbar kod som möjligt. 

#### Redovisning 
Källkoden ska vara pushad till ett eget publikt repository på GitHub. Uppgiften ska 
genomföras parvis och fullständiga namn ska finnas med i README.md i repots 
main-branch (master). 
