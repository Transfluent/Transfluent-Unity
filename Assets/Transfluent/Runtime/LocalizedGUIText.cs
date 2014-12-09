using UnityEngine;

public class LocalizedGUIText : LocalizedTextGeneric<GUIText>
{
	protected override void SetText(string text)
	{
		managedTextMonobhaviour.text = text;
	}
}