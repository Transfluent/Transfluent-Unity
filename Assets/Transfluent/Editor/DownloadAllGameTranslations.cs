using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace transfluent.editor
{
	public class DownloadAllGameTranslations
	{
		// I don't know if I am going to expose this, but it is something to do
		//maybe as sub-functionality on the scriptableobject?  push/pull on the object itself?

		[MenuItem("Translation/Download All Transfluent data")]
		public static void doDownload()
		{
			TransfluentEditorWindowMediator mediator = getAuthenticatedMediator();
			if(mediator == null) return;

			List<string> allLanguageCodes = mediator.getAllLanguageCodes();
			downloadTranslationSetsFromLanguageCodeList(allLanguageCodes);
		}

		private static TransfluentEditorWindowMediator getAuthenticatedMediator()
		{
			var mediator = new TransfluentEditorWindowMediator();
			KeyValuePair<string, string> usernamePassword = mediator.getUserNamePassword();
			if(String.IsNullOrEmpty(usernamePassword.Key) || String.IsNullOrEmpty(usernamePassword.Value))
			{
				EditorUtility.DisplayDialog("Login please",
					"Please login using editor window before trying to use this functionality", "ok");
				//TransfluentEditorWindow.Init();
				return null;
			}
			mediator.doAuth(usernamePassword.Key, usernamePassword.Value);
			return mediator;
		}

		public static void uploadTranslationSet(List<string> languageCodes, string groupid)
		{
			TransfluentEditorWindowMediator mediator = getAuthenticatedMediator();
			if(mediator == null) return;

			foreach(string languageCode in languageCodes)
			{
				try
				{
					GameTranslationSet set = GameTranslationGetter.GetTranslaitonSetFromLanguageCode(languageCode);
					GameTranslationSet.GroupOfTranslations groupData = set.getGroup(groupid);
					TransfluentLanguage lang = ResourceLoadFacade.getLanguageList().getLangaugeByCode(languageCode);
					if(groupData.translations.Count > 0)
						mediator.SaveGroupToServer(groupData, lang);
				}
				catch
				{
				}
			}
		}

		public static void downloadTranslationSetsFromLanguageCodeList(List<string> languageCodes, string groupid = null)
		{
			TransfluentEditorWindowMediator mediator = getAuthenticatedMediator();
			if(mediator == null) return;

			foreach(string languageCode in languageCodes)
			{
				try
				{
					mediator.setCurrentLanguageFromLanguageCode(languageCode);
					TransfluentLanguage currentlanguage = mediator.GetCurrentLanguage();

					List<TransfluentTranslation> translations = mediator.knownTextEntries(groupid);
					//Debug.Log("CURRENT LANGUAGE:" + currentlanguage.code + " translation count:" + translations.Count);
					if(translations.Count > 0)
					{
						GameTranslationSet set = GameTranslationGetter.GetTranslaitonSetFromLanguageCode(languageCode) ??
												ResourceCreator.CreateSO<GameTranslationSet>(GameTranslationGetter.fileNameFromLanguageCode(languageCode));

						set.language = currentlanguage;
						Dictionary<string, Dictionary<string, string>> groupToTranslationMap = groupidToDictionaryMap(translations);
						foreach(var kvp in groupToTranslationMap)
						{
							Dictionary<string, string> dictionaryOfStrings = kvp.Value;
							if(languageCode.Equals("xx-xx")) //backwards string
								dictionaryOfStrings = cleanBackwardsLanguageStringDictionary(dictionaryOfStrings);
							set.mergeInSet(kvp.Key, dictionaryOfStrings);
						}

						EditorUtility.SetDirty(set);
						AssetDatabase.SaveAssets();
					}
				}
				catch(Exception e)
				{
					Debug.LogError("error while downloading translations:" + e.Message + " stack:" + e.StackTrace);
				}
			}
		}

		public static Dictionary<string, string> cleanBackwardsLanguageStringDictionary(Dictionary<string, string> dic)
		{
			var copy = new Dictionary<string, string>(dic);
			foreach(var kvp in dic)
			{
				copy[kvp.Key] = cleanBackwardsLanguageString(kvp.Value);
			}
			return copy;
		}

		//so that formatted text fields still work, as it will change format tokens from {0} to }0{
		public static string cleanBackwardsLanguageString(string inputString)
		{
			string pattern = @"(}(?<formatNumber>\d+){)";
			var rx = new Regex(pattern, RegexOptions.Multiline);
			string result = rx.Replace(inputString, "{$2}");
			return result;
		}

		//[MenuItem("Transfluent/fixJumbed format string")]
		public static void TestRegex()
		{
			string jumbledThing = "hello }0{, how are you?";
			Debug.Log(cleanBackwardsLanguageString(jumbledThing));
		}

		public static Dictionary<string, Dictionary<string, string>> groupidToDictionaryMap(
			List<TransfluentTranslation> translations)
		{
			var map = new Dictionary<string, Dictionary<string, string>>();
			foreach(TransfluentTranslation translation in translations)
			{
				string group = translation.group_id ?? "";
				if(!map.ContainsKey(group))
					map.Add(group, new Dictionary<string, string>());

				Dictionary<string, string> dic = map[group];
				dic.Add(translation.text_id, translation.text);
			}
			return map;
		}
	}
}