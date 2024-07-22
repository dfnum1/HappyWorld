// /********************************************************************
// 生成日期:	2022-3-10
// 类    名: 	HierarchyEditor
// 作    者:	zdq
// 描    述:	Hierarchy 面板绘制
// *********************************************************************/
// 
// #if UNITY_EDITOR
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEditor;
// using System;
// using TopGame.UI;
// using UnityEditor.SceneManagement;
// using UnityEngine.SceneManagement;
// 
// namespace TopGame.ED
// {
//     [InitializeOnLoad]
//     public class HierarchyEditor
//     {
//         static Dictionary<int,GameObject> m_vUIs = null;
//         static Color m_NormalColor = Color.white;
//         public static Color m_SpecialColor = new Color(0.74f,0f,0.97f,0.41f);
//         static HierarchyEditor()
//         {
//             EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyWindowItemOnGUI;
//             EditorSceneManager.sceneOpened += OnsceneOpened;
//             EditorSceneManager.sceneDirtied += OnsceneDirtied;
//             if (m_vUIs == null)
//             {
//                 GetSerializeUI();
//             }
//             //UnityEngine.Debug.Log("HierarchyEditor");
//         }
//         //------------------------------------------------------
//         ~HierarchyEditor() 
//         {
//             UnityEngine.Debug.Log("~HierarchyEditor~");
//         }
//         //------------------------------------------------------
//         private static void OnsceneDirtied(Scene scene)
//         {
//             //Debug.Log("OnsceneDirtied:" + scene.name);
//             GetSerializeUI();
//         }
// 
//         //------------------------------------------------------
//         private static void OnsceneOpened(Scene scene, OpenSceneMode mode)
//         {
//             //Debug.Log("Opened scene:" + scene.name);
//             GetSerializeUI();
//         }
//         //------------------------------------------------------
//         private static void OnHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
//         {
//             var go = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
//             if (go == null)
//                 return;
// 
//             
// 
//             //删除确认
//             if (Selection.objects.Length > 0 && Contain(Selection.objects,go))
//             {
//                 Event e = Event.current;
//                 if (e == null)
//                 {
//                     return;
//                 }
//                 if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Delete)
//                 {
//                     ////选择包括判断,如果删除的子物体包括记录的ui,需要提示
//                     bool isContain = false;
//                     foreach (GameObject item in Selection.objects)//多选支持
//                     {
//                         if (m_vUIs.ContainsKey(item.GetInstanceID()))
//                         {
//                             isContain = true;
//                             break;
//                         }
//                         else
//                         {
//                             if (item.transform.childCount > 0)
//                             {
//                                 foreach (Transform trans in item.transform)
//                                 {
//                                     if (m_vUIs.ContainsKey(trans.gameObject.GetInstanceID()))
//                                     {
//                                         isContain = true;
//                                         break;
//                                     }
//                                 }
//                             }
//                         }
//                     }
// 
// 
//                     if (isContain)//包含特定UI情况
//                     {
//                         if (EditorUtility.DisplayDialog("警告", String.Format("删除的物体:{0} 包含指定UI,确认删除?", Selection.objects[0].name), "确认", "取消"))
//                         {
//                             foreach (var item in Selection.objects)
//                             {
//                                 Undo.DestroyObjectImmediate(item);
//                             }
//                         }
//                     }
//                     else//不包含特定UI情况,正常删除
//                     {
//                         foreach (var item in Selection.objects)
//                         {
//                             Undo.DestroyObjectImmediate(item);
//                         }
//                     }
//                     e.Use();
//                 }
//             }
// 
//             //UI判断
//             if (m_vUIs == null || !m_vUIs.ContainsKey(instanceID))
//             {
//                 return;
//             }
// 
//             //绘制背景
//             GUI.backgroundColor = m_SpecialColor;
//             GUI.Box(selectionRect, "");
//             GUI.backgroundColor = m_NormalColor;
// 
//             //GUI.color = Color.blue;
//             //GUI.backgroundColor = Color.green;
//             //GUI.contentColor = Color.yellow;
//         }
//         //------------------------------------------------------
//         /// <summary>
//         /// 获取SerializeUI列表
//         /// 目前存在问题就是无法及时刷新,在新增UI的时候
//         /// </summary>
//         static void GetSerializeUI()
//         {
//             if (m_vUIs == null)
//             {
//                 m_vUIs = new Dictionary<int, GameObject>();
//             }
//             m_vUIs.Clear();
//             
//             UISerialized[] serializeds = Resources.FindObjectsOfTypeAll<UISerialized>();//GameObject.FindObjectsOfTypeAll 会存在 serialized 如果默认状态是隐藏时,无法获取到的问题
//             if (serializeds == null)
//             {
//                 return;
//             }
// 
//             foreach (var serialized in serializeds)
//             {
//                 //Gameobject
//                 if (serialized.Elements != null)
//                 {
//                     foreach (var item in serialized.Elements)
//                     {
//                         if (item == null)
//                         {
//                             continue;
//                         }
//                         int instanceID = item.GetInstanceID();
//                         if (m_vUIs.ContainsKey(instanceID))
//                         {
//                             continue;
//                         }
//                         m_vUIs.Add(instanceID, item);
//                     }
//                 }
// 
//                 //Widget
//                 if (serialized.Widgets != null)
//                 {
//                     foreach (var item in serialized.Widgets)
//                     {
//                         if (item.widget == null)
//                         {
//                             continue;
//                         }
//                         int instanceID = item.widget.gameObject.GetInstanceID();
//                         if (m_vUIs.ContainsKey(instanceID))
//                         {
//                             continue;
//                         }
//                         m_vUIs.Add(instanceID, item.widget.gameObject);
//                     }
//                 }
//                 
//             }
//         }
//         //------------------------------------------------------
//         static bool Contain(object[] select,GameObject go)
//         {
//             if (select == null || select.Length ==0 || go == null)
//             {
//                 return false;
//             }
// 
//             foreach (var item in select)
//             {
//                 if (item is GameObject && (GameObject)item == go)
//                 {
//                     return true;
//                 }
//             }
//             return false;
//         }
//     }
// }
// #endif
