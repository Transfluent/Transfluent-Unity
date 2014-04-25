using System;
using System.Collections.Generic;
using transfluent;
using UnityEngine;
using GUILayout = transfluent.guiwrapper.GUILayout;

public class Sample2_StringsWithDynamicText : MonoBehaviour
{
	[SerializeField]
	private TextMesh textMesh;

	private string helloTodayMessage = "Hello, {0} how are you doing today: {1}";
	private string textFormat = "going to play soon {0}";

	private string userName = "Alex";
	private string date = DateTime.Now.ToString();

	private List<string> languagesToShow = new List<string>()
	{
		"en-us",
		"de-de",
		"fr-fr",
		"xx-xx"
	};

	private void Start()
	{
	}

	private void OnGUI()
	{
		int secondToDisplay = Mathf.FloorToInt(Time.timeSinceLevelLoad) % 4 + 1;
		string secondToken = TranslationUtility.get(secondToDisplay.ToString());
		string textToDisplay = TranslationUtility.getFormatted(textFormat, secondToken);
		GUILayout.Label(textToDisplay);
		textMesh.text = textToDisplay;

		//get the username from the 
		GUILayout.BeginHorizontal();
		GUILayout.Label("Username:");
		userName = GUILayout.TextField(userName);
		GUILayout.EndHorizontal();

		string formattedText = TranslationUtility.getFormatted(helloTodayMessage, userName, date);
		GUILayout.Label(formattedText);

		foreach(string languageCode in languagesToShow)
		{
			if(GUILayout.Button(TranslationUtility.getFormatted("Translate to language: {0}", languageCode) ))
			{
				TranslationUtility.changeStaticInstanceConfig(languageCode);
			}	
		}
	}
}