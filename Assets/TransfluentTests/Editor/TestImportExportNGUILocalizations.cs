using System.Collections.Generic;
using System.Diagnostics;
using NUnit.Framework;

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
						//new TransfluentLanguage {code = "fo-br", id = 1, name = "foobar"},
						new TransfluentLanguage {code = "en-us", id = 148, name = "English (United States)"}
					}
				);
			Assert.NotNull( exporter.getCSV() );
			UnityEngine.Debug.Log(exporter.getCSV());
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

		}
	}
}