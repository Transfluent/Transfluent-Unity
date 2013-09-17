using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

public class TransfluentUtility : EditorWindow
{
    #region Internal classes etc...
    private class Message
    {
        public enum MessageType { Normal, Warning, Error };

        public MessageType m_type;
        public string m_text;

        private DateTime m_logDateTime;

        public bool isWarning
        {
            get { return m_type == MessageType.Warning; }
        }

        public bool isError
        {
            get { return m_type == MessageType.Error; }
        }

        public DateTime logDateLocal
        {
            get { return m_logDateTime.ToLocalTime(); }
        }

        public Message(MessageType type, string text)
        {
            m_logDateTime = DateTime.UtcNow;
            m_type = type;
            m_text = text;
        }
    }

    private enum Mode { Authenticate, Selection, Translations, Settings, Methods, Log, Debug };

    // These are the actual int values sent to the Transfluent Backend, with their default English names
    private enum TranslationLevel { Native_Speaker = 1, Professional_Translator = 2, Pair_Of_Translators = 3 };
    #endregion

    private static TransfluentUtility s_instance;

    #region Variables
    // Preference keys and values
    private const string m_transfluentTokenKey = "TransfluentAPIToken";
    private string m_transfluentToken;

    private const string m_userEmailKey = "TransfluentUserEmail";
    private string m_userEmail;

    private const string m_userPasswordKey = "TransfluentUserPassword";
    private string m_userPassword;

    private const string m_previousFilePathKey = "TransfluentPreviousFilePath";

    private const string m_userLocaleKey = "TransluentUserLocale";

    private const string m_translationCostEstimateCurrencySymbolKey = "TransfluentTranslationCostEstimateCurrencySymbol";
    private string m_translationCostEstimateCurrencySymbol;

    private const string m_translationCostEstimatePerWordKey = "TransfluentTranslationCostEstimatePerWord";
    private float m_translationCostEstimatePerWord;

    private const string m_translatorLevelKey = "TransfluentTranslatorLevel";
	
	// Data containers
    private List<Message> m_log;

    private List<TransfluentMethod> m_transfluentMethods;

    private Dictionary<int, TransfluentLanguage> m_transfluentLanguages;
    private Dictionary<string, int> m_languageIdsByCode;

    private List<int> m_languages;
    private Dictionary<string, TransfluentText> m_texts;
    private Dictionary<string, TransfluentTranslationEntry> m_translations;

    private Dictionary<int, TransfluentLanguage> m_selectedLanguages;
    private Dictionary<string, TransfluentText> m_selectedTexts;

    private Dictionary<string, GUIStyle> m_styles;
    private Dictionary<string, GUILayoutOption[]> m_glos;

    private Dictionary<int, TransfluentLozalization> m_locales;

    // Scrollview positions
    private Vector2 m_languageScrollPos;
    private Vector2 m_textScrollPos;
    private Vector2 m_textDetailScrollPos;
    private Vector2 m_methodScrollPos;
    private Vector2 m_methodDetailScrollPos;
    private Vector2 m_transfluentLanguageScrollPos;
    private Vector2 m_translationsScrollPos;
    private Vector2 m_translationDetailsPos;
    private Vector2 m_logScrollPos;
    private Vector2 m_debugScrollPos;

    // Other variables

    private Mode m_mode;
    private bool m_dataChanged;

    private string m_currentDataObjectPath;

    private TransfluentLozalization m_currentLocale;

    private string m_focusedTextId;
    private string m_focusedTranslationId;
    private int m_focusedMethod;

    private TransfluentMethod m_authRequest;
    private TransfluentMethod m_languagesRequest;

    private bool m_textsInvalidateTranslations;
    private TransfluentMethodType m_newTransfluentMethodType;
    private TransfluentMethodMode m_newTransfluentMethodMode;
    private int m_translationSourceLanguageId;
    private string m_translationOrderComment;
    private int m_translatorLevel;

    private string m_newTextId;
    private Dictionary<string, string> m_textIdChanges;
    private Dictionary<string, Dictionary<string, string>> m_textItemChanges;
    private string m_searchString;
    private string[] m_searchWords;
    private List<string> m_searchResultIds;
    #endregion

    [MenuItem("Tools/Transfluent Utility")]
    public static TransfluentUtility Open()
    {
        return EditorWindow.GetWindow<TransfluentUtility>("Transfluent");
    }

    // Called when window is closing or being reloaded
    void OnDisable()
    {
        if (m_dataChanged)
        {
            PromptSave("Window is being disabled or closed but there are unsaved changes.");

            // Handle GUI error in older Unity versions
            if (!Application.unityVersion.StartsWith("4"))
                Debug.LogWarning("Unity 3.5 (maybe older ones too) has a bug causing the EditorWindow to sometimes call the GUI after closing a dialog, even while closing the window.\nThis can cause an CheckOnGUI() error below. No need to worry though, all data was handled properly before the error.");
        }
    }

    // This is run if the plugin is loaded or reloaded
    private void Init()
    {
        s_instance = this;

        m_log = new List<Message>();

        AddMessage("Initializing...");

        wantsMouseMove = true;

        // If no token can be found, set mode to authentication
        if (!EditorPrefs.HasKey(m_transfluentTokenKey))
            SetMode(Mode.Authenticate);
        else
            SetMode(Mode.Selection);

        m_authRequest = null;

        // Get the preferences
        m_transfluentToken = DecryptString(EditorPrefs.GetString(m_transfluentTokenKey, ""));
        m_userEmail = DecryptString(EditorPrefs.GetString(m_userEmailKey, ""));
        m_userPassword = DecryptString(EditorPrefs.GetString(m_userPasswordKey, ""));
        m_currentDataObjectPath = EditorPrefs.GetString(m_previousFilePathKey, "");

        int localeId = EditorPrefs.GetInt(m_userLocaleKey, 148);

        if (EditorPrefs.HasKey(m_translationCostEstimateCurrencySymbolKey))
            m_translationCostEstimateCurrencySymbol = EditorPrefs.GetString(m_translationCostEstimateCurrencySymbolKey, "");
        else
        {
            m_translationCostEstimateCurrencySymbol = "EUR";//System.Globalization.RegionInfo.CurrentRegion.ISOCurrencySymbol; // ISOCurrencySymbol is broken, it gives "US Dollar" when it should be "USD"
            EditorPrefs.SetString(m_translationCostEstimateCurrencySymbolKey, m_translationCostEstimateCurrencySymbol);
        }

        if (EditorPrefs.HasKey(m_translationCostEstimatePerWordKey))
            m_translationCostEstimatePerWord = EditorPrefs.GetFloat(m_translationCostEstimatePerWordKey, 0f);
        else
        {
            m_translationCostEstimatePerWord = 0f;
            EditorPrefs.SetFloat(m_translationCostEstimatePerWordKey, m_translationCostEstimatePerWord);
        }

        if (EditorPrefs.HasKey(m_translatorLevelKey))
            m_translatorLevel = EditorPrefs.GetInt(m_translatorLevelKey);
        else
        {
            m_translatorLevel = (int)TranslationLevel.Pair_Of_Translators;
            EditorPrefs.SetInt(m_translatorLevelKey, m_translatorLevel);
        }

        m_focusedTextId = "";
        m_focusedTranslationId = "";
        m_focusedMethod = -1;

        m_newTextId = "";
        m_searchString = "";

        m_textsInvalidateTranslations = true;
        m_translationOrderComment = "";

        // Init GUI
        InitStyles();

        // Init Locale
        InitLocales();
        SetLocale(localeId);

        // Initialize data by loading the previous file if found, or check if the asset shown in inspector is a TransfluentData file
        if (m_currentDataObjectPath.Length > 0)
            LoadData(m_currentDataObjectPath);
        else
            InitData(Selection.activeObject as TransfluentData);

        AddMessage("TransfluentUtility initialized");
    }

    private void InitStyles()
    {
        m_styles = new Dictionary<string, GUIStyle>();

        GUIStyle loginLabel = new GUIStyle(GUI.skin.box);
        loginLabel.normal.textColor = GUI.skin.label.normal.textColor;
        loginLabel.margin = new RectOffset();
        loginLabel.padding = new RectOffset(4, 4, 4, 4);
        m_styles.Add("LoginLabel", loginLabel);

        GUIStyle modeRow = new GUIStyle();
        modeRow.margin = new RectOffset();
        modeRow.padding = new RectOffset();
        m_styles.Add("ModeRow", modeRow);

        GUIStyle topRow = new GUIStyle();
        topRow.margin = new RectOffset();
        topRow.padding = new RectOffset();
        m_styles.Add("TopRow", topRow);

        GUIStyle bottomRow = new GUIStyle();
        bottomRow.margin = new RectOffset();
        bottomRow.padding = new RectOffset();
        m_styles.Add("BottomRow", bottomRow);

        GUIStyle modeButton = new GUIStyle(GUI.skin.button);
        modeButton.fontSize = 15;
        m_styles.Add("ModeButton", modeButton);

        GUIStyle modeButtonActive = new GUIStyle(GUI.skin.button);
        modeButtonActive.fontSize = 15;
        modeButtonActive.normal.background = GUI.skin.button.active.background;
        m_styles.Add("ModeButtonActive", modeButtonActive);

        GUIStyle selectedGroup = new GUIStyle(GUI.skin.button);
        selectedGroup.alignment = TextAnchor.MiddleLeft;
        selectedGroup.normal.background = selectedGroup.active.background;
        m_styles.Add("FocusedGroup", selectedGroup);

        GUIStyle unselectedGroup = new GUIStyle(GUI.skin.button);
        unselectedGroup.alignment = TextAnchor.MiddleLeft;
        unselectedGroup.hover.background = unselectedGroup.normal.background;
        unselectedGroup.normal.background = null;
        m_styles.Add("UnfocusedGroup", unselectedGroup);

        GUIStyle selectedText = new GUIStyle(GUI.skin.button);
        selectedText.alignment = TextAnchor.MiddleLeft;
        selectedText.normal.background = selectedText.active.background;
        m_styles.Add("FocusedText", selectedText);

        GUIStyle unselectedText = new GUIStyle(GUI.skin.button);
        unselectedText.alignment = TextAnchor.MiddleLeft;
        unselectedText.hover.background = unselectedText.normal.background;
        unselectedText.normal.background = null;
        m_styles.Add("UnfocusedText", unselectedText);

        GUIStyle selectedTextChanged = new GUIStyle(GUI.skin.button);
        selectedTextChanged.alignment = TextAnchor.MiddleLeft;
        selectedTextChanged.normal.background = selectedTextChanged.active.background;
        selectedTextChanged.normal.textColor = Color.red;
        m_styles.Add("FocusedTextChanged", selectedTextChanged);

        GUIStyle unselectedTextChanged = new GUIStyle(GUI.skin.button);
        unselectedTextChanged.alignment = TextAnchor.MiddleLeft;
        unselectedTextChanged.hover.background = unselectedTextChanged.normal.background;
        unselectedTextChanged.normal.background = null;
        unselectedTextChanged.normal.textColor = Color.red;
        m_styles.Add("UnfocusedTextChanged", unselectedTextChanged);

        GUIStyle paneArea = new GUIStyle(GUI.skin.box);
        paneArea.margin = new RectOffset(4, 4, 4, 0);
        paneArea.padding = new RectOffset();
        m_styles.Add("PaneArea", paneArea);

        GUIStyle paneTitle = new GUIStyle();
        paneTitle.margin = new RectOffset();
        paneTitle.padding = new RectOffset(4, 4, 4, 4);
        m_styles.Add("PaneTitle", paneTitle);

        GUIStyle pane = new GUIStyle(GUI.skin.box);
        pane.margin = new RectOffset();
        pane.padding = new RectOffset(4, 4, 4, 4);
        m_styles.Add("Pane", pane);

        GUIStyle detailPane = new GUIStyle(GUI.skin.box);
        detailPane.margin = new RectOffset();
        detailPane.padding = new RectOffset();
        m_styles.Add("DetailPane", detailPane);

        GUIStyle detailItem = new GUIStyle(GUI.skin.box);
        detailItem.normal.textColor = GUI.skin.label.normal.textColor;
        m_styles.Add("DetailItem", detailItem);

        GUIStyle detailItemText = new GUIStyle(GUI.skin.label);
        m_styles.Add("DetailItemLabel", detailItemText);

        GUIStyle detailItemTextUnsaved = new GUIStyle(GUI.skin.label);
        detailItemTextUnsaved.normal.textColor = Color.red;
        m_styles.Add("DetailItemLabelTextUnsaved", detailItemTextUnsaved);

        GUIStyle transfluentLanguageAddButton = new GUIStyle(GUI.skin.button);
        m_styles.Add("TransfluentLanguageAddButton", transfluentLanguageAddButton);

        GUIStyle transfluentLanguageRemoveButton = new GUIStyle(GUI.skin.button);
        transfluentLanguageRemoveButton.normal.textColor = Color.red;
        m_styles.Add("TransfluentLanguageRemoveButton", transfluentLanguageRemoveButton);

        GUIStyle methodPane = new GUIStyle();
        methodPane.margin = new RectOffset();
        methodPane.padding = new RectOffset(4, 4, 4, 4);
        m_styles.Add("MethodPane", methodPane);

        GUIStyle methodTranslateComment = new GUIStyle(GUI.skin.textArea);
        methodTranslateComment.wordWrap = true;
        m_styles.Add("MethodTranslateComment", methodTranslateComment);

        GUIStyle methodList = new GUIStyle(GUI.skin.box);
        methodList.margin = new RectOffset();
        methodList.padding = new RectOffset(4, 4, 4, 4);
        m_styles.Add("MethodList", methodList);

        GUIStyle methodItem = new GUIStyle(GUI.skin.box);
        methodItem.margin = new RectOffset(4, 4, 4, 4);
        methodItem.padding = new RectOffset(4, 4, 4, 4);
        m_styles.Add("MethodItem", methodItem);

        GUIStyle methodItemError = new GUIStyle(GUI.skin.label);
        methodItemError.normal.textColor = Color.red;
        m_styles.Add("MethodItemError", methodItemError);

        GUIStyle methodButton = new GUIStyle(GUI.skin.button);
        methodButton.alignment = TextAnchor.MiddleLeft;
        m_styles.Add("MethodButton", methodButton);

        GUIStyle methodDetails = new GUIStyle(GUI.skin.box);
        methodDetails.margin = new RectOffset(4, 4, 4, 4);
        methodDetails.padding = new RectOffset(4, 4, 4, 4);
        m_styles.Add("MethodDetails", methodDetails);

        GUIStyle methodParameters = new GUIStyle(GUI.skin.label);
        methodParameters.alignment = TextAnchor.MiddleLeft;
        methodParameters.wordWrap = true;
        m_styles.Add("MethodParameters", methodParameters);

        GUIStyle methodResponse = new GUIStyle(GUI.skin.label);
        methodResponse.alignment = TextAnchor.MiddleLeft;
        methodResponse.wordWrap = true;
        m_styles.Add("MethodResponse", methodResponse);

        GUIStyle translationDetails = new GUIStyle(GUI.skin.box);
        translationDetails.margin = new RectOffset(4, 4, 4, 4);
        translationDetails.padding = new RectOffset(4, 4, 4, 4);
        m_styles.Add("TranslationDetails", translationDetails);

        GUIStyle translationEntry = new GUIStyle(GUI.skin.box);
        translationEntry.normal.textColor = GUI.skin.label.normal.textColor;
        m_styles.Add("TranslationEntry", translationEntry);

        GUIStyle translationEntryCategories = new GUIStyle(GUI.skin.box);
        translationEntryCategories.normal.background = null;
        m_styles.Add("TranslationEntryCategories", translationEntryCategories);

        GUIStyle translationEntryCategory = new GUIStyle(GUI.skin.label);
        m_styles.Add("TranslationEntryCategory", translationEntryCategory);

        GUIStyle log = new GUIStyle(GUI.skin.box);
        log.margin = new RectOffset(4, 4, 4, 4);
        log.padding = new RectOffset(4, 4, 4, 4);
        m_styles.Add("Log", log);

        GUIStyle logEntry = new GUIStyle(GUI.skin.box);
        m_styles.Add("LogEntry", logEntry);

        GUIStyle logEntryNormal = new GUIStyle(GUI.skin.label);
        m_styles.Add("LogEntryNormal", logEntryNormal);

        GUIStyle logEntryWarning = new GUIStyle(GUI.skin.label);
        logEntryWarning.normal.textColor = Color.yellow;
        m_styles.Add("LogEntryWarning", logEntry);

        GUIStyle logEntryError = new GUIStyle(GUI.skin.label);
        logEntryError.normal.textColor = Color.red;
        m_styles.Add("LogEntryError", logEntry);

        m_glos = new Dictionary<string, GUILayoutOption[]>();

        GUILayoutOption[] loginLabelGLOs = new GUILayoutOption[] { GUILayout.Width(300f) };
        m_glos.Add("LoginLabel", loginLabelGLOs);

        GUILayoutOption[] fileButtonGLOs = new GUILayoutOption[] { GUILayout.Width(60f) };
        m_glos.Add("FileButton", fileButtonGLOs);

        GUILayoutOption[] modeRowGLOs = new GUILayoutOption[] { GUILayout.Height(30f) };
        m_glos.Add("ModeRow", modeRowGLOs);

        GUILayoutOption[] topRowGLOs = new GUILayoutOption[] { GUILayout.ExpandHeight(true) };
        m_glos.Add("TopRow", topRowGLOs);

        GUILayoutOption[] bottomRowGLOs = new GUILayoutOption[] { GUILayout.Height(160f) };
        m_glos.Add("BottomRow", bottomRowGLOs);

        GUILayoutOption[] modeButtonGLOs = new GUILayoutOption[] { GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true) };
        m_glos.Add("ModeButton", modeButtonGLOs);

