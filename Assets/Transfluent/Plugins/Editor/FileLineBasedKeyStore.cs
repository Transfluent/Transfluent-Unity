using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace transfluent.editor
{
	public class FileLineBasedKeyStore : IKeyStore
	{
		private readonly List<string> _keyMap;
		private List<string> lines;

		public FileLineBasedKeyStore(StreamReader reader, List<string> keyMap)
		{
			string text = reader.ReadToEnd();
			_keyMap = keyMap;
			init(text);
		}

		//Takes a project based path and returns a line delimited file store
		//TODO: check this path on windows, as I believe I need to change the directory characters to prevent paths like C:\foo\bar\project\Assets/stuff/junk
		public FileLineBasedKeyStore(string fileName, List<string> keyMap)
		{
			_keyMap = keyMap;
			string projectBase = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
			string text = File.ReadAllText(projectBase + Path.DirectorySeparatorChar + fileName);
				//this gives you the wrong path, not a project based one
			//string text = (AssetDatabase.LoadAssetAtPath(fileName, typeof (TextAsset)) as TextAsset).text;
			init(text);
		}

		public string get(string key)
		{
			if (_keyMap.Contains(key) == false)
			{
				return "";
			}

			//we checked that these indexes aren't invalid up front
			return lines[_keyMap.IndexOf(key)];
		}

		public void set(string key, string value)
		{
			//Debug.Log(string.Format("key{0} index{1}  lineCount{2}", key, _keyMap.IndexOf(key), lines.Count));
			if (_keyMap.Contains(key))
			{
				lines[_keyMap.IndexOf(key)] = value;
			}
			_keyMap.Add(key);
			lines.Add(value);
		}

		public void init(string text)
		{
			lines = new List<string>(text.Split(new[] {'\r', '\n'}));

			if (_keyMap.Count < lines.Count)
			{
				throw new FileLoadException("More keys requested than there were lines in the file");
			}
		}
	}
}