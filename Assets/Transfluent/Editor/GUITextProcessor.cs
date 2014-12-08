using UnityEngine;
using UnityEditor;

namespace transfluent
{
	public class GUITextProcessor : IGameProcessor
	{
		public void process(GameObject go, CustomScriptProcessorState processorState)
		{
			var guiText = go.GetComponent<GUIText>();
			if(guiText == null) return;

			string newKey = guiText.text;
			processorState.addToDB(newKey, newKey);
			processorState.addToBlacklist(go);

			var translatable = guiText.GetComponent<LocalizedGUIText>();
			if(processorState.shouldIgnoreString(guiText.text))
			{
				processorState.addToBlacklist(go);
				return;
			}

			if(translatable == null)
			{
				translatable = guiText.gameObject.AddComponent<LocalizedGUIText>();
				translatable.guiTextToModify = guiText; //just use whatever the source text is upfront, and allow the user to
			}

			translatable.localizableText.globalizationKey = guiText.text;
			//For guitext and other unity managed objects, this setDirty is not needed according to http://docs.unity3d.com/Documentation/ScriptReference/EditorUtility.SetDirty.html
			EditorUtility.SetDirty(guiText.gameObject);
			EditorUtility.SetDirty(guiText);
		}
	}
}