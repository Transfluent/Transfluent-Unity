using UnityEngine;
using transfluent;

public class ProgramaticOnGUIExample : MonoBehaviour
{
	public LocalizeUtil managedText;
	public string programaticallyManagedTextKey;
	public string programaticallyManagedText;
	private bool languageSelectToggle;

	// Use this for initialization
	void Start () {
	
	}
	
	void OnGUI()
	{
		//2 different ways of programmatically getting text
		GUILayout.Label(managedText.current);
		GUILayout.Label(programaticallyManagedText);
		languageSelectToggle = GUILayout.Toggle(languageSelectToggle, "Language Select");

		if(languageSelectToggle)
		{
			if(GUILayout.Button("English"))
				TranslationUtility.changeStaticInstanceConfig("en-us");
			if(GUILayout.Button("French"))
				TranslationUtility.changeStaticInstanceConfig("fr-fr");
			if(GUILayout.Button("German"))
				TranslationUtility.changeStaticInstanceConfig("de-de");
			if(GUILayout.Button("Backwards (test language)"))
				TranslationUtility.changeStaticInstanceConfig("xx-xx");
		}
	}

	public void OnEnable()
	{
		OnLocalize();
	}
	public void OnLocalize()
	{
		managedText.OnLocalize();
		programaticallyManagedText = TranslationUtility.get(programaticallyManagedTextKey);
	}
}
