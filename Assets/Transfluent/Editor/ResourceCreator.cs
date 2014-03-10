using UnityEditor;
using UnityEngine;

namespace transfluent.editor
{
	public class ResourceCreator
	{
		private const string basePath = "Assets/Transfluent/Resources/";

		public static void SaveSO(string fileName, ScriptableObject so)
		{
			string path = getPathFromFileName(fileName);
			AssetDatabase.CreateAsset(so, path);
			SetResourceDirtyAndSave(so);
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
			string path = getPathFromFileName(fileName);

			AssetDatabase.CreateAsset(resource, path);
			SetResourceDirtyAndSave(resource);

			return resource;
		}

		public static void SetResourceDirtyAndSave(Object obj)
		{
			EditorUtility.SetDirty(obj);
			AssetDatabase.SaveAssets();
		}
	}
}