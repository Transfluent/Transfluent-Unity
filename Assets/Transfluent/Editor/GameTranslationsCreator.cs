using System;
using System.Collections.Generic;
using System.Security;
using transfluent.editor;
using UnityEditor;
using UnityEngine;

namespace transfluent
{
	public class GameTranslationsCreator
	{
		private static string basePath = "Assets/Transfluent/Resources/";
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

		// I don't know if I am going to expose this, but it is something to do
		//maybe as sub-functionality on the scriptableobject?  push/pull on the object itself?
		[MenuItem("Window/Download All Transfluent data")]
		public static void doDownload()
		{
			TransfluentEditorWindowMediator mediator = new TransfluentEditorWindowMediator();
			var usernamePassword = mediator.getUserNamePassword();
			if (string.IsNullOrEmpty(usernamePassword.Key) || string.IsNullOrEmpty(usernamePassword.Value))
			{
				EditorUtility.DisplayDialog("Login please","Please login using editor window before trying to use this functionality","ok");
				TransfluentEditorWindow.Init();
				return;
			}
			mediator.doAuth(usernamePassword.Key, usernamePassword.Value);
			List<string> allLanguageCodes = mediator.getAllLanguageCodes();
			foreach (string languageCode in allLanguageCodes)
			{
				try
				{
					mediator.setCurrentLanguageFromLanguageCode(languageCode);
					List<TransfluentTranslation> translations = mediator.knownTextEntries();
					if (translations.Count > 0)
					{
						GameTranslationSet set = CreateGameTranslation("AutoDownloaded-" + languageCode);
						set.allTranslations = translations;
						EditorUtility.SetDirty(set); 
						AssetDatabase.SaveAssets();
						//break;
					}
				}
				catch (Exception e)
				{
					Debug.LogError("error while downloading translations:"+e.Message + " stack:"+e.StackTrace);
				}
			}
		}
	}
}

