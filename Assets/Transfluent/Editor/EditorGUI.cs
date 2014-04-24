using System;

//wrapper around unity's gui, except to grab text as quickly as possbile and spit it into an internal db
//http://docs.unity3d.com/Documentation/ScriptReference/GUI.html
namespace transfluent.guiwrapper
{
#pragma warning disable 618

	public partial class EditorGUI
	{
		public static System.Boolean showMixedValue
		{
			get { return UnityEditor.EditorGUI.showMixedValue; }
			set { UnityEditor.EditorGUI.showMixedValue = value; }
		}

		public static System.Boolean actionKey
		{
			get { return UnityEditor.EditorGUI.actionKey; }
		}

		public static System.Int32 indentLevel
		{
			get { return UnityEditor.EditorGUI.indentLevel; }
			set { UnityEditor.EditorGUI.indentLevel = value; }
		}

		public static void DrawRect(UnityEngine.Rect rect, UnityEngine.Color color)
		{
			UnityEditor.EditorGUI.DrawRect(rect, color);
		}

		public static void FocusTextInControl(System.String name)
		{
			UnityEditor.EditorGUI.FocusTextInControl(name);
		}

		public static void BeginDisabledGroup(bool disabled)
		{
			UnityEditor.EditorGUI.BeginDisabledGroup(disabled);
		}

		public static void EndDisabledGroup()
		{
			UnityEditor.EditorGUI.EndDisabledGroup();
		}

		public static void BeginChangeCheck()
		{
			UnityEditor.EditorGUI.BeginChangeCheck();
		}

		public static bool EndChangeCheck()
		{
			return UnityEditor.EditorGUI.EndChangeCheck();
		}

		public static void DropShadowLabel(UnityEngine.Rect position, System.String text)
		{
			UnityEditor.EditorGUI.DropShadowLabel(position, TranslationUtility.get(text));
		}

		public static void DropShadowLabel(UnityEngine.Rect position, UnityEngine.GUIContent content)
		{
			UnityEditor.EditorGUI.DropShadowLabel(position, content);
		}

		public static void DropShadowLabel(UnityEngine.Rect position, System.String text, UnityEngine.GUIStyle style)
		{
			UnityEditor.EditorGUI.DropShadowLabel(position, TranslationUtility.get(text), style);
		}

		public static void DropShadowLabel(UnityEngine.Rect position, UnityEngine.GUIContent content, UnityEngine.GUIStyle style)
		{
			UnityEditor.EditorGUI.DropShadowLabel(position, content, style);
		}

		public static void LabelField(UnityEngine.Rect position, System.String label)
		{
			UnityEditor.EditorGUI.LabelField(position, TranslationUtility.get(label));
		}

		public static void LabelField(UnityEngine.Rect position, System.String label, UnityEngine.GUIStyle style)
		{
			UnityEditor.EditorGUI.LabelField(position, TranslationUtility.get(label), style);
		}

		public static void LabelField(UnityEngine.Rect position, UnityEngine.GUIContent label)
		{
			UnityEditor.EditorGUI.LabelField(position, label);
		}

		public static void LabelField(UnityEngine.Rect position, UnityEngine.GUIContent label, UnityEngine.GUIStyle style)
		{
			UnityEditor.EditorGUI.LabelField(position, label, style);
		}

		public static void LabelField(UnityEngine.Rect position, System.String label, System.String label2)
		{
			UnityEditor.EditorGUI.LabelField(position, TranslationUtility.get(label), TranslationUtility.get(label2));
		}

		public static void LabelField(UnityEngine.Rect position, System.String label, System.String label2, UnityEngine.GUIStyle style)
		{
			UnityEditor.EditorGUI.LabelField(position, TranslationUtility.get(label), TranslationUtility.get(label), style);
		}

		public static void LabelField(UnityEngine.Rect position, UnityEngine.GUIContent label, UnityEngine.GUIContent label2)
		{
			UnityEditor.EditorGUI.LabelField(position, label, label2);
		}

		public static void LabelField(UnityEngine.Rect position, UnityEngine.GUIContent label, UnityEngine.GUIContent label2, UnityEngine.GUIStyle style)
		{
			UnityEditor.EditorGUI.LabelField(position, label, label2, style);
		}

		public static bool Toggle(UnityEngine.Rect position, bool value)
		{
			return UnityEditor.EditorGUI.Toggle(position, value);
		}

		public static bool Toggle(UnityEngine.Rect position, System.String label, bool value)
		{
			return UnityEditor.EditorGUI.Toggle(position, TranslationUtility.get(label), value);
		}

		public static bool Toggle(UnityEngine.Rect position, bool value, UnityEngine.GUIStyle style)
		{
			return UnityEditor.EditorGUI.Toggle(position, value, style);
		}

		public static bool Toggle(UnityEngine.Rect position, System.String label, bool value, UnityEngine.GUIStyle style)
		{
			return UnityEditor.EditorGUI.Toggle(position, TranslationUtility.get(label), value, style);
		}

		public static bool Toggle(UnityEngine.Rect position, UnityEngine.GUIContent label, bool value)
		{
			return UnityEditor.EditorGUI.Toggle(position, label, value);
		}

		public static bool Toggle(UnityEngine.Rect position, UnityEngine.GUIContent label, bool value, UnityEngine.GUIStyle style)
		{
			return UnityEditor.EditorGUI.Toggle(position, label, value, style);
		}

