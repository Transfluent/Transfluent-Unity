//wrapper around unity's gui, except to grab text as quickly as possbile and spit it into an internal db
//http://docs.unity3d.com/Documentation/ScriptReference/GUI.html
namespace transfluent.guiwrapper
{
#pragma warning disable 618

	public partial class GUI
	{
		public static UnityEngine.GUISkin skin
		{
			get { return UnityEngine.GUI.skin; }
			set { UnityEngine.GUI.skin = value; }
		}

		public static UnityEngine.Color color
		{
			get { return UnityEngine.GUI.color; }
			set { UnityEngine.GUI.color = value; }
		}

		public static UnityEngine.Color backgroundColor
		{
			get { return UnityEngine.GUI.backgroundColor; }
			set { UnityEngine.GUI.backgroundColor = value; }
		}

		public static UnityEngine.Color contentColor
		{
			get { return UnityEngine.GUI.contentColor; }
			set { UnityEngine.GUI.contentColor = value; }
		}

		public static System.Boolean changed
		{
			get { return UnityEngine.GUI.changed; }
			set { UnityEngine.GUI.changed = value; }
		}

		public static System.Boolean enabled
		{
			get { return UnityEngine.GUI.enabled; }
			set { UnityEngine.GUI.enabled = value; }
		}

		public static UnityEngine.Matrix4x4 matrix
		{
			get { return UnityEngine.GUI.matrix; }
			set { UnityEngine.GUI.matrix = value; }
		}

		public static System.String tooltip
		{
			get { return UnityEngine.GUI.tooltip; }
			set { UnityEngine.GUI.tooltip = value; }
		}

		public static System.Int32 depth
		{
			get { return UnityEngine.GUI.depth; }
			set { UnityEngine.GUI.depth = value; }
		}

		public static void Label(UnityEngine.Rect position, System.String text)
		{
			UnityEngine.GUI.Label(position, TranslationUtility.get(text));
		}

		public static void Label(UnityEngine.Rect position, UnityEngine.Texture image)
		{
			UnityEngine.GUI.Label(position, image);
		}

		public static void Label(UnityEngine.Rect position, UnityEngine.GUIContent content)
		{
			UnityEngine.GUI.Label(position, content);
		}

		public static void Label(UnityEngine.Rect position, System.String text, UnityEngine.GUIStyle style)
		{
			UnityEngine.GUI.Label(position, TranslationUtility.get(text), style);
		}

		public static void Label(UnityEngine.Rect position, UnityEngine.Texture image, UnityEngine.GUIStyle style)
		{
			UnityEngine.GUI.Label(position, image, style);
		}

		public static void Label(UnityEngine.Rect position, UnityEngine.GUIContent content, UnityEngine.GUIStyle style)
		{
			UnityEngine.GUI.Label(position, content, style);
		}

		public static void DrawTexture(UnityEngine.Rect position, UnityEngine.Texture image, UnityEngine.ScaleMode scaleMode, bool alphaBlend)
		{
			UnityEngine.GUI.DrawTexture(position, image, scaleMode, alphaBlend);
		}

		public static void DrawTexture(UnityEngine.Rect position, UnityEngine.Texture image, UnityEngine.ScaleMode scaleMode)
		{
			UnityEngine.GUI.DrawTexture(position, image, scaleMode);
		}

		public static void DrawTexture(UnityEngine.Rect position, UnityEngine.Texture image)
		{
			UnityEngine.GUI.DrawTexture(position, image);
		}

		public static void DrawTexture(UnityEngine.Rect position, UnityEngine.Texture image, UnityEngine.ScaleMode scaleMode, bool alphaBlend, float imageAspect)
		{
			UnityEngine.GUI.DrawTexture(position, image, scaleMode, alphaBlend, imageAspect);
		}

		public static void DrawTextureWithTexCoords(UnityEngine.Rect position, UnityEngine.Texture image, UnityEngine.Rect texCoords)
		{
			UnityEngine.GUI.DrawTextureWithTexCoords(position, image, texCoords);
		}

