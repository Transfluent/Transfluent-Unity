using System;
using System.Collections.Generic;
using System.Text;
using transfluent;
using UnityEngine;

public class InternationalTextDisplay : MonoBehaviour
{
	private List<TransfluentLanguage> supportedLanguages = new List<TransfluentLanguage>(); 

	private TranslationConfigurationSO config;
	[SerializeField]
	private string testText;

	// Use this for initialization
	private void Start()
	{
		config = ResourceLoadFacade.LoadConfigGroup("");
		populateKnownTranslationsInGroup();
		TransfluentUtility.changeStaticInstanceConfig("xx-xx");
	}

	private void populateKnownTranslationsInGroup()
	{
		
		supportedLanguages.Add(config.sourceLanguage);
		
		foreach (TransfluentLanguage lang in config.destinationLanguages)
		{
			supportedLanguages.Add(lang);
		}
	}

	public GameTranslationSet translationSetFromLanguage(TransfluentLanguage language)
	{
		return GameTranslationGetter.GetTranslaitonSetFromLanguageCode(language.code);
	}

	private List<TransfluentTranslation> currentTranslationSet;
	private Vector2 scrollPosition;
	private void OnGUI()
	{
		GUILayout.Label("Test manual text:" + testText);
		
			GUILayout.BeginVertical();
			//GUI.BeginScrollView(new Rect(10, 300, 100, 100), Vector2.zero, new Rect(0, 0, 220, 200));
			scrollPosition = GUILayout.BeginScrollView(scrollPosition);
			int guiHeight = 40;
			int currenty = 0;

			foreach(TransfluentLanguage language in supportedLanguages)
			{
				if (GUILayout.Button(language.name))
				{
					TransfluentUtility.changeStaticInstanceConfig(language.code);
					TransfluentUtilityInstance utilityInstance = TransfluentUtility.getUtilityInstanceForDebugging();

					foreach (KeyValuePair<string,string> trans in utilityInstance.allKnownTranslations)
					{
						Debug.Log(string.Format("key:{0} value:{1}",trans.Key,trans.Value));
					}
				}
				//GUI.Button(new Rect(0, currenty, 100, guiHeight), language.name);
				currenty += guiHeight;
			}
			GUILayout.EndScrollView();

			GUILayout.EndVertical();

			GUILayout.BeginVertical();
			if(currentTranslationSet != null)
			{
				foreach(TransfluentTranslation translation in currentTranslationSet)
				{
					GUILayout.Label(string.Format("text id:{0} group id:{1} text:{2}",translation.text_id,translation.group_id,translation.text));
				}
			}
			GUILayout.EndVertical();
		}
}