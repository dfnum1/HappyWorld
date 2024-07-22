
using Framework.Core;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TopGame.UI
{
    public class EditorWidgetMgr : Base.Singleton<EditorWidgetMgr>
    {
        Stack<UISerialized>[] m_vPools = new Stack<UISerialized>[(int)EditorWidgetType.Count];
        UISerialized[] m_EditorWidgets = null;

        EditorMessageBox m_MessageBox;
        public EditorWidgetMgr()
        {
            for(int i =0; i < (int)EditorWidgetType.Count; ++i)
            {
                m_vPools[i] = new Stack<UISerialized>();
            }
        }
        //------------------------------------------------------
        public void Init(UISerialized[] widgets)
        {
            m_EditorWidgets = widgets;
        }
        //------------------------------------------------------
        public EditorMessageBox PopMessage(string strTitle, string strMsg, System.Action onOk = null, System.Action onCancel = null)
        {
            if (!m_MessageBox.IsValid())
            {
#if USE_FAIRYGUI
                return m_MessageBox;
#else

                RectTransform root = UIKits.GetAutoUIRoot();
                if (root == null) return m_MessageBox;
                m_MessageBox = new EditorMessageBox(root);
#endif

            }
            if (m_MessageBox.IsValid())
            {
                m_MessageBox.Title = strTitle;
                m_MessageBox.Content = strMsg;
                m_MessageBox.ShowOk(false);
                m_MessageBox.ShowCancel(false);
                m_MessageBox.SetCallback(onOk, onCancel);
                m_MessageBox.Show();
            }
            return m_MessageBox;
        }
        //------------------------------------------------------
        public UISerialized CreateWidget(RectTransform pParent, EditorWidgetType type)
        {
            int index = (int)type;
            if (index < 0 || index >= m_vPools.Length) return null;
            if (m_vPools[index].Count > 0)
                return m_vPools[index].Pop();
            if (m_EditorWidgets == null || index >= m_EditorWidgets.Length || m_EditorWidgets[index] == null) return null;
            GameObject pObject = GameObject.Instantiate(m_EditorWidgets[index].gameObject);
            UISerialized seriazed = pObject.GetComponent<UISerialized>();
            if(seriazed == null)
            {
                Base.GlobalUtil.Desytroy(pObject);
                return null;
            }
            seriazed.transform.SetParent(pParent);
            RectTransform rect = seriazed.transform as RectTransform;
            rect.localScale = Vector3.one;
            rect.anchoredPosition = Vector2.zero;
//             rect.offsetMax = Vector3.zero;
//             rect.offsetMin = Vector3.zero;
//             rect.anchorMin = Vector2.zero;
//             rect.anchorMax = Vector2.one;
            return seriazed;
        }
        //------------------------------------------------------
        public void RecyleWidget(EditorWidgetType type, UISerialized widget)
        {
            int index = (int)type;
            if (index < 0 || index >= m_vPools.Length)
            {
                if (widget) Base.GlobalUtil.Desytroy(widget.gameObject);
                return;
            }
            widget.transform.SetParent(FileSystemUtil.PoolRoot);
            m_vPools[index].Push(widget);
        }
    }
    //------------------------------------------------------
    public struct EditorWidgetUI
    {
        private Dictionary<string, EditorWidget> m_vWidgets;
        public void AddWidget(string name, EditorWidget widget)
        {
            if (string.IsNullOrEmpty(name)) return;
            UISerialized ui = widget.GetUI();
            if (ui == null) return;

            if (m_vWidgets == null) m_vWidgets = new Dictionary<string, EditorWidget>();
            else
            {
                if (m_vWidgets.ContainsKey(name))
                    return;
            }
            RectTransform rectTrans = ui.transform as RectTransform;
            rectTrans.anchoredPosition = new Vector3(0, m_vWidgets.Count*30);
            rectTrans.sizeDelta = new Vector2((rectTrans.parent as RectTransform).rect.width, 30 );
            m_vWidgets[name] = widget;
        }
        //------------------------------------------------------
        public void Destroy()
        {
            if (m_vWidgets == null) return;
            foreach (var db in m_vWidgets)
            {
                db.Value.Dispose();
            }
            m_vWidgets.Clear();
        }
        //------------------------------------------------------
        public T GetWidget<T>(string strWidgetName) where T : EditorWidget
        {
            EditorWidget widget;
            if (m_vWidgets.TryGetValue(strWidgetName, out widget))
            {
                if (widget is T)
                    return (T)widget;
            }
            return default;
        }
        //------------------------------------------------------
    }
    //------------------------------------------------------
    public interface EditorWidget : System.IDisposable
    {
        bool IsValid();
        UISerialized GetUI();
    }
    //------------------------------------------------------
    public struct EditorIcon : EditorWidget
    {
        string m_strIcon;
        UISerialized m_ui;
        RawImageEx m_pIcon;
        public EditorIcon(RectTransform pParent, string icon)
        {
            m_strIcon = icon;
            m_ui = EditorWidgetMgr.getInstance().CreateWidget(pParent, EditorWidgetType.Icon);
            if(m_ui) m_pIcon = m_ui.GetWidget<RawImageEx>("self");
            else m_pIcon = null;
            if (m_pIcon)
                m_pIcon.SetAssetByPath(icon, null);
        }
        //------------------------------------------------------
        public UISerialized GetUI() { return m_ui; }
        public bool IsValid() { return m_ui != null; }
        //------------------------------------------------------
        public string icon
        {
            get
            {
                return m_strIcon;
            }
            set
            {
                if(m_strIcon == null || m_strIcon.CompareTo(value)!=0)
                {
                   
                     m_strIcon = value;
                    if (m_pIcon == null) return;
                    m_pIcon.SetAssetByPath(m_strIcon, null);
                }
            }
        }
        //------------------------------------------------------
        public void Dispose()
        {
            if (m_ui)
            {
                EditorWidgetMgr.getInstance().RecyleWidget(EditorWidgetType.Icon, m_ui);
                m_ui = null;
            }
            m_pIcon = null;
        }
    }
    //------------------------------------------------------
    public struct EditorLabel : EditorWidget
    {
        string m_strValue;
        UISerialized m_ui;
        Text m_pLabel;
        public EditorLabel(RectTransform pParent, string label)
        {
            m_strValue = label;
            m_ui = EditorWidgetMgr.getInstance().CreateWidget(pParent, EditorWidgetType.Label);
            if (m_ui) m_pLabel = m_ui.GetWidget<Text>("self");
            else m_pLabel = null;
        }
        //------------------------------------------------------
        public UISerialized GetUI() { return m_ui; }
        public bool IsValid() { return m_ui != null; }

        //------------------------------------------------------
        public string label
        {
            get
            {
                return m_strValue;
            }
            set
            {
                m_strValue = value;
                if (m_pLabel == null) return;
                m_pLabel.text = m_strValue;
            }
        }
        //------------------------------------------------------
        public void Dispose()
        {
            if (m_ui)
            {
                EditorWidgetMgr.getInstance().RecyleWidget(EditorWidgetType.Label, m_ui);
                m_ui = null;
            }
            m_pLabel = null;
        }
    }
    //------------------------------------------------------
    public struct EditText : EditorWidget
    {
        Variable1 m_Value;
        UISerialized m_ui;
        InputField m_pInput;
        System.Action<VariablePoolAble> m_pCallback;
        public EditText(RectTransform pParent, string strLabel, int val, System.Action<VariablePoolAble> callback=null)
        {
            m_pCallback = callback;
            m_Value = new Variable1() { intVal = val };
            m_ui = EditorWidgetMgr.getInstance().CreateWidget(pParent, EditorWidgetType.InputText);
            if (m_ui)
            {
                m_pInput = m_ui.GetWidget<InputField>("InputField");
                Text label = m_ui.GetWidget<Text>("Label");
                if (label) label.text = strLabel;
            }
            else
            {
                m_pInput = null;
            }
            if(m_pInput)
            {
                m_pInput.text = val.ToString();
                m_pInput.contentType = InputField.ContentType.IntegerNumber;
                m_pInput.onValueChanged.AddListener(OnValueChange);
            }
        }
        //------------------------------------------------------
        public EditText(RectTransform pParent, string strLabel, float val, System.Action<VariablePoolAble> callback = null)
        {
            m_pCallback = callback;
            m_Value = new Variable1() { floatVal = val };
            m_ui = EditorWidgetMgr.getInstance().CreateWidget(pParent, EditorWidgetType.InputText);
            if (m_ui)
            {
                m_pInput = m_ui.GetWidget<InputField>("InputField");
                Text label = m_ui.GetWidget<Text>("Label");
                if (label) label.text = strLabel;

            }
            else
            {
                m_pInput = null;
            }
            if (m_pInput)
            {
                m_pInput.text = val.ToString("F2");
                m_pInput.contentType = InputField.ContentType.DecimalNumber;
                m_pInput.onValueChanged.AddListener(OnValueChange);
            }
        }
        //------------------------------------------------------
        public EditText(RectTransform pParent, string strLabel, string val, System.Action<VariablePoolAble> callback = null)
        {
            m_pCallback = callback;
            m_Value = new Variable1();
            m_ui = EditorWidgetMgr.getInstance().CreateWidget(pParent, EditorWidgetType.InputText);
            if (m_ui)
            {
                m_pInput = m_ui.GetWidget<InputField>("InputField");
                Text label = m_ui.GetWidget<Text>("Label");
                if (label) label.text = strLabel;
            }
            else
            {
                m_pInput = null;
            }
            if (m_pInput)
            {
                m_pInput.text = val;
                m_pInput.contentType = InputField.ContentType.Name;
                m_pInput.onValueChanged.RemoveAllListeners();
            }
        }
        //------------------------------------------------------
        public UISerialized GetUI() { return m_ui; }
        public bool IsValid() { return m_ui != null; }
        //------------------------------------------------------
        void OnValueChange(string value)
        {
            if(m_pInput)
            {
                if (m_pInput.contentType == InputField.ContentType.IntegerNumber)
                {
                    int temp;
                    if (int.TryParse(value, out temp))
                        m_Value.intVal = temp;
                    if (m_pCallback != null) m_pCallback(m_Value);
                }
                if (m_pInput.contentType == InputField.ContentType.DecimalNumber)
                {
                    float temp;
                    if (float.TryParse(value, out temp))
                        m_Value.floatVal = temp;
                    if (m_pCallback != null) m_pCallback(m_Value);
                }
                if (m_pCallback != null) m_pCallback(new VariableString() { strValue = value });
            }
        }
        //------------------------------------------------------
        public int intValue
        {
            get
            {
                return m_Value.intVal;
            }
            set
            {
                if (m_Value.intVal == value)
                    return;
                m_Value.intVal = value;
                if (m_pInput)
                {
                    m_pInput.onValueChanged.RemoveAllListeners();
                    m_pInput.text = m_Value.intVal.ToString();
                    m_pInput.onValueChanged.AddListener(OnValueChange);
                }
            }
        }
        //------------------------------------------------------
        public float floatValue
        {
            get
            {
                return m_Value.floatVal;
            }
            set
            {
                if (m_Value.intVal == value)
                    return;
                m_Value.floatVal = value;
                if (m_pInput)
                {
                    m_pInput.onValueChanged.RemoveAllListeners();
                    m_pInput.text = m_Value.floatVal.ToString("F2");
                    m_pInput.onValueChanged.AddListener(OnValueChange);
                }
            }
        }
        //------------------------------------------------------
        public string strValue
        {
            get
            {
                if (m_pInput) return m_pInput.text;
                return null;
            }
            set
            {
                if (m_pInput)
                {
                    m_pInput.onValueChanged.RemoveAllListeners();
                    m_pInput.text = value;
                    m_pInput.onValueChanged.AddListener(OnValueChange);
                }
            }
        }
        //------------------------------------------------------
        public void Dispose()
        {
            if(m_pInput) m_pInput.onValueChanged.RemoveAllListeners();
            if (m_ui)
            {
                EditorWidgetMgr.getInstance().RecyleWidget(EditorWidgetType.InputText, m_ui);
                m_ui = null;
            }
            m_pCallback = null;
        }
    }

    //------------------------------------------------------
    public struct EditorButton : EditorWidget
    {
        string m_strLabel;
        UISerialized m_ui;
        Button m_pButton;

        System.Action m_pClick;
        public EditorButton(RectTransform pParent, string label, System.Action onClick)
        {
            m_pClick = onClick;
            m_strLabel = label;
            m_ui = EditorWidgetMgr.getInstance().CreateWidget(pParent, EditorWidgetType.Button);
            if (m_ui)
            {
                Text pLabel = m_ui.GetWidget<Text>("Text");
                if (pLabel) pLabel.text = label;
                 m_pButton = m_ui.GetWidget<Button>("self");
                m_pButton.onClick.AddListener(OnClick);
            }
            else
            {
                m_pButton = null;
            }
        }
        //------------------------------------------------------
        public UISerialized GetUI() { return m_ui; }
        public bool IsValid() { return m_ui != null; }
        //------------------------------------------------------
        void OnClick()
        {
            if (m_pClick != null) m_pClick();
        }
        //------------------------------------------------------
        public void Dispose()
        {
            if (m_pButton) m_pButton.onClick.RemoveAllListeners();
            if (m_ui)
            {
                EditorWidgetMgr.getInstance().RecyleWidget(EditorWidgetType.Button, m_ui);
                m_ui = null;
            }
            m_pButton = null;
            m_pClick = null;
        }
    }
    //------------------------------------------------------
    public struct EditorBool : EditorWidget
    {
        UISerialized m_ui;
        Toggle m_pWidget;
        System.Action<bool> m_pCallback;
        public EditorBool(RectTransform pParent, string label, System.Action<bool> onCallback)
        {
            m_pCallback = onCallback;
            m_ui = EditorWidgetMgr.getInstance().CreateWidget(pParent, EditorWidgetType.Bool);
            if (m_ui)
            {
                Text pLabel = m_ui.GetWidget<Text>("Label");
                m_pWidget = m_ui.GetWidget<Toggle>("self");
                if (m_pWidget) m_pWidget.onValueChanged.AddListener(OnToggle);
                if (pLabel) pLabel.text = label;
            }
            else
            {
                m_pWidget = null;
            }
        }
        //------------------------------------------------------
        public UISerialized GetUI() { return m_ui; }
        public bool IsValid() { return m_ui != null; }
        //------------------------------------------------------
        public bool toggle
        {
            get
            {
                if (m_pWidget == null) return false;
                return m_pWidget.isOn;
            }
            set
            {
                if (m_pWidget == null) return;
                m_pWidget.isOn = value;
            }
        }
        //------------------------------------------------------
        void OnToggle(bool bToggle)
        {
            if (m_pCallback != null) m_pCallback(bToggle);
        }
        //------------------------------------------------------
        public void Dispose()
        {
            if (m_pWidget) m_pWidget.onValueChanged.RemoveAllListeners();
            if (m_ui)
            {
                EditorWidgetMgr.getInstance().RecyleWidget(EditorWidgetType.Bool, m_ui);
                m_ui = null;
            }
            m_pWidget = null;
            m_pCallback = null;
        }
    }
    //------------------------------------------------------
    public struct EditorValue : EditorWidget
    {
        UISerialized m_ui;
        InputField m_pWidget;
        System.Action<VariablePoolAble> m_pCallback;
        public EditorValue(RectTransform pParent, string label, int value, System.Action<VariablePoolAble> onCallback)
        {
            m_pCallback = onCallback;
            m_ui = EditorWidgetMgr.getInstance().CreateWidget(pParent, EditorWidgetType.Value);
            if (m_ui)
            {
                Text pLabel = m_ui.GetWidget<Text>("Label");
                m_pWidget = m_ui.GetWidget<InputField>("0");
                if (m_pWidget)
                {
                    m_pWidget.onValueChanged.AddListener(OnWidgetCallback);
                    m_pWidget.contentType = InputField.ContentType.IntegerNumber;
                    m_pWidget.text = value.ToString();
                }
                if (pLabel)
                {
                    pLabel.text = label;
                }
            }
            else
            {
                m_pWidget = null;
            }
        }
        public EditorValue(RectTransform pParent, string label, float value, System.Action<VariablePoolAble> onCallback)
        {
            m_pCallback = onCallback;
            m_ui = EditorWidgetMgr.getInstance().CreateWidget(pParent, EditorWidgetType.Value);
            if (m_ui)
            {
                Text pLabel = m_ui.GetWidget<Text>("Label");
                m_pWidget = m_ui.GetWidget<InputField>("0");
                if (m_pWidget)
                {
                    m_pWidget.onValueChanged.AddListener(OnWidgetCallback);
                    m_pWidget.contentType = InputField.ContentType.DecimalNumber;
                    m_pWidget.text = value.ToString("F2");
                }
                if (pLabel) pLabel.text = label;
            }
            else
            {
                m_pWidget = null;
            }
        }
        //------------------------------------------------------
        public UISerialized GetUI() { return m_ui; }
        public bool IsValid() { return m_ui != null; }
        //------------------------------------------------------
        public int intValue
        {
            get
            {
                if (m_pWidget == null) return 0;
                int temp;
                int.TryParse(m_pWidget.text, out temp);
                return temp;
            }
            set
            {
                if (m_pWidget == null) return;
                m_pWidget.text = value.ToString();
            }
        }
        //------------------------------------------------------
        public float floatValue
        {
            get
            {
                if (m_pWidget == null) return 0;
                float temp;
                float.TryParse(m_pWidget.text, out temp);
                return temp;
            }
            set
            {
                if (m_pWidget == null) return;
                m_pWidget.text = value.ToString("F2");
            }
        }
        //------------------------------------------------------
        void OnWidgetCallback(string val)
        {
            if (m_pCallback != null)
            {
                if(m_pWidget.contentType == InputField.ContentType.IntegerNumber)
                    m_pCallback(new Variable1() { intVal = int.Parse(val) });
                else if (m_pWidget.contentType == InputField.ContentType.DecimalNumber)
                    m_pCallback(new Variable1() { floatVal = float.Parse(val) });
                else if (m_pWidget.contentType == InputField.ContentType.Name)
                    m_pCallback(new VariableString() { strValue = val });
            }
        }
        //------------------------------------------------------
        public void Dispose()
        {
            if (m_pWidget) m_pWidget.onValueChanged.RemoveAllListeners();
            if (m_ui)
            {
                EditorWidgetMgr.getInstance().RecyleWidget(EditorWidgetType.Value, m_ui);
                m_ui = null;
            }
            m_pWidget = null;
            m_pCallback = null;
        }
    }
    //------------------------------------------------------
    public struct EditorValue2 : EditorWidget
    {
        UISerialized m_ui;
        InputField m_pWidget0;
        InputField m_pWidget1;
        System.Action<VariablePoolAble> m_pCallback;
        public EditorValue2(RectTransform pParent, string label, Vector2 value, System.Action<VariablePoolAble> onCallback)
        {
            m_pCallback = onCallback;
            m_ui = EditorWidgetMgr.getInstance().CreateWidget(pParent, EditorWidgetType.Value2);
            if (m_ui)
            {
                Text pLabel = m_ui.GetWidget<Text>("Label");
                m_pWidget0 = m_ui.GetWidget<InputField>("0");
                m_pWidget1 = m_ui.GetWidget<InputField>("1");
                if (m_pWidget0)
                {
                    m_pWidget0.onValueChanged.AddListener(OnWidgetCallback);
                    m_pWidget0.contentType = InputField.ContentType.DecimalNumber;
                    m_pWidget0.text = value.x.ToString("F2");
                }
                if (m_pWidget1)
                {
                    m_pWidget1.onValueChanged.AddListener(OnWidgetCallback);
                    m_pWidget1.contentType = InputField.ContentType.DecimalNumber;
                    m_pWidget1.text = value.y.ToString("F2");
                }
                if (pLabel) pLabel.text = label;
            }
            else
            {
                m_pWidget0 = null;
                m_pWidget1 = null;
            }
        }
        //------------------------------------------------------
        public EditorValue2(RectTransform pParent, string label, Vector2Int value, System.Action<VariablePoolAble> onCallback)
        {
            m_pCallback = onCallback;
            m_ui = EditorWidgetMgr.getInstance().CreateWidget(pParent, EditorWidgetType.Value2);
            if (m_ui)
            {
                Text pLabel = m_ui.GetWidget<Text>("Label");
                m_pWidget0 = m_ui.GetWidget<InputField>("0");
                m_pWidget1 = m_ui.GetWidget<InputField>("1");
                if (m_pWidget0)
                {
                    m_pWidget0.onValueChanged.AddListener(OnWidgetCallback);
                    m_pWidget0.contentType = InputField.ContentType.IntegerNumber;
                    m_pWidget0.text = value.x.ToString();
                }
                if (m_pWidget1)
                {
                    m_pWidget1.onValueChanged.AddListener(OnWidgetCallback);
                    m_pWidget1.contentType = InputField.ContentType.IntegerNumber;
                    m_pWidget1.text = value.y.ToString();
                }
                if (pLabel) pLabel.text = label;
            }
            else
            {
                m_pWidget0 = null;
                m_pWidget1 = null;
            }
        }
        //------------------------------------------------------
        public UISerialized GetUI() { return m_ui; }
        public bool IsValid() { return m_ui != null; }
        //------------------------------------------------------
        public Vector2Int intValue
        {
            get
            {
                Vector2Int temp = Vector2Int.zero;
                if (m_pWidget0 == null || m_pWidget1 == null) return temp;
                int tV;
                if (int.TryParse(m_pWidget0.text, out tV)) temp.x = tV;
                if(int.TryParse(m_pWidget1.text, out tV)) temp.y = tV;
                return temp;
            }
            set
            {
                if (m_pWidget0 == null || m_pWidget1 == null) return;
                m_pWidget0.text = value.x.ToString();
                m_pWidget1.text = value.y.ToString();
            }
        }//------------------------------------------------------
        public Vector2 floatValue
        {
            get
            {
                Vector2 temp = Vector3.zero;
                if (m_pWidget0 == null || m_pWidget1 == null) return temp;
                float tV;
                if (float.TryParse(m_pWidget0.text, out tV)) temp.x = tV;
                if (float.TryParse(m_pWidget1.text, out tV)) temp.y = tV;
                return temp;
            }
            set
            {
                if (m_pWidget0 == null || m_pWidget1 == null) return;
                m_pWidget0.text = value.x.ToString("F2");
                m_pWidget1.text = value.y.ToString("F2");
            }
        }
        //------------------------------------------------------
        void OnWidgetCallback(string val)
        {
            OnWidgetCallback();
        }
        //------------------------------------------------------
        void OnWidgetCallback()
        {
            if (m_pCallback == null) return;
            if (m_pWidget0.contentType == InputField.ContentType.IntegerNumber)
            {
                Variable2 vec = new Variable2();
                int tV;
                if (int.TryParse(m_pWidget0.text, out tV)) vec.intVal0 = tV;
                if (int.TryParse(m_pWidget1.text, out tV)) vec.intVal1 = tV;
                m_pCallback(vec);

            }
            else if (m_pWidget0.contentType == InputField.ContentType.DecimalNumber)
            {
                Variable2 vec = new Variable2();
                float tV;
                if (float.TryParse(m_pWidget0.text, out tV)) vec.floatVal0 = tV;
                if (float.TryParse(m_pWidget1.text, out tV)) vec.floatVal1 = tV;
                m_pCallback(vec);
            }
        }
        //------------------------------------------------------
        public void Dispose()
        {
            if (m_pWidget0) m_pWidget0.onValueChanged.RemoveAllListeners();
            if (m_pWidget1) m_pWidget1.onValueChanged.RemoveAllListeners();
            if (m_ui)
            {
                EditorWidgetMgr.getInstance().RecyleWidget(EditorWidgetType.Value2, m_ui);
                m_ui = null;
            }
            m_pWidget0 = null;
            m_pWidget1 = null;
            m_pCallback = null;
        }
    }

    //------------------------------------------------------
    public struct EditorValue3 : EditorWidget
    {
        UISerialized m_ui;
        InputField m_pWidget0;
        InputField m_pWidget1;
        InputField m_pWidget2;
        System.Action<VariablePoolAble> m_pCallback;
        public EditorValue3(RectTransform pParent, string label, Vector3 value, System.Action<VariablePoolAble> onCallback)
        {
            m_pCallback = onCallback;
            m_ui = EditorWidgetMgr.getInstance().CreateWidget(pParent, EditorWidgetType.Value3);
            if (m_ui)
            {
                Text pLabel = m_ui.GetWidget<Text>("Label");
                m_pWidget0 = m_ui.GetWidget<InputField>("0");
                m_pWidget1 = m_ui.GetWidget<InputField>("1");
                m_pWidget2 = m_ui.GetWidget<InputField>("2");
                if (m_pWidget0)
                {
                    m_pWidget0.onValueChanged.AddListener(OnWidgetCallback);
                    m_pWidget0.contentType = InputField.ContentType.DecimalNumber;
                    m_pWidget0.text = value.x.ToString("F2");
                }
                if (m_pWidget1)
                {
                    m_pWidget1.onValueChanged.AddListener(OnWidgetCallback);
                    m_pWidget1.contentType = InputField.ContentType.DecimalNumber;
                    m_pWidget1.text = value.y.ToString("F2");
                }
                if (m_pWidget2)
                {
                    m_pWidget2.onValueChanged.AddListener(OnWidgetCallback);
                    m_pWidget2.contentType = InputField.ContentType.DecimalNumber;
                    m_pWidget2.text = value.z.ToString("F2");
                }
                if (pLabel) pLabel.text = label;
            }
            else
            {
                m_pWidget0 = null;
                m_pWidget1 = null;
                m_pWidget2 = null;
            }
        }
        //------------------------------------------------------
        public EditorValue3(RectTransform pParent, string label, Vector3Int value, System.Action<VariablePoolAble> onCallback)
        {
            m_pCallback = onCallback;
            m_ui = EditorWidgetMgr.getInstance().CreateWidget(pParent, EditorWidgetType.Value3);
            if (m_ui)
            {
                Text pLabel = m_ui.GetWidget<Text>("Label");
                m_pWidget0 = m_ui.GetWidget<InputField>("0");
                m_pWidget1 = m_ui.GetWidget<InputField>("1");
                m_pWidget2 = m_ui.GetWidget<InputField>("2");
                if (m_pWidget0)
                {
                    m_pWidget0.onValueChanged.AddListener(OnWidgetCallback);
                    m_pWidget0.contentType = InputField.ContentType.DecimalNumber;
                    m_pWidget0.text = value.x.ToString();
                }
                if (m_pWidget1)
                {
                    m_pWidget1.onValueChanged.AddListener(OnWidgetCallback);
                    m_pWidget1.contentType = InputField.ContentType.DecimalNumber;
                    m_pWidget1.text = value.y.ToString();
                }
                if (m_pWidget2)
                {
                    m_pWidget2.onValueChanged.AddListener(OnWidgetCallback);
                    m_pWidget2.contentType = InputField.ContentType.DecimalNumber;
                    m_pWidget2.text = value.z.ToString();
                }
                if (pLabel) pLabel.text = label;
            }
            else
            {
                m_pWidget0 = null;
                m_pWidget1 = null;
                m_pWidget2 = null;
            }
        }
        //------------------------------------------------------
        public Vector3Int intValue
        {
            get
            {
                Vector3Int temp = Vector3Int.zero;
                if (m_pWidget0 == null || m_pWidget1 == null || m_pWidget2 == null) return temp;
                int tV;
                if (int.TryParse(m_pWidget0.text, out tV)) temp.x = tV;
                if (int.TryParse(m_pWidget1.text, out tV)) temp.y = tV;
                if (int.TryParse(m_pWidget2.text, out tV)) temp.z = tV;
                return temp;
            }
            set
            {
                if (m_pWidget0 == null || m_pWidget1 == null || m_pWidget2 == null) return;
                m_pWidget0.text = value.x.ToString();
                m_pWidget1.text = value.y.ToString();
                m_pWidget2.text = value.z.ToString();
            }
        }
        //------------------------------------------------------
        public Vector3 floatValue
        {
            get
            {
                Vector3 temp = Vector3.zero;
                if (m_pWidget0 == null || m_pWidget1 == null || m_pWidget2 == null) return temp;
                float tV;
                if (float.TryParse(m_pWidget0.text, out tV)) temp.x = tV;
                if (float.TryParse(m_pWidget1.text, out tV)) temp.y = tV;
                if (float.TryParse(m_pWidget2.text, out tV)) temp.z = tV;
                return temp;
            }
            set
            {
                if (m_pWidget0 == null || m_pWidget1 == null || m_pWidget2 == null) return;
                m_pWidget0.text = value.x.ToString("F2");
                m_pWidget1.text = value.y.ToString("F2");
                m_pWidget2.text = value.z.ToString("F2");
            }
        }
        //------------------------------------------------------
        public UISerialized GetUI() { return m_ui; }
        public bool IsValid() { return m_ui != null; }
        //------------------------------------------------------
        void OnWidgetCallback(string val)
        {
            OnWidgetCallback();
        }
        //------------------------------------------------------
        void OnWidgetCallback()
        {
            if (m_pCallback == null) return;
            if (m_pWidget0.contentType == InputField.ContentType.IntegerNumber)
            {
                Variable3 vec = new Variable3();
                int tV;
                if (int.TryParse(m_pWidget0.text, out tV)) vec.intVal0 = tV;
                if (int.TryParse(m_pWidget1.text, out tV)) vec.intVal1 = tV;
                if (int.TryParse(m_pWidget2.text, out tV)) vec.intVal2 = tV;
                m_pCallback(vec);

            }
            else if (m_pWidget0.contentType == InputField.ContentType.DecimalNumber)
            {
                Variable3 vec = new Variable3();
                float tV;
                if (float.TryParse(m_pWidget0.text, out tV)) vec.floatVal0 = tV;
                if (float.TryParse(m_pWidget1.text, out tV)) vec.floatVal1 = tV;
                if (float.TryParse(m_pWidget2.text, out tV)) vec.floatVal2 = tV;
                m_pCallback(vec);
            }
        }
        //------------------------------------------------------
        public void Dispose()
        {
            if (m_pWidget0) m_pWidget0.onValueChanged.RemoveAllListeners();
            if (m_pWidget1) m_pWidget1.onValueChanged.RemoveAllListeners();
            if (m_pWidget2) m_pWidget2.onValueChanged.RemoveAllListeners();
            if (m_ui)
            {
                EditorWidgetMgr.getInstance().RecyleWidget(EditorWidgetType.Value3, m_ui);
                m_ui = null;
            }
            m_pWidget0 = null;
            m_pWidget1 = null;
            m_pWidget2 = null;
            m_pCallback = null;
        }
    }
    //------------------------------------------------------
    public struct EditorValue4 : EditorWidget
    {
        UISerialized m_ui;
        InputField m_pWidget0;
        InputField m_pWidget1;
        InputField m_pWidget2;
        InputField m_pWidget3;
        System.Action<VariablePoolAble> m_pCallback;
        public EditorValue4(RectTransform pParent, string label, Vector4 value, System.Action<VariablePoolAble> onCallback)
        {
            m_pCallback = onCallback;
            m_ui = EditorWidgetMgr.getInstance().CreateWidget(pParent, EditorWidgetType.Value4);
            if (m_ui)
            {
                Text pLabel = m_ui.GetWidget<Text>("Label");
                m_pWidget0 = m_ui.GetWidget<InputField>("0");
                m_pWidget1 = m_ui.GetWidget<InputField>("1");
                m_pWidget2 = m_ui.GetWidget<InputField>("2");
                m_pWidget3 = m_ui.GetWidget<InputField>("3");
                if (m_pWidget0)
                {
                    m_pWidget0.onValueChanged.AddListener(OnWidgetCallback);
                    m_pWidget0.contentType = InputField.ContentType.DecimalNumber;
                    m_pWidget0.text = value.x.ToString("F2");
                }
                if (m_pWidget1)
                {
                    m_pWidget1.onValueChanged.AddListener(OnWidgetCallback);
                    m_pWidget1.contentType = InputField.ContentType.DecimalNumber;
                    m_pWidget1.text = value.y.ToString("F2");
                }
                if (m_pWidget2)
                {
                    m_pWidget2.onValueChanged.AddListener(OnWidgetCallback);
                    m_pWidget2.contentType = InputField.ContentType.DecimalNumber;
                    m_pWidget2.text = value.z.ToString("F2");
                }
                if (m_pWidget3)
                {
                    m_pWidget3.onValueChanged.AddListener(OnWidgetCallback);
                    m_pWidget3.contentType = InputField.ContentType.DecimalNumber;
                    m_pWidget3.text = value.w.ToString("F2");
                }
                if (pLabel) pLabel.text = label;
            }
            else
            {
                m_pWidget0 = null;
                m_pWidget1 = null;
                m_pWidget2 = null;
                m_pWidget3 = null;
            }
        }
        //------------------------------------------------------
        public EditorValue4(RectTransform pParent, string label, Rect value, System.Action<VariablePoolAble> onCallback)
        {
            m_pCallback = onCallback;
            m_ui = EditorWidgetMgr.getInstance().CreateWidget(pParent, EditorWidgetType.Value4);
            if (m_ui)
            {
                Text pLabel = m_ui.GetWidget<Text>("Label");
                m_pWidget0 = m_ui.GetWidget<InputField>("0");
                m_pWidget1 = m_ui.GetWidget<InputField>("1");
                m_pWidget2 = m_ui.GetWidget<InputField>("2");
                m_pWidget3 = m_ui.GetWidget<InputField>("3");
                if (m_pWidget0)
                {
                    m_pWidget0.onValueChanged.AddListener(OnWidgetCallback);
                    m_pWidget0.contentType = InputField.ContentType.DecimalNumber;
                    m_pWidget0.text = value.x.ToString("F2");
                }
                if (m_pWidget1)
                {
                    m_pWidget1.onValueChanged.AddListener(OnWidgetCallback);
                    m_pWidget1.contentType = InputField.ContentType.DecimalNumber;
                    m_pWidget1.text = value.y.ToString("F2");
                }
                if (m_pWidget2)
                {
                    m_pWidget2.onValueChanged.AddListener(OnWidgetCallback);
                    m_pWidget2.contentType = InputField.ContentType.DecimalNumber;
                    m_pWidget2.text = value.width.ToString("F2");
                }
                if (m_pWidget3)
                {
                    m_pWidget3.onValueChanged.AddListener(OnWidgetCallback);
                    m_pWidget3.contentType = InputField.ContentType.DecimalNumber;
                    m_pWidget3.text = value.height.ToString("F2");
                }
                if (pLabel) pLabel.text = label;
            }
            else
            {
                m_pWidget0 = null;
                m_pWidget1 = null;
                m_pWidget2 = null;
                m_pWidget3 = null;
            }
        }
        //------------------------------------------------------
        public EditorValue4(RectTransform pParent, string label, RectInt value, System.Action<VariablePoolAble> onCallback)
        {
            m_pCallback = onCallback;
            m_ui = EditorWidgetMgr.getInstance().CreateWidget(pParent, EditorWidgetType.Value4);
            if (m_ui)
            {
                Text pLabel = m_ui.GetWidget<Text>("Label");
                m_pWidget0 = m_ui.GetWidget<InputField>("0");
                m_pWidget1 = m_ui.GetWidget<InputField>("1");
                m_pWidget2 = m_ui.GetWidget<InputField>("2");
                m_pWidget3 = m_ui.GetWidget<InputField>("3");
                if (m_pWidget0)
                {
                    m_pWidget0.onValueChanged.AddListener(OnWidgetCallback);
                    m_pWidget0.contentType = InputField.ContentType.DecimalNumber;
                    m_pWidget0.text = value.x.ToString();
                }
                if (m_pWidget1)
                {
                    m_pWidget1.onValueChanged.AddListener(OnWidgetCallback);
                    m_pWidget1.contentType = InputField.ContentType.DecimalNumber;
                    m_pWidget1.text = value.y.ToString();
                }
                if (m_pWidget2)
                {
                    m_pWidget2.onValueChanged.AddListener(OnWidgetCallback);
                    m_pWidget2.contentType = InputField.ContentType.DecimalNumber;
                    m_pWidget2.text = value.width.ToString();
                }
                if (m_pWidget3)
                {
                    m_pWidget3.onValueChanged.AddListener(OnWidgetCallback);
                    m_pWidget3.contentType = InputField.ContentType.DecimalNumber;
                    m_pWidget3.text = value.height.ToString();
                }
                if (pLabel) pLabel.text = label;
            }
            else
            {
                m_pWidget0 = null;
                m_pWidget1 = null;
                m_pWidget2 = null;
                m_pWidget3 = null;
            }
        }
        //------------------------------------------------------
        public UISerialized GetUI() { return m_ui; }
        public bool IsValid() { return m_ui != null; }
        //------------------------------------------------------
        public RectInt rectIntValue
        {
            get
            {
                RectInt temp = new RectInt();
                if (m_pWidget0 == null || m_pWidget1 == null || m_pWidget2 == null || m_pWidget3 == null) return temp;
                int tV;
                if (int.TryParse(m_pWidget0.text, out tV)) temp.x = tV;
                if (int.TryParse(m_pWidget1.text, out tV)) temp.y = tV;
                if (int.TryParse(m_pWidget2.text, out tV)) temp.width = tV;
                if (int.TryParse(m_pWidget3.text, out tV)) temp.height = tV;
                return temp;
            }
            set
            {
                if (m_pWidget0 == null || m_pWidget1 == null || m_pWidget2 == null || m_pWidget3 == null) return;
                m_pWidget0.text = value.x.ToString();
                m_pWidget1.text = value.y.ToString();
                m_pWidget2.text = value.width.ToString();
                m_pWidget3.text = value.height.ToString();
            }
        }
        //------------------------------------------------------
        public Vector4 floatValue
        {
            get
            {
                Vector4 temp = Vector4.zero;
                if (m_pWidget0 == null || m_pWidget1 == null || m_pWidget2 == null || m_pWidget3 == null) return temp;
                float tV;
                if (float.TryParse(m_pWidget0.text, out tV)) temp.x = tV;
                if (float.TryParse(m_pWidget1.text, out tV)) temp.y = tV;
                if (float.TryParse(m_pWidget2.text, out tV)) temp.z = tV;
                if (float.TryParse(m_pWidget3.text, out tV)) temp.w = tV;
                return temp;
            }
            set
            {
                if (m_pWidget0 == null || m_pWidget1 == null || m_pWidget2 == null || m_pWidget3 == null) return;
                m_pWidget0.text = value.x.ToString("F2");
                m_pWidget1.text = value.y.ToString("F2");
                m_pWidget2.text = value.z.ToString("F2");
                m_pWidget3.text = value.w.ToString("F2");
            }
        }
        //------------------------------------------------------
        public Rect rectValue
        {
            get
            {
                Rect temp = Rect.zero;
                if (m_pWidget0 == null || m_pWidget1 == null || m_pWidget2 == null || m_pWidget3 == null) return temp;
                float tV;
                if (float.TryParse(m_pWidget0.text, out tV)) temp.x = tV;
                if (float.TryParse(m_pWidget1.text, out tV)) temp.y = tV;
                if (float.TryParse(m_pWidget2.text, out tV)) temp.width = tV;
                if (float.TryParse(m_pWidget3.text, out tV)) temp.height = tV;
                return temp;
            }
            set
            {
                if (m_pWidget0 == null || m_pWidget1 == null || m_pWidget2 == null || m_pWidget3 == null) return;
                m_pWidget0.text = value.x.ToString("F2");
                m_pWidget1.text = value.y.ToString("F2");
                m_pWidget2.text = value.width.ToString("F2");
                m_pWidget3.text = value.height.ToString("F2");
            }
        }
        //------------------------------------------------------
        void OnWidgetCallback(string val)
        {
            OnWidgetCallback();
        }
        //------------------------------------------------------
        void OnWidgetCallback()
        {
            if (m_pCallback == null) return;
            if (m_pWidget0.contentType == InputField.ContentType.IntegerNumber)
            {
                Variable4 vec = new Variable4();
                int tV;
                if (int.TryParse(m_pWidget0.text, out tV)) vec.intVal0 = tV;
                if (int.TryParse(m_pWidget1.text, out tV)) vec.intVal1 = tV;
                if (int.TryParse(m_pWidget2.text, out tV)) vec.intVal2 = tV;
                if (int.TryParse(m_pWidget3.text, out tV)) vec.intVal3 = tV;
                m_pCallback(vec);

            }
            else if (m_pWidget0.contentType == InputField.ContentType.DecimalNumber)
            {
                Variable4 vec = new Variable4();
                float tV;
                if (float.TryParse(m_pWidget0.text, out tV)) vec.floatVal0 = tV;
                if (float.TryParse(m_pWidget1.text, out tV)) vec.floatVal1 = tV;
                if (float.TryParse(m_pWidget2.text, out tV)) vec.floatVal2 = tV;
                if (float.TryParse(m_pWidget3.text, out tV)) vec.floatVal3 = tV;
                m_pCallback(vec);
            }
        }
        //------------------------------------------------------
        public void Dispose()
        {
            if (m_pWidget0) m_pWidget0.onValueChanged.RemoveAllListeners();
            if (m_pWidget1) m_pWidget1.onValueChanged.RemoveAllListeners();
            if (m_pWidget2) m_pWidget2.onValueChanged.RemoveAllListeners();
            if (m_pWidget3) m_pWidget3.onValueChanged.RemoveAllListeners();
            if (m_ui)
            {
                EditorWidgetMgr.getInstance().RecyleWidget(EditorWidgetType.Value4, m_ui);
                m_ui = null;
            }
            m_pWidget0 = null;
            m_pWidget1 = null;
            m_pWidget2 = null;
            m_pWidget3 = null;
            m_pCallback = null;
        }
    }

    //------------------------------------------------------
    public struct EditorLayoutH : EditorWidget
    {
        UISerialized m_ui;
        HorizontalLayoutGroup m_pWidget;
        public EditorLayoutH(RectTransform pParent)
        {
            m_ui = EditorWidgetMgr.getInstance().CreateWidget(pParent, EditorWidgetType.LayoutH);
            if (m_ui)
            {
                m_pWidget = m_ui.GetWidget<HorizontalLayoutGroup>("self");
            }
            else
            {
                m_pWidget = null;
            }
        }
        //------------------------------------------------------
        public UISerialized GetUI() { return m_ui; }
        public bool IsValid() { return m_ui != null; }
        //------------------------------------------------------
        public void Dispose()
        {
            if (m_ui)
            {
                EditorWidgetMgr.getInstance().RecyleWidget(EditorWidgetType.LayoutH, m_ui);
                m_ui = null;
            }
            m_pWidget = null;
        }
    }
    //------------------------------------------------------
    public struct EditorLayoutV : EditorWidget
    {
        UISerialized m_ui;
        HorizontalLayoutGroup m_pWidget;
        public EditorLayoutV(RectTransform pParent)
        {
            m_ui = EditorWidgetMgr.getInstance().CreateWidget(pParent, EditorWidgetType.LayoutV);
            if (m_ui)
            {
                m_pWidget = m_ui.GetWidget<HorizontalLayoutGroup>("self");
            }
            else
            {
                m_pWidget = null;
            }
        }
        //------------------------------------------------------
        public UISerialized GetUI() { return m_ui; }
        public bool IsValid() { return m_ui != null; }
        //------------------------------------------------------
        public void Dispose()
        {
            if (m_ui)
            {
                EditorWidgetMgr.getInstance().RecyleWidget(EditorWidgetType.LayoutV, m_ui);
                m_ui = null;
            }
            m_pWidget = null;
        }
    }
    //------------------------------------------------------
    public struct EditorScrollView : EditorWidget
    {
        UISerialized m_ui;
        HorizontalLayoutGroup m_pWidget;
        public EditorScrollView(RectTransform pParent)
        {
            m_ui = EditorWidgetMgr.getInstance().CreateWidget(pParent, EditorWidgetType.ScrollView);
            if (m_ui)
            {
                m_pWidget = m_ui.GetWidget<HorizontalLayoutGroup>("self");
            }
            else
            {
                m_pWidget = null;
            }
        }
        //------------------------------------------------------
        public UISerialized GetUI() { return m_ui; }
        public bool IsValid() { return m_ui != null; }
        //------------------------------------------------------
        public void Dispose()
        {
            if (m_ui)
            {
                EditorWidgetMgr.getInstance().RecyleWidget(EditorWidgetType.ScrollView, m_ui);
                m_ui = null;
            }
            m_pWidget = null;
        }
    }
    //------------------------------------------------------
    public struct EditorDropdown : EditorWidget
    {
        UISerialized m_ui;
        Text m_pLabel;
        Dropdown m_pDropdown;
        System.Action<int> m_pCallback;
        public EditorDropdown(RectTransform pParent, string strLabel, string curValue, List<string> options, System.Action<int> onCallback =null)
        {
            m_pCallback = onCallback;
            m_pLabel = null;
            m_pDropdown = null;
            m_ui = EditorWidgetMgr.getInstance().CreateWidget(pParent, EditorWidgetType.Dropdown);
            if (m_ui)
            {
                m_pDropdown = m_ui.GetWidget<Dropdown>("Dropdown");
                m_pLabel = m_ui.GetWidget<Text>("Label");
            }
            if (m_pDropdown != null)
            {
                m_pDropdown.ClearOptions();
                m_pDropdown.value = options.IndexOf(curValue);
                m_pDropdown.onValueChanged.AddListener(OnValueChange);
                m_pDropdown.AddOptions(options);
            }
            if (m_pLabel) m_pLabel.text = strLabel;
        }
        //------------------------------------------------------
        public EditorDropdown(RectTransform pParent, string strLabel, int curValue, List<string> options, System.Action<int> onCallback = null)
        {
            m_pCallback = onCallback;
            m_pLabel = null;
            m_pDropdown = null;
            m_ui = EditorWidgetMgr.getInstance().CreateWidget(pParent, EditorWidgetType.Dropdown);
            if (m_ui)
            {
                m_pDropdown = m_ui.GetWidget<Dropdown>("Dropdown");
                m_pLabel = m_ui.GetWidget<Text>("Label");
            }
            if (m_pDropdown != null)
            {
                m_pDropdown.ClearOptions();
                m_pDropdown.value = curValue;
                m_pDropdown.onValueChanged.AddListener(OnValueChange);
                m_pDropdown.AddOptions(options);
            }
            if (m_pLabel) m_pLabel.text = strLabel;
        }
        //------------------------------------------------------
        public UISerialized GetUI() { return m_ui; }
        //------------------------------------------------------
        public bool IsValid()
        {
            return m_ui;
        }
        //------------------------------------------------------
        void OnValueChange(int value)
        {
            if (m_pDropdown && m_pCallback != null) m_pCallback(m_pDropdown.value);
        }
        //------------------------------------------------------
        public string strCurrent
        {
            get
            {
                if (m_pDropdown && m_pDropdown.value>=0 && m_pDropdown.options!=null && m_pDropdown.value< m_pDropdown.options.Count)
                    return m_pDropdown.options[m_pDropdown.value].text;
                return null;
            }
            set
            {
                if (m_pDropdown)
                {
                    if(m_pDropdown.options!=null)
                    {
                        for(int i =0; i< m_pDropdown.options.Count; ++i)
                        {
                            if(m_pDropdown.options[i].text.CompareTo(value) ==0)
                            {
                                m_pDropdown.value = i;
                                break;
                            }
                        }
                    }
                }
            }
        }
        //------------------------------------------------------
        public int Current
        {
            get
            {
                if (m_pDropdown)
                    return m_pDropdown.value;
                return -1;
            }
            set
            {
                if (m_pDropdown)
                {
                    m_pDropdown.value = value;
                }
            }
        }
        //------------------------------------------------------
        public void Dispose()
        {
            if (m_pDropdown) m_pDropdown.onValueChanged.RemoveAllListeners();
             m_pDropdown = null;
            m_pLabel = null;
            if (m_ui)
            {
                EditorWidgetMgr.getInstance().RecyleWidget(EditorWidgetType.Dropdown, m_ui);
                m_ui = null;
            }
        }
    }
    //------------------------------------------------------
    public struct EditorMessageBox : EditorWidget
    {
        UISerialized m_ui;
        Text m_pTitle;
        Text m_pContent;
        Button m_pOk;
        Button m_pCancel;
        Text m_pOkText;
        Text m_pCancelText;

        System.Action m_pOkCallback;
        System.Action m_pCancelCallback;
        public EditorMessageBox(RectTransform pParent)
        {
            m_pOkCallback = null;
            m_pCancelCallback = null;
            m_ui = EditorWidgetMgr.getInstance().CreateWidget(pParent, EditorWidgetType.MessageBox);
            if (m_ui)
            {
                m_pContent = m_ui.GetWidget<Text>("Content");
                m_pTitle = m_ui.GetWidget<Text>("Title");
                m_pCancelText = m_ui.GetWidget<Text>("CancelText");
                m_pOkText = m_ui.GetWidget<Text>("OkText");
                m_pCancel = m_ui.GetWidget<Button>("Cancel");
                m_pOk = m_ui.GetWidget<Button>("Ok");
                if (m_pOk)
                {
                    m_pOk.gameObject.SetActive(false);
                    m_pOk.onClick.AddListener(OnOkClick);
                }
                if (m_pCancel)
                {
                    m_pCancel.gameObject.SetActive(false);
                    m_pCancel.onClick.AddListener(OnOkCancel);
                }
            }
            else
            {
                m_pTitle = null;
                m_pContent = null;
                m_pOk = null;
                m_pCancel = null;
                m_pOkText = null;
                m_pCancelText = null;
            }
        }
        //------------------------------------------------------
        public UISerialized GetUI() { return m_ui; }
        //------------------------------------------------------
        public bool IsValid()
        {
            return m_ui;
        }
        //------------------------------------------------------
        public void SetOkText(string okText)
        {
            if (m_pOkText) m_pOkText.text = okText;
        }
        //------------------------------------------------------
        public void SetCancelText(string cancelText)
        {
            if (m_pCancelText) m_pCancelText.text = cancelText;
        }
        //------------------------------------------------------
        public void SetCallback(System.Action Ok, System.Action Cancel)
        {
            m_pOkCallback = Ok;
            m_pCancelCallback = Cancel;
        }
        //------------------------------------------------------
        public void Show()
        {
            if (m_ui)
            {
                m_ui.gameObject.SetActive(true);
                RectTransform rect = m_ui.transform as RectTransform;
                rect.localScale = Vector3.one;
                rect.anchoredPosition3D = Vector2.zero;
                rect.offsetMax = Vector3.zero;
                rect.offsetMin = Vector3.zero;
                rect.anchorMin = Vector2.zero;
                rect.anchorMax = Vector2.one;
                UIKits.PlayTween(m_ui, true);
            }
        }
        //------------------------------------------------------
        public void Hide()
        {
            if (m_ui)
            {
                m_ui.gameObject.SetActive(false);
            }
        }
        //------------------------------------------------------
        public void ShowCancel(bool bShow)
        {
            if (m_pCancel) m_pCancel.gameObject.SetActive(bShow);
        }
        //------------------------------------------------------
        public void ShowOk(bool bShow)
        {
            if (m_pOk) m_pOk.gameObject.SetActive(bShow);
        }
        //------------------------------------------------------
        void OnOkClick()
        {
            if (m_pOkCallback != null) m_pOkCallback();
            Hide();
        }
        //------------------------------------------------------
        void OnOkCancel()
        {
            if (m_pCancelCallback != null) m_pCancelCallback();
            Hide();
        }
        //------------------------------------------------------
        public string Title
        {
            get
            {
                if (m_pTitle) return m_pTitle.text;
                return null;
            }
            set
            {
                if (m_pTitle) m_pTitle.text = value;
            }
        }
        //------------------------------------------------------
        public string Content
        {
            get
            {
                if (m_pContent) return m_pContent.text;
                return null;
            }
            set
            {
                if (m_pContent) m_pContent.text = value;
            }
        }
        //------------------------------------------------------
        public void Dispose()
        {
            if (m_pOk)
                m_pOk.onClick.RemoveAllListeners();
            if (m_pCancel)
                m_pOk.onClick.RemoveAllListeners();
            if (m_ui)
            {
                EditorWidgetMgr.getInstance().RecyleWidget(EditorWidgetType.MessageBox, m_ui);
                m_ui = null;
            }
            m_pTitle = null;
            m_pContent = null;
            m_pOk = null;
            m_pCancel = null;
            m_pCancelText = null;
            m_pOkText = null;
        }
    }
}