using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace transfluent
{
	public class AssetScanner
	{
		private readonly List<IGameProcessor> _gameProcessors =
			new List<IGameProcessor>();
		List<GameObject> toIgnore = new List<GameObject>();
		private readonly CustomScriptProcessorState _customProcessorState;
		public AssetScanner(List<IGameProcessor> customProcessors=null)
		{
			var stringFormatToIgnore = new List<string>() {"XXXX"};

			_customProcessorState = new CustomScriptProcessorState(toIgnore, TranslationUtility.getUtilityInstanceForDebugging(), stringFormatToIgnore);

			if(customProcessors != null)
				_gameProcessors.AddRange(customProcessors);

			//_gameProcessors.Add(new ButtonViewProcessor());
			
			_gameProcessors.Add(new TextMeshProcessor());
			_gameProcessors.Add(new GUITextProcessor());
		}

		[MenuItem("Translation/testScan")]
		public static void fullMigration()
		{
			AssetScanner scanner = new AssetScanner();
			/*var gos = scanner.getAllGameObjectsInScene();
			foreach (var go in gos)
			{
				//Debug.Log("name:"+go.name);
			}*/
			//scanner.searchGameObjects(scanner.getAllGameObjectsInScene());

			//string activeSelectionPath = AssetDatabase.GetAssetPath(Selection.activeObject);
			//activeSelectionPath = "Assets/art/scenes/level_block.prefab";
			//Debug.Log("Active selection path:" + activeSelectionPath);
			//scanner.searchPrefab(activeSelectionPath);
			//scanner.searchGameObjects();
			scanner.searchScenes();

			scanner.searchPrefabs();
		}

		public void searchPrefabs()
		{
			string[] aMaterialFiles = Directory.GetFiles(Application.dataPath, "*.prefab", SearchOption.AllDirectories);
			foreach(string matFile in aMaterialFiles)
			{
				string assetPath = "Assets" + matFile.Replace(Application.dataPath, "").Replace('\\', '/');

				searchPrefab(assetPath);
			}
		}

		public void searchScenes()
		{
			string[] sceneFiles = Directory.GetFiles(Application.dataPath, "*.unity", SearchOption.AllDirectories);
			foreach (string scene in sceneFiles)
			{
				//Debug.Log("Looking at scene file:" + scene);
				EditorApplication.OpenScene(scene);

				searchGameObjects(getAllGameObjectsInScene());

				EditorApplication.SaveScene();

				AssetDatabase.SaveAssets();
			}
		}

		public void searchPrefab(string prefabLocation)
		{
			string assetPath = prefabLocation.Replace(Application.dataPath, "").Replace('\\', '/');
			if(!prefabLocation.StartsWith("Assets"))
			{
				assetPath = "Assets/" + assetPath;
			}
			var prefab = (GameObject)AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject));

			if(prefab == null)
			{
				Debug.LogError("could not load prefab at path: "+assetPath + " input:"+prefabLocation);
			}

			//getAllGameObjectsInPrefab()

			var originalScene = EditorApplication.currentScene;
			//cheat with a scene in order to make sure to get disabled components.  Feels odd, but I hope I find something better later
			EditorApplication.NewScene();

			//delete the main camera and any other default objects
			var gameobjects = GameObject.FindObjectsOfType<GameObject>();
			foreach(var existingGo in gameobjects)
			{
				GameObject.DestroyImmediate(existingGo);
			}

			GameObject instanceOfPrefab = GameObject.Instantiate(prefab) as GameObject;

			var listOfGameobjectsInPrefab = getAllGameObjectsInScene();

			searchGameObjects(listOfGameobjectsInPrefab);

			PrefabUtility.ReconnectToLastPrefab(instanceOfPrefab);
			
			EditorUtility.SetDirty(instanceOfPrefab);
			AssetDatabase.SaveAssets();

			GameObject.DestroyImmediate(instanceOfPrefab);
			string scene = originalScene.Replace("Assets/", "");
			EditorApplication.OpenScene(scene); //go back to the scene we started with
		}

		public void searchGameObjects(List<GameObject> allGameObjectsInScene)
		{
			//loop through the processors in order (so that gameobjects not directly linked can get blacklisted)
			foreach(IGameProcessor processor in _gameProcessors)
			{
				foreach(GameObject go in allGameObjectsInScene)
				{
					if(go == null)
						continue;
					if(toIgnore.Contains(go))
						continue;
					//Debug.Log("Looking at go:" + go.name);

					processor.process(go, _customProcessorState);
				}
			}
		}
		
		//finding disabled objects
		//http://docs.unity3d.com/Documentation/ScriptReference/Resources.FindObjectsOfTypeAll.html
		public List<GameObject> getAllGameObjectsInScene()
		{
			var objectsInScene = new List<GameObject>();

			foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof (GameObject)) as GameObject[])
			{
				//this filter seems to miss a few important things.  a bunch of primitives show up
				if(go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave || 
					go.hideFlags == HideFlags.HideInHierarchy || go.hideFlags == HideFlags.HideInInspector)
					continue;
				
				
				string assetPath = AssetDatabase.GetAssetPath(go.transform.root.gameObject);
				if(!String.IsNullOrEmpty(assetPath))
					continue;

				objectsInScene.Add(go);
			}

			return objectsInScene;
		}
	}
}
