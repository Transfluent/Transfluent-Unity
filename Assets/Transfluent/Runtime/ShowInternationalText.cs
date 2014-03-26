using System.Collections.Generic;
using UnityEngine;

public class ShowAllKnownTextAtSameTime : MonoBehaviour
{
	private readonly List<string> knownStrings = new List<string>();

	private void Start()
	{
		knownStrings.Clear();
		GameTranslationSet[] list = Resources.LoadAll<GameTranslationSet>("");
		//this is *not* Assets/Transfluent/Resources, since all resources get put in the "resources" folder
		//Debug.Log("Number of translation sets:" + list.Length);
		foreach(GameTranslationSet set in list)
		{
			foreach(string value in set.getAllValues())
			{
				knownStrings.Add(value);
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