		public static bool ToggleLeft(UnityEngine.Rect position, System.String label, bool value)
		{
			return UnityEditor.EditorGUI.ToggleLeft(position, TranslationUtility.get(label), value);
		}

		public static bool ToggleLeft(UnityEngine.Rect position, System.String label, bool value, UnityEngine.GUIStyle labelStyle)
		{
			return UnityEditor.EditorGUI.ToggleLeft(position, TranslationUtility.get(label), value, labelStyle);
		}

		public static bool ToggleLeft(UnityEngine.Rect position, UnityEngine.GUIContent label, bool value)
		{
			return UnityEditor.EditorGUI.ToggleLeft(position, label, value);
		}

		public static bool ToggleLeft(UnityEngine.Rect position, UnityEngine.GUIContent label, bool value, UnityEngine.GUIStyle labelStyle)
		{
			return UnityEditor.EditorGUI.ToggleLeft(position, label, value, labelStyle);
		}

		public static System.String TextField(UnityEngine.Rect position, System.String text)
		{
			return UnityEditor.EditorGUI.TextField(position, TranslationUtility.get(text));
		}

		public static System.String TextField(UnityEngine.Rect position, System.String text, UnityEngine.GUIStyle style)
		{
			return UnityEditor.EditorGUI.TextField(position, TranslationUtility.get(text), style);
		}

		public static System.String TextField(UnityEngine.Rect position, System.String label, System.String text)
		{
			return UnityEditor.EditorGUI.TextField(position, TranslationUtility.get(label), TranslationUtility.get(text));
		}

		public static System.String TextField(UnityEngine.Rect position, System.String label, System.String text, UnityEngine.GUIStyle style)
		{
			return UnityEditor.EditorGUI.TextField(position, TranslationUtility.get(label), TranslationUtility.get(text), style);
		}

		public static System.String TextField(UnityEngine.Rect position, UnityEngine.GUIContent label, System.String text)
		{
			return UnityEditor.EditorGUI.TextField(position, label, TranslationUtility.get(text));
		}

		public static System.String TextField(UnityEngine.Rect position, UnityEngine.GUIContent label, System.String text, UnityEngine.GUIStyle style)
		{
			return UnityEditor.EditorGUI.TextField(position, label, TranslationUtility.get(text), style);
		}

		public static System.String TextArea(UnityEngine.Rect position, System.String text)
		{
			return UnityEditor.EditorGUI.TextArea(position, TranslationUtility.get(text));
		}

		public static System.String TextArea(UnityEngine.Rect position, System.String text, UnityEngine.GUIStyle style)
		{
			return UnityEditor.EditorGUI.TextArea(position, TranslationUtility.get(text), style);
		}

		public static void SelectableLabel(UnityEngine.Rect position, System.String text)
		{
			UnityEditor.EditorGUI.SelectableLabel(position, TranslationUtility.get(text));
		}

		public static void SelectableLabel(UnityEngine.Rect position, System.String text, UnityEngine.GUIStyle style)
		{
			UnityEditor.EditorGUI.SelectableLabel(position, TranslationUtility.get(text), style);
		}

		public static System.String DoPasswordField(int id, UnityEngine.Rect position, System.String password, UnityEngine.GUIStyle style)
		{
			return UnityEditor.EditorGUI.DoPasswordField(id, position, password, style);
		}

		public static System.String DoPasswordField(int id, UnityEngine.Rect position, UnityEngine.GUIContent label, System.String password, UnityEngine.GUIStyle style)
		{
			return UnityEditor.EditorGUI.DoPasswordField(id, position, label, password, style);
		}

		public static System.String PasswordField(UnityEngine.Rect position, System.String password)
		{
			return UnityEditor.EditorGUI.PasswordField(position, password);
		}

		public static System.String PasswordField(UnityEngine.Rect position, System.String password, UnityEngine.GUIStyle style)
		{
			return UnityEditor.EditorGUI.PasswordField(position, password, style);
		}

		public static System.String PasswordField(UnityEngine.Rect position, System.String label, System.String password)
		{
			return UnityEditor.EditorGUI.PasswordField(position, TranslationUtility.get(label), password);
		}

		public static System.String PasswordField(UnityEngine.Rect position, System.String label, System.String password, UnityEngine.GUIStyle style)
		{
			return UnityEditor.EditorGUI.PasswordField(position, TranslationUtility.get(label), password, style);
		}

		public static System.String PasswordField(UnityEngine.Rect position, UnityEngine.GUIContent label, System.String password)
		{
			return UnityEditor.EditorGUI.PasswordField(position, label, password);
		}

		public static System.String PasswordField(UnityEngine.Rect position, UnityEngine.GUIContent label, System.String password, UnityEngine.GUIStyle style)
		{
			return UnityEditor.EditorGUI.PasswordField(position, label, password, style);
		}

		public static float FloatField(UnityEngine.Rect position, float value)
		{
			return UnityEditor.EditorGUI.FloatField(position, value);
		}

		public static float FloatField(UnityEngine.Rect position, float value, UnityEngine.GUIStyle style)
		{
			return UnityEditor.EditorGUI.FloatField(position, value, style);
		}

		public static float FloatField(UnityEngine.Rect position, System.String label, float value)
		{
			return UnityEditor.EditorGUI.FloatField(position, TranslationUtility.get(label), value);
		}

		public static float FloatField(UnityEngine.Rect position, System.String label, float value, UnityEngine.GUIStyle style)
		{
			return UnityEditor.EditorGUI.FloatField(position, TranslationUtility.get(label), value, style);
		}

