/********************************************************************
生成日期:	6:15:2020 10:06
类    名: 	SingleSelectBtnMgr
作    者:	JaydenHe
描    述:	单选按钮
*********************************************************************/
using System.Collections;
using System.Collections.Generic;
using TopGame.UI;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Button;
using System.Linq;
using UnityEditor;
using System.Reflection;
using TopGame.Core;
using System;
using Framework.Core;

namespace TopGame.UI
{
#if UNITY_EDITOR
    [CustomEditor(typeof(SingleSelectBtnMgr), true)]
    public class UIMultiBtnEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();

            GUILayout.BeginHorizontal();
            GUILayout.Label("按钮数量:");
            SingleSelectBtnMgr mgr = target as SingleSelectBtnMgr;
            mgr.DefaultCnt = EditorGUILayout.IntField(mgr.DefaultCnt);

            if (GUILayout.Button("确定"))
            {
                Transform[] childs = mgr.GetComponentsInChildren<Transform>(true);
                for (int i = childs.Length - 1; i >= 0; i--)
                {
                    GameObject.DestroyImmediate(childs[i].gameObject);
                }
                mgr.LightBtns = new Button[mgr.DefaultCnt];
                mgr.DarkBtns = new Button[mgr.DefaultCnt];

                for (int i=0;i< mgr.DefaultCnt; i++)
                {
                    GameObject light = new GameObject(mgr.transform.name + "_Light"+i, typeof(Button));
                    light.AddComponent<RectTransform>();
                    light.AddComponent<CanvasRenderer>();
                    light.AddComponent<Image>();
                    light.transform.SetParent(mgr.transform);
                    light.transform.localPosition = new Vector3(160*i,0,0);
                    light.transform.localScale = Vector3.one;
                    GameObject lightBtnText = new GameObject(mgr.transform.name + "_LightText" + i, typeof(Text));
                    Text lightTxt = lightBtnText.GetComponent<Text>();
                    lightTxt.text = "按钮";
                    lightTxt.alignment = TextAnchor.MiddleCenter;
                    lightTxt.color = Color.black;
                    lightTxt.raycastTarget = false;

                    lightBtnText.transform.SetParent(light.transform);
                    lightBtnText.transform.localPosition = Vector3.zero;
                    lightBtnText.transform.localScale = Vector3.one;

                    GameObject dark = new GameObject(mgr.transform.name + "_Dark" + i, typeof(Button));
                    dark.AddComponent<RectTransform>();
                    dark.AddComponent<CanvasRenderer>();
                    dark.AddComponent<Image>();
                    dark.transform.SetParent(mgr.transform);
                    dark.transform.localPosition = new Vector3(160 * i, 0, 0);
                    dark.transform.localScale = Vector3.one;

                    GameObject darkBtnText = new GameObject(mgr.transform.name + "_DarkText" + i, typeof(Text));
                    Text darkTxt = darkBtnText.GetComponent<Text>();
                    darkTxt.text = "按钮";
                    darkTxt.alignment = TextAnchor.MiddleCenter;
                    darkTxt.color = Color.black;
                    darkTxt.raycastTarget = false;
                    darkBtnText.transform.SetParent(dark.transform);
                    darkBtnText.transform.localPosition = Vector3.zero;
                    darkBtnText.transform.localScale = Vector3.one;

                    mgr.LightBtns[i] = light.GetComponent<Button>();
                    mgr.DarkBtns[i] = dark.GetComponent<Button>();
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("清除"))
            {
                Transform[] childs = mgr.GetComponentsInChildren<Transform>(true);
                for (int i= childs.Length-1;i>=0; i--)
                {
                    GameObject.DestroyImmediate(childs[i].gameObject); 
                }
            }
            GUILayout.EndHorizontal();

            FieldInfo[] fields = target.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
            for (int i = 0; i < fields.Length; ++i)
            {
                if (fields[i] == null) return;
                if (fields[i].Name.CompareTo("DefaultCnt") == 0) continue;
                if (fields[i].Name.CompareTo("LightBtns") == 0) continue;
                if (fields[i].Name.CompareTo("DarkBtns") == 0) continue;
                if (fields[i].IsNotSerialized) continue;

                SerializedProperty prop = serializedObject.FindProperty(fields[i].Name);
                if (prop == null) return;
                EditorGUILayout.PropertyField(serializedObject.FindProperty(fields[i].Name), true);
            }

            serializedObject.ApplyModifiedProperties();
        }
     }
#endif

    public class SingleSelectBtnMgr : MonoBehaviour, VariablePoolAble
    {
        [System.NonSerialized]
        public int DefaultCnt = 2;

        public Button[] LightBtns = null;
        public Button[] DarkBtns = null;

        Dictionary<int, ButtonClickedEvent> lightMaps = new Dictionary<int, ButtonClickedEvent>();
        Dictionary<int, ButtonClickedEvent> darkMaps = new Dictionary<int, ButtonClickedEvent>();

        public System.Action<int> SelectBtnAction;

        private void Awake()
        {
            BindEvent();
        }

        private void BindEvent()
        {
            if (LightBtns == null || DarkBtns == null || LightBtns.Length == 0 || DarkBtns.Length == 0 || LightBtns.Length != DarkBtns.Length)
            {
                Framework.Plugin.Logger.Warning("按钮数量错误!");
                return;
            }

            for (int i = 0; i < LightBtns.Length; i++)
            {
                Button light = LightBtns[i];
                EventTriggerListener.Get(light.gameObject).onClick = (go, param) =>
                {
                    OnClickBtn(light);
                };
                Button dark = DarkBtns[i];
                EventTriggerListener.Get(dark.gameObject).onClick = (go,param) => {
                    OnClickBtn(dark);
                };
            }
        }
        //------------------------------------------------------
        private void OnClickBtn(Button go)
        {
            int idx = System.Array.FindIndex(LightBtns, p => p == go);
            if(idx == -1) idx = System.Array.FindIndex(DarkBtns, p => p == go);

            for (int i = 0; i < LightBtns.Length; i++)
            {
                LightBtns[i].gameObject.SetActive(i== idx);
                DarkBtns[i].gameObject.SetActive(i != idx);
            }

            if (SelectBtnAction != null) SelectBtnAction(idx);
        }

        //------------------------------------------------------
        public void LightAll()
        {
            for (int i = 0; i < LightBtns.Length; i++)
            {
                LightBtns[i].gameObject.SetActive(true);
                DarkBtns[i].gameObject.SetActive(false);
            }
        }
        //------------------------------------------------------
        public void UnlightAll()
        {
            for (int i = 0; i < LightBtns.Length; i++)
            {
                LightBtns[i].gameObject.SetActive(false);
                DarkBtns[i].gameObject.SetActive(true);
            }
        }
        //------------------------------------------------------
        public void Show(int idx)
        {
            for (int i = 0; i < LightBtns.Length; i++)
            {
                LightBtns[i].gameObject.SetActive(idx==i);
                DarkBtns[i].gameObject.SetActive(idx != i);
            }
        }
        //------------------------------------------------------
        public void SimulateClick(int idx)
        {
            Show(idx);
            SelectBtnAction?.Invoke(idx);
        }

        public void Destroy()
        {

        }
    }
}