		public static void DrawTextureWithTexCoords(UnityEngine.Rect position, UnityEngine.Texture image, UnityEngine.Rect texCoords, bool alphaBlend)
		{
			UnityEngine.GUI.DrawTextureWithTexCoords(position, image, texCoords, alphaBlend);
		}

		public static void Box(UnityEngine.Rect position, System.String text)
		{
			UnityEngine.GUI.Box(position, TranslationUtility.get(text));
		}

		public static void Box(UnityEngine.Rect position, UnityEngine.Texture image)
		{
			UnityEngine.GUI.Box(position, image);
		}

		public static void Box(UnityEngine.Rect position, UnityEngine.GUIContent content)
		{
			UnityEngine.GUI.Box(position, content);
		}

		public static void Box(UnityEngine.Rect position, System.String text, UnityEngine.GUIStyle style)
		{
			UnityEngine.GUI.Box(position, text, style);
		}

		public static void Box(UnityEngine.Rect position, UnityEngine.Texture image, UnityEngine.GUIStyle style)
		{
			UnityEngine.GUI.Box(position, image, style);
		}

		public static void Box(UnityEngine.Rect position, UnityEngine.GUIContent content, UnityEngine.GUIStyle style)
		{
			UnityEngine.GUI.Box(position, content, style);
		}

		public static bool Button(UnityEngine.Rect position, System.String text)
		{
			return UnityEngine.GUI.Button(position, TranslationUtility.get(text));
		}

		public static bool Button(UnityEngine.Rect position, UnityEngine.Texture image)
		{
			return UnityEngine.GUI.Button(position, image);
		}

		public static bool Button(UnityEngine.Rect position, UnityEngine.GUIContent content)
		{
			return UnityEngine.GUI.Button(position, content);
		}

		public static bool Button(UnityEngine.Rect position, System.String text, UnityEngine.GUIStyle style)
		{
			return UnityEngine.GUI.Button(position, text, style);
		}

		public static bool Button(UnityEngine.Rect position, UnityEngine.Texture image, UnityEngine.GUIStyle style)
		{
			return UnityEngine.GUI.Button(position, image, style);
		}

		public static bool Button(UnityEngine.Rect position, UnityEngine.GUIContent content, UnityEngine.GUIStyle style)
		{
			return UnityEngine.GUI.Button(position, content, style);
		}

		public static bool RepeatButton(UnityEngine.Rect position, System.String text)
		{
			return UnityEngine.GUI.RepeatButton(position, TranslationUtility.get(text));
		}

		public static bool RepeatButton(UnityEngine.Rect position, UnityEngine.Texture image)
		{
			return UnityEngine.GUI.RepeatButton(position, image);
		}

		public static bool RepeatButton(UnityEngine.Rect position, UnityEngine.GUIContent content)
		{
			return UnityEngine.GUI.RepeatButton(position, content);
		}

		public static bool RepeatButton(UnityEngine.Rect position, System.String text, UnityEngine.GUIStyle style)
		{
			return UnityEngine.GUI.RepeatButton(position, TranslationUtility.get(text), style);
		}

		public static bool RepeatButton(UnityEngine.Rect position, UnityEngine.Texture image, UnityEngine.GUIStyle style)
		{
			return UnityEngine.GUI.RepeatButton(position, image, style);
		}

		public static bool RepeatButton(UnityEngine.Rect position, UnityEngine.GUIContent content, UnityEngine.GUIStyle style)
		{
			return UnityEngine.GUI.RepeatButton(position, content, style);
		}

		public static System.String TextField(UnityEngine.Rect position, System.String text)
		{
			return UnityEngine.GUI.TextField(position, TranslationUtility.get(text));
		}

		public static System.String TextField(UnityEngine.Rect position, System.String text, int maxLength)
		{
			return UnityEngine.GUI.TextField(position, TranslationUtility.get(text), maxLength);
		}

		public static System.String TextField(UnityEngine.Rect position, System.String text, UnityEngine.GUIStyle style)
		{
			return UnityEngine.GUI.TextField(position, TranslationUtility.get(text), style);
		}

