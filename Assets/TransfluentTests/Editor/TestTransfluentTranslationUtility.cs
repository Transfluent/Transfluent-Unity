using NUnit.Framework;
using Pathfinding.Serialization.JsonFx;
using transfluent;
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
		Debug.Log("PATH:" + TranslfuentLanguageListGetter.LanguageListPath());
		AssetDatabase.DeleteAsset(TranslfuentLanguageListGetter.LanguageListPath().Replace(".asset",""));
		//AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
		
		new TranslfuentLanguageListGetter((LanguageList newList) =>
		{	
			Assert.NotNull(newList);
			Assert.NotNull(newList.languages);
			Assert.Greater(newList.languages.Count,0);
			
			AssetDatabase.SaveAssets();
			//manual load
			var fromDisk = AssetDatabase.LoadAssetAtPath(TranslfuentLanguageListGetter.LanguageListPath(), typeof (LanguageListSO)) as LanguageListSO;
			Assert.NotNull(fromDisk);
			Assert.NotNull(fromDisk.list);
			Assert.NotNull(fromDisk.list.languages);
			Assert.Greater(fromDisk.list.languages.Count, 0);
			Debug.Log("newlist:"+JsonWriter.Serialize(newList));
		});
	}
}