using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using Pathfinding.Serialization.JsonFx;
using UnityEngine;
using Debug = UnityEngine.Debug;

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
		public ITransfluentParameters serviceParams;
		public int httpErrorCode;
		public TimeSpan requestTimeTaken;

		public string text; //if text is the  requested thing

		public override string ToString()
		{
			return "RETURN STATUS:" + JsonWriter.Serialize(this) + " time in seconds taken:" + requestTimeTaken.TotalSeconds;
		}
	}
}