using UnityEditor;
using transfluent;

public class TestEditorWindow : EditorWindow
{
	private TestEditorWindowMediator _mediator;
	static void Init()
	{
		GetWindow<TestEditorWindow>();
	}

	public TestEditorWindow()
	{
		_mediator = new TestEditorWindowMediator();
	}

	void OnGUI()
	{
		
	}
}
