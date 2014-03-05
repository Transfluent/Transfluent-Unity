using System;
using UnityEngine;



//wrapper around unity's gui, except to grab text as quickly as possbile and spit it into an internal db

//http://docs.unity3d.com/Documentation/ScriptReference/GUI.html
#if false
namespace transfluent

{



	public partial class GUI

	{
		System.Void set_skin(UnityEngine.GUISkin value)
		{
			UnityEngine.GUI.skin = value;
			//return UnityEngine.GUI.set_skin(UnityEngine.GUISkin value);
		}
		/*
		 * public static GUISkin skin
		{
			get { return UnityEngine.GUI.skin; }
			set { UnityEngine.GUI.skin = value; }
		}
		 * public static Rect ModalWindow(int id, Rect clientRect, UnityEngine.GUI.WindowFunction func, string text)
		{
			return UnityEngine.GUI.ModalWindow(id, clientRect, func,text);
		}
		 */
		UnityEngine.Rect ModalWindow(System.Int32 id,UnityEngine.Rect clientRect,UnityEngine.GUI.WindowFunction func,System.String text)
{
 return UnityEngine.GUI.ModalWindow(id,clientRect, func, text);
}
	}

}

#endif //false
using UnityEngine;



//wrapper around unity's gui, except to grab text as quickly as possbile and spit it into an internal db

//http://docs.unity3d.com/Documentation/ScriptReference/GUI.html

namespace transfluent

{



	public partial class GUI

