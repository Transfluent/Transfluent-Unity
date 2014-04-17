using System.Collections.Generic;
using transfluent;
using UnityEngine;
using GUILayout = transfluent.guiwrapper.GUILayout;

public class SimpleTranslatedTextDisplay : MonoBehaviour
{
	[SerializeField]
	private TextMesh textMesh;

	private string textFormat = "going to play soon {0}";

	private List<string> languagesToShow = new List<string>()
	{
		"en-us",
		"de-de",
		"fr-fr",
		"xx-xx"
	};

	private void Start()
	{
		TranslationUtility.changeStaticInstanceConfig("en-us");
	}

	private void OnGUI()
	{
		int secondToDisplay = Mathf.FloorToInt(Time.timeSinceLevelLoad) % 4 + 1;
		string secondToken = TranslationUtility.get(secondToDisplay.ToString());
		string textToDisplay = TranslationUtility.getFormatted(textFormat, secondToken);
		GUILayout.Label(textToDisplay);
		textMesh.text = textToDisplay;

		foreach(string languageCode in languagesToShow)
		{
			if(GUILayout.Button("Translate to language:" + languageCode))
			{
				TranslationUtility.changeStaticInstanceConfig(languageCode);
			}
		}
	}
}