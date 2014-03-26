﻿using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace transfluent.editor
{
	public class DownloadAllGameTranslations
	{
		// I don't know if I am going to expose this, but it is something to do
		//maybe as sub-functionality on the scriptableobject?  push/pull on the object itself?

		[MenuItem("Transfluent/Download All Transfluent data")]
		public static void doDownload()
		{
			TransfluentEditorWindowMediator mediator = getAuthenticatedMediator();
			if (mediator == null) return;

			List<string> allLanguageCodes = mediator.getAllLanguageCodes();
			downloadTranslationSetsFromLanguageCodeList(allLanguageCodes);
		}

		private static TransfluentEditorWindowMediator getAuthenticatedMediator()
		{
			var mediator = new TransfluentEditorWindowMediator();
			KeyValuePair<string, string> usernamePassword = mediator.getUserNamePassword();
			if (String.IsNullOrEmpty(usernamePassword.Key) || String.IsNullOrEmpty(usernamePassword.Value))
			{
				EditorUtility.DisplayDialog("Login please",
					"Please login using editor window before trying to use this functionality", "ok");
				TransfluentEditorWindow.Init();
				return null;
			}
			mediator.doAuth(usernamePassword.Key, usernamePassword.Value);
			return mediator;
		}

		public static void downloadTranslationSetsFromLanguageCodeList(List<string> languageCodes, string groupid = null)
		{
			TransfluentEditorWindowMediator mediator = getAuthenticatedMediator();
			if (mediator == null) return;

			foreach (string languageCode in languageCodes)
			{
				try
				{
					mediator.setCurrentLanguageFromLanguageCode(languageCode);
					TransfluentLanguage currentlanguage = mediator.GetCurrentLanguage();
					List<TransfluentTranslation> translations = mediator.knownTextEntries(groupid);
					if (translations.Count > 0)
					{
						GameTranslationSet set = GameTranslationGetter.GetTranslaitonSetFromLanguageCode(languageCode) ??
						                         ResourceCreator.CreateSO<GameTranslationSet>(
							                         GameTranslationGetter.fileNameFromLanguageCode(languageCode));
						if (set.langaugeThatTranslationsAreIn == null)
						{
							set.langaugeThatTranslationsAreIn = currentlanguage;
						}
						set.mergeInNewListOfTranslations(translations);

						EditorUtility.SetDirty(set);
						AssetDatabase.SaveAssets();
					}
				}
				catch (Exception e)
				{
					Debug.LogError("error while downloading translations:" + e.Message + " stack:" + e.StackTrace);
				}
			}
		}
	}
}