using System.Collections.Generic;
using transfluent;
using UnityEngine;
using GUI = UnityEngine.GUI;
using GUILayout = UnityEngine.GUILayout;

public class ShowAllKnownTextAtSameTime : MonoBehaviour
{
	private readonly List<string> knownStrings = new List<string>();

	[SerializeField]
	private TransfluentTranslation translation = new TransfluentTranslation();

	// Use this for initialization
	private void Start()
	{
		knownStrings.Clear();
		GameTranslationSet[] list = Resources.LoadAll<GameTranslationSet>("");
		//this is *not* Assets/Transfluent/Resources, since all resources get put in the "resources" folder
		//Debug.Log("Number of translation sets:" + list.Length);
		foreach(GameTranslationSet set in list)
		{
			foreach(TransfluentTranslation trans in set.allTranslations)
			{
				knownStrings.Add(trans.text);
			}
		}
	}

	private void OnGUI()
	{
		foreach(string knownString in knownStrings)
		{
			GUILayout.Label(knownString);
		}
	}
}