using UnityEngine;
using System.Collections.Generic;
using GUI = transfluent.guiwrapper.GUI;
using GUILayout = transfluent.guiwrapper.GUILayout; 
using transfluent;

public class SimpleTranslatedTextDisplay : MonoBehaviour
{
	[SerializeField] private TextMesh textMesh;
	private string textFormat = "going to play soon {0}";
	List<string> languagesToShow = new List<string>()
	{
		"en-us","de-de","fr-fr","xx-xx"
	};

	void Start()
	{
		TransfluentUtility.changeStaticInstanceConfig("en-us");
	}

	void OnGUI()
	{
		int secondToDisplay = Mathf.FloorToInt(Time.timeSinceLevelLoad)%4 + 1;
		//GUILayout.BeginArea(new Rect(Screen.width - 150,40,200,100));
		string secondToken = TransfluentUtility.getTranslation(secondToDisplay.ToString());
		string textToDisplay = TransfluentUtility.getFormattedTranslation(textFormat, secondToken);
		GUILayout.Label(textToDisplay);
		textMesh.text = textToDisplay;

		foreach (string languageCode in languagesToShow)
		{
			if (GUILayout.Button("Translate to language:" + languageCode))
			{
				TransfluentUtility.changeStaticInstanceConfig(languageCode);
			}
		}

		//GUILayout.EndArea();


	}
}
