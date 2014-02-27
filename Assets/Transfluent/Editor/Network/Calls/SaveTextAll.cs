using System;
using System.Collections.Generic;

namespace transfluent
{
	public class SaveTextKey : ITransfluentCall 
	{
		//URL: https://transfluent.com/v2/text/ ( HTTPS only)
		//Parameters: text_id, group_id, language, text, invalidate_translations [=1], is_draft, token
		public bool savedSuccessfully;
		public string text_id { get; set; } //the key we will refer to later
		public string text { get; set; }
		public string group_id { get; set; }
		public int language { get; set; } //language id, source

		[Inject(NamedInjections.API_TOKEN)]
		public string authToken { get; set; }

		[Inject]
		public IWebService service { get; set; }

		public WebServiceReturnStatus webServiceStatus { get; private set; }

		public void Execute()
		{
			var webserviceParams = new Dictionary<string, string>
			{
				{"text_id", text_id},
				{"language", language.ToString()},
				{"token", authToken},
				{"text", text}
			};

			if (group_id != null)
			{
				webserviceParams.Add("group_id", group_id);
			}
			string url = RestUrl.getURL(RestUrl.RestAction.TEXT);
			webServiceStatus = service.request(url, webserviceParams);

			string responseText = webServiceStatus.text;

			if(webServiceStatus.status != ServiceStatus.SUCCESS)
				throw new Exception("Unsuccessful request " + webServiceStatus.rawErrorCode + " response" + responseText + " url:" + url);

			var reader = new ResponseReader<bool>
			{
				text = responseText
			};
			savedSuccessfully = false;
			try
			{
				reader.deserialize();
				savedSuccessfully = reader.response;
			}
			catch (Exception e)
			{
				if (e is ResponseReader<bool>.ApplicatonLevelException)
				{
					e.Message.Contains("EBackendTextAlreadyUpToDate"); //this is not an error, it is ok
					savedSuccessfully = true;
				}
			}
		}

	}
}