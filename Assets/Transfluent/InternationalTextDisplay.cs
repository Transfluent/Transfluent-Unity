using UnityEngine;

public class InternationalTextDisplay : MonoBehaviour
{
	[SerializeField] string textToDisplay = "我是一个中国人的一句。";
	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	void OnGUI()
	{
		GUILayout.TextField(textToDisplay);
	}
}
