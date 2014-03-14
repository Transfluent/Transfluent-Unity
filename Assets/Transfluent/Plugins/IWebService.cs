using Pathfinding.Serialization.JsonFx;
using System;
using System.Collections.Generic;

namespace transfluent
{
	public interface IWebService
	{
		WebServiceReturnStatus request(string url);

		WebServiceReturnStatus request(string url, Dictionary<string, string> postParams);

		WebServiceReturnStatus request(ITransfluentParameters parameters);
	}

	public struct WebServiceReturnStatus
	{
		public int httpErrorCode;
		public TimeSpan requestTimeTaken;
		public ITransfluentParameters serviceParams;

		public string text; //if text is the  requested thing

		public override string ToString()
		{
			return "RETURN STATUS:" + JsonWriter.Serialize(this) + " time in seconds taken:" + requestTimeTaken.TotalSeconds;
		}
	}
}