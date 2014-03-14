using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Assets.Transfluent
{
	internal class WrapperGenerator
	{
		private const string headerFormat = @"using UnityEngine;
using System;
//wrapper around unity's gui, except to grab text as quickly as possbile and spit it into an internal db
//http://docs.unity3d.com/Documentation/ScriptReference/GUI.html
namespace transfluent.guiwrapper
{{
#pragma warning disable 618
	public partial class {0}
	{{";

		private const string footer = @"	}
#pragma warning restore 618
}";

		private static readonly Dictionary<Type, string> objectToPrimitiveNameMap = new Dictionary<Type, string>
		{
			{typeof (bool), "bool"},
			{typeof (int), "int"},
			{typeof (float), "float"},
			{typeof (byte), "byte"},
			{typeof (double), "double"},
			{typeof (char), "char"},
			{typeof (void), "void"},
		};

		public bool debug = false;

		[MenuItem("Window/TypeTest")]
		public static void typeTest()
		{
			Debug.Log(typeof(bool));
		}

		//The primitive types are Boolean, Byte, SByte, Int16, UInt16, Int32, UInt32, Int64, UInt64, IntPtr, UIntPtr, Char, Double, and Single.

		public string getWrappedFile(Type type)
		{
			string forwardToType = type.FullName; // "UnityEngine."+ type.Name;
			forwardToType = forwardToType.Replace("+", ".");
			//handle inner classes per http://msdn.microsoft.com/en-us/library/w3f99sx1(v=vs.110).aspx
			PropertyInfo[] properties =
				type.GetProperties(BindingFlags.Static | BindingFlags.FlattenHierarchy | BindingFlags.SetProperty |
								   BindingFlags.GetProperty | BindingFlags.Public);

			var gettersSetters = new StringBuilder();
			foreach(PropertyInfo property in properties)
			{
				//string propForward = "UnityEngine." + forwardToType;//not sue why the method info works and this doesn'
				if(property.CanRead || property.CanWrite)
				{
					string name = property.Name;
					string possibleGetter = property.CanRead ? string.Format("get {{ return {0}.{1}; }}", forwardToType, name) : "";
					string possibleSetter = property.CanWrite ? string.Format("set {{ {0}.{1} = value; }}", forwardToType, name) : "";

					string stringProp = string.Format("\n public static {0} {1} {{\n {2}\n {3}\n}}", property.PropertyType,
						property.Name, possibleGetter, possibleSetter);
					if(debug) Debug.Log("Prop: " + stringProp);
					gettersSetters.Append(stringProp);
				}
			}

			var funcitons = new StringBuilder();
			MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static);
			var specialMethods = new List<MethodInfo>();
			foreach(MethodInfo methodInfo in methods)
			{
				if(methodInfo.IsSpecialName) //do not add the getters/setters, as those are added another way
				{
					specialMethods.Add(methodInfo);
					continue;
				}

				string functionDef = new MethodRepresentation(methodInfo, forwardToType).functionString;
				funcitons.Append(functionDef);
				if(debug) Debug.Log(functionDef);
			}
			string header = string.Format(headerFormat, type.Name);
			string fullFile = string.Format("{0}\n{1}\n{2}\n{3}", header, gettersSetters, funcitons, footer);
			fullFile = fullFile.Replace("System.Single&", "float"); //ugh
			fullFile = fullFile.Replace("System.Void", "void");
			return fullFile.Replace("\r\n", "\n"); //could do the other way around, but just want the line endings to be the same
		}

		[MenuItem("Window/Test Generating GUI file")]
		public static void test()
		{
			generateSourceFromType("Assets/Transfluent/GUI.cs", typeof(GUI));
			generateSourceFromType("Assets/Transfluent/GUILayout.cs", typeof(GUILayout));
			generateSourceFromType("Assets/Transfluent/Editor/EditorGUI.cs", typeof(EditorGUI));
			generateSourceFromType("Assets/Transfluent/Editor/EditorGUILayout.cs", typeof(EditorGUILayout));
		}

		private static void generateSourceFromType(string file, Type type)
		{
			var generator = new WrapperGenerator();
			string guiFileText = generator.getWrappedFile(type);

			//Debug.Log(guiFileText);

			FileUtil.DeleteFileOrDirectory(file);
			File.WriteAllText(file, guiFileText);
			AssetDatabase.SaveAssets();
			AssetDatabase.ImportAsset(file);
			AssetDatabase.Refresh();
		}

		public class MethodRepresentation
		{
			private readonly string typeThatWeAreForwardingTo;
			public string functionString;

			public MethodRepresentation(MethodInfo methodInfo, string unitysTargetType)
			{
				typeThatWeAreForwardingTo = unitysTargetType;

				var parameters = new List<ParamWrapped>();
				var sb = new StringBuilder("name:" + methodInfo.Name + " returns:" + methodInfo.ReturnType);
				ParameterInfo[] myParameters = methodInfo.GetParameters();
				sb.Append(" (");

				for(int i = 0; i < myParameters.Length; i++)
				{
					ParameterInfo paramInfo = myParameters[i];
					//TODO: use paramInfo.ParameterType.IsPrimitive to make System.Void be just void
					var toAdd = new ParamWrapped
					{
						defaultValue = paramInfo.DefaultValue.ToString(),
						isOptional = paramInfo.IsOptional,
						name = paramInfo.Name,
						type = paramInfo.ParameterType,
						isParams = paramInfo.GetCustomAttributes(typeof(ParamArrayAttribute), false).Length > 0,
						//http://stackoverflow.com/questions/627656/determining-if-a-parameter-uses-params-using-reflection-in-c
						isRef = paramInfo.ParameterType.IsByRef,
					};

					parameters.Add(toAdd);
				}
				//TODO: ensure default values are handled appropriately
				functionString = createRealParamString(methodInfo.Name, methodInfo.ReturnType, parameters.ToArray(),
					obsoleteString(methodInfo));
			}

			private string obsoleteString(MethodInfo methodInfo)
			{
				var ObsolteAttributes = methodInfo.GetCustomAttributes(typeof(ObsoleteAttribute), false) as ObsoleteAttribute[];
				if(ObsolteAttributes.Length <= 0)
					return "";
				//only return the first "obsolte" attribute
				return "[Obsolete(\"" + ObsolteAttributes[0].Message + "\")]\n";
			}

			private string createRealParamString(string methodName, Type returnType, ParamWrapped[] parameters, string attributes)
			{
				var paramBuilder = new StringBuilder();
				var valuesToPassToRealFunction = new StringBuilder();
				for(int i = 0; i < parameters.Length; i++)
				{
					ParamWrapped paramInfo = parameters[i];
					string specialModifierString = "";
					if(paramInfo.isRef)
						specialModifierString = "ref";
					if(paramInfo.isParams)
						specialModifierString = "params";
					paramBuilder.Append(string.Format("{0} {1} {2}", specialModifierString, cleanType(paramInfo.type), paramInfo.name));

					valuesToPassToRealFunction.Append(paramInfo.isRef ? "ref " + paramInfo.name : paramInfo.name);

					if(paramInfo.isOptional)
					{
						paramBuilder.Append(string.Format("={0}", paramInfo.defaultValue));
						//how are strings handled in this?  "" vs just a blank
					}
					if(i != parameters.Length - 1)
					{
						valuesToPassToRealFunction.Append(",");
						paramBuilder.Append(",");
					}
				}
				string optionallyReturnTheValue = returnType == typeof(void) ? "" : "return ";
				string functionFormatted = string.Format("{6}public static {0} {1}({2})\n{{\n {4} {5}.{1}({3});\n}}\n",
					cleanType(returnType), methodName, paramBuilder, valuesToPassToRealFunction, optionallyReturnTheValue,
					typeThatWeAreForwardingTo, attributes);
				return functionFormatted;
			}

			private string cleanType(Type type)
			{
				if(type.IsByRef)
				{
					if(type.Name == "Single&")
					{
						return objectToPrimitiveNameMap[typeof(float)];
					}
					Debug.LogWarning("Likely missing a reference type:" + type.Name);
				}
				if(type.IsPrimitive)
				{
					//Debug.Log("GETTING TYPE:"+type.Name);
					try
					{
						return objectToPrimitiveNameMap[type];
					}
					catch(Exception e)
					{
						throw new Exception("Tried looking up an unmapped primitive of type:" + type + " ", e);
					}
				}

				return type.FullName.Replace("+", ".");
			}

			private struct ParamWrapped
			{
				public string defaultValue;
				public bool isOptional;
				public bool isParams;
				public bool isRef;
				public string name;
				public Type type;
			}
		}
	}
}