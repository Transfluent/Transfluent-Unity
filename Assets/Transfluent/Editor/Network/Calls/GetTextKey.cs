using System;
using System.Collections.Generic;

namespace transfluent
{
	public class GetTextKey
	{
		public string keyValue { get; set; }
		public string text_id { get; set; }
		public string group_id { get; set; }
		public int languageID { get; set; }

		public string authToken { get; set; }

		[Inject]
		public IWebService service { get; set; }

		public void Execute()
		{
			var webserviceParams = new Dictionary<string, string>
			{
				{"text_id", text_id},
				{"language", languageID.ToString()},
				{"token", authToken}
			};

			if (group_id != null)
			{
				webserviceParams.Add("group_id", group_id);
			}
			string url = RestUrl.getURL(RestUrl.RestAction.TEXT) + service.encodeGETParams(webserviceParams);
			ReturnStatus status = service.request(url);
			// + service.encodeGETParams(webserviceParams)
			string responseText = status.text;

			if (status.status != ServiceStatus.SUCCESS)
				throw new Exception("Unsuccessful request " + status.rawErrorCode + " response" + responseText + " url:" + url);

			var reader = new ResponseReader<string>
			{
				text = responseText
			};
			reader.deserialize();

			keyValue = reader.response;
		}
	}
}