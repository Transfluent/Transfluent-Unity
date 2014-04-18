Transfluent-Unity
=================

Transfluent's Unity-plugin.


# Transfluent Unity Plugin
This is a tool used for translating textual assets within Unity 3D, integrating into various UI frameworks, and getting translations for game text.

## Getting started:
[Download](https://github.com/Transfluent/Transfluent-Unity/blob/master/Release/2.0/TransfluentEditor-2.0.unitypackage) the most current unity package (currently 2.0) and double click the package to import the package into your project.

### Configure the game - source and destination languages
Select the menu "Transfluent/Game Configuration".  Set your source language, and add/remove any destination languages.

#Example translated project: https://github.com/hardcoded2/strangerocks
This is a tech demo project that has text embedded in OnGUI scripts, prefabs, and in scene files.  
###Script-based migration (get all the text out of your game objects and into the translation db)
By altering the GameSpecificMigration file to handle managed code, you can handle the migration process scripts that set values on start or awake.  This will also let the rest of the migration process know not to touch those managed text fields.  
###Alter OnGUI Scripts and other formatted text fields
If you're using OnGUI to display text in your app, make sure to add these two lines to the top of your file:
using GUILayout = transfluent.guiwrapper.GUILayout; 
using GUI = transfluent.guiwrapper.GUI; 

This will automatically translate the *literal* text sent to GUI and GUILabel.  If you have text fields that combine strings (ie "Hello {0}, how are you today?"), then you will want to use the TranslationUtility.getFormatted("Hello {0}, how are you today?",adventurerName); so that the utility knows how to format that string for future translations.(also so we're not saving the literal text "Hello Alex, how are you today")

For custom managed scripts, make sure to change over text fields to use TranslationUtility.get() and that they support the OnLocalize() field so that they get the text as expected (See sample project for an example)

Great!  Now you're ready for capture mode.  You can manually copy all your source text fields over to the file Transfluent/Resources/AutoDownloaded-<MY_LANGUAGE_CODE> or you can use capture mode to capture translated text

#Programmatic APIs
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