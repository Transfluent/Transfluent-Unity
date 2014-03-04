using UnityEditor;
using UnityEngine;
using System.Collections;

namespace transfluent
{
	public class GameTranslationsCreator
	{
		private static string basePath = "Assets/Transfluent/Resources/";
		[MenuItem("Window/Create new game translations set")]
		public static void DoMenuItem()
		{
			AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<GameTranslationSet>(), basePath + "GameTranslationSet.asset");
		}
	}
}

