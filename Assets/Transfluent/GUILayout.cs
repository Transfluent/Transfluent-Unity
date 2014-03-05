using UnityEngine;

//wrapper around unity's gui, except to grab text as quickly as possbile and spit it into an internal db
//http://docs.unity3d.com/Documentation/ScriptReference/GUI.html
namespace transfluent.guiwrapper
{

	public partial class GUILayout
	{

public static System.Void Label(UnityEngine.Texture image,UnityEngine.GUILayoutOption[] options)
{
  UnityEngine.GUILayout.Label(image,options);
}
public static System.Void Label(System.String text,UnityEngine.GUILayoutOption[] options=null)
{
  UnityEngine.GUILayout.Label(text,options);
}
public static System.Void Label(UnityEngine.GUIContent content,UnityEngine.GUILayoutOption[] options)
{
  UnityEngine.GUILayout.Label(content,options);
}
public static System.Void Label(UnityEngine.Texture image,UnityEngine.GUIStyle style,UnityEngine.GUILayoutOption[] options)
{
  UnityEngine.GUILayout.Label(image,style,options);
}
public static System.Void Label(System.String text,UnityEngine.GUIStyle style,UnityEngine.GUILayoutOption[] options)
{
  UnityEngine.GUILayout.Label(text,style,options);
}
public static System.Void Label(UnityEngine.GUIContent content,UnityEngine.GUIStyle style,UnityEngine.GUILayoutOption[] options)
{
  UnityEngine.GUILayout.Label(content,style,options);
}
public static System.Void Box(UnityEngine.Texture image,UnityEngine.GUILayoutOption[] options)
{
  UnityEngine.GUILayout.Box(image,options);
}
public static System.Void Box(System.String text,UnityEngine.GUILayoutOption[] options)
{
  UnityEngine.GUILayout.Box(text,options);
}
public static System.Void Box(UnityEngine.GUIContent content,UnityEngine.GUILayoutOption[] options)
{
  UnityEngine.GUILayout.Box(content,options);
}
public static System.Void Box(UnityEngine.Texture image,UnityEngine.GUIStyle style,UnityEngine.GUILayoutOption[] options)
{
  UnityEngine.GUILayout.Box(image,style,options);
}
public static System.Void Box(System.String text,UnityEngine.GUIStyle style,UnityEngine.GUILayoutOption[] options)
{
  UnityEngine.GUILayout.Box(text,style,options);
}
public static System.Void Box(UnityEngine.GUIContent content,UnityEngine.GUIStyle style,UnityEngine.GUILayoutOption[] options)
{
  UnityEngine.GUILayout.Box(content,style,options);
}
public static System.Boolean Button(UnityEngine.Texture image,UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.Button(image,options);
}
public static System.Boolean Button(System.String text,UnityEngine.GUILayoutOption[] options=null)
{
 return  UnityEngine.GUILayout.Button(text,options);
}
public static System.Boolean Button(UnityEngine.GUIContent content,UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.Button(content,options);
}
public static System.Boolean Button(UnityEngine.Texture image,UnityEngine.GUIStyle style,UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.Button(image,style,options);
}
public static System.Boolean Button(System.String text,UnityEngine.GUIStyle style,UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.Button(text,style,options);
}
public static System.Boolean Button(UnityEngine.GUIContent content,UnityEngine.GUIStyle style,UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.Button(content,style,options);
}
public static System.Boolean RepeatButton(UnityEngine.Texture image,UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.RepeatButton(image,options);
}
public static System.Boolean RepeatButton(System.String text,UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.RepeatButton(text,options);
}
public static System.Boolean RepeatButton(UnityEngine.GUIContent content,UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.RepeatButton(content,options);
}
public static System.Boolean RepeatButton(UnityEngine.Texture image,UnityEngine.GUIStyle style,UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.RepeatButton(image,style,options);
}
public static System.Boolean RepeatButton(System.String text,UnityEngine.GUIStyle style,UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.RepeatButton(text,style,options);
}
public static System.Boolean RepeatButton(UnityEngine.GUIContent content,UnityEngine.GUIStyle style,UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.RepeatButton(content,style,options);
}
public static System.String TextField(System.String text,UnityEngine.GUILayoutOption[] options=null)
{
 return  UnityEngine.GUILayout.TextField(text,options);
}
public static System.String TextField(System.String text,System.Int32 maxLength,UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.TextField(text,maxLength,options);
}
public static System.String TextField(System.String text,UnityEngine.GUIStyle style,UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.TextField(text,style,options);
}
public static System.String TextField(System.String text,System.Int32 maxLength,UnityEngine.GUIStyle style,UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.TextField(text,maxLength,style,options);
}
public static System.String PasswordField(System.String password,System.Char maskChar,UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.PasswordField(password,maskChar,options);
}
public static System.String PasswordField(System.String password,System.Char maskChar,System.Int32 maxLength,UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.PasswordField(password,maskChar,maxLength,options);
}
public static System.String PasswordField(System.String password,System.Char maskChar,UnityEngine.GUIStyle style,UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.PasswordField(password,maskChar,style,options);
}
public static System.String PasswordField(System.String password,System.Char maskChar,System.Int32 maxLength,UnityEngine.GUIStyle style,UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.PasswordField(password,maskChar,maxLength,style,options);
}
public static System.String TextArea(System.String text,UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.TextArea(text,options);
}
public static System.String TextArea(System.String text,System.Int32 maxLength,UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.TextArea(text,maxLength,options);
}
public static System.String TextArea(System.String text,UnityEngine.GUIStyle style,UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.TextArea(text,style,options);
}
public static System.String TextArea(System.String text,System.Int32 maxLength,UnityEngine.GUIStyle style,UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.TextArea(text,maxLength,style,options);
}
public static System.Boolean Toggle(System.Boolean value,UnityEngine.Texture image,UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.Toggle(value,image,options);
}
public static System.Boolean Toggle(System.Boolean value,System.String text,UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.Toggle(value,text,options);
}
public static System.Boolean Toggle(System.Boolean value,UnityEngine.GUIContent content,UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.Toggle(value,content,options);
}
public static System.Boolean Toggle(System.Boolean value,UnityEngine.Texture image,UnityEngine.GUIStyle style,UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.Toggle(value,image,style,options);
}
public static System.Boolean Toggle(System.Boolean value,System.String text,UnityEngine.GUIStyle style,UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.Toggle(value,text,style,options);
}
public static System.Boolean Toggle(System.Boolean value,UnityEngine.GUIContent content,UnityEngine.GUIStyle style,UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.Toggle(value,content,style,options);
}
public static System.Int32 Toolbar(System.Int32 selected,System.String[] texts,UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.Toolbar(selected,texts,options);
}
public static System.Int32 Toolbar(System.Int32 selected,UnityEngine.Texture[] images,UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.Toolbar(selected,images,options);
}
public static System.Int32 Toolbar(System.Int32 selected,UnityEngine.GUIContent[] content,UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.Toolbar(selected,content,options);
}
public static System.Int32 Toolbar(System.Int32 selected,System.String[] texts,UnityEngine.GUIStyle style,UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.Toolbar(selected,texts,style,options);
}
public static System.Int32 Toolbar(System.Int32 selected,UnityEngine.Texture[] images,UnityEngine.GUIStyle style,UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.Toolbar(selected,images,style,options);
}
public static System.Int32 Toolbar(System.Int32 selected,UnityEngine.GUIContent[] contents,UnityEngine.GUIStyle style,UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.Toolbar(selected,contents,style,options);
}
public static System.Int32 SelectionGrid(System.Int32 selected,System.String[] texts,System.Int32 xCount,UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.SelectionGrid(selected,texts,xCount,options);
}
public static System.Int32 SelectionGrid(System.Int32 selected,UnityEngine.Texture[] images,System.Int32 xCount,UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.SelectionGrid(selected,images,xCount,options);
}
public static System.Int32 SelectionGrid(System.Int32 selected,UnityEngine.GUIContent[] content,System.Int32 xCount,UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.SelectionGrid(selected,content,xCount,options);
}
public static System.Int32 SelectionGrid(System.Int32 selected,System.String[] texts,System.Int32 xCount,UnityEngine.GUIStyle style,UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.SelectionGrid(selected,texts,xCount,style,options);
}
public static System.Int32 SelectionGrid(System.Int32 selected,UnityEngine.Texture[] images,System.Int32 xCount,UnityEngine.GUIStyle style,UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.SelectionGrid(selected,images,xCount,style,options);
}
public static System.Int32 SelectionGrid(System.Int32 selected,UnityEngine.GUIContent[] contents,System.Int32 xCount,UnityEngine.GUIStyle style,UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.SelectionGrid(selected,contents,xCount,style,options);
}
public static System.Single HorizontalSlider(System.Single value,System.Single leftValue,System.Single rightValue,UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.HorizontalSlider(value,leftValue,rightValue,options);
}
public static System.Single HorizontalSlider(System.Single value,System.Single leftValue,System.Single rightValue,UnityEngine.GUIStyle slider,UnityEngine.GUIStyle thumb,UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.HorizontalSlider(value,leftValue,rightValue,slider,thumb,options);
}
public static System.Single VerticalSlider(System.Single value,System.Single leftValue,System.Single rightValue,UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.VerticalSlider(value,leftValue,rightValue,options);
}
public static System.Single VerticalSlider(System.Single value,System.Single leftValue,System.Single rightValue,UnityEngine.GUIStyle slider,UnityEngine.GUIStyle thumb,UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.VerticalSlider(value,leftValue,rightValue,slider,thumb,options);
}
public static System.Single HorizontalScrollbar(System.Single value,System.Single size,System.Single leftValue,System.Single rightValue,UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.HorizontalScrollbar(value,size,leftValue,rightValue,options);
}
public static System.Single HorizontalScrollbar(System.Single value,System.Single size,System.Single leftValue,System.Single rightValue,UnityEngine.GUIStyle style,UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.HorizontalScrollbar(value,size,leftValue,rightValue,style,options);
}
public static System.Single VerticalScrollbar(System.Single value,System.Single size,System.Single topValue,System.Single bottomValue,UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.VerticalScrollbar(value,size,topValue,bottomValue,options);
}
public static System.Single VerticalScrollbar(System.Single value,System.Single size,System.Single topValue,System.Single bottomValue,UnityEngine.GUIStyle style,UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.VerticalScrollbar(value,size,topValue,bottomValue,style,options);
}
public static System.Void Space(System.Single pixels)
{
  UnityEngine.GUILayout.Space(pixels);
}
public static System.Void FlexibleSpace()
{
  UnityEngine.GUILayout.FlexibleSpace();
}
public static System.Void BeginHorizontal(UnityEngine.GUILayoutOption[] options)
{
  UnityEngine.GUILayout.BeginHorizontal(options);
}
public static System.Void BeginHorizontal(UnityEngine.GUIStyle style,UnityEngine.GUILayoutOption[] options)
{
  UnityEngine.GUILayout.BeginHorizontal(style,options);
}
public static System.Void BeginHorizontal(System.String text,UnityEngine.GUIStyle style,UnityEngine.GUILayoutOption[] options)
{
  UnityEngine.GUILayout.BeginHorizontal(text,style,options);
}
public static System.Void BeginHorizontal(UnityEngine.Texture image,UnityEngine.GUIStyle style,UnityEngine.GUILayoutOption[] options)
{
  UnityEngine.GUILayout.BeginHorizontal(image,style,options);
}
public static System.Void BeginHorizontal(UnityEngine.GUIContent content,UnityEngine.GUIStyle style,UnityEngine.GUILayoutOption[] options)
{
  UnityEngine.GUILayout.BeginHorizontal(content,style,options);
}
public static System.Void EndHorizontal()
{
  UnityEngine.GUILayout.EndHorizontal();
}
public static System.Void BeginVertical(UnityEngine.GUILayoutOption[] options)
{
  UnityEngine.GUILayout.BeginVertical(options);
}
public static System.Void BeginVertical(UnityEngine.GUIStyle style,UnityEngine.GUILayoutOption[] options)
{
  UnityEngine.GUILayout.BeginVertical(style,options);
}
public static System.Void BeginVertical(System.String text,UnityEngine.GUIStyle style,UnityEngine.GUILayoutOption[] options)
{
  UnityEngine.GUILayout.BeginVertical(text,style,options);
}
public static System.Void BeginVertical(UnityEngine.Texture image,UnityEngine.GUIStyle style,UnityEngine.GUILayoutOption[] options)
{
  UnityEngine.GUILayout.BeginVertical(image,style,options);
}
public static System.Void BeginVertical(UnityEngine.GUIContent content,UnityEngine.GUIStyle style,UnityEngine.GUILayoutOption[] options)
{
  UnityEngine.GUILayout.BeginVertical(content,style,options);
}
public static System.Void EndVertical()
{
  UnityEngine.GUILayout.EndVertical();
}
public static System.Void BeginArea(UnityEngine.Rect screenRect)
{
  UnityEngine.GUILayout.BeginArea(screenRect);
}
public static System.Void BeginArea(UnityEngine.Rect screenRect,System.String text)
{
  UnityEngine.GUILayout.BeginArea(screenRect,text);
}
public static System.Void BeginArea(UnityEngine.Rect screenRect,UnityEngine.Texture image)
{
  UnityEngine.GUILayout.BeginArea(screenRect,image);
}
public static System.Void BeginArea(UnityEngine.Rect screenRect,UnityEngine.GUIContent content)
{
  UnityEngine.GUILayout.BeginArea(screenRect,content);
}
public static System.Void BeginArea(UnityEngine.Rect screenRect,UnityEngine.GUIStyle style)
{
  UnityEngine.GUILayout.BeginArea(screenRect,style);
}
public static System.Void BeginArea(UnityEngine.Rect screenRect,System.String text,UnityEngine.GUIStyle style)
{
  UnityEngine.GUILayout.BeginArea(screenRect,text,style);
}
public static System.Void BeginArea(UnityEngine.Rect screenRect,UnityEngine.Texture image,UnityEngine.GUIStyle style)
{
  UnityEngine.GUILayout.BeginArea(screenRect,image,style);
}
public static System.Void BeginArea(UnityEngine.Rect screenRect,UnityEngine.GUIContent content,UnityEngine.GUIStyle style)
{
  UnityEngine.GUILayout.BeginArea(screenRect,content,style);
}
public static System.Void EndArea()
{
  UnityEngine.GUILayout.EndArea();
}
public static UnityEngine.Vector2 BeginScrollView(UnityEngine.Vector2 scrollPosition,UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.BeginScrollView(scrollPosition,options);
}
public static UnityEngine.Vector2 BeginScrollView(UnityEngine.Vector2 scrollPosition,System.Boolean alwaysShowHorizontal,System.Boolean alwaysShowVertical,UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.BeginScrollView(scrollPosition,alwaysShowHorizontal,alwaysShowVertical,options);
}
public static UnityEngine.Vector2 BeginScrollView(UnityEngine.Vector2 scrollPosition,UnityEngine.GUIStyle horizontalScrollbar,UnityEngine.GUIStyle verticalScrollbar,UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.BeginScrollView(scrollPosition,horizontalScrollbar,verticalScrollbar,options);
}
public static UnityEngine.Vector2 BeginScrollView(UnityEngine.Vector2 scrollPosition,UnityEngine.GUIStyle style)
{
 return  UnityEngine.GUILayout.BeginScrollView(scrollPosition,style);
}
public static UnityEngine.Vector2 BeginScrollView(UnityEngine.Vector2 scrollPosition,UnityEngine.GUIStyle style,UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.BeginScrollView(scrollPosition,style,options);
}
public static UnityEngine.Vector2 BeginScrollView(UnityEngine.Vector2 scrollPosition,System.Boolean alwaysShowHorizontal,System.Boolean alwaysShowVertical,UnityEngine.GUIStyle horizontalScrollbar,UnityEngine.GUIStyle verticalScrollbar,UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.BeginScrollView(scrollPosition,alwaysShowHorizontal,alwaysShowVertical,horizontalScrollbar,verticalScrollbar,options);
}
public static UnityEngine.Vector2 BeginScrollView(UnityEngine.Vector2 scrollPosition,System.Boolean alwaysShowHorizontal,System.Boolean alwaysShowVertical,UnityEngine.GUIStyle horizontalScrollbar,UnityEngine.GUIStyle verticalScrollbar,UnityEngine.GUIStyle background,UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.BeginScrollView(scrollPosition,alwaysShowHorizontal,alwaysShowVertical,horizontalScrollbar,verticalScrollbar,background,options);
}
public static System.Void EndScrollView()
{
  UnityEngine.GUILayout.EndScrollView();
}
public static UnityEngine.Rect Window(System.Int32 id,UnityEngine.Rect screenRect,UnityEngine.GUI.WindowFunction func,System.String text,UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.Window(id,screenRect,func,text,options);
}
public static UnityEngine.Rect Window(System.Int32 id,UnityEngine.Rect screenRect,UnityEngine.GUI.WindowFunction func,UnityEngine.Texture image,UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.Window(id,screenRect,func,image,options);
}
public static UnityEngine.Rect Window(System.Int32 id,UnityEngine.Rect screenRect,UnityEngine.GUI.WindowFunction func,UnityEngine.GUIContent content,UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.Window(id,screenRect,func,content,options);
}
public static UnityEngine.Rect Window(System.Int32 id,UnityEngine.Rect screenRect,UnityEngine.GUI.WindowFunction func,System.String text,UnityEngine.GUIStyle style,UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.Window(id,screenRect,func,text,style,options);
}
public static UnityEngine.Rect Window(System.Int32 id,UnityEngine.Rect screenRect,UnityEngine.GUI.WindowFunction func,UnityEngine.Texture image,UnityEngine.GUIStyle style,UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.Window(id,screenRect,func,image,style,options);
}
public static UnityEngine.Rect Window(System.Int32 id,UnityEngine.Rect screenRect,UnityEngine.GUI.WindowFunction func,UnityEngine.GUIContent content,UnityEngine.GUIStyle style,UnityEngine.GUILayoutOption[] options)
{
 return  UnityEngine.GUILayout.Window(id,screenRect,func,content,style,options);
}
public static UnityEngine.GUILayoutOption Width(System.Single width)
{
 return  UnityEngine.GUILayout.Width(width);
}
public static UnityEngine.GUILayoutOption MinWidth(System.Single minWidth)
{
 return  UnityEngine.GUILayout.MinWidth(minWidth);
}
public static UnityEngine.GUILayoutOption MaxWidth(System.Single maxWidth)
{
 return  UnityEngine.GUILayout.MaxWidth(maxWidth);
}
public static UnityEngine.GUILayoutOption Height(System.Single height)
{
 return  UnityEngine.GUILayout.Height(height);
}
public static UnityEngine.GUILayoutOption MinHeight(System.Single minHeight)
{
 return  UnityEngine.GUILayout.MinHeight(minHeight);
}
public static UnityEngine.GUILayoutOption MaxHeight(System.Single maxHeight)
{
 return  UnityEngine.GUILayout.MaxHeight(maxHeight);
}
public static UnityEngine.GUILayoutOption ExpandWidth(System.Boolean expand)
{
 return  UnityEngine.GUILayout.ExpandWidth(expand);
}
public static UnityEngine.GUILayoutOption ExpandHeight(System.Boolean expand)
{
 return  UnityEngine.GUILayout.ExpandHeight(expand);
}

	}
}