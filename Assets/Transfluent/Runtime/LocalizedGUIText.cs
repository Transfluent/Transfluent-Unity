using UnityEngine;

public class LocalizedGUIText : MonoBehaviour
{
	public bool textIsManagedExternally;  //if someone else is managing this

	public GUIText guiTextToModify; //gets set in editor, mostly through editor scripts

	public LocalizeUtil localizableText = new LocalizeUtil();

	public void OnLocalize()
	{
		if(textIsManagedExternally) return;
		localizableText.OnLocalize();
		guiTextToModify.text = localizableText.current;
	}

	public void OnEnable()
	{
		OnLocalize();
	}

#if UNITY_EDITOR

	public void OnValidate()
	{
		guiTextToModify.text = localizableText.current;  //make sure to update the textmesh
	}

#endif

	public void Start()
	{
		guiTextToModify.text = localizableText.current;
	}
}