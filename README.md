# Transfluent Unity Plugin
The purpose of this tool:
1. Getting text into a format that can be translated
2. Translating the text
3. Simple APIs for manual integration ( TranslationUtility.get("MY_KEY");)
4. Automated tools to help text in scenes and to/from other formats (ie NGUI localization)

## Getting started:
1. [Download](https://raw.githubusercontent.com/Transfluent/Transfluent-Unity/master/Release/2.0/TransfluentEditor-2.0.3.unitypackage) the most current unity package (currently 2.0) and double click the package to import the package into your project.
2. Configure your game for translation in the game configuration menu -- ie it's currently in English and I want to translate to German and French
3. Get text into the Translation database (a scriptable object) -- You can use the tools described later in this document to help you get your text automatically migrated, or you can manually enter text through the inspector by looking at the asset Transfluent/Resources/Autodownloaded-en-us (or whatever your source language code may be)
4. Get your text translated!  Go to the menu item Translation/Game Configuration and select Translate my game.  You can view your order status online at https://www.transfluent.com/en/my-account/ , but you should be able to select the "Download all translations"


### Quick links for technical users
1. [Download](https://raw.githubusercontent.com/Transfluent/Transfluent-Unity/master/Release/2.0/TransfluentEditor-2.0.3.unitypackage) (examples are included)
2. [Tech Docs:] (https://github.com/Transfluent/Transfluent-Unity/wiki/JustTech)
3. [Sample project] A demo game that was *not* built for translation had it's text extracted and translated. (https://github.com/hardcoded2/strangerocks)
4. Basic usage: 
````C# 
TranslationUtility.get("myKey");
```` 
````C# 
TranslationUtility.changeStaticInstanceConfig("zh-cn");
````

## Getting your game translated

### Why Translate
1. Free advertising on foreign app stores
2. Extend your reach to find new customers, who may respond to your app differently -- ie a soccer game in Spanish
3. Potentially lower cost of aquisition and/or higher expected revenue per download [Distimo Publication - How the Most Successful Apps Monetize Globally](http://www.distimo.com/download/publication/Distimo_Publication_-_February_2014/EN/archive/)

### Potential issues confronted when localizing
1. Getting strings out of your current app
*   It's easy to have text in scenes, prefabs and code.  This takes different handling 
2. Dynamic text
*   "Hello, "+username+", how are you" is something that is very common in games.  These strings need to be extracted from your code in a different way than other text.
3. Storing the text
*   The format must be fast to load and use, merge well in source control when working in a multi-person studio, and be in a format that can be handled by translators
4. Displaying the text, changing it when langauges cahnge
*   Updating and changing the desired language, and updating your text when it changes has many sub-issues that need to be addressed, such as how to handle sleeping objects, how to keep a reference from the key (which can complicate getting strings out of your app)
5. Can slow down iteration time, verifying the results of a translation and then re-integrating the results from translators
*   Getting the translations back into the app after an iteration cycle can be time consuming
6. Ensuring all text is translated.
*   Unclear how to test that all of your strings are in a different language without understanding that language
*   I don't understand French!  How am I supposed to navigate my game UI when I switch it to a different langauge?  But I need to figure out how to verify that there aren't any un-translated strings hanging around...


### Approach to solving potential issues
1. Extracting strings --> Migration tools
 1. Scanning your scenes and prefabs to extract text.  Migration tools to help extract text from 
  * TextMeshes - Automatcially attach scripts that will change your textmeshes when you change languages.  Internally saves your localization key and then responds to the global OnLocalize() events, automatically updating your textmesh.  Also exposes ways to opt-out of this for specific meshes.
  * Custom scripts - Get strings out of your custom scripts by specifying how to extract it in the GameSpecificMigration.cs file. Also optionally keep the migration tool from managing any text meshes that you want to manage yourself.
  * Import from other known formats such as NGUI, csv, and newline delimited file
 2. Capture mode -- after initial integration, you can enable capture mode -- 
   * save any text that goes through the translation tool in the database (for you to review before requesting translation)
   * Helps you get text that is buried in code
2. Dynamic text --> Formatted string translation: 
  * Handle formatted translated text: 
  ```C# 
  TranslationUtility.getFormatted("Hello {0}, how are you?",username); 
  ````
3. Storing the text - store in Scriptable Objects
  * At runtime, it is serialized into a binary file that is serialized in native code, and packaged in a way that is fast as possible in each respective platform.  Fast to access, as it is a simple dictionary.
  * In source, it is stored Unity's native YAML format (Set in Edit->Project Settings->Editor Asset Serialization Force Text to make sure), which is easily merged and managed with source control such as git.
  * Import/
4. Displaying text --> Integrations with UI toolkits
  * Textmesh - LocalizedTextMesh keeps the source key, and responds to language change events for you.  The migration tools can automatically attach these to items that you don't blacklist. (which you would do for script-controlled text in some cases)
  * OnGUI - Wrap Unity's GUI and GUILayout functions to automatically translate any text calling them as long as you add the following lines to the top of the file using OnGUI:
  ````#C#
  using GUILayout = transfluent.guiwrapper.GUILayout;
  using GUI = transfluent.guiwrapper.GUI;
  ````
  * NGUI - export to NGUI's localization format.  
  * Custom scripts - 
   1. LocalizeUtil is a simple helper to store your localization key, and interact with the translation api for you (as long as you pass the OnLocalize() message on to it)
   2. Raw Api access.  store your localization key and manage the handling of text yourself.
5. Maintaining iteration time
  * Migration tools can be re-run as often as needed to re-capture text
  * Translations can be ordered, and will be automatically merged into your existing set.  You will only be charged for changes to your text, not for previous translations.
  * No need to sign deals or with humans or business people, but still proviiding translations from real people leading for quality results at reasonable rates ( https://www.transfluent.com/en/pricing/ ).  Fully automated api integration that gets results from human translators.
6. Free testing language -- backwards words. 
  * Provide free and instant language "translation" that will help you identify non-translated strings in your app in a form that you can still get through your game

##HOW TO
  
### Configure the game - source and destination languages
Select the menu "Transfluent/Game Configuration".  Set your source language, and add/remove any destination languages.

##Example translated project: https://github.com/hardcoded2/strangerocks
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

```C#
using UnityEngine;
using System.Collections.Generic;
using GUILayout = transfluent.guiwrapper.GUILayout; 
using transfluent;
public class ExampleTransfluent : MonoBehaviour
{
	void Start()
	{
		TranslationUtility.changeStaticInstanceConfig("zh-cn"); //you want to get this from playerprefs or some other part of your configuration settings, most likely
	}

	void OnGUI()
	{
		GUILayout.Label("hello");
	}
}
````

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
```C#
using GUI = transfluent.guiwrapper.GUI;
using GUILayout = transfluent.guiwrapper.GUILayout; 
````
And the rest of your GUILayout and GUI function calls will automatically use the transfluent API to translate text.

note: not handled by translated code due to technical restraints:
Toolbar - array of texts is too much
SelectionGrid


## Files in package:
* Transfluent:
  * Editor -- editor tools - editor windows, custom inspectors, and other utilities to perform operations only relevant to initial setup
  * Examples - Sample scenes, scripts showing how programatic localization(DIY) and UnityGUI(OnGUI, Textmesh) work
  * NGUIExtensions - tools to help use Transfluent data with the NGUI "Globalization" component. Import/export to and from the NGUI localization CSV.
  * Plugins - Code shared by both runtime and editor time scripts - serialization, web service interfaces
  * Resources - Data related to the configuration of your game (ie Source language: english, destination languages: Korean, Chinese, Japanese), and listings of individual translations.
  * Runtime - Interfaces to game code live in this folder -- these are the things you will be using directly
  
  
  
 