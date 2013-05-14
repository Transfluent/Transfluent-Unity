using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

public class TransfluentUtility : EditorWindow
{
    public class Message
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

    private enum Mode { Selection, Translations, Settings, Methods, Log, Debug };

    private static TransfluentUtility s_instance;

    private Mode m_mode;

    private const string m_transfluentTokenKey = "TransfluentAPIToken";
    private string m_transfluentToken;

    private const string m_userEmailKey = "TransfluentUserEmail";
    private string m_userEmail;

    private const string m_userPasswordKey = "TransfluentUserPassword";
    private string m_userPassword;

    private const string m_translationCostEstimateCurrencySymbolKey = "TransfluentTranslationCostEstimateCurrencySymbol";
    private string m_translationCostEstimateCurrencySymbol;

    private const string m_translationCostEstimatePerWordKey = "TransfluentTranslationCostEstimatePerWord";
    private float m_translationCostEstimatePerWord;

    //private TransfluentMethod m_newTransfluentMethod;
    private string[] m_newTransfluentMethodNames = new string[]
    {
        "Texts",
        "TextTranslate",
        "TextStatus"
    };
    private int[] m_newTransfluentMethodTypes = new int[]
    {
        (int)TransfluentMethodType.Texts,
        (int)TransfluentMethodType.TextsTranslate,
        (int)TransfluentMethodType.TextStatus
    };

    private List<TransfluentMethod> m_transfluentMethods;

    private List<Message> m_log;

    private Dictionary<int, TransfluentLanguage> m_transfluentLanguages;
    private Dictionary<string, int> m_languageIdsByCode;

    private List<int> m_languages;
    //private Dictionary<string, TransfluentText> m_texts;
    private Dictionary<string, Dictionary<string, TransfluentText>> m_textGroups;
    private Dictionary<string, TransfluentTranslationEntry> m_translations;

    private Dictionary<int, TransfluentLanguage> m_selectedLanguages;
    private List<string> m_selectedGroups;
    private Dictionary<string, Dictionary<string, TransfluentText>> m_selectedTexts;

    private Dictionary<string, GUIStyle> m_styles;
    private Dictionary<string, GUILayoutOption[]> m_glos;

    // Normal view
    private Vector2 m_languageScrollPos;
    private Vector2 m_groupScrollPos;
    private Vector2 m_textScrollPos;
    private Vector2 m_textDetailScrollPos;

    // Methods view
    private Vector2 m_methodScrollPos;
    private Vector2 m_methodDetailScrollPos;

    // Settings view
    private Vector2 m_transfluentLanguageScrollPos;

    // Translations view
    private Vector2 m_translationsScrollPos;
    private Vector2 m_translationDetailsPos;

    // Log view
    private Vector2 m_logScrollPos;

    // Debug view
    private Vector2 m_debugScrollPos;

    //private bool m_foldLanguages = false;
    //private bool m_foldGroups = false;
    //private Dictionary<string, bool> m_foldedGroups;
    //private bool m_foldTexts = false;

    private string m_currentDataObjectPath = "";

    private string m_focusedGroupId = "";
    private string m_focusedTextId = "";
    private string m_focusedTranslationId = "";
    private int m_focusedMethod = -1;

    private bool m_loginNeeded;
    private bool m_dataChanged;

    private bool m_textsInvalidateTranslations = true;
    private TransfluentMethodType m_newTransfluentMethodType;
    private TransfluentMethodMode m_newTransfluentMethodMode;
    private int m_translationSourceLanguageId = 1;
    private string m_translationOrderComment = "";

    private string m_newItemId = "";
    private string m_newItemText = "";
    private string m_newTextId = "";
    private string m_newGroupId = "";

    //private bool m_clearSearchString = false;
    private string m_searchString = "";
    private string[] m_searchWords;
    private List<string> m_searchResultIds;

    [MenuItem("Tools/Transfluent Utility")]
    public static TransfluentUtility Open()
    {
        return EditorWindow.GetWindow<TransfluentUtility>("Transfluent");
    }

    public void Init()
    {
        s_instance = this;

        m_log = new List<Message>();

        AddMessage("Initializing...");

        wantsMouseMove = true;

        if (!EditorPrefs.HasKey(m_userEmailKey))
        {
            EditorPrefs.SetString(m_userEmailKey, "");
            m_mode = Mode.Settings;
            m_loginNeeded = true;
        }
        else
        {
            m_mode = Mode.Selection;
            m_loginNeeded = false;
        }

        m_transfluentToken = EditorPrefs.GetString(m_transfluentTokenKey);
        m_userEmail = EditorPrefs.GetString(m_userEmailKey);
        m_userPassword = EditorPrefs.GetString(m_userPasswordKey);

        if (EditorPrefs.HasKey(m_translationCostEstimateCurrencySymbolKey))
            m_translationCostEstimateCurrencySymbol = EditorPrefs.GetString(m_translationCostEstimateCurrencySymbolKey);
        else
        {
            m_translationCostEstimateCurrencySymbol = "EUR";//System.Globalization.RegionInfo.CurrentRegion.ISOCurrencySymbol; // ISOCurrencySymbol is broken, it gives "US Dollar" when it should be "USD"
            EditorPrefs.SetString(m_translationCostEstimateCurrencySymbolKey, m_translationCostEstimateCurrencySymbol);
        }

        if (EditorPrefs.HasKey(m_translationCostEstimatePerWordKey))
            m_translationCostEstimatePerWord = EditorPrefs.GetFloat(m_translationCostEstimatePerWordKey);
        else
        {
            m_translationCostEstimatePerWord = 0f;
            EditorPrefs.SetFloat(m_translationCostEstimatePerWordKey, m_translationCostEstimatePerWord);
        }

        m_transfluentMethods = new List<TransfluentMethod>();

        m_searchWords = new string[0];

        m_selectedLanguages = new Dictionary<int, TransfluentLanguage>();
        m_selectedGroups = new List<string>();
        m_selectedTexts = new Dictionary<string, Dictionary<string, TransfluentText>>();

        InitStyles();

        InitData(Selection.activeObject as TransfluentData);

        AddMessage("TransfluentUtility initialized");
    }

