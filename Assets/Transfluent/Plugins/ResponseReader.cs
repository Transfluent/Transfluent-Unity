using Pathfinding.Serialization.JsonFx;

namespace transfluent
{
	public interface IResponseReader
	{
		T deserialize<T>(string text);
	}

	public class ResponseReader : IResponseReader
	{
		public T deserialize<T>(string text)
		{
			return JsonReader.Deserialize<T>(text);
		}
	}
}