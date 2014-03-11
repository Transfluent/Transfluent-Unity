using System.Collections.Generic;
using transfluent;
using UnityEngine;

public class TranslationConfigurationSO : ScriptableObject
{
	public TransfluentLanguage sourceLanguage;
	public List<TransfluentLanguage> destinationLanguages;

	//allows for multiple clients to run simultaniously, so you can have 2 names in 2 different namespaces -- translates to "group id" for transfluent
	public string translation_set_group;  //so that sets of translations to clash, like a namespace.  means group_id for the transfluent api, but wou
}