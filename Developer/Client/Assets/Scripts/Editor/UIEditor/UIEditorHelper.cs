using Framework.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TopGame.Base;
using TopGame.Core;
using TopGame.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using static TopGame.ED.PublishPanel;

namespace TopGame.ED
{
    public class UIEditorHelper
    {
        private static string DEF_TEXTURE_PATH = "Assets/DatasRef/UI/Textures/default/ui_empty.png";
        static public string GenMD5String(string str)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            str = System.BitConverter.ToString(md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(str)), 4, 8);
            return str.Replace("-", "");
        }

        public static bool IsNodeCanDivide(GameObject obj)
        {
            if (obj == null)
                return false;
            return obj.transform != null && obj.transform.childCount > 0 && obj.GetComponent<Canvas>() == null && obj.transform.parent != null && obj.transform.parent.GetComponent<Canvas>() == null;
        }

        public static string GenerateUniqueName(GameObject parent, string type)
        {
            Transform canvasRoot = parent.transform.GetComponentInParent<UISerialized>().transform;
            if (canvasRoot == null) return type;

            var widgets = canvasRoot.GetComponentsInChildren<RectTransform>();
            int test_num = widgets.Length;
            if (type == "Button") test_num -= 2;
            if (type == "ScrollView" || type == "ScrollView2" || type == "DynamicScrollView") test_num -= 9;

            return type + "_" + test_num;
        }

        static public void AddImageComponent()
        {
            if (Selection.activeGameObject == null)
                return;
            Image old_img = Selection.activeGameObject.GetComponent<Image>();
            if (old_img != null)
            {
                bool isOk = EditorUtility.DisplayDialog("警告", "该GameObject已经有Image组件了,你想替换吗?", "添加", "取消");
                if (!isOk)
                {
                    //Selection.activeGameObject.
                    return;
                }
            }
            Image img = Selection.activeGameObject.AddComponent<Image>();
            if (img != null)
                img.raycastTarget = false;
        }

        static public void AddHorizontalLayoutComponent()
        {
            if (Selection.activeGameObject == null)
                return;
            HorizontalLayoutGroup layout = Selection.activeGameObject.AddComponent<HorizontalLayoutGroup>();
            layout.childForceExpandWidth = false;
            layout.childForceExpandHeight = false;
            layout.childControlWidth = false;
            layout.childControlHeight = false;
        }

        static public void AddVerticalLayoutComponent()
        {
            if (Selection.activeGameObject == null)
                return;
            VerticalLayoutGroup layout = Selection.activeGameObject.AddComponent<VerticalLayoutGroup>();
            layout.childForceExpandWidth = false;
            layout.childForceExpandHeight = false;
            layout.childControlWidth = false;
            layout.childControlHeight = false;
        }

        static public void AddGridLayoutGroupComponent()
        {
            if (Selection.activeGameObject == null)
                return;
            GridLayoutGroup layout = Selection.activeGameObject.AddComponent<GridLayoutGroup>();
        }

        static public void AddSingleSelectBtnMgrComponent()
        {
            if (Selection.activeGameObject == null)
                return;
            UI.SingleSelectBtnMgr layout = Selection.activeGameObject.AddComponent<UI.SingleSelectBtnMgr>();
        }

        static public void AddGallerySliderComponent()
        {
            if (Selection.activeGameObject == null)
                return;
            UI.GallerySlider layout = Selection.activeGameObject.AddComponent<UI.GallerySlider>();
        }

        static public void AddGridBoxMgrComponent()
        {
            if (Selection.activeGameObject == null)
                return;
            UI.ListView layout = Selection.activeGameObject.AddComponent<UI.ListView>();
        }


        static public void AddHTabBoxComponent()
        {
            if (Selection.activeGameObject == null)
                return;
            UI.HTabBox layout = Selection.activeGameObject.AddComponent<UI.HTabBox>();
        }

        static public void AddVTabBoxComponent()
        {
            if (Selection.activeGameObject == null)
                return;
            UI.VTabBox layout = Selection.activeGameObject.AddComponent<UI.VTabBox>();
        }

        static public void CreateEmptyObj()
        {
            if (Selection.activeGameObject == null)
                return;
            GameObject go = new GameObject(GenerateUniqueName(Selection.activeGameObject, "GameObject"), typeof(RectTransform));
            go.transform.SetParent(GetGoodContainer(Selection.activeTransform), false);
            Selection.activeGameObject = go;
        }

        static public void CreateImageObj()
        {
            if (Selection.activeTransform && Selection.activeTransform.GetComponentInParent<Canvas>())
            {
                GameObject go = new GameObject(GenerateUniqueName(Selection.activeGameObject, "Image"), typeof(Image));
                Image img = go.GetComponent<Image>();
                img.raycastTarget = false;
                go.transform.SetParent(GetGoodContainer(Selection.activeTransform), false);
                Selection.activeGameObject = go;
                AutoBindUserInterface(img);
            }
        }

        static public void CreateSingleSelectBtnMgr()
        {
            if (Selection.activeTransform && Selection.activeTransform.GetComponentInParent<Canvas>())
            {
                GameObject go = new GameObject(GenerateUniqueName(Selection.activeGameObject, "SingleSelectBtns"), typeof(GameObject));
                go.transform.SetParent(GetGoodContainer(Selection.activeTransform), false);
                Selection.activeGameObject = go;
                go.AddComponent<RectTransform>();
                SingleSelectBtnMgr mgr = go.AddComponent<SingleSelectBtnMgr>();
                AutoBindUserInterface(mgr);
            }
        }
        static public void CreateDropdown()
        {
            if (Selection.activeTransform && Selection.activeTransform.GetComponentInParent<Canvas>())
            {
                Transform last_trans = Selection.activeTransform;
                bool isOk = EditorApplication.ExecuteMenuItem("GameObject/UI/Dropdown");
                if (isOk)
                {
                    Selection.activeGameObject.name = GenerateUniqueName(Selection.activeGameObject, "Dropdown");
                    Selection.activeTransform.SetParent(GetGoodContainer(last_trans), false);
                    Dropdown drop = Selection.activeGameObject.gameObject.GetComponent<Dropdown>();
                    AutoBindUserInterface(drop);
                }
            }
        }

        static public void CreateSlider()
        {
            if (Selection.activeTransform && Selection.activeTransform.GetComponentInParent<Canvas>())
            {
                Transform last_trans = Selection.activeTransform;
                bool isOk = EditorApplication.ExecuteMenuItem("GameObject/UI/Slider");
                if (isOk)
                {
                    Selection.activeGameObject.name = GenerateUniqueName(Selection.activeGameObject, "Slider");
                    Selection.activeTransform.SetParent(GetGoodContainer(last_trans), false);
                    Slider drop = Selection.activeGameObject.gameObject.GetComponent<Slider>();
                    AutoBindUserInterface(drop);
                }
            }
        }

        static public void CreateSwapablePanel()
        {
            if (Selection.activeTransform && Selection.activeTransform.GetComponentInParent<Canvas>())
            {
                GameObject go = new GameObject(GenerateUniqueName(Selection.activeGameObject, "EmptyImg"), typeof(GameObject));
                go.AddComponent<EmptyImage>();
                UISwapEvent swap = go.AddComponent<UISwapEvent>();
                go.transform.SetParent(GetGoodContainer(Selection.activeTransform), false);
                Selection.activeGameObject = go;
                AutoBindUserInterface(swap);
            }
        }

        static public void CreateStaticImageObj()
        {
            if (Selection.activeTransform && Selection.activeTransform.GetComponentInParent<Canvas>())
            {
                GameObject go = new GameObject(GenerateUniqueName(Selection.activeGameObject, "Image"), typeof(Image));
                go.GetComponent<Image>().raycastTarget = false;
                go.transform.SetParent(GetGoodContainer(Selection.activeTransform), false);
                Selection.activeGameObject = go;

            }
        }

        static public void CreateStaticRawImageObj()
        {
            if (Selection.activeTransform && Selection.activeTransform.GetComponentInParent<Canvas>())
            {
                GameObject go = new GameObject(GenerateUniqueName(Selection.activeGameObject, "RawImage"), typeof(RawImage));
                go.GetComponent<RawImage>().raycastTarget = false;
                go.transform.SetParent(GetGoodContainer(Selection.activeTransform), false);
                Selection.activeGameObject = go;

            }
        }

        static public void CreateRawImageObj()
        {
            if (Selection.activeTransform && Selection.activeTransform.GetComponentInParent<Canvas>())
            {
                GameObject go = new GameObject(GenerateUniqueName(Selection.activeGameObject, "RawImage"), typeof(RawImage));
                RawImage img = go.GetComponent<RawImage>();
                img.raycastTarget = false;
                go.transform.SetParent(GetGoodContainer(Selection.activeTransform), false);
                Selection.activeGameObject = go;
                AutoBindUserInterface(img);
            }
        }

        static public void CreateButtonObj()
        {
            if (Selection.activeTransform && Selection.activeTransform.GetComponentInParent<Canvas>())
            {
                Transform last_trans = Selection.activeTransform;
                bool isOk = EditorApplication.ExecuteMenuItem("GameObject/UI/Button");
                if (isOk)
                {
                    Button btn = Selection.activeGameObject.GetComponent<Button>();
                    btn.gameObject.AddComponent<EventTriggerListener>();
                    Text btnTxt = btn.transform.Find("Text").GetComponent<Text>();
                    btn.name = GenerateUniqueName(Selection.activeGameObject, "Button");
                    btnTxt.name = btn.name + "_Txt";
                    Selection.activeTransform.SetParent(GetGoodContainer(last_trans), false);
                    AutoBindUserInterface(btn);
                    AutoBindUserInterface(btnTxt);
                }
            }
        }

        static public void CreateTextObj()
        {
            if (Selection.activeTransform && Selection.activeTransform.GetComponentInParent<Canvas>())
            {
                GameObject go = new GameObject(GenerateUniqueName(Selection.activeGameObject, "Text"), typeof(Text));
                Text txt = go.GetComponent<Text>();
                txt.raycastTarget = false;
                txt.text = "I am a Text";
                go.transform.SetParent(GetGoodContainer(Selection.activeTransform), false);
                go.transform.localPosition = Vector3.zero;
                Selection.activeGameObject = go;
                AutoBindUserInterface(txt);
            }
        }

        static public void CreateStaticTextObj()
        {
            if (Selection.activeTransform && Selection.activeTransform.GetComponentInParent<Canvas>())
            {
                GameObject go = new GameObject(GenerateUniqueName(Selection.activeGameObject, "Text"), typeof(Text));
                Text txt = go.GetComponent<Text>();
                txt.raycastTarget = false;
                txt.text = "I am a Text";
                go.transform.SetParent(GetGoodContainer(Selection.activeTransform), false);
                go.transform.localPosition = Vector3.zero;
                Selection.activeGameObject = go;
            }
        }
        static public void CreateHScrollViewObj()
        {
            if (Selection.activeTransform && Selection.activeTransform.GetComponentInParent<Canvas>())
            {
                Transform last_trans = Selection.activeTransform;
                bool isOk = EditorApplication.ExecuteMenuItem("GameObject/UI/Scroll View");
                if (isOk)
                {
                    Selection.activeGameObject.name = GenerateUniqueName(Selection.activeGameObject, "ScrollView");
                    Selection.activeGameObject.transform.Find("Viewport/Content").name = Selection.activeGameObject.name + "_CNT";
                    Selection.activeGameObject.transform.Find("Viewport").name = Selection.activeGameObject.name + "_VP";

                    Selection.activeTransform.SetParent(GetGoodContainer(last_trans), false);
                    InitScrollView(true);
                }
            }
        }

        static public void CreateHScrollView2Obj()
        {
            if (Selection.activeTransform && Selection.activeTransform.GetComponentInParent<Canvas>())
            {
                Transform last_trans = Selection.activeTransform;
                bool isOk = EditorApplication.ExecuteMenuItem("GameObject/UI/Scroll View");
                if (isOk)
                {
                    Selection.activeGameObject.name = GenerateUniqueName(Selection.activeGameObject, "ScrollView2");
                    Selection.activeTransform.SetParent(GetGoodContainer(last_trans), false);
                    InitScrollView2(true);
                }
            }
        }

        static public void CreateVScrollViewObj()
        {
            if (Selection.activeTransform && Selection.activeTransform.GetComponentInParent<Canvas>())
            {
                Transform last_trans = Selection.activeTransform;
                bool isOk = EditorApplication.ExecuteMenuItem("GameObject/UI/Scroll View");
                if (isOk)
                {
                    Selection.activeGameObject.name = GenerateUniqueName(Selection.activeGameObject, "ScrollView");
                    Selection.activeTransform.SetParent(GetGoodContainer(last_trans), false);
                    InitScrollView(false);
                }
            }
        }


        static public void CreateVScrollView2Obj()
        {
            if (Selection.activeTransform && Selection.activeTransform.GetComponentInParent<Canvas>())
            {
                Transform last_trans = Selection.activeTransform;
                bool isOk = EditorApplication.ExecuteMenuItem("GameObject/UI/Scroll View");
                if (isOk)
                {
                    Selection.activeGameObject.name = GenerateUniqueName(Selection.activeGameObject, "ScrollView2");
                    Selection.activeTransform.SetParent(GetGoodContainer(last_trans), false);
                    InitScrollView2(false);
                }
            }
        }


        static public void CreateDynamicScrollView()
        {
            if (Selection.activeTransform && Selection.activeTransform.GetComponentInParent<Canvas>())
            {
                Transform last_trans = Selection.activeTransform;
                bool isOk = EditorApplication.ExecuteMenuItem("GameObject/UI/Scroll View");
                if (isOk)
                {
                    Selection.activeGameObject.name = GenerateUniqueName(Selection.activeGameObject, "DynamicScrollView");
                    Selection.activeTransform.SetParent(GetGoodContainer(last_trans), false);
                    InitDynamicScrollView();
                }
            }

        }

        static private void InitScrollView2(bool isHorizontal)
        {
            ScrollRect scroll = Selection.activeTransform.GetComponent<ScrollRect>();
            if (scroll == null)
                return;
            //scroll.gameObject.AddComponent<Image>();
            scroll.gameObject.AddComponent<Mask>();

            RectTransform scrollRect = scroll.GetComponent<RectTransform>();
            UI.UIKits.SetAnchor(scrollRect, AnchorPresets.StretchAll);

            //Image img = Selection.activeTransform.GetComponent<Image>();
            //if (img != null)
            //    UnityEngine.Object.DestroyImmediate(img);
            scroll.horizontal = isHorizontal;
            scroll.vertical = !isHorizontal;
            scroll.horizontalScrollbar = null;
            scroll.verticalScrollbar = null;

            Transform horizontalObj = Selection.activeTransform.Find("Scrollbar Horizontal");
            if (horizontalObj != null)
                GameObject.DestroyImmediate(horizontalObj.gameObject);
            Transform verticalObj = Selection.activeTransform.Find("Scrollbar Vertical");
            if (verticalObj != null)
                GameObject.DestroyImmediate(verticalObj.gameObject);

            RectTransform content = Selection.activeTransform.Find("Viewport/Content").GetComponent<RectTransform>();
            content.SetParent(Selection.activeTransform);
            content.name = Selection.activeTransform.name + "_Content";
            UI.UIKits.SetAnchor(content, AnchorPresets.HorStretchTop);

            GameObject cellTemplate = new GameObject(Selection.activeTransform.name + "_Cell");
            cellTemplate.transform.SetParent(Selection.activeTransform);
            cellTemplate.AddComponent<Image>();
            cellTemplate.transform.localScale = Vector3.one;
            RectTransform rect = cellTemplate.GetComponent<RectTransform>();
            rect.SetAsFirstSibling();
            rect.anchoredPosition = Vector3.zero;

            ListView gridBoxMgr = content.gameObject.AddComponent<ListView>();
            gridBoxMgr.prefab = cellTemplate.transform as RectTransform;

            RectTransform viewPort = Selection.activeTransform.Find("Viewport") as RectTransform;
            GameObject.DestroyImmediate(viewPort.gameObject);

            AutoBindUserInterface(gridBoxMgr);
        }

        static private void ClearNullWidget(ref UISerialized.Widget[] widgets)
        {
            if (widgets == null) return;

            List<UISerialized.Widget> widgetLst = widgets.ToList<UISerialized.Widget>();
            for (int i = widgetLst.Count - 1; i >= 0; i--)
            {
                if (widgetLst[i].widget == null)
                {
                    widgetLst.Remove(widgetLst[i]);
                }
            }
            widgets = widgetLst.ToArray();
        }

        static private void AutoBindUserInterface(Component component)
        {
            UserInterface root = Selection.activeTransform.GetComponentInParent<UserInterface>();
            ClearNullWidget(ref root.Widgets);
            if (root.Widgets == null) root.Widgets = new UISerialized.Widget[0];
            int length = root.Widgets.Length;
            Array.Resize<TopGame.UI.UISerialized.Widget>(ref root.Widgets, length + 1);
            root.Widgets[length] = new UISerialized.Widget();
            root.Widgets[length].widget = component;
        }

        static private void InitDynamicScrollView()
        {
            ScrollRect scroll = Selection.activeTransform.GetComponent<ScrollRect>();
            if (scroll == null)
                return;
            scroll.gameObject.AddComponent<Mask>();

            Image img = Selection.activeTransform.GetComponent<Image>();
            if (img != null)
                UnityEngine.Object.DestroyImmediate(img);
            scroll.horizontal = false;
            scroll.vertical = true;
            scroll.horizontalScrollbar = null;
            scroll.verticalScrollbar = null;

            RectTransform scrollRect = scroll.GetComponent<RectTransform>();
            UI.UIKits.SetAnchor(scrollRect, AnchorPresets.HorStretchTop);

            Transform horizontalObj = Selection.activeTransform.Find("Scrollbar Horizontal");
            if (horizontalObj != null)
                GameObject.DestroyImmediate(horizontalObj.gameObject);
            Transform verticalObj = Selection.activeTransform.Find("Scrollbar Vertical");
            if (verticalObj != null)
                GameObject.DestroyImmediate(verticalObj.gameObject);

            RectTransform content = Selection.activeTransform.Find("Viewport/Content").GetComponent<RectTransform>();
            content.name = Selection.activeTransform.name + "_Content";
            content.transform.SetParent(scroll.transform);

            DynamicGridBoxMgr boxMgr = content.gameObject.AddComponent<DynamicGridBoxMgr>();
            VerticalLayoutGroup layout = content.gameObject.AddComponent<VerticalLayoutGroup>();
            layout.childForceExpandHeight = true;
            layout.childForceExpandWidth = true;
            UI.UIKits.SetAnchor(content, AnchorPresets.HorStretchTop);

            Transform viewPort = Selection.activeTransform.Find("Viewport");
            if (viewPort != null) GameObject.DestroyImmediate(viewPort.gameObject);

            GameObject cellTemplate = new GameObject(Selection.activeTransform.name + "_Cell");
            cellTemplate.transform.SetParent(scroll.transform);
            cellTemplate.AddComponent<Image>();
            cellTemplate.transform.localScale = Vector3.one;

            RectTransform rect = cellTemplate.GetComponent<RectTransform>();
            rect.anchoredPosition = Vector3.zero;

            GameObject titleTemplate = new GameObject(Selection.activeTransform.name + "_Title");
            titleTemplate.transform.SetParent(scroll.transform);
            titleTemplate.AddComponent<Image>();
            titleTemplate.transform.localScale = Vector3.one;

            RectTransform rect2 = titleTemplate.GetComponent<RectTransform>();
            rect2.anchoredPosition = Vector3.zero;

            GameObject subContentTemplate = new GameObject(Selection.activeTransform.name + "_SubContent");
            subContentTemplate.transform.SetParent(scroll.transform);
            subContentTemplate.transform.localScale = Vector3.one;

            RectTransform rect3 = subContentTemplate.AddComponent<RectTransform>();
            rect3.anchoredPosition = Vector3.zero;
            UI.UIKits.SetAnchor(rect3, AnchorPresets.HorStretchTop);

            GridLayoutGroup gridLayout = subContentTemplate.AddComponent<GridLayoutGroup>();
            gridLayout.startCorner = GridLayoutGroup.Corner.UpperLeft;
            gridLayout.startAxis = GridLayoutGroup.Axis.Horizontal;
            gridLayout.childAlignment = TextAnchor.UpperCenter;
            gridLayout.constraint = GridLayoutGroup.Constraint.Flexible;

            ContentSizeFitter filter = subContentTemplate.AddComponent<ContentSizeFitter>();
            filter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            filter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            boxMgr.CellTemplate = cellTemplate.transform;
            boxMgr.TitleTemplate = titleTemplate.transform;
            boxMgr.ContentTemplate = subContentTemplate.transform;

            AutoBindUserInterface(boxMgr);
        }


        static private void InitScrollView(bool isHorizontal)
        {
            ScrollRect scroll = Selection.activeTransform.GetComponent<ScrollRect>();
            if (scroll == null)
                return;

            UI.UIKits.SetAnchor(scroll.GetComponent<RectTransform>(), AnchorPresets.HorStretchTop);

            Image img = Selection.activeTransform.GetComponent<Image>();
            if (img != null)
                UnityEngine.Object.DestroyImmediate(img);
            scroll.horizontal = isHorizontal;
            scroll.vertical = !isHorizontal;
            scroll.horizontalScrollbar = null;
            scroll.verticalScrollbar = null;
            Transform horizontalObj = Selection.activeTransform.Find("Scrollbar Horizontal");
            if (horizontalObj != null)
                GameObject.DestroyImmediate(horizontalObj.gameObject);
            Transform verticalObj = Selection.activeTransform.Find("Scrollbar Vertical");
            if (verticalObj != null)
                GameObject.DestroyImmediate(verticalObj.gameObject);
            RectTransform viewPort = Selection.activeTransform.Find("Viewport") as RectTransform;
            if (viewPort != null)
            {
                viewPort.offsetMin = new Vector2(0, 0);
                viewPort.offsetMax = new Vector2(0, 0);
            }

            AutoBindUserInterface(scroll);

        }

        public static void ShowAllSelectedWidgets()
        {
            foreach (var item in Selection.gameObjects)
            {
                item.SetActive(true);
            }
        }
        public static void HideAllSelectedWidgets()
        {
            foreach (var item in Selection.gameObjects)
            {
                item.SetActive(false);
            }
        }

        public static void OptimizeBatchForMenu()
        {
            OptimizeBatch(Selection.activeTransform);
        }

        public static void OptimizeBatch(Transform trans)
        {
            if (trans == null)
                return;
            Dictionary<string, List<Transform>> imageGroup = new Dictionary<string, List<Transform>>();
            Dictionary<string, List<Transform>> textGroup = new Dictionary<string, List<Transform>>();
            List<List<Transform>> sortedImgageGroup = new List<List<Transform>>();
            List<List<Transform>> sortedTextGroup = new List<List<Transform>>();
            for (int i = 0; i < trans.childCount; i++)
            {
                Transform child = trans.GetChild(i);
                Texture cur_texture = null;
                Image img = child.GetComponent<Image>();
                if (img != null)
                {
                    cur_texture = img.mainTexture;
                }
                else
                {
                    RawImage rimg = child.GetComponent<RawImage>();
                    if (rimg != null)
                        cur_texture = rimg.mainTexture;
                }
                if (cur_texture != null)
                {
                    string cur_path = AssetDatabase.GetAssetPath(cur_texture);
                    TextureImporter importer = AssetImporter.GetAtPath(cur_path) as TextureImporter;
                    //Debug.Log("cur_path : " + cur_path + " importer:"+(importer!=null).ToString());
                    if (importer != null)
                    {
                        string atlas = importer.spritePackingTag;
                        //Debug.Log("atlas : " + atlas);
                        if (atlas != "")
                        {
                            if (!imageGroup.ContainsKey(atlas))
                            {
                                List<Transform> list = new List<Transform>();
                                sortedImgageGroup.Add(list);
                                imageGroup.Add(atlas, list);
                            }
                            imageGroup[atlas].Add(child);
                        }
                    }
                }
                else
                {
                    Text text = child.GetComponent<Text>();
                    if (text != null)
                    {
                        string fontName = text.font.name;
                        //Debug.Log("fontName : " + fontName);
                        if (!textGroup.ContainsKey(fontName))
                        {
                            List<Transform> list = new List<Transform>();
                            sortedTextGroup.Add(list);
                            textGroup.Add(fontName, list);
                        }
                        textGroup[fontName].Add(child);
                    }
                }
                OptimizeBatch(child);
            }
            //同一图集的Image间层级顺序继续保留,不同图集的顺序就按每组第一张的来
            for (int i = sortedImgageGroup.Count - 1; i >= 0; i--)
            {
                List<Transform> children = sortedImgageGroup[i];
                for (int j = children.Count - 1; j >= 0; j--)
                {
                    children[j].SetAsFirstSibling();
                }

            }
            foreach (var item in sortedTextGroup)
            {
                List<Transform> children = item;
                for (int i = 0; i < children.Count; i++)
                {
                    children[i].SetAsLastSibling();
                }
            }
        }

        public static void ExportUIPrefab(object argv)
        {
            UI.UISerialized user = argv as UI.UISerialized;
            UI.UIEditorTemper temper = user.GetComponent<UI.UIEditorTemper>();
            if (user.GetComponent<GraphicRaycaster>() == null)
            {
                user.gameObject.AddComponent<GraphicRaycaster>();
            }

            string strPath = "";
            bool bOpenDialog = false;
            if (temper == null || string.IsNullOrEmpty(temper.SaveToPrefab))
            {
                strPath = EditorUtility.SaveFilePanel("界面导出", UIEditorConfig.GetLastPath(UIEditorPathType.ExportPrefab), user.name, "prefab").Replace("\\", "/");
                if (strPath.Length <= 0) return;
                if (!strPath.Contains(Application.dataPath))
                {
                    EditorUtility.DisplayDialog("提示", "需要保存到[" + Application.dataPath + "]子路径中!!", "好的");
                    return;
                }
                bOpenDialog = true;
            }
            else
            {
                strPath = temper.SaveToPrefab;
                if (!System.IO.File.Exists(Application.dataPath.Replace("/Assets", "/") + strPath))
                {
                    strPath = EditorUtility.SaveFilePanel("界面导出", UIEditorConfig.GetLastPath(UIEditorPathType.ExportPrefab), user.name, "prefab").Replace("\\", "/");
                    if (strPath.Length <= 0) return;
                    if (!strPath.Contains(Application.dataPath))
                    {
                        EditorUtility.DisplayDialog("提示", "需要保存到[" + Application.dataPath + "]子路径中!!", "好的");
                        return;
                    }
                    bOpenDialog = true;
                }
            }

            strPath = strPath.Replace(Application.dataPath, "Assets");
            if (bOpenDialog)
                UIEditorConfig.SetLastPath(UIEditorPathType.ExportPrefab, strPath);
            string strFileName = System.IO.Path.GetFileNameWithoutExtension(strPath);
            if (string.IsNullOrEmpty(strFileName))
            {
                EditorUtility.DisplayDialog("提示", "名称不能为空!!!", "好的");
                return;
            }
            if (user != null)
            {
                user.gameObject.name = strFileName;
                GameObject prefab = PrefabUtility.SaveAsPrefabAsset(user.gameObject, strPath);
                if (UnityEditorInternal.ComponentUtility.CopyComponent(user.transform))
                {
                    UnityEditorInternal.ComponentUtility.PasteComponentValues(prefab.transform);
                }
                UISerialized prefabUI = prefab.GetComponent<UISerialized>();
                if (prefabUI!=null)
                {
                    ClearNullWidget(ref prefabUI.Widgets);
                }
                CheckDefaultFont(prefab);
                ReplaceWrongRefImage(prefab, false);
                CheckUITweener(prefab);

                UI.UIEditorTemper prefabtemper = prefab.GetComponent<UI.UIEditorTemper>();
                if (prefabtemper) GameObject.DestroyImmediate(prefabtemper, true);

                if (temper == null)
                    temper = user.gameObject.AddComponent<UI.UIEditorTemper>();
                temper.SaveToPrefab = strPath;
            }
        }

        public static void ExportUIPrefabWithEmptyWrongRef(object argv)
        {
            UI.UISerialized user = argv as UI.UISerialized;
            ClearNullWidget(ref user.Widgets);

            UI.UIEditorTemper temper = user.GetComponent<UI.UIEditorTemper>();
            if (user.GetComponent<GraphicRaycaster>() == null)
            {
                user.gameObject.AddComponent<GraphicRaycaster>();
            }
            CheckDefaultFont(user.gameObject);
            EmptyWrongRefImage(user.gameObject);

            string strPath = "";
            bool bOpenDialog = false;
            if (temper == null || string.IsNullOrEmpty(temper.SaveToPrefab))
            {
                strPath = EditorUtility.SaveFilePanel("界面导出", UIEditorConfig.GetLastPath(UIEditorPathType.ExportPrefab), user.name, "prefab").Replace("\\", "/");
                if (strPath.Length <= 0) return;
                if (!strPath.Contains(Application.dataPath))
                {
                    EditorUtility.DisplayDialog("提示", "需要保存到[" + Application.dataPath + "]子路径中!!", "好的");
                    return;
                }
                bOpenDialog = true;
            }
            else
            {
                strPath = temper.SaveToPrefab;
                if (!System.IO.File.Exists(Application.dataPath.Replace("/Assets", "/") + strPath))
                {
                    strPath = EditorUtility.SaveFilePanel("界面导出", UIEditorConfig.GetLastPath(UIEditorPathType.ExportPrefab), user.name, "prefab").Replace("\\", "/");
                    if (strPath.Length <= 0) return;
                    if (!strPath.Contains(Application.dataPath))
                    {
                        EditorUtility.DisplayDialog("提示", "需要保存到[" + Application.dataPath + "]子路径中!!", "好的");
                        return;
                    }
                    bOpenDialog = true;
                }
            }

            strPath = strPath.Replace(Application.dataPath, "Assets");
            if (bOpenDialog)
                UIEditorConfig.SetLastPath(UIEditorPathType.ExportPrefab, strPath);
            string strFileName = System.IO.Path.GetFileNameWithoutExtension(strPath);
            if (string.IsNullOrEmpty(strFileName))
            {
                EditorUtility.DisplayDialog("提示", "名称不能为空!!!", "好的");
                return;
            }
            if (user != null)
            {
                user.gameObject.name = strFileName;


                GameObject prefab = PrefabUtility.SaveAsPrefabAsset(user.gameObject, strPath);
                if (UnityEditorInternal.ComponentUtility.CopyComponent(user.transform))
                {
                    UnityEditorInternal.ComponentUtility.PasteComponentValues(prefab.transform);
                }
                UI.UIEditorTemper prefabtemper = prefab.GetComponent<UI.UIEditorTemper>();
                if (prefabtemper) GameObject.DestroyImmediate(prefabtemper, true);

                if (temper == null)
                    temper = user.gameObject.AddComponent<UI.UIEditorTemper>();
                temper.SaveToPrefab = strPath;
            }
        }

        public static void EmptyWrongRefImage(GameObject prefab)
        {
            if (prefab != null)
            {
                PublishSetting m_BuildSetting = PublishPanel.LoadSetting();
                List<string> buildDirs = m_BuildSetting.buildDirs;
                List<string> unbuildDirs = m_BuildSetting.unbuildDirs;
                var images = prefab.GetComponentsInChildren<UnityEngine.UI.Image>(true);
                var rawImages = prefab.GetComponentsInChildren<UnityEngine.UI.RawImage>(true);
                var uiReferences = prefab.GetComponentsInChildren<UISerialized>(true);
                for (int i = 0; i < images.Length; ++i)
                {
                    Image img = images[i];
                    string path = AssetDatabase.GetAssetPath(img.sprite);
                    if (path == null) continue;
                    if (CanReplaceImageByPath(path))
                    {
                        img.sprite = null;
                        Debug.Log("图片错误引用资源替换:" + " -> " + GetFullParent(img.transform));//只打印有引用替换的组件,
                    }
                }


                for (int i = 0; i < rawImages.Length; ++i)
                {
                    RawImage img = rawImages[i];
                    string path = AssetDatabase.GetAssetPath(img.texture);
                    if (path == null) continue;
                    if (CanReplaceImageByPath(path))
                    {
                        img.texture = null;
                        Debug.Log("图片错误引用资源替换:" + " -> " + GetFullParent(img.transform));//只打印有引用替换的组件,
                    }
                }

                EditorUtility.SetDirty(prefab);
                AssetDatabase.SaveAssets();
            }
        }

        public static bool CheckDefaultFont(GameObject prefab)
        {
            if (prefab != null)
            {
                HashSet<string> UIFonts = new HashSet<string>();
                if(System.IO.File.Exists("Assets/EditorDatas/uiUseFont.txt"))
                {
                    string[] fonts = System.IO.File.ReadAllLines("Assets/EditorDatas/uiUseFont.txt");
                    for (int i = 0; i < fonts.Length; ++i)
                        UIFonts.Add(fonts[i]);
                }

                Font defaultFont = AssetDatabase.LoadAssetAtPath<Font>("Assets/Datas/Fonts/default.ttf");
                if (defaultFont == null) return false;
                var text = prefab.GetComponentsInChildren<UnityEngine.UI.Text>(true);

                bool bDirty = false;
                for (int i = 0; i < text.Length; ++i)
                {
                    var textComp = text[i];

                    if (textComp.font == null)
                    {
                        textComp.font = defaultFont;
                        bDirty = true;
                        continue;
                    }
                    string fontPath = AssetDatabase.GetAssetPath(textComp.font.GetInstanceID());
                    if (fontPath.Contains(".fontsetting"))
                        continue;
                    FontSwitch fontSwitch = textComp.GetComponent<FontSwitch>();
                    if (fontSwitch == null)
                    {
                        fontSwitch = textComp.gameObject.AddComponent<FontSwitch>();
                        bDirty = true;
                    }
                    System.Reflection.FieldInfo fieldInfo = fontSwitch.GetType().GetField("text", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    if(fieldInfo!=null)
                    {
                        if (textComp != fieldInfo.GetValue(fontSwitch))
                        {
                            bDirty = true;
                            fieldInfo.SetValue(fontSwitch, textComp);
                        }
                    }

                    if (UIFonts.Contains(fontPath))
                        continue;

                    if (textComp.font != defaultFont && fontSwitch.bCheckFont)
                    {
                        bDirty = true;
                        textComp.font = defaultFont;
                    }
                }
                if(bDirty)
                {
                    EditorUtility.SetDirty(prefab);
                    AssetDatabase.SaveAssets();
                }
                return bDirty;
            }
            return false;
        }

        public static bool CheckUITweener(GameObject prefab, Dictionary<int, UI.UIAnimatorGroupData> vUIAnims = null)
        {
            if (prefab != null)
            {
                if(vUIAnims == null)
                {
                    vUIAnims = new Dictionary<int, UI.UIAnimatorGroupData>();
                    UI.UIAnimatorAssets animator = AssetDatabase.LoadAssetAtPath<UI.UIAnimatorAssets>("Assets/DatasRef/UI/Animation/UIAnimations.asset");
                    if (animator != null)
                    {
                        for (int i = 0; i < animator.animations.Length; ++i)
                        {
                            if (animator.animations[i].Parameters == null)
                            {
                                continue;
                            }
                            bool bHasAlpha = false;
                            for (int j = 0; j < animator.animations[i].Parameters.Length; ++j)
                            {
                                if (animator.animations[i].Parameters[j].type == UI.UIAnimatorElementType.ALPAH)
                                {
                                    bHasAlpha = true;
                                    break; ;
                                }
                            }
                            if (bHasAlpha)
                                vUIAnims[animator.animations[i].nID] = animator.animations[i];
                        }
                    }
                }

                var text = prefab.GetComponentsInChildren<RtgTween.TweenerGroup>(true);

                int bDirty = 0;
                for (int t = 0; t < text.Length; ++t)
                {
                    var textComp = text[t];
                    for (int j = 0; j < textComp.Groups.Count; ++j)
                    {
                        if (RtgTween.TweenerEditorDrawer.TestTweenAlphaParam(textComp.gameObject, textComp.Groups[j].Propertys))
                            bDirty++;
                    }
                }
                var sers = prefab.GetComponentsInChildren<UI.UISerialized>(true);
                for (int t = 0; t < sers.Length; ++t)
                {
                    var textComp = sers[t];
                    if (textComp.UIEventDatas == null) continue;
                    for (int j = 0; j < textComp.UIEventDatas.Length; ++j)
                    {
                        UI.UIAnimatorGroupData gpData;
                        if (textComp.UIEventDatas[j] != null && vUIAnims.TryGetValue(textComp.UIEventDatas[j].animationID, out gpData))
                        {
                            Transform root = textComp.UIEventDatas[j].ApplyRoot;
                            if (root == null) root = textComp.transform;
                            TransformRef[] tranRefs = root.GetComponentsInChildren<TransformRef>();
                            Dictionary<int, Transform> vSlots = new Dictionary<int, Transform>();
                            Dictionary<string, Transform> vStrSlots = new Dictionary<string, Transform>();
                            for (int k = 0; k < tranRefs.Length; ++k)
                            {
                                Transform slot = tranRefs[k].transform;
                                if (tranRefs[k].refTransform) slot = tranRefs[k].refTransform;
                                if (tranRefs[k].GUID != 0)
                                    vSlots[tranRefs[k].GUID] = slot;
                                string name = tranRefs[k].strSymbole;
                                if (string.IsNullOrEmpty(name)) name = tranRefs[k].transform.name;
                                vStrSlots[name] = slot;
                            }
                            for (int k = 0; k < gpData.Parameters.Length; ++k)
                            {
                                if (gpData.Parameters[k].type == UI.UIAnimatorElementType.ALPAH)
                                {
                                    Transform find = null;
                                    if (gpData.Parameters[k].bFirstParent)
                                    {
                                        find = root;
                                    }
                                    else
                                    {
                                        if (gpData.Parameters[k].controllerTag != 0) vSlots.TryGetValue(gpData.Parameters[k].controllerTag, out find);
                                        if (find == null && !string.IsNullOrEmpty(gpData.Parameters[k].strControllerName))
                                        {
                                            find = DyncmicTransformCollects.FindTransformByName(gpData.Parameters[k].strControllerName);
                                            if (find == null)
                                                find = Framework.Core.CommonUtility.FindTransform(root, gpData.Parameters[k].strControllerName);
                                        }
                                    }

                                    if (find)
                                    {
                                        CanvasGroup gp = find.GetComponent<CanvasGroup>();
                                        if (gp == null)
                                        {
                                            find.gameObject.AddComponent<CanvasGroup>();
                                            EditorUtility.SetDirty(find);
                                            bDirty++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                return bDirty > 0;
            }
            return false;
        }

        public static bool ReplaceWrongRefImage(GameObject prefab, bool bPop = true)
        {
            bool bReplace = false;
            if (prefab != null)
            {
                var images = prefab.GetComponentsInChildren<UnityEngine.UI.Image>(true);
                var rawImages = prefab.GetComponentsInChildren<UnityEngine.UI.RawImage>(true);
                var uiReferences = prefab.GetComponentsInChildren<UISerialized>(true);
                for (int i = 0; i < images.Length; ++i)
                {
                    Image img = images[i];
                    string path = AssetDatabase.GetAssetPath(img.sprite);
                    if (img is ImageEx)
                    {
                        if (!string.IsNullOrEmpty((img as ImageEx).texturePath))
                            path = (img as ImageEx).texturePath;
                    }
                    if (string.IsNullOrEmpty(path))
                    {
                        if (img is ImageEx)
                        {
                            img.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(DEF_TEXTURE_PATH);
                            if (!CanReplaceImageByPath((img as ImageEx).texturePath))
                                ReplaceComponentNoPath<ImageEx>(img, (img as ImageEx).texturePath, uiReferences, bPop); //to image
                            bReplace = true;
                        }
                        else if(Framework.Core.CommonUtility.Equal(img.color, Color.white))
                            img.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(DEF_TEXTURE_PATH);
                        continue;
                    }
                    if (CanReplaceImageByPath(path))
                    {
                        if (!(img is ImageEx))
                        {
                            ReplaceComponent<Image>(img, path, uiReferences, bPop); //to ImageEx
                            bReplace = true;
                        }
                    }
                    else
                    {
                        if(img is ImageEx)
                        {
                            if(path.CompareTo(DEF_TEXTURE_PATH) ==0) continue;
                            ReplaceComponentNoPath<ImageEx>(img, path, uiReferences, bPop); // to image 
                            bReplace = true;
                        }
                    }
                }


                for (int i = 0; i < rawImages.Length; ++i)
                {
                    RawImage img = rawImages[i];
                    string path = AssetDatabase.GetAssetPath(img.texture);
                    if (img is RawImageEx)
                    {
                        if(!string.IsNullOrEmpty((img as RawImageEx).texturePath))
                            path = (img as RawImageEx).texturePath;
                    }
                    if (string.IsNullOrEmpty(path))
                    {
                        if (img is RawImageEx)
                        {
                            img.texture = AssetDatabase.LoadAssetAtPath<Texture2D>(DEF_TEXTURE_PATH);
                            if (!CanReplaceImageByPath((img as RawImageEx).texturePath))
                                ReplaceComponentNoPath<RawImageEx>(img, (img as RawImageEx).texturePath, uiReferences, bPop);   //to rawimage
                            bReplace = true;
                        }
                        else if (Framework.Core.CommonUtility.Equal(img.color, Color.white))
                            img.texture = AssetDatabase.LoadAssetAtPath<Texture2D>(DEF_TEXTURE_PATH);

                        continue;
                    }
                    if (CanReplaceImageByPath(path))
                    {
                        if (!(img is RawImageEx))
                        {
                            ReplaceComponent<RawImage>(img, path, uiReferences, bPop);  // to rawimageex
                            bReplace = true;
                        }
                    }
                    else
                    {
                        if (img is RawImageEx)
                        {
                            if (path.CompareTo(DEF_TEXTURE_PATH) == 0) continue;
                            ReplaceComponentNoPath<RawImageEx>(img, path, uiReferences, bPop); // to rawimage
                            bReplace = true;
                        }
                    }
                }

                if(bReplace)
                {
                    EditorUtility.SetDirty(prefab);
                    AssetDatabase.SaveAssets();
                }
            }
            return bReplace;
        }
        
        //------------------------------------------------------
        public static bool CanReplaceImageByPath(string path)
        {
            if (path == null)
            {
                return false;
            }
            if (path.CompareTo(DEF_TEXTURE_PATH) == 0)
                return false;
            PublishSetting m_BuildSetting = PublishPanel.LoadSetting();
            List<string> buildDirs = m_BuildSetting.buildDirs;
            if(path.Contains("Assets/Datas/"))
            {
                List<string> unbuildDirs = m_BuildSetting.unbuildDirs;
                for(int i = 0; i < unbuildDirs.Count; ++i)
                {
                    if (path.Contains(unbuildDirs[i]))
                        return false;
                }
                return true;
            }
            if (buildDirs.Count>0)
            {
                for (int i = 0; i < buildDirs.Count; i++)
                {
                    if(path.Contains(buildDirs[i]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        static void ReplaceComponentNoPath<T>(Graphic component, string path, UISerialized[] references, bool bPop = true) where T : Graphic
        {
            bool result = false;
            if (component == null) return;
            T img = component as T;
            if (img == null)
            {
                return;
            }

            GameObject root = img.gameObject;
            bool raycast = img.raycastTarget;

            //查找引用
            UISerialized uiRef = null;
            int index = -1;
            foreach (var reference in references)
            {
                if (reference.IsExist<T>(img))
                {
                    index = reference.GetIndex<T>(img);
                    uiRef = reference;
                    //reference.SetWidget<>
                    break;//todo:现在只考虑单个组件引用的情况,单个组件多引用不考虑
                }
            }
            Color color = img.color;
            Material mat = img.material;
            if (!string.IsNullOrEmpty(path))
            {
                if (img is ImageEx)
                {
                    var maskAble = (img as Image).maskable;
                    var fillAmount = (img as ImageEx).fillAmount;
                    var fillMethod = (img as ImageEx).fillMethod;
                    var fillOrigin = (img as ImageEx).fillOrigin;
                    var preserveAspect = (img as ImageEx).preserveAspect;
                    var type = (img as ImageEx).type;
                    var fillCenter = (img as ImageEx).fillCenter;
                    var fillClockwise = (img as ImageEx).fillClockwise;
                    GameObject.DestroyImmediate(img, true);
                    Image ex = root.AddComponent<Image>();
                    ex.raycastTarget = raycast;
                    ex.color = color;
                    ex.material = mat;
                    ex.fillAmount = fillAmount;
                    ex.fillMethod = fillMethod;
                    ex.fillOrigin = fillOrigin;
                    ex.preserveAspect = preserveAspect;
                    ex.type = type;
                    ex.fillCenter = fillCenter;
                    ex.fillClockwise = fillClockwise;
                    ex.maskable = maskAble;
                    ex.sprite = AssetDatabase.LoadAssetAtPath(path, typeof(Sprite)) as Sprite;
                    if (uiRef != null && index != -1)
                    {
                        uiRef.SetWidget<Image>(index, ex);
                        //只打印有引用替换的组件,
                        Debug.Log("图片错误引用资源替换:" + " -> " + GetFullParent(root.transform) + "-> Image");
                    }
                }
                else if (img is RawImageEx)
                {
                    var uvRect = (img as RawImageEx).uvRect;
                    GameObject.DestroyImmediate(img, true);
                    RawImage ex = root.AddComponent<RawImage>();
                    ex.raycastTarget = raycast;
                    ex.raycastTarget = raycast;
                    ex.color = color;
                    ex.uvRect = uvRect;
                    ex.texture = AssetDatabase.LoadAssetAtPath(path, typeof(Texture)) as Texture;
                    if (uiRef != null && index != -1)
                    {
                        uiRef.SetWidget<RawImage>(index, ex);
                        //只打印有引用替换的组件,
                        Debug.Log("图片错误引用资源替换:" + " -> " + GetFullParent(root.transform) + "-> RawImage");
                    }
                }
            }
        }
        static bool ReplaceComponent<T>(Graphic component,string path,UISerialized[] references, bool  bPop = true) where T: Graphic
        {
            bool result = false;
            if (component == null) return result;
            T img = component as T;
            if (img == null)
            {
                return result;
            }

            GameObject root = img.gameObject;
            bool raycast = img.raycastTarget;

            //查找引用
            UISerialized uiRef = null;
            int index = -1;
            foreach (var reference in references)
            {
                if (reference.IsExist<T>(img))
                {
                    index = reference.GetIndex<T>(img);
                    uiRef = reference;
                    //reference.SetWidget<>
                    break;//todo:现在只考虑单个组件引用的情况,单个组件多引用不考虑
                }
            }
            Color color = img.color;
            Material mat = img.material;
            if (img is ImageEx || img is RawImageEx)//ex组件过滤
            {

                if(img is ImageEx)
                {
                    (img as ImageEx).sprite = AssetDatabase.LoadAssetAtPath<Sprite>(DEF_TEXTURE_PATH);
                }
                else if (img is RawImageEx)
                {
                    (img as RawImageEx).texture = AssetDatabase.LoadAssetAtPath<Texture2D>(DEF_TEXTURE_PATH);
                }
                return result;
            }

            if (img.GetComponent<LocalizationImage>())//多语言图片的过滤掉图片类型替换,但是去掉默认sprite资源
            {
                (img as Image).sprite = null;
                return result;
            }

            if (img.GetComponent<LocalizationRawImage>())//多语言图片的过滤掉图片类型替换,但是去掉默认texture资源
            {
                (img as RawImage).texture = null;
                return result;
            }


            //Selection.activeObject = img;// new UnityEngine.Object[] {img };


            if (img is Image)
            {
                bool bReplace = true;
                if (bPop)
                    bReplace = UnityEditor.EditorUtility.DisplayDialog("提示", $"{img.name}:使用动态图片,并且组件类型有误,确认操作?", "转成ImageEx组件", "清空图标");
                if (!bReplace)
                {
                    (img as Image).sprite = null;
                }
                else
                {
                    var maskAble = (img as Image).maskable;
                    var fillAmount = (img as Image).fillAmount;
                    var fillMethod = (img as Image).fillMethod;
                    var fillOrigin = (img as Image).fillOrigin;
                    var preserveAspect = (img as Image).preserveAspect;
                    var type = (img as Image).type;
                    var fillCenter = (img as Image).fillCenter;
                    var fillClockwise = (img as Image).fillClockwise;
                    GameObject.DestroyImmediate(img, true);
                    ImageEx ex = root.AddComponent<ImageEx>();
                    ex.raycastTarget = raycast;
                    ex.color = color;
                    ex.material = mat;
                    ex.fillAmount = fillAmount;
                    ex.fillMethod = fillMethod;
                    ex.fillOrigin = fillOrigin;
                    ex.preserveAspect = preserveAspect;
                    ex.type = type;
                    ex.fillCenter = fillCenter;
                    ex.fillClockwise = fillClockwise;
                    ex.texturePath = path;
                    ex.maskable = maskAble;
                    //   var textureObj = AssetDatabase.LoadAssetAtPath<Sprite>(path);
                    ex.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(DEF_TEXTURE_PATH);// textureObj;

                    if (uiRef != null && index != -1)
                    {
                        uiRef.SetWidget<ImageEx>(index, ex);
                        Debug.Log("图片错误引用资源替换:" + " -> " + GetFullParent(root.transform) + "-> ImageEx");//只打印有引用替换的组件,
                    }
                }

                
                result = true;
            }
            else if (img is RawImage)
            {
                bool bReplace = true;
                if (bPop)
                    bReplace = UnityEditor.EditorUtility.DisplayDialog("提示", $"{img.name}:使用动态图片,并且组件类型有误,确认操作?", "转成RawImageEx组件","清空图标");
                if (!bReplace)
                {
                    (img as RawImage).texture = null;
                }
                else
                {
                    var uvRect = (img as RawImage).uvRect;
                    GameObject.DestroyImmediate(img, true);
                    RawImageEx ex = root.AddComponent<RawImageEx>();
                    ex.raycastTarget = raycast;
                    ex.texturePath = path;
                    ex.uvRect = uvRect;
                    ex.material = mat;
                    // var textureObj = AssetDatabase.LoadAssetAtPath(path, typeof(UnityEngine.Object));
                    ex.texture =  AssetDatabase.LoadAssetAtPath<Texture2D>(DEF_TEXTURE_PATH);// textureObj as Texture2D;

                    if (uiRef != null && index != -1)
                    {
                        uiRef.SetWidget<RawImageEx>(index, ex);
                        //只打印有引用替换的组件,
                        Debug.Log("图片错误引用资源替换:" + " -> " + GetFullParent(root.transform) + "-> RawImageEx");
                    }
                }
                
                result = true;
            }

            return result;
        }

        static string GetFullParent(Transform trs)
        {
            string fullParent = "";
            if (trs == null)
            {
                return fullParent;
            }

            while (trs.parent != null)
            {
                fullParent += trs.name + " <- ";
                trs = trs.parent;
            }
            return fullParent;
        }

        public static void CreateReferToPic(object argv)
        {
            UI.UIEditorTemper user = argv as UI.UIEditorTemper;

            if (user != null)
            {
                Selection.activeTransform = user.transform;
                SelectPicForReferTo(user);
            }
        }

        public static bool SelectPicForReferTo(UI.UIEditorTemper user)
        {
            if (user != null)
            {
                string default_path = UIEditorConfig.GetLastPath(UIEditorPathType.OpenReferTo);
                string spr_path = EditorUtility.OpenFilePanel("加载外部图片", default_path, "");
                if (spr_path.Length > 0)
                {
                    user.ReferToPic = spr_path;
                    if (UIEditor.Instance != null)
                        user.LoadReferToPic(UIEditor.Instance.referTo);
                    else
                    {
                        user.LoadReferToPic(null);
                    }
                    UIEditorConfig.SetLastPath(UIEditorPathType.OpenReferTo, spr_path);
                    EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

                    return true;
                }
            }
            return false;
        }

        static private Transform GetGoodContainer(Transform trans)
        {
            if (trans == null)
                return null;
            if (trans.GetComponent<Canvas>() != null || trans.GetComponent<UI.UserInterface>() != null)
                return GetUserInterface(trans.gameObject);
            return trans;
        }
        //------------------------------------------------------
        public static Transform GetUserInterface(GameObject anyObj)
        {
            UI.UserInterface layoutInfo = anyObj.GetComponentInParent<UI.UserInterface>();
            if (layoutInfo == null) return null;
            return layoutInfo.transform;
        }

        public static Bounds GetBounds(GameObject obj)
        {
            Vector3 Min = new Vector3(99999, 99999, 99999);
            Vector3 Max = new Vector3(-99999, -99999, -99999);
            MeshRenderer[] renders = obj.GetComponentsInChildren<MeshRenderer>();
            if (renders.Length > 0)
            {
                for (int i = 0; i < renders.Length; i++)
                {
                    if (renders[i].bounds.min.x < Min.x)
                        Min.x = renders[i].bounds.min.x;
                    if (renders[i].bounds.min.y < Min.y)
                        Min.y = renders[i].bounds.min.y;
                    if (renders[i].bounds.min.z < Min.z)
                        Min.z = renders[i].bounds.min.z;

                    if (renders[i].bounds.max.x > Max.x)
                        Max.x = renders[i].bounds.max.x;
                    if (renders[i].bounds.max.y > Max.y)
                        Max.y = renders[i].bounds.max.y;
                    if (renders[i].bounds.max.z > Max.z)
                        Max.z = renders[i].bounds.max.z;
                }
            }
            else
            {
                RectTransform[] rectTrans = obj.GetComponentsInChildren<RectTransform>();
                Vector3[] corner = new Vector3[4];
                for (int i = 0; i < rectTrans.Length; i++)
                {
                    //获取节点的四个角的世界坐标，分别按顺序为左下左上，右上右下
                    rectTrans[i].GetWorldCorners(corner);
                    if (corner[0].x < Min.x)
                        Min.x = corner[0].x;
                    if (corner[0].y < Min.y)
                        Min.y = corner[0].y;
                    if (corner[0].z < Min.z)
                        Min.z = corner[0].z;

                    if (corner[2].x > Max.x)
                        Max.x = corner[2].x;
                    if (corner[2].y > Max.y)
                        Max.y = corner[2].y;
                    if (corner[2].z > Max.z)
                        Max.z = corner[2].z;
                }
            }

            Vector3 center = (Min + Max) / 2;
            Vector3 size = new Vector3(Max.x - Min.x, Max.y - Min.y, Max.z - Min.z);
            return new Bounds(center, size);
        }
    }
}