        GUILayoutOption[] paneTitleGLOs = new GUILayoutOption[] { GUILayout.Height(24f) };
        m_glos.Add("PaneTitle", paneTitleGLOs);

        GUILayoutOption[] groupPaneGLOs = new GUILayoutOption[] { GUILayout.Width(180f) };
        m_glos.Add("GroupPane", groupPaneGLOs);

        GUILayoutOption[] textPaneGLOs = new GUILayoutOption[] { GUILayout.ExpandWidth(true) };
        m_glos.Add("TextPane", textPaneGLOs);

        GUILayoutOption[] languagePaneGLOs = new GUILayoutOption[] { GUILayout.Width(260f) };
        m_glos.Add("LanguagePane", languagePaneGLOs);

        GUILayoutOption[] sourcelanguageLabelGLOs = new GUILayoutOption[] { GUILayout.ExpandWidth(false) };
        m_glos.Add("SourceLanguageLabel", sourcelanguageLabelGLOs);

        GUILayoutOption[] transfluentLanguagePaneGLOs = new GUILayoutOption[] { GUILayout.Width(320f) };
        m_glos.Add("TransfluentLanguagePane", transfluentLanguagePaneGLOs);

        GUILayoutOption[] transfluentLanguageAddRemoveButtonGLOs = new GUILayoutOption[] { GUILayout.Width(60f) };
        m_glos.Add("TransfluentLanguageAddRemoveButton", transfluentLanguageAddRemoveButtonGLOs);

        GUILayoutOption[] detailPaneGLOs = new GUILayoutOption[] { GUILayout.ExpandWidth(true) };
        m_glos.Add("DetailPane", detailPaneGLOs);

        GUILayoutOption[] detailItemGLOs = new GUILayoutOption[] { GUILayout.ExpandWidth(true) };
        m_glos.Add("DetailItem", detailItemGLOs);

        GUILayoutOption[] detailItemLabelGLOs = new GUILayoutOption[] { GUILayout.Width(60f) };
        m_glos.Add("DetailItemLabel", detailItemLabelGLOs);

        GUILayoutOption[] detailSaveButtonGLOs = new GUILayoutOption[] { GUILayout.Width(80f) };
        m_glos.Add("DetailSaveButton", detailSaveButtonGLOs);

        GUILayoutOption[] methodPaneGLOs = new GUILayoutOption[] { GUILayout.Width(260f) };
        m_glos.Add("MethodPane", methodPaneGLOs);

        GUILayoutOption[] methodSentGLOs = new GUILayoutOption[] { GUILayout.Width(160f) };
        m_glos.Add("MethodSent", methodSentGLOs);

        GUILayoutOption[] methodDoneGLOs = new GUILayoutOption[] { GUILayout.Width(40f) };
        m_glos.Add("MethodDone", methodDoneGLOs);

        GUILayoutOption[] methodModeGLOs = new GUILayoutOption[] { GUILayout.Width(50f) };
        m_glos.Add("MethodMode", methodModeGLOs);

        GUILayoutOption[] methodTypeGLOs = new GUILayoutOption[] { GUILayout.Width(80f) };
        m_glos.Add("MethodType", methodTypeGLOs);

        GUILayoutOption[] methodDetailsGLOs = new GUILayoutOption[] { GUILayout.Height(200f), GUILayout.ExpandWidth(true) };
        m_glos.Add("MethodDetails", methodDetailsGLOs);

        GUILayoutOption[] methodDetailLabelGLOs = new GUILayoutOption[] { GUILayout.Width(80f) };
        m_glos.Add("MethodDetailLabel", methodDetailLabelGLOs);

        GUILayoutOption[] methodTranslateCommentGLOs = new GUILayoutOption[] { GUILayout.Height(30f), GUILayout.Width(254f) };
        m_glos.Add("MethodTranslateComment", methodTranslateCommentGLOs);

        GUILayoutOption[] translationDetailsGLOs = new GUILayoutOption[] { GUILayout.Height(160f), GUILayout.ExpandWidth(true) };
        m_glos.Add("TranslationDetails", translationDetailsGLOs);

        GUILayoutOption[] translationCheckStatusAllGLOs = new GUILayoutOption[] { GUILayout.Width(70f + GUI.skin.verticalScrollbar.fixedWidth) };
        m_glos.Add("TranslationCheckStatusAll", translationCheckStatusAllGLOs);

        GUILayoutOption[] translationGroupIdGLOs = new GUILayoutOption[] { GUILayout.Width(160f) };
        m_glos.Add("TranslationGroupId", translationGroupIdGLOs);

        GUILayoutOption[] translationTextIdGLOs = new GUILayoutOption[] { GUILayout.ExpandWidth(true) };
        m_glos.Add("TranslationTextId", translationTextIdGLOs);

        GUILayoutOption[] translationLanguagesGLOs = new GUILayoutOption[] { GUILayout.Width(180f) };
        m_glos.Add("TranslationLanguages", translationLanguagesGLOs);

        GUILayoutOption[] translationFocusGLOs = new GUILayoutOption[] { GUILayout.Width(70f) };
        m_glos.Add("TranslationFocus", translationFocusGLOs);

        GUILayoutOption[] translationDetailLabelGLOs = new GUILayoutOption[] { GUILayout.Width(120f) };
        m_glos.Add("TranslationDetailLabel", translationDetailLabelGLOs);

