/********************************************************************
生成日期:	8:2:2021 15:00
类    名: 	TextPro
作    者:	JaydenHe
描    述:	汉字行首字不为中文字符
*********************************************************************/
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;

public class TextPro : Text
{
    private readonly string cnMarkList = @"(\。|\？|\！|\，|\、|\；|\：|\「|\」|\『|\』|\‘|\’|\“|\”|\（|\）|\〔|\〕|\【|\】|\—|\…|\–|\．|\《|\》|\〈|\〉)";
    StringBuilder textStr;

    public override void SetVerticesDirty()
    {
        var settings = GetGenerationSettings(rectTransform.rect.size);
        cachedTextGenerator.Populate(this.text, settings);

        textStr = new StringBuilder(this.text);

        IList<UILineInfo> lineList = this.cachedTextGenerator.lines;
        int changeIndex = -1;
        for (int i = 1; i < lineList.Count; i++)
        {
            if (lineList[i].startCharIdx >= text.Length) break;
            bool isMark = Regex.IsMatch(text[lineList[i].startCharIdx].ToString(), cnMarkList);

            if (isMark)
            {
                changeIndex = lineList[i].startCharIdx - 1;
                int startIdx = lineList[i - 1].startCharIdx;
                int lastIdx = lineList[i].startCharIdx;
                string str = text.Substring(startIdx, lastIdx - startIdx);
                MatchCollection richStrMatch = Regex.Matches(str, ".(</color>|<color=#\\w{6}>|" + cnMarkList + ")+$");
                if (richStrMatch.Count > 0)
                {
                    string richStr = richStrMatch[0].ToString();
                    int length = richStr.Length;
                    changeIndex = lineList[i].startCharIdx - length;
                    break;
                }
            }
        }

        if (changeIndex >= 0)
        {
            textStr.Insert(changeIndex, '\n');
            this.text = textStr.ToString();
        }

        base.SetVerticesDirty();
    }
}