		public static float FloatField(UnityEngine.Rect position, UnityEngine.GUIContent label, float value)
		{
			return UnityEditor.EditorGUI.FloatField(position, label, value);
		}

		public static float FloatField(UnityEngine.Rect position, UnityEngine.GUIContent label, float value, UnityEngine.GUIStyle style)
		{
			return UnityEditor.EditorGUI.FloatField(position, label, value, style);
		}

		public static int IntField(UnityEngine.Rect position, int value)
		{
			return UnityEditor.EditorGUI.IntField(position, value);
		}

		public static int IntField(UnityEngine.Rect position, int value, UnityEngine.GUIStyle style)
		{
			return UnityEditor.EditorGUI.IntField(position, value, style);
		}

		public static int IntField(UnityEngine.Rect position, System.String label, int value)
		{
			return UnityEditor.EditorGUI.IntField(position, TranslationUtility.get(label), value);
		}

		public static int IntField(UnityEngine.Rect position, System.String label, int value, UnityEngine.GUIStyle style)
		{
			return UnityEditor.EditorGUI.IntField(position, TranslationUtility.get(label), value, style);
		}

		public static int IntField(UnityEngine.Rect position, UnityEngine.GUIContent label, int value)
		{
			return UnityEditor.EditorGUI.IntField(position, label, value);
		}

		public static int IntField(UnityEngine.Rect position, UnityEngine.GUIContent label, int value, UnityEngine.GUIStyle style)
		{
			return UnityEditor.EditorGUI.IntField(position, label, value, style);
		}

		public static float Slider(UnityEngine.Rect position, float value, float leftValue, float rightValue)
		{
			return UnityEditor.EditorGUI.Slider(position, value, leftValue, rightValue);
		}

		public static float Slider(UnityEngine.Rect position, System.String label, float value, float leftValue, float rightValue)
		{
			return UnityEditor.EditorGUI.Slider(position, TranslationUtility.get(label), value, leftValue, rightValue);
		}

		public static float Slider(UnityEngine.Rect position, UnityEngine.GUIContent label, float value, float leftValue, float rightValue)
		{
			return UnityEditor.EditorGUI.Slider(position, label, value, leftValue, rightValue);
		}

		public static void Slider(UnityEngine.Rect position, UnityEditor.SerializedProperty property, float leftValue, float rightValue)
		{
			UnityEditor.EditorGUI.Slider(position, property, leftValue, rightValue);
		}

		public static void Slider(UnityEngine.Rect position, UnityEditor.SerializedProperty property, float leftValue, float rightValue, System.String label)
		{
			UnityEditor.EditorGUI.Slider(position, property, leftValue, rightValue, TranslationUtility.get(label));
		}

		public static void Slider(UnityEngine.Rect position, UnityEditor.SerializedProperty property, float leftValue, float rightValue, UnityEngine.GUIContent label)
		{
			UnityEditor.EditorGUI.Slider(position, property, leftValue, rightValue, label);
		}

		public static int IntSlider(UnityEngine.Rect position, int value, int leftValue, int rightValue)
		{
			return UnityEditor.EditorGUI.IntSlider(position, value, leftValue, rightValue);
		}

		public static int IntSlider(UnityEngine.Rect position, System.String label, int value, int leftValue, int rightValue)
		{
			return UnityEditor.EditorGUI.IntSlider(position, TranslationUtility.get(label), value, leftValue, rightValue);
		}

		public static int IntSlider(UnityEngine.Rect position, UnityEngine.GUIContent label, int value, int leftValue, int rightValue)
		{
			return UnityEditor.EditorGUI.IntSlider(position, label, value, leftValue, rightValue);
		}

		public static void IntSlider(UnityEngine.Rect position, UnityEditor.SerializedProperty property, int leftValue, int rightValue)
		{
			UnityEditor.EditorGUI.IntSlider(position, property, leftValue, rightValue);
		}

		public static void IntSlider(UnityEngine.Rect position, UnityEditor.SerializedProperty property, int leftValue, int rightValue, System.String label)
		{
			UnityEditor.EditorGUI.IntSlider(position, property, leftValue, rightValue, TranslationUtility.get(label));
		}

		public static void IntSlider(UnityEngine.Rect position, UnityEditor.SerializedProperty property, int leftValue, int rightValue, UnityEngine.GUIContent label)
		{
			UnityEditor.EditorGUI.IntSlider(position, property, leftValue, rightValue, label);
		}

		public static void MinMaxSlider(UnityEngine.GUIContent label, UnityEngine.Rect position, ref float minValue, ref float maxValue, float minLimit, float maxLimit)
		{
			UnityEditor.EditorGUI.MinMaxSlider(label, position, ref minValue, ref maxValue, minLimit, maxLimit);
		}

		public static void MinMaxSlider(UnityEngine.Rect position, ref float minValue, ref float maxValue, float minLimit, float maxLimit)
		{
			UnityEditor.EditorGUI.MinMaxSlider(position, ref minValue, ref maxValue, minLimit, maxLimit);
		}

		public static int Popup(UnityEngine.Rect position, int selectedIndex, System.String[] displayedOptions)
		{
			return UnityEditor.EditorGUI.Popup(position, selectedIndex, TranslationUtility.get(displayedOptions));
		}

		public static int Popup(UnityEngine.Rect position, int selectedIndex, System.String[] displayedOptions, UnityEngine.GUIStyle style)
		{
			return UnityEditor.EditorGUI.Popup(position, selectedIndex, TranslationUtility.get(displayedOptions), style);
		}

