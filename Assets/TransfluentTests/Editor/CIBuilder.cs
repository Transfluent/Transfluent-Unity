using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using UnityTest;

public class CIBuilder : ScriptableObject
{
	private static int GetBuildNumberFromCommandLine()
	{
		const string buildFlag = "-BUILD_NUMBER=";

		try
		{
			string[] args = Environment.GetCommandLineArgs();
			foreach(string arg in args)
			{
				if(arg.Contains(buildFlag))
				{
					string buildNumber = arg.Replace(buildFlag, "");


					return int.Parse(buildNumber);
				}
			}
		}
		catch(Exception e)
		{
			Debug.LogError("Error setting bundle number;"+ e);
			throw;
		}
		return 0; //not from command line
	}
	[MenuItem("Window/build")]
	public static void manualBuild()
	{
		var build = new BuilderInstance();
		build.RunTests();
		build.Build();
	}
	public class BuilderInstance
	{
		public string appName = "TransfluentEditor";
		public string buildDirectoryPath = "build";
		public readonly string projectPath = Path.GetFullPath(Application.dataPath + Path.DirectorySeparatorChar + "..");

		public int autoBuildNumber = 0;//build number passed from build machine, not marketing version -- 0.2, etc
		public string marketingBuildNumber = "0.1";//major, minor, patchlevel

		public string pathToPackage = "Assets/Transfluent";
		//run tests, export package
		//TODO: make sure that package compiles without test directory -- build a dll
		//TODO: export test results
		public void RunTests()
		{
			UnitTestView.RunAllTestsBatch();
		}
		public void Build()
		{
			string targetBuildPath = projectPath + Path.DirectorySeparatorChar + buildDirectoryPath;
			if (!Directory.Exists(targetBuildPath))
				Directory.CreateDirectory(targetBuildPath);
			AssetDatabase.ExportPackage(pathToPackage,targetBuildPath+Path.DirectorySeparatorChar+appName+autoBuildNumber+".unitypackage",ExportPackageOptions.Recurse);
			
		}
	}
}