using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace transfluent
{
	[Route("texts/orders",RestRequestType.GET)]
	public class GetAllOrders : ITransfluentCall
	{
		public Type expectedReturnType { get { return typeof(List<TransfluentOrder>); } }

		[DefaultValue(100)]
		public int limit { get; set; }

		public string group_id { get; set; }
		public int offset { get; set; }

		[Inject(NamedInjections.API_TOKEN)]
		public string authToken { get; set; }

		private readonly Dictionary<string, string> _getParams;

		public GetAllOrders(string group_id=null,int offset=0,int limit=0)
		{
			_getParams = new Dictionary<string, string>
			{
			};
			if (!string.IsNullOrEmpty(group_id))
			{
				_getParams.Add("groupid", group_id);
			}
			if (limit > 0)
			{
				_getParams.Add("limit", limit.ToString());
			}
			if (offset > 0)
			{
				_getParams.Add("offset", offset.ToString());
			}
		}
		public Dictionary<string, string> getParameters()
		{
			return _getParams;
		}

		public Dictionary<string, string> postParameters()
		{
			throw new System.NotImplementedException();
		}
	}
}