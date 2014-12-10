using System;
using System.Collections.Generic;
using transfluent;

[Serializable]
public class LanguageList
{
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
		foreach(TransfluentLanguage lang in languages)
		{
			list.Add(lang.name);
		}
		return list;
	}

	public List<string> getSimplifiedListOfIdentifiersFromLanguageList()
	{
		var list = new List<string>();
		foreach(string code in getSimplifiedLanguageCodeList())
		{
			list.Add(getLangaugeByCode(code).name);
		}
		return list;
	}

	private List<string> _simplifiedLanguageCodeList;

	private List<string> getSimplifiedLanguageCodeList()
	{
		if(_simplifiedLanguageCodeList == null)
		{
			_simplifiedLanguageCodeList = new List<string>();
			var type = typeof (LanguageName);
			foreach(var languageName in Enum.GetNames(type))
			{
				var meminfo = type.GetMember(languageName);
				var targetAttribute = (LanguageNameAttribute)meminfo[0].GetCustomAttributes(typeof(LanguageNameAttribute), false)[0];
				_simplifiedLanguageCodeList.Add(targetAttribute.CommonName);
			}
		}
		return _simplifiedLanguageCodeList;
	}

	//TODO: find out if there's a reasonable way to create enum list from language list file... this mirrors much of the same data
	public enum LanguageName
	{
		[LanguageNameAttribute("English","en-us",true)]
		ENGLISH,
		[LanguageNameAttribute("French", "fr-fr", true)]
		FRENCH,
		[LanguageNameAttribute("Spanish", "es-la", true)]
		SPANISH,
		[LanguageNameAttribute("Portuguese (Portugal)", "pt-pt", true)]
		PORTUGUESE_PORTUGAL,
		[LanguageNameAttribute("Portuguese (Brazil)", "pt-br", true)]
		PORTUGUESE_BRAZIL,
		[LanguageNameAttribute("Italian", "it-it", true)]
		ITALIAN,
		[LanguageNameAttribute("German", "de-de", true)]
		GERMAN,
		[LanguageNameAttribute("Chinese (Mandarin, Simplified)", "zh-cn", true)]
		CHINESE_SIMPLIFIED,
		[LanguageNameAttribute("Chinese (Traditional)", "zh-tw", true)]
		CHINESE_TRADITIONAL,
		[LanguageNameAttribute("Dutch", "nl-nl", true)]
		DUTCH,
		[LanguageNameAttribute("Japanese", "ja-jp", true)]
		JAPANESE,
		[LanguageNameAttribute("Korean", "ko-kr", true)]
		KOREAN,
		[LanguageNameAttribute("Vietnamese", "vi-vn", true)]
		VIETNAMESE,
		[LanguageNameAttribute("Russian", "ru-ru", true)]
		RUSSIAN,
		[LanguageNameAttribute("Swedish", "sv-se", true)]
		SWEDISH,
		[LanguageNameAttribute("Danish", "da-dk", true)]
		DANISH,
		[LanguageNameAttribute("Norwegian", "no-no", true)]
		NORWEGIAN,
		[LanguageNameAttribute("Turkish", "tr-tr", true)]
		TURKISH,
		[LanguageNameAttribute("Greek", "el-gr", true)]
		GREEK,
		[LanguageNameAttribute("Indoesian", "id-id", true)]
		INDONESIAN,
		[LanguageNameAttribute("Malay", "ms-my", true)]
		MALAY,
		[LanguageNameAttribute("Thai", "th-th", true)]
		THAI,
		[LanguageNameAttribute("Backwards Testing Language", "xx-xx", true)]
		BACKWARDS_TEST_LANGUAGE,
	}

	//TODO: find out if there's a reasonable way to create enum list from language list file... this mirrors much of the same data
	public class LanguageNameAttribute : Attribute
	{
		public string CommonName;
		public string LanguageCode;
		public bool IsInAppStoreLanguageList;
		//NOTE: should I create alternate name list to be able to iterate through?  Hand-input will likley have variations on the core name
		//ie name in it's own language, English (<name native>), <name native> (English), formal, slang, etc

		public LanguageNameAttribute(string commonName, string languageCode,bool isAnAppstoreLanguage=false)
		{
			CommonName = commonName;
			LanguageCode = languageCode;
			IsInAppStoreLanguageList = isAnAppstoreLanguage;
		}
	}
}