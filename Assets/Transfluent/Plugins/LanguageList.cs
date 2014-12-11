using System;
using System.Collections.Generic;
using transfluent;
using UnityEngine;

[Serializable]
public class LanguageList
{
	//TODO: find out if there's a reasonable way to create enum list from language list file... this mirrors much of the same data
	public enum LanguageName
	{
		[LanguageName("English", "en-us", SystemLanguage.English, true)] ENGLISH,
		[LanguageName("French", "fr-fr", SystemLanguage.French, true)] FRENCH,
		[LanguageName("Spanish", "es-la", SystemLanguage.Spanish, true)] SPANISH,
		[LanguageName("Portuguese (Portugal)", "pt-pt", SystemLanguage.Unknown, true)] PORTUGUESE_PORTUGAL,
		[LanguageName("Portuguese (Brazil)", "pt-br", SystemLanguage.Portuguese, true)] PORTUGUESE_BRAZIL,
		[LanguageName("Italian", "it-it", SystemLanguage.Italian, true)] ITALIAN,
		[LanguageName("German", "de-de", SystemLanguage.German, true)] GERMAN,
		[LanguageName("Chinese (Mandarin, Simplified)", "zh-cn", SystemLanguage.Chinese, true)] CHINESE_SIMPLIFIED,
		[LanguageName("Chinese (Traditional)", "zh-tw", SystemLanguage.Unknown, true)] CHINESE_TRADITIONAL,
		[LanguageName("Dutch", "nl-nl", SystemLanguage.Dutch, true)] DUTCH,
		[LanguageName("Japanese", "ja-jp", SystemLanguage.Japanese, true)] JAPANESE,
		[LanguageName("Korean", "ko-kr", SystemLanguage.Korean, true)] KOREAN,
		[LanguageName("Vietnamese", "vi-vn", SystemLanguage.Vietnamese, true)] VIETNAMESE,
		[LanguageName("Russian", "ru-ru", SystemLanguage.Russian, true)] RUSSIAN,
		[LanguageName("Swedish", "sv-se", SystemLanguage.Swedish, true)] SWEDISH,
		[LanguageName("Danish", "da-dk", SystemLanguage.Danish, true)] DANISH,
		[LanguageName("Norwegian", "no-no", SystemLanguage.Norwegian, true)] NORWEGIAN,
		[LanguageName("Turkish", "tr-tr", SystemLanguage.Turkish, true)] TURKISH,
		[LanguageName("Greek", "el-gr", SystemLanguage.Greek, true)] GREEK,
		[LanguageName("Indoesian", "id-id", SystemLanguage.Indonesian, true)] INDONESIAN,
		[LanguageName("Malay", "ms-my", SystemLanguage.Unknown, true)] MALAY,
		[LanguageName("Thai", "th-th", SystemLanguage.Thai, true)] THAI,
		[LanguageName("Backwards Testing Language", "xx-xx", SystemLanguage.Unknown, true)] BACKWARDS_TEST_LANGUAGE
	}

	private static Dictionary<LanguageName, LanguageNameAttribute> _cacheOfAllSimplifiedLanguageAttributes;
	private List<string> _simplifiedLanguageCodeList;
	public List<TransfluentLanguage> languages;

	public List<string> allLanguageNames()
	{
		var languageCodes = new List<string>();
		languages.ForEach((TransfluentLanguage lang) => { languageCodes.Add(lang.name); });
		return languageCodes;
	}

	public List<string> allLanguageCodes()
	{
		var languageCodes = new List<string>();
		languages.ForEach((TransfluentLanguage lang) => { languageCodes.Add(lang.code); });
		return languageCodes;
	}

	public TransfluentLanguage getLangaugeByID(int id)
	{
		return languages.Find((TransfluentLanguage lang) => { return lang.id == id; });
	}

	public TransfluentLanguage getLangaugeByCode(string code)
	{
		return languages.Find((TransfluentLanguage lang) => { return lang.code == code; });
	}

	public TransfluentLanguage getLangaugeByName(string name)
	{
		return languages.Find((TransfluentLanguage lang) => { return lang.name == name; });
	}

