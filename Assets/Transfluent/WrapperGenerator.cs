using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Assets.Transfluent
{
	class WrapperGenerator
	{
		const string headerFormat = @"using UnityEngine;

//wrapper around unity's gui, except to grab text as quickly as possbile and spit it into an internal db
//http://docs.unity3d.com/Documentation/ScriptReference/GUI.html
namespace transfluent
{{

	public partial class {0}
	{{";
		private const string footer = @"	}
}";
		public bool debug = false;
		public WrapperGenerator(){}
			
		public string getWrappedFile(Type type)
		{
			PropertyInfo[] properties = type.GetProperties(BindingFlags.Static | BindingFlags.FlattenHierarchy | BindingFlags.SetProperty | BindingFlags.GetProperty| BindingFlags.Public);

			StringBuilder gettersSetters = new StringBuilder();
			foreach(PropertyInfo property in properties)
			{
				if (property.CanRead || property.CanWrite)
				{
					string name = property.Name;
					string possibleGetter = property.CanRead ? string.Format("get {{ return UnityEngine.GUI.{0}; }}", name) : "";
					string possibleSetter = property.CanWrite ? string.Format("set {{ UnityEngine.GUI.{0} = value; }}",name) : "";

					string stringProp = string.Format("\n public static {0} {1} {{\n {2}\n {3}\n}}",property.PropertyType,  property.Name, possibleGetter, possibleSetter);
					if(debug) Debug.Log("Prop: " + stringProp);
					gettersSetters.Append(stringProp);
				}
			}

			StringBuilder funcitons = new StringBuilder();
			MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static);
			List<MethodInfo> specialMethods = new List<MethodInfo>();
			foreach(MethodInfo methodInfo in methods)
			{
				if(methodInfo.IsSpecialName) //do not add the getters/setters, as those are added another way
				{
					specialMethods.Add(methodInfo);
					continue;
				}
				string functionDef = new MethodRepresentation(methodInfo).functionString;
				funcitons.Append(functionDef);
				if(debug) Debug.Log(functionDef);
			}
			string header = string.Format(headerFormat, type.Name);
			string fullFile = string.Format("{0}\n{1}\n{2}\n{3}", header, gettersSetters, funcitons, footer);

			return fullFile.Replace("\r\n", "\n"); //could do the other way around, but just want the line endings to be the same
		}

		[MenuItem("Window/Test Generating GUI file")]
		public static void test()
		{
			var generator = new WrapperGenerator();
			var guiFileText = generator.getWrappedFile(typeof(UnityEngine.GUI));
			Debug.Log(guiFileText);

			FileUtil.DeleteFileOrDirectory("Assets/Transfluent/GUI.cs");
			File.WriteAllText("Assets/Transfluent/GUI.cs",guiFileText);
			AssetDatabase.SaveAssets();
			AssetDatabase.ImportAsset("Assets/Transfluent/GUI.cs");
			AssetDatabase.Refresh();
		}

		public class MethodRepresentation
		{
			struct ParamWrapped
			{
				public Type type;
				public string name;
				public bool isOptional;
				public string defaultValue;
			}

			public string functionString;

			public MethodRepresentation(MethodInfo methodInfo)
			{
				List<ParamWrapped> parameters = new List<ParamWrapped>();
				StringBuilder sb = new StringBuilder("name:" + methodInfo.Name + " returns:" + methodInfo.ReturnType);
				ParameterInfo[] myParameters = methodInfo.GetParameters();
				sb.Append(" (");
				//foreach (ParameterInfo paramInfo in myParameters)
				for(int i = 0; i < myParameters.Length; i++)
				{
					ParameterInfo paramInfo = myParameters[i];
					//TODO: use paramInfo.ParameterType.IsPrimitive to make System.Void be just void

					ParamWrapped toAdd = new ParamWrapped()
					{
						defaultValue = paramInfo.DefaultValue.ToString(),
						isOptional = paramInfo.IsOptional,
						name = paramInfo.Name,
						type = paramInfo.ParameterType
					};
					parameters.Add(toAdd);
				}
				//TODO: ensure default values are handled appropriately
				functionString = createRealParamString(methodInfo.Name, methodInfo.ReturnType, parameters.ToArray());
			}
			string createRealParamString(string methodName, Type returnType, ParamWrapped[] parameters)
			{
				StringBuilder paramBuilder = new StringBuilder();
				StringBuilder valuesToPassToRealFunction = new StringBuilder();
				for(int i = 0; i < parameters.Length; i++)
				{
					ParamWrapped paramInfo = parameters[i];
					paramBuilder.Append(string.Format("{0} {1}", cleanType(paramInfo.type), paramInfo.name));
					valuesToPassToRealFunction.Append(paramInfo.name);

					if(paramInfo.isOptional)
					{
						paramBuilder.Append(string.Format("={0}", paramInfo.defaultValue));  //how are strings handled in this?  "" vs just a blank
					}
					if(i != parameters.Length - 1)
					{
						valuesToPassToRealFunction.Append(",");
						paramBuilder.Append(",");
					}
				}
				string optionallyReturnTheValue = returnType == typeof(void) ? "" : "return ";
				string functionFormatted = string.Format("{0} {1}({2})\n{{\n {4} UnityEngine.GUI.{1}({3});\n}}\n",
					cleanType(returnType), methodName, paramBuilder, valuesToPassToRealFunction, optionallyReturnTheValue);
				return functionFormatted;
			}
			string cleanType(Type type)
			{
				return type.FullName.Replace("+", ".");
			}
		}
	}
}
