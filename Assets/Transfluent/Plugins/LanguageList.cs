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
		foreach(string code in simplifiedLanguageCodeList)
		{
			list.Add(getLangaugeByCode(code).name);
		}
		return list;
	}

	//show the most commonly used languages that game developers will display
	public List<string> simplifiedLanguageCodeList = new List<string>()
	{
		"en-us",//English. US version
		"fr-fr", //French
		"es-la", //Spanish, latin american version
		"pt-pt", //Portuguese (Portugal)
		"pt-br",//Portuguese (Brazil)
		"it-it",//Italian
		"de-de",//German
		"zh-cn",//Chinese (Mandarin, Simplified) (People's Republic of China)
		"zh-tw",//Chinese (Traditional) (Taiwan)
		"nl-nl",//Dutch
		"ja-jp",//Japanese
		"ko-kr",//Korean
		"vi-vn",//Vietnamese
		"ru-ru",//Russian
		"sv-se",//Swedish
		"da-dk",//Danish
		"fi-fi",//Finnish
		"no-no",//Norwegian (Bokmal)
		"tr-tr",//Turkish
		"el-gr",//Greek
		"id-id",//Indonesian
		"ms-my",//Malay
		"th-th",//Thai
		"xx-xx", //backwards language, used for testing.  *HIGHLY* reccommended, instant, and free
	};
}