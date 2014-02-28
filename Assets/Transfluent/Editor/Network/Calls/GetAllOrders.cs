using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Mime;
using UnityEditor;

namespace transfluent
{
	public class GetAllOrders : ITransfluentCall
	{
		public List<TransfluentTranslation> translations;

		[DefaultValue(100)]
		public int limit { get; set; }

		public string group_id { get; set; }
		public int offset { get; set; }


		[Inject(NamedInjections.API_TOKEN)]
		public string authToken { get; set; }

		[Inject]
		public IWebService service { get; set; }

		public WebServiceReturnStatus webServiceStatus { get; private set; }

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
			UnityEngine.Application.OpenURL(url);
			webServiceStatus = service.request(url);

			string responseText = webServiceStatus.text;

			var reader = new ResponseReader<List<TransfluentTranslation>>
			{
				text = responseText
			};
			reader.deserialize();
			translations = reader.response;
		}

	}
}