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
		TransfluentUtility.changeStaticInstanceConfig("en-us");
	}

	private void OnGUI()
	{
		int secondToDisplay = Mathf.FloorToInt(Time.timeSinceLevelLoad) % 4 + 1;
		string secondToken = TransfluentUtility.get(secondToDisplay.ToString());
		string textToDisplay = TransfluentUtility.getFormatted(textFormat, secondToken);
		GUILayout.Label(textToDisplay);
		textMesh.text = textToDisplay;

		foreach(string languageCode in languagesToShow)
		{
			if(GUILayout.Button("Translate to language:" + languageCode))
			{
				TransfluentUtility.changeStaticInstanceConfig(languageCode);
			}
		}
	}
}