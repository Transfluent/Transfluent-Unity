using System;
using System.Runtime.InteropServices;
using Pathfinding.Serialization.JsonFx;
using transfluent;
using UnityEditor;
using UnityEngine;
using System.Collections;

public class TestCall : MonoBehaviour
{
	[MenuItem("Window/TestWWW")]
	public static void doesGameTimeWWWWorkInEditorMenuContext()
	{
		GameTimeWWW www = new GameTimeWWW();
		www.webRequest(langParser, OnStatusDoneStatic);
	}
	static RequestAllLanguages langParser = new RequestAllLanguages();
	static IEnumerator OnStatusDoneStatic(WebServiceReturnStatus status)
	{
		Debug.Log("GOT A THING:" + JsonWriter.Serialize(status));
		Debug.Log("SERIALIZED:" + JsonWriter.Serialize(langParser.Parse(status.text)));
		yield return null;
	}

	private RequestAllLanguages langGetter;
	// Use this for initialization
	void Start()
	{
		GameTimeWWW www = new GameTimeWWW();
		langGetter = new RequestAllLanguages();
		www.webRequest(new RequestAllLanguages(), OnStatusDone);
		TransfluentUtility.utility.getTranslation("HELLO WORLD");
		//Action<> <WebServiceReturnStatus>
		//www.webRequest(, OnStatusDone);
	}

	IEnumerator OnStatusDone(WebServiceReturnStatus status)
	{
		Debug.Log("GOT A THING:"+JsonWriter.Serialize(status));
		Debug.Log(JsonWriter.Serialize(langGetter.Parse(status.text))); ;
		yield return null;
	}

	// Update is called once per frame
	void Update()
	{

	}
}
