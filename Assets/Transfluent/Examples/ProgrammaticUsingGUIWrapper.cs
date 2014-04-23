using UnityEngine;
using GUILayout = transfluent.guiwrapper.GUILayout; //use wrappers to automatically translate your text

public class ProgramaticUsingGUIWrapper : MonoBehaviour
{
	public string keyTransformedByWrappers;

	// Use this for initialization
	void Start () {
	
	}

	void OnGUI()
	{
		GUILayout.Label(keyTransformedByWrappers);
		GUILayout.Label("Begin!");
	}
}
