using System;
using Pathfinding.Serialization.JsonFx;

namespace transfluent
{
	public class ResponseReader<T>
	{
		public Error error;
		public T response;
		public string text { get; set; }

		public void deserialize()
		{
			//TODO: figure out an elegant way of doing this without parsing twice
			//NOTE: I do this mainly because sometimes I get an empty/unexpected response type and an "OK" type, but an error
			//Debug.Log("STATUS:" + JsonWriter.Serialize(shell));
			var shell = JsonReader.Deserialize<EmptyResponseContainer>(text);
			if (shell.isOK())
			{
				var container = JsonReader.Deserialize<ResponseContainer<T>>(text);

				if (container.isOK())
				{
					response = container.response;
				}
			}
			else
			{
				error = shell.error;
				throw new ApplicatonLevelException("APP ERROR:" + error + " From raw text:" + text);
			}
		}

		public class ApplicatonLevelException : Exception
		{
			public ApplicatonLevelException(string message)
				: base(message)
			{
			}
		}
	}
}