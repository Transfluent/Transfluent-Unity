using System.Collections.Generic;

namespace transfluent
{
	public class LanguageList
	{
		public List<TransfluentLanguage> languages { get; set; }

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

	public class RequestAllLanguages : ITransfluentCall
	{
		public LanguageList languagesRetrieved;

		[Inject]
		public IWebService service { get; set; }

		public WebServiceReturnStatus webServiceStatus { get; private set; }

		public void Execute()
		{
			webServiceStatus = service.request(RestUrl.getURL(RestUrl.RestAction.LANGUAGES));

			string responseText = webServiceStatus.text;


			var reader = new ResponseReader<List<Dictionary<string, TransfluentLanguage>>>
			{
				text = responseText
			};
			reader.deserialize();

			var languages = new List<TransfluentLanguage>();
			foreach (var listitem in reader.response)
			{
				foreach (var kvp in listitem)
				{
					languages.Add(kvp.Value);
				}
			}
			languagesRetrieved = new LanguageList
			{
				languages = languages
			};
		}
	}
}