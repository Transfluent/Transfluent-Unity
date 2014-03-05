using UnityEngine;

//wrapper around unity's gui, except to grab text as quickly as possbile and spit it into an internal db
//http://docs.unity3d.com/Documentation/ScriptReference/GUI.html
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

		private static Vector2 BeginScrollView(Rect position, Vector2 scrollPosition, Rect viewRect)
		{
			return UnityEngine.GUI.BeginScrollView(position, scrollPosition, viewRect);
		}

		private static Vector2 BeginScrollView(Rect position, Vector2 scrollPosition, Rect viewRect, bool alwaysShowHorizontal,
			bool alwaysShowVertical)
		{
			return UnityEngine.GUI.BeginScrollView(position, scrollPosition, viewRect, alwaysShowHorizontal, alwaysShowVertical);
		}

		private static Vector2 BeginScrollView(Rect position, Vector2 scrollPosition, Rect viewRect,
			GUIStyle horizontalScrollbar, GUIStyle verticalScrollbar)
		{
			return UnityEngine.GUI.BeginScrollView(position, scrollPosition, viewRect, horizontalScrollbar, verticalScrollbar);
		}

		static Vector2 BeginScrollView(Rect position, Vector2 scrollPosition, Rect viewRect, bool alwaysShowHorizontal,
			bool alwaysShowVertical, GUIStyle horizontalScrollbar, GUIStyle verticalScrollbar)
		{
			return UnityEngine.GUI.BeginScrollView(position, scrollPosition, viewRect, alwaysShowHorizontal, alwaysShowVertical,
				horizontalScrollbar, verticalScrollbar);
		}

		static void Box(Rect position, string text)
		{
			UnityEngine.GUI.Box(position,text);
		}

		static void Box(Rect position, Texture image)
		{
			UnityEngine.GUI.Box(position, image);
		}

		static void Box(Rect position, GUIContent content)
		{
			UnityEngine.GUI.Box(position, content);
		}

		static void Box(Rect position, string text, GUIStyle style)
		{
			UnityEngine.GUI.Box(position, text,style);
		}

		static void Box(Rect position, Texture image, GUIStyle style)
		{
			UnityEngine.GUI.Box(position, image, style);
		}

		static void Box(Rect position, GUIContent content, GUIStyle style)
		{
			UnityEngine.GUI.Box(position, content, style);
		}

		static void BringWindowToBack(int windowID)
		{
			UnityEngine.GUI.BringWindowToBack(windowID);
		}

		static void BringWindowToFront(int windowID)
		{
			UnityEngine.GUI.BringWindowToFront(windowID);
		}

		static bool Button(Rect position, string text)
		{
			return UnityEngine.GUI.Button(position, text);
		}

		static bool Button(Rect position, Texture image)
		{
			return UnityEngine.GUI.Button(position, image);
		}

		static bool Button(Rect position, GUIContent content)
		{
			return UnityEngine.GUI.Button(position, content);
		}

		static bool Button(Rect position, string text, GUIStyle style)
		{
			return UnityEngine.GUI.Button(position, text,style);
		}

		static bool Button(Rect position, Texture image, GUIStyle style)
		{
			return UnityEngine.GUI.Button(position, image,style);
		}

		static bool Button(Rect position, GUIContent content, GUIStyle style)
		{
			return UnityEngine.GUI.Button(position, content, style);
		}

		static void DragWindow(Rect position)
		{
			UnityEngine.GUI.DragWindow(position);
		}

		static void DrawTexture(Rect position, Texture image, ScaleMode scaleMode = ScaleMode.StretchToFill,
			bool alphaBlend = true, float imageAspect = 0)
		{
			UnityEngine.GUI.DrawTexture(position,image,scaleMode,alphaBlend,imageAspect);
		}

		static void DrawTextureWithTexCoords(Rect position, Texture image, Rect texCoords, bool alphaBlend = true)
		{
			UnityEngine.GUI.DrawTextureWithTexCoords(position, image, texCoords, alphaBlend);
		}

		static void EndGroup()
		{
			UnityEngine.GUI.EndGroup();
		}

		static void EndScrollView()
		{
			UnityEngine.GUI.EndScrollView();
		}

		static void EndScrollView(bool handleScrollWheel)
		{
			UnityEngine.GUI.EndScrollView(handleScrollWheel);
		}

		private static void FocusControl(string name)
		{
			UnityEngine.GUI.FocusControl(name);
		}

		static void FocusWindow(int windowID)
		{
			UnityEngine.GUI.FocusWindow(windowID);
		}

		static string GetNameOfFocusedControl()
		{
			return UnityEngine.GUI.GetNameOfFocusedControl();
		}

		static float HorizontalScrollbar(Rect position, float value, float size, float leftValue, float rightValue)
		{
			return UnityEngine.GUI.HorizontalScrollbar(position, value, size, leftValue, rightValue);
		}

		static float HorizontalScrollbar(Rect position, float value, float size, float leftValue, float rightValue,
			GUIStyle style)
		{
			return UnityEngine.GUI.HorizontalScrollbar(position, value, size, leftValue, rightValue,style);
		}

		static float HorizontalSlider(Rect position, float value, float leftValue, float rightValue)
		{
			return UnityEngine.GUI.HorizontalSlider(position, value, leftValue, rightValue);
		}

		static float HorizontalSlider(Rect position, float value, float leftValue, float rightValue, GUIStyle slider,
			GUIStyle thumb)
		{
			return UnityEngine.GUI.HorizontalSlider(position, value, leftValue, rightValue, slider, thumb);
		}

		static void Label(Rect position, string text)
		{
			UnityEngine.GUI.Label(position, text);
		}

		static void Label(Rect position, Texture image)
		{
			UnityEngine.GUI.Label(position, image);
		}

		static void Label(Rect position, GUIContent content)
		{
			UnityEngine.GUI.Label(position, content);
		}

		static void Label(Rect position, string text, GUIStyle style)
		{
			UnityEngine.GUI.Label(position, text, style);
		}

		static void Label(Rect position, Texture image, GUIStyle style)
		{
			UnityEngine.GUI.Label(position, image, style);
		}

		static void Label(Rect position, GUIContent content, GUIStyle style)
		{
			UnityEngine.GUI.Label(position, content, style);
		}

	}
}