		public static int Popup(UnityEngine.Rect position, int selectedIndex, UnityEngine.GUIContent[] displayedOptions)
		{
			return UnityEditor.EditorGUI.Popup(position, selectedIndex, displayedOptions);
		}

		public static int Popup(UnityEngine.Rect position, int selectedIndex, UnityEngine.GUIContent[] displayedOptions, UnityEngine.GUIStyle style)
		{
			return UnityEditor.EditorGUI.Popup(position, selectedIndex, displayedOptions, style);
		}

		public static int Popup(UnityEngine.Rect position, System.String label, int selectedIndex, System.String[] displayedOptions)
		{
			return UnityEditor.EditorGUI.Popup(position, TranslationUtility.get(label), selectedIndex, TranslationUtility.get(displayedOptions));
		}

		public static int Popup(UnityEngine.Rect position, System.String label, int selectedIndex, System.String[] displayedOptions, UnityEngine.GUIStyle style)
		{
			return UnityEditor.EditorGUI.Popup(position, TranslationUtility.get(label), selectedIndex, TranslationUtility.get(displayedOptions), style);
		}

		public static int Popup(UnityEngine.Rect position, UnityEngine.GUIContent label, int selectedIndex, UnityEngine.GUIContent[] displayedOptions)
		{
			return UnityEditor.EditorGUI.Popup(position, label, selectedIndex, displayedOptions);
		}

		public static int Popup(UnityEngine.Rect position, UnityEngine.GUIContent label, int selectedIndex, UnityEngine.GUIContent[] displayedOptions, UnityEngine.GUIStyle style)
		{
			return UnityEditor.EditorGUI.Popup(position, label, selectedIndex, displayedOptions, style);
		}

		public static System.Enum EnumPopup(UnityEngine.Rect position, System.Enum selected)
		{
			return UnityEditor.EditorGUI.EnumPopup(position, selected);
		}

		public static System.Enum EnumPopup(UnityEngine.Rect position, System.Enum selected, UnityEngine.GUIStyle style)
		{
			return UnityEditor.EditorGUI.EnumPopup(position, selected, style);
		}

		public static System.Enum EnumPopup(UnityEngine.Rect position, System.String label, System.Enum selected)
		{
			return UnityEditor.EditorGUI.EnumPopup(position, TranslationUtility.get(label), selected);
		}

		public static System.Enum EnumPopup(UnityEngine.Rect position, System.String label, System.Enum selected, UnityEngine.GUIStyle style)
		{
			return UnityEditor.EditorGUI.EnumPopup(position, TranslationUtility.get(label), selected, style);
		}

		public static System.Enum EnumPopup(UnityEngine.Rect position, UnityEngine.GUIContent label, System.Enum selected)
		{
			return UnityEditor.EditorGUI.EnumPopup(position, label, selected);
		}

		public static System.Enum EnumPopup(UnityEngine.Rect position, UnityEngine.GUIContent label, System.Enum selected, UnityEngine.GUIStyle style)
		{
			return UnityEditor.EditorGUI.EnumPopup(position, label, selected, style);
		}

		public static int IntPopup(UnityEngine.Rect position, int selectedValue, System.String[] displayedOptions, System.Int32[] optionValues)
		{
			return UnityEditor.EditorGUI.IntPopup(position, selectedValue, TranslationUtility.get(displayedOptions), optionValues);
		}

		public static int IntPopup(UnityEngine.Rect position, int selectedValue, System.String[] displayedOptions, System.Int32[] optionValues, UnityEngine.GUIStyle style)
		{
			return UnityEditor.EditorGUI.IntPopup(position, selectedValue, TranslationUtility.get(displayedOptions), optionValues, style);
		}

		public static int IntPopup(UnityEngine.Rect position, int selectedValue, UnityEngine.GUIContent[] displayedOptions, System.Int32[] optionValues)
		{
			return UnityEditor.EditorGUI.IntPopup(position, selectedValue, displayedOptions, optionValues);
		}

		public static int IntPopup(UnityEngine.Rect position, int selectedValue, UnityEngine.GUIContent[] displayedOptions, System.Int32[] optionValues, UnityEngine.GUIStyle style)
		{
			return UnityEditor.EditorGUI.IntPopup(position, selectedValue, displayedOptions, optionValues, style);
		}

		public static int IntPopup(UnityEngine.Rect position, UnityEngine.GUIContent label, int selectedValue, UnityEngine.GUIContent[] displayedOptions, System.Int32[] optionValues)
		{
			return UnityEditor.EditorGUI.IntPopup(position, label, selectedValue, displayedOptions, optionValues);
		}

		public static int IntPopup(UnityEngine.Rect position, UnityEngine.GUIContent label, int selectedValue, UnityEngine.GUIContent[] displayedOptions, System.Int32[] optionValues, UnityEngine.GUIStyle style)
		{
			return UnityEditor.EditorGUI.IntPopup(position, label, selectedValue, displayedOptions, optionValues, style);
		}

		public static void IntPopup(UnityEngine.Rect position, UnityEditor.SerializedProperty property, UnityEngine.GUIContent[] displayedOptions, System.Int32[] optionValues)
		{
			UnityEditor.EditorGUI.IntPopup(position, property, displayedOptions, optionValues);
		}

		public static void IntPopup(UnityEngine.Rect position, UnityEditor.SerializedProperty property, UnityEngine.GUIContent[] displayedOptions, System.Int32[] optionValues, UnityEngine.GUIContent label)
		{
			UnityEditor.EditorGUI.IntPopup(position, property, displayedOptions, optionValues, label);
		}

