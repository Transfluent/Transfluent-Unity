using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using transfluent;

namespace transfluent
{
	//TODO: make savetext a child of gettext
	public class GetTextKey : SaveTextKey
	{
		public string keyValue { get; set; }

		public new void Execute()
		{
			IWebService service = new SyncronousEditorWebRequest();
			var webserviceParams = new Dictionary<string, string>
			{
				{"text_id", text_id},
				{"language", language.ToString()},
				{"token", authToken}
			};

			if(group_id != null)
			{
				webserviceParams.Add("group_id", group_id);
			}
			string url = RestUrl.getURL(RestUrl.RestAction.TEXT) + service.encodeGETParams(webserviceParams);
			ReturnStatus status = service.request(url);
			// + service.encodeGETParams(webserviceParams)
			string responseText = status.text;

			if(status.status != ServiceStatus.SUCCESS)
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
