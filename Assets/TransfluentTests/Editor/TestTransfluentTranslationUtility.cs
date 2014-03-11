using System.Collections.Generic;
using NUnit.Framework;
using Pathfinding.Serialization.JsonFx;
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
	public void testLoadingEnglishMissingKey()
	{
		//by default the utility goes to backwards language
		string thisTextDoesNotExist = "THIS DOES NOT EXIST" + Random.value;
		//string noDestinationLanguageSet = TransfluentUtility.utility.getTranslation();
		var util = new TransfluentUtility("en-us", "en-us");
		
		Assert.AreEqual(util.getTranslation(thisTextDoesNotExist), thisTextDoesNotExist);
	}
	[Test]
	public void testLoadingEnglishKnownKey()
	{
		//by default the utility goes to backwards language
		string textKeyExists = "HELLO_WORLD_TEXT_KEY";

		//string noDestinationLanguageSet = TransfluentUtility.utility.getTranslation();
		var util = new TransfluentUtility("en-us", "en-us");
		Debug.Log("TEXT KEY:" + util.getTranslation(textKeyExists));

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
		var status = service.request(request);
		LanguageList list = request.Parse(status.text);
		Assert.NotNull(list);
		Assert.NotNull(list.languages);
		Assert.Greater(list.languages.Count, 0);

		var so = ResourceCreator.CreateSO<LanguageListSO>("LanguageList");
		so.list = list;
		EditorUtility.SetDirty(so);
		
		LanguageList newList = ResourceLoadFacade.getLanguageList(); //NOTE: THIS IS THE RUNTIME VERSION... not the editor time version

		AssetDatabase.SaveAssets();
		//manual load

		LanguageListSO fromDisk = AssetDatabase.LoadAssetAtPath(languageListPath, typeof (LanguageListSO)) as LanguageListSO;
		Assert.NotNull(fromDisk);
		Assert.NotNull(fromDisk.list);
		Assert.NotNull(fromDisk.list.languages);
		Assert.Greater(fromDisk.list.languages.Count, 0);
		Debug.Log("newlist:"+JsonWriter.Serialize(newList));
	}


	[Test]
	public void testInstance()
	{
		var sourceLanguage = new TransfluentLanguage() {code = "ba-r", id = 502, name = "bar"};
		var destinationLanguage = new TransfluentLanguage() {code = "fo-o", id = 501, name = "foo"};

		TransfluentUtilityInstance instance = new TransfluentUtilityInstance()
		{
			languageList = new LanguageList()
			{
				languages = new List<TransfluentLanguage>()
				{
					sourceLanguage,
					destinationLanguage
				}
			},
			destinationLanguage = destinationLanguage,
			sourceLanguage = sourceLanguage,
			destinationLanguageTranslationDB = new List<TransfluentTranslation>()
				{
					new TransfluentTranslation(){language = sourceLanguage,text="world hello",text_id = "hello world"},
					new TransfluentTranslation(){language = sourceLanguage,text="formatted {0} text",text_id = "formatted text"},
				},
			missingTranslationDB  = new List<TransfluentTranslation>()
		};
		instance.init();

		Assert.Less(instance.missingTranslationDB.Count,1);
		string toTranslateDoesNotExist = "THIS DOES NOT EXIST";
		Assert.AreEqual(instance.getTranslation(toTranslateDoesNotExist), toTranslateDoesNotExist);
		Assert.AreEqual(instance.missingTranslationDB.Count, 1);

		string toTranslateDoesNotExist2 = "THIS DOES NOT EXIST formattted {0}";
		Assert.AreEqual(instance.getFormattedTranslation(toTranslateDoesNotExist2, "nope"), string.Format(toTranslateDoesNotExist2, "nope"));
		Assert.AreEqual(instance.missingTranslationDB.Count, 2);
		bool hasThing = false;
		instance.missingTranslationDB.ForEach((TransfluentTranslation trans)=>
		{
			if (trans.text == toTranslateDoesNotExist2) hasThing = true;
		});
		Assert.IsTrue(hasThing);

		string formattedStringThatExists = "formatted {0} text";
		Assert.AreEqual(instance.getFormattedTranslation(formattedStringThatExists,"success"),"formatted success text");

		string toTranslateAndExists = "hello world";
		string translationResult = "world hello";
		Assert.AreEqual(translationResult,instance.getTranslation(toTranslateAndExists));


	}
}