		public static int IntPopup(UnityEngine.Rect position, System.String label, int selectedValue, System.String[] displayedOptions, System.Int32[] optionValues)
		{
			return UnityEditor.EditorGUI.IntPopup(position, TranslationUtility.get(label), selectedValue, TranslationUtility.get(displayedOptions), optionValues);
		}

		public static int IntPopup(UnityEngine.Rect position, System.String label, int selectedValue, System.String[] displayedOptions, System.Int32[] optionValues, UnityEngine.GUIStyle style)
		{
			return UnityEditor.EditorGUI.IntPopup(position, TranslationUtility.get(label), selectedValue, TranslationUtility.get(displayedOptions), optionValues, style);
		}

		public static System.String TagField(UnityEngine.Rect position, System.String tag)
		{
			return UnityEditor.EditorGUI.TagField(position, tag);
		}

		public static System.String TagField(UnityEngine.Rect position, System.String tag, UnityEngine.GUIStyle style)
		{
			return UnityEditor.EditorGUI.TagField(position, tag, style);
		}

		public static System.String TagField(UnityEngine.Rect position, System.String label, System.String tag)
		{
			return UnityEditor.EditorGUI.TagField(position, TranslationUtility.get(label), tag);
		}

		public static System.String TagField(UnityEngine.Rect position, System.String label, System.String tag, UnityEngine.GUIStyle style)
		{
			return UnityEditor.EditorGUI.TagField(position, TranslationUtility.get(label), tag, style);
		}

		public static System.String TagField(UnityEngine.Rect position, UnityEngine.GUIContent label, System.String tag)
		{
			return UnityEditor.EditorGUI.TagField(position, label, tag);
		}

		public static System.String TagField(UnityEngine.Rect position, UnityEngine.GUIContent label, System.String tag, UnityEngine.GUIStyle style)
		{
			return UnityEditor.EditorGUI.TagField(position, label, tag, style);
		}

		public static int LayerField(UnityEngine.Rect position, int layer)
		{
			return UnityEditor.EditorGUI.LayerField(position, layer);
		}

		public static int LayerField(UnityEngine.Rect position, int layer, UnityEngine.GUIStyle style)
		{
			return UnityEditor.EditorGUI.LayerField(position, layer, style);
		}

		public static int LayerField(UnityEngine.Rect position, System.String label, int layer)
		{
			return UnityEditor.EditorGUI.LayerField(position, TranslationUtility.get(label), layer);
		}

		public static int LayerField(UnityEngine.Rect position, System.String label, int layer, UnityEngine.GUIStyle style)
		{
			return UnityEditor.EditorGUI.LayerField(position, TranslationUtility.get(label), layer, style);
		}

		public static int LayerField(UnityEngine.Rect position, UnityEngine.GUIContent label, int layer)
		{
			return UnityEditor.EditorGUI.LayerField(position, label, layer);
		}

		public static int LayerField(UnityEngine.Rect position, UnityEngine.GUIContent label, int layer, UnityEngine.GUIStyle style)
		{
			return UnityEditor.EditorGUI.LayerField(position, label, layer, style);
		}

		public static int MaskField(UnityEngine.Rect position, UnityEngine.GUIContent label, int mask, System.String[] displayedOptions)
		{
			return UnityEditor.EditorGUI.MaskField(position, label, mask, TranslationUtility.get(displayedOptions));
		}

		public static int MaskField(UnityEngine.Rect position, UnityEngine.GUIContent label, int mask, System.String[] displayedOptions, UnityEngine.GUIStyle style)
		{
			return UnityEditor.EditorGUI.MaskField(position, label, mask, TranslationUtility.get(displayedOptions), style);
		}

		public static int MaskField(UnityEngine.Rect position, System.String label, int mask, System.String[] displayedOptions)
		{
			return UnityEditor.EditorGUI.MaskField(position, TranslationUtility.get(label), mask, TranslationUtility.get(displayedOptions));
		}

		public static int MaskField(UnityEngine.Rect position, System.String label, int mask, System.String[] displayedOptions, UnityEngine.GUIStyle style)
		{
			return UnityEditor.EditorGUI.MaskField(position, TranslationUtility.get(label), mask, TranslationUtility.get(displayedOptions), style);
		}

		public static int MaskField(UnityEngine.Rect position, int mask, System.String[] displayedOptions)
		{
			return UnityEditor.EditorGUI.MaskField(position, mask, TranslationUtility.get(displayedOptions));
		}

		public static int MaskField(UnityEngine.Rect position, int mask, System.String[] displayedOptions, UnityEngine.GUIStyle style)
		{
			return UnityEditor.EditorGUI.MaskField(position, mask, TranslationUtility.get(displayedOptions), style);
		}

		public static System.Enum EnumMaskField(UnityEngine.Rect position, UnityEngine.GUIContent label, System.Enum enumValue)
		{
			return UnityEditor.EditorGUI.EnumMaskField(position, label, enumValue);
		}

		public static System.Enum EnumMaskField(UnityEngine.Rect position, UnityEngine.GUIContent label, System.Enum enumValue, UnityEngine.GUIStyle style)
		{
			return UnityEditor.EditorGUI.EnumMaskField(position, label, enumValue, style);
		}