		public static System.String TextField(UnityEngine.Rect position, System.String text, int maxLength, UnityEngine.GUIStyle style)
		{
			return UnityEngine.GUI.TextField(position, TranslationUtility.get(text), maxLength, style);
		}

		public static System.String PasswordField(UnityEngine.Rect position, System.String password, char maskChar)
		{
			return UnityEngine.GUI.PasswordField(position, password, maskChar);
		}

		public static System.String PasswordField(UnityEngine.Rect position, System.String password, char maskChar, int maxLength)
		{
			return UnityEngine.GUI.PasswordField(position, password, maskChar, maxLength);
		}

		public static System.String PasswordField(UnityEngine.Rect position, System.String password, char maskChar, UnityEngine.GUIStyle style)
		{
			return UnityEngine.GUI.PasswordField(position, password, maskChar, style);
		}

		public static System.String PasswordField(UnityEngine.Rect position, System.String password, char maskChar, int maxLength, UnityEngine.GUIStyle style)
		{
			return UnityEngine.GUI.PasswordField(position, password, maskChar, maxLength, style);
		}

		public static System.String TextArea(UnityEngine.Rect position, System.String text)
		{
			return UnityEngine.GUI.TextArea(position, TranslationUtility.get(text));
		}

		public static System.String TextArea(UnityEngine.Rect position, System.String text, int maxLength)
		{
			return UnityEngine.GUI.TextArea(position, TranslationUtility.get(text), maxLength);
		}

		public static System.String TextArea(UnityEngine.Rect position, System.String text, UnityEngine.GUIStyle style)
		{
			return UnityEngine.GUI.TextArea(position, TranslationUtility.get(text), style);
		}

		public static System.String TextArea(UnityEngine.Rect position, System.String text, int maxLength, UnityEngine.GUIStyle style)
		{
			return UnityEngine.GUI.TextArea(position, TranslationUtility.get(text), maxLength, style);
		}

		public static void SetNextControlName(System.String name)
		{
			UnityEngine.GUI.SetNextControlName(name);
		}

		public static System.String GetNameOfFocusedControl()
		{
			return UnityEngine.GUI.GetNameOfFocusedControl();
		}

		public static void FocusControl(System.String name)
		{
			UnityEngine.GUI.FocusControl(name);
		}

		public static bool Toggle(UnityEngine.Rect position, bool value, System.String text)
		{
			return UnityEngine.GUI.Toggle(position, value, TranslationUtility.get(text));
		}

		public static bool Toggle(UnityEngine.Rect position, bool value, UnityEngine.Texture image)
		{
			return UnityEngine.GUI.Toggle(position, value, image);
		}

		public static bool Toggle(UnityEngine.Rect position, bool value, UnityEngine.GUIContent content)
		{
			return UnityEngine.GUI.Toggle(position, value, content);
		}

		public static bool Toggle(UnityEngine.Rect position, bool value, System.String text, UnityEngine.GUIStyle style)
		{
			return UnityEngine.GUI.Toggle(position, value, TranslationUtility.get(text), style);
		}

		public static bool Toggle(UnityEngine.Rect position, bool value, UnityEngine.Texture image, UnityEngine.GUIStyle style)
		{
			return UnityEngine.GUI.Toggle(position, value, image, style);
		}

		public static bool Toggle(UnityEngine.Rect position, bool value, UnityEngine.GUIContent content, UnityEngine.GUIStyle style)
		{
			return UnityEngine.GUI.Toggle(position, value, content, style);
		}

		//NOTE: *NOT* translated
		public static int Toolbar(UnityEngine.Rect position, int selected, System.String[] texts)
		{
			return UnityEngine.GUI.Toolbar(position, selected, texts);
		}

		public static int Toolbar(UnityEngine.Rect position, int selected, UnityEngine.Texture[] images)
		{
			return UnityEngine.GUI.Toolbar(position, selected, images);
		}

		public static int Toolbar(UnityEngine.Rect position, int selected, UnityEngine.GUIContent[] content)
		{
			return UnityEngine.GUI.Toolbar(position, selected, content);
		}

		public static int Toolbar(UnityEngine.Rect position, int selected, System.String[] texts, UnityEngine.GUIStyle style)
		{
			return UnityEngine.GUI.Toolbar(position, selected, texts, style);
		}

