﻿using System.Collections.Generic;
using System.ComponentModel;

namespace transfluent
{
	public class GetAllExistingTranslationKeys
	{
		public List<TransfluentTranslation> translations;

		[DefaultValue(100)]
		public int limit { get; set; }

		public string group_id { get; set; }
		public string authToken { get; set; }
		public int offset { get; set; }

		[Inject]
		public IWebService service { get; set; }

		public void Execute()
		{
			var getParams = new Dictionary<string, string>
			{
				{"token", authToken}
			};
			if (!string.IsNullOrEmpty(group_id))
			{
				getParams.Add("groupid", group_id);
			}
			if (limit > 0)
			{
				getParams.Add("limit",limit.ToString());
			}
			if (offset > 0)
			{
				getParams.Add("offset",offset.ToString());
			}
			string url = RestUrl.getURL(RestUrl.RestAction.TEXTSORDERS) + service.encodeGETParams(getParams);
			ReturnStatus status = service.request(url);

			string responseText = status.text;

			var reader = new ResponseReader<List<TransfluentTranslation>>
			{
				text = responseText
			};
			reader.deserialize();
			translations = reader.response;
		}
	}
}