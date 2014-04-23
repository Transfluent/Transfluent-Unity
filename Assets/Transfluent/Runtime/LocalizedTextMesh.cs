using System;
using transfluent;
using UnityEngine;

public class LocalizedTextMesh : MonoBehaviour
{
	public bool textIsManagedExternally;  //if someone else is managing this

	public TextMesh textmesh; //gets set in editor

	public LocalizeUtil localizableText = new LocalizeUtil();

	public void OnLocalize()
	{
		if(textIsManagedExternally) return;
		localizableText.OnLocalize();
		textmesh.text = localizableText.current;
	}

	public void OnEnable()
	{
		OnLocalize();
	}
#if UNITY_EDITOR
	public void OnValidate()
	{
		textmesh.text = localizableText.current;  //make sure to update the textmesh
	}
#endif

	public void Start()
	{
		textmesh.text = localizableText.current;
	}
}

