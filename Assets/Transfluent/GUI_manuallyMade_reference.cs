using UnityEngine;

//wrapper around unity's gui, except to grab text as quickly as possbile and spit it into an internal db
//http://docs.unity3d.com/Documentation/ScriptReference/GUI.html
/*
namespace transfluent
{

	public class GUI
	{
		public static bool changed
		{
			get { return UnityEngine.GUI.changed; }
			set { UnityEngine.GUI.changed = value; }
		}

		public static Color color
		{
			get { return UnityEngine.GUI.color; }
			set { UnityEngine.GUI.color = value; }
		}

		public static Color contentColor
		{
			get { return UnityEngine.GUI.contentColor; }
			set { UnityEngine.GUI.contentColor = value; }
		}

		public static int depth
		{
			get { return UnityEngine.GUI.depth; }
			set { UnityEngine.GUI.depth = value; }
		}

		public static bool enabled
		{
			get { return UnityEngine.GUI.enabled; }
			set { UnityEngine.GUI.enabled = value; }
		}

		public static Matrix4x4 matrix
		{
			get { return UnityEngine.GUI.matrix; }
			set { UnityEngine.GUI.matrix = value; }
		}

		public static GUISkin skin
		{
			get { return UnityEngine.GUI.skin; }
			set { UnityEngine.GUI.skin = value; }
		}

		public static string tooltip
		{
			get { return UnityEngine.GUI.tooltip; }
			set { UnityEngine.GUI.tooltip = value; }
		}

		public static Color backgroundColor
		{
			get { return UnityEngine.GUI.backgroundColor; }
			set { UnityEngine.GUI.backgroundColor = value; }
		}

		//http://docs.unity3d.com/Documentation/ScriptReference/GUI.BeginGroup.html
		public static void BeginGroup(Rect position)
		{
			UnityEngine.GUI.BeginGroup(position);
		}

		public static void BeginGroup(Rect position, string text)
		{
			UnityEngine.GUI.BeginGroup(position, text);
		}

		public static void BeginGroup(Rect position, Texture image)
		{
			UnityEngine.GUI.BeginGroup(position, image);
		}

		public static void BeginGroup(Rect position, GUIContent content)
		{
			UnityEngine.GUI.BeginGroup(position, content);
		}

		public static void BeginGroup(Rect position, GUIStyle style)
		{
			UnityEngine.GUI.BeginGroup(position, style);
		}

		public static void BeginGroup(Rect position, string text, GUIStyle style)
		{
			UnityEngine.GUI.BeginGroup(position, text, style);
		}

		public static void BeginGroup(Rect position, Texture image, GUIStyle style)
		{
			UnityEngine.GUI.BeginGroup(position, image, style);
		}

		public static void BeginGroup(Rect position, GUIContent content, GUIStyle style)
		{
			UnityEngine.GUI.BeginGroup(position, content, style);
		}

		public static Vector2 BeginScrollView(Rect position, Vector2 scrollPosition, Rect viewRect)
		{
			return UnityEngine.GUI.BeginScrollView(position, scrollPosition, viewRect);
		}

		public static Vector2 BeginScrollView(Rect position, Vector2 scrollPosition, Rect viewRect, bool alwaysShowHorizontal,
			bool alwaysShowVertical)
		{
			return UnityEngine.GUI.BeginScrollView(position, scrollPosition, viewRect, alwaysShowHorizontal, alwaysShowVertical);
		}

		public static Vector2 BeginScrollView(Rect position, Vector2 scrollPosition, Rect viewRect,
			GUIStyle horizontalScrollbar, GUIStyle verticalScrollbar)
		{
			return UnityEngine.GUI.BeginScrollView(position, scrollPosition, viewRect, horizontalScrollbar, verticalScrollbar);
		}

		public static Vector2 BeginScrollView(Rect position, Vector2 scrollPosition, Rect viewRect, bool alwaysShowHorizontal,
			bool alwaysShowVertical, GUIStyle horizontalScrollbar, GUIStyle verticalScrollbar)
		{
			return UnityEngine.GUI.BeginScrollView(position, scrollPosition, viewRect, alwaysShowHorizontal, alwaysShowVertical,
				horizontalScrollbar, verticalScrollbar);
		}

		public static void Box(Rect position, string text)
		{
			UnityEngine.GUI.Box(position,text);
		}

		public static void Box(Rect position, Texture image)
		{
			UnityEngine.GUI.Box(position, image);
		}

		public static void Box(Rect position, GUIContent content)
		{
			UnityEngine.GUI.Box(position, content);
		}

		public static void Box(Rect position, string text, GUIStyle style)
		{
			UnityEngine.GUI.Box(position, text,style);
		}

		public static void Box(Rect position, Texture image, GUIStyle style)
		{
			UnityEngine.GUI.Box(position, image, style);
		}

		public static void Box(Rect position, GUIContent content, GUIStyle style)
		{
			UnityEngine.GUI.Box(position, content, style);
		}

		public static void BringWindowToBack(int windowID)
		{
			UnityEngine.GUI.BringWindowToBack(windowID);
		}

		public static void BringWindowToFront(int windowID)
		{
			UnityEngine.GUI.BringWindowToFront(windowID);
		}

		public static bool Button(Rect position, string text)
		{
			return UnityEngine.GUI.Button(position, text);
		}

		public static bool Button(Rect position, Texture image)
		{
			return UnityEngine.GUI.Button(position, image);
		}

		public static bool Button(Rect position, GUIContent content)
		{
			return UnityEngine.GUI.Button(position, content);
		}

		public static bool Button(Rect position, string text, GUIStyle style)
		{
			return UnityEngine.GUI.Button(position, text,style);
		}

		public static bool Button(Rect position, Texture image, GUIStyle style)
		{
			return UnityEngine.GUI.Button(position, image,style);
		}

		public static bool Button(Rect position, GUIContent content, GUIStyle style)
		{
			return UnityEngine.GUI.Button(position, content, style);
		}

		public static void DragWindow(Rect position)
		{
			UnityEngine.GUI.DragWindow(position);
		}

		public static void DrawTexture(Rect position, Texture image, ScaleMode scaleMode = ScaleMode.StretchToFill,
			bool alphaBlend = true, float imageAspect = 0)
		{
			UnityEngine.GUI.DrawTexture(position,image,scaleMode,alphaBlend,imageAspect);
		}

		public static void DrawTextureWithTexCoords(Rect position, Texture image, Rect texCoords, bool alphaBlend = true)
		{
			UnityEngine.GUI.DrawTextureWithTexCoords(position, image, texCoords, alphaBlend);
		}

		public static void EndGroup()
		{
			UnityEngine.GUI.EndGroup();
		}

		public static void EndScrollView()
		{
			UnityEngine.GUI.EndScrollView();
		}

		public static void EndScrollView(bool handleScrollWheel)
		{
			UnityEngine.GUI.EndScrollView(handleScrollWheel);
		}

		public static void FocusControl(string name)
		{
			UnityEngine.GUI.FocusControl(name);
		}

		public static void FocusWindow(int windowID)
		{
			UnityEngine.GUI.FocusWindow(windowID);
		}

		public static string GetNameOfFocusedControl()
		{
			return UnityEngine.GUI.GetNameOfFocusedControl();
		}

		public static float HorizontalScrollbar(Rect position, float value, float size, float leftValue, float rightValue)
		{
			return UnityEngine.GUI.HorizontalScrollbar(position, value, size, leftValue, rightValue);
		}

		public static float HorizontalScrollbar(Rect position, float value, float size, float leftValue, float rightValue,
			GUIStyle style)
		{
			return UnityEngine.GUI.HorizontalScrollbar(position, value, size, leftValue, rightValue,style);
		}

		public static float HorizontalSlider(Rect position, float value, float leftValue, float rightValue)
		{
			return UnityEngine.GUI.HorizontalSlider(position, value, leftValue, rightValue);
		}

		public static float HorizontalSlider(Rect position, float value, float leftValue, float rightValue, GUIStyle slider,
			GUIStyle thumb)
		{
			return UnityEngine.GUI.HorizontalSlider(position, value, leftValue, rightValue, slider, thumb);
		}

		public static void Label(Rect position, string text)
		{
			UnityEngine.GUI.Label(position, text);
		}

		public static void Label(Rect position, Texture image)
		{
			UnityEngine.GUI.Label(position, image);
		}

		public static void Label(Rect position, GUIContent content)
		{
			UnityEngine.GUI.Label(position, content);
		}

		public static void Label(Rect position, string text, GUIStyle style)
		{
			UnityEngine.GUI.Label(position, text, style);
		}

		public static void Label(Rect position, Texture image, GUIStyle style)
		{
			UnityEngine.GUI.Label(position, image, style);
		}

		public static void Label(Rect position, GUIContent content, GUIStyle style)
		{
			UnityEngine.GUI.Label(position, content, style);
		}

		public static Rect ModalWindow(int id, Rect clientRect, UnityEngine.GUI.WindowFunction func, string text)
		{
			return UnityEngine.GUI.ModalWindow(id, clientRect, func,text);
		}

		public static Rect ModalWindow(int id, Rect clientRect, UnityEngine.GUI.WindowFunction func, Texture image)
		{
			return UnityEngine.GUI.ModalWindow(id, clientRect, func, image);
		}

		public static Rect ModalWindow(int id, Rect clientRect, UnityEngine.GUI.WindowFunction func, GUIContent content)
		{
			return UnityEngine.GUI.ModalWindow(id, clientRect, func, content);
		}

		public static Rect ModalWindow(int id, Rect clientRect, UnityEngine.GUI.WindowFunction func, string text, GUIStyle style)
		{
			return UnityEngine.GUI.ModalWindow(id, clientRect, func, text,style);
		}

		public static Rect ModalWindow(int id, Rect clientRect, UnityEngine.GUI.WindowFunction func, Texture image, GUIStyle style)
		{
			return UnityEngine.GUI.ModalWindow(id, clientRect, func, image, style);
		}

		public static Rect ModalWindow(int id, Rect clientRect, UnityEngine.GUI.WindowFunction func, GUIContent content,
			GUIStyle style)
		{
			return UnityEngine.GUI.ModalWindow(id, clientRect, func, content, style);
		}

		public static string PasswordField(Rect position, string password, char maskChar)
		{
			return UnityEngine.GUI.PasswordField(position, password, maskChar);
		}

		public static string PasswordField(Rect position, string password, char maskChar, int maxLength)
		{
			return UnityEngine.GUI.PasswordField(position, password, maskChar,maxLength);
		}
		public static string PasswordField(Rect position, string password, char maskChar, GUIStyle style)
		{
			return UnityEngine.GUI.PasswordField(position, password, maskChar, style);
		}
		public static string PasswordField(Rect position, string password, char maskChar, int maxLength, GUIStyle style)
		{
			return UnityEngine.GUI.PasswordField(position, password, maskChar, maxLength, style);
		}

		public static bool RepeatButton(Rect position, string text)
		{
			return UnityEngine.GUI.RepeatButton(position, text);
		}
		public static bool RepeatButton(Rect position, Texture image)
		{
			return UnityEngine.GUI.RepeatButton(position, image);
		}
		public static bool RepeatButton(Rect position, GUIContent content)
		{
			return UnityEngine.GUI.RepeatButton(position, content);
		}
		public static bool RepeatButton(Rect position, string text, GUIStyle style)
		{
			return UnityEngine.GUI.RepeatButton(position, text, style);
		}
		public static bool RepeatButton(Rect position, Texture image, GUIStyle style)
		{
			return UnityEngine.GUI.RepeatButton(position, image,style);
		}
		public static bool RepeatButton(Rect position, GUIContent content, GUIStyle style)
		{
			return UnityEngine.GUI.RepeatButton(position, content,style);
		}
		public static void ScrollTo(Rect position)
		{
			UnityEngine.GUI.ScrollTo(position);
		}

		public static int SelectionGrid(Rect position, int selected, string[] texts, int xCount)
		{
			return UnityEngine.GUI.SelectionGrid(position, selected, texts, xCount);
		}

		public static int SelectionGrid(Rect position, int selected, Texture[] images, int xCount)
		{
			return UnityEngine.GUI.SelectionGrid(position, selected, images, xCount);
		}

		public static int SelectionGrid(Rect position, int selected, GUIContent[] content, int xCount)
		{
			return UnityEngine.GUI.SelectionGrid(position, selected, content, xCount);
		}

		public static int SelectionGrid(Rect position, int selected, string[] texts, int xCount, GUIStyle style)
		{
			return UnityEngine.GUI.SelectionGrid(position, selected, texts, xCount,style);
		}

		public static int SelectionGrid(Rect position, int selected, Texture[] images, int xCount, GUIStyle style)
		{
			return UnityEngine.GUI.SelectionGrid(position, selected, images, xCount,style);
		}

		public static int SelectionGrid(Rect position, int selected, GUIContent[] contents, int xCount, GUIStyle style)
		{
			return UnityEngine.GUI.SelectionGrid(position, selected, contents, xCount,style);
		}

		public static void SetNextControlName(string name)
		{
			UnityEngine.GUI.SetNextControlName(name);
		}

		public static string TextArea(Rect position, string text)
		{
			return UnityEngine.GUI.TextArea(position, text);
		}
		public static string TextArea(Rect position, string text, int maxLength)
		{
			return UnityEngine.GUI.TextArea(position, text,maxLength);
		}
		public static string TextArea(Rect position, string text, GUIStyle style)
		{
			return UnityEngine.GUI.TextArea(position, text,style);
		}

		public static string TextArea(Rect position, string text, int maxLength, GUIStyle style)
		{
			return UnityEngine.GUI.TextArea(position, text,maxLength,style);
		}

		public static string TextField(Rect position, string text)
		{
			return UnityEngine.GUI.TextField(position, text);
		}

		public static string TextField(Rect position, string text, int maxLength)
		{
			return UnityEngine.GUI.TextField(position, text,maxLength);
		}

		public static string TextField(Rect position, string text, GUIStyle style)
		{
			return UnityEngine.GUI.TextField(position, text, style);
		}

		public static string TextField(Rect position, string text, int maxLength, GUIStyle style)
		{
			return UnityEngine.GUI.TextField(position, text, maxLength, style);
		}

		public static bool Toggle(Rect position, bool value, string text)
		{
			return UnityEngine.GUI.Toggle(position, value, text);
		}

		public static bool Toggle(Rect position, bool value, Texture image)
		{
			return UnityEngine.GUI.Toggle(position, value, image);
		}

		public static bool Toggle(Rect position, bool value, GUIContent content)
		{
			return UnityEngine.GUI.Toggle(position, value, content);
		}

		public static bool Toggle(Rect position, bool value, string text, GUIStyle style)
		{
			return UnityEngine.GUI.Toggle(position, value, text,style);
		}

		public static bool Toggle(Rect position, bool value, Texture image, GUIStyle style)
		{
			return UnityEngine.GUI.Toggle(position, value, image, style);
		}

		public static bool Toggle(Rect position, bool value, GUIContent content, GUIStyle style)
		{
			return UnityEngine.GUI.Toggle(position, value, content, style);
		}
	}
}*/