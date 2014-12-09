using transfluent;
using UnityEngine;
using UnityEditor;

namespace transfluent
{
	public class TextMeshProcessor : IGameProcessor
	{
		public void process(GameObject go, CustomScriptProcessorState processorState)
		{
			var textMesh = go.GetComponent<TextMesh>();
			if(textMesh == null) return;

			string newKey = textMesh.text;
			processorState.addToDB(newKey, newKey);
			processorState.addToBlacklist(go);

			var translatable = textMesh.GetComponent<LocalizedTextMesh>();
			if(processorState.shouldIgnoreString(textMesh.text))
			{
				processorState.addToBlacklist(go);
				return;
			}

			if(translatable == null)
			{
				translatable = textMesh.gameObject.AddComponent<LocalizedTextMesh>();
				translatable.managedTextMonobhaviour = textMesh; //just use whatever the source text is upfront, and allow the user to
			}

			translatable.localizableText.globalizationKey = textMesh.text;
			//For textmesh specificially, this setDirty is not needed according to http://docs.unity3d.com/Documentation/ScriptReference/EditorUtility.SetDirty.html
			//EditorUtility.SetDirty(textMesh);
		}
	}
}
