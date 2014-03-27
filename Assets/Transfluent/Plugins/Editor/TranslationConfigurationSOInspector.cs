using System.Collections.Generic;
using transfluent;
using transfluent.editor;
using UnityEditor;
using UnityEngine;

//add on extra funcitonality to manually manipulate sets of translations
//initially just a button to upload and download
[CustomEditor(typeof(TranslationConfigurationSO))]
public class TranslationConfigurationSOInspector : Editor
{
	public override void OnInspectorGUI()
	{
		TranslationConfigurationSO so = (TranslationConfigurationSO)target;
		DrawDefaultInspector();
		if(GUILayout.Button("Upload current set")) //TODO: order translations
		{
			var languageCodeList = new List<string> { so.sourceLanguage.code };
			so.destinationLanguages.ForEach((TransfluentLanguage lang) => { languageCodeList.Add(lang.code); });

			DownloadAllGameTranslations.uploadTranslationSet(languageCodeList, so.translation_set_group);
		}
		if(GUILayout.Button("Download known translations"))
		{
			if(EditorUtility.DisplayDialog("Downloading", "Downloading will overwrite any local changes to existing keys do you want to proceed?", "OK", "Cancel / Let me upload first"))
			{
				var languageCodeList = new List<string> { so.sourceLanguage.code };
				so.destinationLanguages.ForEach((TransfluentLanguage lang) => { languageCodeList.Add(lang.code); });
				DownloadAllGameTranslations.downloadTranslationSetsFromLanguageCodeList(languageCodeList, so.translation_set_group);
			}
		}
	}
}