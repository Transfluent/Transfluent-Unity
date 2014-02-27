using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;

namespace transfluent
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public class Inject : Attribute
	{
		public string name;

		public Inject()
		{

		}

		public Inject(string injectionName)
		{
			name = injectionName;
		}
	}

	public class UnboundInjectionException : Exception
	{
		public UnboundInjectionException() : base(){}
		public UnboundInjectionException(string message) : base(message) { }
	}

	//right now only singleton mapping :(
	public class InjectionContext
	{
		private Dictionary<Type, object> injectionMap = new Dictionary<Type, object>();
		private Dictionary<string,Dictionary<Type,object>> namedInjectionMap = new Dictionary<string, Dictionary<Type, object>>(); 

		public void addMapping(Type typeToHandle, object valueToPutIn)
		{
			injectionMap.Add(typeToHandle, valueToPutIn);
		}

		public void addMapping<T>(object valueToPutIn)
		{
			addMapping(typeof(T),valueToPutIn);
		}

		public void addNamedMapping<T>(string name, object valueToPutIn)
		{
			if(!namedInjectionMap.ContainsKey(name))
				namedInjectionMap.Add(name,new Dictionary<Type, object>());
			namedInjectionMap[name].Add(valueToPutIn.GetType(),valueToPutIn);
		}

		public void setMappings(object toInject)
		{
			setMappings(toInject, toInject.GetType());
		}
		public void setMappings(object toInject, Type type)
		{
			MemberInfo[] members = type.FindMembers(MemberTypes.Property, BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.SetProperty | BindingFlags.Public, null, null);

			foreach(MemberInfo member in members)
			{
				object[] injections = member.GetCustomAttributes(typeof(Inject), true);
				if(injections.Length > 0)
				{
					var propertyInfo = member as PropertyInfo;
					var injectionAttribute = injections[0] as Inject; //TOOD: handle named stuff?
					Type typeToInject = propertyInfo.PropertyType;
					try
					{
						var injectionMapToUse = injectionMap;
						if (!string.IsNullOrEmpty(injectionAttribute.name))
							injectionMapToUse = namedInjectionMap[injectionAttribute.name];

						object valueToInject = injectionMapToUse[typeToInject];
						propertyInfo.SetValue(toInject, valueToInject, null);
					}
					catch (KeyNotFoundException k)
					{
						throw new UnboundInjectionException("Injeciton not set for type:" + typeToInject.Name + " when trying to set on a sub object");
					}
					
				}
			}
		}
	}

}