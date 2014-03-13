using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace transfluent
{
	public class GameTranslationGetter
	{
		public static string fileNameFromLanguageCode(string languageCode)
		{
			return "AutoDownloaded-" + languageCode + ".asset";
		}

		public static GameTranslationSet GetTranslaitonSetFromPath(string path)
		{
			return Resources.Load(path) as GameTranslationSet;
		}

		public static string GetMissingTranslationSetFilename(int sourceLanguageID, int destinationLanguageID)
		{
			const string fileName = "UnknownTranslations";
			string missingSetList = string.Format("{0}-fromid_{1}-toid_{2}.asset", fileName, sourceLanguageID,
				destinationLanguageID);
			return missingSetList;
		}

		public static List<GameTranslationSet> GetTranslationSet(List<string> languageCodes)
		{
			var list = new List<GameTranslationSet>();
			foreach (string code in languageCodes)
			{
				list.Add(GetTranslaitonSetFromLanguageCode(code));
			}
			return list;
		}

		public static GameTranslationSet GetTranslaitonSetFromLanguageCode(string langaugeCode)
		{
			string fileName = fileNameFromLanguageCode(langaugeCode).Replace(".asset","");
			var loaded =  ResourceLoadFacade.LoadResource<GameTranslationSet>(fileName);
			return loaded;
		}

		public static GameTranslationSet GetMissingTranslationSet(int sourceLanguageID, int destinationLanguageID)
		{
			string filename = GetMissingTranslationSetFilename(sourceLanguageID, destinationLanguageID);
			return ResourceLoadFacade.LoadResource<GameTranslationSet>(filename);
		}
	}

}