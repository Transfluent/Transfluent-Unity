#if UNITY_4_6 || UNITY_5
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

namespace transfluent
{
	public class TextProcessor : IGameProcessor
	{
		public void process(GameObject go, CustomScriptProcessorState processorState)
		{
			var guiText = go.GetComponent<Text>();
			if(guiText == null) return;

			string newKey = guiText.text;
			processorState.addToDB(newKey, newKey);
			processorState.addToBlacklist(go);

			var translatable = guiText.GetComponent<LocalizedText>();
			if(processorState.shouldIgnoreString(guiText.text))
			{
				processorState.addToBlacklist(go);
				return;
			}

			if(translatable == null)
			{
				translatable = guiText.gameObject.AddComponent<LocalizedText>();
				translatable.managedTextMonobhaviour = guiText;
			}

			translatable.localizableText.globalizationKey = guiText.text;
			//For guitext and other unity managed objects, this setDirty is not needed according to http://docs.unity3d.com/Documentation/ScriptReference/EditorUtility.SetDirty.html
			EditorUtility.SetDirty(guiText.gameObject);
			EditorUtility.SetDirty(guiText);
		}
	}
}
#endif // UNITY_4_6 || UNITY_5