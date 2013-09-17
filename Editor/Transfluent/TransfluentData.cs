using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransfluentData : ScriptableObject
{
    public int m_sourceLanguageId;
    public List<TransfluentLanguage> m_languages;
    //public List<TransfluentTextGroup> m_textGroups;
    public TransfluentTextGroup m_textGroup;
    public List<TransfluentTranslationEntry> m_translations;
}

[System.Serializable]
public class TransfluentLanguage
{
    public string m_code;
    public string m_name;
    public int m_id;

    public TransfluentLanguage(string name, string code, int id)
    {
        m_name = name;
        m_code = code;
        m_id = id;
    }

    public static TransfluentLanguage Clone(TransfluentLanguage original)
    {
        return new TransfluentLanguage(original.m_name, original.m_code, original.m_id);
    }
}

[System.Serializable]
public class TransfluentTextGroup
{
    public string m_id;
    public List<TransfluentText> m_texts;

    public TransfluentTextGroup(string id)
    {
        m_id = id;
        m_texts = new List<TransfluentText>();
    }
}

[System.Serializable]
public class TransfluentText
{
    public string m_id;
    public string m_groupId;
    public List<TransfluentTextItem> m_texts;

    public TransfluentText(string id, string groupId)
    {
        m_id = id;
        m_groupId = groupId;
        m_texts = new List<TransfluentTextItem>();
    }

    /*public TransfluentText(string id, List<TransfluentLanguage> languages, string groupId)
    {
        m_id = id;
        m_groupId = groupId;

        m_texts = new List<TransfluentTextItem>();

        foreach (TransfluentLanguage lang in languages)
            m_texts.Add(new TransfluentTextItem(lang.m_code, ""));
    }*/

    public static TransfluentText Clone(TransfluentText original)
    {
        TransfluentText newText = new TransfluentText(original.m_id, original.m_groupId);

        foreach (TransfluentTextItem item in original.m_texts)
            newText.m_texts.Add(new TransfluentTextItem(item.m_languageCode, item.m_text));

        return newText;
    }

    public bool TryGetText(string languageCode, out TransfluentTextItem text)
    {
        text = null;

        foreach (TransfluentTextItem item in m_texts)
        {
            if (item.m_languageCode == languageCode)
            {
                text = item;
                return true;
            }
        }

        return false;
    }
}

[System.Serializable]
public class TransfluentTextItem
{
    public string m_languageCode;
    public string m_text;

    public TransfluentTextItem(string languageCode, string text)
    {
        m_languageCode = languageCode;
        m_text = text;
    }
}

[System.Serializable]
public class TransfluentTranslationEntry
{
    public string m_textId;
    public string m_groupId;
    public int m_sourceLanguageId;
    public int m_targetLanguageId;
    public TransfluentTranslationDateTime m_ordered;
    public TransfluentTranslationDateTime m_lastChecked;

    public TransfluentTranslationEntry(string textId, string groupId, int sourceLanguageId, int targetLanguageId)
    {
        m_textId = textId;
        m_groupId = groupId;
        m_sourceLanguageId = sourceLanguageId;
        m_targetLanguageId = targetLanguageId;

        DateTime now = DateTime.Now;
        m_ordered = new TransfluentTranslationDateTime(now);
        m_lastChecked = new TransfluentTranslationDateTime(now);
    }

    public TransfluentTranslationEntry(string textId, string groupId, int sourceLanguageId, int targetLanguageId, DateTime ordered, DateTime lastChecked)
    {
        m_textId = textId;
        m_groupId = groupId;
        m_sourceLanguageId = sourceLanguageId;
        m_targetLanguageId = targetLanguageId;

        m_ordered = new TransfluentTranslationDateTime(ordered);
        m_lastChecked = new TransfluentTranslationDateTime(lastChecked);
    }

    public DateTime ordered
    {
        get { return m_ordered.dateTime; }
        set { m_ordered = new TransfluentTranslationDateTime(value); }
    }

    public DateTime lastChecked
    {
        get { return m_lastChecked.dateTime; }
        set { m_lastChecked = new TransfluentTranslationDateTime(value); }
    }

    public static TransfluentTranslationEntry Clone(TransfluentTranslationEntry original)
    {
        return new TransfluentTranslationEntry(original.m_textId, original.m_groupId, original.m_sourceLanguageId, original.m_targetLanguageId, original.ordered, original.lastChecked);
    }
}

[System.Serializable]
public class TransfluentTranslationDateTime
{
    public int m_year;
    public int m_month;
    public int m_day;
    public int m_hour;
    public int m_minute;
    public int m_second;

    public TransfluentTranslationDateTime(DateTime now)
    {
        dateTime = now;
    }

    public TransfluentTranslationDateTime(int year, int month, int day, int hour, int minute, int second)
    {
        m_year = year;
        m_month = month;
        m_day = day;
        m_hour = hour;
        m_minute = minute;
        m_second = second;
    }

    public DateTime dateTime
    {
        get
        {
            return new DateTime(m_year, m_month, m_day, m_hour, m_minute, m_second, DateTimeKind.Utc);
        }
        set
        {
            DateTime dt = value.ToUniversalTime();

            m_year = dt.Year;
            m_month = dt.Month;
            m_day = dt.Day;
            m_hour = dt.Hour;
            m_minute = dt.Minute;
            m_second = dt.Second;
        }
    }
}