using System;
using System.Collections.Generic;
using transfluent;

[Serializable]
public class LanguageList
{
	public List<TransfluentLanguage> languages;

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
}

