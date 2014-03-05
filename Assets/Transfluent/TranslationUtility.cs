using UnityEditor;
using UnityEngine;
using System.Collections;

namespace transfluent
{

	public class TranslfuentLanguageListGetter
	{
		private static string basePath = "Assets/Transfluent/Resources";
		private static string fileName = "LanguageList";
		public static LanguageList getLanguageListFromSO()
		{
			string languageListFilePath = basePath + fileName + ".asset";
			LanguageListSO set = AssetDatabase.LoadAssetAtPath(languageListFilePath,typeof(LanguageListSO)) as LanguageListSO;
			if(set != null)
				return set.list;

			set = ScriptableObject.CreateInstance<LanguageListSO>();
			AssetDatabase.CreateAsset(set, languageListFilePath);
			AssetDatabase.SaveAssets();
			if (set.list == null)
			{
				set.list = new LanguageList();
			}
			return set.list;
		}

		public static void saveLanguageList(LanguageList list)
		{
			var oldList = getLanguageListFromSO();
			oldList.languages.Clear();
			oldList.languages.AddRange(list.languages);
		}
	}

	public class TransfluentUtility
	{
		private LanguageList list;
		private bool failedSetup = false;
		private TransfluentUtilityInstance _utility;
		public bool Init(string currentLanguage)
		{
			if (failedSetup) return false; //don't spam the below operations!
			try
			{
				if (list == null)
				{
					list = TranslfuentLanguageListGetter.getLanguageListFromSO();
				}
				_utility = new TransfluentUtilityInstance()
				{
					languageList = list,
					sourceLanguage = list.getLangaugeByCode("en-us"),
					destinationLanguage = list.getLangaugeByCode("xx-xx")
				};
			}
			catch
			{
				failedSetup = true;
				return false;
			}
			return true;
		}
	}

	public class TransfluentUtilityInstance
	{
		public TransfluentLanguage sourceLanguage { get; set; }
		public TransfluentLanguage destinationLanguage { get; set; }
		public LanguageList languageList { get; set; }

	}
}