		public static System.Enum EnumMaskField(UnityEngine.Rect position, System.String label, System.Enum enumValue)
		{
			return UnityEditor.EditorGUI.EnumMaskField(position, TranslationUtility.get(label), enumValue);
		}

		public static System.Enum EnumMaskField(UnityEngine.Rect position, System.String label, System.Enum enumValue, UnityEngine.GUIStyle style)
		{
			return UnityEditor.EditorGUI.EnumMaskField(position, TranslationUtility.get(label), enumValue, style);
		}

		public static System.Enum EnumMaskField(UnityEngine.Rect position, System.Enum enumValue)
		{
			return UnityEditor.EditorGUI.EnumMaskField(position, enumValue);
		}

		public static System.Enum EnumMaskField(UnityEngine.Rect position, System.Enum enumValue, UnityEngine.GUIStyle style)
		{
			return UnityEditor.EditorGUI.EnumMaskField(position, enumValue, style);
		}

		public static UnityEngine.Object ObjectField(UnityEngine.Rect position, UnityEngine.Object obj, System.Type objType, bool allowSceneObjects)
		{
			return UnityEditor.EditorGUI.ObjectField(position, obj, objType, allowSceneObjects);
		}

		[Obsolete("Check the docs for the usage of the new parameter 'allowSceneObjects'.")]
		public static UnityEngine.Object ObjectField(UnityEngine.Rect position, UnityEngine.Object obj, System.Type objType)
		{
			return UnityEditor.EditorGUI.ObjectField(position, obj, objType);
		}

		public static UnityEngine.Object ObjectField(UnityEngine.Rect position, System.String label, UnityEngine.Object obj, System.Type objType, bool allowSceneObjects)
		{
			return UnityEditor.EditorGUI.ObjectField(position, TranslationUtility.get(label), obj, objType, allowSceneObjects);
		}

		[Obsolete("Check the docs for the usage of the new parameter 'allowSceneObjects'.")]
		public static UnityEngine.Object ObjectField(UnityEngine.Rect position, System.String label, UnityEngine.Object obj, System.Type objType)
		{
			return UnityEditor.EditorGUI.ObjectField(position, TranslationUtility.get(label), obj, objType);
		}

		public static UnityEngine.Object ObjectField(UnityEngine.Rect position, UnityEngine.GUIContent label, UnityEngine.Object obj, System.Type objType, bool allowSceneObjects)
		{
			return UnityEditor.EditorGUI.ObjectField(position, label, obj, objType, allowSceneObjects);
		}

		[Obsolete("Check the docs for the usage of the new parameter 'allowSceneObjects'.")]
		public static UnityEngine.Object ObjectField(UnityEngine.Rect position, UnityEngine.GUIContent label, UnityEngine.Object obj, System.Type objType)
		{
			return UnityEditor.EditorGUI.ObjectField(position, label, obj, objType);
		}

		public static UnityEngine.Rect IndentedRect(UnityEngine.Rect source)
		{
			return UnityEditor.EditorGUI.IndentedRect(source);
		}

		public static UnityEngine.Vector2 Vector2Field(UnityEngine.Rect position, System.String label, UnityEngine.Vector2 value)
		{
			return UnityEditor.EditorGUI.Vector2Field(position, TranslationUtility.get(label), value);
		}

		public static UnityEngine.Vector2 Vector2Field(UnityEngine.Rect position, UnityEngine.GUIContent label, UnityEngine.Vector2 value)
		{
			return UnityEditor.EditorGUI.Vector2Field(position, label, value);
		}

		public static UnityEngine.Vector3 Vector3Field(UnityEngine.Rect position, System.String label, UnityEngine.Vector3 value)
		{
			return UnityEditor.EditorGUI.Vector3Field(position, TranslationUtility.get(label), value);
		}

		public static UnityEngine.Vector3 Vector3Field(UnityEngine.Rect position, UnityEngine.GUIContent label, UnityEngine.Vector3 value)
		{
			return UnityEditor.EditorGUI.Vector3Field(position, label, value);
		}

		public static UnityEngine.Vector4 Vector4Field(UnityEngine.Rect position, System.String label, UnityEngine.Vector4 value)
		{
			return UnityEditor.EditorGUI.Vector4Field(position, TranslationUtility.get(label), value);
		}

		public static UnityEngine.Rect RectField(UnityEngine.Rect position, UnityEngine.Rect value)
		{
			return UnityEditor.EditorGUI.RectField(position, value);
		}

		public static UnityEngine.Rect RectField(UnityEngine.Rect position, System.String label, UnityEngine.Rect value)
		{
			return UnityEditor.EditorGUI.RectField(position, TranslationUtility.get(label), value);
		}

		public static UnityEngine.Rect RectField(UnityEngine.Rect position, UnityEngine.GUIContent label, UnityEngine.Rect value)
		{
			return UnityEditor.EditorGUI.RectField(position, label, value);
		}

		public static UnityEngine.Bounds BoundsField(UnityEngine.Rect position, UnityEngine.Bounds value)
		{
			return UnityEditor.EditorGUI.BoundsField(position, value);
		}

		public static UnityEngine.Bounds BoundsField(UnityEngine.Rect position, UnityEngine.GUIContent label, UnityEngine.Bounds value)
		{
			return UnityEditor.EditorGUI.BoundsField(position, label, value);
		}

		public static UnityEngine.Color ColorField(UnityEngine.Rect position, UnityEngine.Color value)
		{
			return UnityEditor.EditorGUI.ColorField(position, value);
		}

