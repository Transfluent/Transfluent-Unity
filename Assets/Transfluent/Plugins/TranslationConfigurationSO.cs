using System.Collections.Generic;
using transfluent;
using UnityEngine;

public class TranslationConfigurationSO : ScriptableObject
{
	public List<TransfluentLanguage> destinationLanguages;
	public TransfluentLanguage sourceLanguage;

	//allows for multiple clients to run simultaniously, so you can have 2 names in 2 different namespaces -- translates to "group id" for transfluent
	[HideInInspector] public string translation_set_group;
	
	public OrderTranslation.TranslationQuality QualityToRequest = OrderTranslation.TranslationQuality.PROFESSIONAL_TRANSLATOR;
	//so that sets of translations to clash, like a namespace.  means group_id for the transfluent api, but wou
}