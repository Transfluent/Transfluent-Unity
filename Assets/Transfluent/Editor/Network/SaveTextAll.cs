﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using transfluent;

namespace transfluent
{
	public class SaveTextKey
	{
		//URL: https://transfluent.com/v2/text/ ( HTTPS only)
		//Parameters: text_id, group_id, language, text, invalidate_translations [=1], is_draft, token
		public bool savedSuccessfully;
		public string text_id { get; set; } //the key we will refer to later
		public string text { get; set; }
		public string group_id { get; set; }
		public int language { get; set; } //language id, source

		public string authToken { get; set; }

		public void Execute()
		{
			IWebService service = new DebugSyncronousEditorWebRequest();
			var webserviceParams = new Dictionary<string, string>
			{
				{"text_id", text_id},
				{"language", language.ToString()},
				{"token", authToken},
				{"text", text}
			};

			if(group_id != null)
			{
				webserviceParams.Add("group_id", group_id);
			}
			string url = RestUrl.getURL(RestUrl.RestAction.TEXT);
			ReturnStatus status = service.request(url, webserviceParams);

			string responseText = status.text;

			if(status.status != ServiceStatus.SUCCESS)
				throw new Exception("Unsuccessful request " + status.rawErrorCode + " response" + responseText + " url:" + url);

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
			catch(Exception e)
			{
				if(e is ResponseReader<bool>.ApplicatonLevelException)
				{
					e.Message.Contains("EBackendTextAlreadyUpToDate"); //this is not an error, it is ok
					savedSuccessfully = true;
				}
			}
		}
	}
}