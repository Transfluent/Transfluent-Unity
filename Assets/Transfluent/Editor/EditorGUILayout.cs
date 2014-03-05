using UnityEngine;
using System;
//wrapper around unity's gui, except to grab text as quickly as possbile and spit it into an internal db
//http://docs.unity3d.com/Documentation/ScriptReference/GUI.html
namespace transfluent.guiwrapper
{
#pragma warning disable 618
	public partial class EditorGUILayout
	{

public static void LabelField( System.String label,params UnityEngine.GUILayoutOption[] options)
{
  UnityEditor.EditorGUILayout.LabelField(label,options);
}
public static void LabelField( System.String label, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
  UnityEditor.EditorGUILayout.LabelField(label,style,options);
}
public static void LabelField( UnityEngine.GUIContent label,params UnityEngine.GUILayoutOption[] options)
{
  UnityEditor.EditorGUILayout.LabelField(label,options);
}
public static void LabelField( UnityEngine.GUIContent label, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
  UnityEditor.EditorGUILayout.LabelField(label,style,options);
}
public static void LabelField( System.String label, System.String label2,params UnityEngine.GUILayoutOption[] options)
{
  UnityEditor.EditorGUILayout.LabelField(label,label2,options);
}
public static void LabelField( System.String label, System.String label2, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
  UnityEditor.EditorGUILayout.LabelField(label,label2,style,options);
}
public static void LabelField( UnityEngine.GUIContent label, UnityEngine.GUIContent label2,params UnityEngine.GUILayoutOption[] options)
{
  UnityEditor.EditorGUILayout.LabelField(label,label2,options);
}
public static void LabelField( UnityEngine.GUIContent label, UnityEngine.GUIContent label2, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
  UnityEditor.EditorGUILayout.LabelField(label,label2,style,options);
}
public static bool Toggle( bool value,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.Toggle(value,options);
}
public static bool Toggle( System.String label, bool value,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.Toggle(label,value,options);
}
public static bool Toggle( UnityEngine.GUIContent label, bool value,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.Toggle(label,value,options);
}
public static bool Toggle( bool value, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.Toggle(value,style,options);
}
public static bool Toggle( System.String label, bool value, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.Toggle(label,value,style,options);
}
public static bool Toggle( UnityEngine.GUIContent label, bool value, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.Toggle(label,value,style,options);
}
public static bool ToggleLeft( System.String label, bool value,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.ToggleLeft(label,value,options);
}
public static bool ToggleLeft( UnityEngine.GUIContent label, bool value,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.ToggleLeft(label,value,options);
}
public static bool ToggleLeft( System.String label, bool value, UnityEngine.GUIStyle labelStyle,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.ToggleLeft(label,value,labelStyle,options);
}
public static bool ToggleLeft( UnityEngine.GUIContent label, bool value, UnityEngine.GUIStyle labelStyle,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.ToggleLeft(label,value,labelStyle,options);
}
public static System.String TextField( System.String text,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.TextField(text,options);
}
public static System.String TextField( System.String text, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.TextField(text,style,options);
}
public static System.String TextField( System.String label, System.String text,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.TextField(label,text,options);
}
public static System.String TextField( System.String label, System.String text, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.TextField(label,text,style,options);
}
public static System.String TextField( UnityEngine.GUIContent label, System.String text,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.TextField(label,text,options);
}
public static System.String TextField( UnityEngine.GUIContent label, System.String text, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.TextField(label,text,style,options);
}
public static System.String TextArea( System.String text,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.TextArea(text,options);
}
public static System.String TextArea( System.String text, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.TextArea(text,style,options);
}
public static void SelectableLabel( System.String text,params UnityEngine.GUILayoutOption[] options)
{
  UnityEditor.EditorGUILayout.SelectableLabel(text,options);
}
public static void SelectableLabel( System.String text, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
  UnityEditor.EditorGUILayout.SelectableLabel(text,style,options);
}
public static System.String PasswordField( System.String password,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.PasswordField(password,options);
}
public static System.String PasswordField( System.String password, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.PasswordField(password,style,options);
}
public static System.String PasswordField( System.String label, System.String password,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.PasswordField(label,password,options);
}
public static System.String PasswordField( System.String label, System.String password, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.PasswordField(label,password,style,options);
}
public static System.String PasswordField( UnityEngine.GUIContent label, System.String password,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.PasswordField(label,password,options);
}
public static System.String PasswordField( UnityEngine.GUIContent label, System.String password, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.PasswordField(label,password,style,options);
}
public static float FloatField( float value,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.FloatField(value,options);
}
public static float FloatField( float value, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.FloatField(value,style,options);
}
public static float FloatField( System.String label, float value,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.FloatField(label,value,options);
}
public static float FloatField( System.String label, float value, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.FloatField(label,value,style,options);
}
public static float FloatField( UnityEngine.GUIContent label, float value,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.FloatField(label,value,options);
}
public static float FloatField( UnityEngine.GUIContent label, float value, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.FloatField(label,value,style,options);
}
public static int IntField( int value,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.IntField(value,options);
}
public static int IntField( int value, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.IntField(value,style,options);
}
public static int IntField( System.String label, int value,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.IntField(label,value,options);
}
public static int IntField( System.String label, int value, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.IntField(label,value,style,options);
}
public static int IntField( UnityEngine.GUIContent label, int value,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.IntField(label,value,options);
}
public static int IntField( UnityEngine.GUIContent label, int value, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.IntField(label,value,style,options);
}
public static float Slider( float value, float leftValue, float rightValue,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.Slider(value,leftValue,rightValue,options);
}
public static float Slider( System.String label, float value, float leftValue, float rightValue,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.Slider(label,value,leftValue,rightValue,options);
}
public static float Slider( UnityEngine.GUIContent label, float value, float leftValue, float rightValue,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.Slider(label,value,leftValue,rightValue,options);
}
public static void Slider( UnityEditor.SerializedProperty property, float leftValue, float rightValue,params UnityEngine.GUILayoutOption[] options)
{
  UnityEditor.EditorGUILayout.Slider(property,leftValue,rightValue,options);
}
public static void Slider( UnityEditor.SerializedProperty property, float leftValue, float rightValue, System.String label,params UnityEngine.GUILayoutOption[] options)
{
  UnityEditor.EditorGUILayout.Slider(property,leftValue,rightValue,label,options);
}
public static void Slider( UnityEditor.SerializedProperty property, float leftValue, float rightValue, UnityEngine.GUIContent label,params UnityEngine.GUILayoutOption[] options)
{
  UnityEditor.EditorGUILayout.Slider(property,leftValue,rightValue,label,options);
}
public static int IntSlider( int value, int leftValue, int rightValue,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.IntSlider(value,leftValue,rightValue,options);
}
public static int IntSlider( System.String label, int value, int leftValue, int rightValue,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.IntSlider(label,value,leftValue,rightValue,options);
}
public static int IntSlider( UnityEngine.GUIContent label, int value, int leftValue, int rightValue,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.IntSlider(label,value,leftValue,rightValue,options);
}
public static void IntSlider( UnityEditor.SerializedProperty property, int leftValue, int rightValue,params UnityEngine.GUILayoutOption[] options)
{
  UnityEditor.EditorGUILayout.IntSlider(property,leftValue,rightValue,options);
}
public static void IntSlider( UnityEditor.SerializedProperty property, int leftValue, int rightValue, System.String label,params UnityEngine.GUILayoutOption[] options)
{
  UnityEditor.EditorGUILayout.IntSlider(property,leftValue,rightValue,label,options);
}
public static void IntSlider( UnityEditor.SerializedProperty property, int leftValue, int rightValue, UnityEngine.GUIContent label,params UnityEngine.GUILayoutOption[] options)
{
  UnityEditor.EditorGUILayout.IntSlider(property,leftValue,rightValue,label,options);
}
public static void MinMaxSlider(ref float minValue,ref float maxValue, float minLimit, float maxLimit,params UnityEngine.GUILayoutOption[] options)
{
  UnityEditor.EditorGUILayout.MinMaxSlider(ref minValue,ref maxValue,minLimit,maxLimit,options);
}
public static void MinMaxSlider( UnityEngine.GUIContent label,ref float minValue,ref float maxValue, float minLimit, float maxLimit,params UnityEngine.GUILayoutOption[] options)
{
  UnityEditor.EditorGUILayout.MinMaxSlider(label,ref minValue,ref maxValue,minLimit,maxLimit,options);
}
public static int Popup( int selectedIndex, System.String[] displayedOptions,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.Popup(selectedIndex,displayedOptions,options);
}
public static int Popup( int selectedIndex, System.String[] displayedOptions, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.Popup(selectedIndex,displayedOptions,style,options);
}
public static int Popup( int selectedIndex, UnityEngine.GUIContent[] displayedOptions,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.Popup(selectedIndex,displayedOptions,options);
}
public static int Popup( int selectedIndex, UnityEngine.GUIContent[] displayedOptions, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.Popup(selectedIndex,displayedOptions,style,options);
}
public static int Popup( System.String label, int selectedIndex, System.String[] displayedOptions,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.Popup(label,selectedIndex,displayedOptions,options);
}
public static int Popup( System.String label, int selectedIndex, System.String[] displayedOptions, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.Popup(label,selectedIndex,displayedOptions,style,options);
}
public static int Popup( UnityEngine.GUIContent label, int selectedIndex, UnityEngine.GUIContent[] displayedOptions,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.Popup(label,selectedIndex,displayedOptions,options);
}
public static int Popup( UnityEngine.GUIContent label, int selectedIndex, UnityEngine.GUIContent[] displayedOptions, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.Popup(label,selectedIndex,displayedOptions,style,options);
}
public static System.Enum EnumPopup( System.Enum selected,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.EnumPopup(selected,options);
}
public static System.Enum EnumPopup( System.Enum selected, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.EnumPopup(selected,style,options);
}
public static System.Enum EnumPopup( System.String label, System.Enum selected,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.EnumPopup(label,selected,options);
}
public static System.Enum EnumPopup( System.String label, System.Enum selected, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.EnumPopup(label,selected,style,options);
}
public static System.Enum EnumPopup( UnityEngine.GUIContent label, System.Enum selected,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.EnumPopup(label,selected,options);
}
public static System.Enum EnumPopup( UnityEngine.GUIContent label, System.Enum selected, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.EnumPopup(label,selected,style,options);
}
public static int IntPopup( int selectedValue, System.String[] displayedOptions, System.Int32[] optionValues,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.IntPopup(selectedValue,displayedOptions,optionValues,options);
}
public static int IntPopup( int selectedValue, System.String[] displayedOptions, System.Int32[] optionValues, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.IntPopup(selectedValue,displayedOptions,optionValues,style,options);
}
public static int IntPopup( int selectedValue, UnityEngine.GUIContent[] displayedOptions, System.Int32[] optionValues,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.IntPopup(selectedValue,displayedOptions,optionValues,options);
}
public static int IntPopup( int selectedValue, UnityEngine.GUIContent[] displayedOptions, System.Int32[] optionValues, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.IntPopup(selectedValue,displayedOptions,optionValues,style,options);
}
public static int IntPopup( System.String label, int selectedValue, System.String[] displayedOptions, System.Int32[] optionValues,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.IntPopup(label,selectedValue,displayedOptions,optionValues,options);
}
public static int IntPopup( System.String label, int selectedValue, System.String[] displayedOptions, System.Int32[] optionValues, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.IntPopup(label,selectedValue,displayedOptions,optionValues,style,options);
}
public static int IntPopup( UnityEngine.GUIContent label, int selectedValue, UnityEngine.GUIContent[] displayedOptions, System.Int32[] optionValues,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.IntPopup(label,selectedValue,displayedOptions,optionValues,options);
}
public static int IntPopup( UnityEngine.GUIContent label, int selectedValue, UnityEngine.GUIContent[] displayedOptions, System.Int32[] optionValues, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.IntPopup(label,selectedValue,displayedOptions,optionValues,style,options);
}
public static void IntPopup( UnityEditor.SerializedProperty property, UnityEngine.GUIContent[] displayedOptions, System.Int32[] optionValues,params UnityEngine.GUILayoutOption[] options)
{
  UnityEditor.EditorGUILayout.IntPopup(property,displayedOptions,optionValues,options);
}
public static void IntPopup( UnityEditor.SerializedProperty property, UnityEngine.GUIContent[] displayedOptions, System.Int32[] optionValues, UnityEngine.GUIContent label,params UnityEngine.GUILayoutOption[] options)
{
  UnityEditor.EditorGUILayout.IntPopup(property,displayedOptions,optionValues,label,options);
}
[Obsolete("This function is obsolete and the style is not used.")]
public static void IntPopup( UnityEditor.SerializedProperty property, UnityEngine.GUIContent[] displayedOptions, System.Int32[] optionValues, UnityEngine.GUIContent label, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
  UnityEditor.EditorGUILayout.IntPopup(property,displayedOptions,optionValues,label,style,options);
}
public static System.String TagField( System.String tag,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.TagField(tag,options);
}
public static System.String TagField( System.String tag, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.TagField(tag,style,options);
}
public static System.String TagField( System.String label, System.String tag,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.TagField(label,tag,options);
}
public static System.String TagField( System.String label, System.String tag, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.TagField(label,tag,style,options);
}
public static System.String TagField( UnityEngine.GUIContent label, System.String tag,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.TagField(label,tag,options);
}
public static System.String TagField( UnityEngine.GUIContent label, System.String tag, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.TagField(label,tag,style,options);
}
public static int LayerField( int layer,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.LayerField(layer,options);
}
public static int LayerField( int layer, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.LayerField(layer,style,options);
}
public static int LayerField( System.String label, int layer,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.LayerField(label,layer,options);
}
public static int LayerField( System.String label, int layer, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.LayerField(label,layer,style,options);
}
public static int LayerField( UnityEngine.GUIContent label, int layer,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.LayerField(label,layer,options);
}
public static int LayerField( UnityEngine.GUIContent label, int layer, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.LayerField(label,layer,style,options);
}
public static int MaskField( UnityEngine.GUIContent label, int mask, System.String[] displayedOptions, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.MaskField(label,mask,displayedOptions,style,options);
}
public static int MaskField( System.String label, int mask, System.String[] displayedOptions, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.MaskField(label,mask,displayedOptions,style,options);
}
public static int MaskField( UnityEngine.GUIContent label, int mask, System.String[] displayedOptions,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.MaskField(label,mask,displayedOptions,options);
}
public static int MaskField( System.String label, int mask, System.String[] displayedOptions,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.MaskField(label,mask,displayedOptions,options);
}
public static int MaskField( int mask, System.String[] displayedOptions, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.MaskField(mask,displayedOptions,style,options);
}
public static int MaskField( int mask, System.String[] displayedOptions,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.MaskField(mask,displayedOptions,options);
}
public static System.Enum EnumMaskField( UnityEngine.GUIContent label, System.Enum enumValue, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.EnumMaskField(label,enumValue,style,options);
}
public static System.Enum EnumMaskField( System.String label, System.Enum enumValue, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.EnumMaskField(label,enumValue,style,options);
}
public static System.Enum EnumMaskField( UnityEngine.GUIContent label, System.Enum enumValue,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.EnumMaskField(label,enumValue,options);
}
public static System.Enum EnumMaskField( System.String label, System.Enum enumValue,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.EnumMaskField(label,enumValue,options);
}
public static System.Enum EnumMaskField( System.Enum enumValue, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.EnumMaskField(enumValue,style,options);
}
public static System.Enum EnumMaskField( System.Enum enumValue,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.EnumMaskField(enumValue,options);
}
[Obsolete("Check the docs for the usage of the new parameter 'allowSceneObjects'.")]
public static UnityEngine.Object ObjectField( UnityEngine.Object obj, System.Type objType,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.ObjectField(obj,objType,options);
}
public static UnityEngine.Object ObjectField( UnityEngine.Object obj, System.Type objType, bool allowSceneObjects,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.ObjectField(obj,objType,allowSceneObjects,options);
}
[Obsolete("Check the docs for the usage of the new parameter 'allowSceneObjects'.")]
public static UnityEngine.Object ObjectField( System.String label, UnityEngine.Object obj, System.Type objType,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.ObjectField(label,obj,objType,options);
}
public static UnityEngine.Object ObjectField( System.String label, UnityEngine.Object obj, System.Type objType, bool allowSceneObjects,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.ObjectField(label,obj,objType,allowSceneObjects,options);
}
[Obsolete("Check the docs for the usage of the new parameter 'allowSceneObjects'.")]
public static UnityEngine.Object ObjectField( UnityEngine.GUIContent label, UnityEngine.Object obj, System.Type objType,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.ObjectField(label,obj,objType,options);
}
public static UnityEngine.Object ObjectField( UnityEngine.GUIContent label, UnityEngine.Object obj, System.Type objType, bool allowSceneObjects,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.ObjectField(label,obj,objType,allowSceneObjects,options);
}
public static UnityEngine.Vector2 Vector2Field( System.String label, UnityEngine.Vector2 value,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.Vector2Field(label,value,options);
}
public static UnityEngine.Vector2 Vector2Field( UnityEngine.GUIContent label, UnityEngine.Vector2 value,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.Vector2Field(label,value,options);
}
public static UnityEngine.Vector3 Vector3Field( System.String label, UnityEngine.Vector3 value,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.Vector3Field(label,value,options);
}
public static UnityEngine.Vector3 Vector3Field( UnityEngine.GUIContent label, UnityEngine.Vector3 value,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.Vector3Field(label,value,options);
}
public static UnityEngine.Vector4 Vector4Field( System.String label, UnityEngine.Vector4 value,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.Vector4Field(label,value,options);
}
public static UnityEngine.Rect RectField( UnityEngine.Rect value,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.RectField(value,options);
}
public static UnityEngine.Rect RectField( System.String label, UnityEngine.Rect value,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.RectField(label,value,options);
}
public static UnityEngine.Rect RectField( UnityEngine.GUIContent label, UnityEngine.Rect value,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.RectField(label,value,options);
}
public static UnityEngine.Bounds BoundsField( UnityEngine.Bounds value,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.BoundsField(value,options);
}
public static UnityEngine.Bounds BoundsField( System.String label, UnityEngine.Bounds value,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.BoundsField(label,value,options);
}
public static UnityEngine.Bounds BoundsField( UnityEngine.GUIContent label, UnityEngine.Bounds value,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.BoundsField(label,value,options);
}
public static UnityEngine.Color ColorField( UnityEngine.Color value,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.ColorField(value,options);
}
public static UnityEngine.Color ColorField( System.String label, UnityEngine.Color value,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.ColorField(label,value,options);
}
public static UnityEngine.Color ColorField( UnityEngine.GUIContent label, UnityEngine.Color value,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.ColorField(label,value,options);
}
public static UnityEngine.AnimationCurve CurveField( UnityEngine.AnimationCurve value,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.CurveField(value,options);
}
public static UnityEngine.AnimationCurve CurveField( System.String label, UnityEngine.AnimationCurve value,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.CurveField(label,value,options);
}
public static UnityEngine.AnimationCurve CurveField( UnityEngine.GUIContent label, UnityEngine.AnimationCurve value,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.CurveField(label,value,options);
}
public static UnityEngine.AnimationCurve CurveField( UnityEngine.AnimationCurve value, UnityEngine.Color color, UnityEngine.Rect ranges,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.CurveField(value,color,ranges,options);
}
public static UnityEngine.AnimationCurve CurveField( System.String label, UnityEngine.AnimationCurve value, UnityEngine.Color color, UnityEngine.Rect ranges,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.CurveField(label,value,color,ranges,options);
}
public static UnityEngine.AnimationCurve CurveField( UnityEngine.GUIContent label, UnityEngine.AnimationCurve value, UnityEngine.Color color, UnityEngine.Rect ranges,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.CurveField(label,value,color,ranges,options);
}
public static bool InspectorTitlebar( bool foldout, UnityEngine.Object targetObj)
{
 return  UnityEditor.EditorGUILayout.InspectorTitlebar(foldout,targetObj);
}
public static bool InspectorTitlebar( bool foldout, UnityEngine.Object[] targetObjs)
{
 return  UnityEditor.EditorGUILayout.InspectorTitlebar(foldout,targetObjs);
}
public static bool Foldout( bool foldout, System.String content)
{
 return  UnityEditor.EditorGUILayout.Foldout(foldout,content);
}
public static bool Foldout( bool foldout, System.String content, UnityEngine.GUIStyle style)
{
 return  UnityEditor.EditorGUILayout.Foldout(foldout,content,style);
}
public static bool Foldout( bool foldout, UnityEngine.GUIContent content)
{
 return  UnityEditor.EditorGUILayout.Foldout(foldout,content);
}
public static bool Foldout( bool foldout, UnityEngine.GUIContent content, UnityEngine.GUIStyle style)
{
 return  UnityEditor.EditorGUILayout.Foldout(foldout,content,style);
}
public static void HelpBox( System.String message, UnityEditor.MessageType type)
{
  UnityEditor.EditorGUILayout.HelpBox(message,type);
}
public static void HelpBox( System.String message, UnityEditor.MessageType type, bool wide)
{
  UnityEditor.EditorGUILayout.HelpBox(message,type,wide);
}
public static void PrefixLabel( System.String label)
{
  UnityEditor.EditorGUILayout.PrefixLabel(label);
}
public static void PrefixLabel( System.String label, UnityEngine.GUIStyle followingStyle)
{
  UnityEditor.EditorGUILayout.PrefixLabel(label,followingStyle);
}
public static void PrefixLabel( System.String label, UnityEngine.GUIStyle followingStyle, UnityEngine.GUIStyle labelStyle)
{
  UnityEditor.EditorGUILayout.PrefixLabel(label,followingStyle,labelStyle);
}
public static void PrefixLabel( UnityEngine.GUIContent label)
{
  UnityEditor.EditorGUILayout.PrefixLabel(label);
}
public static void PrefixLabel( UnityEngine.GUIContent label, UnityEngine.GUIStyle followingStyle)
{
  UnityEditor.EditorGUILayout.PrefixLabel(label,followingStyle);
}
public static void PrefixLabel( UnityEngine.GUIContent label, UnityEngine.GUIStyle followingStyle, UnityEngine.GUIStyle labelStyle)
{
  UnityEditor.EditorGUILayout.PrefixLabel(label,followingStyle,labelStyle);
}
public static void Space()
{
  UnityEditor.EditorGUILayout.Space();
}
public static void Separator()
{
  UnityEditor.EditorGUILayout.Separator();
}
public static bool BeginToggleGroup( System.String label, bool toggle)
{
 return  UnityEditor.EditorGUILayout.BeginToggleGroup(label,toggle);
}
public static bool BeginToggleGroup( UnityEngine.GUIContent label, bool toggle)
{
 return  UnityEditor.EditorGUILayout.BeginToggleGroup(label,toggle);
}
public static void EndToggleGroup()
{
  UnityEditor.EditorGUILayout.EndToggleGroup();
}
public static UnityEngine.Rect BeginHorizontal(params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.BeginHorizontal(options);
}
public static UnityEngine.Rect BeginHorizontal( UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.BeginHorizontal(style,options);
}
public static void EndHorizontal()
{
  UnityEditor.EditorGUILayout.EndHorizontal();
}
public static UnityEngine.Rect BeginVertical(params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.BeginVertical(options);
}
public static UnityEngine.Rect BeginVertical( UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.BeginVertical(style,options);
}
public static void EndVertical()
{
  UnityEditor.EditorGUILayout.EndVertical();
}
public static UnityEngine.Vector2 BeginScrollView( UnityEngine.Vector2 scrollPosition,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.BeginScrollView(scrollPosition,options);
}
public static UnityEngine.Vector2 BeginScrollView( UnityEngine.Vector2 scrollPosition, bool alwaysShowHorizontal, bool alwaysShowVertical,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.BeginScrollView(scrollPosition,alwaysShowHorizontal,alwaysShowVertical,options);
}
public static UnityEngine.Vector2 BeginScrollView( UnityEngine.Vector2 scrollPosition, UnityEngine.GUIStyle horizontalScrollbar, UnityEngine.GUIStyle verticalScrollbar,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.BeginScrollView(scrollPosition,horizontalScrollbar,verticalScrollbar,options);
}
public static UnityEngine.Vector2 BeginScrollView( UnityEngine.Vector2 scrollPosition, UnityEngine.GUIStyle style)
{
 return  UnityEditor.EditorGUILayout.BeginScrollView(scrollPosition,style);
}
public static UnityEngine.Vector2 BeginScrollView( UnityEngine.Vector2 scrollPosition, bool alwaysShowHorizontal, bool alwaysShowVertical, UnityEngine.GUIStyle horizontalScrollbar, UnityEngine.GUIStyle verticalScrollbar, UnityEngine.GUIStyle background,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.BeginScrollView(scrollPosition,alwaysShowHorizontal,alwaysShowVertical,horizontalScrollbar,verticalScrollbar,background,options);
}
public static void EndScrollView()
{
  UnityEditor.EditorGUILayout.EndScrollView();
}
public static bool PropertyField( UnityEditor.SerializedProperty property,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.PropertyField(property,options);
}
public static bool PropertyField( UnityEditor.SerializedProperty property, UnityEngine.GUIContent label,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.PropertyField(property,label,options);
}
public static bool PropertyField( UnityEditor.SerializedProperty property, bool includeChildren,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.PropertyField(property,includeChildren,options);
}
public static bool PropertyField( UnityEditor.SerializedProperty property, UnityEngine.GUIContent label, bool includeChildren,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.PropertyField(property,label,includeChildren,options);
}
public static UnityEngine.Rect GetControlRect(params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.GetControlRect(options);
}
public static UnityEngine.Rect GetControlRect( bool hasLabel,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.GetControlRect(hasLabel,options);
}
public static UnityEngine.Rect GetControlRect( bool hasLabel, float height,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.GetControlRect(hasLabel,height,options);
}
public static UnityEngine.Rect GetControlRect( bool hasLabel, float height, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEditor.EditorGUILayout.GetControlRect(hasLabel,height,style,options);
}

	}
#pragma warning restore 618
}