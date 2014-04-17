using System.Collections.Generic;
using System.IO;
using transfluent;
using UnityEditor;
using UnityEngine;

public class FindTextMeshReferences : MonoBehaviour
{
	private static readonly List<string> blacklistStringsContaining = new List<string>
	{
		"XXXX",
	};

	private static void setKeyInDefaultLanguageDB(string key, string value, string groupid = "")
	{
		//Debug.LogWarning("Make sure to set language to game source language before saving a new translation key");
		Dictionary<string, string> translationDictionary =
			TranslationUtility.getUtilityInstanceForDebugging().allKnownTranslations;
		TranslationConfigurationSO config = ResourceLoadFacade.LoadConfigGroup(groupid);

		GameTranslationSet gameTranslationSet =
			GameTranslationGetter.GetTranslaitonSetFromLanguageCode(config.sourceLanguage.code);

		bool exists = translationDictionary.ContainsKey(key);
		if (!exists)
		{
			translationDictionary.Add(key, key);
		}
		translationDictionary[key] = value; //find a way to make sure the the SO gets set dirty?

		gameTranslationSet.mergeInSet(groupid, translationDictionary);
		//EditorUtility.SetnDirty(TransfluentUtility.getUtilityInstanceForDebugging());
	}

	//something that returns a mesh[]
	//TODO: reflection based solution?
	private static List<TextMesh> toExplicitlyIgnore(GameObject inPrefab = null)
	{
		//ignore all textmeshes referenced by all ButtonView components
		var listToIngore = new List<TextMesh>();

		//custom references -- TODO: replace with a reflection based solution
		//find gameobjects with [SerializeField] private or public vars and also define an OnLocalize

		var allButtons = new List<ButtonView>();
		if (inPrefab == null)
		{
			allButtons.AddRange(FindObjectsOfType<ButtonView>());
		}
		else
		{
			allButtons.AddRange(inPrefab.GetComponentsInChildren<ButtonView>(true));
		}
		allButtons.ForEach((ButtonView button) =>
		{
			if (button != null && button.labelMesh != null)
			{
				listToIngore.Add(button.labelMesh);
				string newKey = button.label;
				button.labelData.globalizationKey = newKey;

				setKeyInDefaultLanguageDB(newKey, newKey);

				//TODO: ensure that this is set to the source language of the game config before adding
				EditorUtility.SetDirty(button);
			}
		});

		var allMeshesInSource = new List<TextMesh>();
		if (inPrefab == null)
		{
			allMeshesInSource.AddRange(FindObjectsOfType<TextMesh>());
		}
		else
		{
			allMeshesInSource.AddRange(inPrefab.GetComponentsInChildren<TextMesh>(true));
		}

		foreach (TextMesh mesh in allMeshesInSource)
		{
			if (!shouldGlobalizeText(mesh.text))
			{
				listToIngore.Add(mesh);
			}
		}
		return listToIngore;
	}

	[MenuItem("Helpers/Test known key")]
	public static void TestKnownKey()
	{
		Debug.Log(TranslationUtility.get("Start Game"));
	}

	private static bool shouldGlobalizeText(string textIn)
	{
		foreach (string blacklist in blacklistStringsContaining)
		{
			if (textIn.Contains(blacklist))
				return false;
		}
		return true;
	}

	[MenuItem("Helpers/All of the above")]
	public static void UpdateReferences()
	{
		GetTextMeshReferencesFromPrefabs();
		string scene = EditorApplication.currentScene.Replace("Assets/", "");
		EditorApplication.OpenScene(scene);
		GetTextMeshReferences();
	}

	//NOTE you *must* be in the source language for this to not cause corruption issues!
	[MenuItem("Helpers/TestMesh In Current Scene")]
	public static TextMesh[] GetTextMeshReferences()
	{
		var meshes = FindObjectsOfType(typeof (TextMesh)) as TextMesh[];
		List<TextMesh> blacklist = toExplicitlyIgnore();
		foreach (TextMesh mesh in meshes)
		{
			if (blacklist.Contains(mesh)) continue;

			setTextMesh(mesh);
		}
		AssetDatabase.SaveAssets();
		return meshes;
	}

	[MenuItem("Helpers/TestMesh In All Scene map")]
	public static void GetTextMeshReferencesInScenes()
	{
		var scenePathToReferenceList = new Dictionary<string, TextMesh[]>();

		string[] sceneFiles = Directory.GetFiles(Application.dataPath, "*.unity", SearchOption.AllDirectories);
		foreach (string scene in sceneFiles)
		{
			Debug.Log("Looking at scene file:" + scene);
			EditorApplication.OpenScene(scene);
			TextMesh[] textMeshes = GetTextMeshReferences();
			scenePathToReferenceList.Add(scene, textMeshes);

			foreach (TextMesh mesh in textMeshes)
			{
				Debug.Log("Externally lookin at text mesh named:" + mesh.gameObject.name);
			}
		}
	}

	private static void setTextMesh(TextMesh mesh)
	{
		var translatable = mesh.GetComponent<LocalizedTextMesh>();

		if (translatable == null)
		{
			translatable = mesh.gameObject.AddComponent<LocalizedTextMesh>();
			translatable.textmesh = mesh; //just use whatever the source text is upfront, and allow the user to 
		}

		translatable.textmesh = mesh;

		//should this be reversed?
		translatable.localizableText.globalizationKey = mesh.text;
		setKeyInDefaultLanguageDB(mesh.text, mesh.text);
	}

	public static List<GameObject> getAllPrefabReferences()
	{
		var retList = new List<GameObject>();
		string[] aMaterialFiles = Directory.GetFiles(Application.dataPath, "*.prefab", SearchOption.AllDirectories);
		foreach (string matFile in aMaterialFiles)
		{
			string assetPath = "Assets" + matFile.Replace(Application.dataPath, "").Replace('\\', '/');
			var go = (GameObject) AssetDatabase.LoadAssetAtPath(assetPath, typeof (GameObject));

			retList.Add(go);
		}
		return retList;
	}

	[MenuItem("Helpers/Textmeshes in prefabs")]
	public static void GetTextMeshReferencesFromPrefabs()
	{
		//var assets = AssetDatabase.LoadAllAssetsAtPath("Assets") as Object[];
		List<GameObject> assets = getAllPrefabReferences();

		//Debug.Log("Assets:" + assets.Count);

		foreach (GameObject go in assets)
		{
			//Debug.Log("looking at path:" + AssetDatabase.GetAssetPath(go));
			if (go == null)
				continue;
			//Debug.Log("looking at go:" + go.gameObject);
			TextMesh[] textMeshSubObjects = go.GetComponentsInChildren<TextMesh>(true);
			if (textMeshSubObjects == null || textMeshSubObjects.Length == 0) continue;
			List<TextMesh> blacklisted = toExplicitlyIgnore(go);
			Debug.Log("gameobject has meshes:" + go.gameObject);
			foreach (TextMesh mesh in textMeshSubObjects)
			{
				if (blacklisted.Contains(mesh)) continue;
				setTextMesh(mesh);
			}

			EditorUtility.SetDirty(go);
			EditorApplication.SaveAssets();
		}

		AssetDatabase.SaveAssets();
		//then go through instances?
	}
}