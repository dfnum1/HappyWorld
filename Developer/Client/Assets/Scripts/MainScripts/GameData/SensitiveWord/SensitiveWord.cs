/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	SensitiveWord
作    者:	HappLI
描    述:	敏感词
*********************************************************************/

using Framework.Data;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace TopGame.Data
{
    //------------------------------------------------------
    public class SensitiveWord
    {
        static SensitiveWord ms_Instance;
        public static SensitiveWord getInstance()
        {
            if (ms_Instance == null) ms_Instance = new SensitiveWord();
            return ms_Instance;
        }
        //------------------------------------------------------
        private string m_SensitiveWord = "*";
        private SensitiveNode m_RootNode = new SensitiveNode();
        private StringBuilder m_SB = new StringBuilder(32);
        private bool m_bInited = false;
        //------------------------------------------------------
        public void SetReplace(string strWord)
        {
            m_SensitiveWord = strWord;
        }
        //------------------------------------------------------
        private bool IsSymbol(char c)
        {
            int ic = c;
            // 东亚文字范围 (0x2E80-0x9FFF)
            return !((c >= '0' && c <= '9') || (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z')) && (ic < 0x2E80 || ic > 0x9FFF);
        }
        //------------------------------------------------------
        public void Load(string strName, string strEnter="word")
        {
            if (string.IsNullOrEmpty(strName)) return;
            TextAsset textAsset = Resources.Load<TextAsset>(strName);
            if (textAsset == null) return;

            CsvParser csv = new CsvParser();
            if (!csv.LoadTableString(textAsset.text))
            {
                Resources.UnloadAsset(textAsset);
                return;
            }
            Resources.UnloadAsset(textAsset);

            csv.SetTitleLine(0);
            int i = csv.GetTitleLine();
            int nLineCnt = csv.GetLineCount();
            for (i++; i < nLineCnt; i++)
            {
                AddWord(csv[i]["word"].String());
            }
            m_bInited = true;
        }
        //------------------------------------------------------
        public void AddWords(List<string> words)
        {
            if (words == null || words.Count == 0) return;
            for (int i = 0, count = words.Count; i < count; i++)
            {
                AddWord(words[i]);
            }
        }
        //------------------------------------------------------
        public void AddWord(string word)
        {
            if (string.IsNullOrEmpty(word))
                return;
            SensitiveNode tempNode = m_RootNode;
            for (int i = 0; i < word.Length; ++i)
            {
                char c = word[i];
                if (IsSymbol(c))
                {
                    continue;
                }
                SensitiveNode node = tempNode.GetChid(c);

                if (node == null)
                {
                    node = new SensitiveNode();
                    tempNode.AddSubNode(c, node);
                }

                tempNode = node;

                if (i == word.Length - 1)
                {
                    tempNode.isWordEnd = true;
                }
            }
        }
        //------------------------------------------------------
        public static string FilterWord(string text)
        {
            if (ms_Instance == null || !ms_Instance.m_bInited) return text;
            return ms_Instance.Filter(text);
        }
        //------------------------------------------------------
        public string Filter(string text)
        {
            if (!m_bInited) return text;
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            m_SB.Clear();
            m_SB.Length = 0;
            SensitiveNode tempNode = m_RootNode;
            int begin = 0;
            int position = 0;
            int word_length = text.Length;
            while (position < word_length)
            {
                char c = text[position];
                if (IsSymbol(c))
                {
                    if (tempNode == m_RootNode)
                    {
                        m_SB.Append(c);
                        ++begin;
                    }
                    ++position;
                    continue;
                }

                tempNode = tempNode.GetChid(c);

                if (tempNode == null)
                {
                    m_SB.Append(text[begin]);
                    position = begin + 1;
                    begin = position;
                    tempNode = m_RootNode;
                }
                else if (tempNode.isWordEnd)
                {
                    m_SB.Append(m_SensitiveWord);
                    position = position + 1;
                    begin = position;
                    tempNode = m_RootNode;
                }
                else
                {
                    ++position;
                }
            }

            m_SB.Append(text.Substring(begin));

            return m_SB.ToString();
        }
    }
}