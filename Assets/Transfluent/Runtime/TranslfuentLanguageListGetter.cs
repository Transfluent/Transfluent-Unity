using System;
using System.Collections;
using Pathfinding.Serialization.JsonFx;
using transfluent;
using UnityEditor;
using UnityEngine;

public class TranslfuentLanguageListGetter
{
	private const string basePath = "Assets/Transfluent/Resources/";

	private const
		string fileName = "LanguageList";

	private LanguageListSO _languageList;

	private Action<LanguageList> _onGotList;
	private RequestAllLanguages webRequest;

	public TranslfuentLanguageListGetter(Action<LanguageList> getList)
	{
		_onGotList = getList;
		getLanguageListSO();
		if (_languageList.list != null && _languageList.list.languages != null && _languageList.list.languages.Count > 0)
		{
			doReturnCall();
		}
		else
		{
			getLanguageListForSO();
		}
	}

	private void doReturnCall()
	{
		if (_onGotList != null)
		{
			Action<LanguageList> tmpReturn = _onGotList;
			_onGotList = null;
			tmpReturn(_languageList.list);
		}
	}

	public static string LanguageListPath()
	{
		return basePath + fileName + ".asset";
	}

	private void getLanguageListSO()
	{
		string languageListFilePath = LanguageListPath();
		_languageList = AssetDatabase.LoadAssetAtPath(languageListFilePath, typeof(LanguageListSO)) as LanguageListSO;
		if (_languageList == null)
		{
			_languageList = ScriptableObject.CreateInstance<LanguageListSO>();
			AssetDatabase.CreateAsset(_languageList, languageListFilePath);
			AssetDatabase.SaveAssets();
		}
	}

	private GameTimeWWW www;
	private void getLanguageListForSO()
	{
		if (www == null)
		{
			www = new GameTimeWWW();
		}
		if (webRequest == null)
		{
			webRequest = new RequestAllLanguages();
		}
		www.webRequest(webRequest, gotResultOfLanguageList);
		
	}

	private IEnumerator gotResultOfLanguageList(WebServiceReturnStatus status)
	{
		Debug.Log("STATUS TEXT:" + status.text);
		if (string.IsNullOrEmpty(status.text))
		{
			Debug.LogWarning("Could not get language list, retrying");
			yield return new WaitForSeconds(5f); //TODO: graceful degredation
			var www = new GameTimeWWW();
			GameTimeWWW.GotstatusUpdate statusUpdate = gotResultOfLanguageList;
			www.webRequest(webRequest, statusUpdate);
			//www.startRoutine(getLanguageList());
			yield break;
		}
		try
		{
			Debug.Log("Debugging status text:"+ status.text);

			var tmpList = webRequest.Parse(status.text);
			_languageList.list = tmpList;
			EditorUtility.SetDirty(_languageList);
			doReturnCall();
		}
		catch (Exception e)
		{
			Debug.LogError("Error while parsing message:" + status.text + "  error:"+ e.Message + " original stack:"+ e.StackTrace);
		}
	}
}