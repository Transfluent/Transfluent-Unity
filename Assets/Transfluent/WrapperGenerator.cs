using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Assets.Transfluent
{
	class WrapperGenerator
	{
		[MenuItem("Window/Test Generating GUI file")]
		public void test()
		{
			Type type = typeof(UnityEngine.GUI);
			MemberInfo[] members = type.FindMembers(MemberTypes.Property,
					BindingFlags.Static | BindingFlags.FlattenHierarchy | BindingFlags.SetProperty | BindingFlags.Public, null, null);

			foreach(MemberInfo member in members)
			{
				Debug.Log("Member name:" + member.Name);
			}


			StringBuilder funcitons = new StringBuilder();
			MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static);
			List<MethodInfo> specialMethods = new List<MethodInfo>();
			foreach(MethodInfo methodInfo in methods)
			{
				//skip the getters/setters for now
				if(methodInfo.IsSpecialName)
				{
					specialMethods.Add(methodInfo);
					continue;
				}
				string functionDef = new MethodRepresentation(methodInfo).functionString;
				funcitons.Append(functionDef);
				Debug.Log(functionDef);
				StringBuilder sb = new StringBuilder("name:" + methodInfo.Name + " returns:" + methodInfo.ReturnType);
				ParameterInfo[] myParameters = methodInfo.GetParameters();
				sb.Append(" (");
				//foreach (ParameterInfo paramInfo in myParameters)
				for(int i = 0; i < myParameters.Length; i++)
				{
					ParameterInfo paramInfo = myParameters[i];
					sb.Append(paramInfo.ParameterType + " " + paramInfo.Name);
					if(paramInfo.IsOptional)
					{
						sb.Append("=" + paramInfo.DefaultValue);
					}
					sb.Append(",");
				}
				//TODO: remove final comma
				sb.Append(")");
				//TODO: default values
				Debug.Log(sb.ToString());
				// methodInfo.Name
			}
			string header = @"using UnityEngine;

//wrapper around unity's gui, except to grab text as quickly as possbile and spit it into an internal db
//http://docs.unity3d.com/Documentation/ScriptReference/GUI.html
namespace transfluent
{

	public partial class GUI
	{";
			string footer = @"	}
}";
			Debug.Log(string.Format("{0}\n{1}\n{2}", header, funcitons, footer));
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
