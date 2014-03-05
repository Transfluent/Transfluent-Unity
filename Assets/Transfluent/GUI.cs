using UnityEngine;

//wrapper around unity's gui, except to grab text as quickly as possbile and spit it into an internal db
//http://docs.unity3d.com/Documentation/ScriptReference/GUI.html
namespace transfluent.guiwrapper
{

	public partial class GUI
	{

 public static UnityEngine.GUISkin skin {
 get { return UnityEngine.GUI.skin; }
 set { UnityEngine.GUI.skin = value; }
}
 public static UnityEngine.Color color {
 get { return UnityEngine.GUI.color; }
 set { UnityEngine.GUI.color = value; }
}
 public static UnityEngine.Color backgroundColor {
 get { return UnityEngine.GUI.backgroundColor; }
 set { UnityEngine.GUI.backgroundColor = value; }
}
 public static UnityEngine.Color contentColor {
 get { return UnityEngine.GUI.contentColor; }
 set { UnityEngine.GUI.contentColor = value; }
}
 public static System.Boolean changed {
 get { return UnityEngine.GUI.changed; }
 set { UnityEngine.GUI.changed = value; }
}
 public static System.Boolean enabled {
 get { return UnityEngine.GUI.enabled; }
 set { UnityEngine.GUI.enabled = value; }
}
 public static UnityEngine.Matrix4x4 matrix {
 get { return UnityEngine.GUI.matrix; }
 set { UnityEngine.GUI.matrix = value; }
}
 public static System.String tooltip {
 get { return UnityEngine.GUI.tooltip; }
 set { UnityEngine.GUI.tooltip = value; }
}
 public static System.Int32 depth {
 get { return UnityEngine.GUI.depth; }
 set { UnityEngine.GUI.depth = value; }
}
public static System.Void Label(UnityEngine.Rect position,System.String text)
{
  UnityEngine.GUI.Label(position,text);
}
public static System.Void Label(UnityEngine.Rect position,UnityEngine.Texture image)
{
  UnityEngine.GUI.Label(position,image);
}
public static System.Void Label(UnityEngine.Rect position,UnityEngine.GUIContent content)
{
  UnityEngine.GUI.Label(position,content);
}
public static System.Void Label(UnityEngine.Rect position,System.String text,UnityEngine.GUIStyle style)
{
  UnityEngine.GUI.Label(position,text,style);
}
public static System.Void Label(UnityEngine.Rect position,UnityEngine.Texture image,UnityEngine.GUIStyle style)
{
  UnityEngine.GUI.Label(position,image,style);
}
public static System.Void Label(UnityEngine.Rect position,UnityEngine.GUIContent content,UnityEngine.GUIStyle style)
{
  UnityEngine.GUI.Label(position,content,style);
}
public static System.Void DrawTexture(UnityEngine.Rect position,UnityEngine.Texture image,UnityEngine.ScaleMode scaleMode,System.Boolean alphaBlend)
{
  UnityEngine.GUI.DrawTexture(position,image,scaleMode,alphaBlend);
}
public static System.Void DrawTexture(UnityEngine.Rect position,UnityEngine.Texture image,UnityEngine.ScaleMode scaleMode)
{
  UnityEngine.GUI.DrawTexture(position,image,scaleMode);
}
public static System.Void DrawTexture(UnityEngine.Rect position,UnityEngine.Texture image)
{
  UnityEngine.GUI.DrawTexture(position,image);
}
public static System.Void DrawTexture(UnityEngine.Rect position,UnityEngine.Texture image,UnityEngine.ScaleMode scaleMode,System.Boolean alphaBlend,System.Single imageAspect)
{
  UnityEngine.GUI.DrawTexture(position,image,scaleMode,alphaBlend,imageAspect);
}
public static System.Void DrawTextureWithTexCoords(UnityEngine.Rect position,UnityEngine.Texture image,UnityEngine.Rect texCoords)
{
  UnityEngine.GUI.DrawTextureWithTexCoords(position,image,texCoords);
}
public static System.Void DrawTextureWithTexCoords(UnityEngine.Rect position,UnityEngine.Texture image,UnityEngine.Rect texCoords,System.Boolean alphaBlend)
{
  UnityEngine.GUI.DrawTextureWithTexCoords(position,image,texCoords,alphaBlend);
}
public static System.Void Box(UnityEngine.Rect position,System.String text)
{
  UnityEngine.GUI.Box(position,text);
}
public static System.Void Box(UnityEngine.Rect position,UnityEngine.Texture image)
{
  UnityEngine.GUI.Box(position,image);
}
public static System.Void Box(UnityEngine.Rect position,UnityEngine.GUIContent content)
{
  UnityEngine.GUI.Box(position,content);
}
public static System.Void Box(UnityEngine.Rect position,System.String text,UnityEngine.GUIStyle style)
{
  UnityEngine.GUI.Box(position,text,style);
}
public static System.Void Box(UnityEngine.Rect position,UnityEngine.Texture image,UnityEngine.GUIStyle style)
{
  UnityEngine.GUI.Box(position,image,style);
}
public static System.Void Box(UnityEngine.Rect position,UnityEngine.GUIContent content,UnityEngine.GUIStyle style)
{
  UnityEngine.GUI.Box(position,content,style);
}
public static System.Boolean Button(UnityEngine.Rect position,System.String text)
{
 return  UnityEngine.GUI.Button(position,text);
}
public static System.Boolean Button(UnityEngine.Rect position,UnityEngine.Texture image)
{
 return  UnityEngine.GUI.Button(position,image);
}
public static System.Boolean Button(UnityEngine.Rect position,UnityEngine.GUIContent content)
{
 return  UnityEngine.GUI.Button(position,content);
}
public static System.Boolean Button(UnityEngine.Rect position,System.String text,UnityEngine.GUIStyle style)
{
 return  UnityEngine.GUI.Button(position,text,style);
}
public static System.Boolean Button(UnityEngine.Rect position,UnityEngine.Texture image,UnityEngine.GUIStyle style)
{
 return  UnityEngine.GUI.Button(position,image,style);
}
public static System.Boolean Button(UnityEngine.Rect position,UnityEngine.GUIContent content,UnityEngine.GUIStyle style)
{
 return  UnityEngine.GUI.Button(position,content,style);
}
public static System.Boolean RepeatButton(UnityEngine.Rect position,System.String text)
{
 return  UnityEngine.GUI.RepeatButton(position,text);
}
public static System.Boolean RepeatButton(UnityEngine.Rect position,UnityEngine.Texture image)
{
 return  UnityEngine.GUI.RepeatButton(position,image);
}
public static System.Boolean RepeatButton(UnityEngine.Rect position,UnityEngine.GUIContent content)
{
 return  UnityEngine.GUI.RepeatButton(position,content);
}
public static System.Boolean RepeatButton(UnityEngine.Rect position,System.String text,UnityEngine.GUIStyle style)
{
 return  UnityEngine.GUI.RepeatButton(position,text,style);
}
public static System.Boolean RepeatButton(UnityEngine.Rect position,UnityEngine.Texture image,UnityEngine.GUIStyle style)
{
 return  UnityEngine.GUI.RepeatButton(position,image,style);
}
public static System.Boolean RepeatButton(UnityEngine.Rect position,UnityEngine.GUIContent content,UnityEngine.GUIStyle style)
{
 return  UnityEngine.GUI.RepeatButton(position,content,style);
}
public static System.String TextField(UnityEngine.Rect position,System.String text)
{
 return  UnityEngine.GUI.TextField(position,text);
}
public static System.String TextField(UnityEngine.Rect position,System.String text,System.Int32 maxLength)
{
 return  UnityEngine.GUI.TextField(position,text,maxLength);
}
public static System.String TextField(UnityEngine.Rect position,System.String text,UnityEngine.GUIStyle style)
{
 return  UnityEngine.GUI.TextField(position,text,style);
}
public static System.String TextField(UnityEngine.Rect position,System.String text,System.Int32 maxLength,UnityEngine.GUIStyle style)
{
 return  UnityEngine.GUI.TextField(position,text,maxLength,style);
}
public static System.String PasswordField(UnityEngine.Rect position,System.String password,System.Char maskChar)
{
 return  UnityEngine.GUI.PasswordField(position,password,maskChar);
}
public static System.String PasswordField(UnityEngine.Rect position,System.String password,System.Char maskChar,System.Int32 maxLength)
{
 return  UnityEngine.GUI.PasswordField(position,password,maskChar,maxLength);
}
public static System.String PasswordField(UnityEngine.Rect position,System.String password,System.Char maskChar,UnityEngine.GUIStyle style)
{
 return  UnityEngine.GUI.PasswordField(position,password,maskChar,style);
}
public static System.String PasswordField(UnityEngine.Rect position,System.String password,System.Char maskChar,System.Int32 maxLength,UnityEngine.GUIStyle style)
{
 return  UnityEngine.GUI.PasswordField(position,password,maskChar,maxLength,style);
}
public static System.String TextArea(UnityEngine.Rect position,System.String text)
{
 return  UnityEngine.GUI.TextArea(position,text);
}
public static System.String TextArea(UnityEngine.Rect position,System.String text,System.Int32 maxLength)
{
 return  UnityEngine.GUI.TextArea(position,text,maxLength);
}
public static System.String TextArea(UnityEngine.Rect position,System.String text,UnityEngine.GUIStyle style)
{
 return  UnityEngine.GUI.TextArea(position,text,style);
}
public static System.String TextArea(UnityEngine.Rect position,System.String text,System.Int32 maxLength,UnityEngine.GUIStyle style)
{
 return  UnityEngine.GUI.TextArea(position,text,maxLength,style);
}
public static System.Void SetNextControlName(System.String name)
{
  UnityEngine.GUI.SetNextControlName(name);
}
public static System.String GetNameOfFocusedControl()
{
 return  UnityEngine.GUI.GetNameOfFocusedControl();
}
public static System.Void FocusControl(System.String name)
{
  UnityEngine.GUI.FocusControl(name);
}
public static System.Boolean Toggle(UnityEngine.Rect position,System.Boolean value,System.String text)
{
 return  UnityEngine.GUI.Toggle(position,value,text);
}
public static System.Boolean Toggle(UnityEngine.Rect position,System.Boolean value,UnityEngine.Texture image)
{
 return  UnityEngine.GUI.Toggle(position,value,image);
}
public static System.Boolean Toggle(UnityEngine.Rect position,System.Boolean value,UnityEngine.GUIContent content)
{
 return  UnityEngine.GUI.Toggle(position,value,content);
}
public static System.Boolean Toggle(UnityEngine.Rect position,System.Boolean value,System.String text,UnityEngine.GUIStyle style)
{
 return  UnityEngine.GUI.Toggle(position,value,text,style);
}
public static System.Boolean Toggle(UnityEngine.Rect position,System.Boolean value,UnityEngine.Texture image,UnityEngine.GUIStyle style)
{
 return  UnityEngine.GUI.Toggle(position,value,image,style);
}
public static System.Boolean Toggle(UnityEngine.Rect position,System.Boolean value,UnityEngine.GUIContent content,UnityEngine.GUIStyle style)
{
 return  UnityEngine.GUI.Toggle(position,value,content,style);
}
public static System.Int32 Toolbar(UnityEngine.Rect position,System.Int32 selected,System.String[] texts)
{
 return  UnityEngine.GUI.Toolbar(position,selected,texts);
}
public static System.Int32 Toolbar(UnityEngine.Rect position,System.Int32 selected,UnityEngine.Texture[] images)
{
 return  UnityEngine.GUI.Toolbar(position,selected,images);
}
public static System.Int32 Toolbar(UnityEngine.Rect position,System.Int32 selected,UnityEngine.GUIContent[] content)
{
 return  UnityEngine.GUI.Toolbar(position,selected,content);
}
public static System.Int32 Toolbar(UnityEngine.Rect position,System.Int32 selected,System.String[] texts,UnityEngine.GUIStyle style)
{
 return  UnityEngine.GUI.Toolbar(position,selected,texts,style);
}
public static System.Int32 Toolbar(UnityEngine.Rect position,System.Int32 selected,UnityEngine.Texture[] images,UnityEngine.GUIStyle style)
{
 return  UnityEngine.GUI.Toolbar(position,selected,images,style);
}
public static System.Int32 Toolbar(UnityEngine.Rect position,System.Int32 selected,UnityEngine.GUIContent[] contents,UnityEngine.GUIStyle style)
{
 return  UnityEngine.GUI.Toolbar(position,selected,contents,style);
}
public static System.Int32 SelectionGrid(UnityEngine.Rect position,System.Int32 selected,System.String[] texts,System.Int32 xCount)
{
 return  UnityEngine.GUI.SelectionGrid(position,selected,texts,xCount);
}
public static System.Int32 SelectionGrid(UnityEngine.Rect position,System.Int32 selected,UnityEngine.Texture[] images,System.Int32 xCount)
{
 return  UnityEngine.GUI.SelectionGrid(position,selected,images,xCount);
}
public static System.Int32 SelectionGrid(UnityEngine.Rect position,System.Int32 selected,UnityEngine.GUIContent[] content,System.Int32 xCount)
{
 return  UnityEngine.GUI.SelectionGrid(position,selected,content,xCount);
}
public static System.Int32 SelectionGrid(UnityEngine.Rect position,System.Int32 selected,System.String[] texts,System.Int32 xCount,UnityEngine.GUIStyle style)
{
 return  UnityEngine.GUI.SelectionGrid(position,selected,texts,xCount,style);
}
public static System.Int32 SelectionGrid(UnityEngine.Rect position,System.Int32 selected,UnityEngine.Texture[] images,System.Int32 xCount,UnityEngine.GUIStyle style)
{
 return  UnityEngine.GUI.SelectionGrid(position,selected,images,xCount,style);
}
public static System.Int32 SelectionGrid(UnityEngine.Rect position,System.Int32 selected,UnityEngine.GUIContent[] contents,System.Int32 xCount,UnityEngine.GUIStyle style)
{
 return  UnityEngine.GUI.SelectionGrid(position,selected,contents,xCount,style);
}
public static System.Single HorizontalSlider(UnityEngine.Rect position,System.Single value,System.Single leftValue,System.Single rightValue)
{
 return  UnityEngine.GUI.HorizontalSlider(position,value,leftValue,rightValue);
}
public static System.Single HorizontalSlider(UnityEngine.Rect position,System.Single value,System.Single leftValue,System.Single rightValue,UnityEngine.GUIStyle slider,UnityEngine.GUIStyle thumb)
{
 return  UnityEngine.GUI.HorizontalSlider(position,value,leftValue,rightValue,slider,thumb);
}
public static System.Single VerticalSlider(UnityEngine.Rect position,System.Single value,System.Single topValue,System.Single bottomValue)
{
 return  UnityEngine.GUI.VerticalSlider(position,value,topValue,bottomValue);
}
public static System.Single VerticalSlider(UnityEngine.Rect position,System.Single value,System.Single topValue,System.Single bottomValue,UnityEngine.GUIStyle slider,UnityEngine.GUIStyle thumb)
{
 return  UnityEngine.GUI.VerticalSlider(position,value,topValue,bottomValue,slider,thumb);
}
public static System.Single Slider(UnityEngine.Rect position,System.Single value,System.Single size,System.Single start,System.Single end,UnityEngine.GUIStyle slider,UnityEngine.GUIStyle thumb,System.Boolean horiz,System.Int32 id)
{
 return  UnityEngine.GUI.Slider(position,value,size,start,end,slider,thumb,horiz,id);
}
public static System.Single HorizontalScrollbar(UnityEngine.Rect position,System.Single value,System.Single size,System.Single leftValue,System.Single rightValue)
{
 return  UnityEngine.GUI.HorizontalScrollbar(position,value,size,leftValue,rightValue);
}
public static System.Single HorizontalScrollbar(UnityEngine.Rect position,System.Single value,System.Single size,System.Single leftValue,System.Single rightValue,UnityEngine.GUIStyle style)
{
 return  UnityEngine.GUI.HorizontalScrollbar(position,value,size,leftValue,rightValue,style);
}
public static System.Single VerticalScrollbar(UnityEngine.Rect position,System.Single value,System.Single size,System.Single topValue,System.Single bottomValue)
{
 return  UnityEngine.GUI.VerticalScrollbar(position,value,size,topValue,bottomValue);
}
public static System.Single VerticalScrollbar(UnityEngine.Rect position,System.Single value,System.Single size,System.Single topValue,System.Single bottomValue,UnityEngine.GUIStyle style)
{
 return  UnityEngine.GUI.VerticalScrollbar(position,value,size,topValue,bottomValue,style);
}
public static System.Void BeginGroup(UnityEngine.Rect position)
{
  UnityEngine.GUI.BeginGroup(position);
}
public static System.Void BeginGroup(UnityEngine.Rect position,System.String text)
{
  UnityEngine.GUI.BeginGroup(position,text);
}
public static System.Void BeginGroup(UnityEngine.Rect position,UnityEngine.Texture image)
{
  UnityEngine.GUI.BeginGroup(position,image);
}
public static System.Void BeginGroup(UnityEngine.Rect position,UnityEngine.GUIContent content)
{
  UnityEngine.GUI.BeginGroup(position,content);
}
public static System.Void BeginGroup(UnityEngine.Rect position,UnityEngine.GUIStyle style)
{
  UnityEngine.GUI.BeginGroup(position,style);
}
public static System.Void BeginGroup(UnityEngine.Rect position,System.String text,UnityEngine.GUIStyle style)
{
  UnityEngine.GUI.BeginGroup(position,text,style);
}
public static System.Void BeginGroup(UnityEngine.Rect position,UnityEngine.Texture image,UnityEngine.GUIStyle style)
{
  UnityEngine.GUI.BeginGroup(position,image,style);
}
public static System.Void BeginGroup(UnityEngine.Rect position,UnityEngine.GUIContent content,UnityEngine.GUIStyle style)
{
  UnityEngine.GUI.BeginGroup(position,content,style);
}
public static System.Void EndGroup()
{
  UnityEngine.GUI.EndGroup();
}
public static UnityEngine.Vector2 BeginScrollView(UnityEngine.Rect position,UnityEngine.Vector2 scrollPosition,UnityEngine.Rect viewRect)
{
 return  UnityEngine.GUI.BeginScrollView(position,scrollPosition,viewRect);
}
public static UnityEngine.Vector2 BeginScrollView(UnityEngine.Rect position,UnityEngine.Vector2 scrollPosition,UnityEngine.Rect viewRect,System.Boolean alwaysShowHorizontal,System.Boolean alwaysShowVertical)
{
 return  UnityEngine.GUI.BeginScrollView(position,scrollPosition,viewRect,alwaysShowHorizontal,alwaysShowVertical);
}
public static UnityEngine.Vector2 BeginScrollView(UnityEngine.Rect position,UnityEngine.Vector2 scrollPosition,UnityEngine.Rect viewRect,UnityEngine.GUIStyle horizontalScrollbar,UnityEngine.GUIStyle verticalScrollbar)
{
 return  UnityEngine.GUI.BeginScrollView(position,scrollPosition,viewRect,horizontalScrollbar,verticalScrollbar);
}
public static UnityEngine.Vector2 BeginScrollView(UnityEngine.Rect position,UnityEngine.Vector2 scrollPosition,UnityEngine.Rect viewRect,System.Boolean alwaysShowHorizontal,System.Boolean alwaysShowVertical,UnityEngine.GUIStyle horizontalScrollbar,UnityEngine.GUIStyle verticalScrollbar)
{
 return  UnityEngine.GUI.BeginScrollView(position,scrollPosition,viewRect,alwaysShowHorizontal,alwaysShowVertical,horizontalScrollbar,verticalScrollbar);
}
public static System.Void EndScrollView()
{
  UnityEngine.GUI.EndScrollView();
}
public static System.Void EndScrollView(System.Boolean handleScrollWheel)
{
  UnityEngine.GUI.EndScrollView(handleScrollWheel);
}
public static System.Void ScrollTo(UnityEngine.Rect position)
{
  UnityEngine.GUI.ScrollTo(position);
}
public static System.Boolean ScrollTowards(UnityEngine.Rect position,System.Single maxDelta)
{
 return  UnityEngine.GUI.ScrollTowards(position,maxDelta);
}
public static UnityEngine.Rect Window(System.Int32 id,UnityEngine.Rect clientRect,UnityEngine.GUI.WindowFunction func,System.String text)
{
 return  UnityEngine.GUI.Window(id,clientRect,func,text);
}
public static UnityEngine.Rect Window(System.Int32 id,UnityEngine.Rect clientRect,UnityEngine.GUI.WindowFunction func,UnityEngine.Texture image)
{
 return  UnityEngine.GUI.Window(id,clientRect,func,image);
}
public static UnityEngine.Rect Window(System.Int32 id,UnityEngine.Rect clientRect,UnityEngine.GUI.WindowFunction func,UnityEngine.GUIContent content)
{
 return  UnityEngine.GUI.Window(id,clientRect,func,content);
}
public static UnityEngine.Rect Window(System.Int32 id,UnityEngine.Rect clientRect,UnityEngine.GUI.WindowFunction func,System.String text,UnityEngine.GUIStyle style)
{
 return  UnityEngine.GUI.Window(id,clientRect,func,text,style);
}
public static UnityEngine.Rect Window(System.Int32 id,UnityEngine.Rect clientRect,UnityEngine.GUI.WindowFunction func,UnityEngine.Texture image,UnityEngine.GUIStyle style)
{
 return  UnityEngine.GUI.Window(id,clientRect,func,image,style);
}
public static UnityEngine.Rect Window(System.Int32 id,UnityEngine.Rect clientRect,UnityEngine.GUI.WindowFunction func,UnityEngine.GUIContent title,UnityEngine.GUIStyle style)
{
 return  UnityEngine.GUI.Window(id,clientRect,func,title,style);
}
public static UnityEngine.Rect ModalWindow(System.Int32 id,UnityEngine.Rect clientRect,UnityEngine.GUI.WindowFunction func,System.String text)
{
 return  UnityEngine.GUI.ModalWindow(id,clientRect,func,text);
}
public static UnityEngine.Rect ModalWindow(System.Int32 id,UnityEngine.Rect clientRect,UnityEngine.GUI.WindowFunction func,UnityEngine.Texture image)
{
 return  UnityEngine.GUI.ModalWindow(id,clientRect,func,image);
}
public static UnityEngine.Rect ModalWindow(System.Int32 id,UnityEngine.Rect clientRect,UnityEngine.GUI.WindowFunction func,UnityEngine.GUIContent content)
{
 return  UnityEngine.GUI.ModalWindow(id,clientRect,func,content);
}
public static UnityEngine.Rect ModalWindow(System.Int32 id,UnityEngine.Rect clientRect,UnityEngine.GUI.WindowFunction func,System.String text,UnityEngine.GUIStyle style)
{
 return  UnityEngine.GUI.ModalWindow(id,clientRect,func,text,style);
}
public static UnityEngine.Rect ModalWindow(System.Int32 id,UnityEngine.Rect clientRect,UnityEngine.GUI.WindowFunction func,UnityEngine.Texture image,UnityEngine.GUIStyle style)
{
 return  UnityEngine.GUI.ModalWindow(id,clientRect,func,image,style);
}
public static UnityEngine.Rect ModalWindow(System.Int32 id,UnityEngine.Rect clientRect,UnityEngine.GUI.WindowFunction func,UnityEngine.GUIContent content,UnityEngine.GUIStyle style)
{
 return  UnityEngine.GUI.ModalWindow(id,clientRect,func,content,style);
}
public static System.Void DragWindow(UnityEngine.Rect position)
{
  UnityEngine.GUI.DragWindow(position);
}
public static System.Void DragWindow()
{
  UnityEngine.GUI.DragWindow();
}
public static System.Void BringWindowToFront(System.Int32 windowID)
{
  UnityEngine.GUI.BringWindowToFront(windowID);
}
public static System.Void BringWindowToBack(System.Int32 windowID)
{
  UnityEngine.GUI.BringWindowToBack(windowID);
}
public static System.Void FocusWindow(System.Int32 windowID)
{
  UnityEngine.GUI.FocusWindow(windowID);
}
public static System.Void UnfocusWindow()
{
  UnityEngine.GUI.UnfocusWindow();
}

	}
}