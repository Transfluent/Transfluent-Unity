using UnityEditor;

namespace transfluent.editor
{
	public class EditorKeyStore : IKeyStore
	{
		public string get(string key)
		{
			return EditorPrefs.GetString(key);
		}

		public void set(string key, string value)
		{
			EditorPrefs.SetString(key, value);
		}
	}
}