		public static UnityEngine.Color ColorField(UnityEngine.Rect position, System.String label, UnityEngine.Color value)
		{
			return UnityEditor.EditorGUI.ColorField(position, TranslationUtility.get(label), value);
		}

		public static UnityEngine.Color ColorField(UnityEngine.Rect position, UnityEngine.GUIContent label, UnityEngine.Color value)
		{
			return UnityEditor.EditorGUI.ColorField(position, label, value);
		}

		public static UnityEngine.AnimationCurve CurveField(UnityEngine.Rect position, UnityEngine.AnimationCurve value)
		{
			return UnityEditor.EditorGUI.CurveField(position, value);
		}

		public static UnityEngine.AnimationCurve CurveField(UnityEngine.Rect position, System.String label, UnityEngine.AnimationCurve value)
		{
			return UnityEditor.EditorGUI.CurveField(position, TranslationUtility.get(label), value);
		}

		public static UnityEngine.AnimationCurve CurveField(UnityEngine.Rect position, UnityEngine.GUIContent label, UnityEngine.AnimationCurve value)
		{
			return UnityEditor.EditorGUI.CurveField(position, label, value);
		}

		public static UnityEngine.AnimationCurve CurveField(UnityEngine.Rect position, UnityEngine.AnimationCurve value, UnityEngine.Color color, UnityEngine.Rect ranges)
		{
			return UnityEditor.EditorGUI.CurveField(position, value, color, ranges);
		}

		public static UnityEngine.AnimationCurve CurveField(UnityEngine.Rect position, System.String label, UnityEngine.AnimationCurve value, UnityEngine.Color color, UnityEngine.Rect ranges)
		{
			return UnityEditor.EditorGUI.CurveField(position, TranslationUtility.get(label), value, color, ranges);
		}

		public static UnityEngine.AnimationCurve CurveField(UnityEngine.Rect position, UnityEngine.GUIContent label, UnityEngine.AnimationCurve value, UnityEngine.Color color, UnityEngine.Rect ranges)
		{
			return UnityEditor.EditorGUI.CurveField(position, label, value, color, ranges);
		}

		public static void CurveField(UnityEngine.Rect position, UnityEditor.SerializedProperty value, UnityEngine.Color color, UnityEngine.Rect ranges)
		{
			UnityEditor.EditorGUI.CurveField(position, value, color, ranges);
		}

		public static bool InspectorTitlebar(UnityEngine.Rect position, bool foldout, UnityEngine.Object targetObj)
		{
			return UnityEditor.EditorGUI.InspectorTitlebar(position, foldout, targetObj);
		}

		public static bool InspectorTitlebar(UnityEngine.Rect position, bool foldout, UnityEngine.Object[] targetObjs)
		{
			return UnityEditor.EditorGUI.InspectorTitlebar(position, foldout, targetObjs);
		}

		public static bool Foldout(UnityEngine.Rect position, bool foldout, System.String content)
		{
			return UnityEditor.EditorGUI.Foldout(position, foldout, TranslationUtility.get(content));
		}

		public static bool Foldout(UnityEngine.Rect position, bool foldout, System.String content, UnityEngine.GUIStyle style)
		{
			return UnityEditor.EditorGUI.Foldout(position, foldout, TranslationUtility.get(content), style);
		}

		public static bool Foldout(UnityEngine.Rect position, bool foldout, System.String content, bool toggleOnLabelClick)
		{
			return UnityEditor.EditorGUI.Foldout(position, foldout, TranslationUtility.get(content), toggleOnLabelClick);
		}

		public static bool Foldout(UnityEngine.Rect position, bool foldout, System.String content, bool toggleOnLabelClick, UnityEngine.GUIStyle style)
		{
			return UnityEditor.EditorGUI.Foldout(position, foldout, TranslationUtility.get(content), toggleOnLabelClick, style);
		}

		public static bool Foldout(UnityEngine.Rect position, bool foldout, UnityEngine.GUIContent content)
		{
			return UnityEditor.EditorGUI.Foldout(position, foldout, content);
		}

		public static bool Foldout(UnityEngine.Rect position, bool foldout, UnityEngine.GUIContent content, UnityEngine.GUIStyle style)
		{
			return UnityEditor.EditorGUI.Foldout(position, foldout, content, style);
		}

		public static bool Foldout(UnityEngine.Rect position, bool foldout, UnityEngine.GUIContent content, bool toggleOnLabelClick)
		{
			return UnityEditor.EditorGUI.Foldout(position, foldout, content, toggleOnLabelClick);
		}

		public static bool Foldout(UnityEngine.Rect position, bool foldout, UnityEngine.GUIContent content, bool toggleOnLabelClick, UnityEngine.GUIStyle style)
		{
			return UnityEditor.EditorGUI.Foldout(position, foldout, content, toggleOnLabelClick, style);
		}

		public static void ProgressBar(UnityEngine.Rect position, float value, System.String text)
		{
			UnityEditor.EditorGUI.ProgressBar(position, value, TranslationUtility.get(text));
		}

		public static void HelpBox(UnityEngine.Rect position, System.String message, UnityEditor.MessageType type)
		{
			UnityEditor.EditorGUI.HelpBox(position, TranslationUtility.get(message), type);
		}

		public static void HandlePrefixLabel(UnityEngine.Rect totalPosition, UnityEngine.Rect labelPosition, UnityEngine.GUIContent label, int id)
		{
			UnityEditor.EditorGUI.HandlePrefixLabel(totalPosition, labelPosition, label, id);
		}

