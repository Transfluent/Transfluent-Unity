using System.Collections.Generic;
using System.Runtime.Versioning;
using transfluent;
using UnityEngine;

public class InternationalTextDisplay : MonoBehaviour
{
	[SerializeField] string textToDisplay = "我是一个中国人的一句。";

	[SerializeField]
	private TransfluentTranslation translation = new TransfluentTranslation();

	List<string> knownStrings = new List<string>();

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	void OnGUI()
	{
		GUILayout.TextField(textToDisplay);
		if(GUILayout.Button("TEST GET KNOWN TRANSLATIONS"))
		{
			knownStrings.Clear();
			var list = Resources.LoadAll<GameTranslationSet>("");  //this is *not* Assets/Transfluent/Resources, since all resources get put in the "resources" folder
			//Debug.Log("Number of translation sets:" + list.Length);
			foreach (GameTranslationSet set in list)
			{
				foreach (TransfluentTranslation trans in set.allTranslations)
				{
					knownStrings.Add(trans.text);
				}
			}
		}
		foreach (string knownString in knownStrings)
		{
			GUILayout.Label(knownString);
		}
	}
}
