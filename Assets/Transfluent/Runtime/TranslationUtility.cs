using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Pathfinding.Serialization.JsonFx;
using UnityEditor;
using UnityEngine;

namespace transfluent
{
	public class TransfluentUtility
	{
		public static TransfluentUtility utility = new TransfluentUtility();

		[MenuItem("TEST/BLAH")]
		public static void getLanguageList()
		{
			new TranslfuentLanguageListGetter((LanguageList myList) =>
			{
				Debug.Log("NEW LIST:"+JsonWriter.Serialize(myList));

			});
		}

		private LanguageList _newList;
		private TransfluentUtilityInstance _instance;

		private bool _failedSetup;
		public bool failedSetup { get { return _failedSetup; } }

		public string getTranslation(string sourceText)
		{
			if (_instance == null)
			{
				return sourceText; //TODO: add the missing translations db in this condition
			}
			return _instance.getTranslation(sourceText);
		}


		public TransfluentUtility()
		{
			Init("en-us", "xx-xx");
		}

		private string sourceLang;
		private string destLang;

		public TransfluentUtility(string sourceLanguage, string destinationLanguage)
		{

			Init(sourceLanguage, destinationLanguage);
		}

		void Init(string sourceLanguage, string destinationLanguage)
		{
			sourceLang = sourceLanguage;
			destLang = destinationLanguage;

			try
			{
				new TranslfuentLanguageListGetter(onGotList);
			}
			catch(Exception e)
			{
				Debug.LogError("Failure to get assets:" + e.Message + " stack:" + e.StackTrace);
				_failedSetup = true;
			}

		}

		void onGotList(LanguageList newList)
		{
			if (newList == null)
			{
				_failedSetup = true;
				Debug.LogError("Could not load new language list");
				return;
				//Init(sourceLang, destLang);
			}
			
			_newList = newList;

			TrnaslationSetGetter missing = new TrnaslationSetGetter();
			Debug.Log("DEST LANG:" + destLang);
			Debug.Log("NEW LIST:"+JsonWriter.Serialize(newList));
			var dest = _newList.getLangaugeByCode(destLang);
			var source = _newList.getLangaugeByCode(sourceLang);
			
			_instance = new TransfluentUtilityInstance()
			{
				destinationLanguage = dest,
				sourceLanguage = source,
				languageList = _newList,
				destinationLanguageTranslationDB = getTranslationSet(destLang),
				missingTranslationDB = missing.getMissingSet(source.id,dest.id)
			};
			_instance.destinationLanguageTranslationDB = missing.getMissingSet(source.id,dest.id);
		}

		GameTranslationSet getTranslationSet(string languageCode)
		{
			var destinationLanguageKnownTranslationSet = GameTranslationsCreator.GetTranslaitonSet(languageCode);
			if(destinationLanguageKnownTranslationSet == null)
			{
				destinationLanguageKnownTranslationSet = GameTranslationsCreator.CreateGameTranslation(languageCode);
			}
			return destinationLanguageKnownTranslationSet;
		}
	}

	public class TrnaslationSetGetter
	{
		private const string basePath = "Assets/Transfluent/Resources/";
		private const string fileName = "UnknownTranslations";

		public GameTranslationSet getMissingSet(int sourceLanguageID,int destinationLanguageID)
		{
			string missingSetList = string.Format("{0}{1}-fromid_{2}-toid_{3}", basePath, fileName, sourceLanguageID, destinationLanguageID);
			Debug.Log("Creating GameTranslationSet " + missingSetList);
			GameTranslationSet set = AssetDatabase.LoadAssetAtPath(missingSetList, typeof(GameTranslationSet)) as GameTranslationSet;
			if(set != null)
				return set;


			set = ScriptableObject.CreateInstance<GameTranslationSet>();
			AssetDatabase.CreateAsset(set, missingSetList);
			return set; 
		}

	}

	//NOTE: this is using the source text as the text key itself
	public class TransfluentUtilityInstance
	{
		public TransfluentLanguage sourceLanguage { get; set; }
		public TransfluentLanguage destinationLanguage { get; set; }
		public GameTranslationSet missingTranslationDB { get; set; }
		public GameTranslationSet destinationLanguageTranslationDB { get; set; }
		public LanguageList languageList { get; set; }

		private void addNewMissingTranslation(string sourceText)
		{
			missingTranslationDB.allTranslations.Add(new TransfluentTranslation()
			{
				group_id = sourceLanguage.id.ToString(),
				language = destinationLanguage,
				text = sourceText,
				text_id = sourceText
			});
#if UNITY_EDITOR
			EditorUtility.SetDirty(missingTranslationDB);
#endif
		}

		public bool init()
		{
			foreach (TransfluentTranslation trans in destinationLanguageTranslationDB.allTranslations)
			{
				allKnownTranslations.Add(trans.text_id,trans.text);
			}
			return true;
		}
		
		/*
		public string getGroupId()
		{
			return "UNITY_AUTO_GENERATED";
		}*/
		Dictionary<string,string> allKnownTranslations = new Dictionary<string, string>(); 
		List<string> notTranslatedCache = new List<string>(); 
		public string getTranslation(string sourceText)
		{
			if (allKnownTranslations.ContainsKey(sourceText))
			{
				return allKnownTranslations[sourceText];
			}
			if (!notTranslatedCache.Contains(sourceText))
			{
				addNewMissingTranslation(sourceText);
			}
			return sourceText;
		}

	}
	public class GameTranslationsCreator
	{
		private static
			
			string basePath = "Assets/Transfluent/Resources/";
		[MenuItem("Window/Create new game translations set")]
		public static void DoMenuItem()
		{
			CreateGameTranslation("GameTranslationSet");
		}

		public static GameTranslationSet CreateGameTranslation(string fileName)
		{
			string gameTranslationFileName = basePath + fileName + ".asset";
			string uniqueName = AssetDatabase.GenerateUniqueAssetPath(gameTranslationFileName);
			var set = ScriptableObject.CreateInstance<GameTranslationSet>();
			AssetDatabase.CreateAsset(set, uniqueName);
			AssetDatabase.SaveAssets();

			return set;
		}

		public static string fileNameFromLanguageCode(string languageCode)
		{
			return "AutoDownloaded-" + languageCode + ".asset";
		}

		public static GameTranslationSet GetTranslaitonSet(string langaugeCode)
		{
			string fileName = fileNameFromLanguageCode(langaugeCode);
			string path = basePath + fileName;
			GameTranslationSet set = AssetDatabase.LoadAssetAtPath(path, typeof(GameTranslationSet)) as GameTranslationSet;
			if(set != null)
				return set;
			return CreateGameTranslation(fileName);
		}
	}
}
