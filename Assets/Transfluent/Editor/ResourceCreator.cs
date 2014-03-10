using UnityEditor;
using UnityEngine;

namespace transfluent.editor
{
	public class ResourceCreator
	{
		private const string basePath = "Assets/Transfluent/Resources/";

		public static T CreateSO<T>(string fileName) where T : ScriptableObject
		{
			var resource = ScriptableObject.CreateInstance<T>();
			string path;
			if (!fileName.Contains(basePath))
			{
				path = basePath + fileName;
			}
			else
			{
				path = fileName;
			}
			if (!path.ToLower().EndsWith(".asset"))
			{
				path += ".asset";
			}

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