        GUILayoutOption[] logEntryDateGLOs = new GUILayoutOption[] { GUILayout.Width(150f) };
        m_glos.Add("LogEntryDate", logEntryDateGLOs);
    }

    // Load and initialize the locales
    private void InitLocales()
    {
        m_locales = new Dictionary<int, TransfluentLozalization>();

        string localeFileDirectoryPath = Application.dataPath;
        localeFileDirectoryPath = Path.Combine(localeFileDirectoryPath, "Editor");
        localeFileDirectoryPath = Path.Combine(localeFileDirectoryPath, "Transfluent");
        localeFileDirectoryPath = Path.Combine(localeFileDirectoryPath, "TransfluentLocalizations");

        // Debug info
        if (!Directory.Exists(localeFileDirectoryPath))
        {
            AddError("No locale directory was found!");
            return;
        }
        else
            AddMessage("Loading localizations from directory:\n" + localeFileDirectoryPath);

        string[] localeFiles = Directory.GetFiles(localeFileDirectoryPath, "TransfluentLocalization_*.xml");

        TransfluentLozalization locale;
        foreach (string file in localeFiles)
        {
            AddMessage("Localization file found:\n" + file);
            locale = new TransfluentLozalization(file);
            m_locales.Add(locale.id, locale);
        }

        if (m_locales.Count == 0)
            AddError("No locales were loaded!");

        m_currentLocale = new TransfluentLozalization(null);
    }

    private void SetLocale(int localeId)
    {
        TransfluentLozalization locale;
        if (m_locales.TryGetValue(localeId, out locale))
            m_currentLocale = locale;
        else
            AddError("Locale with id '" + localeId + "' was not found");
    }

    private string GetLocalizedString(string id)
    {
        if (m_currentLocale != null)
            return m_currentLocale.GetText(id);

        return "ERROR ID='" + id + "' No locale loaded";
    }

    private void SetSourceLanguage(int languageId)
    {
        TransfluentLanguage lang;
        if (m_transfluentLanguages.TryGetValue(languageId, out lang))
        {
            // If this language is not in use yet, add it now
            if (!m_languages.Contains(languageId))
                AddLanguage(lang);

            AddMessage("Source language id changed: " + m_translationSourceLanguageId + " -> " + languageId);
            m_translationSourceLanguageId = languageId;
        }
        else
        {
            AddWarning("Source language id not found: " + languageId);
            m_translationSourceLanguageId = -1;
        }
    }

    // Initialize the utility with a new, empty data
    public void ResetData()
    {
        InitData(null);
    }

    // Initialize the utility with the given data object
    private void InitData(TransfluentData data)
    {
        m_dataChanged = false;
        m_textIdChanges = new Dictionary<string, string>();
        m_textItemChanges = new Dictionary<string, Dictionary<string, string>>();

        m_transfluentLanguages = new Dictionary<int, TransfluentLanguage>();
        m_languageIdsByCode = new Dictionary<string, int>();

        m_languages = new List<int>();
        m_texts = new Dictionary<string, TransfluentText>();
        m_translations = new Dictionary<string, TransfluentTranslationEntry>();

        m_translationSourceLanguageId = -1;

        m_transfluentMethods = new List<TransfluentMethod>();

        m_searchWords = new string[0];

        m_selectedLanguages = new Dictionary<int, TransfluentLanguage>();
        m_selectedTexts = new Dictionary<string, TransfluentText>();

        m_currentDataObjectPath = "";

        if (data != null)
        {
            // Load data from object, note that we need to remove a duplicate "Assets/" from the database asset path first
            m_currentDataObjectPath = Path.Combine(Application.dataPath, AssetDatabase.GetAssetPath(data).Remove(0, 7));
            //AddMessage("Reading data from local file: " + m_currentDataObjectPath);

            foreach (TransfluentLanguage lang in data.m_languages)
                AddTransfluentLanguage(TransfluentLanguage.Clone(lang));

            // Handle group errors
            if (data.m_textGroup == null)
            {
                Debug.LogError("Can't load: Text group is null");
                return;
            }

            if (data.m_textGroup.m_id != UnityEditor.PlayerSettings.productName)
            {
                Debug.LogError("Can't load: Text group id '" + data.m_textGroup.m_id + "' does not match with project name " + UnityEditor.PlayerSettings.productName);
                return;
            }

            // Retrieve the data
            foreach (TransfluentText text in data.m_textGroup.m_texts)
            {
                int langId;
                foreach (TransfluentTextItem item in text.m_texts)
                {
                    if (m_languageIdsByCode.TryGetValue(item.m_languageCode, out langId))
                    {
                        if (!m_languages.Contains(langId))
                            m_languages.Add(langId);
                    }
                    else
                        Debug.LogWarning("Text with id '" + text.m_id + "' contains an entry with unknown language code " + item.m_languageCode);
                }

                AddText(TransfluentText.Clone(text));
            }

            // Retrieve any unfinished translation entries
            string id;
            foreach (TransfluentTranslationEntry translation in data.m_translations)
            {
                id = translation.m_groupId + "_" + translation.m_textId + "_" + translation.m_targetLanguageId;
                m_translations.Add(id, TransfluentTranslationEntry.Clone(translation));
            }

            // Ensure all text items have entries for each used language
            TransfluentLanguage language;
            foreach (int langId in m_languages)
            {
                if (m_transfluentLanguages.TryGetValue(langId, out language))
                {
                    AddTextItemsInLanguage(language);
                }
            }
            
            SetSourceLanguage(data.m_sourceLanguageId);
        }
        else
        {
            AddMessage("Initialized empty data");
            Repaint();
        }

        // Save the file path in registry
        EditorPrefs.SetString(m_previousFilePathKey, m_currentDataObjectPath);

        // Always check for updates for the language list
        MethodRequestLanguages();
    }

    private void SetMode(Mode mode)
    {
        m_mode = mode;
        //Debug.Log("Switching mode to " + mode);
    }

    public void AddMessage(string text)
    {
        m_log.Add(new Message(Message.MessageType.Normal, text));
        Debug.Log("TransfluentUtility: " + text);
    }

    public void AddWarning(string text)
    {
        m_log.Add(new Message(Message.MessageType.Warning, text));
        Debug.LogWarning("TransfluentUtility: " + text);
    }

    public void AddError(string text)
    {
        m_log.Add(new Message(Message.MessageType.Error, text));
        Debug.LogError("TransfluentUtility: " + text);
    }

    private void AddText(TransfluentText text)
    {
        m_texts.Add(text.m_id, text);
    }

    private void AddTextItemsInLanguage(TransfluentLanguage language)
    {
        AddMessage("Checking language id " + language.m_id + " [" + language.m_code + "] " + language.m_name + " is used in all texts");

        int added = 0;
        bool found;
        //foreach (string groupId in m_textGroups.Keys)
        //{
        //foreach (TransfluentText text in m_textGroups[groupId].Values)
        foreach (TransfluentText text in m_texts.Values)
        {
            found = false;

            foreach (TransfluentTextItem item in text.m_texts)
            {
                if (item.m_languageCode == language.m_code)
                {
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                added++;
                text.m_texts.Add(new TransfluentTextItem(language.m_code, ""));

                // Mark this up as a change
                Dictionary<string, string> itemChanges;
                if (m_textItemChanges.TryGetValue(text.m_id, out itemChanges))
                    itemChanges[language.m_code] = "";
                else
                {
                    itemChanges = new Dictionary<string,string>();
                    itemChanges.Add(language.m_code, "");
                    m_textItemChanges.Add(text.m_id, itemChanges);
                }
            }
        }
        //}

        if (added > 0)
            AddMessage("Added text items for language '" + language.m_code + "' to " + added + " texts");
    }

    private void RemoveTextItemsOfLanguage(TransfluentLanguage language)
    {
        int itemIndex;
        foreach (TransfluentText text in m_texts.Values)
        {
            itemIndex = -1;
            for (int i = 0; i < text.m_texts.Count; i++)
            {
                if (text.m_texts[i].m_languageCode == language.m_code)
                {
                    itemIndex = i;
                    break;
                }
            }

            if (itemIndex != -1)
            {
                Dictionary<string, string> itemChanges;
                if (m_textItemChanges.TryGetValue(text.m_id, out itemChanges))
                    itemChanges[language.m_code] = text.m_texts[itemIndex].m_text;
                else
                {
                    itemChanges = new Dictionary<string, string>();
                    itemChanges.Add(language.m_code, text.m_texts[itemIndex].m_text);
                    m_textItemChanges.Add(text.m_id, itemChanges);
                }

                text.m_texts.RemoveAt(itemIndex);
            }
        }
    }

    // Set the focus on a certain text item
    private void SetTextFocus(string textId)
    {
        m_focusedTextId = textId;
        m_newTextId = textId;

        if (textId.Length > 0)
            GUI.FocusControl("AddText");
    }

    // Get all texts in a certain language, formatted as string arrays of ids and actual texts
    private void GetTextsOfLanguage(int languageId, out string[] textIds, out string[] texts)
    {
        List<string> selectedIds = new List<string>(m_selectedTexts.Count);
        List<string> selectedTexts = new List<string>(m_selectedTexts.Count);
        TransfluentLanguage language = m_transfluentLanguages[languageId];

        bool found;
        foreach (TransfluentText textEntry in m_selectedTexts.Values)
        {
            found = false;

            foreach (TransfluentTextItem textItem in textEntry.m_texts)
            {
                if (textItem.m_languageCode == language.m_code)
                {
                    selectedIds.Add(textEntry.m_id);
                    selectedTexts.Add(textItem.m_text);
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                AddWarning("Text entry '" + textEntry.m_id + "' is missing an entry in language '" + language.m_code + "'");
            }
        }

        textIds = selectedIds.ToArray();
        texts = selectedTexts.ToArray();
    }

    // Get all texts in a certain language, formatted as a Dictionary with string keys (id) and string values (text)
    public Dictionary<string, string> GetTextsOfLanguage(int languageId)
    {
        Dictionary<string, string> texts = new Dictionary<string, string>();

        TransfluentLanguage language;
        if (m_transfluentLanguages.TryGetValue(languageId, out language))
        {
            foreach (TransfluentText textEntry in m_selectedTexts.Values)
            {
                TransfluentTextItem textItem;
                if (textEntry.TryGetText(language.m_code, out textItem))
                    texts.Add(textEntry.m_id, textItem.m_text);
                else
                    AddWarning("Text entry by the id of '" + textEntry.m_id + "' doesn't contain text in language '" + language.m_code + "'");
            }
        }
        else
            AddError("Could not get texts by language, TransfluentLanguage by the id of " + languageId + " was not found.");

        return texts;
    }

    // Get all texts, key will be the entry id, value is the text entry as TransfluentText
    public Dictionary<string, TransfluentText> GetTexts()
    {
        return new Dictionary<string, TransfluentText>(m_texts);
    }

    // Gets a text entry with the specified id, null if no entry exists
    public TransfluentText GetText(string textId)
    {
        TransfluentText text;
        if (m_texts.TryGetValue(textId, out text))
            return text;
        else
            AddError("Could not find text with the id: '" + textId + "'");

        return null;
    }

    // Sets or adds a text entry to the data
    public void SetText(TransfluentText text)
    {
        if (m_texts.ContainsKey(text.m_id))
        {
            m_texts[text.m_id] = text;
            AddMessage("Replaced text with id: '" + text.m_id + "' with new data");
        }
        else
            AddText(text);
    }

    // Sets or adds a text to a text entry, if no entry exists a new one is created
    public void SetTextItem(string textId, int languageId, string text)
    {
        TransfluentLanguage lang;
        if (m_transfluentLanguages.TryGetValue(languageId, out lang))
        {
            TransfluentText textEntry;
            if (m_texts.TryGetValue(textId, out textEntry))
            {
                TransfluentTextItem item;
                if (textEntry.TryGetText(lang.m_code, out item))
                    item.m_text = text;
                else
                    textEntry.m_texts.Add(new TransfluentTextItem(lang.m_code, text));

                AddMessage("Text of language " + languageId + " set to entry with the id: '" + textEntry.m_id + "'");
            }
            else
            {
                textEntry = new TransfluentText(textId, UnityEditor.PlayerSettings.productName);
                textEntry.m_texts.Add(new TransfluentTextItem(lang.m_code, text));
                AddText(textEntry);

                AddMessage("Text of language " + languageId + " set to a new text entry with the id: '" + textEntry.m_id + "'");
            }
        }
        else
            AddError("Language by the id of " + languageId + " wasn't found");
    }

    // Add the specified language to languages in use
    public void AddLanguage(TransfluentLanguage language)
    {
        AddMessage("Adding Text Language (" + language.m_code + ") ID[" + language.m_id + "]: '" + language.m_name + "'");

        if (!m_languages.Contains(language.m_id))
            m_languages.Add(language.m_id);
        else
            AddWarning("Text Language already exists (" + language.m_code + ") ID[" + language.m_id + "]: '" + language.m_name + "'");

        // Make sure all text items have an entry of this language
        AddTextItemsInLanguage(language);
    }

    private void RemoveLanguage(TransfluentLanguage language)
    {
        AddMessage("Removing Text Language (" + language.m_code + ") ID[" + language.m_id + "]: '" + language.m_name + "'");

        // Remove all text items in this language
        RemoveTextItemsOfLanguage(language);

        if (m_languages.Contains(language.m_id))
            m_languages.Remove(language.m_id);
        else
            AddWarning("Text Language doesn't exist (" + language.m_code + ") ID[" + language.m_id + "]: '" + language.m_name + "'");

        if (language.m_id == m_translationSourceLanguageId || m_languages.Count == 0)
            m_translationSourceLanguageId = -1;
    }

    // Add the specified language to available languages
    private void AddTransfluentLanguage(TransfluentLanguage language)
    {
        if (!m_transfluentLanguages.ContainsKey(language.m_id))
        {
            AddMessage("Adding Transfluent Language (" + language.m_code + ") ID[" + language.m_id + "]: '" + language.m_name + "'");
            m_transfluentLanguages.Add(language.m_id, language);
        }
        else
        {
            TransfluentLanguage original = m_transfluentLanguages[language.m_id];
            AddWarning("Transfluent Language id already exists! Original will be replaced by new information\nOriginal: (" + original.m_code + ") ID[" + original.m_id + "]: '" + original.m_name + "'\nDuplicate: (" + language.m_code + ") ID[" + language.m_id + "]: '" + language.m_name + "'");
            m_transfluentLanguages[language.m_id] = language;
        }

        // Add new entry to language code to id map, or replace with new information
        if (!m_languageIdsByCode.ContainsKey(language.m_code))
            m_languageIdsByCode.Add(language.m_code, language.m_id);
        else
        {
            m_languageIdsByCode[language.m_code] = language.m_id;
            //AddError("Language code " + language.m_code + " defined more than once! Duplicate: (" + language.m_code + ") ID[" + language.m_id + "]: '" + language.m_name + "'");
        }
    }

    public TransfluentLanguage[] GetTransfluentLanguages()
    {
        TransfluentLanguage[] langs = new TransfluentLanguage[m_transfluentLanguages.Count];
        m_transfluentLanguages.Values.CopyTo(langs, 0);
        return langs;
    }

    public TransfluentLanguage[] GetLanguagesInUse()
    {
        TransfluentLanguage[] langs = new TransfluentLanguage[m_languages.Count];
        TransfluentLanguage lang;
        for (int i = 0; i < m_languages.Count; i++)
        {
            if (m_transfluentLanguages.TryGetValue(m_languages[i], out lang))
                langs[i] = lang;
        }
        return langs;
    }

    // Called 100 times per second if window is visible
    void Update()
    {
        if (s_instance != this)
            return;

        // Check if any method calls to Transfluent API have been completed
        if (m_transfluentMethods != null)
        {
            for (int i = m_transfluentMethods.Count - 1; i >= 0; i--)
            {
                TransfluentMethod method = m_transfluentMethods[i];

                if (method.isConsumed)
                    continue;

                if (method.isDone)
                {
                    if (method.error == null)
                    {
                        AddMessage("Method " + method.m_type.ToString() + " completed");

                        // Parse the response
                        ParseTransfluentJSON(method);
                    }
                    else
                        AddError("Transfluent." + method.GetType() + " error:\n\"" + method.error + "\"\nResponse: " + method.response);

                    // Invoke any specified delegates
                    if (method.hasDelegate)
                        method.InvokeDelegate();

                    method.Consume();
                }
            }
        }

        // Run the text item search here instead of GUI updates
        if (m_searchString.Length > 0 && m_searchResultIds == null)
        {
            //Debug.Log("Running a search!");

            m_searchResultIds = new List<string>();

            m_searchWords = m_searchString.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            //Debug.Log("Search string '" + m_searchString + "' results in " + m_searchWords.Length + " words");

            bool foundMatch, foundWord;
            string searchWordLowercase;

            foreach (TransfluentText text in m_texts.Values)
            {
                foundMatch = true;

                foreach (string searchWord in m_searchWords)
                {
                    searchWordLowercase = searchWord.ToLower();

                    if (text.m_id.ToLower().Contains(searchWordLowercase))
                        continue;

                    foundWord = false;
                    foreach (TransfluentTextItem item in text.m_texts)
                    {
                        if (item.m_text.ToLower().Contains(searchWordLowercase))
                        {
                            foundWord = true;
                            break;
                        }
                    }

                    if (foundWord)
                        continue;

                    foundMatch = false;
                    break;
                }

                if (foundMatch)
                    m_searchResultIds.Add(text.m_id);
            }

            //Debug.Log("Search done, found " + m_searchResultIds.Count + " matches!");
        }
    }

    // Called every time the GUI is drawn
    void OnGUI()
    {
        // Initialize if necessary
        if (m_styles == null)
            Init();

        if (m_mode == Mode.Authenticate)
        {
            OnGUI_Login();
            return;
        }

        // Draw top row
        GUILayout.BeginHorizontal();
        {
            GUILayout.Label("File", GUILayout.ExpandWidth(false));

            if (GUILayout.Button(GetLocalizedString("button_file_open"), m_glos["FileButton"]))
                LoadData();

            if (GUILayout.Button(GetLocalizedString("button_file_save"), m_glos["FileButton"]))
                SaveData();

            if (GUILayout.Button(GetLocalizedString("button_file_close"), m_glos["FileButton"]))
                ResetData();

            if (m_currentDataObjectPath.Length > 0)
                GUILayout.Label(GetLocalizedString("label_file_opened") + ": " + m_currentDataObjectPath + (m_dataChanged ? "*" : ""));
        }
        GUILayout.EndHorizontal();

        // Draw mode buttons
        GUILayout.BeginHorizontal(m_styles["ModeRow"], m_glos["ModeRow"]);
        {
            if (GUILayout.Button(GetLocalizedString("button_mode_selection"), m_styles[m_mode == Mode.Selection ? "ModeButtonActive" : "ModeButton"], m_glos["ModeButton"]))
                SetMode(Mode.Selection);
            if (GUILayout.Button(GetLocalizedString("button_mode_jobs"), m_styles[m_mode == Mode.Translations ? "ModeButtonActive" : "ModeButton"], m_glos["ModeButton"]))
                SetMode(Mode.Translations);
            //if (GUILayout.Button("Method Calls", m_styles[m_mode == Mode.Methods ? "ModeButtonActive" : "ModeButton"], m_glos["ModeButton"]))
            //    SetMode(Mode.Methods);
            if (GUILayout.Button(GetLocalizedString("button_mode_settings"), m_styles[m_mode == Mode.Settings ? "ModeButtonActive" : "ModeButton"], m_glos["ModeButton"]))
                SetMode(Mode.Settings);
            if (GUILayout.Button(GetLocalizedString("button_mode_log"), m_styles[m_mode == Mode.Log ? "ModeButtonActive" : "ModeButton"], m_glos["ModeButton"]))
                SetMode(Mode.Log);
            if (GUILayout.Button("Debug", m_styles[m_mode == Mode.Debug ? "ModeButtonActive" : "ModeButton"], m_glos["ModeButton"]))
                SetMode(Mode.Debug);
        }
        GUILayout.EndHorizontal();

        // Draw the GUI we need for this mode
        if (m_mode == Mode.Selection)
            OnGUI_Strings();
        else if (m_mode == Mode.Translations)
            OnGUI_Translations();
        else if (m_mode == Mode.Settings)
            OnGUI_Settings();
        else if (m_mode == Mode.Methods)
            OnGUI_Methods();
        else if (m_mode == Mode.Log)
            OnGUI_Log();
        else if (m_mode == Mode.Debug)
            OnGUI_Debug();
        else
            GUILayout.Label(GetLocalizedString("button_mode_unknown") + ": " + m_mode);

        // Always repaint GUI when mouse moves on window
        if (Event.current.type == EventType.MouseMove)
            Repaint();
    }
	
	// Draws the login screen
    private void OnGUI_Login()
    {
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal();
        {
            GUILayout.FlexibleSpace();
            GUILayout.BeginVertical(GUILayout.MinWidth(360f));
            {
                DrawAuthenticate();
            }
            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();
        }
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
    }
	
	// Draws debug information about the utility and provides functions it doesn't normally provide to the user
    private void OnGUI_Debug()
    {
		// Draw token
        GUILayout.BeginHorizontal();
        {
			GUILayout.Label("Token", GUILayout.ExpandWidth(false));
			GUILayout.TextArea(m_transfluentToken);
        }
        GUILayout.EndHorizontal();
		
		// Draw selections
        GUILayout.BeginHorizontal();
        {
            GUILayout.BeginVertical(m_styles["Pane"]);
            {
                GUILayout.Label("Selected Texts");

                m_debugScrollPos = GUILayout.BeginScrollView(m_debugScrollPos);
                {
                    if (m_selectedTexts.Count > 0)
                    {
                        foreach (string textId in m_selectedTexts.Keys)
                        {
                            GUILayout.BeginHorizontal();
                            {
                                GUILayout.Space(24f);
                                GUILayout.Label(textId);
                            }
                            GUILayout.EndHorizontal();
                        }
                    }
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndVertical();

            GUILayout.BeginVertical(m_styles["Pane"], GUILayout.Width(120f));
            {
                GUILayout.Label("Selected Languages");

                m_debugScrollPos = GUILayout.BeginScrollView(m_debugScrollPos);
                {
                    foreach (TransfluentLanguage lang in m_selectedLanguages.Values)
                    {
                        GUILayout.BeginHorizontal();
                        {
                            GUILayout.Space(8f);
                            GUILayout.Label("[" + lang.m_id + "] " + lang.m_code);
                        }
                        GUILayout.EndHorizontal();
                    }
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndVertical();
        }
        GUILayout.EndHorizontal();
		
		// Draw debug buttons
        if (m_transfluentLanguages == null)
            return;

        int selectedTextsCount = m_selectedTexts.Count;
        int selectedLanguageCount = m_selectedLanguages.Count;

        TransfluentLanguage sourceLanguage;
        if (!m_transfluentLanguages.TryGetValue(m_translationSourceLanguageId, out sourceLanguage))
            return;

        if (GUILayout.Button("Test Transfluent Hello"))
        {
            TransfluentMethod helloMethod = new TransfluentMethod(TransfluentMethodType.Hello, TransfluentMethodMode.GET, null, TransfluentDelegateType.None, null);
            m_transfluentMethods.Add(helloMethod);
            helloMethod.AddParameter("name", "Recoil Data Robot");
            helloMethod.SendTo("https://transfluent.com/v2/hello/");

            AddMessage("Requesting 'Hello' from Transfluent Backend API");
        }

        if (GUILayout.Button("Set debug settings. Token won't work with Backend!"))
        {
            m_userEmail = "DebugUser";
            m_userPassword = "DebugPassword";
            m_transfluentToken = "DebugToken";
            EditorPrefs.SetString(m_userEmailKey, m_userEmail);
            EditorPrefs.SetString(m_userPasswordKey, m_userPassword);
            EditorPrefs.SetString(m_transfluentTokenKey, m_transfluentToken);
        }

        if (GUILayout.Button("Delete registry data"))
        {
            EditorPrefs.DeleteKey(m_userEmailKey);
            EditorPrefs.DeleteKey(m_userPasswordKey);
            EditorPrefs.DeleteKey(m_transfluentTokenKey);
            EditorPrefs.DeleteKey(m_translationCostEstimateCurrencySymbolKey);
            EditorPrefs.DeleteKey(m_translationCostEstimatePerWordKey);
        }

        if (GUILayout.Button("Get Texts in selected " + (selectedLanguageCount > 0 ? selectedLanguageCount + " languages" : "source language '" + sourceLanguage.m_code + "'")))
        {
            if (selectedLanguageCount > 0)
            {
                foreach (TransfluentLanguage lang in m_selectedLanguages.Values)
                    MethodRequestTexts(UnityEditor.PlayerSettings.productName, lang.m_id);
            }
            else
                MethodRequestTexts(UnityEditor.PlayerSettings.productName, sourceLanguage.m_id);
        }

        GUI.enabled = selectedTextsCount > 0;

        if (GUILayout.Button("Send selected " + selectedTextsCount + " texts in source language '" + sourceLanguage.m_code + "' to Transfluent Backend"))
        {
            MethodSendTexts(null, TransfluentDelegateType.None, null);
        }

        GUI.enabled = selectedTextsCount > 0 && selectedLanguageCount > 0;

        if (GUILayout.Button("Order translations for " + selectedTextsCount + " text entries in " + selectedLanguageCount + " languages"))
        {
            MethodOrderTranslations(m_translationSourceLanguageId, "Debug translation order test", 3);
        }

        if (GUILayout.Button("Get Translation status for selected " + selectedTextsCount + " texts in " + selectedLanguageCount + " languages"))
        {
            foreach (TransfluentText text in m_selectedTexts.Values)
            {
                foreach (TransfluentLanguage lang in m_selectedLanguages.Values)
                {
                    MethodRequestTextStatus(text.m_id, UnityEditor.PlayerSettings.productName, lang.m_id);
                }
            }
        }

        if (GUILayout.Button("Get TextWordCount for selected " + selectedTextsCount + " texts in " + selectedLanguageCount + " languages"))
        {
            foreach (TransfluentText text in m_selectedTexts.Values)
            {
                foreach (TransfluentTextItem item in text.m_texts)
                {
                    int languageId;
                    if (m_languageIdsByCode.TryGetValue(item.m_languageCode, out languageId))
                    {
                        if (m_selectedLanguages.ContainsKey(languageId))
                        {
                            MethodRequestTextWordCount(text.m_id, UnityEditor.PlayerSettings.productName, languageId, item.m_text);
                        }
                    }
                }
            }
        }

        GUI.enabled = true;
    }
	
	// Draws the message log
    private void OnGUI_Log()
    {
        GUILayout.BeginVertical(m_styles["Log"]);
        {
            m_logScrollPos = GUILayout.BeginScrollView(m_logScrollPos, false, true);
            {
                GUIStyle style;
                for (int i = m_log.Count - 1; i >= 0; i--)
                {
                    Message msg = m_log[i];

                    if (msg.isWarning)
                        style = m_styles["LogEntryWarning"];
                    else if (msg.isError)
                        style = m_styles["LogEntryError"];
                    else
                        style = m_styles["LogEntryNormal"];

                    GUILayout.BeginHorizontal(m_styles["LogEntry"]);
                    {
                        GUILayout.Label(msg.logDateLocal.ToString("yyyy'-'MM'-'dd HH':'mm':'ss"), m_glos["LogEntryDate"]);
                        GUILayout.Label(msg.m_text, style);
                    }
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.EndScrollView();
        }
        GUILayout.EndVertical();
    }
	
	// Draws a list of methods sent to Transfluent API
    private void OnGUI_Methods()
    {
        GUILayout.BeginVertical(m_styles["MethodList"]);
        {
            m_methodScrollPos = GUILayout.BeginScrollView(m_methodScrollPos, false, true);
            {
                for (int i = 0; i < m_transfluentMethods.Count; i++)
                {
                    TransfluentMethod method = m_transfluentMethods[i];

                    GUILayout.BeginHorizontal(m_styles["MethodItem"]);
                    {
                        GUILayout.Label(method.m_timeSent.ToString("yyyy'-'MM'-'dd HH':'mm':'ss"), m_glos["MethodSent"]);

                        if (GUILayout.Button(method.m_mode.ToString() + " " + method.m_type.ToString(), m_styles["MethodButton"]))
                        {
                            if (m_focusedMethod == i)
                                m_focusedMethod = -1;
                            else
                                m_focusedMethod = i;
                        }

                        if (method.error == null)
                            GUILayout.Label((method.isDone ? "DONE" : "WAIT"), m_styles["MethodItemError"], m_glos["MethodDone"]);
                        else
                            GUILayout.Label("ERROR", m_styles["MethodItemError"], m_glos["MethodDone"]);
                    }
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.EndScrollView();
        }
        GUILayout.EndVertical();

        GUILayout.BeginVertical(m_styles["MethodDetails"], m_glos["MethodDetails"]);
        {
            if (m_focusedMethod >= 0 && m_focusedMethod < m_transfluentMethods.Count)
            {
                TransfluentMethod method = m_transfluentMethods[m_focusedMethod];
                m_methodDetailScrollPos = GUILayout.BeginScrollView(m_methodDetailScrollPos, false, true);
                {
                    GUILayout.BeginHorizontal(m_styles["MethodItem"]);
                    {
                        GUILayout.Label("Mode & Type", m_glos["MethodDetailLabel"]);
                        GUILayout.Label(method.m_mode.ToString() + " " + method.m_type.ToString());
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal(m_styles["MethodItem"]);
                    {
                        GUILayout.Label("Sent", m_glos["MethodDetailLabel"]);
                        GUILayout.Label(method.m_timeSent.ToString("yyyy'-'MM'-'dd HH':'mm':'ss"));
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal(m_styles["MethodItem"]);
                    {
                        System.TimeSpan duration = method.GetDuration();
                        GUILayout.Label("Time taken", m_glos["MethodDetailLabel"]);
                        GUILayout.Label(duration.TotalSeconds.ToString("F3") + " seconds");
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal(m_styles["MethodItem"]);
                    {
                        string parametersText = "";
                        foreach (TransfluentMethod.Parameter parameter in method.GetParameters())
                        {
                            if (parametersText.Length > 0)
                                parametersText += "\n";

                            parametersText += parameter.m_name + " = ";

                            if (parameter is TransfluentMethod.ParameterText)
                                parametersText += "'" + ((TransfluentMethod.ParameterText)parameter).m_text + "'";
                            else if (parameter is TransfluentMethod.ParameterValue)
                                parametersText += ((TransfluentMethod.ParameterValue)parameter).m_value;
                            else
                                parametersText += "NULL";
                        }

                        GUILayout.Label("Parameters", m_glos["MethodDetailLabel"]);
                        GUILayout.Label(parametersText, m_styles["MethodParameters"]);
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal(m_styles["MethodItem"]);
                    {
                        GUILayout.Label("Response", m_glos["MethodDetailLabel"]);
                        GUILayout.Label((method.isDone ? method.parsedResponse : "Waiting for response..."), m_styles["MethodResponse"]);
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndScrollView();
            }
            else
                GUILayout.Label("Select a method");
        }
        GUILayout.EndVertical();
    }
	
	// Draws text data
    private void OnGUI_Strings()
    {
        GUILayout.BeginHorizontal(m_styles["TopRow"], m_glos["TopRow"]);
        {
            //DrawGroups();
            DrawTextList();
            DrawLanguages();
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal(m_styles["BottomRow"], m_glos["BottomRow"]);
        {
            DrawDetails();
            //DrawNewMethod();
            DrawTranslate();
        }
        GUILayout.EndHorizontal();
    }
	
	// Draws the utility settings
    private void OnGUI_Settings()
    {
        GUILayout.BeginHorizontal();
        {
            GUILayout.BeginVertical();
            {
                GUILayout.Label(GetLocalizedString("label_user_logged") + " " + m_userEmail);
                if (GUILayout.Button(GetLocalizedString("button_user_log_out")))
                {
                    m_authRequest = null;
                    m_transfluentToken = null;
                    EditorPrefs.DeleteKey(m_transfluentTokenKey);
                    SetMode(Mode.Authenticate);
                }

                GUILayout.Space(8f);

                GUILayout.Label(GetLocalizedString("label_translation_cost_estimate_per_word"));
                GUILayout.BeginVertical(m_styles["PaneArea"]);
                {
                    string newCostCurrencySymbol = EditorGUILayout.TextField(GetLocalizedString("field_currency_symbol"), m_translationCostEstimateCurrencySymbol);

                    if (newCostCurrencySymbol != m_translationCostEstimateCurrencySymbol)
                    {
                        m_translationCostEstimateCurrencySymbol = newCostCurrencySymbol;
                        EditorPrefs.SetString(m_translationCostEstimateCurrencySymbolKey, m_translationCostEstimateCurrencySymbol);
                        AddMessage("Set Transfluent translation cost estimate currency symbol to EditorPreferences.\nSymbol: " + m_translationCostEstimateCurrencySymbol);
                    }
                
                    float newCostEstimate = EditorGUILayout.FloatField(GetLocalizedString("field_translation_cost_per_word") + " (" + m_translationCostEstimateCurrencySymbol + ")", m_translationCostEstimatePerWord);

                    if (newCostEstimate != m_translationCostEstimatePerWord)
                    {
                        m_translationCostEstimatePerWord = newCostEstimate;
                        EditorPrefs.SetFloat(m_translationCostEstimatePerWordKey, m_translationCostEstimatePerWord);
                        AddMessage("Set Transfluent translation cost estimate per word to EditorPreferences.\nPer word cost estimate: " + m_translationCostEstimatePerWord + " " + m_translationCostEstimateCurrencySymbol);
                    }
                }
                GUILayout.EndVertical();

                GUILayout.FlexibleSpace();
            }
            GUILayout.EndVertical();
			
			// Draw known languages
            GUILayout.BeginVertical(m_glos["TransfluentLanguagePane"]);
            {
                GUILayout.Label(GetLocalizedString("label_languages_all"));

                m_transfluentLanguageScrollPos = GUILayout.BeginScrollView(m_transfluentLanguageScrollPos, m_styles["Pane"]);
                {
                    bool inUse;
                    GUIStyle style;
                    string buttonText;
                    foreach (TransfluentLanguage lang in m_transfluentLanguages.Values)
                    {
                        inUse = m_languages.Contains(lang.m_id);
                        
                        GUILayout.BeginHorizontal();
                        {
                            if (inUse)
                            {
                                buttonText = GetLocalizedString("button_language_remove");
                                style = m_styles["TransfluentLanguageRemoveButton"];
                            }
                            else
                            {
                                buttonText = GetLocalizedString("button_language_add");
                                style = m_styles["TransfluentLanguageAddButton"];
                            }
                            
                            if (GUILayout.Button(buttonText, style, m_glos["TransfluentLanguageAddRemoveButton"]))
                            {
                                m_dataChanged = true;

                                if (inUse)
                                {
                                    if (EditorUtility.DisplayDialog("Confirm removal of language", "Are you sure you want to remove\n" + lang.m_name + " [" + lang.m_code + "] language?\nAll text entries in this language will be deleted.", "Yes", "Cancel"))
                                        RemoveLanguage(lang);
                                }
                                else
                                    AddLanguage(lang);
                            }

                            //GUILayout.Label(lang.m_id.ToString(), GUILayout.Width(30f));
                            GUILayout.Label(lang.m_code, GUILayout.Width(50f));
                            GUILayout.Label(lang.m_name);
                        }
                        GUILayout.EndHorizontal();
                    }
                }
                GUILayout.EndScrollView();

                if (m_languagesRequest == null)
                {
                    if (GUILayout.Button(GetLocalizedString("button_languages_get_from_backend")))
                    {
                        MethodRequestLanguages();
                    }
                }
                else
                    GUILayout.Button(GetLocalizedString("button_languages_updating"));
            }
            GUILayout.EndVertical();
        }
        GUILayout.EndHorizontal();
    }
	
	// Draws a list of ordered translations
    private void OnGUI_Translations()
    {
        GUILayout.BeginHorizontal(m_styles["TranslationEntryCategories"]);
        {
            //GUILayout.Label("Ordered On", m_styles["TranslationEntryCategory"], m_glos["TranslationOrdered"]);
            GUILayout.Label(GetLocalizedString("label_translation_group_id"), m_styles["TranslationEntryCategory"], m_glos["TranslationGroupId"]);
            GUILayout.Label(GetLocalizedString("label_translation_text_id"), m_styles["TranslationEntryCategory"], m_glos["TranslationTextId"]);
            GUILayout.Label(GetLocalizedString("label_translation_languages"), m_styles["TranslationEntryCategory"], m_glos["TranslationLanguages"]);

            if (GUILayout.Button(GetLocalizedString("button_translations_check_all") + " " + m_translations.Count, m_glos["TranslationCheckStatusAll"]))
            {
                foreach (TransfluentTranslationEntry entry in m_translations.Values)
                    MethodRequestTextStatus(entry.m_textId, entry.m_groupId, entry.m_targetLanguageId);
            }
        }
        GUILayout.EndHorizontal();

        m_translationsScrollPos = GUILayout.BeginScrollView(m_translationsScrollPos, false, true);
        {
            string sourceLanguageCode, targetLanguageCode;
            TransfluentLanguage sourceLanguage, targetLanguage;
            TransfluentTranslationEntry entry;
            foreach (string entryId in m_translations.Keys)
            {
                entry = m_translations[entryId];

                if (m_transfluentLanguages.TryGetValue(entry.m_sourceLanguageId, out sourceLanguage))
                    sourceLanguageCode = sourceLanguage.m_code;
                else
                    sourceLanguageCode = GetLocalizedString("language_code_unknown");

                if (m_transfluentLanguages.TryGetValue(entry.m_targetLanguageId, out targetLanguage))
                    targetLanguageCode = targetLanguage.m_code;
                else
                    targetLanguageCode = GetLocalizedString("language_code_unknown");

                GUILayout.BeginHorizontal(m_styles["TranslationEntry"]);
                {
                    //GUILayout.Label(entry.ordered.ToString("yyyy'-'MM'-'dd HH':'mm':'ss"), m_glos["TranslationOrdered"]);
                    GUILayout.Label(entry.m_groupId, m_glos["TranslationGroupId"]);
                    GUILayout.Label(entry.m_textId, m_glos["TranslationTextId"]);
                    GUILayout.Label(sourceLanguageCode + " [" + entry.m_sourceLanguageId + "] -> " + targetLanguageCode + " [" + entry.m_targetLanguageId + "]", m_glos["TranslationLanguages"]);

                    if (GUILayout.Button(GetLocalizedString("button_translation_details"), m_glos["TranslationFocus"]))
                    {
                        m_focusedTranslationId = entryId;
                    }
                }
                GUILayout.EndHorizontal();
            }
        }
        GUILayout.EndScrollView();

        GUILayout.BeginVertical(m_styles["TranslationDetails"], m_glos["TranslationDetails"]);
        {
            TransfluentTranslationEntry entry;
            TransfluentLanguage sourceLanguage = null;
            TransfluentLanguage targetLanguage = null;
            Dictionary<string, TransfluentText> group = null;
            TransfluentText text = null;
            TransfluentTextItem textItem = null;

            if (m_translations.TryGetValue(m_focusedTranslationId, out entry))
            {
                m_transfluentLanguages.TryGetValue(entry.m_sourceLanguageId, out sourceLanguage);
                m_transfluentLanguages.TryGetValue(entry.m_targetLanguageId, out targetLanguage);

                if (m_texts.TryGetValue(entry.m_textId, out text))
                {
                    if (sourceLanguage != null)
                    {
                        foreach (TransfluentTextItem item in text.m_texts)
                        {
                            if (item.m_languageCode == sourceLanguage.m_code)
                            {
                                textItem = item;
                                break;
                            }
                        }
                    }
                }

                m_translationDetailsPos = GUILayout.BeginScrollView(m_translationDetailsPos, false, true);
                {
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label(GetLocalizedString("label_translation_ordered_on") + " " + entry.ordered.ToLocalTime().ToString("yyyy'-'MM'-'dd HH':'mm':'ss"));
                        GUILayout.FlexibleSpace();
                        GUILayout.Label(GetLocalizedString("label_last_checked_on") + " " + entry.lastChecked.ToLocalTime().ToString("yyyy'-'MM'-'dd HH':'mm':'ss"));
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal(m_styles["TranslationEntry"]);
                    {
                        GUILayout.Label(GetLocalizedString("label_translation_group_id"), m_glos["TranslationDetailLabel"]);
                        GUILayout.Label(entry.m_groupId);
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal(m_styles["TranslationEntry"]);
                    {
                        GUILayout.Label(GetLocalizedString("label_translation_text_id"), m_glos["TranslationDetailLabel"]);
                        GUILayout.Label(entry.m_textId);
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal(m_styles["TranslationEntry"]);
                    {
                        TimeSpan span = DateTime.UtcNow.Subtract(entry.ordered);
                        GUILayout.Label(GetLocalizedString("label_translation_time_taken"), m_glos["TranslationDetailLabel"]);

                        // TODO: Format timespan into internationally readable text
                        if (span.Days > 0)
                            GUILayout.Label(span.Days + " day" + (span.Days > 1 ? "s " : " ") + span.Hours + " hour" + (span.Hours > 1 ? "s " : " "));
                        else if (span.Hours > 0)
                            GUILayout.Label(span.Hours + " hour" + (span.Hours > 1 ? "s " : " ") + span.Minutes + " minute" + (span.Minutes > 1 ? "s " : " "));
                        else
                            GUILayout.Label(span.Minutes + " minute" + (span.Minutes > 1 ? "s " : " "));
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal(m_styles["TranslationEntry"]);
                    {
                        GUILayout.Label(GetLocalizedString("label_translation_target_language"), m_glos["TranslationDetailLabel"]);

                        if (targetLanguage != null)
                            GUILayout.Label(targetLanguage.m_code + " [" + targetLanguage.m_id + "] " + targetLanguage.m_name);
                        else
                            GUILayout.Label(GetLocalizedString("label_translation_language_not_found") + " " + entry.m_targetLanguageId);
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal(m_styles["TranslationEntry"]);
                    {
                        GUILayout.Label(GetLocalizedString("label_translation_source_language"), m_glos["TranslationDetailLabel"]);

                        if (sourceLanguage != null)
                            GUILayout.Label(sourceLanguage.m_code + " [" + sourceLanguage.m_id + "] " + sourceLanguage.m_name);
                        else
                            GUILayout.Label(GetLocalizedString("label_translation_language_not_found") + " " + entry.m_sourceLanguageId);
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal(m_styles["TranslationEntry"]);
                    {
                        GUILayout.Label(GetLocalizedString("label_translation_source_text"), m_glos["TranslationDetailLabel"]);

                        if (textItem != null)
                            GUILayout.Label(textItem.m_text);
                        else if (text != null)
                            GUILayout.Label(GetLocalizedString("label_translation_text_entry_not_found_in_language").Replace("[TEXT_ID]", entry.m_textId).Replace("[GROUP_ID]", entry.m_groupId).Replace("[LANGUAGE_ID]", entry.m_sourceLanguageId.ToString()));
                        else if (group != null)
                            GUILayout.Label(GetLocalizedString("label_translation_text_entry_not_found_in_group").Replace("[TEXT_ID]", entry.m_textId).Replace("[GROUP_ID]", entry.m_groupId));
                        else
                            GUILayout.Label(GetLocalizedString("label_translation_group_entry_not_found").Replace("[GROUP_ID]", entry.m_groupId));
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndScrollView();

                if (GUILayout.Button(GetLocalizedString("button_translation_check_updates")))
                    MethodRequestTextStatus(entry.m_textId, entry.m_groupId, entry.m_targetLanguageId);
            }
            else
                GUILayout.Label(GetLocalizedString("label_translation_focused_text_id_not_found").Replace("[ID]", m_focusedTranslationId));
        }
        GUILayout.EndVertical();
    }
	
	// Draws the login credential fields
    private void DrawAuthenticate()
    {
        bool invalidCredentials = false;

        GUILayout.BeginVertical(GUILayout.Height(Mathf.Max(position.height / 3f, 180f)));
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();

                if (m_authRequest != null && m_authRequest.error != null)
                {
                    invalidCredentials = (m_authRequest.backendError != null && m_authRequest.backendError.Contains("EBackendSecurityViolation"));

                    if (invalidCredentials)
                        GUILayout.Label(GetLocalizedString("label_login_invalid_credentials"), m_styles["LoginLabel"], m_glos["LoginLabel"]);
                    else
                    {
                        // TODO: Handle errors in login
                        GUILayout.Label("An error occurred while authenticating:\n" + m_authRequest.error + "\nType: " + m_authRequest.backendError, m_styles["LoginLabel"], m_glos["LoginLabel"]);
                    }
                }
                else
                    GUILayout.Label(GetLocalizedString("label_login_welcome"), m_styles["LoginLabel"], m_glos["LoginLabel"]);

                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();

            if (invalidCredentials)
            {
                if (GUILayout.Button(GetLocalizedString("button_login_register")))
                {
                    Application.OpenURL("http://www.transfluent.com/terms-of-service/");

                    if (EditorUtility.DisplayDialog(GetLocalizedString("dialog_confirm_EULA_title"), GetLocalizedString("dialog_confirm_EULA_message"), GetLocalizedString("dialog_confirm_EULA_button_agree"), GetLocalizedString("dialog_confirm_EULA_button_cancel")))
                    {
                        //if (m_userEmail.Contains("@") && m_userEmail.Contains("."))
                        {
                            AddWarning("TODO: We should verify the email address is of valid structure");
                            MethodRegister(m_userEmail);
                        }
                    }
                }
            }

            GUILayout.FlexibleSpace();

            GUILayout.BeginVertical(m_styles["PaneArea"]);
            {
                string newUserEmail = EditorGUILayout.TextField(GetLocalizedString("field_login_email"), m_userEmail);

                if (newUserEmail != m_userEmail)
                {
                    m_userEmail = newUserEmail;
                    EditorPrefs.SetString(m_userEmailKey, EncryptString(m_userEmail));
                    //AddMessage("Set Transfluent User email address to EditorPreferences.\nAddress: '" + m_userEmail + "'");
                }

                string newUserPassword = EditorGUILayout.PasswordField(GetLocalizedString("field_login_password"), m_userPassword);

                if (newUserPassword != m_userPassword)
                {
                    m_userPassword = newUserPassword;
                    EditorPrefs.SetString(m_userPasswordKey, EncryptString(m_userPassword));
                    //AddMessage("Set Transfluent User password to EditorPreferences.\nPassword: '" + m_userPassword + "'");
                }

                GUILayout.BeginHorizontal();
                {
                    GUI.enabled = (m_authRequest == null || m_authRequest.error != null) && m_userEmail.Length > 0 && m_userPassword.Length > 0;
                    if (GUILayout.Button(invalidCredentials ? GetLocalizedString("button_login_retry") : GetLocalizedString("button_login_next")))
                    {
                        m_authRequest = MethodAuthenticate(m_userEmail, m_userPassword);
                    }
                    GUI.enabled = true;

                    if (invalidCredentials)
                    {
                        if (GUILayout.Button(GetLocalizedString("button_login_forgot_password")))
                        {
                            Application.OpenURL("http://www.transfluent.com/contact/");
                        }
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }
        GUILayout.EndVertical();
    }
	
	// Draws text item details
    private void DrawDetails()
    {
        //bool updatePressed = false;

        GUILayout.BeginVertical(m_styles["PaneArea"], m_glos["DetailPane"]);
        {
            if (m_focusedTextId.Length > 0)
            {
                TransfluentText text;
                if (m_texts.TryGetValue(m_focusedTextId, out text))
                {
                    m_textDetailScrollPos = GUILayout.BeginScrollView(m_textDetailScrollPos, false, true);
                    {
                        DrawDetailsTextId(text);

                        GUILayout.BeginHorizontal(m_styles["DetailItem"], m_glos["DetailItem"]);
                        {
                            GUILayout.Label(GetLocalizedString("label_text_details_entry_count").Replace("[ENTRY_COUNT]", text.m_texts.Count.ToString()));
                        }
                        GUILayout.EndHorizontal();
						
						// Draw each entry item in the text
                        foreach (TransfluentTextItem item in text.m_texts)
                            DrawDetailsTextItem(text, item);
                    }
                    GUILayout.EndScrollView();
					
                    // Handle data changes
                    /*if (updatePressed)
                    {
                        Dictionary<string, string> itemChanges;
                        
                        List<string> entryList = new List<string>();
                        string textId, langCode, newText;
						
						// Handle text changes
                        if (m_textItemChanges.TryGetValue(text.m_id, out itemChanges))
                        {
                            textId = text.m_id;
                            foreach (TransfluentTextItem item in text.m_texts)
                            {
                                langCode = item.m_languageCode;
                                if (itemChanges.TryGetValue(langCode, out newText))
                                {
                                    m_dataChanged = true;
                                    AddMessage("Text item text change: '" + item.m_text + "' -> '" + itemChanges[langCode] + "' in text entry '" + textId + "', language " + langCode);

                                    if (item.m_text.Length > 0)
                                    {
                                        item.m_text = itemChanges[langCode];
                                        itemChanges.Remove(langCode);
                                    }
                                    else
                                        itemChanges[langCode] = item.m_text;
                                }
                            }

                            // Check everything was handled properly
                            if (itemChanges.Count == 0)
                                m_textItemChanges.Remove(textId);
                            else
                            {
                                string msg = "Not all changes were saved for text entry '" + textId + "'!";
                                foreach (string key in itemChanges.Keys)
                                    msg += "\nLangCode='" + key + "' Text='" + itemChanges[key] + "'";

                                AddError(msg);
                            }
                        }
                        else
                            AddWarning("No changes found for item with id '" + text.m_id + "'");

                        SetTextFocus(m_newTextId);
                        GUI.FocusControl("AddText");
                    }*/
                }
                else
                {
                    GUILayout.Label(GetLocalizedString("label_text_details_id_not_found").Replace("[TEXT_ID]", m_focusedTextId));
                    GUILayout.FlexibleSpace();
                }
            }
            else
            {
                GUILayout.Label(GetLocalizedString("label_text_details_no_selection"));
                GUILayout.FlexibleSpace();
            }
        }
        GUILayout.EndVertical();
    }

    private void DrawDetailsTextId(TransfluentText text)
    {
        GUIStyle itemStyle;
        string oldTextId = text.m_id;
        Dictionary<string, string> itemChanges;

        GUILayout.BeginHorizontal(m_styles["DetailItem"], m_glos["DetailItem"]);
        {
            if (m_newTextId.Length > 0 && m_newTextId == oldTextId)
                itemStyle = m_styles["DetailItemLabel"];
            else
                itemStyle = m_styles["DetailItemLabelTextUnsaved"];

            GUILayout.Label(GetLocalizedString("label_text_details_id"), itemStyle, m_glos["DetailItemLabel"]);

            m_newTextId = EditorGUILayout.TextField(oldTextId);

            // Handle id change
            if (m_newTextId != oldTextId && m_newTextId.Length > 0)
            {
                if (!m_texts.ContainsKey(m_newTextId))
                {
                    AddMessage("Text Id change: '" + oldTextId + "' -> '" + m_newTextId + "'");

                    // Mark up the change but retain the original id information if there already was a change
                    string originalId;
                    if (m_textIdChanges.TryGetValue(oldTextId, out originalId))
                    {
                        m_textIdChanges.Remove(oldTextId);
                        m_textIdChanges.Add(m_newTextId, originalId);
                    }
                    else
                        m_textIdChanges.Add(m_newTextId, oldTextId);

                    // Also mark up the item changes for the new id and remove the old one
                    if (m_textItemChanges.TryGetValue(oldTextId, out itemChanges))
                    {
                        m_textItemChanges.Remove(oldTextId);
                        m_textItemChanges.Add(m_newTextId, itemChanges);
                    }

                    // Change the text item's id
                    m_texts.Add(m_newTextId, text);
                    m_texts.Remove(oldTextId);

                    text.m_id = m_newTextId;

                    m_focusedTextId = m_newTextId;

                    m_dataChanged = true;
                }
            }
        }
        GUILayout.EndHorizontal();
    }

    private void DrawDetailsTextItem(TransfluentText text, TransfluentTextItem item)
    {
        GUIStyle itemStyle;
        bool textHasChanges, itemHasChanges;
        Dictionary<string, string> itemChanges;
        string oldItemText, newItemText;

        textHasChanges = m_textItemChanges.TryGetValue(text.m_id, out itemChanges);

        if (textHasChanges)
            itemHasChanges = itemChanges.ContainsKey(item.m_languageCode);
        else
            itemHasChanges = false;

        if (itemHasChanges)
            itemStyle = m_styles["DetailItemLabelTextUnsaved"];
        else
            itemStyle = m_styles["DetailItemLabel"];

        oldItemText = item.m_text;

        GUILayout.BeginHorizontal(m_styles["DetailItem"], m_glos["DetailItem"]);
        {
            GUILayout.Label("[" + item.m_languageCode + "]", itemStyle, m_glos["DetailItemLabel"]);

            newItemText = EditorGUILayout.TextField(oldItemText);
        }
        GUILayout.EndHorizontal();

        // Handle text item changes
        if (newItemText != oldItemText)
        {
            // Mark up the changes if there were none previously
            if (!textHasChanges)
            {
                itemChanges = new Dictionary<string, string>();
                itemChanges.Add(item.m_languageCode, oldItemText);
                m_textItemChanges.Add(text.m_id, itemChanges);
            }
            else if (!itemHasChanges)
                itemChanges.Add(item.m_languageCode, oldItemText);

            item.m_text = newItemText;
        }
    }

    // Draw a list of used languages
    private void DrawLanguages()
    {
        GUILayout.BeginVertical(m_styles["PaneArea"], m_glos["LanguagePane"]);
        {
            int newSourceLanguageId = -1;
            int langCount = Mathf.Max(m_languages.Count, 1);
            string[] popupNames = new string[langCount];
            int[] popupValues = new int[langCount];

            if (m_languages.Count > 0)
            {
                int i = 0;
                TransfluentLanguage lang;
                foreach (int langId in m_languages)
                {
                    if (m_transfluentLanguages.TryGetValue(langId, out lang))
                    {
                        popupNames[i] = "[" + lang.m_code + "] " + lang.m_name;
                        popupValues[i] = lang.m_id;
                        i++;
                    }
                    else
                    {
                        popupNames[i] = GetLocalizedString("label_language_id_not_found").Replace("[LANG_ID]", langId.ToString());
                        popupValues[i] = langId;
                        i++;
                    }
                }
            }
            else
            {
                popupNames[0] = GetLocalizedString("label_languages_none_found");
                popupValues[0] = -1;
            }
            
                GUILayout.Label(GetLocalizedString("popup_source_language"), m_glos["SourceLanguageLabel"]);
                newSourceLanguageId = EditorGUILayout.IntPopup(m_translationSourceLanguageId, popupNames, popupValues);

            if (newSourceLanguageId != -1 && newSourceLanguageId != m_translationSourceLanguageId)
            {
                m_dataChanged = true;
                SetSourceLanguage(newSourceLanguageId);
            }

            GUILayout.BeginHorizontal(m_styles["PaneTitle"], m_glos["PaneTitle"]);
            {
                GUILayout.Label(GetLocalizedString("label_languages_in_use"));
            }
            GUILayout.EndHorizontal();

            m_languageScrollPos = GUILayout.BeginScrollView(m_languageScrollPos, false, true, GUI.skin.horizontalScrollbar, GUI.skin.verticalScrollbar, m_styles["Pane"]);
            {
                if (m_languages != null && m_languages.Count > 0)
                {
                    foreach (int langId in m_languages)
                    {

                        DrawLanguage(langId);
                    }
                }
            }
            GUILayout.EndScrollView();

            if (GUILayout.Button(GetLocalizedString("button_languages_add")))
            {
                SetMode(Mode.Settings);
            }
        }
        GUILayout.EndVertical();
    }

    private void DrawLanguage(int languageId)
    {
        TransfluentLanguage lang;
        bool selected, newSelected;

        if (!m_transfluentLanguages.TryGetValue(languageId, out lang))
        {
            lang = null;
            selected = false;
            newSelected = false;
            GUI.enabled = false;
        }
        else
        {
            selected = m_selectedLanguages.ContainsKey(lang.m_id);

            if (lang.m_id != m_translationSourceLanguageId)
                GUI.enabled = true;
            else
            {
                GUI.enabled = false;
                if (selected)
                    m_selectedLanguages.Remove(lang.m_id);
            }
        }
        
        GUILayout.BeginHorizontal();
        {
            newSelected = GUILayout.Toggle(selected, "", GUILayout.ExpandWidth(false));
            
            if (lang != null)
            {
                if (newSelected != selected)
                {
                    if (newSelected)
                    {
                        m_selectedLanguages.Add(lang.m_id, lang);
                        //Debug.Log("Selected " + lang.m_id);
                    }
                    else
                    {
                        m_selectedLanguages.Remove(lang.m_id);
                        //Debug.Log("Removed selection of " + lang.m_id);
                    }
                }

                //GUILayout.Label("Code", GUILayout.ExpandWidth(false));
                GUILayout.Label(lang.m_code, GUILayout.Width(60f));
                //GUILayout.Label("Id", GUILayout.ExpandWidth(false));
                //GUILayout.Label(lang.m_id.ToString(), GUILayout.Width(30f));
                //GUILayout.Label("Name", GUILayout.ExpandWidth(false));
                GUILayout.Label(lang.m_name);
            }
            else
            {
                //AddError("Couldn't find language by the id " + langId + " in transfluent languages!");
                GUILayout.Label(GetLocalizedString("label_language_id_not_found").Replace("[LANG_ID]", languageId.ToString()));
            }
        }
        GUILayout.EndHorizontal();

        GUI.enabled = true;
    }

    // Draws the GUI to order translations
    private void DrawTranslate()
    {
        int selectedLanguageCount = 0;

        foreach (int langId in m_selectedLanguages.Keys)
        {
            if (langId != m_translationSourceLanguageId)
                selectedLanguageCount++;
        }

        int selectedTextCount = m_selectedTexts.Count;

        GUILayout.BeginVertical(m_styles["PaneArea"], m_glos["MethodPane"]);
        {
            GUILayout.BeginVertical(m_styles["MethodPane"]);
            {
                // Token is needed for this
                if (m_transfluentToken.Length == 0)
                {
                    GUILayout.Label(GetLocalizedString("label_translate_not_authenticated"));
                    GUILayout.FlexibleSpace();
                }
                else
                {
                    GUILayout.Label(GetLocalizedString("label_translate_selection").Replace("[TEXT_COUNT]", selectedTextCount.ToString()).Replace("[LANGUAGE_COUNT]", selectedLanguageCount.ToString()));

                    GUILayout.Space(8.0f);

                    GUILayout.Label(GetLocalizedString("label_translate_translator_level"), GUILayout.ExpandWidth(false));

                    int[] levels = (int[])Enum.GetValues(typeof(TranslationLevel));
                    string[] levelNames = new string[levels.Length];
                    for (int i = 0; i < levels.Length; i++)
                        levelNames[i] = GetLocalizedString("popup_translate_translator_level_" + levels[i]);

                    int newTranslatorLevel = EditorGUILayout.IntPopup(m_translatorLevel, levelNames, levels);

                    if (newTranslatorLevel != m_translatorLevel)
                    {
                        m_translatorLevel = newTranslatorLevel;
                        EditorPrefs.SetInt(m_translatorLevelKey, m_translatorLevel);
                    }

                    GUILayout.Space(8.0f);

                    GUILayout.Label(GetLocalizedString("label_translate_comment"));
                    m_translationOrderComment = EditorGUILayout.TextArea(m_translationOrderComment, m_styles["MethodTranslateComment"], m_glos["MethodTranslateComment"]);

                    GUILayout.FlexibleSpace();
                    
                    GUI.enabled = (selectedTextCount > 0 && selectedLanguageCount > 0);

                    if (GUILayout.Button(GetLocalizedString("button_translate")))
                    {
                        CombinedSendTranslateTexts();
                    }

                    GUI.enabled = true;
                }
            }
            GUILayout.EndVertical();
        }
        GUILayout.EndVertical();
    }

    // Draws the text item explorer
    private void DrawTextList()
    {
        GUILayout.BeginVertical(m_styles["PaneArea"], m_glos["TextPane"]);
        {
            // Draw the search bar
            GUILayout.BeginHorizontal(m_styles["PaneTitle"], m_glos["PaneTitle"]);
            {
                GUILayout.Label(GetLocalizedString("label_texts_search"), GUILayout.ExpandWidth(false));

                string newSearchString = m_searchString;

                GUI.SetNextControlName("SearchTexts");
                newSearchString = EditorGUILayout.TextField(newSearchString);

                if (GUI.GetNameOfFocusedControl() == "SearchTexts")
                {
                    if (Event.current.keyCode == KeyCode.Escape)
                        newSearchString = "";
                }

                if (!newSearchString.Equals(m_searchString))
                {
                    m_searchString = newSearchString;

                    if (m_searchString.Length > 0)
                        m_searchResultIds = null;
                }

                GUI.SetNextControlName("ClearSearch");
                if (GUILayout.Button("X", GUILayout.Width(22f), GUILayout.Height(16f)))
                {
                    m_searchString = "";
                    GUI.FocusControl("ClearSearch");
                }
            }
            GUILayout.EndHorizontal();

            // Draw the text items
            m_textScrollPos = GUILayout.BeginScrollView(m_textScrollPos, false, true, GUI.skin.horizontalScrollbar, GUI.skin.verticalScrollbar, m_styles["Pane"]);
            {
                if (m_searchString.Length > 0 && m_searchResultIds != null)
                {
                    // Draw the search results
                    if (m_searchResultIds.Count > 0)
                    {
                        foreach (string textId in m_searchResultIds)
                        {
                            TransfluentText text;
                            if (m_texts.TryGetValue(textId, out text))
                                DrawTextListItem(text, m_texts);
                            else
                                GUILayout.Label("ERROR, couldn't find text by the id of " + textId);
                        }
                    }
                    else
                        GUILayout.Label("No hits found in search");
                }
                else
                {
                    // Draw all text items
                    foreach (TransfluentText text in m_texts.Values)
                    {
                        if (text != null)
                            DrawTextListItem(text, m_selectedTexts);
                            //DrawText(text, selectionGroup);
                        else
                            GUILayout.Label("ERROR, text was null");
                    }
                }
            }
            GUILayout.EndScrollView();

            // Draw the button to add texts
            GUI.SetNextControlName("AddText");
            if (GUILayout.Button(GetLocalizedString("button_texts_add")))
            {
                GUI.FocusControl("AddText");

                m_dataChanged = true;

                // Find a unique new text id
                string newTextId = "NEW_TEXT";

                if (m_texts.ContainsKey(newTextId))
                {
                    int i = 1;
                    while (m_texts.ContainsKey(newTextId + "_" + i))
                        i++;

                    newTextId += "_" + i;
                }

                TransfluentText text = new TransfluentText(newTextId, UnityEditor.PlayerSettings.productName);

                // Add all used languages to the text item
                foreach (int langId in m_languages)
                {
                    TransfluentLanguage lang;
                    if (m_transfluentLanguages.TryGetValue(langId, out lang))
                    {
                        text.m_texts.Add(new TransfluentTextItem(lang.m_code, ""));
                    }
                }

                m_texts.Add(text.m_id, text);

                m_newTextId = text.m_id;
                m_focusedTextId = text.m_id;

                // This scrolls to the bottom even if we don't know the extents of the scrollview content
                m_textScrollPos = new Vector2(0f, float.MaxValue);
            }
        }
        GUILayout.EndVertical();
    }

    // Draws a single text item in the list
    private void DrawTextListItem(TransfluentText text, Dictionary<string, TransfluentText> selectionGroup)
    {
        bool selected, newSelected;

        selected = selectionGroup.ContainsKey(text.m_id);

        GUILayout.BeginHorizontal();
        {
            newSelected = GUILayout.Toggle(selected, "", GUILayout.ExpandWidth(false));

            if (newSelected != selected)
            {
                if (newSelected)
                {
                    selectionGroup.Add(text.m_id, text);
                    //Debug.Log("Selected " + text.m_id);
                }
                else
                {
                    selectionGroup.Remove(text.m_id);
                    //Debug.Log("Removed selection of " + text.m_id);
                }
            }

            bool isFocused = (text.m_id == m_focusedTextId);
            bool hasChanges = m_textItemChanges.ContainsKey(text.m_id);

            GUIStyle style;
            if (isFocused)
                style = m_styles[hasChanges ? "FocusedTextChanged" : "FocusedText"];
            else
                style = m_styles[hasChanges ? "UnfocusedTextChanged" : "UnfocusedText"];

            if (GUILayout.Button(text.m_id, style))
            {
                if (isFocused)
                    SetTextFocus("");
                else
                    SetTextFocus(text.m_id);
            }
        }
        GUILayout.EndHorizontal();
    }

    // Call this to request all available languages from Transfluent API Backend
    private void MethodRequestLanguages()
    {
        TransfluentMethod languagesMethod = new TransfluentMethod(TransfluentMethodType.Languages, TransfluentMethodMode.GET, DelegateTransfluentLanguagesUpdated, TransfluentDelegateType.TransfluentLanguagesUpdated, null);
        m_transfluentMethods.Add(languagesMethod);
        languagesMethod.SendTo("https://transfluent.com/v2/languages/");

        AddMessage("Requesting Languages from Transfluent Backend API");

        m_languagesRequest = languagesMethod;
    }

    private void DelegateTransfluentLanguagesUpdated(TransfluentDelegateType type, object[] data)
    {
        if (m_translationSourceLanguageId == -1)
        {
            if (m_transfluentLanguages.ContainsKey(148))
                SetSourceLanguage(148);
        }
        else if (!m_transfluentLanguages.ContainsKey(m_translationSourceLanguageId))
            AddWarning("Selected source language " + m_translationSourceLanguageId + " could not be found");
    }

    // Use this to request a text in a certain group and language
    private void MethodRequestText(string textId, string groupId, int languageId)
    {
        AddMessage("Requesting text id '" + textId + "' in group '" + groupId + "' for language id " + languageId + " from Transfluent Backend");

        TransfluentMethod textMethod = new TransfluentMethod(TransfluentMethodType.Text, TransfluentMethodMode.GET, null, TransfluentDelegateType.None, null);
        m_transfluentMethods.Add(textMethod);

        textMethod.AddParameter("text_id", textId);
        textMethod.AddParameter("group_id", groupId);
        textMethod.AddParameter("language", languageId);
        textMethod.AddParameter("token", m_transfluentToken);
        textMethod.SendTo("https://transfluent.com/v2/text/");
    }

    // Use this to request a text in a certain group and language
    private void MethodRequestTexts(string groupId, int languageId)
    {
        AddMessage("Requesting texts in group '" + groupId + "' for language id " + languageId + " from Transfluent Backend");

        TransfluentMethod textsMethod = new TransfluentMethod(TransfluentMethodType.Texts, TransfluentMethodMode.GET, null, TransfluentDelegateType.None, null);
        m_transfluentMethods.Add(textsMethod);

        textsMethod.AddParameter("group_id", groupId);
        textsMethod.AddParameter("language", languageId);
        textsMethod.AddParameter("token", m_transfluentToken);
        textsMethod.SendTo("https://transfluent.com/v2/texts/");
    }

    // Use this to send all texts in the chosen source language to the Transfluent API Backend
    private void MethodSendTexts(TransfluentDelegate onSuccessDelegate, TransfluentDelegateType delegateType, object[] delegateData)
    {
        string[] textIds;
        string[] texts;

        GetTextsOfLanguage(m_translationSourceLanguageId, out textIds, out texts);
            
        if (texts.Length > 0)
        {
            MethodSendTexts(UnityEditor.PlayerSettings.productName, m_translationSourceLanguageId, textIds, texts, m_textsInvalidateTranslations, onSuccessDelegate, delegateType, delegateData);
        }
    }

    // Use this to send a group of texts in a specified language to the Transfluent API Backend
    private void MethodSendTexts(string groupId, int languageId, string[] textIds, string[] texts, bool invalidateTranslations, TransfluentDelegate onSuccessDelegate, TransfluentDelegateType delegateType, object[] delegateData)
    {
        if (textIds.Length != texts.Length)
        {
            AddError("Text Id and Text array length mismatch: " + textIds.Length + " / " + texts.Length);
            return;
        }

        TransfluentMethod textsMethod = new TransfluentMethod(TransfluentMethodType.Texts, TransfluentMethodMode.POST, onSuccessDelegate, delegateType, delegateData);
        m_transfluentMethods.Add(textsMethod);
        textsMethod.AddParameter("group_id", groupId);
        textsMethod.AddParameter("language", languageId);
        textsMethod.AddParameterGroup("texts", textIds, texts);
        textsMethod.AddParameter("invalidate_translations", (invalidateTranslations ? 1 : 0));

        textsMethod.AddParameter("token", m_transfluentToken);
        textsMethod.SendTo("https://transfluent.com/v2/texts/");

        AddMessage("Sent " + textIds.Length + " text entries from group " + groupId + " in language id " + languageId + " to Transfluent Backend");
    }

    // Use this to request a word count for estimating translation costs
    private void MethodRequestTextWordCount(string textId, string groupId, int languageId, string text)
    {
        TransfluentMethod textWordCountMethod = new TransfluentMethod(TransfluentMethodType.TextWordCount, TransfluentMethodMode.GET, null, TransfluentDelegateType.None, null);
        textWordCountMethod.AddParameter("text_id", textId);
        textWordCountMethod.AddParameter("group_id", groupId);
        textWordCountMethod.AddParameter("language", languageId);
        textWordCountMethod.AddParameter("text", text);
        textWordCountMethod.AddParameter("token", m_transfluentToken);

        textWordCountMethod.SendTo("https://transfluent.com/v2/text/word/count/");
    }

    // Use this to request an authentication token from the Transfluent API Backend
    private TransfluentMethod MethodAuthenticate(string userName, string password)
    {
        TransfluentMethod authMethod = new TransfluentMethod(TransfluentMethodType.Authenticate, TransfluentMethodMode.GET, null, TransfluentDelegateType.None, null);
        m_transfluentMethods.Add(authMethod);
        authMethod.AddParameter("email", userName);
        authMethod.AddParameter("password", password);
        authMethod.SendTo("https://transfluent.com/v2/authenticate/");

        AddMessage("Authenticating with Transfluent Backend");

        return authMethod;
    }

    // Use this to register with the Transfluent API Backend
    private void MethodRegister(string emailAddress)
    {
        TransfluentMethod registerMethod = new TransfluentMethod(TransfluentMethodType.Register, TransfluentMethodMode.GET, null, TransfluentDelegateType.None, null);
        m_transfluentMethods.Add(registerMethod);
        registerMethod.SendTo("http://transfluent.com/v2/create/account/?terms=ok&email=" + emailAddress);
    }

    // Use this to request a translation for all currently selected texts and target languages
    private void MethodOrderTranslations(int sourceLanguage, string comment, int level)
    {
        Debug.LogWarning("TODO: add feature for selecting translator level");

        // Make an array of languages we want to translate to
        List<int> languages = new List<int>();
        foreach (TransfluentLanguage language in m_selectedLanguages.Values)
            languages.Add(language.m_id);

        List<string> textIds = new List<string>();
        foreach (TransfluentText text in m_selectedTexts.Values)
            textIds.Add(text.m_id);

        MethodOrderTranslations(UnityEditor.PlayerSettings.productName, sourceLanguage, languages.ToArray(), textIds.ToArray(), comment, level);
    }

    // Use this to request a translation for all specified texts and target languages
    private void MethodOrderTranslations(string groupId, int sourceLanguageId, int[] targetLanguageIds, string[] textIds, string comment, int level)
    {
        if (targetLanguageIds.Length <= 0)
        {
            Debug.LogError("Can't order translation: no target languages provided");
            return;
        }

        if (textIds.Length <= 0)
        {
            Debug.LogError("Can't order translation: no texts provided");
            return;
        }

        m_dataChanged = true;
        AddMessage("Sending Selected " + textIds.Length + " Texts to be translated to " + targetLanguageIds.Length + " languages to Transfluent Backend API");

        string entryId;
        DateTime now = DateTime.Now;
        foreach (string textId in textIds)
        {
            foreach (int targetLanguageId in targetLanguageIds)
            {
                TransfluentTranslationEntry entry;
                entryId = groupId + "_" + textId + "_" + targetLanguageId;
                if (!m_translations.TryGetValue(entryId, out entry))
                {
                    entry = new TransfluentTranslationEntry(textId, groupId, sourceLanguageId, targetLanguageId);
                    m_translations.Add(entryId, entry);
                }
                else
                {
                    entry.ordered = now;
                    entry.lastChecked = now;
                }
            }
        }

        TransfluentMethod translationMethod = new TransfluentMethod(TransfluentMethodType.TextsTranslate, TransfluentMethodMode.GET, null, TransfluentDelegateType.None, null);
        m_transfluentMethods.Add(translationMethod);

        translationMethod.AddParameter("group_id", groupId);
        translationMethod.AddParameter("source_language", sourceLanguageId);

        foreach (int langId in targetLanguageIds)
            translationMethod.AddParameter("target_languages[]", langId);

        foreach (string textId in textIds)
            translationMethod.AddParameter("texts[][id]", textId);

        if (comment.Length > 0)
            translationMethod.AddParameter("comment", comment);

        translationMethod.AddParameter("level", level);

        translationMethod.AddParameter("token", m_transfluentToken);

        translationMethod.SendTo("https://transfluent.com/v2/texts/translate/");
    }

    // Use this to combine a text update and a translation request for all currently selected texts and target languages
    private void CombinedSendTranslateTexts()
    {
        string error = "";

        string groupId;
        List<int> langIds = new List<int>();
        List<string> textIds = new List<string>();
        List<string> texts = new List<string>();
        List<string> emptyStringsDetected = new List<string>();

        // Check if the source language exists in the languages list
        TransfluentLanguage sourceLanguage;
        if (m_transfluentLanguages.TryGetValue(m_translationSourceLanguageId, out sourceLanguage))
        {
            TransfluentTextItem item;
            foreach (TransfluentText text in m_selectedTexts.Values)
            {
                // Make sure a text item exists in the source language
                if (text.TryGetText(sourceLanguage.m_code, out item))
                {
                    // Check for empty text string in source language
                    if ((item.m_text == null || item.m_text.Length == 0))
                        emptyStringsDetected.Add(text.m_id);
                    else
                    {
                        textIds.Add(text.m_id);
                        texts.Add(item.m_text);
                    }
                }
                else
                {
                    if (error.Length > 0)
                        error += "\n";
                    else
                        error = "Some texts have no entries in source language";

                    error += sourceLanguage.m_code;
                }
            }

            if (emptyStringsDetected.Count > 0)
            {
                if (error.Length > 0)
                    error += "\n";

                error += "There are some text entries with empty strings in the source language, please make sure there's something to translate in each selected text.";

                foreach (string id in emptyStringsDetected)
                    error += "\n" + id;
            }
        }
        else
            error = "Source language Id=" + m_translationSourceLanguageId + " was not found in languages list";

        if (error.Length > 0)
        {
            EditorUtility.DisplayDialog("Ordering translations failed", error, "OK");
            AddError(error);
        }
        else
        {
            // Get all target languages
            foreach (int langId in m_selectedLanguages.Keys)
                langIds.Add(langId);

            groupId = UnityEditor.PlayerSettings.productName;

            // Create the delegate data so we can later order translations for the texts
            object[] delegateData = { groupId, sourceLanguage.m_id, langIds.ToArray(), textIds.ToArray(), m_translationOrderComment.Replace("\n", " "), m_translatorLevel };

            MethodSendTexts(groupId, sourceLanguage.m_id, textIds.ToArray(), texts.ToArray(), m_textsInvalidateTranslations, new TransfluentDelegate(DelegateMethodOrderTranslation), TransfluentDelegateType.FollowUpTranslateTexts, delegateData);

            // Clear the comment field
            m_translationOrderComment = "";
        }
    }

    // This will be called after calling CombinedSendTranslateTexts() and receiving a response for the text send part of it
    // Orders translations for the specified selection of texts and languages
    private void DelegateMethodOrderTranslation(TransfluentDelegateType type, object[] data)
    {
        string groupId, orderComment;
        string[] textIds;

        int sourceLangId, level;
        int[] targetLangIds;

        if (data == null)
        {
            AddError("Error with delegate method: " + type + ": Data is null!");
            return;
        }

        if (data.Length == 6 &&
            data[0] is string &&
            data[1] is int &&
            data[2] is int[] &&
            data[3] is string[] &&
            data[4] is string &&
            data[5] is int)
        {
            groupId = (string)data[0];
            sourceLangId = (int)data[1];
            targetLangIds = (int[])data[2];
            textIds = (string[])data[3];
            orderComment = (string)data[4];
            level = (int)data[5];
        }
        else
        {
            Debug.LogError("Error with delegate method: " + type + ": Data is malformed!");
            return;
        }

        MethodOrderTranslations(groupId, sourceLangId, targetLangIds, textIds, orderComment, level);
    }

    // Use this to request the status of translation for a specified text and target language
    private void MethodRequestTextStatus(string textId, string groupId, int languageId)
    {
        TransfluentMethod statusMethod = new TransfluentMethod(TransfluentMethodType.TextStatus, TransfluentMethodMode.GET, null, TransfluentDelegateType.None, null);
        m_transfluentMethods.Add(statusMethod);

        statusMethod.AddParameter("text_id", textId);
        statusMethod.AddParameter("group_id", groupId);
        statusMethod.AddParameter("language", languageId);
        statusMethod.AddParameter("token", m_transfluentToken);

        statusMethod.SendTo("https://transfluent.com/v2/text/status/");
    }
    
    // This parses the JSON response code from the Transfluent API Backend and sends it for further analysis
    private void ParseTransfluentJSON(TransfluentMethod method)
    {
        IDictionary dict = method.ParseJSON();

        if (dict == null)
        {
            Debug.LogError("Null dictionary parsed from JSON message! Text:\n" + method.response);
            return;
        }

        // Display results to the user
        string message = "Response received for TransfluentMethod " + method.m_type.ToString() + " " + method.m_mode.ToString();
        message += "\nRequest: " + method.request;
        message += "\nResponse: " + dict.Count + " entries:";
        message += "\n{";

        int entryN = 0;
        string parsedResponse = "";
        foreach (DictionaryEntry entry in dict)
        {
            if (entryN > 0)
                parsedResponse += "\n";

            parsedResponse += DebugParseTransfluentJSON(entryN, 0, entry.Key.ToString(), null, entry.Value);
            entryN++;
        }

        method.SetParsedResponse(parsedResponse);

        message += "\n" + parsedResponse + "\n}";

        Debug.Log(message);

        string status = dict["status"] as string;
        if (status == "ERROR")
        {
            ParseError(method, dict);
        }
        if (status == "OK")
        {
            // Handle the message
            switch (method.m_type)
            {
                case TransfluentMethodType.Authenticate:
                    ParseAuthenticate(method, dict);
                    break;
                case TransfluentMethodType.Register:
                    ParseRegister(method, dict);
                    break;
                case TransfluentMethodType.Languages:
                    ParseLanguages(method, dict);
                    break;
                case TransfluentMethodType.Text:
                    ParseText(method, dict);
                    break;
                case TransfluentMethodType.Texts:
                    ParseTexts(method, dict);
                    break;
                case TransfluentMethodType.TextsTranslate:
                    ParseTranslate(method, dict);
                    break;
                case TransfluentMethodType.TextStatus:
                    ParseTextStatus(method, dict);
                    break;
                default:
                    AddWarning("Unknown TransfluentMethod type " + method.m_type);
                    break;
            }
        }

        // Repaint the GUI to immediately show the results
        Repaint();
    }

    // This will print out the JSON response from the Transfluent API Backend to the editor console in an easily readable format
    private string DebugParseTransfluentJSON(int entryIndex, int entryDepth, string entryKey, string parentKey, object entry)
    {
        string message = "";
        string inset = "";
        for (int i = 0; i < entryDepth; i++)
            inset += "\t";

        if (entry is Dictionary<string, object>)
        {
            message += inset + (parentKey != null ? parentKey : "Entry") + "[" + entryIndex + "] '" + (entryKey != null ? entryKey : "NULL") + "' = Dictionary";
            message += "\n" + inset + "{";

            Dictionary<string, object> valuedict = (Dictionary<string, object>)entry;

            int entryN = 0;
            foreach (string key in valuedict.Keys)
            {
                message += "\n" + DebugParseTransfluentJSON(entryN, entryDepth + 1, key, entryKey, valuedict[key]);

                entryN++;
            }

            message += "\n" + inset + "}";
        }
        else if (entry is List<object>)
        {
            message += inset + (parentKey != null ? parentKey : "Entry") + "[" + entryIndex + "] '" + (entryKey != null ? entryKey : "NULL") + "' = List";
            message += "\n" + inset + "{";

            List<object> list = (List<object>)entry;

            int entryN = 0;
            foreach (object listEntry in list)
            {
                message += "\n" + DebugParseTransfluentJSON(entryN, entryDepth + 1, null, entryKey, listEntry);
                
                entryN++;
            }

            message += "\n" + inset + "}";
        }
        else
            message += inset + (parentKey != null ? parentKey : "Entry") + "[" + entryIndex + "] '" + (entryKey != null ? entryKey : "NULL") + "' = " + (entry != null ? "'" + entry.ToString() + "' of type " + entry.GetType() : "'NULL'");

        return message;
    }

    // This parses the JSON response error from the Transfluent API Backend
    private void ParseError(TransfluentMethod method, IDictionary dict)
    {
        Dictionary<string, object> error = (Dictionary<string, object>)dict["error"];

        string type = "";
        string message = "";

        object value;
        if (error.TryGetValue("type", out value))
        {
            if (value is string)
                type = value as string;
        }
        if (error.TryGetValue("message", out value))
        {
            if (value is string)
                message = value as string;
        }

        method.SetBackendErrorType(type);
        method.SetErrorMessage(message);
        AddError("Error with TransfluentMethod " + method.m_type + "!\n" + type + ": " + message);
    }

    // This parses the JSON response for the authentication request
    private void ParseAuthenticate(TransfluentMethod method, IDictionary dict)
    {
        Dictionary<string, object> response = (Dictionary<string, object>)dict["response"];

        object value;
        if (response.TryGetValue("token", out value))
        {
            if (value is string)
            {
                string token = value as string;
                EditorPrefs.SetString(m_transfluentTokenKey, EncryptString(token));
                m_transfluentToken = token;
                AddMessage("Set Transfluent API token to EditorPreferences.\nToken: '" + token + "'");

                SetMode(Mode.Selection);
            }
            else
                AddError("Token value is not of type string!");
        }
        else
            AddError("Couldn't find the token in response!");
    }

    // This parses the JSON response for the registering request
    private void ParseRegister(TransfluentMethod method, IDictionary dict)
    {
        Debug.LogWarning("TODO: Should we expect a response from the server after registering?");
    }

    // This parses the JSON response for the request for a list of all available languages
    private void ParseLanguages(TransfluentMethod method, IDictionary dict)
    {
        m_dataChanged = true;

        if (m_transfluentLanguages == null)
            m_transfluentLanguages = new Dictionary<int, TransfluentLanguage>();

        List<object> response = (List<object>)dict["response"];

        int langN = 0;
        string message;

        bool success = true;
        foreach (Dictionary<string, object> langEntry in response)
        {
            foreach (KeyValuePair<string, object> langMemberPair in langEntry)
            {
                Dictionary<string, object> langMembers = langMemberPair.Value as Dictionary<string, object>;

                object langName, langCode, langID;

                success = true;
                if (!langMembers.TryGetValue("name", out langName))
                    success = false;
                if (!langMembers.TryGetValue("code", out langCode))
                    success = false;
                if (!langMembers.TryGetValue("id", out langID))
                    success = false;

                if (success)
                {
                    string languageName = langName.ToString();
                    string languageCode = langCode.ToString();
                    int languageId = int.Parse(langID.ToString());
                    AddTransfluentLanguage(new TransfluentLanguage(languageName, languageCode, languageId));
                }
                else
                {
                    message = "ERROR: Could not parse language members from entry " + langN + "!";
                    break;
                }
            }

            if (!success)
                break;

            langN++;
        }

        if (success)
        {
            message = "List of languages received: " + langN + " entries";
            AddMessage(message);

            TransfluentLanguage[] languages = new TransfluentLanguage[m_transfluentLanguages.Count];
            m_transfluentLanguages.Values.CopyTo(languages, 0);
        }

        m_languagesRequest = null;
    }

    // This parses the JSON response for sending a text item to Transfluent API Backend
    private void ParseText(TransfluentMethod method, IDictionary dict)
    {
        if (method.m_mode == TransfluentMethodMode.GET)
        {
            string textId, groupId, text;
            int languageId = 0;
            TransfluentText textEntry = null;
            TransfluentTextItem itemEntry = null;
            TransfluentLanguage languageEntry = null;

            if (TryGetValue<string>(dict, "Text", "response", out text))
            {
                if (method.TryGetTextParameter("text_id", out textId) &&
                    method.TryGetTextParameter("group_id", out groupId) &&
                    method.TryGetValueParameter("language", out languageId))
                {
                    if (m_transfluentLanguages.TryGetValue(languageId, out languageEntry))
                    {
                        if (m_texts.TryGetValue(textId, out textEntry))
                        {
                            TransfluentTextItem item;
                            for (int i = 0; i < textEntry.m_texts.Count; i++)
                            {
                                item = textEntry.m_texts[i];

                                int transfluentLangId;
                                if (m_languageIdsByCode.TryGetValue(item.m_languageCode, out transfluentLangId))
                                {
                                    if (transfluentLangId == languageId)
                                    {
                                        itemEntry = item;
                                        break;
                                    }
                                }
                                else
                                    AddError("Could Not find a TransfluentLanguage by the code " + item.m_languageCode);
                            }

                            if (itemEntry == null)
                            {
                                m_dataChanged = true;
                                AddMessage("New text item: groupId = '" + groupId + "', textId = '" + textId + "', langId = '" + languageId + "', text = '" + text + "'");
                                itemEntry = new TransfluentTextItem(languageEntry.m_code, text);
                                textEntry.m_texts.Add(itemEntry);
                            }
                            else if (text != itemEntry.m_text)
                            {
                                m_dataChanged = true;
                                AddMessage("Text updated: groupId = '" + groupId + "', textId = '" + textId + "', langId = '" + languageId + "', text = '" + text + "'");
                                itemEntry.m_text = text;
                            }
                        }
                        else
                            AddError("Uknown text id " + textId);
                    }
                    else
                        AddError("Unknown language id " + languageId);
                }
                else
                    AddError("Unknown Text entry reads '" + text + "'");
            }
            else
                AddError("Unable to parse Text entry response");
        }
        if (method.m_mode == TransfluentMethodMode.POST)
        {
            Debug.Log("TODO: implement single text item POST");
        }
    }

    // This parses the JSON response for sending a selection of text items to Transfluent API Backend
    private void ParseTexts(TransfluentMethod method, IDictionary dict)
    {
        Dictionary<string, object> response = (Dictionary<string, object>)dict["response"];

        if (method.m_mode == TransfluentMethodMode.GET)
        {
            Dictionary<string, object> langDict;
            string textId, groupId, text;
            long longLanguageId = 0L;
            int languageId = 0;

            bool success = true;
            foreach (string key in response.Keys)
            {
                Dictionary<string, object> entry = response[key] as Dictionary<string, object>;

                if (entry == null)
                {
                    AddError("Entry '" + key + "' in response Dictionary was null or not of type Dictionary<string, object>");
                    continue;
                }

                success &= TryGetValue<string>(entry, key, "text_id", out textId);
                success &= TryGetValue<string>(entry, key, "group_id", out groupId);
                success &= TryGetValue<string>(entry, key, "text", out text);

                success &= TryGetValue<Dictionary<string, object>>(entry, key, "language", out langDict);
                if (langDict != null)
                {
                    success &= TryGetValue<long>(langDict, "language", "id", out longLanguageId);
                    languageId = (int)longLanguageId;
                }

                if (success)
                {
                    TransfluentText textEntry = null;
                    TransfluentTextItem itemEntry = null;
                    TransfluentLanguage languageEntry = null;

                    if (!m_transfluentLanguages.TryGetValue(languageId, out languageEntry))
                    {
                        AddError("Unknown language id " + languageId);
                        continue;
                    }

                    if (m_texts.TryGetValue(textId, out textEntry))
                    {
                        TransfluentTextItem item;
                        for (int i = 0; i < textEntry.m_texts.Count; i++)
                        {
                            item = textEntry.m_texts[i];

                            int transfluentLangId;
                            if (m_languageIdsByCode.TryGetValue(item.m_languageCode, out transfluentLangId))
                            {
                                if (transfluentLangId == languageId)
                                {
                                    itemEntry = item;
                                    break;
                                }
                            }
                            else
                                AddError("Could Not find a TransfluentLanguage by the code " + item.m_languageCode);
                        }
                    }

                    if (textEntry == null)
                    {
                        m_dataChanged = true;
                        AddMessage("New text entry: groupId = '" + groupId + "', textId = '" + textId);
                        textEntry = new TransfluentText(textId, groupId);
                        m_texts.Add(textId, textEntry);
                    }

                    if (itemEntry == null)
                    {
                        m_dataChanged = true;
                        AddMessage("New text item: groupId = '" + groupId + "', textId = '" + textId + "', langId = '" + languageId + "', text = '" + text + "'");
                        itemEntry = new TransfluentTextItem(languageEntry.m_code, text);
                        textEntry.m_texts.Add(itemEntry);

                        // Add empty entries to other languages
                        TransfluentLanguage lang;
                        foreach (int langId in m_languages)
                        {
                            if (langId == languageEntry.m_id)
                                continue;

                            if (m_transfluentLanguages.TryGetValue(langId, out lang))
                                textEntry.m_texts.Add(new TransfluentTextItem(lang.m_code, ""));
                            else
                                AddError("Language with id " + langId + " was not found!");
                        }
                    }
                    else if (text != itemEntry.m_text)
                    {
                        m_dataChanged = true;
                        AddMessage("Text updated: groupId = '" + groupId + "', textId = '" + textId + "', langId = '" + languageId + "', text = '" + text + "'");
                        itemEntry.m_text = text;
                    }
                }
                else
                    AddError("Unable to parse Texts entry with the key " + key);
            }
        }
        if (method.m_mode == TransfluentMethodMode.POST)
        {
            Debug.LogWarning("TODO: request string is bugged, fix it");
            Debug.LogWarning("TODO: Report sent text stats to user");
        }
    }

    // This parses the JSON response for requesting the status of a translation order previously sent to Transfluent API Backend
    private void ParseTextStatus(TransfluentMethod method, IDictionary dict)
    {
        Dictionary<string, object> response = (Dictionary<string, object>)dict["response"];

        string textId, groupId;
        int languageId;

        bool isTranslated;
        if (TryGetValue<bool>(response, "response", "is_translated", out isTranslated))
        {
            if (method.TryGetTextParameter("text_id", out textId) && 
                method.TryGetTextParameter("group_id", out groupId) &&
                method.TryGetValueParameter("language", out languageId))
            {
                string translationId = groupId + "_" + textId + "_" + languageId;
                TransfluentTranslationEntry entry;
                if (m_translations.TryGetValue(translationId, out entry))
                {
                    if (isTranslated)
                    {
                        m_dataChanged = true;
                        m_translations.Remove(translationId);
                        AddMessage("Translation of text id '" + textId + "' in group '" + groupId + "' to language " + languageId + " is done");
                        MethodRequestText(textId, groupId, languageId);
                    }
                    else
                    {
                        m_dataChanged = true;
                        AddMessage("Translation of text id '" + textId + "' in group '" + groupId + "' to language " + languageId + " is not yet done");
                        entry.lastChecked = DateTime.Now;
                    }
                }
                else
                {
                    if (isTranslated)
                    {
                        AddWarning("No translation order logged for text id " + textId + " in group " + groupId + " to language " + languageId + " but it is done");
                        MethodRequestText(textId, groupId, languageId);
                    }
                    else
                    {
                        m_dataChanged = true;
                        AddWarning("No translation order logged for text id " + textId + " in group " + groupId + " to language " + languageId + ", marking it up");
                        TransfluentTranslationEntry newEntry = new TransfluentTranslationEntry(textId, groupId, 0, languageId);
                        m_translations.Add(translationId, newEntry);
                    }
                }
            }
            else
                AddError("Unknown translation status is " + isTranslated);
        }
        else
            AddError("Unable to parse method TextStatus");
    }

    // This parses the JSON response for sending a translation request of a set of text items to Transfluent API Backend
    private void ParseTranslate(TransfluentMethod method, IDictionary dict)
    {
        string status = dict["status"] as string;
        if (status != "OK")
        {
            AddError("Error with TransfluentMethod Translate!");
            return;
        }

        Dictionary<string, object> response = (Dictionary<string, object>)dict["response"];

        bool success = true;
        long wordCount = -1L;
        long orderedWordCount = -1L;
        object word_count, ordered_word_count;

        success &= response.TryGetValue("word_count", out word_count);
        success &= SafeCast<long>(word_count, "word_count", out wordCount);

        success &= response.TryGetValue("ordered_word_count", out ordered_word_count);
        success &= SafeCast<long>(ordered_word_count, "ordered_word_count", out orderedWordCount);

        foreach (TransfluentMethod.Parameter param in method.GetParameters())
            Debug.Log("Param: " + param.ToString());

        string[] textIds;
        if (!method.TryGetTextParameters("texts[][id]", out textIds))
            AddError("Couldn't find text ids in request!");

        int[] langIds;
        if (!method.TryGetValueParameters("target_languages[]", out langIds))
            AddError("Couldn't find target languages in request!");

        if (success)
        {
            if (wordCount > 0L)
            {
                if (orderedWordCount > 0L)
                    AddMessage("Final ordered word count is " + orderedWordCount);
                else if (orderedWordCount == 0)
                {
                    AddMessage("No changes detected in ordered texts, no translations ordered");

                    foreach (int langId in langIds)
                    {
                        if (langId == 500)
                        {
                            Debug.LogWarning("TODO: Transfluent backend doesn't report pseudo language ordered word counts at all. Checking translations for pseudo language automatically.");

                            foreach (string textId in textIds)
                                MethodRequestTextStatus(textId, UnityEditor.PlayerSettings.productName, 500);
                        }
                    }
                }
            }
            else if (wordCount == 0)
            {
                AddError("No words found in the texts, no translations ordered");

                // Since nothing was ordered, remove existing orders
                foreach (int langId in langIds)
                {
                    foreach (string textId in textIds)
                    {
                        m_translations.Remove(UnityEditor.PlayerSettings.productName + "_" + textId + "_" + langId);
                        Debug.Log("Removed translation for " + UnityEditor.PlayerSettings.productName + "_" + textId + "_" + langId);
                    }
                }
            }
        }
        else
            AddError("Unable to parse method Translate");
    }

    // This attempts to parse a value out of an IDictionary entry in a JSON response
    private bool TryGetValue<T>(IDictionary dictionary, string methodName, string propertyName, out T result)
    {
        if (dictionary != null)
        {
            ICollection keys = dictionary.Keys;
            foreach (string key in keys)
            {
                if (key == propertyName)
                    return SafeCast<T>(dictionary[propertyName], propertyName, out result);
            }

            AddError("Property key '" + propertyName + "' does not exist in response of " + methodName);
        }

        result = default(T);
        return false;
    }

    // This attempts to parse a value out of a dictionary entry in a JSON response
    private bool TryGetValue<T>(Dictionary<string, object> dictionary, string dictionaryName, string propertyName, out T result)
    {
        if (dictionary != null)
        {
            object property;
            if (dictionary.TryGetValue(propertyName, out property))
            {
                if (property != null)
                    return SafeCast<T>(property, propertyName, out result);
                
                result = default(T);
                return true;
            }

            AddError("Property key '" + propertyName + "' does not exist in Dictionary named " + dictionaryName);
        }

        result = default(T);
        return false;
    }

    // Use this to safely cast an object property into the specified type
    private bool SafeCast<T>(object property, string name, out T result)
    {
        if (property != null)
        {
            if (property is T)
            {
                result = (T)property;
                return true;
            }
            
            AddError("Property '" + name + "' is not of type " + typeof(T) + ", value = " + property.ToString());
        }
        else if (typeof(T).IsClass)
        {
            result = default(T);
            return true;
        }

        AddError("Property '" + name + "' is NULL");

        result = default(T);
        return false;
    }

    // Displays a load file dialog to find a TransfluentData asset from project
    private void LoadData()
    {
        if (m_dataChanged)
        {
            PromptSave("Loading data from project but there are unsaved changes");
        }

        string path = EditorUtility.OpenFilePanel("Locate a TransfluentData asset", Application.dataPath, "asset");
        if (path.Length > 0)
        {
            LoadData(path);
        }
    }

    // Loads a TransfluentData asset file at specified path, or initializes an empty one if file wasn't found
    private void LoadData(string filePath)
    {
        AddMessage("Loading data from: " + filePath);

        TransfluentData data = null;

        if (filePath.StartsWith(Application.dataPath))
        {
            string projectPath = filePath.Remove(0, Application.dataPath.LastIndexOf('/') + 1);
            data = AssetDatabase.LoadMainAssetAtPath(projectPath) as TransfluentData;

            if (data == null)
                AddError("Couldn't load object at path " + projectPath + "\nFull path: " + filePath);
        }
        else
            AddError("Selected asset doesn't belong to current project, please check your selection.\nSelected path: " + filePath + "\nProject path: " + Application.dataPath);

        InitData(data);
    }

    // Prompt to save data before losing it
    private void PromptSave(string message)
    {
        if (EditorUtility.DisplayDialog("TransfluentUtility: Save changes?", message + " All changes will be lost if you click \"NO\"!", "YES", "NO"))
        {
            SaveData();
        }
    }

    // Displays a save file dialog pointed within the project folders
    private void SaveData()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Transfluent Data", "TransfluentData", "asset", "message!");
        if (path.Length > 0)
        {
            SaveData(path);
        }
    }

    // Creates a data file out of the current texts and languages into the specified path
    private void SaveData(string filePath)
    {
        string error = "";

        TransfluentData data = ScriptableObject.CreateInstance<TransfluentData>();

        data.m_sourceLanguageId = m_translationSourceLanguageId;

        if (m_transfluentLanguages != null)
            data.m_languages = new List<TransfluentLanguage>(m_transfluentLanguages.Values);

        // Add all texts
        if (m_texts != null)
        {
            data.m_textGroup = new TransfluentTextGroup(UnityEditor.PlayerSettings.productName);

            foreach (TransfluentText text in m_texts.Values)
            {
                // First handle text changes
                Dictionary<string, string> itemChanges;

                //List<string> entryList = new List<string>();
                string textId, langCode, newText;

                if (m_textItemChanges.TryGetValue(text.m_id, out itemChanges))
                {
                    textId = text.m_id;
                    foreach (TransfluentTextItem item in text.m_texts)
                    {
                        langCode = item.m_languageCode;
                        if (itemChanges.TryGetValue(langCode, out newText))
                        {
                            AddMessage("Text item text change: '" + item.m_text + "' -> '" + itemChanges[langCode] + "' in text entry '" + textId + "', language " + langCode);
                            item.m_text = itemChanges[langCode];
                            itemChanges.Remove(langCode);
                        }
                    }

                    // Check every change was handled properly
                    if (itemChanges.Count == 0)
                        m_textItemChanges.Remove(textId);
                    else
                    {
                        error += "All changes were not saved for text entry id:'" + textId + "'\nChanges not saved:";
                        foreach (string key in itemChanges.Keys)
                            error += "\nLangCode='" + key + "' Text='" + itemChanges[key] + "'";
                    }
                }

                // Then add the text to the asset
                data.m_textGroup.m_texts.Add(text);
            }
        }

        if (m_translations != null)
        {
            data.m_translations = new List<TransfluentTranslationEntry>();

            foreach (TransfluentTranslationEntry entry in m_translations.Values)
            {
                data.m_translations.Add(TransfluentTranslationEntry.Clone(entry));
            }
        }

        if (error.Length > 0)
        {
            AddError(error);
            if (!EditorUtility.DisplayDialog("Error when saving asset", "Errors were encountered while saving asset:\n" + error, "Save", "Cancel"))
                return;
        }

        AssetDatabase.CreateAsset(data, filePath);
        AddMessage("Transfluent Data saved to " + filePath);

        // Save the file path in registry, note that we need to remove a duplicate "Assets/" from the database asset path first
        string saveFilePath = Path.Combine(Application.dataPath, filePath.Remove(0, 7));
        EditorPrefs.SetString(m_previousFilePathKey, saveFilePath);

        m_currentDataObjectPath = saveFilePath;

        m_dataChanged = false;
    }

    // Simple encryption for login information
    private string EncryptString(string value)
    {
        //Debug.Log("Encrypting a value of " + value.Length + " chars:\n" + value);

        byte[] bytes = System.Text.Encoding.Unicode.GetBytes(value);

        string byteValues = "";
        foreach (byte b in bytes)
        {
            if (byteValues.Length > 0)
                byteValues += ", ";
            byteValues += b;
        }

        //Debug.Log("Value as bytes:\n" + byteValues);

        int shift = 123;
        byte[] encryptedBytes = Crypto(bytes, shift);

        byteValues = "";
        foreach (byte b in encryptedBytes)
        {
            if (byteValues.Length > 0)
                byteValues += ", ";
            byteValues += b;
        }

        //Debug.Log("Encrypted value to " + encryptedBytes.Length + " bytes:\n" + byteValues);

        string encrypted = System.Text.Encoding.Unicode.GetString(encryptedBytes);

        //Debug.Log("Encrypted value as string:\n" + encrypted);

        return encrypted;
    }

    // Simple decryption for login information
    private string DecryptString(string value)
    {
        //Debug.Log("Decrypting a value of " + value.Length + " chars:\n" + value);

        byte[] encryptedBytes = System.Text.Encoding.Unicode.GetBytes(value);

        string byteValues = "";
        foreach (byte b in encryptedBytes)
        {
            if (byteValues.Length > 0)
                byteValues += ", ";
            byteValues += b;
        }

        //Debug.Log("Decrypting a value of " + encryptedBytes.Length + " bytes:\n" + byteValues);

        int shift = -123;
        byte[] bytes = Crypto(encryptedBytes, shift);

        byteValues = "";
        foreach (byte b in bytes)
        {
            if (byteValues.Length > 0)
                byteValues += ", ";
            byteValues += b;
        }

        //Debug.Log("Value as bytes:\n" + byteValues);

        string decrypted = System.Text.Encoding.Unicode.GetString(bytes);
        //Debug.Log("Decrypted value to " + decrypted.Length + " chars:\n'" + decrypted + "'");

        return decrypted;
    }

    // Simple rotation cryptography
    private byte[] Crypto(byte[] value, int cipher)
    {
        byte[] bytes = new byte[value.Length];

        int newValue;
        for (int i = 0; i < value.Length; i++)
        {
            newValue = value[i] + cipher;

            if (newValue > byte.MaxValue)
                newValue -= byte.MaxValue;
            else if (newValue < byte.MinValue)
                newValue += byte.MaxValue;

            bytes[i] = (byte)newValue;
        }

        return bytes;
    }

    public static TransfluentUtility GetInstance()
    {
        return s_instance;
    }
}