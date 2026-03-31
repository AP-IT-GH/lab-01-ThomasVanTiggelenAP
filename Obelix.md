Training van een ML-Agent in Unity Rapport – Obelix Menhir Transport Experiment

Inleiding

Dit rapport beschrijft een experiment waarbij een ML-Agent werd getraind in Unity met behulp van de ML-Agents toolkit.
Het doel van het experiment was het ontwikkelen van een agent die zelfstandig menhirs oppakt en naar een aangewezen bestemming transporteert op basis van een beloningssysteem. 
Dit type experiment sluit aan bij het bredere onderzoeksdomein van reinforcement learning, waarbij een agent via trial-and-error een optimale strategie ontwikkelt.

Methoden

Het experiment werd uitgevoerd in Unity. De omgeving bestond uit een vlak speelveld waarop een agent (Obelix), meerdere menhirs en bijhorende bestemmingen geplaatst werden.
De menhirs en bestemmingen werden bij het begin van elke episode willekeurig geplaatst op de rand van een cirkel binnen het speelveld, zodat overlapping werd vermeden.

Het beloningssysteem bestond uit de volgende componenten:

- Een beloning van +0.5 wanneer een menhir succesvol werd opgepakt
- Een beloning van +1.0 wanneer een menhir werd afgeleverd op de bestemming
- Een eindbeloning van +2.0 wanneer alle menhirs werden afgeleverd
- Een tijdstraf van -0.0001 per stap om efficiënt gedrag te stimuleren

De training werd uitgevoerd over 1.000.000 stappen. De resultaten werden gevisualiseerd via TensorBoard.

Grafieken



Resultaten

De cumulatieve beloning steeg tijdens de training tot een piek van ongeveer +6 à +7 rond 800.000 stappen. 
Dit toont aan dat de agent wel degelijk geleerd heeft om menhirs op te pakken en af te leveren. Echter, na deze piek daalde de reward opnieuw, wat betekent dat de agent niet stabiel bleef in zijn gedrag.
De episode-lengte steeg gedurende de training tot meer dan 20.000 stappen per episode. 
Dit is een probleem: het betekent dat de agent te lang bleef rondlopen per episode zonder alle menhirs af te leveren.
Waarschijnlijk lukte het hem om de eerste menhir te verwerken maar raakte hij daarna de kluts kwijt bij de volgende.
De policy loss bleef schommelen zonder duidelijk te dalen, wat aangeeft dat de agent zijn strategie nooit goed stabiliseerde.

Conclusie

De resultaten tonen aan dat de agent de basisprincipes van de taak heeft aangeleerd: hij beweegt richting menhirs, pakt ze op en brengt ze naar een bestemming.
De piek in de reward rond 800.000 stappen bewijst dat dit werkte. 
Echter, door de complexiteit van de taak met vier menhirs en het ontbreken van een staplimiet per episode bleef het gedrag
instabiel en eindigde de training zonder een volledig betrouwbaar resultaat.


