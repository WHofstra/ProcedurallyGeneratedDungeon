# ProcedurallyGeneratedDungeon
Dit is een project om aan te tonen dat ik een willekeurige 'dungeon' kan creëren.
Doormiddel van een 'Perlin Noise'-gegenereerde texture wordt er een level geplaatst dat verschillende kamers bevat.

## Features
De Unity Engine genereert zelf een Perlin Noise texture. Deze is willekeurig aan de hand van een vooraf gekozen getal.
De tiles worden op het grid geplaatst aan de hand van bepaalde kleurwaarden. De pixels van de texture zijn vergroot. Dit creëert hele kamers.
Deze kamers worden verbonden via de middelpunten.

- [Algoritme voor het plaatsen en verbinden van tiles](https://github.com/WHofstra/ProcedurallyGeneratedDungeon/blob/main/Assets/Scripts/TilePlacement.cs)
- [Dit project in werking](http://29980.hosts2.ma-cloud.nl/bewijzenmap/POR/ProcedurallyGeneratedDungeon/index.html)

## Leerdoelen 
Dit project biedt de mogelijkheid voor mij om onderzoek te doen naar willekeurig gegenereerde levels, Perlin Noise en sorteer algoritmen.

## Bronnen
- [Perlin Noise Wikipedia pagina](https://en.wikipedia.org/wiki/Perlin_noise)
- [Unity referentie naar eigen Perlin Noise generatie](https://docs.unity3d.com/ScriptReference/Mathf.PerlinNoise.html)
- [Drie basis sorteer algoritmen](https://dotnetcoretutorials.com/2020/05/10/basic-sorting-algorithms-in-c/)
