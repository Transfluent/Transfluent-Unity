using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Collections;

namespace transfluent
{

	public class TranslfuentLanguageListGetter
	{
		private static string basePath = "Assets/Transfluent/Resources/";
		private static string fileName = "LanguageList";
		public static LanguageList getLanguageListFromSO()
		{
			string languageListFilePath = basePath + fileName + ".asset";
			LanguageListSO set = AssetDatabase.LoadAssetAtPath(languageListFilePath,typeof(LanguageListSO)) as LanguageListSO;
			if(set != null)
				return set.list;
			Debug.Log("Creating languageListSO");
			set = ScriptableObject.CreateInstance<LanguageListSO>();
			AssetDatabase.CreateAsset(set, languageListFilePath);
			if (set.list == null)
			{
				set.list = new LanguageList();
			}
			EditorUtility.SetDirty(set);
			AssetDatabase.SaveAssets();
			
			return set.list;
		}

		public static void saveLanguageList(LanguageList list)
		{
			var oldList = getLanguageListFromSO();
			oldList.languages.Clear();
			oldList.languages.AddRange(list.languages);
		}
	}

	public class MissingTranslationSetGetter
	{
		private static string basePath = "Assets/Transfluent/Resources/";
		private static string fileName = "UnknownTranslations";

		public static GameTranslationSet getMissingSet(int sourceLanguageID,int destinationLanguageID)
		{
			string missingSetList = string.Format("{0}{1}-fromid_{2}-toid_{3}", basePath, fileName, sourceLanguageID, destinationLanguageID);
			Debug.Log("Creating GameTranslationSet " + missingSetList);
			GameTranslationSet set = AssetDatabase.LoadAssetAtPath(missingSetList, typeof(GameTranslationSet)) as GameTranslationSet;
			if(set != null)
				return set;

			Debug.Log("Creating GameTranslationSet "+missingSetList);

			set = ScriptableObject.CreateInstance<GameTranslationSet>();
			AssetDatabase.CreateAsset(set, missingSetList);
			if(set.allTranslations == null)
			{
				set.allTranslations = new List<TransfluentTranslation>();
			}
			Debug.Log("Creating GameTranslationSet " + missingSetList);
			return set; 
		}

	}

	public class TransfluentUtility
	{
		public static TransfluentUtility utility = new TransfluentUtility();

		public static string getTranslation(string sourceText)
		{
			return utility.getTranslationInstance(sourceText);
		}
		private LanguageList list;
		private bool _failedSetup = false;
		public bool failedSetup { get { return _failedSetup; } }
		private TransfluentUtilityInstance _utility;

		public bool Init()
		{
			return Init("en-us", "xx-xx");
		}

		public bool Init(string sourceLanguage,string destinationLanguage)
		{
			if(_failedSetup) return false; //don't spam the below operations!
			try
			{
				if(list == null)
				{
					list = TranslfuentLanguageListGetter.getLanguageListFromSO();
				}
				_utility = new TransfluentUtilityInstance()
				{
					languageList = list,
					sourceLanguage = list.getLangaugeByCode(sourceLanguage),
					destinationLanguage = list.getLangaugeByCode(destinationLanguage)
				};
			}
			catch(Exception e)
			{
				Debug.LogError("Failure to get assets:" + e.Message + " stack:" + e.StackTrace);
				_failedSetup = true;
				return false;
			}

			return true;
		}

		public string getTranslationInstance(string originalLanguageText)
		{
			if (!Init()) return originalLanguageText;
			return _utility.getTranslation(originalLanguageText);
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
}
