Traning van ML-Agent in unity
Rapport | VR Experience

Inleiding

Dit rapport beschrijft een experiment waarbij een ML-Agent werd getraind in unity. Het doel van het experiment was het ontwikkelen van een agent die zelfstandig taken kan uitvoeren op basis van een beloningsysteem.

In een eerste fase bestond de taak uit het verzamelen van een doelobject (target). In een tweede fase werd het experiment uitgebreid: nadat het doelobject werd bereikt, moest de agent eerst een specifieke zone betreden voordat de beloing werd toegekend. Dit onderzoek onderzoekt hoe de agent dit gedragt aanleert en welke patronen zichtbaar worden tijdens het trainingsproces.

Het rapport beschrijft de resultaten tijdens de training en een interpretatie van het gedrag van de agent.

Methoden

Het experiment werd uitgevoerd in unity. De omgeving bestond uit een valk speelveld waarop verschillende objecten geplaatst waren.

De agent kon zich ocer het platform bewegen om het target te bereiken. Wanneer de agent het target aanraakte, verscheen een nieuw target op een andere locatie. In de tweede fase van het experiment werd een extra stap toegevoegd: de agent moest na het verzamelen van het target eerst naar een groene zone gaan om de beloning effectief te verkrijgen hij kreeg wel al een tussen beloning om de target te hebben gevonden.

Het beloningsysteem bestond uit meerdere componenten:

○ een positieve beloning wanneer het target werzameld weg(tussenbeloning)
○ een positieve beloning wanneer het target naar de groene zone gebracht werd(grote beloning)
○ geen beloning als de agent van het platform viel dan eindigd de episode

De training werd gedurende 100000 stappen uitgevoerd. De resultaten van het trainingsproces werden geanalyseerd via TensorBoard, waarin onder andere de cumulatieve beloning, episode-lengte en loss-waarden werden weergegeven.

Grafieken

<img width="1263" height="792" alt="TensorboardResult" src="https://github.com/user-attachments/assets/2dcecfc3-66f4-407c-b010-655b95cc8001" />

Resultaten

Tijdens het trainingsproces werd een stijging waargenomen in de comulatieve beloning van de agent. In de grafieken uit TensorBoard is zichtbaar dat deze waarde geleidelijk toeneemt en uiteindelijk een plateau bereikt rond een waarde van ongeveer 1.4 tot 1.5. Dit wijst erop dat de agent een strategie heeft ontwikkeld waarmee regelmatig beloningen worden verkregen.

Daarnaast werd een verandering in de episode-lengte waargenomen. In de beginfase van de training waren de episodes relatief lang, waarna een daling zichtbaar werd. Dit kan erop wijzen dat de agent efficiënter werd in het uitvoeren van de taak.

De value loss vertoonde tijdens de training een dalende trend, wat suggereert dat het model beter werd in het voorspellen van de verwachte beloning.

Bij observatie van het gedrag in de simulatie werd vastgesteld dat de agent in staat was om het rode target te bereiken en daarna naar de groene zone te navigeren om de beloning te ontvangen. Na het ontvangen van de beloning verscheen een nieuw target en herhaalde de agent deze cyclus.

Een opvallend gedrag dat werd waargenomen tijdens de simulatie was de manier waarop de agent zich over het platform verplaatste. In plaats van rechte bewegingen te maken, volgde de agent vaak gebogen of cirkelvormige trajecten.

Conclusie

De resulaten suggereren dat de agent succesvol heeft geleerd om de vereiste taak uit te voeren: het verzamelen van een target en vervolgens het bereiken van de groene zone om een beloning te verkrijgen. 

Het waargenomen bewegingspatroon, waarbij de agent zich in bochten of cirkels verplaats, lijkt mogelijk te komen doordat als hij af het platform valt hij niets krijgt en dus probeert voorzichtiger te zijn met bewegen en dus al op voorhand draait. Het lijkt erop dat de agent extra aandacht besteed aan zijn positie op het platform.

Het lijkt er dus op dat het beloningssysteem niet alleen het hoofddoelgedrag beïnvloed, maar ook subtiele aspecten van de beweging van de agent. DIt illustreert hoe reinforcement learning systemen strategiën kunnen ontwikkelen die niet altijd volledig overeenkomen met de intuïtieve verwachtingen van de ontwerper.

