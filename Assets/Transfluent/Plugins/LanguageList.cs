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
}