		public static int Toolbar(UnityEngine.Rect position, int selected, UnityEngine.Texture[] images, UnityEngine.GUIStyle style)
		{
			return UnityEngine.GUI.Toolbar(position, selected, images, style);
		}

		public static int Toolbar(UnityEngine.Rect position, int selected, UnityEngine.GUIContent[] contents, UnityEngine.GUIStyle style)
		{
			return UnityEngine.GUI.Toolbar(position, selected, contents, style);
		}

		public static int SelectionGrid(UnityEngine.Rect position, int selected, System.String[] texts, int xCount)
		{
			return UnityEngine.GUI.SelectionGrid(position, selected, texts, xCount);
		}

		public static int SelectionGrid(UnityEngine.Rect position, int selected, UnityEngine.Texture[] images, int xCount)
		{
			return UnityEngine.GUI.SelectionGrid(position, selected, images, xCount);
		}

		public static int SelectionGrid(UnityEngine.Rect position, int selected, UnityEngine.GUIContent[] content, int xCount)
		{
			return UnityEngine.GUI.SelectionGrid(position, selected, content, xCount);
		}

		public static int SelectionGrid(UnityEngine.Rect position, int selected, System.String[] texts, int xCount, UnityEngine.GUIStyle style)
		{
			return UnityEngine.GUI.SelectionGrid(position, selected, texts, xCount, style);
		}

		public static int SelectionGrid(UnityEngine.Rect position, int selected, UnityEngine.Texture[] images, int xCount, UnityEngine.GUIStyle style)
		{
			return UnityEngine.GUI.SelectionGrid(position, selected, images, xCount, style);
		}

		public static int SelectionGrid(UnityEngine.Rect position, int selected, UnityEngine.GUIContent[] contents, int xCount, UnityEngine.GUIStyle style)
		{
			return UnityEngine.GUI.SelectionGrid(position, selected, contents, xCount, style);
		}

		public static float HorizontalSlider(UnityEngine.Rect position, float value, float leftValue, float rightValue)
		{
			return UnityEngine.GUI.HorizontalSlider(position, value, leftValue, rightValue);
		}

		public static float HorizontalSlider(UnityEngine.Rect position, float value, float leftValue, float rightValue, UnityEngine.GUIStyle slider, UnityEngine.GUIStyle thumb)
		{
			return UnityEngine.GUI.HorizontalSlider(position, value, leftValue, rightValue, slider, thumb);
		}

		public static float VerticalSlider(UnityEngine.Rect position, float value, float topValue, float bottomValue)
		{
			return UnityEngine.GUI.VerticalSlider(position, value, topValue, bottomValue);
		}

		public static float VerticalSlider(UnityEngine.Rect position, float value, float topValue, float bottomValue, UnityEngine.GUIStyle slider, UnityEngine.GUIStyle thumb)
		{
			return UnityEngine.GUI.VerticalSlider(position, value, topValue, bottomValue, slider, thumb);
		}

		public static float Slider(UnityEngine.Rect position, float value, float size, float start, float end, UnityEngine.GUIStyle slider, UnityEngine.GUIStyle thumb, bool horiz, int id)
		{
			return UnityEngine.GUI.Slider(position, value, size, start, end, slider, thumb, horiz, id);
		}

		public static float HorizontalScrollbar(UnityEngine.Rect position, float value, float size, float leftValue, float rightValue)
		{
			return UnityEngine.GUI.HorizontalScrollbar(position, value, size, leftValue, rightValue);
		}

		public static float HorizontalScrollbar(UnityEngine.Rect position, float value, float size, float leftValue, float rightValue, UnityEngine.GUIStyle style)
		{
			return UnityEngine.GUI.HorizontalScrollbar(position, value, size, leftValue, rightValue, style);
		}

		public static float VerticalScrollbar(UnityEngine.Rect position, float value, float size, float topValue, float bottomValue)
		{
			return UnityEngine.GUI.VerticalScrollbar(position, value, size, topValue, bottomValue);
		}

