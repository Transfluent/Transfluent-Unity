//Simple interface for providing the current text and interacting with the localization api
using System;
using transfluent;

[Serializable]
public class LocalizeUtil
{
	//[SerializeField]
	private string _current;
	public string globalizationKey;

	public string current
	{
		get
		{
			if(string.IsNullOrEmpty(globalizationKey))
				return "";
			return _current ?? (_current = TranslationUtility.get(globalizationKey));
		}
		//set { globalizationKey = _current; }
	}

	public string OnLocalize()
	{
		_current = null;
		return current;
	}

	public static implicit operator string(LocalizeUtil util)
	{
		return util.current;
	}

	public override string ToString()
	{
		return current;
	}
}