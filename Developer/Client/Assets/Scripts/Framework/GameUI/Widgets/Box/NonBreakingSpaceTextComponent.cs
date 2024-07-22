using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Text))]
public class NonBreakingSpaceTextComponent : MonoBehaviour
{
    Text m_text;
    static readonly string m_NoBreakSpace = "\u00A0";
    static readonly string m_ChangeLine = "\\n";
    void Awake()
    {
        m_text = GetComponent<Text>();
        m_text.RegisterDirtyVerticesCallback(OnTextChange);
    }

    public void OnTextChange()
    {
        if (m_text.text.Contains(" "))
            m_text.text = m_text.text.Replace(" ", m_NoBreakSpace);
        if(m_text.text.Contains(m_ChangeLine))
            m_text.text = m_text.text.Replace(m_ChangeLine, "\n");
    }
}