		public static float VerticalScrollbar(UnityEngine.Rect position, float value, float size, float topValue, float bottomValue, UnityEngine.GUIStyle style)
		{
			return UnityEngine.GUI.VerticalScrollbar(position, value, size, topValue, bottomValue, style);
		}

		public static void BeginGroup(UnityEngine.Rect position)
		{
			UnityEngine.GUI.BeginGroup(position);
		}

		public static void BeginGroup(UnityEngine.Rect position, System.String text)
		{
			UnityEngine.GUI.BeginGroup(position, TranslationUtility.get(text));
		}

		public static void BeginGroup(UnityEngine.Rect position, UnityEngine.Texture image)
		{
			UnityEngine.GUI.BeginGroup(position, image);
		}

		public static void BeginGroup(UnityEngine.Rect position, UnityEngine.GUIContent content)
		{
			UnityEngine.GUI.BeginGroup(position, content);
		}

		public static void BeginGroup(UnityEngine.Rect position, UnityEngine.GUIStyle style)
		{
			UnityEngine.GUI.BeginGroup(position, style);
		}

		public static void BeginGroup(UnityEngine.Rect position, System.String text, UnityEngine.GUIStyle style)
		{
			UnityEngine.GUI.BeginGroup(position, TranslationUtility.get(text), style);
		}

		public static void BeginGroup(UnityEngine.Rect position, UnityEngine.Texture image, UnityEngine.GUIStyle style)
		{
			UnityEngine.GUI.BeginGroup(position, image, style);
		}

		public static void BeginGroup(UnityEngine.Rect position, UnityEngine.GUIContent content, UnityEngine.GUIStyle style)
		{
			UnityEngine.GUI.BeginGroup(position, content, style);
		}

		public static void EndGroup()
		{
			UnityEngine.GUI.EndGroup();
		}

		public static UnityEngine.Vector2 BeginScrollView(UnityEngine.Rect position, UnityEngine.Vector2 scrollPosition, UnityEngine.Rect viewRect)
		{
			return UnityEngine.GUI.BeginScrollView(position, scrollPosition, viewRect);
		}

		public static UnityEngine.Vector2 BeginScrollView(UnityEngine.Rect position, UnityEngine.Vector2 scrollPosition, UnityEngine.Rect viewRect, bool alwaysShowHorizontal, bool alwaysShowVertical)
		{
			return UnityEngine.GUI.BeginScrollView(position, scrollPosition, viewRect, alwaysShowHorizontal, alwaysShowVertical);
		}

		public static UnityEngine.Vector2 BeginScrollView(UnityEngine.Rect position, UnityEngine.Vector2 scrollPosition, UnityEngine.Rect viewRect, UnityEngine.GUIStyle horizontalScrollbar, UnityEngine.GUIStyle verticalScrollbar)
		{
			return UnityEngine.GUI.BeginScrollView(position, scrollPosition, viewRect, horizontalScrollbar, verticalScrollbar);
		}

		public static UnityEngine.Vector2 BeginScrollView(UnityEngine.Rect position, UnityEngine.Vector2 scrollPosition, UnityEngine.Rect viewRect, bool alwaysShowHorizontal, bool alwaysShowVertical, UnityEngine.GUIStyle horizontalScrollbar, UnityEngine.GUIStyle verticalScrollbar)
		{
			return UnityEngine.GUI.BeginScrollView(position, scrollPosition, viewRect, alwaysShowHorizontal, alwaysShowVertical, horizontalScrollbar, verticalScrollbar);
		}

		public static void EndScrollView()
		{
			UnityEngine.GUI.EndScrollView();
		}

		public static void EndScrollView(bool handleScrollWheel)
		{
			UnityEngine.GUI.EndScrollView(handleScrollWheel);
		}

		public static void ScrollTo(UnityEngine.Rect position)
		{
			UnityEngine.GUI.ScrollTo(position);
		}

		public static bool ScrollTowards(UnityEngine.Rect position, float maxDelta)
		{
			return UnityEngine.GUI.ScrollTowards(position, maxDelta);
		}

