using System;

namespace transfluent
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class Route : Attribute
	{
		public string route;
		public RestRequestType requestType;
		public string helpUrl;

		public Route(string routeIn, RestRequestType reqTypeIn,string helpUrlIn=null)
		{
			route = routeIn;
			requestType = reqTypeIn;
			helpUrl = helpUrlIn;
		}
	}

	public enum RestRequestType
	{
		GET,
		POST,
	}
}
