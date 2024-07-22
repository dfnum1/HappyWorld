using System.Collections;
using System.Collections.Generic;
using TopGame.UI;
using UnityEngine;

namespace AutoCode
{


    /// <summary>
    /// 给点一个ui serialize 然后生成一个logic,logic里面包含所有UI调用接口,用 partial 隔开,
    /// </summary>
    public class AutoCodeViewBuilder : AutoCodeBuilderBase
    {
        public UISerialized m_ui;

        public AutoCodeViewBuilder(string name, string filePath,UISerialized ui) : base(name, filePath)
        {
            m_ui = ui;
        }

        protected override void BuilderAutoCode()
        {
            if (m_ui == null)
            {
                return;
            }

            AddString("//AutoCode");
            AddString("using UnityEngine;");
            AddString("using UnityEngine.UI;");
            AddString("namespace TopGame.UI");
            AddString("{");
            AddTabNum();

            AddString($"public partial class {m_FileName}");
            AddString("{");
            AddTabNum();


            //设置控件变量名
            for (int i = 0; i < m_ui.Widgets.Length; i++)
            {
                var widget = m_ui.Widgets[i];
                if (widget.widget == null)
                {
                    continue;
                }
                string name = widget.widget.name;
                if (!string.IsNullOrWhiteSpace(widget.fastName))
                {
                    name= widget.fastName;
                }

                AddString($"private {widget.widget.GetType().ToString()} m_{name};");

                //Debug.Log($"widget type:{widget.widget.GetType()},type:{widget.assignType}");
            }

            for (int i = 0; i < m_ui.Elements.Length; i++)
            {
                var element = m_ui.Elements[i];
                if (element == null)
                {
                    continue;
                }

                AddString($"private GameObject m_{element.name};");
            }

            AddString("//------------------------------------------------------");
            AddString("public void AwakeUI(UISerialized ui)");

            AddString("{");
            AddTabNum();

            AddString("if (ui == null) return;");

            //设置控件获取
            for (int i = 0; i < m_ui.Widgets.Length; i++)
            {
                var widget = m_ui.Widgets[i];
                if (widget.widget == null)
                {
                    continue;
                }
                string name = widget.widget.name;
                if (!string.IsNullOrWhiteSpace(widget.fastName))
                {
                    name = widget.fastName;
                }

                AddString($"m_{name} = ui.GetWidget<{widget.widget.GetType().ToString()}>(\"{name}\");");

            }

            for (int i = 0; i < m_ui.Elements.Length; i++)
            {
                var element = m_ui.Elements[i];
                if (element == null)
                {
                    continue;
                }

                AddString($"m_{element.name} = ui.Find(\"{element.name}\");");
            }

            SubTabNum();
            AddString("}");

            //设置控件调用函数和点击事件 Text EventTriggerListener ListView 
            for (int i = 0; i < m_ui.Widgets.Length; i++)
            {
                var widget = m_ui.Widgets[i];
                if (widget.widget == null)
                {
                    continue;
                }
                string name = widget.widget.name;
                if (!string.IsNullOrWhiteSpace(widget.fastName))
                {
                    name = widget.fastName;
                }

                switch (widget.widget.GetType().FullName)
                {
                    case "UnityEngine.UI.Text":
                        AddString("//------------------------------------------------------");
                        AddString($"public void Set{name}Label(uint key)");
                        AddString("{");
                        AddTabNum();

                        AddString($"UIUtil.SetLabel(m_{name}, key);");

                        SubTabNum();
                        AddString("}");

                        AddString("//------------------------------------------------------");
                        AddString($"public void Set{name}Label(string content)");
                        AddString("{");
                        AddTabNum();

                        AddString($"UIUtil.SetLabel(m_{name}, content);");


                        SubTabNum();
                        AddString("}");
                        break;
                    case "TopGame.UI.EventTriggerListener":
                        AddString("//------------------------------------------------------");
                        AddString($"public void Set{name}Listener(EventTriggerListener.VoidDelegate click)");
                        AddString("{");
                        AddTabNum();

                        AddString($"if (m_{name}) m_{name}.onClick = click;");


                        SubTabNum();
                        AddString("}");
                        break;
                    case "TopGame.UI.ListView":
                        //RefreshList
                        AddString("//------------------------------------------------------");
                        AddString($"public void Refresh{name}List(ListView list,int count, bool isScroll)");
                        AddString("{");
                        AddTabNum();

                        AddString($"if (list)");
                        AddString("{");
                        AddTabNum();

                        AddString("list.numItems = (uint)count;");
                        AddString("if (isScroll) list.content.anchoredPosition = Vector2.zero;");

                        SubTabNum();
                        AddString("}");

                        SubTabNum();
                        AddString("}");

                        //AddListener
                        AddString("//------------------------------------------------------");
                        AddString($"void AddListener(ListView listView, UnityEngine.Events.UnityAction<int, UISerialized> action)");
                        AddString("{");
                        AddTabNum();

                        AddString($"if (listView)");
                        AddString("{");
                        AddTabNum();

                        AddString("listView.onItemRender.AddListener(action);");

                        SubTabNum();
                        AddString("}");

                        SubTabNum();
                        AddString("}");

                        //RemoveListener
                        AddString($"void RemoveListener(ListView listView)");
                        AddString("{");
                        AddTabNum();

                        AddString($"if (listView)");
                        AddString("{");
                        AddTabNum();

                        AddString("listView.onItemRender.RemoveAllListeners();");

                        SubTabNum();
                        AddString("}");

                        SubTabNum();
                        AddString("}");
                        break;
                }

                

            }

            for (int i = 0; i < m_ui.Elements.Length; i++)
            {
                var element = m_ui.Elements[i];
                if (element == null)
                {
                    continue;
                }

                AddString("//------------------------------------------------------");
                AddString($"public void Set{element.name}Active(bool active)");
                AddString("{");
                AddTabNum();

                AddString($"UIUtil.SetActive(m_{element.name}, active);");


                SubTabNum();
                AddString("}");
            }

            SubTabNum();
            AddString("}");

            SubTabNum();
            AddString("}");
        }
    }
}
