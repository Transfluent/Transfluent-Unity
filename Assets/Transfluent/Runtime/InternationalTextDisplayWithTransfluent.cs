#define transfluent

using System.Collections.Generic;
using transfluent;
using UnityEngine;

#if transfluent

#else
using GUI = UnityEngine.GUI;
using GUILayout = UnityEngine.GUILayout;
#endif

public class InternationalTextDisplayWithTransfluent : MonoBehaviour
{
	private readonly List<TransfluentLanguage> supportedLanguages = new List<TransfluentLanguage>();

	private TranslationConfigurationSO config;
	private Vector2 scrollPosition;

	[SerializeField]
	private string testText;

	private ITranslationUtilityInstance translationHelper;

	// Use this for initialization
	private void Start()
	{
		config = ResourceLoadFacade.LoadConfigGroup("");
		populateKnownTranslationsInGroup();
		TranslationUtility.changeStaticInstanceConfig("xx-xx");
	}

	private void populateKnownTranslationsInGroup()
	{
		supportedLanguages.Add(config.sourceLanguage);

		foreach(TransfluentLanguage lang in config.destinationLanguages)
		{
			supportedLanguages.Add(lang);
		}
	}

	private void OnGUI()
	{
		GUILayout.Label("Test manual text:" + testText);

		GUILayout.BeginVertical();
		scrollPosition = GUILayout.BeginScrollView(scrollPosition);
		int guiHeight = 40;
		int currenty = 0;

		foreach(TransfluentLanguage language in supportedLanguages)
		{
			//TODO: show groups available
			if(GUILayout.Button(language.name))
			{
				TranslationUtility.changeStaticInstanceConfig(language.code);
				translationHelper = TranslationUtility.getUtilityInstanceForDebugging();

				foreach(var trans in translationHelper.allKnownTranslations)
				{
					Debug.Log(string.Format("key:{0} value:{1}", trans.Key, trans.Value));
				}
			}
			currenty += guiHeight;
		}
		GUILayout.EndScrollView();

		GUILayout.EndVertical();

		GUILayout.BeginVertical();
		if(translationHelper != null)
		{
			foreach(var translation in translationHelper.allKnownTranslations)
			{
				GUILayout.Label(string.Format("text id:{0} group id:{1} text:{2}", translation.Key,
					((TranslationUtilityInstance)translationHelper).groupBeingShown, translation.Value));
			}
		}
		GUILayout.EndVertical();
	}
}