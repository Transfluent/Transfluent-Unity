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
		util.getTranslation(thisTextDoesNotExist);
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
}