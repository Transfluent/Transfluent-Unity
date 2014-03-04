using transfluent;
using UnityEditor;
using UnityEngine;

//NOTE:not drawing language right now

namespace transfluent
{
	[CustomPropertyDrawer(typeof(TransfluentTranslation))]
	public class TransfluentTranslationDrawer : PropertyDrawer
	{
		private Rect originalRect;
		private float ypos;

		private Rect currentRect
		{
			get
			{
				var rect = new Rect(originalRect);
				rect.y += ypos;
				return rect;
			}
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return base.GetPropertyHeight(property, label) + 200;
		}

		private void printThing(SerializedProperty prop)
		{
			if(prop == null)
			{
				EditorGUI.LabelField(currentRect, " NULL FIELD");
				return;
			}
			EditorGUI.LabelField(currentRect, prop.name + " type:" + prop.propertyType);
			ypos += 20;
			switch(prop.propertyType)
			{
				case SerializedPropertyType.Integer:
					EditorGUI.LabelField(currentRect, prop.intValue.ToString());
					break;
				case SerializedPropertyType.Boolean:
					EditorGUI.LabelField(currentRect, prop.boolValue.ToString());
					break;
				case SerializedPropertyType.Float:
					EditorGUI.LabelField(currentRect, prop.floatValue.ToString());
					break;
				case SerializedPropertyType.String:
					EditorGUI.LabelField(currentRect, prop.stringValue);
					break;
			}
		}

		private void printThing(SerializedProperty prop, string name)
		{
			prop.FindPropertyRelative(name);
			printThing(prop.FindPropertyRelative(name));
			ypos += 40;
		}

		public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
		{
			if(prop == null)
				return;
			originalRect = pos;
			ypos = 0;

			EditorGUI.LabelField(currentRect, prop.name);
			ypos += 20;


			EditorGUI.BeginProperty(pos, label, prop);

			//EditorGUILayout.LabelField("translation field");
			SerializedProperty textID = prop.FindPropertyRelative("text_id");
			SerializedProperty groupID = prop.FindPropertyRelative("group_id");
			SerializedProperty language = prop.FindPropertyRelative("language");
			SerializedProperty text = prop.FindPropertyRelative("text");

			/*printThing(prop, "text_id");
				printThing(prop, "group_id");
				printThing(prop, "language");
				printThing(prop, "text");*/

			//EditorGUI.BeginChangeCheck();

			//reflection over members?
			if(textID != null)
			{
				ypos += 40;
				var drawArea = new Rect(pos.x, pos.y, pos.width - 50, pos.height);
				Rect textRect = currentRect;
				textRect.height = base.GetPropertyHeight(prop, label);
				ypos += textRect.height;
				textID.stringValue = EditorGUI.TextField(textRect, "text id", textID.stringValue);
			}
			/*
				if (groupID != null)
				{
					Rect drawArea2 = new Rect(pos.x, pos.y, pos.width - 50, pos.height);
					EditorGUI.LabelField(drawArea2, "group id", groupID.stringValue);
				}
				 */

			EditorGUI.EndProperty();
		}
	}
}
