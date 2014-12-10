#if UNITY_4_6 || UNITY_5
using UnityEngine.UI;

public class LocalizedText : LocalizedTextGeneric<Text>
{
	protected override void SetText(string text)
	{
		managedTextMonobhaviour.text = text;
	}
}
#endif // UNITY_4_6 || UNITY_5