		public static UnityEngine.Rect Window(int id, UnityEngine.Rect clientRect, UnityEngine.GUI.WindowFunction func, System.String text)
		{
			return UnityEngine.GUI.Window(id, clientRect, func, TranslationUtility.get(text));
		}

		public static UnityEngine.Rect Window(int id, UnityEngine.Rect clientRect, UnityEngine.GUI.WindowFunction func, UnityEngine.Texture image)
		{
			return UnityEngine.GUI.Window(id, clientRect, func, image);
		}

		public static UnityEngine.Rect Window(int id, UnityEngine.Rect clientRect, UnityEngine.GUI.WindowFunction func, UnityEngine.GUIContent content)
		{
			return UnityEngine.GUI.Window(id, clientRect, func, content);
		}

		public static UnityEngine.Rect Window(int id, UnityEngine.Rect clientRect, UnityEngine.GUI.WindowFunction func, System.String text, UnityEngine.GUIStyle style)
		{
			return UnityEngine.GUI.Window(id, clientRect, func, TranslationUtility.get(text), style);
		}

		public static UnityEngine.Rect Window(int id, UnityEngine.Rect clientRect, UnityEngine.GUI.WindowFunction func, UnityEngine.Texture image, UnityEngine.GUIStyle style)
		{
			return UnityEngine.GUI.Window(id, clientRect, func, image, style);
		}

		public static UnityEngine.Rect Window(int id, UnityEngine.Rect clientRect, UnityEngine.GUI.WindowFunction func, UnityEngine.GUIContent title, UnityEngine.GUIStyle style)
		{
			return UnityEngine.GUI.Window(id, clientRect, func, title, style);
		}

		public static UnityEngine.Rect ModalWindow(int id, UnityEngine.Rect clientRect, UnityEngine.GUI.WindowFunction func, System.String text)
		{
			return UnityEngine.GUI.ModalWindow(id, clientRect, func, text);
		}

		public static UnityEngine.Rect ModalWindow(int id, UnityEngine.Rect clientRect, UnityEngine.GUI.WindowFunction func, UnityEngine.Texture image)
		{
			return UnityEngine.GUI.ModalWindow(id, clientRect, func, image);
		}

		public static UnityEngine.Rect ModalWindow(int id, UnityEngine.Rect clientRect, UnityEngine.GUI.WindowFunction func, UnityEngine.GUIContent content)
		{
			return UnityEngine.GUI.ModalWindow(id, clientRect, func, content);
		}

		public static UnityEngine.Rect ModalWindow(int id, UnityEngine.Rect clientRect, UnityEngine.GUI.WindowFunction func, System.String text, UnityEngine.GUIStyle style)
		{
			return UnityEngine.GUI.ModalWindow(id, clientRect, func, TranslationUtility.get(text), style);
		}

		public static UnityEngine.Rect ModalWindow(int id, UnityEngine.Rect clientRect, UnityEngine.GUI.WindowFunction func, UnityEngine.Texture image, UnityEngine.GUIStyle style)
		{
			return UnityEngine.GUI.ModalWindow(id, clientRect, func, image, style);
		}

		public static UnityEngine.Rect ModalWindow(int id, UnityEngine.Rect clientRect, UnityEngine.GUI.WindowFunction func, UnityEngine.GUIContent content, UnityEngine.GUIStyle style)
		{
			return UnityEngine.GUI.ModalWindow(id, clientRect, func, content, style);
		}

		public static void DragWindow(UnityEngine.Rect position)
		{
			UnityEngine.GUI.DragWindow(position);
		}

		public static void DragWindow()
		{
			UnityEngine.GUI.DragWindow();
		}

		public static void BringWindowToFront(int windowID)
		{
			UnityEngine.GUI.BringWindowToFront(windowID);
		}

		public static void BringWindowToBack(int windowID)
		{
			UnityEngine.GUI.BringWindowToBack(windowID);
		}

		public static void FocusWindow(int windowID)
		{
			UnityEngine.GUI.FocusWindow(windowID);
		}

		public static void UnfocusWindow()
		{
			UnityEngine.GUI.UnfocusWindow();
		}
	}

#pragma warning restore 618
}