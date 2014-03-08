using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace transfluent.editor
{
	public class DownloadAllGameTranslations
	{
		// I don't know if I am going to expose this, but it is something to do
		//maybe as sub-functionality on the scriptableobject?  push/pull on the object itself?

		[MenuItem("Window/Download All Transfluent data")]
		public static void doDownload()
		{
			TransfluentEditorWindowMediator mediator = new TransfluentEditorWindowMediator();
			var usernamePassword = mediator.getUserNamePassword();
			if(String.IsNullOrEmpty(usernamePassword.Key) || String.IsNullOrEmpty(usernamePassword.Value))
			{
				EditorUtility.DisplayDialog("Login please", "Please login using editor window before trying to use this functionality", "ok");
				TransfluentEditorWindow.Init();
				return;
			}
			mediator.doAuth(usernamePassword.Key, usernamePassword.Value);
			List<string> allLanguageCodes = mediator.getAllLanguageCodes();
			foreach(string languageCode in allLanguageCodes)
			{
				try
				{
					mediator.setCurrentLanguageFromLanguageCode(languageCode);
					List<TransfluentTranslation> translations = mediator.knownTextEntries();
					if(translations.Count > 0)
					{
						GameTranslationSet set = GameTranslationGetter.GetTranslaitonSetFromLanguageCode(languageCode);
						if (set == null)
						{
							set = ResourceCreator.CreateSO<GameTranslationSet>(GameTranslationGetter.fileNameFromLanguageCode(languageCode));
						}

						//GameTranslationSet set = GameTranslationsCreator.GetTranslaitonSet(languageCode);
						set.allTranslations = translations;
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
	}
}

