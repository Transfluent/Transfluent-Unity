using System;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using transfluent;
using System.Collections;

namespace transfluent
{
	//not drawing language right now
	[CustomPropertyDrawer(typeof(TransfluentTranslation))]
	public class TransfluentTranslationDrawer : PropertyDrawer
	{
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return base.GetPropertyHeight(property, label) + 500;
		}

		void printThing(SerializedProperty prop)
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

		void printThing(SerializedProperty prop, string name)
		{
			prop.FindPropertyRelative(name);
			printThing(prop.FindPropertyRelative(name));
			ypos += 40;
		}

		private Rect currentRect
		{
			get
			{
				var rect = new Rect(originalRect);
				rect.y += ypos;
				return rect;
			}
		}

		private float ypos;
		private Rect originalRect;

		public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
		{

			if(prop == null)
				return;
			originalRect = pos;
			ypos = 0;

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
				Rect drawArea = new Rect(pos.x, pos.y, pos.width - 50, pos.height);
				var textRect = currentRect;
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
