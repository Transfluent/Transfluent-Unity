Transfluent-Unity
=================

Transfluent's Unity-plugin.


# Transfluent Unity Plugin
This is a tool used for translating textual assets within Unity 3D, integrating into various UI frameworks, and getting translations for game text.

## Getting started:
[Download](https://github.com/Transfluent/Transfluent-Unity/blob/master/Release/2.0/TransfluentEditor-2.0.unitypackage) the most current unity package (currently 2.0) and double click the package to import the package into your project.

### Configure the game - source and destination languages
Select the menu "Transfluent/Game Configuration".  Set your source language, and add/remove any destination languages.

### Example script(OnGUI):

~~~~~~~~
using UnityEngine;
using System.Collections.Generic;
using GUILayout = transfluent.guiwrapper.GUILayout; 
using transfluent;
public class ExampleTransfluent : MonoBehaviour
{
	void Start()
	{
		TransfluentUtility.changeStaticInstanceConfig("zh-cn"); //you want to get this from playerprefs or some other part of your configuration settings, most likely
	}

	void OnGUI()
	{
		GUILayout.Label("hello");
	}
}
~~~~~~~~

###Programmatic interface
####TransfluentUtility 
* changeStaticInstanceConfig(string destinationLanguageCode = "", string translationGroup = "")  
  * Configures the static translation instance to return text in the destination language, and alternatively a separate translation group (think of it like a namespace -- text keys won't clash)   
* getTranslation(string sourceText)
  * Given the original string(or key), the translation tool will return the text for in the language specified by the changeStaticInstanceConfig
* public static string getFormatted(string sourceText, params object[] formatStrings)
* *    A convenience function for handling text with dynamic elements (player name, number of items, etc). Gets formatted text strings in the form of getFormatted("Hello, {0}",username);

## GUI, GUILayout wrappers
* If you are using OnGUI for displaying your text, all you need to do is prefix all files with the lines:
~~~~~~~~
using GUI = transfluent.guiwrapper.GUI;
using GUILayout = transfluent.guiwrapper.GUILayout; 
~~~~~~~~
And the rest of your GUILayout and GUI function calls will automatically use the transfluent API to translate text.

## Files in package:
* Transfluent:
  * Editor -- editor tools - editor windows, custom inspectors, and other utilities to perform operations only relevant to initial setup
  * Examples - Sample scenes, scripts showing how programatic localization(DIY) and UnityGUI(OnGUI, Textmesh) work
  * NGUIExtensions - tools to help use Transfluent data with the NGUI "Globalization" component. Import/export to and from the NGUI localization CSV.
  * Plugins - Code shared by both runtime and editor time scripts - serialization, web service interfaces
  * Resources - Data related to the configuration of your game (ie Source language: english, destination languages: Korean, Chinese, Japanese), and listings of individual translations.
  * Runtime - Interfaces to game code live in this folder -- these are the things you will be using directly