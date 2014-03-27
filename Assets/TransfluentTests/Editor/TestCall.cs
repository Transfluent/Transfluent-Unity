using Pathfinding.Serialization.JsonFx;
using System.Collections;
using transfluent;
using UnityEngine;

public class TestCall : MonoBehaviour
{
	private static readonly RequestAllLanguages langParser = new RequestAllLanguages();

	private RequestAllLanguages langGetter;

	private static IEnumerator OnStatusDoneStatic(WebServiceReturnStatus status)
	{
		Debug.Log("GOT A THING:" + JsonWriter.Serialize(status));
		Debug.Log("SERIALIZED:" + JsonWriter.Serialize(langParser.Parse(status.text)));
		yield return null;
	}

	// Use this for initialization
	private void Start()
	{
		var www = new GameTimeWWW();
		langGetter = new RequestAllLanguages();
		www.webRequest(new RequestAllLanguages(), OnStatusDone);
		TransfluentUtility.get("HELLO WORLD");
		//Action<> <WebServiceReturnStatus>
		//www.webRequest(, OnStatusDone);
	}

	private IEnumerator OnStatusDone(WebServiceReturnStatus status)
	{
		Debug.Log("GOT A THING:" + JsonWriter.Serialize(status));
		Debug.Log(JsonWriter.Serialize(langGetter.Parse(status.text)));

		yield return null;
	}

	// Update is called once per frame
	private void Update()
	{
	}
}