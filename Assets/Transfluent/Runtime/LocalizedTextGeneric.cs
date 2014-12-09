using UnityEngine;
using System;
using UnityEngine.UI;

//A generic abstract type that monobehviours can extend to easily add more specific functionality to be scanned.
//you can implement your own fully, but this should cover 99% of cases
//this is a placeholder until I find the right way to manage this.  perhaps this is a generic class that the others use?
//in the end we need *some* monobehaviour to be set and named fully
//but we want the type to change, and the only thing between them is the type of the text holder and the funciton to set text
public abstract class LocalizedTextGeneric<T> : MonoBehaviour
{
	public bool textIsManagedExternally;  //if someone else is managing this

	public T managedTextMonobhaviour; //Will get set in editor during scan phase

	public LocalizeUtil localizableText = new LocalizeUtil();

	protected abstract void SetText(string text); //hrm... something really wrong here

	//TODO: rethink/review before merge
	Action<String> getSetTextFunction()
	{
		if(managedTextMonobhaviour == null)
			return (string ignored)=>{};
		//there probably is a dictionary or switch type thing I can use here...
		//at least something with generics and the type at initalization
		//TODO: rethink/review before merge
		if(managedTextMonobhaviour is GUIText)
		{
			return (string text) => { (managedTextMonobhaviour as GUIText).text = text; };
		}
		if(managedTextMonobhaviour is Text)
		{
			return (string text) => { (managedTextMonobhaviour as Text).text = text; };
		}
		if(managedTextMonobhaviour is TextMesh)
		{
			return (string text) => { (managedTextMonobhaviour as TextMesh).text = text; };
		}
		return (string ignored) => { };
	}

	private Action<string> _setTextFunction; 
	void _setText(string text)
	{
		if(_setTextFunction == null) _setTextFunction = getSetTextFunction();
		SetText(text);
	}

	public void OnLocalize()
	{
		if(textIsManagedExternally) return;
		localizableText.OnLocalize();
		SetText(localizableText.current);
	}

	public void OnEnable()
	{
		OnLocalize();
	}

#if UNITY_EDITOR

	public void OnValidate()
	{
		SetText(localizableText.current);  //make sure to update the textmesh
	}

#endif

	public void Start()
	{
		SetText(localizableText.current);
	}
}