using NUnit.Framework;
using Pathfinding.Serialization.JsonFx;
using System.Collections.Generic;
using transfluent;
using transfluent.editor;
using UnityEditor;
using UnityEngine;

[TestFixture]
public class TestTransfluentTranslationUtility
{
	[TestFixtureSetUp]
	public void testCreation()
	{
	}

	[Test]
	public void testInstance()
	{
		var destinationLanguage = new TransfluentLanguage { code = "fo-o", id = 501, name = "foo" };

		var instance = new TransfluentUtilityInstance
		{
			destinationLanguage = destinationLanguage,
			allKnownTranslations = new Dictionary<string, string>
			{
				{"hello world", "world hello"},
				{"formatted {0} text", "formatted text"}
			},
		};

		string toTranslateDoesNotExist = "THIS DOES NOT EXIST";
		Assert.AreEqual(instance.getTranslation(toTranslateDoesNotExist), toTranslateDoesNotExist);

		string toTranslateDoesNotExist2 = "THIS DOES NOT EXIST formattted {0}";
		Assert.AreEqual(instance.getFormattedTranslation(toTranslateDoesNotExist2, "nope"),
			string.Format(toTranslateDoesNotExist2, "nope"));

		string formattedStringThatExists = "formatted {0} text blah blah blah";
		Assert.AreEqual(instance.getFormattedTranslation(formattedStringThatExists, "success"),
						string.Format(formattedStringThatExists, "success"));

		string toTranslateAndExists = "hello world";
		string translationResult = "world hello";
		Assert.AreEqual(translationResult, instance.getTranslation(toTranslateAndExists));
	}

	[Test]
	public void testParamsPassthrough()
	{
		string formattedStringThatExists = "formatted {0} text";
		Assert.AreEqual(stringFormatPassthrough(formattedStringThatExists, "success"),
			string.Format(formattedStringThatExists, "success"));
	}

	public string stringFormatPassthrough(string formatString, params object[] passthrough)
	{
		return string.Format(formatString, passthrough);
	}

	[Test]
	public void testLanguageListGetterWithNoList()
	{
		//LanguageList list = ResourceLoadAdapter.getLanguageList();
		//Assets/Transfluent/Resources/LanguageList.asset
		string languageListPath = "Assets/Transfluent/Resources/LanguageList.asset";
		AssetDatabase.DeleteAsset(languageListPath);
		IWebService service = new SyncronousEditorWebRequest();
		var request = new RequestAllLanguages();
		WebServiceReturnStatus status = service.request(request);
		LanguageList list = request.Parse(status.text);
		Assert.NotNull(list);
		Assert.NotNull(list.languages);
		Assert.Greater(list.languages.Count, 0);

		var so = ResourceCreator.CreateSO<LanguageListSO>("LanguageList");
		so.list = list;
		EditorUtility.SetDirty(so);

		LanguageList newList = ResourceLoadFacade.getLanguageList();
		//NOTE: THIS IS THE RUNTIME VERSION... not the editor time version

		AssetDatabase.SaveAssets();
		//manual load

		var fromDisk = AssetDatabase.LoadAssetAtPath(languageListPath, typeof(LanguageListSO)) as LanguageListSO;
		Assert.NotNull(fromDisk);
		Assert.NotNull(fromDisk.list);
		Assert.NotNull(fromDisk.list.languages);
		Assert.Greater(fromDisk.list.languages.Count, 0);
		Debug.Log("newlist:" + JsonWriter.Serialize(newList));
	}
}