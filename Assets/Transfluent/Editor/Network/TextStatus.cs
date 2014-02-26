using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using NSubstitute.Core.Arguments;
using NUnit.Core.Extensibility;
using UnityEngine;
using UnityEditor;

namespace transfluent
{

	public class TextStatus
	{
		public string text_id { get; set; }
		public string group_id { get; set; }
		public int language_id { get; set; }

		public string authToken { get; set; }

		public bool wasTranslated;
		public void Execute()
		{
			IWebService service = new SyncronousEditorWebRequest();
			var postParams = new Dictionary<string, string>
			{
				{"token", authToken},
				{"text_id", text_id},
				{"language", language_id.ToString()},
			};
			if(!string.IsNullOrEmpty(group_id)) postParams.Add("group_id",group_id);

			ReturnStatus status = service.request(RestUrl.getURL(RestUrl.RestAction.TEXTSTATUS) + service.encodeGETParams(postParams));
			

			string responseText = status.text;
			var reader = new ResponseReader<TextStatusResult>
			{
				text = responseText
			};
			reader.deserialize();

			wasTranslated = reader.response.is_translated;
		}
	}
}