	public List<string> getListOfIdentifiersFromLanguageList()
	{
		var list = new List<string>();
		foreach (var lang in languages)
		{
			list.Add(lang.name);
		}
		return list;
	}

	public List<string> getSimplifiedListOfIdentifiersFromLanguageList()
	{
		var list = new List<string>();
		foreach (var code in getSimplifiedLanguageCodeList())
		{
			list.Add(getLangaugeByCode(code).name);
		}
		return list;
	}

	private List<string> getSimplifiedLanguageCodeList()
	{
		if(_simplifiedLanguageCodeList == null)
		{
			_simplifiedLanguageCodeList = new List<string>();
			var type = typeof (LanguageName);
			foreach (LanguageName languageNameValue in Enum.GetValues(type))
			{
				_simplifiedLanguageCodeList.Add(getLanguageNameAttributeForValue(languageNameValue).LanguageCode);
			}
		}
		return _simplifiedLanguageCodeList;
	}

	public static LanguageName getLanguageNameFromSystemLanguage(SystemLanguage lang)
	{
		initialzeCachedSimplifiedLanguages();
		foreach (var attributeKvp in _cacheOfAllSimplifiedLanguageAttributes)
		{
			if(attributeKvp.Value.SystemLang == lang)
				return attributeKvp.Key;
		}
		Debug.LogWarning("unrecognized system language passed in:" + lang);
		return LanguageName.BACKWARDS_TEST_LANGUAGE;
	}

	public static LanguageNameAttribute getLanguageAttributeFromSystemLanguage(SystemLanguage lang)
	{
		initialzeCachedSimplifiedLanguages();
		foreach (var attributeKvp in _cacheOfAllSimplifiedLanguageAttributes)
		{
			if(attributeKvp.Value.SystemLang == lang)
				return attributeKvp.Value;
		}
		Debug.LogWarning("unrecognized system language passed in:" + lang);
		return null;
	}

	private static void initialzeCachedSimplifiedLanguages()
	{
		if(_cacheOfAllSimplifiedLanguageAttributes != null)
			return;
		_cacheOfAllSimplifiedLanguageAttributes = new Dictionary<LanguageName, LanguageNameAttribute>();
		var type = typeof (LanguageName);
		foreach (LanguageName languageNameValue in Enum.GetValues(type))
		{
			var memberInfo = type.GetMember(Enum.GetName(type, languageNameValue));
			var targetAttribute =
				(LanguageNameAttribute) memberInfo[0].GetCustomAttributes(typeof (LanguageNameAttribute), false)[0];
			_cacheOfAllSimplifiedLanguageAttributes.Add(languageNameValue, targetAttribute);
		}
	}

	private static LanguageNameAttribute getLanguageNameAttributeForValue(LanguageName langauageValue)
	{
		initialzeCachedSimplifiedLanguages();
		return _cacheOfAllSimplifiedLanguageAttributes[langauageValue];
	}

	//right nowthis is not often used, so the reflection is probably ok
	public static string getLanguageCodeFromLanguageName(LanguageName languageName)
	{
		return getLanguageNameAttributeForValue(languageName).LanguageCode;
	}

	//TODO: find out if there's a reasonable way to create enum list from language list file... this mirrors much of the same data
	public class LanguageNameAttribute : Attribute
	{
		public string CommonName;
		public bool IsInAppStoreLanguageList;
		public string LanguageCode;
		public SystemLanguage SystemLang;
		//NOTE: should I create alternate name list to be able to iterate through?  Hand-input will likley have variations on the core name
		//ie name in it's own language, English (<name native>), <name native> (English), formal, slang, etc

		public LanguageNameAttribute(string commonName, string languageCode,
			SystemLanguage languageEquivilant = SystemLanguage.Unknown, bool isAnAppstoreLanguage = false)
		{
			CommonName = commonName;
			LanguageCode = languageCode;
			IsInAppStoreLanguageList = isAnAppstoreLanguage;
			SystemLang = languageEquivilant;
		}
	}
}