using System;
using System.Net.Configuration;
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
			EditorGUI.LabelField(currentRect, prop.name,EditorStyles.boldLabel); //+ " type:" + prop.propertyType);
			ypos += 20;
			EditorGUI.LabelField(currentRect, getStringValue(prop));
		}

		private void printThing(SerializedProperty prop, string name)
		{
			prop.FindPropertyRelative(name);
			printThing(prop.FindPropertyRelative(name));
			ypos += 40;
		}
		
		void printEditableField(SerializedProperty prop)
		{
			if(prop == null)
			{
				EditorGUI.LabelField(currentRect, " NULL FIELD");
				return;
			}
			EditorGUI.LabelField(currentRect, "Editable field: " + prop.name, EditorStyles.boldLabel);
			ypos += 20;
			Rect singleHighRect = currentRect;
			singleHighRect.height = 20;
			string newValue = EditorGUI.TextField(singleHighRect, getStringValue(prop));
			try
			{
				switch (prop.propertyType)
				{
					case SerializedPropertyType.Integer:
						prop.intValue = int.Parse(newValue);
						break;
					case SerializedPropertyType.Boolean:
						prop.boolValue = bool.Parse(newValue);
						break;
					case SerializedPropertyType.Float:
						prop.floatValue = float.Parse(newValue);
						break;
					case SerializedPropertyType.String:
						prop.stringValue = newValue;
						break;
				}
			}
			catch
			{
				//parsing errors should be ignored
			}
			
		}

		public string getStringValue(SerializedProperty prop)
		{
			switch(prop.propertyType)
			{
				case SerializedPropertyType.Integer:
					return prop.intValue.ToString();
				case SerializedPropertyType.Boolean:
					return prop.boolValue.ToString();
				case SerializedPropertyType.Float:
					return prop.floatValue.ToString();
				case SerializedPropertyType.String:
					return prop.stringValue;
			}
			throw new Exception("unhandled prop type" + prop.propertyType);
		}



		public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
		{
			if(prop == null)
				return;
			originalRect = pos;
			ypos = 0;

			EditorGUI.LabelField(currentRect, prop.name);
			ypos += 20;
			
			//printThing();
			printThing(prop, "text_id");
			printThing(prop, "group_id");
			//printThing(prop, "language");  //TODO: show as an enum popup?
			printEditableField(prop.FindPropertyRelative("text"));

			//EditorGUI.BeginChangeCheck();

			//reflection over members?
			/*
			 * 
			 * SerializedProperty textID = prop.FindPropertyRelative("text_id");
			SerializedProperty groupID = prop.FindPropertyRelative("group_id");
			SerializedProperty language = prop.FindPropertyRelative("language");
			SerializedProperty text = prop.FindPropertyRelative("text");
			if(textID != null)
			{
				ypos += 40;
				var drawArea = new Rect(pos.x, pos.y, pos.width - 50, pos.height);
				Rect textRect = currentRect;
				textRect.height = base.GetPropertyHeight(prop, label);
				ypos += textRect.height;
				EditorGUI.LabelField(textRect, "text id", textID.stringValue);
			}*/
			/*
				if (groupID != null)
				{
					Rect drawArea2 = new Rect(pos.x, pos.y, pos.width - 50, pos.height);
					EditorGUI.LabelField(drawArea2, "group id", groupID.stringValue);
				}
				 */

			//EditorGUI.EndProperty();
		}
	}
}
