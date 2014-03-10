using System.Collections.Generic;
using transfluent;
using UnityEngine;
using UnityEditor;

public class TranslationConfigurationSO : ScriptableObject
{
	public TransfluentLanguage sourceLanguage;
	public List<TransfluentLanguage> destinationLanguages;

	//allows for multiple clients to run simultaniously
	public string translation_set_group;  //so that sets of translations to clash, like a namespace.  means group_id for the transfluent api, but wou
}