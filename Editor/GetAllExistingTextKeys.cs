using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using transfluent;

namespace transfluent
{
	public class GetAllExistingTranslationKeys
	{
		public List<TransfluentTranslation> translations;

		[DefaultValue(100)]
		public int limit { get; set; }

		public int? groupid { get; set; }
		public string authToken { get; set; }

		public void Execute()
		{
			IWebService service = new SyncronousEditorWebRequest();
			ReturnStatus status = service.request(RestUrl.getURL(RestUrl.RestAction.TEXTSORDERS));

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
