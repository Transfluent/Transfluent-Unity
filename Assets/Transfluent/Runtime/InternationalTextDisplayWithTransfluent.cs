#define transfluent
using System.Collections.Generic;
using transfluent;
using UnityEngine;
#if transfluent
using GUI = transfluent.guiwrapper.GUI;
using GUILayout = transfluent.guiwrapper.GUILayout; 
#else
using GUI = UnityEngine.GUI;
using GUILayout = UnityEngine.GUILayout;
#endif
/*

*/
public class InternationalTextDisplayWithTransfluent : MonoBehaviour
{
	private readonly List<string> knownStrings = new List<string>();
	[SerializeField] private string textToDisplay = "我是一个中国人的一句。";

	[SerializeField] private TransfluentTranslation translation = new TransfluentTranslation();

	// Use this for initialization
	private void Start()
	{
	}

	// Update is called once per frame
	private void Update()
	{
	}


	private void OnGUI()
	{
		GUILayout.TextField(textToDisplay);
		if (GUILayout.Button("TEST GET KNOWN TRANSLATIONS"))
		{
			knownStrings.Clear();
			GameTranslationSet[] list = Resources.LoadAll<GameTranslationSet>("");
				//this is *not* Assets/Transfluent/Resources, since all resources get put in the "resources" folder
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