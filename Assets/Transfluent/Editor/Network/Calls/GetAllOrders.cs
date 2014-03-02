using System.Collections.Generic;

namespace transfluent
{
	[Route("texts/orders", RestRequestType.GET, "http://transfluent.com/backend-api/#TextsOrders")]
	public class GetAllOrders : WebServiceParameters
	{
		[Inject(NamedInjections.API_TOKEN)]
		public string authToken { get; set; }

		public GetAllOrders(string group_id=null,int offset=0,int limit=0)
		{
			if (!string.IsNullOrEmpty(group_id))
			{
				getParameters.Add("groupid", group_id);
			}
			if (limit > 0)
			{
				getParameters.Add("limit", limit.ToString());
			}
			if (offset > 0)
			{
				getParameters.Add("offset", offset.ToString());
			}
		}

		public List<TransfluentOrder> Parse(string text)
		{
			return GetResponse<List<TransfluentOrder>>(text);
		}
	}
}