		public static void HandlePrefixLabel(UnityEngine.Rect totalPosition, UnityEngine.Rect labelPosition, UnityEngine.GUIContent label)
		{
			UnityEditor.EditorGUI.HandlePrefixLabel(totalPosition, labelPosition, label);
		}

		public static void HandlePrefixLabel(UnityEngine.Rect totalPosition, UnityEngine.Rect labelPosition, UnityEngine.GUIContent label, int id, UnityEngine.GUIStyle style)
		{
			UnityEditor.EditorGUI.HandlePrefixLabel(totalPosition, labelPosition, label, id, style);
		}

		public static UnityEngine.Rect PrefixLabel(UnityEngine.Rect totalPosition, UnityEngine.GUIContent label)
		{
			return UnityEditor.EditorGUI.PrefixLabel(totalPosition, label);
		}

		public static UnityEngine.Rect PrefixLabel(UnityEngine.Rect totalPosition, int id, UnityEngine.GUIContent label)
		{
			return UnityEditor.EditorGUI.PrefixLabel(totalPosition, id, label);
		}

		public static UnityEngine.GUIContent BeginProperty(UnityEngine.Rect totalPosition, UnityEngine.GUIContent label, UnityEditor.SerializedProperty property)
		{
			return UnityEditor.EditorGUI.BeginProperty(totalPosition, label, property);
		}

		public static void EndProperty()
		{
			UnityEditor.EditorGUI.EndProperty();
		}

		public static void DrawTextureAlpha(UnityEngine.Rect position, UnityEngine.Texture image, UnityEngine.ScaleMode scaleMode)
		{
			UnityEditor.EditorGUI.DrawTextureAlpha(position, image, scaleMode);
		}

		public static void DrawTextureAlpha(UnityEngine.Rect position, UnityEngine.Texture image)
		{
			UnityEditor.EditorGUI.DrawTextureAlpha(position, image);
		}

		public static void DrawTextureAlpha(UnityEngine.Rect position, UnityEngine.Texture image, UnityEngine.ScaleMode scaleMode, float imageAspect)
		{
			UnityEditor.EditorGUI.DrawTextureAlpha(position, image, scaleMode, imageAspect);
		}

		public static void DrawTextureTransparent(UnityEngine.Rect position, UnityEngine.Texture image, UnityEngine.ScaleMode scaleMode)
		{
			UnityEditor.EditorGUI.DrawTextureTransparent(position, image, scaleMode);
		}

		public static void DrawTextureTransparent(UnityEngine.Rect position, UnityEngine.Texture image)
		{
			UnityEditor.EditorGUI.DrawTextureTransparent(position, image);
		}

		public static void DrawTextureTransparent(UnityEngine.Rect position, UnityEngine.Texture image, UnityEngine.ScaleMode scaleMode, float imageAspect)
		{
			UnityEditor.EditorGUI.DrawTextureTransparent(position, image, scaleMode, imageAspect);
		}

		public static void DrawPreviewTexture(UnityEngine.Rect position, UnityEngine.Texture image, UnityEngine.Material mat, UnityEngine.ScaleMode scaleMode)
		{
			UnityEditor.EditorGUI.DrawPreviewTexture(position, image, mat, scaleMode);
		}

		public static void DrawPreviewTexture(UnityEngine.Rect position, UnityEngine.Texture image, UnityEngine.Material mat)
		{
			UnityEditor.EditorGUI.DrawPreviewTexture(position, image, mat);
		}

		public static void DrawPreviewTexture(UnityEngine.Rect position, UnityEngine.Texture image)
		{
			UnityEditor.EditorGUI.DrawPreviewTexture(position, image);
		}

		public static void DrawPreviewTexture(UnityEngine.Rect position, UnityEngine.Texture image, UnityEngine.Material mat, UnityEngine.ScaleMode scaleMode, float imageAspect)
		{
			UnityEditor.EditorGUI.DrawPreviewTexture(position, image, mat, scaleMode, imageAspect);
		}

		public static float GetPropertyHeight(UnityEditor.SerializedProperty property, UnityEngine.GUIContent label)
		{
			return UnityEditor.EditorGUI.GetPropertyHeight(property, label);
		}

		public static float GetPropertyHeight(UnityEditor.SerializedProperty property)
		{
			return UnityEditor.EditorGUI.GetPropertyHeight(property);
		}

		public static float GetPropertyHeight(UnityEditor.SerializedProperty property, UnityEngine.GUIContent label, bool includeChildren)
		{
			return UnityEditor.EditorGUI.GetPropertyHeight(property, label, includeChildren);
		}

		public static bool PropertyField(UnityEngine.Rect position, UnityEditor.SerializedProperty property)
		{
			return UnityEditor.EditorGUI.PropertyField(position, property);
		}

		public static bool PropertyField(UnityEngine.Rect position, UnityEditor.SerializedProperty property, bool includeChildren)
		{
			return UnityEditor.EditorGUI.PropertyField(position, property, includeChildren);
		}

		public static bool PropertyField(UnityEngine.Rect position, UnityEditor.SerializedProperty property, UnityEngine.GUIContent label)
		{
			return UnityEditor.EditorGUI.PropertyField(position, property, label);
		}

		public static bool PropertyField(UnityEngine.Rect position, UnityEditor.SerializedProperty property, UnityEngine.GUIContent label, bool includeChildren)
		{
			return UnityEditor.EditorGUI.PropertyField(position, property, label, includeChildren);
		}
	}

#pragma warning restore 618
}