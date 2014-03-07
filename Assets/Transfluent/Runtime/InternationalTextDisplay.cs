using System.Collections.Generic;
using System.Text;
using transfluent;
using UnityEngine;

public class InternationalTextDisplay : MonoBehaviour
{
	private readonly Dictionary<TransfluentLanguage, GameTranslationSet> languageToGameTransationSetDictionary =
		new Dictionary<TransfluentLanguage, GameTranslationSet>();

	private LanguageList _list;
	// Use this for initialization
	private void Start()
	{
		var getter = new TranslfuentLanguageListGetter((LanguageList list) =>
		{
			_list = list;
			setLanguage();
		});
	}

	private void setLanguage()
	{
		foreach (TransfluentLanguage lang in _list.languages)
		{
			languageToGameTransationSetDictionary.Add(lang, translationSetFromLanguage(lang));
		}
	}

	public GameTranslationSet translationSetFromLanguage(TransfluentLanguage language)
	{
		return GameTranslationsCreator.GetTranslaitonSet(language.code);
	}

	private GameTranslationSet currentTranslationSet;
	private Vector2 scrollPosition;
	private void OnGUI()
	{
		if (_list == null)
		{
			GUILayout.Label("Loading" + new string('.', Time.frameCount%3));
		}
		else
		{
			GUILayout.BeginVertical();
			//GUI.BeginScrollView(new Rect(10, 300, 100, 100), Vector2.zero, new Rect(0, 0, 220, 200));
			scrollPosition = GUILayout.BeginScrollView(scrollPosition);
			int guiHeight = 40;
			int currenty = 0;

			foreach (TransfluentLanguage language in _list.languages)
			{
				if (GUILayout.Button(language.name))
				{
					currentTranslationSet = translationSetFromLanguage(language);
				}
				//GUI.Button(new Rect(0, currenty, 100, guiHeight), language.name);
				currenty += guiHeight;
			}
			GUILayout.EndScrollView();

			GUILayout.EndVertical();

			GUILayout.BeginVertical();
			if(currentTranslationSet != null)
			{
				foreach(TransfluentTranslation translation in currentTranslationSet.allTranslations)
				{
					GUILayout.Label(string.Format("text id:{0} group id:{1} text:{2}",translation.text_id,translation.group_id,translation.text));
				}
			}
			GUILayout.EndVertical();
		}
	}
}