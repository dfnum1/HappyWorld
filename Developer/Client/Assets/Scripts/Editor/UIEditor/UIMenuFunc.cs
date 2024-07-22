using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TopGame.Base;
using TopGame.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TopGame.ED
{
    public static class UIMenuFunc
    {
        static public bool AddCommonItems(GameObject[] targets)
        {
            bool bCan = false;
            ContextMenu.AddItem("定位界面根节点", false, FintUserInterface);

            UI.UserInterface[] users = GameObject.FindObjectsOfType<UI.UserInterface>();
            for (int i=0;i<users.Length;i++)
            {
                if (users[i].GetType() == typeof(UI.UserInterface))
                {
                    ContextMenu.AddItemWithArge("导出预制体", false, UIEditorHelper.ExportUIPrefab, users[i]);
                    ContextMenu.AddItemWithArge("导出预制体(置空错误引用)", false, UIEditorHelper.ExportUIPrefabWithEmptyWrongRef, users[i]);
                    
                    break;
                }
            }

            if (targets == null || targets.Length <=0)
            {
                ContextMenu.AddItem("新建", false, NewUserInterface);
                ContextMenu.AddItem("打开界面", false, OpenUserInterface);
                ContextMenu.AddItem("打开文件夹", false, OpenUserInterfaceByFolder);
                bCan = true;
            }

            if (targets.Length == 1)
            {
                UI.UserInterface user = targets[0].GetComponent<UI.UserInterface>();
                if(user!=null)
                {
                    UI.UIEditorTemper editor = targets[0].GetComponent<UI.UIEditorTemper>();
                    if (editor == null) editor = targets[0].AddComponent<UI.UIEditorTemper>();
                    ContextMenu.AddItemWithArge("添加参考图", false, UIEditorHelper.CreateReferToPic, editor);
                    //ContextMenu.AddItemWithArge("导出预制体", false, UIEditorHelper.ExportUIPrefab, user);

                    if (user.GetComponent<GraphicRaycaster>())
                        ContextMenu.AddItemWithArge("界面/不可交互", false, NoRayHitUI, user);
                    else
                        ContextMenu.AddItemWithArge("界面/可交互", false, RayHitUI, user);
                    if (user.gameObject.hideFlags == HideFlags.NotEditable)
                        ContextMenu.AddItemWithArge("界面/解锁", false, UnLockWidget, user);
                    else
                        ContextMenu.AddItemWithArge("界面/锁定", false, LockWidget, user);
                    ContextMenu.AddItemWithArge("界面/自适配", false, AdpterView, editor);
                }

                UI.TabBox tab = targets[0].GetComponent<UI.TabBox>();
                if(tab!=null)
                {
                    ContextMenu.AddItemWithArge("TabBox/布局预览", false, PreviewTabBox, tab);
                }

                bCan = true;
            }
            if (targets != null  && targets.Length >0)
            {
                ContextMenu.AddItem("界面/保存", false, SaveUserInteface);
                ContextMenu.AddItem("界面/另存为", false, SaveAsUserInteface);
             //   ContextMenu.AddItem("界面/重新加载", false, ReloadUserInteface);

                ContextMenu.AddSeparator("///");
                if (targets.Length > 1)
                {
                    AddAlignMenu();
                    ContextMenu.AddItem("并组", false, MakeGroup);
                }
                if (targets.Length == 1)
                {
                    AddUIMenu();
                    AddUIComponentMenu();
                    AddPriorityMenu();

                    if (UIEditorHelper.IsNodeCanDivide(targets[0]))
                        ContextMenu.AddItem("解散组", false, UnGroup);

                    AddShowOrHideMenu();
                    AddCopyMenu();
                    ContextMenu.AddSeparator("///");

                    if (targets.Length == 1 && targets[0].transform.childCount > 0)
                        ContextMenu.AddItem("优化层级", false, UIEditorHelper.OptimizeBatchForMenu);
                }
                bCan = true;
            }
            bCan = true;
            ContextMenu.AddItem("排序所有界面", false, ResortAllUserInterface);
            ContextMenu.AddItem("清空界面", false, ClearAllUserInterface);

            if (bCan)
                ContextMenu.Show();
            return bCan;
        }
        //------------------------------------------------------
        static public void AddAlignMenu()
        {
            ContextMenu.AddItem("对齐/左对齐 ←", false, UIAlignTool.AlignInHorziontalLeft);
            ContextMenu.AddItem("对齐/右对齐 →", false, UIAlignTool.AlignInHorziontalRight);
            ContextMenu.AddItem("对齐/上对齐 ↑", false, UIAlignTool.AlignInVerticalUp);
            ContextMenu.AddItem("对齐/下对齐 ↓", false, UIAlignTool.AlignInVerticalDown);
            ContextMenu.AddItem("对齐/水平均匀 |||", false, UIAlignTool.UniformDistributionInHorziontal);
            ContextMenu.AddItem("对齐/垂直均匀 ☰", false, UIAlignTool.UniformDistributionInVertical);
            ContextMenu.AddItem("对齐/一样大 ■", false, UIAlignTool.ResizeMax);
            ContextMenu.AddItem("对齐/一样小 ●", false, UIAlignTool.ResizeMin);
        }
        //------------------------------------------------------
        static public void AddPriorityMenu()
        {
            ContextMenu.AddItem("层级/置底", false, PriorityTool.MoveToTopWidget);
            ContextMenu.AddItem("层次/置顶", false, PriorityTool.MoveToBottomWidget);
            ContextMenu.AddItem("层次/向上提一层", false, PriorityTool.MoveUpWidget);
            ContextMenu.AddItem("层次/向底降一层 ", false, PriorityTool.MoveDownWidget);
        }
        //------------------------------------------------------
        static public void AddUIMenu()
        {
            ContextMenu.AddItem("添加控件/Empty", false, UIEditorHelper.CreateEmptyObj);
            ContextMenu.AddItem("添加控件/Image(绑定)", false, UIEditorHelper.CreateImageObj);
            ContextMenu.AddItem("添加控件/Image(非绑定)", false, UIEditorHelper.CreateStaticImageObj);
            ContextMenu.AddItem("添加控件/RawImage(绑定)", false, UIEditorHelper.CreateRawImageObj);
            ContextMenu.AddItem("添加控件/RawImage(非绑定)", false, UIEditorHelper.CreateStaticRawImageObj);
            ContextMenu.AddItem("添加控件/Button", false, UIEditorHelper.CreateButtonObj);
            ContextMenu.AddItem("添加控件/Text(绑定)", false, UIEditorHelper.CreateTextObj);
            ContextMenu.AddItem("添加控件/Text(非绑定)", false, UIEditorHelper.CreateStaticTextObj);
            ContextMenu.AddItem("添加控件/HScroll", false, UIEditorHelper.CreateHScrollViewObj);
            ContextMenu.AddItem("添加控件/HScroll2", false, UIEditorHelper.CreateHScrollView2Obj);
            ContextMenu.AddItem("添加控件/VScroll", false, UIEditorHelper.CreateVScrollViewObj);
            ContextMenu.AddItem("添加控件/VScroll2", false, UIEditorHelper.CreateVScrollView2Obj);
            ContextMenu.AddItem("添加控件/DynamicSV", false, UIEditorHelper.CreateDynamicScrollView);
            ContextMenu.AddItem("添加控件/多按钮单选", false, UIEditorHelper.CreateSingleSelectBtnMgr);
            ContextMenu.AddItem("添加控件/左右滑动事件面板", false, UIEditorHelper.CreateSwapablePanel);
            ContextMenu.AddItem("添加控件/Dropdown控件", false, UIEditorHelper.CreateDropdown);
            ContextMenu.AddItem("添加控件/Slider控件", false, UIEditorHelper.CreateSlider);
        }
        //------------------------------------------------------
        static public void AddUIComponentMenu()
        {
            ContextMenu.AddItem("添加组件/Image", false, UIEditorHelper.AddImageComponent);
            ContextMenu.AddItem("添加组件/HLayout", false, UIEditorHelper.AddHorizontalLayoutComponent);
            ContextMenu.AddItem("添加组件/VLayout", false, UIEditorHelper.AddVerticalLayoutComponent);
            ContextMenu.AddItem("添加组件/HTabBox", false, UIEditorHelper.AddHTabBoxComponent);
            ContextMenu.AddItem("添加组件/VTabBox", false, UIEditorHelper.AddVTabBoxComponent);
            ContextMenu.AddItem("添加组件/GridLayout", false, UIEditorHelper.AddGridLayoutGroupComponent);
            ContextMenu.AddItem("添加组件/GridBoxMgr", false, UIEditorHelper.AddGridBoxMgrComponent);
            ContextMenu.AddItem("添加组件/SingleSelectBtnMgr", false, UIEditorHelper.AddSingleSelectBtnMgrComponent);
            ContextMenu.AddItem("添加组件/GallerySlider", false, UIEditorHelper.AddGallerySliderComponent);
        }
        //------------------------------------------------------
        static public void AddCopyMenu()
        {
            ContextMenu.AddItem("复制", false, MenuCopy);
            ContextMenu.AddItem("粘贴", false, MenuPaste);
        }

        public static GameObject copyObjInBoard;
        public static int copyIdx = 0;
        static public void MenuCopy()
        {
            if (Selection.activeTransform && Selection.activeTransform.GetComponentInParent<Canvas>())
            {
                copyObjInBoard = Selection.activeTransform.gameObject;
            }
        }        
        //------------------------------------------------------

        static public void MenuPaste()
        {
            if (copyObjInBoard == null) return;
            if (Selection.activeTransform && Selection.activeTransform.GetComponentInParent<Canvas>())
            {
                GameObject copy = GameObject.Instantiate(copyObjInBoard);
                copy.name = copy.name.Replace("(Clone)","");
                Transform[] childs = copy.GetComponentsInChildren<Transform>(true);
                for (int i=0;i<childs.Length;i++)
                {
                    childs[i].name += "_c"+ copyIdx;
                }

                copy.transform.SetParent(Selection.activeTransform.parent, false);
                copyIdx++;
            }
        }

        //------------------------------------------------------
        static public void AddShowOrHideMenu()
        {
            bool hasHideWidget = false;
            foreach (var item in Selection.gameObjects)
            {
                if (!item.activeSelf)
                {
                    hasHideWidget = true;
                    break;
                }
            }
            if (hasHideWidget)
                ContextMenu.AddItem("显示", false, UIEditorHelper.ShowAllSelectedWidgets);
            else
                ContextMenu.AddItem("隐藏", false, UIEditorHelper.HideAllSelectedWidgets);
        }
        //------------------------------------------------------
        public static void UnGroup()
        {
            if (Selection.gameObjects == null || Selection.gameObjects.Length <= 0)
            {
                EditorUtility.DisplayDialog("Error", "当前没有选中节点", "Ok");
                return;
            }
            if (Selection.gameObjects.Length > 1)
            {
                EditorUtility.DisplayDialog("Error", "只能同时解除一个Box", "Ok");
                return;
            }
            GameObject target = Selection.activeGameObject;
            Transform new_parent = target.transform.parent;
            if (target.transform.childCount > 0)
            {
                Transform[] child_ui = target.transform.GetComponentsInChildren<Transform>(true);
                foreach (var item in child_ui)
                {
                    //不是自己的子节点或是自己的话就跳过
                    if (item.transform.parent != target.transform || item.transform == target.transform)
                        continue;

                    item.transform.SetParent(new_parent, true);
                }
                Undo.DestroyObjectImmediate(target);
                //GameObject.DestroyImmediate(target);
            }
            else
            {
                EditorUtility.DisplayDialog("Error", "选择对象容器控件", "Ok");
            }
        }
        //------------------------------------------------------
        public static void MakeGroup()
        {
            if (Selection.gameObjects == null || Selection.gameObjects.Length <= 0)
            {
                EditorUtility.DisplayDialog("Error", "当前没有选中节点", "Ok");
                return;
            }
            //先判断选中的节点是不是挂在同个父节点上的
            Transform parent = Selection.gameObjects[0].transform.parent;
            foreach (var item in Selection.gameObjects)
            {
                if (item.transform.parent != parent)
                {
                    EditorUtility.DisplayDialog("Error", "不能跨容器组合", "Ok");
                    return;
                }
            }
            GameObject box = new GameObject("container", typeof(RectTransform));
            Undo.IncrementCurrentGroup();
            int group_index = Undo.GetCurrentGroup();
            Undo.SetCurrentGroupName("Make Group");
            Undo.RegisterCreatedObjectUndo(box, "create group object");
            RectTransform rectTrans = box.GetComponent<RectTransform>();
            if (rectTrans != null)
            {
                Vector2 left_top_pos = new Vector2(99999, -99999);
                Vector2 right_bottom_pos = new Vector2(-99999, 99999);
                foreach (var item in Selection.gameObjects)
                {
                    Bounds bound = UIEditorHelper.GetBounds(item);
                    Vector3 boundMin = item.transform.parent.InverseTransformPoint(bound.min);
                    Vector3 boundMax = item.transform.parent.InverseTransformPoint(bound.max);
                    //Debug.Log("bound : " + boundMin.ToString() + " max:" + boundMax.ToString());
                    if (boundMin.x < left_top_pos.x)
                        left_top_pos.x = boundMin.x;
                    if (boundMax.y > left_top_pos.y)
                        left_top_pos.y = boundMax.y;
                    if (boundMax.x > right_bottom_pos.x)
                        right_bottom_pos.x = boundMax.x;
                    if (boundMin.y < right_bottom_pos.y)
                        right_bottom_pos.y = boundMin.y;
                }
                rectTrans.SetParent(parent);
                rectTrans.sizeDelta = new Vector2(right_bottom_pos.x - left_top_pos.x, left_top_pos.y - right_bottom_pos.y);
                left_top_pos.x += rectTrans.sizeDelta.x / 2;
                left_top_pos.y -= rectTrans.sizeDelta.y / 2;
                rectTrans.localPosition = left_top_pos;
                rectTrans.localScale = Vector3.one;

                //需要先生成好Box和设置好它的坐标和大小才可以把选中的节点挂进来，注意要先排好序，不然层次就乱了
                GameObject[] sorted_objs = Selection.gameObjects.OrderBy(x => x.transform.GetSiblingIndex()).ToArray();
                for (int i = 0; i < sorted_objs.Length; i++)
                {
                    Undo.SetTransformParent(sorted_objs[i].transform, rectTrans, "move item to group");
                }
            }
            Selection.activeGameObject = box;
            Undo.CollapseUndoOperations(group_index);
        }
        //------------------------------------------------------

        static void NewUserInterface()
        {
            UnityEngine.SceneManagement.Scene pScene = UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene();
            if (pScene.isDirty)
            {
                if (EditorUtility.DisplayDialog("提示", "是否要保存和离开当前场景,并创建新界面场景", "确定", "不保存直接创建"))
                {
                    if (pScene.path == "")
                    {
                        string savePath = EditorUtility.SaveFilePanel("保存当前场景",UIEditorConfig.UIScenesPath,"","unity");
                        if (savePath != "")
                        {
                            EditorSceneManager.SaveScene(pScene, savePath);
                            CreateNewScene();
                        }
                    }
                    else
                    {
                        EditorSceneManager.SaveScene(pScene,pScene.path);
                        CreateNewScene();
                    }
                }
                else
                {
                    CreateNewScene();
                }
            }
            else
            {
                CreateNewScene();
            }
        }
        public static void CreateNewScene()
        {
            string newScenePath = EditorUtility.SaveFilePanel("保存新场景", UIEditorConfig.UIScenesPath, "", "unity").Replace("\\", "/");
            if (newScenePath != "")
            {
                EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects);
                UnityEngine.SceneManagement.Scene pScene = UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene();
                EditorSceneManager.SaveScene(pScene, newScenePath);

                GameObject root = new GameObject(pScene.name);
                UserInterface ui = root.AddComponent<UserInterface>();
                root.AddComponent<UIEditorTemper>().ResetData();
                UIEditor.Instance.SetNewInterface(ui);

                GameObject mask = GameObject.Find("AdapterMask");
                if (mask)
                    mask.gameObject.SetActive(false);
            }
        }
        //------------------------------------------------------
        public static void SaveNewScene()
        {
            EditorApplication.ExecuteMenuItem("File/New Scene");
            EditorApplication.ExecuteMenuItem("File/Save");

            UnityEngine.SceneManagement.Scene pScene = UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene();

            GameObject root = new GameObject(pScene.name);
            UserInterface ui = root.AddComponent<UserInterface>();
            root.AddComponent<UIEditorTemper>().ResetData();
            UIEditor.Instance.SetNewInterface(ui);
        }

        //------------------------------------------------------

        static void OpenUserInterface()
        {
            string strPath = EditorUtility.OpenFilePanel("打开界面", UIEditorConfig.GetLastPath(UIEditorPathType.SceneSave), "unity").Replace("\\", "/");
            if (strPath.Length <= 0) return;
            if (!strPath.Contains(Application.dataPath))
            {
                EditorUtility.DisplayDialog("提示", "需要在[" + Application.dataPath + "]子路径中打开!!", "好的");
                return;
            }
            strPath = strPath.Replace(Application.dataPath, "Assets");
            UIEditorConfig.SetLastPath(UIEditorPathType.SceneSave, strPath);
            UnityEngine.SceneManagement.Scene curScene = UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene();
            if (curScene.path.CompareTo(strPath) == 0) return;

            UnityEngine.SceneManagement.Scene scene = UnityEditor.SceneManagement.EditorSceneManager.OpenScene(strPath, UnityEditor.SceneManagement.OpenSceneMode.Single);
            if(!scene.IsValid())
            {
                if(curScene.IsValid())
                    UnityEditor.SceneManagement.EditorSceneManager.OpenScene(curScene.path, UnityEditor.SceneManagement.OpenSceneMode.Single);
                EditorUtility.DisplayDialog("提示", "无效的UI场景!", "好的");
                return;
            }

            GameObject mask = GameObject.Find("AdapterMask");
            if (mask)
                mask.gameObject.SetActive(false);
        }
        //------------------------------------------------------
        static void OpenUserInterfaceByFolder()
        {
 
        }
        //------------------------------------------------------
        static void FintUserInterface()
        {
            UI.UserInterface[] user = GameObject.FindObjectsOfType<UI.UserInterface>();
            if(user.Length>0)
            {
                Selection.activeObject = user[0].gameObject;
            }
        }
        //------------------------------------------------------
        static void SaveUserInteface()
        {
            EditorApplication.ExecuteMenuItem("File/Save");
        }
        //------------------------------------------------------
        static void SaveAsUserInteface()
        {
            EditorApplication.ExecuteMenuItem("File/Save As...");
        }
        //------------------------------------------------------
        static void ReloadUserInteface()
        {

        }
        //------------------------------------------------------
        public static void ResortAllUserInterface()
        {
            Object[] uis = GameObject.FindObjectsOfType(typeof(UI.UserInterface));
            if (uis != null && uis.Length>0)
            {
                SceneView.lastActiveSceneView.MoveToView((uis[0] as UI.UserInterface).transform);

                RectTransform first_trans = (uis[0] as UI.UserInterface).transform as RectTransform;
                Vector2 startPos = new Vector2(first_trans.sizeDelta.x * first_trans.localScale.x / 2, -first_trans.sizeDelta.y * first_trans.localScale.y / 2);
                int index = 0;
                foreach (var item in uis)
                {
                    int row = index / 5;
                    int col = index % 5;
                    RectTransform rectTrans = (item as UI.UserInterface).transform as RectTransform;
                    Vector3 pos = new Vector3((rectTrans.sizeDelta.x * rectTrans.localScale.x) * col + startPos.x, (-rectTrans.sizeDelta.y * rectTrans.localScale.y) * row + startPos.y, 0);
                    (item as UI.UserInterface).transform.localPosition = pos;
                    index++;
                }
            }
        }
        //------------------------------------------------------
        public static void ClearAllUserInterface()
        {
            bool isDeleteAll = EditorUtility.DisplayDialog("警告", "是否清空掉所有界面？", "是的", "再想想");
            if (isDeleteAll)
            {
                Object[] uis = GameObject.FindObjectsOfType(typeof(UI.UserInterface));
                if (uis != null)
                {
                    for(int i = 0; i < uis.Length; ++i)
                    {
                        UI.UserInterface ui = uis[i] as UI.UserInterface;
                        Undo.DestroyObjectImmediate(ui.gameObject);
                    }
                }
            }
        }
        //------------------------------------------------------
        static void RayHitUI(object arg)
        {
            UI.UserInterface user = arg as UI.UserInterface;
            if (user.GetComponent<GraphicRaycaster>() == null)
                user.gameObject.AddComponent< GraphicRaycaster > ();
        }
        //------------------------------------------------------
        static void NoRayHitUI(object arg)
        {
            UI.UserInterface user = arg as UI.UserInterface;
            GraphicRaycaster it = user.GetComponent<GraphicRaycaster>();
            if (it != null)
                GameObject.DestroyImmediate(it);
        }
        //------------------------------------------------------
        public static void AdpterView(object arg)
        {
            UI.UIEditorTemper user = arg as UI.UIEditorTemper;
            if (user)
                user.ResetData();
        }
        //------------------------------------------------------
        public static void LockWidget(object arg)
        {
            UI.UserInterface user = arg as UI.UserInterface;
            if (Selection.gameObjects.Length > 0)
            {
                user.gameObject.hideFlags = HideFlags.NotEditable;
            }
        }
        //------------------------------------------------------
        public static void UnLockWidget(object arg)
        {
            UI.UserInterface user = arg as UI.UserInterface;
            if (user)
            {
                user.gameObject.hideFlags = HideFlags.None;
            }
        }
        //------------------------------------------------------
        public static void PreviewTabBox(object arg)
        {
            UI.TabBox user = arg as UI.TabBox;
            TabBoxEditor.Preview(user, true);
        }
    }

}

