using System;
using System.Collections.Generic;
using System.Reflection;

namespace transfluent
{
	//The most simplistic IOC style mapping that I can think of.  Requires you to call context.setMappings on objects before using them, and currently does not support setting injection on sub objects, an item I will likely fix ASAP
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public class Inject : Attribute
	{
		public bool isNamed { get; private set; }
		public NamedInjections name;

		public Inject()
		{
		}

		public Inject(NamedInjections injectionName)
		{
			isNamed = true;
			name = injectionName;
		}
	}

	public class UnboundInjectionException : Exception
	{
		public UnboundInjectionException()
		{
		}

		public UnboundInjectionException(string message) : base(message)
		{
		}
	}

	//right now only singleton mapping :(
	public class InjectionContext
	{
		private readonly Dictionary<Type, object> injectionMap = new Dictionary<Type, object>();

		private readonly Dictionary<string, Dictionary<Type, object>> namedInjectionMap =
			new Dictionary<string, Dictionary<Type, object>>();

		private void addMapping(Type typeToHandle, object valueToPutIn)
		{
			injectionMap.Add(typeToHandle, valueToPutIn);
		}

		public void addMapping<T>(object valueToPutIn)
		{
			addMapping(typeof (T), valueToPutIn);
		}

		private void addNamedMapping<T>(string name, object valueToPutIn)
		{
			if (!namedInjectionMap.ContainsKey(name))
				namedInjectionMap.Add(name, new Dictionary<Type, object>());
			namedInjectionMap[name].Add(typeof(T), valueToPutIn);
		}

		public void removeNamedMapping<T>(NamedInjections namedInjection)
		{
			string name = namedInjection.ToString();
			
			if(!namedInjectionMap.ContainsKey(name))
				namedInjectionMap.Add(name, new Dictionary<Type, object>());
			if (namedInjectionMap[name].ContainsKey(typeof (T)))
			{
				namedInjectionMap[name].Remove(typeof (T));
			}
		}

		public void addNamedMapping<T>(NamedInjections name, object valueToPutIn)
		{
			addNamedMapping<T>(name.ToString(), valueToPutIn);
		}

		public T manualGetMapping<T>() where T : class
		{
			return injectionMap[typeof (T)] as T;
		}

		public T manualGetMapping<T>(NamedInjections name) where T : class
		{
			return namedInjectionMap[name.ToString()][typeof (T)] as T;
		}

		public void setMappings(object toInject)
		{
			setMappings(toInject, toInject.GetType());
		}

		public void setMappings(object toInject, Type type)
		{
			MemberInfo[] members = type.FindMembers(MemberTypes.Property,
				BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.SetProperty | BindingFlags.Public, null, null);

			foreach (MemberInfo member in members)
			{
				object[] injections = member.GetCustomAttributes(typeof (Inject), true);
				if (injections.Length > 0)
				{
					var propertyInfo = member as PropertyInfo;
					var injectionAttribute = injections[0] as Inject; //TOOD: handle named stuff?
					Type typeToInject = propertyInfo.PropertyType;
					try
					{
						Dictionary<Type, object> injectionMapToUse = injectionMap;
						if (injectionAttribute.isNamed)
							injectionMapToUse = namedInjectionMap[injectionAttribute.name.ToString()];


						object valueToInject = injectionMapToUse[typeToInject];
						propertyInfo.SetValue(toInject, valueToInject, null);
					}
					catch (KeyNotFoundException k)
					{
						throw new UnboundInjectionException("Injection not set for type:" + typeToInject.Name +
						                                    " when trying to set on a sub objectnamed:" + injectionAttribute.name +
						                                    " core exception:" + k);
					}
				}
			}
		}
	}
}