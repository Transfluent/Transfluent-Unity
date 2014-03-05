using UnityEngine;
using System;
//wrapper around unity's gui, except to grab text as quickly as possbile and spit it into an internal db
//http://docs.unity3d.com/Documentation/ScriptReference/GUI.html
namespace transfluent.guiwrapper
{
#pragma warning disable 618
	public partial class GUILayout
	{

public static void Label( UnityEngine.Texture image,params UnityEngine.GUILayoutOption[] options)
{
  UnityEngine.GUILayout.Label(image,options);
}
public static void Label( System.String text,params UnityEngine.GUILayoutOption[] options)
{
  UnityEngine.GUILayout.Label(text,options);
}
public static void Label( UnityEngine.GUIContent content,params UnityEngine.GUILayoutOption[] options)
{
  UnityEngine.GUILayout.Label(content,options);
}
public static void Label( UnityEngine.Texture image, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
  UnityEngine.GUILayout.Label(image,style,options);
}
public static void Label( System.String text, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
  UnityEngine.GUILayout.Label(text,style,options);
}
public static void Label( UnityEngine.GUIContent content, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
  UnityEngine.GUILayout.Label(content,style,options);
}
public static void Box( UnityEngine.Texture image,params UnityEngine.GUILayoutOption[] options)
{
  UnityEngine.GUILayout.Box(image,options);
}
public static void Box( System.String text,params UnityEngine.GUILayoutOption[] options)
{
  UnityEngine.GUILayout.Box(text,options);
}
public static void Box( UnityEngine.GUIContent content,params UnityEngine.GUILayoutOption[] options)
{
  UnityEngine.GUILayout.Box(content,options);
}
public static void Box( UnityEngine.Texture image, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
  UnityEngine.GUILayout.Box(image,style,options);
}
public static void Box( System.String text, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
  UnityEngine.GUILayout.Box(text,style,options);
}
public static void Box( UnityEngine.GUIContent content, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
  UnityEngine.GUILayout.Box(content,style,options);
}
public static bool Button( UnityEngine.Texture image,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.Button(image,options);
}
public static bool Button( System.String text,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.Button(text,options);
}
public static bool Button( UnityEngine.GUIContent content,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.Button(content,options);
}
public static bool Button( UnityEngine.Texture image, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.Button(image,style,options);
}
public static bool Button( System.String text, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.Button(text,style,options);
}
public static bool Button( UnityEngine.GUIContent content, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.Button(content,style,options);
}
public static bool RepeatButton( UnityEngine.Texture image,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.RepeatButton(image,options);
}
public static bool RepeatButton( System.String text,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.RepeatButton(text,options);
}
public static bool RepeatButton( UnityEngine.GUIContent content,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.RepeatButton(content,options);
}
public static bool RepeatButton( UnityEngine.Texture image, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.RepeatButton(image,style,options);
}
public static bool RepeatButton( System.String text, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.RepeatButton(text,style,options);
}
public static bool RepeatButton( UnityEngine.GUIContent content, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.RepeatButton(content,style,options);
}
public static System.String TextField( System.String text,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.TextField(text,options);
}
public static System.String TextField( System.String text, int maxLength,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.TextField(text,maxLength,options);
}
public static System.String TextField( System.String text, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.TextField(text,style,options);
}
public static System.String TextField( System.String text, int maxLength, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.TextField(text,maxLength,style,options);
}
public static System.String PasswordField( System.String password, char maskChar,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.PasswordField(password,maskChar,options);
}
public static System.String PasswordField( System.String password, char maskChar, int maxLength,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.PasswordField(password,maskChar,maxLength,options);
}
public static System.String PasswordField( System.String password, char maskChar, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.PasswordField(password,maskChar,style,options);
}
public static System.String PasswordField( System.String password, char maskChar, int maxLength, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.PasswordField(password,maskChar,maxLength,style,options);
}
public static System.String TextArea( System.String text,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.TextArea(text,options);
}
public static System.String TextArea( System.String text, int maxLength,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.TextArea(text,maxLength,options);
}
public static System.String TextArea( System.String text, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.TextArea(text,style,options);
}
public static System.String TextArea( System.String text, int maxLength, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.TextArea(text,maxLength,style,options);
}
public static bool Toggle( bool value, UnityEngine.Texture image,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.Toggle(value,image,options);
}
public static bool Toggle( bool value, System.String text,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.Toggle(value,text,options);
}
public static bool Toggle( bool value, UnityEngine.GUIContent content,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.Toggle(value,content,options);
}
public static bool Toggle( bool value, UnityEngine.Texture image, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.Toggle(value,image,style,options);
}
public static bool Toggle( bool value, System.String text, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.Toggle(value,text,style,options);
}
public static bool Toggle( bool value, UnityEngine.GUIContent content, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.Toggle(value,content,style,options);
}
public static int Toolbar( int selected, System.String[] texts,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.Toolbar(selected,texts,options);
}
public static int Toolbar( int selected, UnityEngine.Texture[] images,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.Toolbar(selected,images,options);
}
public static int Toolbar( int selected, UnityEngine.GUIContent[] content,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.Toolbar(selected,content,options);
}
public static int Toolbar( int selected, System.String[] texts, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.Toolbar(selected,texts,style,options);
}
public static int Toolbar( int selected, UnityEngine.Texture[] images, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.Toolbar(selected,images,style,options);
}
public static int Toolbar( int selected, UnityEngine.GUIContent[] contents, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.Toolbar(selected,contents,style,options);
}
public static int SelectionGrid( int selected, System.String[] texts, int xCount,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.SelectionGrid(selected,texts,xCount,options);
}
public static int SelectionGrid( int selected, UnityEngine.Texture[] images, int xCount,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.SelectionGrid(selected,images,xCount,options);
}
public static int SelectionGrid( int selected, UnityEngine.GUIContent[] content, int xCount,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.SelectionGrid(selected,content,xCount,options);
}
public static int SelectionGrid( int selected, System.String[] texts, int xCount, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.SelectionGrid(selected,texts,xCount,style,options);
}
public static int SelectionGrid( int selected, UnityEngine.Texture[] images, int xCount, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.SelectionGrid(selected,images,xCount,style,options);
}
public static int SelectionGrid( int selected, UnityEngine.GUIContent[] contents, int xCount, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.SelectionGrid(selected,contents,xCount,style,options);
}
public static float HorizontalSlider( float value, float leftValue, float rightValue,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.HorizontalSlider(value,leftValue,rightValue,options);
}
public static float HorizontalSlider( float value, float leftValue, float rightValue, UnityEngine.GUIStyle slider, UnityEngine.GUIStyle thumb,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.HorizontalSlider(value,leftValue,rightValue,slider,thumb,options);
}
public static float VerticalSlider( float value, float leftValue, float rightValue,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.VerticalSlider(value,leftValue,rightValue,options);
}
public static float VerticalSlider( float value, float leftValue, float rightValue, UnityEngine.GUIStyle slider, UnityEngine.GUIStyle thumb,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.VerticalSlider(value,leftValue,rightValue,slider,thumb,options);
}
public static float HorizontalScrollbar( float value, float size, float leftValue, float rightValue,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.HorizontalScrollbar(value,size,leftValue,rightValue,options);
}
public static float HorizontalScrollbar( float value, float size, float leftValue, float rightValue, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.HorizontalScrollbar(value,size,leftValue,rightValue,style,options);
}
public static float VerticalScrollbar( float value, float size, float topValue, float bottomValue,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.VerticalScrollbar(value,size,topValue,bottomValue,options);
}
public static float VerticalScrollbar( float value, float size, float topValue, float bottomValue, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.VerticalScrollbar(value,size,topValue,bottomValue,style,options);
}
public static void Space( float pixels)
{
  UnityEngine.GUILayout.Space(pixels);
}
public static void FlexibleSpace()
{
  UnityEngine.GUILayout.FlexibleSpace();
}
public static void BeginHorizontal(params UnityEngine.GUILayoutOption[] options)
{
  UnityEngine.GUILayout.BeginHorizontal(options);
}
public static void BeginHorizontal( UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
  UnityEngine.GUILayout.BeginHorizontal(style,options);
}
public static void BeginHorizontal( System.String text, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
  UnityEngine.GUILayout.BeginHorizontal(text,style,options);
}
public static void BeginHorizontal( UnityEngine.Texture image, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
  UnityEngine.GUILayout.BeginHorizontal(image,style,options);
}
public static void BeginHorizontal( UnityEngine.GUIContent content, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
  UnityEngine.GUILayout.BeginHorizontal(content,style,options);
}
public static void EndHorizontal()
{
  UnityEngine.GUILayout.EndHorizontal();
}
public static void BeginVertical(params UnityEngine.GUILayoutOption[] options)
{
  UnityEngine.GUILayout.BeginVertical(options);
}
public static void BeginVertical( UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
  UnityEngine.GUILayout.BeginVertical(style,options);
}
public static void BeginVertical( System.String text, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
  UnityEngine.GUILayout.BeginVertical(text,style,options);
}
public static void BeginVertical( UnityEngine.Texture image, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
  UnityEngine.GUILayout.BeginVertical(image,style,options);
}
public static void BeginVertical( UnityEngine.GUIContent content, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
  UnityEngine.GUILayout.BeginVertical(content,style,options);
}
public static void EndVertical()
{
  UnityEngine.GUILayout.EndVertical();
}
public static void BeginArea( UnityEngine.Rect screenRect)
{
  UnityEngine.GUILayout.BeginArea(screenRect);
}
public static void BeginArea( UnityEngine.Rect screenRect, System.String text)
{
  UnityEngine.GUILayout.BeginArea(screenRect,text);
}
public static void BeginArea( UnityEngine.Rect screenRect, UnityEngine.Texture image)
{
  UnityEngine.GUILayout.BeginArea(screenRect,image);
}
public static void BeginArea( UnityEngine.Rect screenRect, UnityEngine.GUIContent content)
{
  UnityEngine.GUILayout.BeginArea(screenRect,content);
}
public static void BeginArea( UnityEngine.Rect screenRect, UnityEngine.GUIStyle style)
{
  UnityEngine.GUILayout.BeginArea(screenRect,style);
}
public static void BeginArea( UnityEngine.Rect screenRect, System.String text, UnityEngine.GUIStyle style)
{
  UnityEngine.GUILayout.BeginArea(screenRect,text,style);
}
public static void BeginArea( UnityEngine.Rect screenRect, UnityEngine.Texture image, UnityEngine.GUIStyle style)
{
  UnityEngine.GUILayout.BeginArea(screenRect,image,style);
}
public static void BeginArea( UnityEngine.Rect screenRect, UnityEngine.GUIContent content, UnityEngine.GUIStyle style)
{
  UnityEngine.GUILayout.BeginArea(screenRect,content,style);
}
public static void EndArea()
{
  UnityEngine.GUILayout.EndArea();
}
public static UnityEngine.Vector2 BeginScrollView( UnityEngine.Vector2 scrollPosition,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.BeginScrollView(scrollPosition,options);
}
public static UnityEngine.Vector2 BeginScrollView( UnityEngine.Vector2 scrollPosition, bool alwaysShowHorizontal, bool alwaysShowVertical,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.BeginScrollView(scrollPosition,alwaysShowHorizontal,alwaysShowVertical,options);
}
public static UnityEngine.Vector2 BeginScrollView( UnityEngine.Vector2 scrollPosition, UnityEngine.GUIStyle horizontalScrollbar, UnityEngine.GUIStyle verticalScrollbar,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.BeginScrollView(scrollPosition,horizontalScrollbar,verticalScrollbar,options);
}
public static UnityEngine.Vector2 BeginScrollView( UnityEngine.Vector2 scrollPosition, UnityEngine.GUIStyle style)
{
 return  UnityEngine.GUILayout.BeginScrollView(scrollPosition,style);
}
public static UnityEngine.Vector2 BeginScrollView( UnityEngine.Vector2 scrollPosition, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.BeginScrollView(scrollPosition,style,options);
}
public static UnityEngine.Vector2 BeginScrollView( UnityEngine.Vector2 scrollPosition, bool alwaysShowHorizontal, bool alwaysShowVertical, UnityEngine.GUIStyle horizontalScrollbar, UnityEngine.GUIStyle verticalScrollbar,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.BeginScrollView(scrollPosition,alwaysShowHorizontal,alwaysShowVertical,horizontalScrollbar,verticalScrollbar,options);
}
public static UnityEngine.Vector2 BeginScrollView( UnityEngine.Vector2 scrollPosition, bool alwaysShowHorizontal, bool alwaysShowVertical, UnityEngine.GUIStyle horizontalScrollbar, UnityEngine.GUIStyle verticalScrollbar, UnityEngine.GUIStyle background,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.BeginScrollView(scrollPosition,alwaysShowHorizontal,alwaysShowVertical,horizontalScrollbar,verticalScrollbar,background,options);
}
public static void EndScrollView()
{
  UnityEngine.GUILayout.EndScrollView();
}
public static UnityEngine.Rect Window( int id, UnityEngine.Rect screenRect, UnityEngine.GUI.WindowFunction func, System.String text,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.Window(id,screenRect,func,text,options);
}
public static UnityEngine.Rect Window( int id, UnityEngine.Rect screenRect, UnityEngine.GUI.WindowFunction func, UnityEngine.Texture image,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.Window(id,screenRect,func,image,options);
}
public static UnityEngine.Rect Window( int id, UnityEngine.Rect screenRect, UnityEngine.GUI.WindowFunction func, UnityEngine.GUIContent content,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.Window(id,screenRect,func,content,options);
}
public static UnityEngine.Rect Window( int id, UnityEngine.Rect screenRect, UnityEngine.GUI.WindowFunction func, System.String text, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.Window(id,screenRect,func,text,style,options);
}
public static UnityEngine.Rect Window( int id, UnityEngine.Rect screenRect, UnityEngine.GUI.WindowFunction func, UnityEngine.Texture image, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.Window(id,screenRect,func,image,style,options);
}
public static UnityEngine.Rect Window( int id, UnityEngine.Rect screenRect, UnityEngine.GUI.WindowFunction func, UnityEngine.GUIContent content, UnityEngine.GUIStyle style,params UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.Window(id,screenRect,func,content,style,options);
}
public static UnityEngine.GUILayoutOption Width( float width)
{
 return  UnityEngine.GUILayout.Width(width);
}
public static UnityEngine.GUILayoutOption MinWidth( float minWidth)
{
 return  UnityEngine.GUILayout.MinWidth(minWidth);
}
public static UnityEngine.GUILayoutOption MaxWidth( float maxWidth)
{
 return  UnityEngine.GUILayout.MaxWidth(maxWidth);
}
public static UnityEngine.GUILayoutOption Height( float height)
{
 return  UnityEngine.GUILayout.Height(height);
}
public static UnityEngine.GUILayoutOption MinHeight( float minHeight)
{
 return  UnityEngine.GUILayout.MinHeight(minHeight);
}
public static UnityEngine.GUILayoutOption MaxHeight( float maxHeight)
{
 return  UnityEngine.GUILayout.MaxHeight(maxHeight);
}
public static UnityEngine.GUILayoutOption ExpandWidth( bool expand)
{
 return  UnityEngine.GUILayout.ExpandWidth(expand);
}
public static UnityEngine.GUILayoutOption ExpandHeight( bool expand)
{
 return  UnityEngine.GUILayout.ExpandHeight(expand);
}

	}
#pragma warning restore 618
}