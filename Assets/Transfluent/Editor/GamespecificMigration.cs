#if TRANSFLUENT_EXAMPLE
using strange.examples.strangerocks;
#endif //!TRANSFLUENT_EXAMPLE
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif	

namespace transfluent
{
	public class GameSpecificMigration : MonoBehaviour
	{
		//ignore all textmeshes referenced by all ButtonView components
		public static void toExplicitlyIgnore(List<TextMesh> toIgnore,GameObject inPrefab = null)
		{
#if TRANSFLUENT_EXAMPLE
			//or maybe just find this class with reflection?
			//custom references -- replace with a reflection based solution maybe?
			//   find gameobjects with [SerializeField] private or public vars and also define an OnLocalize

			var allButtons = new List<ButtonView>();
			if (inPrefab == null)
			{
				allButtons.AddRange(FindObjectsOfType<ButtonView>());
			}
			else
			{
				allButtons.AddRange(inPrefab.GetComponentsInChildren<ButtonView>(true));
			}
			allButtons.ForEach((ButtonView button) =>
			{
				if (button != null && button.labelMesh != null)
				{
					toIgnore.Add(button.labelMesh);
					string newKey = button.label;
					button.labelData.globalizationKey = newKey;

					FindTextMeshReferences.setKeyInDefaultLanguageDB(newKey, newKey);

					//TODO: ensure that this is set to the source language of the game config before adding
					EditorUtility.SetDirty(button);
				}
			});
#endif //!TRANSFLUENT_EXAMPLE
		}
	}
}
