using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace transfluent
{
	public class WordReverser
	{
		public string reverseString(string str)
		{
			string[] words = str.Split(new[] { " " }, StringSplitOptions.None);
			var sb = new StringBuilder();
			for(int i = words.Length - 1; i >= 0; i--)
			{
				string word = words[i];

				char[] charArray = word.ToCharArray();
				IEnumerable<char> walker = charArray.Reverse();
				foreach(char t in walker)
				{
					sb.Append(t);
				}
				if(i != 0)
					sb.Append(" ");
			}

			return sb.ToString();
		}
	}
}