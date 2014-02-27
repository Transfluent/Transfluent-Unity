using System.IO;
using Pathfinding.Serialization.JsonFx;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using Microsoft.CSharp;
using System.CodeDom.Compiler;

namespace transfluent
{
	//NOTE: REQUIRES /Applications/Unity/Unity.app/Contents/Frameworks/Mono/bin to be in your path!  I'm sure I can work around this... but not now
	public class StaticDLLBuilder
	{
#if UNITY_EDITOR_OSX
		public static readonly string managedDLLsPath = "Frameworks/Managed/";
		#else
		public static readonly string managedDLLsPath = "Managed"+Path.DirectorySeparatorChar;
#endif
		public static readonly string unityLibraryPathRootFolder = Path.Combine(EditorApplication.applicationContentsPath, managedDLLsPath);
		public static readonly string UnityEngineDll = unityLibraryPathRootFolder + "UnityEngine.dll";
		public static readonly string UnityEditorDll = unityLibraryPathRootFolder + "UnityEditor.dll";

		[MenuItem("Window/build dll")]
		public static void buildDLL()
		{
			string baseProjectPath = Path.GetFullPath(Application.dataPath + Path.DirectorySeparatorChar + ".." );

			DLLBuilder builder = new DLLBuilder()
			{
				linkedAssemblies = new List<string>()
				{
					UnityEngineDll,
					UnityEditorDll
				},
				sourcePath = Application.dataPath + Path.DirectorySeparatorChar + "Transfluent",
				targetName = "TransfluentDLL",
				targetPath=baseProjectPath+Path.DirectorySeparatorChar+"build"
			};
			builder.Build();
		}
	}
	public class DLLBuilder
	{
		public string sourcePath { get; set; }
		public string targetName { get; set; }
		public string targetPath { get; set; }

		public List<string> linkedAssemblies { get; set; }

		public void Build()
		{
			CompilerParameters options = new CompilerParameters();
			options.OutputAssembly = string.Format("{0}{1}{2}.dll", targetPath, Path.DirectorySeparatorChar+"",targetName);
			options.CompilerOptions = "/optimize";

			//ReferencedAssemblies
			List<string> allSourceCSFiles = new List<string>();
			getAllSourceFilesInDir(sourcePath, allSourceCSFiles);

			var allDlls = new List<string>(linkedAssemblies);
			getDllsInDir(sourcePath, allDlls);
			allDlls.ForEach((string dllPath)=> { options.ReferencedAssemblies.Add(dllPath); });

			var compileOptions = new Dictionary<string, string>() {{"CompilerVersion", "v3.0"}};
			var cSharpCodeProvider = new CSharpCodeProvider(compileOptions);
			Debug.Log("ALL library dll FILES:" + JsonWriter.Serialize(allDlls));
			Debug.Log("ALL SOURCE FILES:" + JsonWriter.Serialize(allSourceCSFiles));

			Debug.Log("OUTPUT PATH:" + options.OutputAssembly);
			allSourceCSFiles.ForEach((string path) =>
			{
				if(!File.Exists(path))
					Debug.LogError("BOGUS FILE PATH:"+ path);
			});
			allDlls.ForEach((string path) =>
			{
				if(!File.Exists(path))
					Debug.LogError("BOGUS FILE DLL PATH:" + path);
			});
			var compilerResults = cSharpCodeProvider.CompileAssemblyFromSource(options, allSourceCSFiles.ToArray());
			if (compilerResults.Errors.HasErrors)
			{
				foreach (CompilerError error in compilerResults.Errors)
				{
					if (!error.IsWarning)
						Debug.LogError("Error compiling dll:" + error);
				}
			}
			else
			{
				Debug.Log("SUCCESSFULLY CREATED DLL at :"+options.OutputAssembly);
			}
		}

		public void getAllSourceFilesInDir(string directory, List<string> listToAddTo )
		{
			getAllFilesWithPatternInDir(directory, listToAddTo, "*.cs");
		}
		public void getDllsInDir(string directory, List<string> listToAddTo)
		{
			getAllFilesWithPatternInDir(directory, listToAddTo, "*.dll");
		}

		public void getAllFilesWithPatternInDir(string directory, List<string> listToAddTo, string searchPattern)
		{
			foreach(var file in Directory.GetFiles(directory, searchPattern))
			{
				listToAddTo.Add(file);
			}
			foreach(var dir in Directory.GetDirectories(directory))
			{
				getAllFilesWithPatternInDir(dir, listToAddTo,searchPattern);
			}
		}
	}

}
