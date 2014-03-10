using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace transfluent.editor
{
	public class ResourceCreator
	{
		private const string basePath = "Assets/Transfluent/Resources/";

		public static void SaveSO(string fileName, ScriptableObject so)
		{
			string path = getPathFromFileName(fileName);
			AssetDatabase.CreateAsset(so, path);
		}

		static string getPathFromFileName(string fileName)
		{
			string path;
			if(!fileName.Contains(basePath))
			{
				path = basePath + fileName;
			}
			else
			{
				path = fileName;
			}
			if(!path.ToLower().EndsWith(".asset"))
			{
				path += ".asset";
			}
			return path;
		}

		public static T CreateSO<T>(string fileName) where T : ScriptableObject
		{
			var resource = ScriptableObject.CreateInstance<T>();
			if (resource == null)
			{
				throw new Exception("Cannot create SO of type:"+typeof(T));
			}
			string path = getPathFromFileName(fileName);

			AssetDatabase.CreateAsset(resource, path);
			EditorUtility.SetDirty(resource);
			AssetDatabase.SaveAssets();

			return resource;
		}

	}
}