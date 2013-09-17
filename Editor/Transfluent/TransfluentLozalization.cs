using UnityEngine;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

public class TransfluentLozalization
{
    private int m_languageId;
    private string m_languageCode;
    private string m_languageName;

    private Dictionary<string, string> m_texts;

    public int id
    {
        get { return m_languageId; }
    }
    public string code
    {
        get { return m_languageCode; }
    }
    public string name
    {
        get { return m_languageName; }
    }

    public TransfluentLozalization(string filePath)
    {
        m_languageId = -1;
        m_languageCode = null;
        m_languageName = null;
        m_texts = new Dictionary<string, string>();

        if (filePath != null && filePath.Length > 0)
            ReadXML(filePath);
    }

    private void ReadXML(string filePath)
    {
        TransfluentUtility util = TransfluentUtility.GetInstance();

        //util.AddWarning("Reading " + filePath);

        XmlTextReader reader = new XmlTextReader(filePath);

        while (reader.Read())
        {
            if (reader.IsStartElement())
            {
                if (reader.IsEmptyElement)
                    util.AddMessage("Empty element: " + reader.Name);
                else
                {
                    if (reader.Name.Equals("locale"))
                        ReadLocale(reader);
                    else
                        util.AddMessage("Start of an element: " + reader.Name);
                }
            }
            else
            {
                util.AddMessage("End of an element: " + reader.Name);
            }
        }

        util.AddMessage("Done reading " + filePath + "\nLanguage Id=" + m_languageId + ", Code='" + m_languageCode + "', Name='" + m_languageName + "', Found " + m_texts.Count + " text entries");
    }

    private void ReadLocale(XmlTextReader reader)
    {
        TransfluentUtility util = TransfluentUtility.GetInstance();

        while (reader.Read())
        {
            if (reader.IsStartElement())
            {
                //util.AddMessage("Start locale entry element: " + reader.Name);

                if (reader.Name.Equals("id"))
                {
                    string id = reader.ReadString();
                    if (!int.TryParse(id, out m_languageId))
                        util.AddError("Couldn't parse language id from '" + id + "'");
                }
                if (reader.Name.Equals("code"))
                    m_languageCode = reader.ReadString();
                if (reader.Name.Equals("name"))
                    m_languageName = reader.ReadString();
                if (reader.Name.Equals("texts"))
                    ReadTexts(reader);
            }
            else if (reader.Name.Equals("locale"))
            {
                // Done reading locale
                //util.AddMessage("End of locale");
                break;
            }
            else
                util.AddWarning("Unexpected element closed: " + reader.Name);
        }

        if (m_languageId == -1)
            util.AddError("Couldn't parse language id");
        if (m_languageCode == null)
            util.AddError("Couldn't parse language code");
        if (m_languageName == null)
            util.AddError("Couldn't parse language name");
        if (m_texts.Count == 0)
            util.AddWarning("No texts parsed or found");
    }

    private void ReadTexts(XmlTextReader reader)
    {
        TransfluentUtility util = TransfluentUtility.GetInstance();

        while (reader.Read())
        {
            if (reader.IsStartElement())
            {
                if (reader.Name.Equals("text"))
                {
                    //util.AddMessage("Text entry found");
                    ReadText(reader);
                }
                else
                    util.AddWarning("Unexpected element started: " + reader.Name);
            }
            else if (reader.Name.Equals("texts"))
            {
                // Done reading texts
                //util.AddMessage("End of texts");
                break;
            }
            else
                util.AddWarning("Unexpected element closed: " + reader.Name);
        }
    }

    private void ReadText(XmlTextReader reader)
    {
        TransfluentUtility util = TransfluentUtility.GetInstance();

        string textId = null;
        string textString = null;

        while (reader.Read())
        {
            if (reader.IsStartElement())
            {
                //util.AddMessage("Start text entry element: " + reader.Name);

                if (reader.Name.Equals("textId"))
                    textId = reader.ReadString();
                else if (reader.Name.Equals("textString"))
                    textString = reader.ReadString();
                else
                    util.AddWarning("Unexpected element started: " + reader.Name);
            }
            else if (reader.Name.Equals("text"))
            {
                // Done reading this element
                //util.AddMessage("End of text entry");
                break;
            }
            else
                util.AddWarning("Unexpected element closed: " + reader.Name);
        }

        if (textId != null && textString != null)
        {
            //util.AddMessage("XML: Found text entry: Id='" + textId + "', String='" + textString + "'");
            m_texts.Add(textId, textString);
        }
        else
            util.AddError("XML: Error reading text entry: Id='" + textId + "', String='" + textString + "'");
    }

    public string GetText(string textId)
    {
        string text;
        if (!m_texts.TryGetValue(textId, out text))
            text = "ERROR: Unknown Id '" + textId + "'";

        return text;
    }

    public void SetText(string id, string text)
    {
        m_texts.Add(id, text);
    }
}
