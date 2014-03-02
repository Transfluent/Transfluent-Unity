using System;
using System.Collections.Generic;
using System.Net;

namespace transfluent
{
	public interface ITransfluentParameters

	{
		Dictionary<string, string> getParameters { get; }
		Dictionary<string, string> postParameters { get; }
	}

	public class WebServiceParameters : ITransfluentParameters
	{
		readonly Dictionary<string,string> _getParameters = new Dictionary<string, string>();
		readonly Dictionary<string,string> _postParameters = new Dictionary<string, string>();
		public Dictionary<string, string> getParameters { get { return _getParameters; } }
		public Dictionary<string, string> postParameters { get { return _postParameters; } }
		/*
		[Inject]
		public IResponseReader responseReader { get; set; }

		EmptyResponseContainer getResponseContainer(string text)
		{
			return responseReader.deserialize<EmptyResponseContainer>(text);
		}

		protected T GetResponse<T>(string text)
		{
			var responseContainer = getResponseContainer(text);
			
			if (!responseContainer.isOK())
			{
				throw new ApplicatonLevelException("Response indicated an error:" + responseContainer, responseContainer.error);
			}
			return responseReader.deserialize<T>(text);
		}
		*/
		public class HttpErrorCode : Exception
		{
			public int code;
			public HttpErrorCode(int httpErrorCode) : base()
			{
				code = httpErrorCode;
			}
		}

		public class ApplicatonLevelException : Exception
		{
			public Error details;
			public ApplicatonLevelException(string message, Error error)
				: base(message)
			{
				details = error;
			}
		}
	}
}
