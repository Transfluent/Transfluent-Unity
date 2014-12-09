using UnityEngine.UI;

public class LocalizedText : LocalizedTextGeneric<Text>
{
	protected override void SetText(string text)
	{
		managedTextMonobhaviour.text = text;
	}
}
