using System.Collections.Generic;

namespace transfluent
{
	public class TextStatus : ITransfluentCall
	{
		public bool wasTranslated;
		public string text_id { get; set; }
		public string group_id { get; set; }
		public int language_id { get; set; }

		[Inject(NamedInjections.API_TOKEN)]
		public string authToken { get; set; }

		[Inject]
		public IWebService service { get; set; }

		public WebServiceReturnStatus webServiceStatus { get; private set; }

		public void Execute()
		{
			var postParams = new Dictionary<string, string>
			{
				{"token", authToken},
				{"text_id", text_id},
				{"language", language_id.ToString()},
			};
			if (!string.IsNullOrEmpty(group_id)) postParams.Add("group_id", group_id);

			webServiceStatus =
				service.request(RestUrl.getURL(RestUrl.RestAction.TEXTSTATUS) + service.encodeGETParams(postParams));
			if (webServiceStatus.status != ServiceStatus.SUCCESS)
			{
				wasTranslated = false;
				return;
			}

			string responseText = webServiceStatus.text;
			var reader = new ResponseReader<TextStatusResult>
			{
				text = responseText
			};
			reader.deserialize();

			wasTranslated = reader.response.is_translated;
		}

		
	}
}