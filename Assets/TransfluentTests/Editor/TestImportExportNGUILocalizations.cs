using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor.VersionControl;
using UnityEngine;

namespace transfluent.tests
{
	//a vandilay industries joint venture
	[TestFixture]
	public class TestImportExportNGUILocalizations
	{
		[Test]
		public void testExport()
		{
			var exporter = new ImportExportNGUILocalization.NGUICSVExporter(new List<TransfluentLanguage>
			{
				new TransfluentLanguage {code = "en-us", id = 148, name = "English (United States)"}
			}
				);
			Assert.NotNull(exporter.getCSV());
			Debug.Log(exporter.getCSV());
		}

		[Test]
		public void testImport()
		{
			string fakeImportString =
				@"KEYS,English,Français
Language,English,Français
Medina,Funky,Cold
Stop,HammerTime,Fries
escapedTex,""foobar foo bar monkey potatoe, lots of dandruff,"""" if a quiz is quizzical, what's a test"",not escaped at all.  I'm still trying to break out
";
			var importer = new ImportExportNGUILocalization.NGUILocalizationCSVImporter(fakeImportString);
			Dictionary<string, Dictionary<string, string>> map = importer.getMapOfLanguagesToKeyValueTranslations();
			Assert.NotNull(map);
		}

		[Test]
		public void testImportExport()
		{
			string fakeImportString =
				@"KEYS,English,Français
Language,English,Français
Medina,Funky,Cold
Stop,HammerTime,Fries
escapedTex,""foobar foo bar monkey potatoe, lots of dandruff,"" if a quiz is quizzical, what's a test"",not escaped at all.  I'm still trying to break out
";
			//escapedTex,""foobar foo bar monkey potatoe, lots of dandruff,"""" if a quiz is quizzical, what's a test"",not escaped at all.  I'm still trying to break out

			var importer = new ImportExportNGUILocalization.NGUILocalizationCSVImporter(fakeImportString);
			Dictionary<string, Dictionary<string, string>> map = importer.getMapOfLanguagesToKeyValueTranslations();
			Assert.NotNull(map);
			Assert.IsTrue(map.ContainsKey("English"));
			Assert.IsTrue(map.ContainsKey("Français"));

			var exporter = new ImportExportNGUILocalization.NGUICSVExporter(map);
			string resultCSV = exporter.getCSV();
			string cleanInput = fakeImportString.Replace("\r\n", "\n");
			Debug.Log(resultCSV);
			Assert.AreEqual(cleanInput, resultCSV);
		}

		[Test]
		public void testCSVUtilEscapeUnescape()
		{
			var util = new ImportExportNGUILocalization.NGUICSVUtil();
			string simpleTest = "this is simple";
			string result = util.escapeCSVString(simpleTest);
			Assert.AreEqual(simpleTest,result);
			Assert.AreEqual(simpleTest,util.unescapeCSVString(result));

			string hasACommaInIt = "I have a comma, in me";
			result = util.escapeCSVString(hasACommaInIt);  //oxford won't save you now!
			Assert.AreEqual(result, "\"I have a comma, in me\"");
			Assert.AreEqual(hasACommaInIt, util.unescapeCSVString(result));

			string commaAndAQuote = "I have a comma, a \"quote\", and something else";
			result = util.escapeCSVString(commaAndAQuote);
			string mockResult = "\"I have a comma, a \"\"quote\"\", and something else\"";
			Assert.AreEqual(result, mockResult);
			Assert.AreEqual(commaAndAQuote, util.unescapeCSVString(mockResult));
		}

	}
}