    private void InitStyles()
    {
        m_styles = new Dictionary<string, GUIStyle>();

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

        GUIStyle detailItemTextEntry = new GUIStyle(GUI.skin.label);
        detailItemTextEntry.normal.textColor = Color.red;
        m_styles.Add("DetailItemLabelTextEmpty", detailItemTextEntry);

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

        GUILayoutOption[] transfluentLanguagePaneGLOs = new GUILayoutOption[] { GUILayout.Width(320f) };
        m_glos.Add("TransfluentLanguagePane", transfluentLanguagePaneGLOs);

        GUILayoutOption[] detailPaneGLOs = new GUILayoutOption[] { GUILayout.ExpandWidth(true) };
        m_glos.Add("DetailPane", detailPaneGLOs);

        GUILayoutOption[] detailItemGLOs = new GUILayoutOption[] { GUILayout.ExpandWidth(true) };
        m_glos.Add("DetailItem", detailItemGLOs);

        GUILayoutOption[] detailItemLabelGLOs = new GUILayoutOption[] { GUILayout.Width(60f) };
        m_glos.Add("DetailItemLabel", detailItemLabelGLOs);

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

    public void InitData()
    {
        m_currentDataObjectPath = "";
        TransfluentData data = null;
        InitData(data);
    }

    private void InitData(string path)
    {
        UnityEngine.Object obj = AssetDatabase.LoadMainAssetAtPath(path);

        if (obj != null && obj is TransfluentData)
            InitData((TransfluentData)obj);
        else
            Debug.LogError("Couldn't load object at path " + path);
    }

    private void InitData(TransfluentData data)
    {
        m_dataChanged = false;

        //m_languageIdsByCode = new Dictionary<string, int>();
        m_transfluentLanguages = new Dictionary<int, TransfluentLanguage>();
        m_languageIdsByCode = new Dictionary<string, int>();

        m_languages = new List<int>();
        m_textGroups = new Dictionary<string, Dictionary<string, TransfluentText>>();
        m_translations = new Dictionary<string, TransfluentTranslationEntry>();

        if (data != null)
        {
            m_currentDataObjectPath = AssetDatabase.GetAssetPath(data);
            AddMessage("Reading data from local file: " + m_currentDataObjectPath);

            m_translationSourceLanguageId = data.m_sourceLanguageId;

            foreach (TransfluentLanguage lang in data.m_languages)
                AddTransfluentLanguage(TransfluentLanguage.Clone(lang));

            Dictionary<string, TransfluentText> textGroup;
            foreach (TransfluentTextGroup group in data.m_textGroups)
            {
                if (!m_textGroups.TryGetValue(group.m_id, out textGroup))
                {
                    textGroup = new Dictionary<string, TransfluentText>();
                    m_textGroups.Add(group.m_id, textGroup);
                    //m_foldedGroups.Add(group.m_id, false);
                }

                foreach (TransfluentText text in group.m_texts)
                {
                    if (text.m_groupId != group.m_id)
                    {
                        AddMessage("Text " + text.m_id + " had wrong group id: " + text.m_groupId + "\nChanged to: " + group.m_id);
                        text.m_groupId = group.m_id;
                    }

                    AddText(TransfluentText.Clone(text), textGroup);
                }
            }

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
                    AddTextItemsForLanguage(language);
                }
            }
        }
    }

    private void AddMessage(string text)
    {
        m_log.Add(new Message(Message.MessageType.Normal, text));
        Debug.Log("TransfluentUtility: " + text);
    }

    private void AddWarning(string text)
    {
        m_log.Add(new Message(Message.MessageType.Warning, text));
        Debug.LogWarning("TransfluentUtility: " + text);
    }

    private void AddError(string text)
    {
        m_log.Add(new Message(Message.MessageType.Error, text));
        Debug.LogError("TransfluentUtility: " + text);
    }

    void OnGUI()
    {
        if (m_styles == null)
            Init();

        if (m_loginNeeded)
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

            return;
        }

        GUILayout.BeginHorizontal();
        {
            GUILayout.Label("File", GUILayout.ExpandWidth(false));

            if (GUILayout.Button("Load", m_glos["FileButton"]))
                LoadData();

            if (GUILayout.Button("Save", m_glos["FileButton"]))
                SaveData();

            if (GUILayout.Button("Close", m_glos["FileButton"]))
                InitData();

            if (m_currentDataObjectPath.Length > 0)
                GUILayout.Label("Opened: " + m_currentDataObjectPath);
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal(m_styles["ModeRow"], m_glos["ModeRow"]);
        {
            if (GUILayout.Button("Selection", m_styles[m_mode == Mode.Selection ? "ModeButtonActive" : "ModeButton"], m_glos["ModeButton"]))
                m_mode = Mode.Selection;
            if (GUILayout.Button("Translations", m_styles[m_mode == Mode.Translations ? "ModeButtonActive" : "ModeButton"], m_glos["ModeButton"]))
                m_mode = Mode.Translations;
            if (GUILayout.Button("Method Calls", m_styles[m_mode == Mode.Methods ? "ModeButtonActive" : "ModeButton"], m_glos["ModeButton"]))
                m_mode = Mode.Methods;
            if (GUILayout.Button("Settings", m_styles[m_mode == Mode.Settings ? "ModeButtonActive" : "ModeButton"], m_glos["ModeButton"]))
                m_mode = Mode.Settings;
            if (GUILayout.Button("Log", m_styles[m_mode == Mode.Log ? "ModeButtonActive" : "ModeButton"], m_glos["ModeButton"]))
                m_mode = Mode.Log;
            if (GUILayout.Button("Debug", m_styles[m_mode == Mode.Debug ? "ModeButtonActive" : "ModeButton"], m_glos["ModeButton"]))
                m_mode = Mode.Debug;
        }
        GUILayout.EndHorizontal();

        if (m_mode == Mode.Selection)
            OnGUI_Selection();
        if (m_mode == Mode.Translations)
            OnGUI_Translations();
        if (m_mode == Mode.Settings)
            OnGUI_Settings();
        if (m_mode == Mode.Methods)
            OnGUI_Methods();
        if (m_mode == Mode.Log)
            OnGUI_Log();
        if (m_mode == Mode.Debug)
            OnGUI_Debug();

        if (Event.current.type == EventType.MouseMove)
            Repaint();
    }

    private void OnGUI_Debug()
    {
        GUILayout.BeginHorizontal();
        {
			GUILayout.Label("Token", GUILayout.ExpandWidth(false));
			GUILayout.TextArea(m_transfluentToken);
        }
        GUILayout.EndHorizontal();
		
        #region Selections
        GUILayout.BeginHorizontal();
        {
            GUILayout.BeginVertical(m_styles["Pane"], GUILayout.Width(180f));
            {
                GUILayout.Label("Selected Groups");

                m_debugScrollPos = GUILayout.BeginScrollView(m_debugScrollPos);
                {
                    foreach (string groupId in m_selectedGroups)
                    {
                        GUILayout.BeginHorizontal();
                        {
                            GUILayout.Space(8f);
                            GUILayout.Label(groupId);
                        }
                        GUILayout.EndHorizontal();
                    }
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndVertical();

            GUILayout.BeginVertical(m_styles["Pane"]);
            {
                GUILayout.Label("Selected Texts");

                m_debugScrollPos = GUILayout.BeginScrollView(m_debugScrollPos);
                {
                    foreach (string groupId in m_selectedTexts.Keys)
                    {
                        Dictionary<string, TransfluentText> texts = m_selectedTexts[groupId];
                        if (texts.Count > 0)
                        {
                            GUILayout.BeginHorizontal();
                            {
                                GUILayout.Space(8f);
                                GUILayout.Label(groupId);
                            }
                            GUILayout.EndHorizontal();

                            foreach (TransfluentText text in texts.Values)
                            {
                                GUILayout.BeginHorizontal();
                                {
                                    GUILayout.Space(24f);
                                    GUILayout.Label(text.m_id);
                                }
                                GUILayout.EndHorizontal();
                            }
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
        #endregion

        int selectedGroupsCount = m_selectedGroups.Count;

        int selectedTextsCount = 0;
        foreach (Dictionary<string, TransfluentText> group in m_selectedTexts.Values)
            selectedTextsCount += group.Count;

        int selectedLanguageCount = m_selectedLanguages.Count;

        if (GUILayout.Button("Test Transfluent Hello"))
        {
            TransfluentMethod helloMethod = new TransfluentMethod(TransfluentMethodType.Hello, TransfluentMethodMode.GET, null);
            m_transfluentMethods.Add(helloMethod);
            helloMethod.AddParameter("name", "Recoil Data Robot");
            helloMethod.SendTo("https://transfluent.com/v2/hello/");

            AddMessage("Requesting 'Hello' from Transfluent Backend API");
        }

        if (GUILayout.Button("Delete registry data"))
        {
            EditorPrefs.DeleteKey(m_userEmailKey);
            EditorPrefs.DeleteKey(m_userPasswordKey);
            EditorPrefs.DeleteKey(m_transfluentTokenKey);
            EditorPrefs.DeleteKey(m_translationCostEstimateCurrencySymbolKey);
            EditorPrefs.DeleteKey(m_translationCostEstimatePerWordKey);
        }

        /*if (GUILayout.Button("Get Languages"))
        {
            RequestLanguages(null);
        }

        if (GUILayout.Button("Get GameTexts for en-us"))
        {
            RequestTexts("GameText", 148);
        }

        if (GUILayout.Button("Get GameTexts for pseudo"))
        {
            RequestTexts("GameText", 500);
        }*/

        GUI.enabled = selectedGroupsCount > 0 && selectedLanguageCount > 0;

        if (GUILayout.Button("Get Texts in selected " + selectedGroupsCount + " groups and " + selectedLanguageCount + " languages"))
        {
            foreach (string groupId in m_selectedGroups)
            {
                foreach (TransfluentLanguage lang in m_selectedLanguages.Values)
                {
                    MethodRequestTexts(groupId, lang.m_id);
                }
            }
        }

        GUI.enabled = selectedTextsCount > 0 && selectedLanguageCount > 0;

        if (GUILayout.Button("Send selected " + selectedTextsCount + " texts in " + selectedLanguageCount + " languages to Transfluent Backend"))
            MethodSendTexts(null);

        if (GUILayout.Button("Order translations for " + selectedTextsCount + " text entries in " + selectedLanguageCount + " languages"))
        {
            AddMessage("Ordering translations, TODO: make user select source language and write a comment");
            MethodOrderTranslations(148, "Testing", 3);
        }

        if (GUILayout.Button("Get Translation status for selected " + selectedTextsCount + " texts in " + selectedLanguageCount + " languages"))
        {
            foreach (string groupId in m_selectedTexts.Keys)
            {
                foreach (TransfluentText text in m_selectedTexts[groupId].Values)
                {
                    foreach (TransfluentLanguage lang in m_selectedLanguages.Values)
                    {
                        MethodRequestTextStatus(text.m_id, groupId, lang.m_id);
                    }
                }
            }
        }

        if (GUILayout.Button("Get TextWordCount for selected " + selectedTextsCount + " texts in " + selectedLanguageCount + " languages"))
        {
            foreach (string groupId in m_selectedTexts.Keys)
            {
                foreach (TransfluentText text in m_selectedTexts[groupId].Values)
                {
                    foreach (TransfluentTextItem item in text.m_texts)
                    {
                        int languageId;
                        if (m_languageIdsByCode.TryGetValue(item.m_languageCode, out languageId))
                        {
                            if (m_selectedLanguages.ContainsKey(languageId))
                            {
                                MethodRequestTextWordCount(text.m_id, groupId, languageId, item.m_text);
                            }
                        }
                    }
                }
            }
        }

        GUI.enabled = true;
    }

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
                        //GUILayout.Label(msg.m_type.ToString(), style, m_glos["LogEntryType"]);
                        GUILayout.Label(msg.m_text, style);
                    }
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.EndScrollView();
        }
        GUILayout.EndVertical();
    }

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

    private void OnGUI_Selection()
    {
        GUILayout.BeginHorizontal(m_styles["TopRow"], m_glos["TopRow"]);
        {
            DrawGroups();
            DrawTexts();
            DrawLanguages();
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal(m_styles["BottomRow"], m_glos["BottomRow"]);
        {
            DrawDetails();
            DrawNewMethod();
        }
        GUILayout.EndHorizontal();
    }

    private void OnGUI_Settings()
    {
        GUILayout.BeginHorizontal();
        {
            GUILayout.BeginVertical();
            {
                GUILayout.Label("Authentication information");
                DrawAuthenticate();

                GUILayout.Space(8f);

                GUILayout.Label("Transfluent translation cost estimate per word");
                GUILayout.BeginVertical(m_styles["PaneArea"]);
                {
                    string newCostCurrencySymbol = EditorGUILayout.TextField("Currency symbol", m_translationCostEstimateCurrencySymbol);

                    if (newCostCurrencySymbol != m_translationCostEstimateCurrencySymbol)
                    {
                        m_translationCostEstimateCurrencySymbol = newCostCurrencySymbol;
                        EditorPrefs.SetString(m_translationCostEstimateCurrencySymbolKey, m_translationCostEstimateCurrencySymbol);
                        AddMessage("Set Transfluent translation cost estimate currency symbol to EditorPreferences.\nSymbol: " + m_translationCostEstimateCurrencySymbol);
                    }
                
                    float newCostEstimate = EditorGUILayout.FloatField("Cost estimate (" + m_translationCostEstimateCurrencySymbol + ")", m_translationCostEstimatePerWord);

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

            GUILayout.BeginVertical(m_glos["TransfluentLanguagePane"]);
            {
                string[] popupNames = new string[m_transfluentLanguages.Count];
                int[] popupValues = new int[m_transfluentLanguages.Count];

                int i = 0;
                foreach (TransfluentLanguage lang in m_transfluentLanguages.Values)
                {
                    popupNames[i] = lang.m_id + ": [" + lang.m_code + "] " + lang.m_name;
                    popupValues[i] = lang.m_id;
                    i++;
                }

                int newSourceLanguageId = EditorGUILayout.IntPopup("Source Language", m_translationSourceLanguageId, popupNames, popupValues);

                if (newSourceLanguageId != m_translationSourceLanguageId)
                {
                    m_dataChanged = true;
                    Debug.Log("Source language id changed: " + m_translationSourceLanguageId + " -> " + newSourceLanguageId);

                    m_translationSourceLanguageId = newSourceLanguageId;
                }

                GUILayout.Label("All Transfluent languages");

                m_transfluentLanguageScrollPos = GUILayout.BeginScrollView(m_transfluentLanguageScrollPos, m_styles["Pane"]);
                {
                    bool inUse;
                    foreach (TransfluentLanguage lang in m_transfluentLanguages.Values)
                    {
                        inUse = m_languages.Contains(lang.m_id);

                        GUILayout.BeginHorizontal();
                        {
                            GUI.enabled = !inUse;

                            if (GUILayout.Button((inUse ? "Added" : "Add"), GUILayout.Width(50f)))
                            {
                                m_dataChanged = true;

                                AddLanguage(lang);
                                AddTextItemsForLanguage(lang);
                            }

                            GUI.enabled = true;

                            GUILayout.Label(lang.m_id.ToString(), GUILayout.Width(30f));
                            GUILayout.Label(lang.m_code, GUILayout.Width(50f));
                            GUILayout.Label(lang.m_name);
                        }
                        GUILayout.EndHorizontal();
                    }
                }
                GUILayout.EndScrollView();

                if (GUILayout.Button("Get Languages from Transfluent Backend"))
                {
                    MethodRequestLanguages(null);
                }
            }
            GUILayout.EndVertical();
        }
        GUILayout.EndHorizontal();
    }

    private void OnGUI_Translations()
    {
        GUILayout.BeginHorizontal(m_styles["TranslationEntryCategories"]);
        {
            //GUILayout.Label("Ordered On", m_styles["TranslationEntryCategory"], m_glos["TranslationOrdered"]);
            GUILayout.Label("Group Id", m_styles["TranslationEntryCategory"], m_glos["TranslationGroupId"]);
            GUILayout.Label("Text Id", m_styles["TranslationEntryCategory"], m_glos["TranslationTextId"]);
            GUILayout.Label("Languages", m_styles["TranslationEntryCategory"], m_glos["TranslationLanguages"]);

            if (GUILayout.Button("Check all " + m_translations.Count, m_glos["TranslationCheckStatusAll"]))
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
                    sourceLanguageCode = "unknown";

                if (m_transfluentLanguages.TryGetValue(entry.m_targetLanguageId, out targetLanguage))
                    targetLanguageCode = targetLanguage.m_code;
                else
                    targetLanguageCode = "unknown";

                GUILayout.BeginHorizontal(m_styles["TranslationEntry"]);
                {
                    //GUILayout.Label(entry.ordered.ToString("yyyy'-'MM'-'dd HH':'mm':'ss"), m_glos["TranslationOrdered"]);
                    GUILayout.Label(entry.m_groupId, m_glos["TranslationGroupId"]);
                    GUILayout.Label(entry.m_textId, m_glos["TranslationTextId"]);
                    GUILayout.Label(sourceLanguageCode + " [" + entry.m_sourceLanguageId + "] -> " + targetLanguageCode + " [" + entry.m_targetLanguageId + "]", m_glos["TranslationLanguages"]);

                    if (GUILayout.Button("Details", m_glos["TranslationFocus"]))
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

                if (m_textGroups.TryGetValue(entry.m_groupId, out group))
                {
                    if (group.TryGetValue(entry.m_textId, out text))
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
                }

                m_translationDetailsPos = GUILayout.BeginScrollView(m_translationDetailsPos, false, true);
                {
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label("Translation ordered on " + entry.ordered.ToLocalTime().ToString("yyyy'-'MM'-'dd HH':'mm':'ss"));
                        GUILayout.FlexibleSpace();
                        GUILayout.Label("Last checked on " + entry.lastChecked.ToLocalTime().ToString("yyyy'-'MM'-'dd HH':'mm':'ss"));
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal(m_styles["TranslationEntry"]);
                    {
                        GUILayout.Label("Group Id", m_glos["TranslationDetailLabel"]);
                        GUILayout.Label(entry.m_groupId);
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal(m_styles["TranslationEntry"]);
                    {
                        GUILayout.Label("Text Id", m_glos["TranslationDetailLabel"]);
                        GUILayout.Label(entry.m_textId);
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal(m_styles["TranslationEntry"]);
                    {
                        TimeSpan span = DateTime.UtcNow.Subtract(entry.ordered);
                        GUILayout.Label("Time taken", m_glos["TranslationDetailLabel"]);

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
                        GUILayout.Label("Target Language", m_glos["TranslationDetailLabel"]);

                        if (targetLanguage != null)
                            GUILayout.Label(targetLanguage.m_code + " [" + targetLanguage.m_id + "] " + targetLanguage.m_name);
                        else
                            GUILayout.Label("Couldn't find language by id " + entry.m_targetLanguageId);
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal(m_styles["TranslationEntry"]);
                    {
                        GUILayout.Label("Source Language", m_glos["TranslationDetailLabel"]);

                        if (sourceLanguage != null)
                            GUILayout.Label(sourceLanguage.m_code + " [" + sourceLanguage.m_id + "] " + sourceLanguage.m_name);
                        else
                            GUILayout.Label("Couldn't find language by id " + entry.m_sourceLanguageId);
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal(m_styles["TranslationEntry"]);
                    {
                        GUILayout.Label("Source Text", m_glos["TranslationDetailLabel"]);

                        if (textItem != null)
                            GUILayout.Label(textItem.m_text);
                        else if (text != null)
                            GUILayout.Label("Couldn't find text item by text id '" + entry.m_textId + "' in group '" + entry.m_groupId + "' in language '" + entry.m_sourceLanguageId + "'");
                        else if (group != null)
                            GUILayout.Label("Couldn't find text by id '" + entry.m_textId + "' in group '" + entry.m_groupId + "'");
                        else
                            GUILayout.Label("Couldn't find group by id '" + entry.m_groupId + "'");
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndScrollView();

                if (GUILayout.Button("Check for updates"))
                    MethodRequestTextStatus(entry.m_textId, entry.m_groupId, entry.m_targetLanguageId);
            }
            else
                GUILayout.Label("No translation entry found with id '" + m_focusedTranslationId + "'");
        }
        GUILayout.EndVertical();
    }

    private void DrawAuthenticate()
    {
        GUILayout.BeginVertical(m_styles["PaneArea"]);
        {
            string newUserEmail = EditorGUILayout.TextField("Email address", m_userEmail);

            if (newUserEmail != m_userEmail)
            {
                m_userEmail = newUserEmail;
                EditorPrefs.SetString(m_userEmailKey, m_userEmail);
                //AddMessage("Set Transfluent User email address to EditorPreferences.\nAddress: '" + m_userEmail + "'");
            }

            string newUserPassword = EditorGUILayout.PasswordField("Password", m_userPassword);

            if (newUserPassword != m_userPassword)
            {
                m_userPassword = newUserPassword;
                EditorPrefs.SetString(m_userPasswordKey, m_userPassword);
                //AddMessage("Set Transfluent User password to EditorPreferences.\nPassword: '" + m_userPassword + "'");
            }

            if (m_loginNeeded)
            {
                GUI.enabled = (m_userEmail.Length > 0 && m_userPassword.Length > 0);
                if (GUILayout.Button("Authenticate with Transfluent Backend"))
                {
                    MethodAuthenticate(m_userEmail, m_userPassword);

                    if (m_transfluentLanguages.Count == 0)
                        MethodRequestLanguages(null);

                    m_loginNeeded = false;
                }
                GUI.enabled = true;
            }
            else
            {
                if (m_transfluentToken.Length == 0)
                    GUILayout.Label("Not authenticated with Transfluent Backend, Please retrieve a token!");
                else
                    GUILayout.Label("");

                GUI.enabled = (m_userEmail.Length > 0 && m_userPassword.Length > 0);
                if (GUILayout.Button("Authenticate with Transfluent Backend"))
                    MethodAuthenticate(m_userEmail, m_userPassword);
                GUI.enabled = true;
            }
        }
        GUILayout.EndVertical();
    }

    private void DrawDetails()
    {
        GUILayout.BeginVertical(m_styles["PaneArea"], m_glos["DetailPane"]);
        {
            if (m_focusedGroupId.Length > 0)
            {
                //string newTextId = m_focusedTextId;
                //string newGroupId = m_focusedGroupId;
                Dictionary<string, TransfluentText> group;
                if (m_textGroups.TryGetValue(m_focusedGroupId, out group))
                {
                    if (m_focusedTextId.Length > 0)
                    {
                        TransfluentText text;
                        if (group.TryGetValue(m_focusedTextId, out text))
                        {
                            m_textDetailScrollPos = GUILayout.BeginScrollView(m_textDetailScrollPos, false, true);
                            {
                                GUILayout.BeginHorizontal(m_styles["DetailItem"], m_glos["DetailItem"]);
                                {
                                    GUILayout.Label("Text Id", m_glos["DetailItemLabel"]);
                                    m_newTextId = EditorGUILayout.TextField(m_newTextId);

                                    GUI.SetNextControlName("UpdateTextId");
                                    if (GUILayout.Button("Update", m_glos["DetailItemLabel"]))
                                    {
                                        if (m_newTextId.Length > 0 && m_newTextId != text.m_id && !group.ContainsKey(m_newTextId))
                                        {
                                            m_dataChanged = true;
                                            Debug.Log("Text Id change: '" + text.m_id + "' -> '" + m_newTextId + "'");

                                            group.Add(m_newTextId, text);
                                            group.Remove(text.m_id);

                                            text.m_id = m_newTextId;

                                            SetTextFocus(m_newTextId);
                                        }

                                        GUI.FocusControl("UpdateTextId");
                                    }
                                }
                                GUILayout.EndHorizontal();

                                GUILayout.BeginHorizontal(m_styles["DetailItem"], m_glos["DetailItem"]);
                                {
                                    GUILayout.Label("Group Id", m_glos["DetailItemLabel"]);
                                    m_newGroupId = EditorGUILayout.TextField(m_newGroupId);

                                    GUI.SetNextControlName("UpdateTextGroupId");
                                    if (GUILayout.Button("Update", m_glos["DetailItemLabel"]))
                                    {
                                        if (m_newGroupId.Length > 0 && m_newGroupId != text.m_groupId)
                                        {
                                            m_dataChanged = true;
                                            Debug.Log("Text Id '" + text.m_id + "' Group Id change: '" + text.m_groupId + "' -> '" + m_newGroupId + "'");

                                            Dictionary<string, TransfluentText> newGroup;
                                            if (m_textGroups.TryGetValue(m_newGroupId, out newGroup))
                                            {
                                                newGroup.Add(text.m_id, text);
                                            }
                                            else
                                            {
                                                newGroup = new Dictionary<string, TransfluentText>();
                                                newGroup.Add(text.m_id, text);

                                                m_textGroups.Add(m_newGroupId, newGroup);
                                            }

                                            group.Remove(text.m_id);

                                            text.m_groupId = m_newGroupId;

                                            SetGroupFocus(m_newGroupId);
                                        }

                                        GUI.FocusControl("UpdateTextGroupId");
                                    }
                                }
                                GUILayout.EndHorizontal();
                                
                                GUILayout.BeginHorizontal(m_styles["DetailItem"], m_glos["DetailItem"]);
                                {
                                    GUILayout.Label(text.m_texts.Count + " localized entries in this text");
                                }
                                GUILayout.EndHorizontal();

                                GUIStyle itemStyle;
                                string itemId, oldItemText, newItemText;
                                foreach (TransfluentTextItem item in text.m_texts)
                                {
                                    if (item.m_text != null && item.m_text.Length > 0)
                                        itemStyle = m_styles["DetailItemLabel"];
                                    else
                                        itemStyle = m_styles["DetailItemLabelTextEmpty"];
                                    
                                    GUILayout.BeginHorizontal(m_styles["DetailItem"], m_glos["DetailItem"]);
                                    {
                                        GUILayout.Label("[" + item.m_languageCode + "]", itemStyle, m_glos["DetailItemLabel"]);

                                        itemId = text.m_id + item.m_languageCode;

                                        if (itemId == m_newItemId)
                                            oldItemText = m_newItemText;
                                        else
                                            oldItemText = item.m_text;

                                        newItemText = EditorGUILayout.TextField(oldItemText);

                                        if (newItemText != oldItemText)
                                        {
                                            m_newItemId = itemId;
                                            m_newItemText = newItemText;
                                        }

                                        GUI.SetNextControlName("TextItem" + itemId);
                                        if (GUILayout.Button("Update", m_glos["DetailItemLabel"]))
                                        {
                                            if (itemId == m_newItemId)
                                            {
                                                GUI.FocusControl("TextItem" + itemId);

                                                Debug.Log("Text item text change: '" + item.m_text + "' -> '" + newItemText + "'");

                                                m_dataChanged = true;

                                                item.m_text = m_newItemText;
                                            }
                                        }
                                    }
                                    GUILayout.EndHorizontal();
                                }
                            }
                            GUILayout.EndScrollView();
                        }
                        else
                        {
                            GUILayout.Label("Couldn't find text id in group");
                            GUILayout.FlexibleSpace();
                        }
                    }
                    else
                    {
                        GUILayout.BeginHorizontal(m_styles["DetailItem"], m_glos["DetailItem"]);
                        {
                            GUILayout.Label("Group Id", m_glos["DetailItemLabel"]);
                            m_newGroupId = EditorGUILayout.TextField(m_newGroupId);

                            GUI.SetNextControlName("UpdateGroupId");
                            if (GUILayout.Button("Update", m_glos["DetailItemLabel"]))
                            {
                                if (m_newGroupId.Length > 0 && m_newGroupId != m_focusedGroupId && !m_textGroups.ContainsKey(m_newGroupId))
                                {
                                    m_dataChanged = true;
                                    Debug.Log("Group Id change: '" + m_focusedGroupId + "' -> '" + m_newGroupId + "'");

                                    foreach (TransfluentText text in group.Values)
                                        text.m_groupId = m_newGroupId;

                                    m_textGroups.Add(m_newGroupId, group);
                                    m_textGroups.Remove(m_focusedGroupId);

                                    m_focusedGroupId = m_newGroupId;
                                }

                                GUI.FocusControl("UpdateGroupId");
                            }
                        }
                        GUILayout.EndHorizontal();
                        
                        GUILayout.BeginHorizontal(m_styles["DetailItem"], m_glos["DetailItem"]);
                        {
                            GUILayout.Label(group.Count + " text entries in this group");
                        }
                        GUILayout.EndHorizontal();

                        GUILayout.FlexibleSpace();
                    }
                }
                else
                {
                    GUILayout.Label("No group found by this id");
                    GUILayout.FlexibleSpace();
                }
            }
            else
                GUILayout.FlexibleSpace();
        }
        GUILayout.EndVertical();
    }

    private void DrawGroups()
    {
        bool selected, newSelected, isFocused;

        GUILayout.BeginVertical(m_styles["PaneArea"], m_glos["GroupPane"]);
        {
            GUILayout.BeginHorizontal(m_styles["PaneTitle"], m_glos["PaneTitle"]);
            {
                GUILayout.Label("Groups");
            }
            GUILayout.EndHorizontal();

            m_groupScrollPos = GUILayout.BeginScrollView(m_groupScrollPos, false, true, GUI.skin.horizontalScrollbar, GUI.skin.verticalScrollbar, m_styles["Pane"]);
            {
                if (m_textGroups != null && m_textGroups.Count > 0)
                {
                    foreach (string groupId in m_textGroups.Keys)
                    {
                        selected = m_selectedGroups.Contains(groupId);

                        GUILayout.BeginHorizontal();
                        {
                            newSelected = GUILayout.Toggle(selected, "", GUILayout.ExpandWidth(false));

                            if (newSelected != selected)
                            {
                                if (newSelected)
                                {
                                    m_selectedGroups.Add(groupId);
                                    //Debug.Log("Selected group " + groupId);
                                }
                                else
                                {
                                    m_selectedGroups.Remove(groupId);
                                    //Debug.Log("Removed selection of group " + groupId);
                                }
                            }

                            isFocused = (m_focusedGroupId == groupId);
                            if (GUILayout.Button(groupId, m_styles[isFocused ? "FocusedGroup" : "UnfocusedGroup"]))
                            {
                                SetGroupFocus(isFocused ? "" : groupId);
                            }
                        }
                        GUILayout.EndHorizontal();
                    }
                }
            }
            GUILayout.EndScrollView();

            GUI.SetNextControlName("AddGroup");
            if (GUILayout.Button("Add Group"))
            {
                GUI.FocusControl("AddGroup");

                m_dataChanged = true;

                string newGroupId = "New_Group";

                if (m_textGroups.ContainsKey(newGroupId))
                {
                    int i = 1;
                    while (m_textGroups.ContainsKey(newGroupId + "_" + i))
                        i++;

                    newGroupId += "_" + i;
                }

                m_textGroups.Add(newGroupId, new Dictionary<string, TransfluentText>());
                
                m_focusedGroupId = newGroupId;
                m_focusedTextId = "";
                m_newGroupId = newGroupId;

                m_groupScrollPos = new Vector2(0f, float.MaxValue);
            }
        }
        GUILayout.EndVertical();
    }

    private void DrawLanguages()
    {
        bool selected, newSelected;

        GUILayout.BeginVertical(m_styles["PaneArea"], m_glos["LanguagePane"]);
        {
            GUILayout.BeginHorizontal(m_styles["PaneTitle"], m_glos["PaneTitle"]);
            {
                GUILayout.Label("Languages in use");
            }
            GUILayout.EndHorizontal();

            m_languageScrollPos = GUILayout.BeginScrollView(m_languageScrollPos, false, true, GUI.skin.horizontalScrollbar, GUI.skin.verticalScrollbar, m_styles["Pane"]);
            {
                if (m_languages != null && m_languages.Count > 0)
                {
                    foreach (int langId in m_languages)
                    {
                        TransfluentLanguage lang;
                        if (!m_transfluentLanguages.TryGetValue(langId, out lang))
                        {
                            AddError("Couldn't find language by the id " + langId + " in transfluent languages!");
                            continue;
                        }

                        selected = m_selectedLanguages.ContainsKey(lang.m_id);

                        GUILayout.BeginHorizontal();
                        {
                            newSelected = GUILayout.Toggle(selected, "", GUILayout.ExpandWidth(false));

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
                        GUILayout.EndHorizontal();
                    }
                }
            }
            GUILayout.EndScrollView();

            if (GUILayout.Button("Add Languages"))
            {
                m_mode = Mode.Settings;
            }
        }
        GUILayout.EndVertical();
    }

    private void DrawNewMethod()
    {
        int selectedGroupCount = m_selectedGroups.Count;
        int selectedLanguageCount = m_selectedLanguages.Count;

        int selectedTextCount = 0;
        int selectedGroupCountByTexts = 0;
        
        foreach (string groupId in m_selectedTexts.Keys)
        {
            selectedTextCount += m_selectedTexts[groupId].Count;

            if (m_selectedTexts[groupId].Count > 0)
                selectedGroupCountByTexts++;
        }

        GUILayout.BeginVertical(m_styles["PaneArea"], m_glos["MethodPane"]);
        {
            GUILayout.BeginVertical(m_styles["MethodPane"]);
            {
                if (m_transfluentToken.Length == 0)
                {
                    GUILayout.Label("Not authenticated with Transfluent Backend\nPlease refresh token from settings");
                    GUILayout.FlexibleSpace();
                }
                else
                {
                    m_newTransfluentMethodType = (TransfluentMethodType)EditorGUILayout.IntPopup("Select Method", (int)m_newTransfluentMethodType, m_newTransfluentMethodNames, m_newTransfluentMethodTypes);

                    if (m_newTransfluentMethodType == TransfluentMethodType.Texts)
                    {
                        m_newTransfluentMethodMode = (TransfluentMethodMode)EditorGUILayout.EnumPopup("Select Mode", m_newTransfluentMethodMode);

                        if (m_newTransfluentMethodMode == TransfluentMethodMode.POST)
                        {
                            m_textsInvalidateTranslations = EditorGUILayout.Toggle("Invalidate Translations", m_textsInvalidateTranslations);

                            GUILayout.FlexibleSpace();

                            GUILayout.Label(selectedTextCount + " texts selected in " + selectedGroupCountByTexts + " groups");
                            GUILayout.Label(selectedLanguageCount + " languages selected");

                            GUILayout.FlexibleSpace();

                            GUI.enabled = (selectedTextCount > 0);

                            if (GUILayout.Button("Send"))
                                MethodSendTexts(null);

                            GUI.enabled = true;
                        }
                        else
                        {
                            GUILayout.FlexibleSpace();

                            GUILayout.Label(selectedGroupCount + " groups selected");
                            GUILayout.Label(selectedLanguageCount + " languages selected");

                            GUILayout.FlexibleSpace();

                            GUI.enabled = (selectedGroupCount > 0 && selectedLanguageCount > 0);

                            if (GUILayout.Button("Send"))
                            {
                                foreach (string groupId in m_selectedGroups)
                                {
                                    foreach (TransfluentLanguage lang in m_selectedLanguages.Values)
                                        MethodRequestTexts(groupId, lang.m_id);
                                }
                            }

                            GUI.enabled = true;
                        }
                    }
                    else if (m_newTransfluentMethodType == TransfluentMethodType.TextsTranslate)
                    {
                        string[] popupNames = new string[m_languages.Count];
                        int[] popupValues = new int[m_languages.Count];

                        TransfluentLanguage lang;
                        for (int i = 0; i < m_languages.Count; i++)
                        {
                            if (m_transfluentLanguages.TryGetValue(m_languages[i], out lang))
                            {
                                popupNames[i] = lang.m_id + ": [" + lang.m_code + "] " + lang.m_name;
                                popupValues[i] = lang.m_id;
                            }
                            else
                            {
                                popupNames[i] = "Unknown id " + m_languages[i];
                                popupValues[i] = m_languages[i];
                            }
                        }

                        m_translationSourceLanguageId = EditorGUILayout.IntPopup("Source Language", m_translationSourceLanguageId, popupNames, popupValues);

                        GUILayout.FlexibleSpace();

                        GUILayout.Label(selectedTextCount + " texts selected in " + selectedGroupCountByTexts + " groups");
                        GUILayout.Label(selectedLanguageCount + " languages selected");

                        GUILayout.FlexibleSpace();

                        GUILayout.Label("Comment");
                        m_translationOrderComment = EditorGUILayout.TextArea(m_translationOrderComment, m_styles["MethodTranslateComment"], m_glos["MethodTranslateComment"]);

                        GUILayout.FlexibleSpace();

                        GUI.enabled = (selectedTextCount > 0 && selectedLanguageCount > 0 && m_transfluentLanguages.ContainsKey(m_translationSourceLanguageId));

                        if (GUILayout.Button("Send"))
                        {
                            int sourceWordCount = 0;
                            TransfluentLanguage sourceLang;
                            if (m_transfluentLanguages.TryGetValue(m_translationSourceLanguageId, out sourceLang))
                            {
                                foreach (string groupId in m_selectedTexts.Keys)
                                {
                                    foreach (TransfluentText text in m_selectedTexts[groupId].Values)
                                    {
                                        int sourceLangWordCount = -1;

                                        foreach (TransfluentTextItem item in text.m_texts)
                                        {
                                            int targetLangId;
                                            if (m_languageIdsByCode.TryGetValue(item.m_languageCode, out targetLangId))
                                            {
                                                if (targetLangId == sourceLang.m_id)
                                                    sourceLangWordCount = item.m_text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Length;
                                            }
                                            else
                                                AddWarning("Unknown language code in text item in group[" + groupId + "] text[" + text.m_id + "]: '" + item.m_languageCode + "'");
                                        }

                                        if (sourceLangWordCount >= 0)
                                        {
                                            sourceWordCount += sourceLangWordCount;
                                        }
                                        else
                                            AddError("Couldn't find source language text from text item in group[" + groupId + "] text[" + text.m_id + "]\nUnable to calculate word count for this text!");
                                    }
                                }

                                string title = "Are you sure you want to order translations for " + selectedTextCount + " text" + (selectedTextCount > 1 ? "s" : "") + "?";

                                string description = "Translate " + sourceWordCount + " word" + (sourceWordCount > 1 ? "s" : "") + " into " + m_selectedLanguages.Count + " language" + (m_selectedLanguages.Count > 1 ? "s" : "");
                                if (m_translationCostEstimatePerWord > 0f)
                                {
                                    float costEstimate = m_translationCostEstimatePerWord * sourceWordCount * m_selectedLanguages.Count;
                                    description += "\nCost estimate (" + m_translationCostEstimateCurrencySymbol + "): " + costEstimate;
                                }
                                else
                                    description += "\nCost estimate per word is not defined in the settings!";

                                if (EditorUtility.DisplayDialog(title, description, "YES", "NO"))
                                {
                                    MethodOrderTranslations(m_translationSourceLanguageId, m_translationOrderComment.Replace("\n", " "), 3);
                                    m_translationOrderComment = "";
                                }
                            }
                            else
                                AddError("Unknown source language id: " + m_translationSourceLanguageId);
                        }

                        GUI.enabled = true;
                    }
                    else if (m_newTransfluentMethodType == TransfluentMethodType.TextStatus)
                    {
                        GUILayout.FlexibleSpace();

                        GUILayout.Label(selectedTextCount + " texts selected in " + selectedGroupCountByTexts + " groups");
                        GUILayout.Label(selectedLanguageCount + " languages selected");

                        GUILayout.FlexibleSpace();

                        GUI.enabled = (selectedTextCount > 0 && selectedLanguageCount > 0);

                        if (GUILayout.Button("Send"))
                        {
                            Dictionary<string, TransfluentText> group;
                            foreach (TransfluentLanguage lang in m_selectedLanguages.Values)
                            {
                                if (lang.m_id == m_translationSourceLanguageId)
                                    continue;

                                foreach (string groupId in m_selectedTexts.Keys)
                                {
                                    group = m_selectedTexts[groupId];

                                    foreach (TransfluentText text in group.Values)
                                        MethodRequestTextStatus(text.m_id, groupId, lang.m_id);
                                }
                            }
                        }

                        GUI.enabled = true;
                    }
                    else
                        GUILayout.FlexibleSpace();
                }
            }
            GUILayout.EndVertical();
        }
        GUILayout.EndVertical();
    }

    private void DrawTexts()
    {
        GUILayout.BeginVertical(m_styles["PaneArea"], m_glos["TextPane"]);
        {
            Dictionary<string, TransfluentText> textGroup;
            Dictionary<string, TransfluentText> selectionGroup;
            if (m_focusedGroupId.Length > 0)
            {
                GUILayout.BeginHorizontal(m_styles["PaneTitle"], m_glos["PaneTitle"]);
                {
                    GUILayout.Label("Search", GUILayout.ExpandWidth(false));

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

                if (m_textGroups.TryGetValue(m_focusedGroupId, out textGroup))
                {
                    m_textScrollPos = GUILayout.BeginScrollView(m_textScrollPos, false, true, GUI.skin.horizontalScrollbar, GUI.skin.verticalScrollbar, m_styles["Pane"]);
                    {
                        if (!m_selectedTexts.TryGetValue(m_focusedGroupId, out selectionGroup))
                        {
                            selectionGroup = new Dictionary<string, TransfluentText>();
                            m_selectedTexts.Add(m_focusedGroupId, selectionGroup);
                        }

                        if (m_searchString.Length > 0 && m_searchResultIds != null)
                        {
                            if (m_searchResultIds.Count > 0)
                            {
                                foreach (string textId in m_searchResultIds)
                                {
                                    TransfluentText text;
                                    if (textGroup.TryGetValue(textId, out text))
                                        DrawText(text, selectionGroup);
                                    else
                                        GUILayout.Label("Error, couldn't find text by the id of " + textId);
                                }
                            }
                            else
                                GUILayout.Label("No hits found in search");
                        }
                        else
                        {
                            foreach (TransfluentText text in textGroup.Values)
                            {
                                if (text != null)
                                    DrawText(text, selectionGroup);
                                else
                                    GUILayout.Label("Error, text was null");
                            }
                        }
                    }
                    GUILayout.EndScrollView();

                    GUI.SetNextControlName("AddText");
                    if (GUILayout.Button("Add Text"))
                    {
                        GUI.FocusControl("AddText");

                        m_dataChanged = true;

                        string newTextId = "NEW_TEXT";

                        if (textGroup.ContainsKey(newTextId))
                        {
                            int i = 1;
                            while (textGroup.ContainsKey(newTextId + "_" + i))
                                i++;

                            newTextId += "_" + i;
                        }

                        TransfluentText text = new TransfluentText(newTextId, m_focusedGroupId);

                        foreach (int langId in m_languages)
                        {
                            TransfluentLanguage lang;
                            if (m_transfluentLanguages.TryGetValue(langId, out lang))
                            {
                                text.m_texts.Add(new TransfluentTextItem(lang.m_code, ""));
                            }
                        }

                        textGroup.Add(text.m_id, text);

                        m_newTextId = text.m_id;
                        m_focusedTextId = text.m_id;

                        m_textScrollPos = new Vector2(0f, float.MaxValue);
                    }
                }
            }
            else
            {
                GUILayout.FlexibleSpace();
            }
        }
        GUILayout.EndVertical();
    }

    private void DrawText(TransfluentText text, Dictionary<string, TransfluentText> selectionGroup)
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
            if (GUILayout.Button(text.m_id, m_styles[isFocused ? "FocusedText" : "UnfocusedText"]))
            {
                SetTextFocus((isFocused ? "" : text.m_id));
            }
        }
        GUILayout.EndHorizontal();
    }

    void OnDisable()
    {
        if (m_dataChanged)
        {
            PromptSave("Window is being disabled or closed but there are unsaved changes.");

            if (!Application.unityVersion.StartsWith("4"))
                Debug.LogWarning("Unity 3.5 (maybe older ones too) has a bug causing the EditorWindow to sometimes call the GUI after closing a dialog, even while closing the window.\nThis can cause an CheckOnGUI() error below. No need to worry though, all data was handled properly before the error.");
        }
    }

    void Update()
    {
        if (s_instance != this)
            return;

        if (m_transfluentMethods != null)
        {
            for (int i = m_transfluentMethods.Count - 1; i >= 0; i--)
            {
                TransfluentMethod method = m_transfluentMethods[i];
                if (!method.isConsumed && method.isDone)
                {
                    if (method.error == null)
                    {
                        AddMessage("Method " + method.m_type.ToString() + " completed");

                        ParseTransfluentJSON(method);

                        method.Consume();
                        //m_transfluentMethods.RemoveAt(i);
                    }
                    else
                        AddError("Transfluent." + method.GetType() + " error: \"" + method.error + "\"");
                    
                    method.Consume();
                }
            }
        }

        if (m_searchString.Length > 0 && m_searchResultIds == null)
        {
            //Debug.Log("Running a search!");

            m_searchResultIds = new List<string>();

            m_searchWords = m_searchString.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            //Debug.Log("Search string '" + m_searchString + "' results in " + m_searchWords.Length + " words");

            bool foundMatch, foundWord;
            string searchWordLowercase;
            Dictionary<string, TransfluentText> textGroup;
            if (m_textGroups.TryGetValue(m_focusedGroupId, out textGroup))
            {
                foreach (TransfluentText text in textGroup.Values)
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
            }

            //Debug.Log("Search done, found " + m_searchResultIds.Count + " matches!");
        }
    }

    private void SetGroupFocus(string groupId)
    {
        m_focusedGroupId = groupId;
        m_newGroupId = groupId;
    }

    private void SetTextFocus(string textId)
    {
        m_focusedTextId = textId;
        m_newTextId = textId;
    }

    private void MethodRequestLanguages(TransfluentDelegate onSuccessDelegate)
    {
        TransfluentMethod languagesMethod = new TransfluentMethod(TransfluentMethodType.Languages, TransfluentMethodMode.GET, onSuccessDelegate);
        m_transfluentMethods.Add(languagesMethod);
        languagesMethod.SendTo("https://transfluent.com/v2/languages/");

        AddMessage("Requesting Languages from Transfluent Backend API");
    }

    public void GetLanguages(TransfluentDelegate delegateMethod)
    {
        if (m_transfluentLanguages != null && m_transfluentLanguages.Count > 0)
        {
            TransfluentLanguage[] array = new TransfluentLanguage[m_transfluentLanguages.Count];
            m_transfluentLanguages.Values.CopyTo(array, 0);
            delegateMethod(TransfluentDelegateType.GetLanguages, array);
        }
        else
            MethodRequestLanguages(delegateMethod);
    }

    public void AddLanguage(TransfluentLanguage language)
    {
        AddMessage("Adding Text Language (" + language.m_code + ") ID[" + language.m_id + "]: '" + language.m_name + "'");

        if (!m_languages.Contains(language.m_id))
            m_languages.Add(language.m_id);
        else
            AddWarning("Text Language already exists (" + language.m_code + ") ID[" + language.m_id + "]: '" + language.m_name + "'");
    }

    private void AddTransfluentLanguage(TransfluentLanguage language)
    {
        AddMessage("Adding Transfluent Language (" + language.m_code + ") ID[" + language.m_id + "]: '" + language.m_name + "'");

        if (!m_transfluentLanguages.ContainsKey(language.m_id))
        {
            if (!m_transfluentLanguages.ContainsKey(language.m_id))
                m_transfluentLanguages.Add(language.m_id, language);
            else
                AddError("Language id " + language.m_id + " defined more than once! Duplicate: (" + language.m_code + ") ID[" + language.m_id + "]: '" + language.m_name + "'");

            if (!m_languageIdsByCode.ContainsKey(language.m_code))
                m_languageIdsByCode.Add(language.m_code, language.m_id);
            else
                AddError("Language code " + language.m_code + " defined more than once! Duplicate: (" + language.m_code + ") ID[" + language.m_id + "]: '" + language.m_name + "'");
        }
        else
            AddWarning("Transfluent Language id already exists! Duplicate: (" + language.m_code + ") ID[" + language.m_id + "]: '" + language.m_name + "'");
    }

    private void MethodRequestText(string textId, string groupId, int languageId)
    {
        AddMessage("Requesting text id '" + textId + "' in group '" + groupId + "' for language id " + languageId + " from Transfluent Backend");

        TransfluentMethod textMethod = new TransfluentMethod(TransfluentMethodType.Text, TransfluentMethodMode.GET, null);
        m_transfluentMethods.Add(textMethod);

        textMethod.AddParameter("text_id", textId);
        textMethod.AddParameter("group_id", groupId);
        textMethod.AddParameter("language", languageId);
        textMethod.AddParameter("token", m_transfluentToken);
        textMethod.SendTo("https://transfluent.com/v2/text/");
    }

    private void MethodRequestTexts(string groupId, int languageId)
    {
        AddMessage("Requesting texts in group '" + groupId + "' for language id " + languageId + " from Transfluent Backend");

        TransfluentMethod textsMethod = new TransfluentMethod(TransfluentMethodType.Texts, TransfluentMethodMode.GET, null);
        m_transfluentMethods.Add(textsMethod);

        textsMethod.AddParameter("group_id", groupId);
        textsMethod.AddParameter("language", languageId);
        textsMethod.AddParameter("token", m_transfluentToken);
        textsMethod.SendTo("https://transfluent.com/v2/texts/");
    }

    private void MethodSendTexts(TransfluentDelegate onSuccessDelegate)
    {
        int languageCount = m_selectedLanguages.Count;
        int groupCount = m_selectedTexts.Count;

        AddMessage("Sending selected text entries from " + groupCount + " groups in " + languageCount + " languages to Transfluent Backend");

        bool found;
        List<string> texts = new List<string>();
        List<string> textIds = new List<string>();
        foreach (TransfluentLanguage language in m_selectedLanguages.Values)
        {
            foreach (string groupId in m_selectedTexts.Keys)
            {
                Dictionary<string, TransfluentText> group;
                if (!m_selectedTexts.TryGetValue(groupId, out group))
                {
                    AddError("Couldn't find group by the id of '" + groupId + "' in the selection even though the group was listed");
                    continue;
                }
                
                texts.Clear();
                textIds.Clear();

                foreach (TransfluentText text in group.Values)
                {
                    found = false;

                    foreach (TransfluentTextItem item in text.m_texts)
                    {
                        if (item.m_languageCode == language.m_code)
                        {
                            found = true;
                            texts.Add(item.m_text);
                            textIds.Add(text.m_id);
                            break;
                        }
                    }

                    if (!found)
                        AddError("Couldn't find language " + language.m_code + " in text " + text.m_id);
                }

                if (texts.Count > 0)
                {
                    MethodSendTexts(groupId, language.m_id, textIds.ToArray(), texts.ToArray(), m_textsInvalidateTranslations, onSuccessDelegate);
                }
            }
        }
    }

    public void MethodSendTexts(string groupId, int languageId, string[] textIds, string[] texts, bool invalidateTranslations, TransfluentDelegate onSuccessDelegate)
    {
        if (textIds.Length != texts.Length)
        {
            AddError("Text Id and Text array length mismatch: " + textIds.Length + " / " + texts.Length);
            return;
        }

        TransfluentMethod textsMethod = new TransfluentMethod(TransfluentMethodType.Texts, TransfluentMethodMode.POST, onSuccessDelegate);
        m_transfluentMethods.Add(textsMethod);
        textsMethod.AddParameter("group_id", groupId);
        textsMethod.AddParameter("language", languageId);

        for (int i = 0; i < texts.Length; i++)//TransfluentTextItem textItem in text.m_texts)
            textsMethod.AddParameter("texts[" + textIds[i] + "]", texts[i]);

        textsMethod.AddParameter("invalidate_translations", (invalidateTranslations ? 1 : 0));

        textsMethod.AddParameter("token", m_transfluentToken);
        textsMethod.SendTo("https://transfluent.com/v2/texts/");

        AddMessage("Sent " + textIds.Length + " text entries from group " + groupId + " in language id " + languageId + " to Transfluent Backend");
    }

    private void MethodRequestTextWordCount(string textId, string groupId, int languageId, string text)
    {
        TransfluentMethod textWordCountMethod = new TransfluentMethod(TransfluentMethodType.TextWordCount, TransfluentMethodMode.GET, null);
        textWordCountMethod.AddParameter("text_id", textId);
        textWordCountMethod.AddParameter("group_id", groupId);
        textWordCountMethod.AddParameter("language", languageId);
        textWordCountMethod.AddParameter("text", text);
        textWordCountMethod.AddParameter("token", m_transfluentToken);

        textWordCountMethod.SendTo("https://transfluent.com/v2/text/word/count/");
    }

    public void AddText(TransfluentText text)
    {
        Dictionary<string, TransfluentText> textGroup;
        if (!m_textGroups.TryGetValue(text.m_groupId, out textGroup))
        {
            textGroup = new Dictionary<string, TransfluentText>();
            m_textGroups.Add(text.m_groupId, textGroup);
            //m_foldedGroups.Add(text.m_groupId, false);
        }

        AddText(text, textGroup);
    }

    public void AddText(TransfluentText text, Dictionary<string, TransfluentText> group)
    {
        if (group == null)
        {
            AddError("Unable to add text " + text.m_id + ", group can not be NULL!");
            return;
        }

        int langId;
        foreach (TransfluentTextItem item in text.m_texts)
        {
            if (m_languageIdsByCode.TryGetValue(item.m_languageCode, out langId))
            {
                if (!m_languages.Contains(langId))
                {
                    AddLanguage(m_transfluentLanguages[langId]);
                }
            }
        }

        group.Add(text.m_id, text);

        if (!m_selectedTexts.ContainsKey(text.m_groupId))
            m_selectedTexts.Add(text.m_groupId, new Dictionary<string, TransfluentText>());
    }

    private void AddTextItemsForLanguage(TransfluentLanguage language)
    {
        AddMessage("Checking language id " + language.m_id + " [" + language.m_code + "] " + language.m_name + " is used in all texts");

        int added = 0;
        bool found;
        foreach (string groupId in m_textGroups.Keys)
        {
            foreach (TransfluentText text in m_textGroups[groupId].Values)
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
                }
            }
        }

        if (added > 0)
            AddMessage("Added text items for language '" + language.m_code + "' to " + added + " texts");
    }

    private void MethodAuthenticate(string userName, string password)
    {
        TransfluentMethod authMethod = new TransfluentMethod(TransfluentMethodType.Authenticate, TransfluentMethodMode.GET, null);
        m_transfluentMethods.Add(authMethod);
        authMethod.AddParameter("email", userName);
        authMethod.AddParameter("password", password);
        authMethod.SendTo("https://transfluent.com/v2/authenticate/");

        AddMessage("Authenticating with Transfluent Backend");
    }

    private void MethodOrderTranslations(int sourceLanguage, string comment, int level)
    {
        Debug.LogWarning("TODO: add feature for selecting translator level");

        // Make an array of languages we want to translate to
        List<int> languages = new List<int>();
        foreach (TransfluentLanguage language in m_selectedLanguages.Values)
            languages.Add(language.m_id);

        // Create a list of texts by group id
        //Dictionary<string, List<TransfluentText>> groups = GetGroupSortedTexts();

        List<string> textIds;
        foreach (string groupId in m_selectedTexts.Keys)
        {
            textIds = new List<string>();

            foreach (TransfluentText text in m_selectedTexts[groupId].Values)
                textIds.Add(text.m_id);

            MethodOrderTranslations(groupId, sourceLanguage, languages.ToArray(), textIds.ToArray(), comment, level);
        }
    }

    public void MethodOrderTranslations(string groupId, int sourceLanguageId, int[] targetLanguageIds, string[] textIds, string comment, int level)
    {
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

        TransfluentMethod translationMethod = new TransfluentMethod(TransfluentMethodType.TextsTranslate, TransfluentMethodMode.GET, null);
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

    public void MethodRequestTextStatus(string textId, string groupId, int languageId)
    {
        TransfluentMethod statusMethod = new TransfluentMethod(TransfluentMethodType.TextStatus, TransfluentMethodMode.GET, null);
        m_transfluentMethods.Add(statusMethod);

        statusMethod.AddParameter("text_id", textId);
        statusMethod.AddParameter("group_id", groupId);
        statusMethod.AddParameter("language", languageId);
        statusMethod.AddParameter("token", m_transfluentToken);

        statusMethod.SendTo("https://transfluent.com/v2/text/status/");
    }

    /*private Dictionary<string, List<TransfluentText>> GetGroupSortedTexts()
    {
        Dictionary<string, List<TransfluentText>> groups = new Dictionary<string, List<TransfluentText>>();

        List<TransfluentText> list;
        foreach (TransfluentText text in m_selectedTexts.Values)
        {
            if (!groups.TryGetValue(text.m_groupId, out list))
            {
                list = new List<TransfluentText>();
                groups.Add(text.m_groupId, list);
            }

            list.Add(text);
        }

        return groups;
    }*/

    private void ParseTransfluentJSON(TransfluentMethod method)
    {
        IDictionary dict = method.ParseJSON();

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

        // Handle the message
        if (method.m_type == TransfluentMethodType.Authenticate)
        {
            ParseAuthenticate(method, dict);
        }
        else if (method.m_type == TransfluentMethodType.Languages)
        {
            ParseLanguages(method, dict);
        }
        else if (method.m_type == TransfluentMethodType.Text)
        {
            ParseText(method, dict);
        }
        else if (method.m_type == TransfluentMethodType.Texts)
        {
            ParseTexts(method, dict);
        }
        else if (method.m_type == TransfluentMethodType.TextsTranslate)
        {
            ParseTranslate(method, dict);
        }
        else if (method.m_type == TransfluentMethodType.TextStatus)
        {
            ParseTextStatus(method, dict);
        }
        else
            AddWarning("Unknown TransfluentMethod type " + method.m_type);

        Repaint();
    }

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

    private void ParseAuthenticate(TransfluentMethod method, IDictionary dict)
    {
        string status = dict["status"] as string;
        if (status != "OK")
        {
            AddError("Error with TransfluentMethod Authenticate!");
            return;
        }

        Dictionary<string, object> response = (Dictionary<string, object>)dict["response"];
        object value;
        if (response.TryGetValue("token", out value))
        {
            if (value is string)
            {
                string token = value as string;
                EditorPrefs.SetString(m_transfluentTokenKey, token);
                m_transfluentToken = token;
                AddMessage("Set Transfluent API token to EditorPreferences.\nToken: '" + token + "'");
            }
            else
                AddError("Token value is not of type string!");
        }
        else
            AddError("Couldn't find the token in response!");
    }

    private void ParseLanguages(TransfluentMethod method, IDictionary dict)
    {
        string status = dict["status"] as string;
        if (status != "OK")
        {
            AddError("Error with TransfluentMethod Languages!");
            return;
        }

        m_dataChanged = true;
        m_transfluentLanguages = new Dictionary<int, TransfluentLanguage>();
        //m_languagesByCode = new Dictionary<string, TransfluentLanguage>();

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

            /*if (m_localLanguages != null)
            {
                m_languagesLocalToTransfluent = new Dictionary<int,int>();
                m_languagesTransfluentToLocal = new Dictionary<int,int>();

                TransfluentLanguage lang;
                TransfluentLanguage transfluentLang;

                for (int i = 0; i < m_localLanguages.Count; i++)
                {
                    lang = m_localLanguages[i];

                    for (int l = 0; l < m_transfluentLanguages.Count; l++)
                    {
                        transfluentLang = m_transfluentLanguages[l];

                        if (lang.m_code == transfluentLang.m_code)
                        {
                            Debug.Log("Transfluent language [" + transfluentLang.m_id + "] " + transfluentLang.m_name + " (" + transfluentLang.m_code + ")\nmatch Local language [" + lang.m_id + "] " + lang.m_name + " (" + lang.m_code + ")");
                            m_languagesLocalToTransfluent.Add(lang.m_id, transfluentLang.m_id);
                            m_languagesTransfluentToLocal.Add(transfluentLang.m_id, lang.m_id);
                        }
                    }
                }
            }*/

            // Send the delegate call
            TransfluentLanguage[] languages = new TransfluentLanguage[m_transfluentLanguages.Count];
            m_transfluentLanguages.Values.CopyTo(languages, 0);
            method.InvokeDelegate(TransfluentDelegateType.GetLanguages, languages);
        }
    }

    private void ParseText(TransfluentMethod method, IDictionary dict)
    {
        string status = dict["status"] as string;
        if (status != "OK")
        {
            AddError("Error with TransfluentMethod Text!");
            return;
        }

        if (method.m_mode == TransfluentMethodMode.GET)
        {
            string textId, groupId, text;
            int languageId = 0;
            TransfluentText textEntry = null;
            TransfluentTextItem itemEntry = null;
            TransfluentLanguage languageEntry = null;
            Dictionary<string, TransfluentText> group = null;

            if (TryGetValue<string>(dict, "Text", "response", out text))
            {
                if (method.TryGetTextParameter("text_id", out textId) &&
                    method.TryGetTextParameter("group_id", out groupId) &&
                    method.TryGetValueParameter("language", out languageId))
                {
                    if (m_transfluentLanguages.TryGetValue(languageId, out languageEntry))
                    {
                        if (m_textGroups.TryGetValue(groupId, out group))
                        {
                            if (group.TryGetValue(textId, out textEntry))
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
                            AddError("Unknown group id " + groupId);
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

    private void ParseTexts(TransfluentMethod method, IDictionary dict)
    {
        string status = dict["status"] as string;
        if (status != "OK")
        {
            AddError("Error with TransfluentMethod Texts!");
            return;
        }

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
                    Dictionary<string, TransfluentText> group = null;

                    if (!m_transfluentLanguages.TryGetValue(languageId, out languageEntry))
                    {
                        AddError("Unknown language id " + languageId);
                        continue;
                    }

                    if (m_textGroups.TryGetValue(groupId, out group))
                    {
                        if (group.TryGetValue(textId, out textEntry))
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
                    }

                    if (group == null)
                    {
                        m_dataChanged = true;
                        AddMessage("New text group: groupId = '" + groupId);
                        group = new Dictionary<string, TransfluentText>();
                        m_textGroups.Add(groupId, group);
                    }

                    if (textEntry == null)
                    {
                        m_dataChanged = true;
                        AddMessage("New text entry: groupId = '" + groupId + "', textId = '" + textId);
                        textEntry = new TransfluentText(textId, groupId);
                        group.Add(textId, textEntry);
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
                    AddError("Unable to parse Texts entry with the key " + key);
            }
        }
        if (method.m_mode == TransfluentMethodMode.POST)
        {
            Debug.LogWarning("TODO: Report sent text stats to user");
        }
    }

    private void ParseTextStatus(TransfluentMethod method, IDictionary dict)
    {
        string status = dict["status"] as string;
        if (status != "OK")
        {
            AddError("Error with TransfluentMethod TextStatus!");
            return;
        }

        Dictionary<string, object> response = (Dictionary<string, object>)dict["response"];

        string textId, groupId;
        int languageId;
        bool isTranslated;
        if (TryGetValue<bool>(response, "response", "is_translated", out isTranslated))
        {
            /*if (is_translated is string)
                isTranslated = (is_translated.ToString() == "True");
            else
                AddError("Variable is_translated is not a string!\n" + is_translated.GetType() + ".ToString() = " + is_translated.ToString());*/

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

        /*string groupId;
        string[] textIds;
        int[] languageIds;
        int sourceLanguageId;

        if (method.TryGetTextParameter("group_id", out groupId) &&
            method.TryGetValueParameter("source_language", out sourceLanguageId) &&
            method.TryGetValueParameters("target_languages[]", out languageIds) &&
            method.TryGetTextParameters("texts[][id]", out textIds))*/

        if (success)
        {

            if (wordCount > 0L)
            {
                if (orderedWordCount > 0L)
                    AddMessage("Final ordered word count is " + orderedWordCount);
                else if (orderedWordCount == 0)
                    AddMessage("No changes detected in ordered texts, no translations ordered");
            }
            else if (wordCount == 0)
                AddError("No words found in the texts, no translations ordered");
        }
        else
            AddError("Unable to parse method Translate");
    }

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

    private void LoadData()
    {
        if (m_dataChanged)
        {
            PromptSave("Loading data from project but there are unsaved changes");
        }

        string path = EditorUtility.OpenFilePanel("Locate a TransfluentData asset", Application.dataPath, "asset");
        if (path.Length > 0)
            InitData(path.Substring(Application.dataPath.Length - 6));
    }

    private void PromptSave(string message)
    {
        if (EditorUtility.DisplayDialog("TransfluentUtility: Save changes?", message + " All changes will be lost if you click \"NO\"!", "YES", "NO"))
        {
            SaveData();
        }
    }

    private void SaveData()
    {
        TransfluentData data = ScriptableObject.CreateInstance<TransfluentData>();

        data.m_sourceLanguageId = m_translationSourceLanguageId;

        if (m_transfluentLanguages != null)
            data.m_languages = new List<TransfluentLanguage>(m_transfluentLanguages.Values);

        if (m_textGroups != null)
        {
            data.m_textGroups = new List<TransfluentTextGroup>();

            foreach (string groupId in m_textGroups.Keys)
            {
                TransfluentTextGroup group = new TransfluentTextGroup(groupId);

                foreach (TransfluentText text in m_textGroups[groupId].Values)
                {
                    group.m_texts.Add(TransfluentText.Clone(text));
                }

                data.m_textGroups.Add(group);
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

        string path = EditorUtility.SaveFilePanelInProject("Save Transfluent Data", "TransfluentData", "asset", "message!");
        if (path.Length > 0)
        {
            AssetDatabase.CreateAsset(data, path);
            AddMessage("Transfluent Data saved to " + path);

            m_dataChanged = false;
        }
    }

    public static TransfluentUtility GetInstance()
    {
        return s_instance;
    }
}