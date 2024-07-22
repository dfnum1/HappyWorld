#if UNITY_EDITOR
/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	GameSystem
作    者:	HappLI
描    述:	UI 模版
*********************************************************************/

using Framework.Core;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace TopGame.UI
{
    public class UIThemeMenu
    {
        private static HashSet<int> ms_vSerilaizeSets = new HashSet<int>();
        static Color SPECIALCOLOR = new Color(0.74f, 0f, 0.97f, 0.41f);
        static Color PRO_SPECIALCOLOR = new Color(1f, 0,0, 0.48f);
        [InitializeOnLoadMethod]
        static void StartInitializeOnLoadMethod()
        {
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
            EditorSceneManager.sceneOpened += OnsceneOpened;
            EditorSceneManager.sceneDirtied += OnsceneDirtied;
            EditorSceneManager.sceneSaved += OnsceneDirtied;
        }
        //------------------------------------------------------
        private static void OnsceneDirtied(Scene scene)
        {
            RefreshSerializeUI();
        }
        //------------------------------------------------------
        private static void OnsceneOpened(Scene scene, OpenSceneMode mode)
        {
            RefreshSerializeUI();
        }
        //------------------------------------------------------
        static void OnHierarchyGUI(int instanceID, Rect selectionRect)
        {
            Event evt = Event.current;
            if (evt == null) return;
            if(evt.type == EventType.KeyDown && evt.keyCode == KeyCode.Delete)
            {
                bool isContain = false;
                foreach (GameObject item in Selection.objects)//多选支持
                {
                    if (ms_vSerilaizeSets.Contains(item.GetInstanceID()))
                    {
                        isContain = true;
                        break;
                    }
                    else
                    {
                        if (item.transform.childCount > 0)
                        {
                            foreach (Transform trans in item.transform)
                            {
                                if (ms_vSerilaizeSets.Contains(trans.gameObject.GetInstanceID()))
                                {
                                    isContain = true;
                                    break;
                                }
                            }
                        }
                    }
                }
                if (isContain)//包含特定UI情况
                {
                    if (EditorUtility.DisplayDialog("警告", System.String.Format("删除的物体:{0} 包含指定UI,确认删除?", Selection.objects[0].name), "确认", "取消"))
                    {
                        foreach (var item in Selection.objects)
                        {
                            Undo.DestroyObjectImmediate(item);
                        }
                    }
                }
                else//不包含特定UI情况,正常删除
                {
                    foreach (var item in Selection.objects)
                    {
                        Undo.DestroyObjectImmediate(item);
                    }
                }
                evt.Use();
            }
            else if (selectionRect.Contains(evt.mousePosition) && evt.button == 2 && evt.type <= EventType.MouseUp )
            {
                GameObject selectedGameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
                if (selectedGameObject)
                {
                    Vector2 mousePosition = evt.mousePosition;

                    EditorUtility.DisplayPopupMenu(new Rect(mousePosition.x, mousePosition.y, 0, 0), "GameObject/", null);
                    evt.Use();
                }
            }

            if (ms_vSerilaizeSets.Count>0 && ms_vSerilaizeSets.Contains(instanceID))
            {
                if(EditorGUIUtility.isProSkin)
                {
                    Color color = GUI.backgroundColor;
                    GUI.backgroundColor = PRO_SPECIALCOLOR;
                    GUI.Box(selectionRect, "");
                    GUI.backgroundColor = color;
                }
                else
                {
                    Color color = GUI.backgroundColor;
                    GUI.backgroundColor = SPECIALCOLOR;
                    GUI.Box(selectionRect, "");
                    GUI.backgroundColor = color;
                }

            }
        }
        //------------------------------------------------------
        [MenuItem("GameObject/UI/控件模版")]
        public static void PopThemeWidetPrefab()
        {
            ThemeWidgetWindow.OpenWindow(Selection.activeTransform);
        }
        //------------------------------------------------------
        [MenuItem("Assets/添加到模版", true)]
        public static bool AddThemeCheck()
        {
            return Selection.gameObjects != null && Selection.gameObjects.Length>0;
        }
        //------------------------------------------------------
        [MenuItem("Assets/添加到模版")]
        public static void AddThemePrefab()
        {
            ThemeUIData themeData = ThemeUIData.Load();
            if (themeData == null) themeData = new ThemeUIData();
            int resule = 0;
            for(int i =0; i < Selection.gameObjects.Length; ++i)
            {
                GameObject prefab = Selection.gameObjects[i];
                if(themeData.AddPrefab(prefab)>0)
                {
                    resule = 1;
                }
            }
            if(resule>0)
            {
                themeData.Save();
                EditorUtility.DisplayDialog("提示", "成功添加为UI模版", "Ok");
            }
        }
        //------------------------------------------------------
        static void RefreshSerializeUI()
        {
            ms_vSerilaizeSets.Clear();

            AComSerialized[] serializeds = Resources.FindObjectsOfTypeAll<AComSerialized>();
            if (serializeds == null)
            {
                return;
            }

            foreach (var serialized in serializeds)
            {
                if(serialized is UISerialized)
                {
                    UISerialized uiSer = serialized as UISerialized;
                    if (uiSer.Elements != null)
                    {
                        foreach (var item in uiSer.Elements)
                        {
                            if (item != null)
                                ms_vSerilaizeSets.Add(item.GetInstanceID());
                        }
                    }
                }

                //Widget
                if (serialized.Widgets != null)
                {
                    foreach (var item in serialized.Widgets)
                    {
                        if (item.widget != null)
                            ms_vSerilaizeSets.Add(item.widget.gameObject.GetInstanceID());
                    }
                }
            }
        }
    }
}
#endif