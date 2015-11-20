﻿using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityTest;

namespace transfluent
{
	public class CIBuilder
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
				Debug.LogError("Error setting bundle number;" + e);
				throw;
			}
			return 0; //not from command line
		}

		[MenuItem("Translation/internal/build")]
		public static void manualBuild()
		{
			var build = new BuilderInstance();
			build.autoBuildNumber = GetBuildNumberFromCommandLine();
			//build.RunTests();
			build.Build();
			string basePath = Path.GetFullPath(Application.dataPath + ".." + Path.DirectorySeparatorChar);
			string docsPath = basePath + Path.DirectorySeparatorChar + "docs";
			if(Directory.Exists(docsPath))
			{
				Directory.CreateDirectory(docsPath);
			}
		}

		public class BuilderInstance
		{
			public readonly string projectPath = Path.GetFullPath(Application.dataPath + Path.DirectorySeparatorChar + "..");
			public string appName = "TransfluentEditor";

			public int autoBuildNumber = 0; //build number passed from build machine, not marketing version -- 0.2, etc
			public string buildDirectoryPath = "build";
			public string marketingBuildNumber = "0.3"; //major, minor, patchlevel

			public string pathToPackage = "Assets/Transfluent";

			//run tests, export package
			//TODO: make sure that package compiles without test directory -- build a dll
			public void RunTests()
			{
				//UnitTestView.RunAllTestsBatch();
			}

			public void Build()
			{
				string targetBuildPath = projectPath + Path.DirectorySeparatorChar + buildDirectoryPath;
				if(!Directory.Exists(targetBuildPath))
					Directory.CreateDirectory(targetBuildPath);
				string fileLocation = string.Format("{0}-{1}.unitypackage", targetBuildPath + Path.DirectorySeparatorChar + appName,
					autoBuildNumber);
				AssetDatabase.ExportPackage(pathToPackage, fileLocation, ExportPackageOptions.Recurse);
			}
		}
	}
}