using System;

namespace transfluent
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class Route : Attribute
	{
		public string helpUrl;
		public RestRequestType requestType;
		public string route;

		public Route(string routeIn, RestRequestType reqTypeIn, string helpUrlIn = null)
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