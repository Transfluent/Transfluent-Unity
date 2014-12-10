using UnityEngine;

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

	void _setText(string text)
	{
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