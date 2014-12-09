using UnityEngine;

public class LocalizedTextMesh : LocalizedTextGeneric<TextMesh>
{
	protected override void SetText(string text)
	{
		managedTextMonobhaviour.text = text;
	}
}