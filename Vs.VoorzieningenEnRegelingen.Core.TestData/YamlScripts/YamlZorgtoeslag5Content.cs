﻿namespace Vs.VoorzieningenEnRegelingen.Core.TestData.YamlScripts
{
    public class YamlZorgtoeslag5Content
    {
        public readonly static string Body = @"Content:
 - key: berekening.header
   titel: Zorgtoeslag
   ondertitel: Proefberekening
 - key: stap.woonland
   vraag: Waar bent u woonachtig?
   titel: Selecteer uw woonland.
   tekst: Indien u niet zeker weet wat uw woonland is, kijk dan op de website van de Belastingdienst.
   hint: Selecteer ""Anders"" wanneer uw woonland niet in de lijst staat.
   label: Woonland
 - key: stap.woonsituatie
   vraag: Wat is uw woonsituatie?
   titel: Wat is uw woonsituatie?
   tekst: Indien u niet zeker weet wat uw woonsituatie is, kijk dan op de website van de Belastingdienst.
 - key: stap.woonsituatie.keuze.alleenstaande
   hint: Geef aan of u alleenstaande bent of dat u een toeslagpartner heeft.
   tekst: Alleenstaande
 - key: stap.woonsituatie.keuze.aanvrager_met_toeslagpartner
   tekst: Aanvrager met toeslagpartner 
 - key: stap.vermogensdrempel.situatie.alleenstaande
   vraag: Is uw vermogen hoger dan de drempelwaarde?
   titel: Vermogensdrempel
   tekst: Wanneer u als alleenstaande meer vermogen heeft dan €{{toetsingsvermogensdrempel | string.to_double | math.format ""N"" ""nl-NL""}}, overschrijdt u de vermogensdrempel. U heeft dan geen recht op zorgtoeslag.
     <br />Indien u niet zeker weet wat uw vermogen is, kijk dan op de website van de Belastingdienst.
 - key: stap.vermogensdrempel.situatie.alleenstaande.keuze.hoger_dan_vermogensdrempel   
   tekst: Ja, mijn vermogen is **hoger** dan €{{toetsingsvermogensdrempel | string.to_double | math.format ""N"" ""nl-NL""}}
   hint: De huidige vermogensdrempel voor alleenstaanden is €{{toetsingsvermogensdrempel | string.to_double | math.format ""N"" ""nl-NL""}}.
 - key: stap.vermogensdrempel.situatie.alleenstaande.keuze.lager_dan_vermogensdrempel
   tekst: Nee, mijn vermogen is **lager** dan €{{toetsingsvermogensdrempel | string.to_double | math.format ""N"" ""nl-NL""}}
 - key: stap.vermogensdrempel.situatie.aanvrager_met_toeslagpartner
   vraag: Is uw vermogen hoger dan de drempelwaarde?
   titel: Vermogensdrempel
   tekst: Wanneer u samen met een toeslagpartner meer vermogen heeft dan €{{toetsingsvermogensdrempel | string.to_double | math.format ""N"" ""nl-NL""}} overschrijdt u de vermogensdrempel. U heeft dan geen recht op zorgtoeslag. 
     <br />Indien u niet zeker weet wat uw vermogen is, kijk dan op de website van de Belastingdienst.
 - key: stap.vermogensdrempel.situatie.aanvrager_met_toeslagpartner.keuze.hoger_dan_vermogensdrempel   
   tekst: Ja, het gezamenlijk vermogen is **hoger** dan €{{toetsingsvermogensdrempel | string.to_double | math.format ""N"" ""nl-NL""}}
   hint: De huidige vermogensdrempel voor aanvragers met toeslagpartners is €{{toetsingsvermogensdrempel | string.to_double | math.format ""N"" ""nl-NL""}}.
 - key: stap.vermogensdrempel.situatie.aanvrager_met_toeslagpartner.keuze.lager_dan_vermogensdrempel   
   tekst: Nee, het gezamenlijk vermogen is **lager** dan €{{toetsingsvermogensdrempel | string.to_double | math.format ""N"" ""nl-NL""}}
 - key: stap.inkomensdrempel.situatie.alleenstaande
   vraag: Is uw toetsingsinkomen hoger dan de inkomensdrempel?
   titel: Inkomensdrempel
   tekst: Wanneer u als alleenstaande meer inkomen heeft dan €{{toetsingsinkomensdrempel | string.to_double | math.format ""N"" ""nl-NL""}} per jaar, overschrijdt u de inkomensdrempel. U heeft dan geen recht op zorgtoeslag.
     <br />Indien u niet zeker weet wat uw inkomen is, kijk dan op de website van de Belastingdienst.
 - key: stap.inkomensdrempel.situatie.alleenstaande.keuze.hoger_dan_inkomensdrempel
   tekst: Ja, mijn inkomen is **hoger** dan €{{toetsingsinkomensdrempel | string.to_double | math.format ""N"" ""nl-NL""}}
   hint: De huidige inkomensdrempel voor alleenstaanden is €{{toetsingsinkomensdrempel | string.to_double | math.format ""N"" ""nl-NL""}} per jaar.
 - key: stap.inkomensdrempel.situatie.alleenstaande.keuze.lager_dan_inkomensdrempel
   tekst: Nee, mijn inkomen is **lager** dan €{{toetsingsinkomensdrempel | string.to_double | math.format ""N"" ""nl-NL""}}
 - key: stap.inkomensdrempel.situatie.aanvrager_met_toeslagpartner
   vraag: Is uw toetsingsinkomen hoger dan de inkomensdrempel?
   titel: Inkomensdrempel
   tekst: Wanneer u samen met een toeslagpartner meer inkomen heeft dan €{{toetsingsinkomensdrempel | string.to_double | math.format ""N"" ""nl-NL""}} per jaar overschrijdt u de inkomensdrempel. U heeft dan geen recht op zorgtoeslag.
     <br />Indien u niet zeker weet wat uw gezamenlijk inkomen is, kijk dan op de website van de Belastingdienst.
 - key: stap.inkomensdrempel.situatie.aanvrager_met_toeslagpartner.keuze.hoger_dan_inkomensdrempel
   tekst: Ja, het gezamenlijk inkomen is **hoger** dan €{{toetsingsinkomensdrempel | string.to_double | math.format ""N"" ""nl-NL""}}
   hint: De huidige inkomensdrempel voor aanvragers met toeslagpartners is €{{toetsingsinkomensdrempel | string.to_double | math.format ""N"" ""nl-NL""}} per jaar
 - key: stap.inkomensdrempel.situatie.aanvrager_met_toeslagpartner.keuze.lager_dan_inkomensdrempel   
   tekst: Nee, het gezamenlijk inkomen is **lager** dan €{{toetsingsinkomensdrempel | string.to_double | math.format ""N"" ""nl-NL""}}
 - key: stap.toetsingsinkomen.situatie.alleenstaande
   vraag: Wat is uw toetsingsinkomen?
   titel: Toetsingsinkomen
   tekst: Vul uw toetsingsinkomen in. Gebruik een komma als scheidingsteken tussen euro's en centen.
     <br />Indien u niet zeker weet wat uw inkomen is, kijk dan op de website van de Belastingdienst.
 - key: stap.toetsingsinkomen.situatie.alleenstaande.toetsingsinkomen
   hint: De huidige inkomensdrempel voor alleenstaanden is €{{toetsingsinkomensdrempel | string.to_double | math.format ""N"" ""nl-NL""}} per jaar.
 - key: stap.toetsingsinkomen.situatie.aanvrager_met_toeslagpartner
   vraag: Wat is uw toetsingsinkomen?
   titel: Gezamenlijk toetsingsinkomen
   tekst: Vul de som van uw toetsingsinkomen en het toetsingsinkomen van uw toeslagpartner in. Gebruik een komma als scheidingsteken tussen euro's en centen.
     <br />Indien u niet zeker weet wat uw gezamenlijk inkomen is, kijk dan op de website van de Belastingdienst.
 - key: stap.toetsingsinkomen.situatie.aanvrager_met_toeslagpartner.toetsingsinkomen
   hint: Vul een getal in. Gebruik een komma (,) in plaats van een punt (.) als scheidingsteken tussen euro's en centen.
 - key: stap.zorgtoeslag.situatie.alleenstaande
   vraag: Maandelijkse zorgtoeslag
   titel: Uw zorgtoeslag is €{{zorgtoeslag | string.to_double | math.format ""N"" ""nl-NL""}} per maand.
   tekst: De berekening is afgelopen. U kunt eventueel naar een vorige pagina gaan om een antwoord aan te passen.
 - key: stap.zorgtoeslag.situatie.aanvrager_met_toeslagpartner
   vraag: Maandelijkse zorgtoeslag samen met uw toeslagpartner
   titel: Uw gezamenlijke zorgtoeslag is €{{zorgtoeslag | string.to_double | math.format ""N"" ""nl-NL""}} per maand.
   tekst: De berekening is afgelopen. U kunt eventueel naar een vorige pagina gaan om een antwoord aan te passen.
 - key: 
     stap.woonland.geen_recht, 
     stap.vermogensdrempel.situatie.alleenstaande.geen_recht,
     stap.vermogensdrempel.situatie.aanvrager_met_toeslagpartner.geen_recht,
     stap.inkomensdrempel.situatie.alleenstaande.geen_recht,
     stap.inkomensdrempel.situatie.aanvrager_met_toeslagpartner.geen_recht, 
     stap.toetsingsinkomen.situatie.aanvrager_met_toeslagpartner.geen_recht, 
     stap.toetsingsinkomen.situatie.aanvrager_met_toeslagpartner.geen_recht
   vraag: Geen Recht
   titel: U heeft geen recht op zorgtoeslag.
   tekst: Met de door u ingevulde gegevens heeft u geen recht op zorgtoeslag. Voor meer informatie over zorgtoeslag in uw situatie, neem contact op met de Belastingdienst. 
     <br />De berekening is afgelopen. U kunt eventueel naar een vorige pagina gaan om een antwoord aan te passen.
";
    }
}
