using System;
using System.Collections.Generic;

namespace transfluent
{
	public interface ITransfluentParameters

	{
		Dictionary<string, string> getParameters { get; }
		Dictionary<string, string> postParameters { get; }
	}

	public class WebServiceParameters : ITransfluentParameters
	{
		private readonly Dictionary<string, string> _getParameters = new Dictionary<string, string>();
		private readonly Dictionary<string, string> _postParameters = new Dictionary<string, string>();

		//[Inject]
		public IResponseReader responseReader = new ResponseReader(); //{ get; set; }

		public Dictionary<string, string> getParameters
		{
			get { return _getParameters; }
		}

		public Dictionary<string, string> postParameters
		{
			get { return _postParameters; }
		}

		private EmptyResponseContainer getResponseContainer(string text)
		{
			return responseReader.deserialize<EmptyResponseContainer>(text);
		}

		protected T GetResponse<T>(string text)
		{
			EmptyResponseContainer responseContainer = getResponseContainer(text);

			if (!responseContainer.isOK())
			{
				throw new ApplicatonLevelException("Response indicated an error:" + responseContainer, responseContainer.error);
			}
			var responseFullyParse = responseReader.deserialize<ResponseContainer<T>>(text);
			return responseFullyParse.response;
		}
	}

	//something specific to the call went wrong
	public class ApplicatonLevelException : CallException
	{
		public Error details;
		public ApplicatonLevelException(string message, int httpStatusCode)
			: base(message)
		{
			details = new Error() {message = "HTTP ERROR CODE:" + httpStatusCode,type=httpStatusCode.ToString()};
		}
		public ApplicatonLevelException(string message, Error error)
			: base(message)
		{
			details = error;
		}
	}

	//base class for handling known exceptions 
	public abstract class CallException : Exception
	{
		public CallException()
		{
		}

		public CallException(string message) : base(message)
		{
		}

		//use this constructor for other exceptions that we want to wrap
		public CallException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}

	//We got an http error code
	public class HttpErrorCode : CallException
	{
		public int code;

		public HttpErrorCode(int httpErrorCode)
		{
			code = httpErrorCode;
		}
	}

	//Other unknown transport exception
	public class TransportException : CallException
	{
		public TransportException(string message) : base(message)
		{
		}
	}
}