using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace transfluent
{
	public class GetAllExistingTranslationKeys : ITransfluentCall
	{
		public List<TransfluentTranslation> translations;
		public Dictionary<string, TransfluentTranslation> translationsDictionary;

		[DefaultValue(100)]
		public int limit { get; set; }

		public string group_id { get; set; }
		public int offset { get; set; }
		public int language { get; set; }

		[Inject(NamedInjections.API_TOKEN)]
		public string authToken { get; set; }

		[Inject]
		public IWebService service { get; set; }

		public WebServiceReturnStatus webServiceStatus { get; private set; }

		public void Execute()
		{
			if (language <= 0) throw new Exception("INVALID Language in getAllExistingKeys");

			var getParams = new Dictionary<string, string>
			{
				{"language", language.ToString()},
				{"token", authToken}
			};
			if (!string.IsNullOrEmpty(group_id))
			{
				getParams.Add("groupid", group_id);
			}
			if (limit > 0)
			{
				getParams.Add("limit", limit.ToString());
			}
			if (offset > 0)
			{
				getParams.Add("offset", offset.ToString());
			}
			string url = RestUrl.getURL(RestUrl.RestAction.TEXTS) + service.encodeGETParams(getParams);
			webServiceStatus = service.request(url);

			if (webServiceStatus.status != ServiceStatus.SUCCESS)
				return;

			string responseText = webServiceStatus.text;

			var reader = new ResponseReader<Dictionary<string, TransfluentTranslation>>
			{
				text = responseText
			};
			reader.deserialize();
			translationsDictionary = reader.response;

			translations = new List<TransfluentTranslation>();
			foreach (var kvp in translationsDictionary)
			{
				translations.Add(kvp.Value);
			}
		}
	}
}