	{
System.Void Label(UnityEngine.Rect position,System.String text)
{
  UnityEngine.GUI.Label(position,text);
}
System.Void Label(UnityEngine.Rect position,UnityEngine.Texture image)
{
  UnityEngine.GUI.Label(position,image);
}
System.Void Label(UnityEngine.Rect position,UnityEngine.GUIContent content)
{
  UnityEngine.GUI.Label(position,content);
}
System.Void Label(UnityEngine.Rect position,System.String text,UnityEngine.GUIStyle style)
{
  UnityEngine.GUI.Label(position,text,style);
}
System.Void Label(UnityEngine.Rect position,UnityEngine.Texture image,UnityEngine.GUIStyle style)
{
  UnityEngine.GUI.Label(position,image,style);
}
System.Void Label(UnityEngine.Rect position,UnityEngine.GUIContent content,UnityEngine.GUIStyle style)
{
  UnityEngine.GUI.Label(position,content,style);
}
System.Void DrawTexture(UnityEngine.Rect position,UnityEngine.Texture image,UnityEngine.ScaleMode scaleMode,System.Boolean alphaBlend)
{
  UnityEngine.GUI.DrawTexture(position,image,scaleMode,alphaBlend);
}
System.Void DrawTexture(UnityEngine.Rect position,UnityEngine.Texture image,UnityEngine.ScaleMode scaleMode)
{
  UnityEngine.GUI.DrawTexture(position,image,scaleMode);
}
System.Void DrawTexture(UnityEngine.Rect position,UnityEngine.Texture image)
{
  UnityEngine.GUI.DrawTexture(position,image);
}
System.Void DrawTexture(UnityEngine.Rect position,UnityEngine.Texture image,UnityEngine.ScaleMode scaleMode,System.Boolean alphaBlend,System.Single imageAspect)
{
  UnityEngine.GUI.DrawTexture(position,image,scaleMode,alphaBlend,imageAspect);
}
System.Void DrawTextureWithTexCoords(UnityEngine.Rect position,UnityEngine.Texture image,UnityEngine.Rect texCoords)
{
  UnityEngine.GUI.DrawTextureWithTexCoords(position,image,texCoords);
}
System.Void DrawTextureWithTexCoords(UnityEngine.Rect position,UnityEngine.Texture image,UnityEngine.Rect texCoords,System.Boolean alphaBlend)
{
  UnityEngine.GUI.DrawTextureWithTexCoords(position,image,texCoords,alphaBlend);
}
System.Void Box(UnityEngine.Rect position,System.String text)
{
  UnityEngine.GUI.Box(position,text);
}
System.Void Box(UnityEngine.Rect position,UnityEngine.Texture image)
{
  UnityEngine.GUI.Box(position,image);
}
System.Void Box(UnityEngine.Rect position,UnityEngine.GUIContent content)
{
  UnityEngine.GUI.Box(position,content);
}
System.Void Box(UnityEngine.Rect position,System.String text,UnityEngine.GUIStyle style)
{
  UnityEngine.GUI.Box(position,text,style);
}
System.Void Box(UnityEngine.Rect position,UnityEngine.Texture image,UnityEngine.GUIStyle style)
{
  UnityEngine.GUI.Box(position,image,style);
}
System.Void Box(UnityEngine.Rect position,UnityEngine.GUIContent content,UnityEngine.GUIStyle style)
{
  UnityEngine.GUI.Box(position,content,style);
}
System.Boolean Button(UnityEngine.Rect position,System.String text)
{
 return  UnityEngine.GUI.Button(position,text);
}
System.Boolean Button(UnityEngine.Rect position,UnityEngine.Texture image)
{
 return  UnityEngine.GUI.Button(position,image);
}
System.Boolean Button(UnityEngine.Rect position,UnityEngine.GUIContent content)
{
 return  UnityEngine.GUI.Button(position,content);
}
System.Boolean Button(UnityEngine.Rect position,System.String text,UnityEngine.GUIStyle style)
{
 return  UnityEngine.GUI.Button(position,text,style);
}
System.Boolean Button(UnityEngine.Rect position,UnityEngine.Texture image,UnityEngine.GUIStyle style)
{
 return  UnityEngine.GUI.Button(position,image,style);
}
System.Boolean Button(UnityEngine.Rect position,UnityEngine.GUIContent content,UnityEngine.GUIStyle style)
{
 return  UnityEngine.GUI.Button(position,content,style);
}
System.Boolean RepeatButton(UnityEngine.Rect position,System.String text)
{
 return  UnityEngine.GUI.RepeatButton(position,text);
}
System.Boolean RepeatButton(UnityEngine.Rect position,UnityEngine.Texture image)
{
 return  UnityEngine.GUI.RepeatButton(position,image);
}
System.Boolean RepeatButton(UnityEngine.Rect position,UnityEngine.GUIContent content)
{
 return  UnityEngine.GUI.RepeatButton(position,content);
}
System.Boolean RepeatButton(UnityEngine.Rect position,System.String text,UnityEngine.GUIStyle style)
{
 return  UnityEngine.GUI.RepeatButton(position,text,style);
}
System.Boolean RepeatButton(UnityEngine.Rect position,UnityEngine.Texture image,UnityEngine.GUIStyle style)
{
 return  UnityEngine.GUI.RepeatButton(position,image,style);
}
System.Boolean RepeatButton(UnityEngine.Rect position,UnityEngine.GUIContent content,UnityEngine.GUIStyle style)
{
 return  UnityEngine.GUI.RepeatButton(position,content,style);
}
System.String TextField(UnityEngine.Rect position,System.String text)
{
 return  UnityEngine.GUI.TextField(position,text);
}
System.String TextField(UnityEngine.Rect position,System.String text,System.Int32 maxLength)
{
 return  UnityEngine.GUI.TextField(position,text,maxLength);
}
System.String TextField(UnityEngine.Rect position,System.String text,UnityEngine.GUIStyle style)
{
 return  UnityEngine.GUI.TextField(position,text,style);
}
System.String TextField(UnityEngine.Rect position,System.String text,System.Int32 maxLength,UnityEngine.GUIStyle style)
{
 return  UnityEngine.GUI.TextField(position,text,maxLength,style);
}
System.String PasswordField(UnityEngine.Rect position,System.String password,System.Char maskChar)
{
 return  UnityEngine.GUI.PasswordField(position,password,maskChar);
}
System.String PasswordField(UnityEngine.Rect position,System.String password,System.Char maskChar,System.Int32 maxLength)
{
 return  UnityEngine.GUI.PasswordField(position,password,maskChar,maxLength);
}
System.String PasswordField(UnityEngine.Rect position,System.String password,System.Char maskChar,UnityEngine.GUIStyle style)
{
 return  UnityEngine.GUI.PasswordField(position,password,maskChar,style);
}
System.String PasswordField(UnityEngine.Rect position,System.String password,System.Char maskChar,System.Int32 maxLength,UnityEngine.GUIStyle style)
{
 return  UnityEngine.GUI.PasswordField(position,password,maskChar,maxLength,style);
}
System.String TextArea(UnityEngine.Rect position,System.String text)
{
 return  UnityEngine.GUI.TextArea(position,text);
}
System.String TextArea(UnityEngine.Rect position,System.String text,System.Int32 maxLength)
{
 return  UnityEngine.GUI.TextArea(position,text,maxLength);
}
System.String TextArea(UnityEngine.Rect position,System.String text,UnityEngine.GUIStyle style)
{
 return  UnityEngine.GUI.TextArea(position,text,style);
}
System.String TextArea(UnityEngine.Rect position,System.String text,System.Int32 maxLength,UnityEngine.GUIStyle style)
{
 return  UnityEngine.GUI.TextArea(position,text,maxLength,style);
}
System.Void SetNextControlName(System.String name)
{
  UnityEngine.GUI.SetNextControlName(name);
}
System.String GetNameOfFocusedControl()
{
 return  UnityEngine.GUI.GetNameOfFocusedControl();
}
System.Void FocusControl(System.String name)
{
  UnityEngine.GUI.FocusControl(name);
}
System.Boolean Toggle(UnityEngine.Rect position,System.Boolean value,System.String text)
{
 return  UnityEngine.GUI.Toggle(position,value,text);
}
System.Boolean Toggle(UnityEngine.Rect position,System.Boolean value,UnityEngine.Texture image)
{
 return  UnityEngine.GUI.Toggle(position,value,image);
}
System.Boolean Toggle(UnityEngine.Rect position,System.Boolean value,UnityEngine.GUIContent content)
{
 return  UnityEngine.GUI.Toggle(position,value,content);
}
System.Boolean Toggle(UnityEngine.Rect position,System.Boolean value,System.String text,UnityEngine.GUIStyle style)
{
 return  UnityEngine.GUI.Toggle(position,value,text,style);
}
System.Boolean Toggle(UnityEngine.Rect position,System.Boolean value,UnityEngine.Texture image,UnityEngine.GUIStyle style)
{
 return  UnityEngine.GUI.Toggle(position,value,image,style);
}
System.Boolean Toggle(UnityEngine.Rect position,System.Boolean value,UnityEngine.GUIContent content,UnityEngine.GUIStyle style)
{
 return  UnityEngine.GUI.Toggle(position,value,content,style);
}
System.Int32 Toolbar(UnityEngine.Rect position,System.Int32 selected,System.String[] texts)
{
 return  UnityEngine.GUI.Toolbar(position,selected,texts);
}
System.Int32 Toolbar(UnityEngine.Rect position,System.Int32 selected,UnityEngine.Texture[] images)
{
 return  UnityEngine.GUI.Toolbar(position,selected,images);
}
System.Int32 Toolbar(UnityEngine.Rect position,System.Int32 selected,UnityEngine.GUIContent[] content)
{
 return  UnityEngine.GUI.Toolbar(position,selected,content);
}
System.Int32 Toolbar(UnityEngine.Rect position,System.Int32 selected,System.String[] texts,UnityEngine.GUIStyle style)
{
 return  UnityEngine.GUI.Toolbar(position,selected,texts,style);
}
System.Int32 Toolbar(UnityEngine.Rect position,System.Int32 selected,UnityEngine.Texture[] images,UnityEngine.GUIStyle style)
{
 return  UnityEngine.GUI.Toolbar(position,selected,images,style);
}
System.Int32 Toolbar(UnityEngine.Rect position,System.Int32 selected,UnityEngine.GUIContent[] contents,UnityEngine.GUIStyle style)
{
 return  UnityEngine.GUI.Toolbar(position,selected,contents,style);
}
System.Int32 SelectionGrid(UnityEngine.Rect position,System.Int32 selected,System.String[] texts,System.Int32 xCount)
{
 return  UnityEngine.GUI.SelectionGrid(position,selected,texts,xCount);
}
System.Int32 SelectionGrid(UnityEngine.Rect position,System.Int32 selected,UnityEngine.Texture[] images,System.Int32 xCount)
{
 return  UnityEngine.GUI.SelectionGrid(position,selected,images,xCount);
}
System.Int32 SelectionGrid(UnityEngine.Rect position,System.Int32 selected,UnityEngine.GUIContent[] content,System.Int32 xCount)
{
 return  UnityEngine.GUI.SelectionGrid(position,selected,content,xCount);
}
System.Int32 SelectionGrid(UnityEngine.Rect position,System.Int32 selected,System.String[] texts,System.Int32 xCount,UnityEngine.GUIStyle style)
{
 return  UnityEngine.GUI.SelectionGrid(position,selected,texts,xCount,style);
}
System.Int32 SelectionGrid(UnityEngine.Rect position,System.Int32 selected,UnityEngine.Texture[] images,System.Int32 xCount,UnityEngine.GUIStyle style)
{
 return  UnityEngine.GUI.SelectionGrid(position,selected,images,xCount,style);
}
System.Int32 SelectionGrid(UnityEngine.Rect position,System.Int32 selected,UnityEngine.GUIContent[] contents,System.Int32 xCount,UnityEngine.GUIStyle style)
{
 return  UnityEngine.GUI.SelectionGrid(position,selected,contents,xCount,style);
}
System.Single HorizontalSlider(UnityEngine.Rect position,System.Single value,System.Single leftValue,System.Single rightValue)
{
 return  UnityEngine.GUI.HorizontalSlider(position,value,leftValue,rightValue);
}
System.Single HorizontalSlider(UnityEngine.Rect position,System.Single value,System.Single leftValue,System.Single rightValue,UnityEngine.GUIStyle slider,UnityEngine.GUIStyle thumb)
{
 return  UnityEngine.GUI.HorizontalSlider(position,value,leftValue,rightValue,slider,thumb);
}
System.Single VerticalSlider(UnityEngine.Rect position,System.Single value,System.Single topValue,System.Single bottomValue)
{
 return  UnityEngine.GUI.VerticalSlider(position,value,topValue,bottomValue);
}
System.Single VerticalSlider(UnityEngine.Rect position,System.Single value,System.Single topValue,System.Single bottomValue,UnityEngine.GUIStyle slider,UnityEngine.GUIStyle thumb)
{
 return  UnityEngine.GUI.VerticalSlider(position,value,topValue,bottomValue,slider,thumb);
}
System.Single Slider(UnityEngine.Rect position,System.Single value,System.Single size,System.Single start,System.Single end,UnityEngine.GUIStyle slider,UnityEngine.GUIStyle thumb,System.Boolean horiz,System.Int32 id)
{
 return  UnityEngine.GUI.Slider(position,value,size,start,end,slider,thumb,horiz,id);
}
System.Single HorizontalScrollbar(UnityEngine.Rect position,System.Single value,System.Single size,System.Single leftValue,System.Single rightValue)
{
 return  UnityEngine.GUI.HorizontalScrollbar(position,value,size,leftValue,rightValue);
}
System.Single HorizontalScrollbar(UnityEngine.Rect position,System.Single value,System.Single size,System.Single leftValue,System.Single rightValue,UnityEngine.GUIStyle style)
{
 return  UnityEngine.GUI.HorizontalScrollbar(position,value,size,leftValue,rightValue,style);
}
System.Single VerticalScrollbar(UnityEngine.Rect position,System.Single value,System.Single size,System.Single topValue,System.Single bottomValue)
{
 return  UnityEngine.GUI.VerticalScrollbar(position,value,size,topValue,bottomValue);
}
System.Single VerticalScrollbar(UnityEngine.Rect position,System.Single value,System.Single size,System.Single topValue,System.Single bottomValue,UnityEngine.GUIStyle style)
{
 return  UnityEngine.GUI.VerticalScrollbar(position,value,size,topValue,bottomValue,style);
}
System.Void BeginGroup(UnityEngine.Rect position)
{
  UnityEngine.GUI.BeginGroup(position);
}
System.Void BeginGroup(UnityEngine.Rect position,System.String text)
{
  UnityEngine.GUI.BeginGroup(position,text);
}
System.Void BeginGroup(UnityEngine.Rect position,UnityEngine.Texture image)
{
  UnityEngine.GUI.BeginGroup(position,image);
}
System.Void BeginGroup(UnityEngine.Rect position,UnityEngine.GUIContent content)
{
  UnityEngine.GUI.BeginGroup(position,content);
}
System.Void BeginGroup(UnityEngine.Rect position,UnityEngine.GUIStyle style)
{
  UnityEngine.GUI.BeginGroup(position,style);
}
System.Void BeginGroup(UnityEngine.Rect position,System.String text,UnityEngine.GUIStyle style)
{
  UnityEngine.GUI.BeginGroup(position,text,style);
}
System.Void BeginGroup(UnityEngine.Rect position,UnityEngine.Texture image,UnityEngine.GUIStyle style)
{
  UnityEngine.GUI.BeginGroup(position,image,style);
}
System.Void BeginGroup(UnityEngine.Rect position,UnityEngine.GUIContent content,UnityEngine.GUIStyle style)
{
  UnityEngine.GUI.BeginGroup(position,content,style);
}
System.Void EndGroup()
{
  UnityEngine.GUI.EndGroup();
}
UnityEngine.Vector2 BeginScrollView(UnityEngine.Rect position,UnityEngine.Vector2 scrollPosition,UnityEngine.Rect viewRect)
{
 return  UnityEngine.GUI.BeginScrollView(position,scrollPosition,viewRect);
}
UnityEngine.Vector2 BeginScrollView(UnityEngine.Rect position,UnityEngine.Vector2 scrollPosition,UnityEngine.Rect viewRect,System.Boolean alwaysShowHorizontal,System.Boolean alwaysShowVertical)
{
 return  UnityEngine.GUI.BeginScrollView(position,scrollPosition,viewRect,alwaysShowHorizontal,alwaysShowVertical);
}
UnityEngine.Vector2 BeginScrollView(UnityEngine.Rect position,UnityEngine.Vector2 scrollPosition,UnityEngine.Rect viewRect,UnityEngine.GUIStyle horizontalScrollbar,UnityEngine.GUIStyle verticalScrollbar)
{
 return  UnityEngine.GUI.BeginScrollView(position,scrollPosition,viewRect,horizontalScrollbar,verticalScrollbar);
}
UnityEngine.Vector2 BeginScrollView(UnityEngine.Rect position,UnityEngine.Vector2 scrollPosition,UnityEngine.Rect viewRect,System.Boolean alwaysShowHorizontal,System.Boolean alwaysShowVertical,UnityEngine.GUIStyle horizontalScrollbar,UnityEngine.GUIStyle verticalScrollbar)
{
 return  UnityEngine.GUI.BeginScrollView(position,scrollPosition,viewRect,alwaysShowHorizontal,alwaysShowVertical,horizontalScrollbar,verticalScrollbar);
}
System.Void EndScrollView()
{
  UnityEngine.GUI.EndScrollView();
}
System.Void EndScrollView(System.Boolean handleScrollWheel)
{
  UnityEngine.GUI.EndScrollView(handleScrollWheel);
}
System.Void ScrollTo(UnityEngine.Rect position)
{
  UnityEngine.GUI.ScrollTo(position);
}
System.Boolean ScrollTowards(UnityEngine.Rect position,System.Single maxDelta)
{
 return  UnityEngine.GUI.ScrollTowards(position,maxDelta);
}
UnityEngine.Rect Window(System.Int32 id,UnityEngine.Rect clientRect,UnityEngine.GUI.WindowFunction func,System.String text)
{
 return  UnityEngine.GUI.Window(id,clientRect,func,text);
}
UnityEngine.Rect Window(System.Int32 id,UnityEngine.Rect clientRect,UnityEngine.GUI.WindowFunction func,UnityEngine.Texture image)
{
 return  UnityEngine.GUI.Window(id,clientRect,func,image);
}
UnityEngine.Rect Window(System.Int32 id,UnityEngine.Rect clientRect,UnityEngine.GUI.WindowFunction func,UnityEngine.GUIContent content)
{
 return  UnityEngine.GUI.Window(id,clientRect,func,content);
}
UnityEngine.Rect Window(System.Int32 id,UnityEngine.Rect clientRect,UnityEngine.GUI.WindowFunction func,System.String text,UnityEngine.GUIStyle style)
{
 return  UnityEngine.GUI.Window(id,clientRect,func,text,style);
}
UnityEngine.Rect Window(System.Int32 id,UnityEngine.Rect clientRect,UnityEngine.GUI.WindowFunction func,UnityEngine.Texture image,UnityEngine.GUIStyle style)
{
 return  UnityEngine.GUI.Window(id,clientRect,func,image,style);
}
UnityEngine.Rect Window(System.Int32 id,UnityEngine.Rect clientRect,UnityEngine.GUI.WindowFunction func,UnityEngine.GUIContent title,UnityEngine.GUIStyle style)
{
 return  UnityEngine.GUI.Window(id,clientRect,func,title,style);
}
UnityEngine.Rect ModalWindow(System.Int32 id,UnityEngine.Rect clientRect,UnityEngine.GUI.WindowFunction func,System.String text)
{
 return  UnityEngine.GUI.ModalWindow(id,clientRect,func,text);
}
UnityEngine.Rect ModalWindow(System.Int32 id,UnityEngine.Rect clientRect,UnityEngine.GUI.WindowFunction func,UnityEngine.Texture image)
{
 return  UnityEngine.GUI.ModalWindow(id,clientRect,func,image);
}
UnityEngine.Rect ModalWindow(System.Int32 id,UnityEngine.Rect clientRect,UnityEngine.GUI.WindowFunction func,UnityEngine.GUIContent content)
{
 return  UnityEngine.GUI.ModalWindow(id,clientRect,func,content);
}
UnityEngine.Rect ModalWindow(System.Int32 id,UnityEngine.Rect clientRect,UnityEngine.GUI.WindowFunction func,System.String text,UnityEngine.GUIStyle style)
{
 return  UnityEngine.GUI.ModalWindow(id,clientRect,func,text,style);
}
UnityEngine.Rect ModalWindow(System.Int32 id,UnityEngine.Rect clientRect,UnityEngine.GUI.WindowFunction func,UnityEngine.Texture image,UnityEngine.GUIStyle style)
{
 return  UnityEngine.GUI.ModalWindow(id,clientRect,func,image,style);
}
UnityEngine.Rect ModalWindow(System.Int32 id,UnityEngine.Rect clientRect,UnityEngine.GUI.WindowFunction func,UnityEngine.GUIContent content,UnityEngine.GUIStyle style)
{
 return  UnityEngine.GUI.ModalWindow(id,clientRect,func,content,style);
}
System.Void DragWindow(UnityEngine.Rect position)
{
  UnityEngine.GUI.DragWindow(position);
}
System.Void DragWindow()
{
  UnityEngine.GUI.DragWindow();
}
System.Void BringWindowToFront(System.Int32 windowID)
{
  UnityEngine.GUI.BringWindowToFront(windowID);
}
System.Void BringWindowToBack(System.Int32 windowID)
{
  UnityEngine.GUI.BringWindowToBack(windowID);
}
System.Void FocusWindow(System.Int32 windowID)
{
  UnityEngine.GUI.FocusWindow(windowID);
}
System.Void UnfocusWindow()
{
  UnityEngine.GUI.UnfocusWindow();
}

	}

}