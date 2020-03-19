﻿namespace Vs.VoorzieningenEnRegelingen.Core.TestData.YamlScripts
{
    /// <summary>
    /// Script used for mocking tests
    /// </summary>
    public static class YamlWettelijkeRente
    {
        public readonly static string Body = @"stuurinformatie:
  onderwerp: Wettelijke rente handelstransacties en niet-handelstransacties
  organisatie: De Nederlandsche Bank (DNB)
  type: rente
  domein: Bancaire wezen
  versie: 1.0
  status: ontwikkel
  jaar: 2020
  bron: https://www.rijksoverheid.nl/onderwerpen/schulden/vraag-en-antwoord/hoogte-wettelijke-rente
berekening:
 - stap: 1
   omschrijving: Wat is de hoofdsom?
   formule: f_hoofdsom
 - stap: 2
   omschrijving: Looptijd?
   formule: f_looptijd
 - stap: 3
   omschrijving: Soort Rente?
   formule: f_rente_soort
formules:
 - f_hoofdsom:
     formule: hoofdsom
 - f_looptijd:
     formule: periode(looptijd)
 - f_rente_soort:
     formule: lookup('soort_rente',rente_soort,'omschrijving','soort', 0)
 - f_rente:
     formule: rente(hoofdsom, looptijd)
tabellen:
  - naam: wettelijke_rente
    per, handelstransacties, consumententransacties:
      - [ 1 januari 2020,   8.00, 2.00 ]
      - [ 1 januari 2019,   8.00, 2.00 ]
      - [ 1 juli 2018,      8.00, 2.00 ]
      - [ 1 januari 2018, 	8.00, 2.00 ]
      - [ 1 juli 2017, 	    8.00, 2.00 ]
      - [ 1 januari 2017, 	8.00, 2.00 ]
      - [ 1 juli 2016, 	    8.00, 2.00 ]
      - [ 1 januari 2016, 	8.05, 2.00 ]
      - [ 1 januari 2015, 	8.05, 2.00 ]
      - [ 7 januari 2014,  	8.15, 3.00 ]
      - [ 1 januari 2014,  	8.25, 3.00 ]
      - [ 1 juli 2013, 	    8.50, 3.00 ]
      - [ 16 maart 2013,    8.75, 3.00 ]
      - [ 1 januari 2013, 	7.75, 3.00 ]
      - [ 1 juli 2012, 	    8.00, 3.00 ]
      - [ 1 januari 2012, 	8.00, 4.00 ]
      - [ 1 juli 2011, 	    8.25, 4.00 ]
      - [ 1 januari 2011, 	8.00, 3.00 ]
      - [ 1 januari 2010, 	8.00, 3.00 ]
      - [ 1 juli 2009, 	    8.00, 4.00 ]
      - [ 1 januari 2009, 	9.50, 6.00 ]
  - naam: soort_rente
    omschrijving, soort:
       - [ Wettelijke rente voor handelstransacties, handelstransacties ]
       - [ Wettelijke rente voor consumententransacties, consumententransacties ]";
    }
}
