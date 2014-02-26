using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace transfluent
{
	public class LanguageList
	{
		public List<TransfluentLanguage2> languages { get; set; }

		public TransfluentLanguage2 getLangaugeByID(int id)
		{
			return languages.Find((TransfluentLanguage2 lang) => { return lang.id == id; });
		}

		public TransfluentLanguage2 getLangaugeByCode(string code)
		{
			return languages.Find((TransfluentLanguage2 lang) => { return lang.code == code; });
		}

		public TransfluentLanguage2 getLangaugeByName(string name)
		{
			return languages.Find((TransfluentLanguage2 lang) => { return lang.name == name; });
		}
	}
	public class RequestAllLanguages
	{
		public LanguageList languagesRetrieved;

		public void Execute()
		{
			IWebService service = new SyncronousEditorWebRequest();
			ReturnStatus status = service.request(RestUrl.getURL(RestUrl.RestAction.LANGUAGES));

			string responseText = status.text;


			var reader = new ResponseReader<List<Dictionary<string, TransfluentLanguage2>>>
			{
				text = responseText
			};
			reader.deserialize();
			
			var languages = new List<TransfluentLanguage2>();
			foreach(var listitem in reader.response)
			{
				foreach(var kvp in listitem)
				{
					languages.Add(kvp.Value);
				}
			}
			languagesRetrieved = new LanguageList()
			{
				languages = languages
			};
		}
	}
}
