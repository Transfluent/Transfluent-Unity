using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace transfluent.tests
{
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
";
			var importer = new ImportExportNGUILocalization.NGUILocalizationCSVImporter(fakeImportString);
			Dictionary<string, Dictionary<string, string>> map = importer.getMapOfLanguagesToKeyValueTranslations();
			Assert.NotNull(map);
			Assert.IsTrue(map.ContainsKey("English"));
			Assert.IsTrue(map.ContainsKey("Français"));

			var exporter = new ImportExportNGUILocalization.NGUICSVExporter(map);
			string resultCSV = exporter.getCSV();
			Assert.AreEqual(fakeImportString.Replace("\r\n", "\n"), resultCSV);
		}
	}
}