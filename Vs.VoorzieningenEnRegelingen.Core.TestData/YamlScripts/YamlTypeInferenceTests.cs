﻿namespace Vs.VoorzieningenEnRegelingen.Core.TestData.YamlScripts
{
    /// <summary>
    /// Script used for mocking tests
    /// </summary>
    public static class YamlVariableTypeTests
    {
        public readonly static string Body = @"# See if all variable types are correctly inferred Unit Tests
stuurinformatie:
  onderwerp: See if all variable types are correctly inferred Unit Tests
  organisatie: unit test
  type: unit test
  domein: unit test
  versie: 1.0
  status: ontwikkel
  jaar: 2019
  bron: unit test
berekening:
 - stap: 1
   omschrijving: vraag 1
   formule: formule1
 - stap: 2
   omschrijving: vraag 2
   situatie: situatie1
   formule: formule2
 - stap: 3
   omschrijving: vraag 3
   situatie: situatie2
   formule: formule3
formules:
 - formule1:
     formule: formule1_waarde
 - formule2:
     formule: formule1_waarde * 100
 - formule3:
     formule: formule1_waarde * 1000
 - situatie1:
     formule: formule1_waarde < 0
 - situatie2:
    formule: formule1_waarde > 0
";
    }
}
