using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TopGame.ED;
using TopGame.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TopGame
{


    public class UIToolsEditor : EditorWindow
    {
        static EditorWindow m_Window = null;
        [MenuItem("Tools/UI/UIEditorTools _F12")]
        public static void ShowWindow2()
        {
            EditorWindow window = EditorWindow.GetWindow(typeof(UIToolsEditor));
            window.titleContent = new GUIContent("ui编辑工具面板");
            m_Window = window;
        }

        [MenuItem("Assets/复制文件路径")]
        public static void CopyPath()
        {
            if (Selection.objects == null || Selection.objects.Length == 0)
            {
                return;
            }
            GUIUtility.systemCopyBuffer = AssetDatabase.GetAssetPath(Selection.objects[0]);
            if (m_Window != null)
            {
                m_Window.ShowNotification(new GUIContent("复制成功:" + GUIUtility.systemCopyBuffer));
            }
        }

        Vector2 m_ScrollPos;
        private void OnGUI()
        {
            m_ScrollPos = EditorGUILayout.BeginScrollView(m_ScrollPos);
            ReloadScene();
            SetSelectGameobjectPos();
            SetSelectGameobjectSize();
            SetUIName();
            CopyGameObject();
            IsAddReferencesToggle();
            AddSelectToUISerialized();
            EditorGUILayout.BeginHorizontal();
            CreateImage();
            CreateText();
            CreateBtn();
            CreateList();
            CreateProgressBar();
            EditorGUILayout.EndHorizontal();
            TextReplaceWithHexText();
            ImageConvertRawImage();
            ConvertSelectEmptyImage();
            SetParticleOrder();
            SetParticleMasking();
            CopySelectPosition();
            SetAnchor();
            SetRaycast();
            EditorGUILayout.EndScrollView();
        }
        //------------------------------------------------------
        bool m_RaycastToggle = false;
        private void SetRaycast()
        {
            
            EditorGUILayout.BeginHorizontal();
            m_RaycastToggle = EditorGUILayout.Toggle("射线检测:",m_RaycastToggle);
            if (GUILayout.Button("设置"))
            {
                var gos = Selection.gameObjects;
                if (gos == null || gos.Length == 0)
                {
                    ShowNotification(new GUIContent("请选择一个UI"));
                    return;
                }

                foreach (var item in gos)
                {
                    var graphic = item.GetComponent<Graphic>();
                    if (graphic)
                    {
                        graphic.raycastTarget = m_RaycastToggle;
                    }
                }
                ShowNotification(new GUIContent("设置完成!"));
            }
            EditorGUILayout.EndHorizontal();
        }
        //------------------------------------------------------
        Vector3 m_SetSelectPos;
        private void SetSelectGameobjectPos()
        {
            m_SetSelectPos = EditorGUILayout.Vector3Field("设置世界坐标:", m_SetSelectPos);

            EditorGUILayout.BeginHorizontal();
            
            if (GUILayout.Button("设置"))
            {
                var gos = Selection.gameObjects;
                if (gos == null || gos.Length == 0)
                {
                    ShowNotification(new GUIContent("请选择一个UI"));
                    return;
                }

                foreach (var item in gos)
                {
                    item.transform.position = m_SetSelectPos;
                }
            }

            if (GUILayout.Button("读取选择的物体世界坐标"))
            {
                var gos = Selection.gameObjects;
                if (gos == null || gos.Length == 0)
                {
                    ShowNotification(new GUIContent("请选择一个UI"));
                    return;
                }

                m_SetSelectPos = gos[0].transform.position;
            }
            if (GUILayout.Button("读取选择的物体View坐标"))
            {
                var gos = Selection.gameObjects;
                if (gos == null || gos.Length == 0)
                {
                    ShowNotification(new GUIContent("请选择一个UI"));
                    return;
                }

                var pos = gos[0].transform.position;
                var cam = UIManager.GetInstance().GetUICamera();
                if (cam == null)
                {
                    return;
                }


                m_SetSelectPos = cam.WorldToViewportPoint(pos);
            }
            if (GUILayout.Button("读取选择的物体Screen坐标"))
            {
                var gos = Selection.gameObjects;
                if (gos == null || gos.Length == 0)
                {
                    ShowNotification(new GUIContent("请选择一个UI"));
                    return;
                }

                var pos = gos[0].transform.position;
                var cam = UIManager.GetInstance().GetUICamera();
                if (cam == null)
                {
                    return;
                }


                m_SetSelectPos = cam.WorldToScreenPoint(pos);
            }
            EditorGUILayout.EndHorizontal();
        }
        //------------------------------------------------------
        Vector2 m_SetSelectSize;
        private void SetSelectGameobjectSize()
        {
            m_SetSelectSize = EditorGUILayout.Vector2Field("设置大小:", m_SetSelectSize);

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("设置"))
            {
                var gos = Selection.gameObjects;
                if (gos == null || gos.Length == 0)
                {
                    ShowNotification(new GUIContent("请选择一个UI"));
                    return;
                }

                RectTransform rect;
                foreach (var item in gos)
                {
                    rect = item.GetComponent<RectTransform>();
                    if (rect)//
                    {
                        //rect.sizeDelta = m_SetSelectSize;//当Anchors不重合的时候，设置sizeDelta就不能正确控制RectTransform的大小
                        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, m_SetSelectSize.x);
                        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, m_SetSelectSize.y);
                    }
                }
            }

            if (GUILayout.Button("读取选择的物体UI大小"))
            {
                var gos = Selection.gameObjects;
                if (gos == null || gos.Length == 0)
                {
                    ShowNotification(new GUIContent("请选择一个UI"));
                    return;
                }

                var rect = gos[0].GetComponent<RectTransform>();
                if (rect == null)
                {
                    ShowNotification(new GUIContent("请选择一个UI"));
                    return;
                }

                m_SetSelectSize  = rect.rect.size;
                ShowNotification(new GUIContent("sizeDelta:" + rect.sizeDelta));
            }
            EditorGUILayout.EndHorizontal();
        }
        //------------------------------------------------------
        string renameText;
        void SetUIName()
        {
            #region 设置名字


            EditorGUILayout.BeginHorizontal();
            //输入框控件
            renameText = EditorGUILayout.TextField("重命名名字：", renameText);

            if (GUILayout.Button("设置名字", GUILayout.Width(100)))
            {
                //打开通知栏
                //this.ShowNotification(new GUIContent("this is a notification"));
                //关闭通知栏
                //this.RemoveNotification();
                var selectList = Selection.gameObjects;

                if (selectList.Length <= 0)
                {
                    this.ShowNotification(new GUIContent("当前没有选择物体!!"));
                    return;
                }
                Undo.RecordObjects(selectList, "selectList1");
                foreach (var item in selectList)
                {
                    item.name = renameText;
                }

            }

            if (GUILayout.Button("添加后缀数字", GUILayout.Width(100)))
            {
                var selectList = Selection.gameObjects;

                if (selectList.Length <= 0)
                {
                    this.ShowNotification(new GUIContent("当前没有选择物体!!"));
                    return;
                }

                //根据Hierarchy上的顺序进行从小到大的排序
                List<GameObject> list = new List<GameObject>(selectList);
                list.Sort((a, b) => a.transform.GetSiblingIndex().CompareTo(b.transform.GetSiblingIndex()));
                foreach (var item in list)
                {
                    Debug.Log(item.name);
                    Debug.Log("顺序:" + item.transform.GetSiblingIndex());
                }
                Undo.RecordObjects(selectList, "selectList2");
                for (int i = 1; i <= list.Count; i++)
                {
                    string name = list[i - 1].name;
                    name = name.Split(' ')[0];
                    list[i - 1].name = name + i.ToString();
                }

            }

            if (GUILayout.Button("添加父物体名字前缀", GUILayout.Width(100)))
            {
                var selectList = Selection.gameObjects;

                if (selectList.Length <= 0)
                {
                    this.ShowNotification(new GUIContent("当前没有选择物体!!"));
                    return;
                }

                //根据Hierarchy上的顺序进行从小到大的排序
                Undo.RecordObjects(selectList, "selectList3");
                foreach (var item in selectList)
                {
                    if (item.transform.parent == null)
                    {
                        continue;
                    }

                    item.name = item.transform.parent.name + item.name;
                }

            }
            EditorGUILayout.EndHorizontal();
            #endregion
        }
        //------------------------------------------------------
        //------------------------------------------------------
        GameObject m_CopyGo;
        private void CopyGameObject()
        {
            if (GUILayout.Button("复制一个GameObject", GUILayout.Height(50)))
            {
                if (Selection.gameObjects.Length > 0)
                {
                    m_CopyGo = Selection.gameObjects[0];
                    this.ShowNotification(new GUIContent("复制:" + m_CopyGo.name + ",完成!"));
                }
                else
                {
                    this.ShowNotification(new GUIContent("请选择一个GameObject!!"));
                }
            }
            if (GUILayout.Button("黏贴一个GameObject", GUILayout.Height(50)))
            {
                if (Selection.gameObjects.Length > 0)
                {
                    var pasteGO = Selection.gameObjects[0];
                    Undo.RecordObject(pasteGO, "pasteGO");
                    if (m_CopyGo == null)
                    {
                        this.ShowNotification(new GUIContent("请先复制一个GameObject!!"));
                        return;
                    }
                    var copyComponents = m_CopyGo.GetComponents<Component>();
                    var pasteComponents = pasteGO.GetComponents<Component>();
                    foreach (var copyComponent in copyComponents)
                    {
                        bool isPaste = false;
                        foreach (var pasteComponent in pasteComponents)
                        {
                            if (pasteComponent.GetType() == copyComponent.GetType())
                            {
                                if (copyComponent is Transform)
                                {
                                    (pasteComponent as Transform).position = (copyComponent as Transform).position;
                                    break;
                                }
                                //怎么黏贴?
                                if (UnityEditorInternal.ComponentUtility.CopyComponent(copyComponent))
                                {
                                    UnityEditorInternal.ComponentUtility.PasteComponentValues(pasteComponent);
                                    isPaste = true;
                                }
                                break;
                            }
                        }

                        if (isPaste == false)
                        {
                            UnityEditorInternal.ComponentUtility.CopyComponent(copyComponent);
                            UnityEditorInternal.ComponentUtility.PasteComponentAsNew(pasteGO);//新增组件
                        }
                    }

                    m_CopyGo = null;
                }
                else
                {
                    this.ShowNotification(new GUIContent("请选择一个GameObject!!"));
                }
            }
        }
        #region UIReferences
        UISerialized FindReferences<T>(Component component,out int index) where T : Component
        {
            index = -1;
            if (Selection.activeGameObject == null)
            {
                ShowNotification(new GUIContent("请选择一个物体"));
                return null;
            }

            var uis = GameObject.FindObjectsOfType<UISerialized>();
            if (uis != null)
            {
                foreach (var ui in uis)
                {
                    if (ui.IsExist<T>(component))
                    {
                        index = ui.GetIndex<T>(component);
                        return ui;
                    }
                }
            }

            return null;
        }
        //------------------------------------------------------
        /// <summary>
        /// 查找最近的一个 UISerialized 组件是否引用改组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="component"></param>
        /// <returns></returns>
        UISerialized FindUISerializedReferences()
        {
            if (Selection.activeTransform == null)
            {
                ShowNotification(new GUIContent("请选择一个父物体"));
                return null;
            }

            UISerialized ui = null;
            Transform trs = Selection.activeTransform;
            while (ui == null && trs.parent != null)//获取到距离当前组件最近的一个 UISerialized
            {
                if (trs.GetComponent<UISerialized>())
                {
                    ui = trs.GetComponent<UISerialized>();
                }
                else
                {
                    trs = trs.parent;
                }
            }
            return ui;
        }
        //------------------------------------------------------
        void AddSelectToUISerialized()
        {
            if (GUILayout.Button("添加选中物体到uiSerialized"))
            {
                var selectList = Selection.gameObjects;

                if (selectList.Length <= 0)
                {
                    this.ShowNotification(new GUIContent("当前没有选择物体!!"));
                    return;
                }

                UISerialized uISerialized = FindUISerializedReferences();
                if (uISerialized == null)
                {
                    return;
                }
                Undo.RecordObject(uISerialized, "AddSelectToUISerialized");
                foreach (var item in selectList)
                {
                    AddGameObjectToUISerialized(uISerialized, item);
                }
                ShowNotification(new GUIContent("添加完成!!"));
            }
        }
        //------------------------------------------------------
        void AddComponentToUISerialized<T>(UISerialized ui, Component component) where T : Graphic
        {
            if (m_IsAddReferencesToggle == false)
            {
                return;
            }
            if (component == null || ui == null)
            {
                ShowNotification(new GUIContent("添加到 UISerialized 组件失败"));
                return;
            }

            //先判断是否存在
            if (ui.IsExist<T>(component))//已存在应该就不用添加了
            {
                //int index = ui.GetIndex<T>(component);
                //ui.SetWidget<T>(index, component);
                ShowNotification(new GUIContent(" UISerialized 中已存在"));
                return;
            }

            //再判断是否长度为0需要创建
            if (ui.Widgets == null)
            {
                ui.Widgets = new UISerialized.Widget[1];
                ui.Widgets[0].widget = component;
            }
            else
            {
                int length = ui.Widgets.Length;
                Array.Resize<TopGame.UI.UISerialized.Widget>(ref ui.Widgets, length + 1);
                ui.Widgets[length] = new UISerialized.Widget();
                ui.Widgets[length].widget = component;
            }
        }
        //------------------------------------------------------
        void AddGameObjectToUISerialized(UISerialized ui, GameObject go)
        {
            if (m_IsAddReferencesToggle == false)
            {
                return;
            }
            if (go == null || ui == null)
            {
                ShowNotification(new GUIContent("添加到 UISerialized 组件失败"));
                return;
            }

            //再判断是否长度为0需要创建
            if (ui.Elements == null)
            {
                ui.Elements = new GameObject[1]; ;
                ui.Elements[0] = go;
            }
            else
            {
                int length = ui.Elements.Length;
                Array.Resize<GameObject>(ref ui.Elements, length + 1);
                ui.Elements[length] = go;
            }
        }
        #endregion
        //------------------------------------------------------
        #region CreatUI
        bool m_IsAddReferencesToggle = false;
        void IsAddReferencesToggle()
        {
            m_IsAddReferencesToggle = EditorGUILayout.Toggle(new GUIContent("是否添加控件到引用"),m_IsAddReferencesToggle);
        }

        //------------------------------------------------------
        void CreateImage()
        {
            if (GUILayout.Button("创建Image", new GUILayoutOption[] { GUILayout.Height(30) }))
            {
                if (Selection.activeTransform && Selection.activeTransform.GetComponentInParent<Canvas>())
                {
                    GameObject go = new GameObject(Selection.activeGameObject.name + "Image", typeof(Image));
                    Image img = go.GetComponent<Image>();
                    img.raycastTarget = false;
                    go.transform.SetParent(Selection.activeTransform, false);
                    UISerialized ui = FindUISerializedReferences();
                    AddComponentToUISerialized<Image>(ui,img);
                    go.layer = LayerMask.NameToLayer("UI");
                    Selection.activeGameObject = go;
                }
            }
        }
        //------------------------------------------------------
        //------------------------------------------------------
        void CreateText()
        {
            if (GUILayout.Button("创建Text", new GUILayoutOption[] { GUILayout.Height(30) }))
            {
                if (Selection.activeTransform && Selection.activeTransform.GetComponentInParent<Canvas>())
                {
                    GameObject go = new GameObject(Selection.activeGameObject.name + "Text", typeof(Text));
                    Text text = go.GetComponent<Text>();

                    text.raycastTarget = false;
                    text.font = AssetDatabase.LoadAssetAtPath<Font>("Assets/Datas/Fonts/default.ttf");
                    text.supportRichText = false;
                    text.fontSize = 20;
                    text.alignment = TextAnchor.MiddleCenter;
                    text.rectTransform.sizeDelta = new Vector2(100,100);//必要时,可暴露出来给外部面板填写参数
                    text.text = "Hi";
                    //text.rectTransform.anchorMin = Vector2.zero;
                    //text.rectTransform.anchorMax = Vector2.one;
                    //text.rectTransform.sizeDelta = Vector2.zero;

                    go.transform.SetParent(Selection.activeTransform, false);
                    go.layer = LayerMask.NameToLayer("UI");
                    UISerialized ui = FindUISerializedReferences();
                    AddComponentToUISerialized<Text>(ui, text);
                    Selection.activeGameObject = go;
                }
            }
        }
        //------------------------------------------------------
        void CreateBtn()
        {
            if (GUILayout.Button("创建Button", new GUILayoutOption[] { GUILayout.Height(30) }))
            {
                if (Selection.activeTransform && Selection.activeTransform.GetComponentInParent<Canvas>())
                {
                    GameObject go = new GameObject("Btn", typeof(EventTriggerListener), typeof(EmptyImage));
                    GameObject icon = new GameObject("icon", typeof(Image));

                    go.transform.SetParent(Selection.activeTransform, false);
                    var goRect = go.transform as RectTransform;
                    goRect.sizeDelta = new Vector2(220,100);

                    icon.GetComponent<Image>().raycastTarget = false;
                    icon.transform.SetParent(go.transform, false);
                    var iconRect = icon.transform as RectTransform;
                    iconRect.sizeDelta = new Vector2(220, 79);
                    icon.GetComponent<Image>().sprite = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>("Assets/DatasRef/UI/Textures/common/buttons/button_general_A02.png");
                    //UISerialized ui = FindUISerializedReferences();
                    //AddComponentToUISerialized<Text>(ui, text);//按钮不需要添加到UISerialized中


                    go.layer = LayerMask.NameToLayer("UI");
                    icon.layer = LayerMask.NameToLayer("UI");

                    Selection.activeGameObject = go;
                }
            }
        }
        //------------------------------------------------------
        void CreateList()
        {
            if (GUILayout.Button("创建List", new GUILayoutOption[] { GUILayout.Height(30) }))
            {
                if (Selection.activeTransform && Selection.activeTransform.GetComponentInParent<Canvas>())
                {
                    ListView asset = UnityEditor.AssetDatabase.LoadAssetAtPath<ListView>("Assets/DatasRef/UI/Prefabs/Scroll View.prefab");
                    var go = Instantiate<ListView>(asset);

                    go.transform.SetParent(Selection.activeTransform, false);
                    
                    go.gameObject.layer = LayerMask.NameToLayer("UI");
                    go.name = "List";

                    Selection.activeGameObject = go.gameObject;
                }
            }
        }
        //------------------------------------------------------
        void CreateProgressBar()
        {
            if (GUILayout.Button("创建 ProgressBar", new GUILayoutOption[] { GUILayout.Height(30) }))
            {
                if (Selection.activeTransform && Selection.activeTransform.GetComponentInParent<Canvas>())
                {
                    GameObject asset = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/DatasRef/UI/Prefabs/ProgressBar.prefab");
                    var go = Instantiate<GameObject>(asset);

                    go.transform.SetParent(Selection.activeTransform, false);

                    go.gameObject.layer = LayerMask.NameToLayer("UI");
                    go.name = "ProgressBar";

                    Selection.activeGameObject = go.gameObject;
                }
            }
        }
        #endregion
        //------------------------------------------------------
        #region ReplaceComponent
        void TextReplaceWithHexText()
        {
            if (GUILayout.Button("Text替换成HexText"))
            {
                var selectList = Selection.gameObjects;

                if (selectList.Length <= 0)
                {
                    this.ShowNotification(new GUIContent("当前没有选择物体!!"));
                    return;
                }

                Undo.RecordObjects(selectList, "TextReplaceWithHexText");

                List<Text> texts = new List<Text>();
                foreach (var item in selectList)
                {
                    var items = item.GetComponentsInChildren<Text>(true);
                    foreach (var img in items)
                    {
                        texts.Add(img);
                    }
                }

                foreach (var item in texts)
                {
                    Text text = item;
                    if (text == null)
                    {
                        continue;
                    }

                    var ui = FindReferences<Text>(text,out int index);

                    var font = text.font;
                    var fontStyle = text.fontStyle;
                    var fontSize = text.fontSize;
                    var lineSpacing = text.lineSpacing;
                    var supportRichText = text.supportRichText;
                    var alignment = text.alignment;
                    //var horizontalOverflow = text.horizontalOverflow;//在HexText.Awake中进行设置成Wrap
                    //var verticalOverflow = text.verticalOverflow;//在HexText.Awake中进行设置成Truncate
                    //var resizeTextForBestFit = text.resizeTextForBestFit;
                    var color = text.color;
                    var material = text.material;
                    //var raycast = text.raycastTarget;
                    var txt = text.text;

                    GameObject go = item.gameObject;
                    DestroyImmediate(text);
                    HexText hexText = Undo.AddComponent<HexText>(go);// go.AddComponent<HexText>();
                    hexText.font = font;
                    hexText.fontSize = fontSize;
                    hexText.resizeTextMaxSize = fontSize;
                    hexText.fontStyle = fontStyle;
                    hexText.lineSpacing = lineSpacing;
                    hexText.supportRichText = supportRichText;
                    hexText.alignment = alignment;
                    hexText.color = color;
                    hexText.material = material;
                    hexText.text = txt;

                    if (ui && index >= 0)
                    {
                        ui.SetWidget<HexText>(index, hexText);
                        Debug.Log("替换了:" + ui.name + ",的ui引用组件中的:" + go.name +",为HexText");
                    }

                    UnityEditor.EditorUtility.SetDirty(go);
                }
                ShowNotification(new GUIContent("替换完成"));
            }
        }
        //------------------------------------------------------
        void ImageConvertRawImage()
        {
            if (GUILayout.Button("Image互转RawImage"))
            {
                var selectList = Selection.gameObjects;

                if (selectList.Length <= 0)
                {
                    this.ShowNotification(new GUIContent("当前没有选择物体!!"));
                    return;
                }

                Undo.RecordObjects(selectList, "selectList5");

                List<Image> images = new List<Image>();
                foreach (var item in selectList)
                {
                    var items = item.GetComponentsInChildren<Image>(true);
                    foreach (var img in items)
                    {
                        if (img.GetType() == typeof(Image))
                        {
                            images.Add(img);
                        }
                    }
                }

                List<RawImage> rawImages = new List<RawImage>();
                foreach (var item in selectList)
                {
                    var items = item.GetComponentsInChildren<RawImage>(true);
                    foreach (var img in items)
                    {
                        if (img.GetType() == typeof(RawImage))
                        {
                            rawImages.Add(img);
                        }
                    }
                }




                foreach (var item in images)
                {
                    Image image = item;
                    if (image == null)
                    {
                        continue;
                    }

                    var ui = FindReferences<Image>(image,out int index);

                    Texture2D texture = null;
                    if (image.sprite)
                    {
                        texture = image.sprite.texture;
                    }
                    string path = "";
                    if (texture != null)
                    {
                        path = AssetDatabase.GetAssetPath(texture.GetInstanceID());
                    }

                    var color = image.color;
                    var material = image.material;
                    var raycast = image.raycastTarget;
                    var maskable = image.maskable;

                    GameObject go = item.gameObject;
                    DestroyImmediate(image);

                    RawImage raw = go.AddComponent<RawImage>();
                    //image.sprite = Sprite.Create((Texture2D)texture, new Rect(Vector2.zero, texture.texelSize), new Vector2(0.5f,0.5f));
                    if (string.IsNullOrWhiteSpace(path))
                    {
                        raw.texture = null;
                    }
                    else
                    {
                        raw.texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
                    }

                    raw.color = color;
                    raw.material = material;
                    raw.raycastTarget = raycast;
                    raw.maskable = maskable;

                    if (ui != null && index != -1)
                    {
                        ui.SetWidget<RawImage>(index, raw);
                        Debug.Log("ui:" + ui.name + ",设置组件:" + raw.name);
                    }

                    UnityEditor.EditorUtility.SetDirty(go);

                    this.ShowNotification(new GUIContent(go.name + " 转换成功!"));
                }

                foreach (var item in rawImages)
                {
                    var rawImage = item;
                    if (rawImage == null)
                    {
                        continue;
                    }

                    var ui = FindReferences<Image>(rawImage,out int index);

                    Texture texture = null;
                    if (rawImage.texture)
                    {
                        texture = rawImage.texture;
                    }
                    string path = "";
                    if (texture)
                    {
                        path = AssetDatabase.GetAssetPath(texture.GetInstanceID());
                    }

                    var color = rawImage.color;
                    var material = rawImage.material;
                    var raycast = rawImage.raycastTarget;
                    var maskable = rawImage.maskable;

                    GameObject go = item.gameObject;
                    DestroyImmediate(rawImage);

                    Image image = go.AddComponent<Image>();
                    //image.sprite = Sprite.Create((Texture2D)texture, new Rect(Vector2.zero, texture.texelSize), new Vector2(0.5f,0.5f));
                    if (string.IsNullOrWhiteSpace(path))
                    {
                        image.sprite = null;
                    }
                    else
                    {
                        image.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
                    }
                    image.color = color;
                    image.material = material;
                    image.raycastTarget = raycast;
                    image.maskable = maskable;

                    if (ui != null && index != -1)
                    {
                        ui.SetWidget<Image>(index, image);
                        Debug.Log("ui:" + ui.name + ",设置组件:" + image.name);
                    }

                    UnityEditor.EditorUtility.SetDirty(go);

                    this.ShowNotification(new GUIContent(go.name + " 转换成功!"));
                }


                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            }

        }
        //------------------------------------------------------
        void ConvertSelectEmptyImage()
        {
            if (GUILayout.Button("选择的转EmptyImage"))
            {
                var selectList = Selection.gameObjects;

                if (selectList.Length <= 0)
                {
                    this.ShowNotification(new GUIContent("当前没有选择物体!!"));
                    return;
                }

                Undo.RecordObjects(selectList, "selectList4");

                List<MaskableGraphic> components = new List<MaskableGraphic>();
                foreach (var item in selectList)
                {
                    var images = item.GetComponent<Image>();
                    var rawImages = item.GetComponent<RawImage>();
                    if (images != null)
                    {
                        components.Add(images);
                    }
                    if (rawImages != null)
                    {
                        components.Add(rawImages);
                    }
                }

                foreach (var item in components)
                {
                    var raycast = item.raycastTarget;
                    GameObject go = item.gameObject;
                    DestroyImmediate(item);
                    EmptyImage rawImage = go.AddComponent<EmptyImage>();
                    rawImage.raycastTarget = raycast;

                    UnityEditor.EditorUtility.SetDirty(go);
                }
                ShowNotification(new GUIContent("替换完成"));
            }
        }
        #endregion
        //------------------------------------------------------
        //------------------------------------------------------
        int m_OrderValue;
        void SetParticleOrder()
        {
            EditorGUILayout.BeginHorizontal();

            m_OrderValue = EditorGUILayout.IntField("层级:", m_OrderValue);
            if (GUILayout.Button("叠加层级"))
            {
                var selectList = Selection.gameObjects;

                if (selectList.Length <= 0)
                {
                    this.ShowNotification(new GUIContent("当前没有选择物体!!"));
                    return;
                }

                List<Renderer> allParticles = new List<Renderer>();
                foreach (var go in selectList)
                {
                    var renders = go.GetComponentsInChildren<Renderer>();
                    foreach (var renderer in renders)
                    {
                        allParticles.Add(renderer);
                    }
                }

                foreach (var renderer in allParticles)
                {
                    renderer.sortingOrder += m_OrderValue;
                }
                this.ShowNotification(new GUIContent("设置层级完成!!"));
            }

            EditorGUILayout.EndHorizontal();
        }
        //------------------------------------------------------
        void SetParticleMasking()
        {
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("设置粒子Masking VisibleInsideMask"))
            {
                var selectList = Selection.gameObjects;

                if (selectList.Length <= 0)
                {
                    this.ShowNotification(new GUIContent("当前没有选择物体!!"));
                    return;
                }

                List<ParticleSystemRenderer> allParticles = new List<ParticleSystemRenderer>();
                foreach (var go in selectList)
                {
                    var renders = go.GetComponentsInChildren<Renderer>();
                    foreach (var renderer in renders)
                    {
                        if (renderer is ParticleSystemRenderer)
                        {
                            allParticles.Add(renderer as ParticleSystemRenderer);
                        }
                    }
                }

                foreach (var renderer in allParticles)
                {
                    renderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
                    EditorUtility.SetDirty(renderer.gameObject);
                }
                this.ShowNotification(new GUIContent("设置完成!!"));
            }

            if (GUILayout.Button("设置粒子 None Masking"))
            {
                var selectList = Selection.gameObjects;

                if (selectList.Length <= 0)
                {
                    this.ShowNotification(new GUIContent("当前没有选择物体!!"));
                    return;
                }

                List<ParticleSystemRenderer> allParticles = new List<ParticleSystemRenderer>();
                foreach (var go in selectList)
                {
                    var renders = go.GetComponentsInChildren<Renderer>();
                    foreach (var renderer in renders)
                    {
                        if (renderer is ParticleSystemRenderer)
                        {
                            allParticles.Add(renderer as ParticleSystemRenderer);
                        }
                    }
                }

                foreach (var renderer in allParticles)
                {
                    renderer.maskInteraction = SpriteMaskInteraction.None;
                    EditorUtility.SetDirty(renderer.gameObject);
                }
                this.ShowNotification(new GUIContent("设置完成!!"));
            }

            EditorGUILayout.EndHorizontal();
        }
        //------------------------------------------------------
        void CopySelectPosition()
        {
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("复制选择的UI锚点坐标"))
            {
                var selectList = Selection.gameObjects;

                if (selectList.Length <= 0)
                {
                    this.ShowNotification(new GUIContent("当前没有选择物体!!"));
                    return;
                }

                StringBuilder stringBuilder = new StringBuilder();

                List<GameObject> selected = new List<GameObject>(selectList);
                selected.Sort(SortFunc);

                foreach (var go in selected)
                {
                    if (go.transform is RectTransform)
                    {
                        var rect = go.transform as RectTransform;
                        stringBuilder.AppendLine(rect.anchoredPosition.ToString());
                    }
                }

                GUIUtility.systemCopyBuffer = stringBuilder.ToString();
                Debug.Log(stringBuilder.ToString());

                this.ShowNotification(new GUIContent("复制完成!!"));
            }

            

            EditorGUILayout.EndHorizontal();
        }
        //------------------------------------------------------
        private int SortFunc(GameObject x, GameObject y)
        {
            int indexX =  x.transform.GetSiblingIndex();
            int indexY = y.transform.GetSiblingIndex();
            if (indexX > indexY)
            {
                return 1;
            }
            else if (indexX == indexY)
            {
                return 0;
            }
            return -1;
        }
        //------------------------------------------------------
        void SetAnchor()
        {
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("设置UI锚点按比例拉伸大小"))
            {
                var selectList = Selection.gameObjects;

                if (selectList.Length <= 0)
                {
                    this.ShowNotification(new GUIContent("当前没有选择物体!!"));
                    return;
                }
                
                for (int i = 0; i < selectList.Length; i++)
                {
                    var ui= selectList[i];
                    Undo.RecordObject(ui.transform, "setAnchors");
                    if (ui.transform is RectTransform && ui.transform.parent && ui.transform.parent is RectTransform)
                    {
                        var parentRect = ui.transform.parent as RectTransform;
                        var rect = ui.transform as RectTransform;
                        //rect.anchorMax
                        //rect.anchorMin
                        //用当前大小,除父类大小,计算出锚点
                        float xMin = (rect.rect.xMin + parentRect.rect.width /2 + rect.anchoredPosition.x) / parentRect.rect.width;
                        float yMin = (rect.rect.yMin + parentRect.rect.height / 2 + rect.anchoredPosition.y) / parentRect.rect.height;

                        float xMax = (rect.rect.xMax + parentRect.rect.width / 2 + rect.anchoredPosition.x) / parentRect.rect.width;
                        float yMax = (rect.rect.yMax + parentRect.rect.height / 2 + rect.anchoredPosition.y) / parentRect.rect.height;

                        rect.anchorMin = new Vector2(xMin, yMin);
                        rect.anchorMax = new Vector2(xMax, yMax);
                        rect.anchoredPosition = Vector2.zero;
                        rect.sizeDelta = Vector2.zero;
                    }
                }

                this.ShowNotification(new GUIContent("设置完成!!"));
            }

            if (GUILayout.Button("设置UI锚点到当前UI位置"))
            {
                var selectList = Selection.gameObjects;

                if (selectList.Length <= 0)
                {
                    this.ShowNotification(new GUIContent("当前没有选择物体!!"));
                    return;
                }

                for (int i = 0; i < selectList.Length; i++)
                {
                    var ui = selectList[i];
                    Undo.RecordObject(ui.transform, "setAnchors2");
                    if (ui.transform is RectTransform && ui.transform.parent && ui.transform.parent is RectTransform)
                    {
                        var parentRect = ui.transform.parent as RectTransform;
                        var rect = ui.transform as RectTransform;

                        //先锚点重置到中心点
                        //rect.anchorMin = new Vector2(0.5f, 0.5f);
                        //rect.anchorMax = new Vector2(0.5f, 0.5f);

                        Vector2 anchorPosition = rect.anchoredPosition;

                        //rect.point

                        //用当前大小,除父类大小,计算出锚点
                        float x = (rect.rect.center.x + parentRect.rect.width / 2 + anchorPosition.x) / parentRect.rect.width;
                        float y = (rect.rect.center.y + parentRect.rect.height / 2 + anchorPosition.y) / parentRect.rect.height;

                        rect.anchorMin = new Vector2(x, y);
                        rect.anchorMax = new Vector2(x, y);
                        rect.anchoredPosition = Vector2.zero;
                    }
                }

                this.ShowNotification(new GUIContent("设置完成!!"));
            }



            EditorGUILayout.EndHorizontal();
        }
        //------------------------------------------------------
        void ReloadScene()
        {
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("重新加载当前场景(不保存)"))
            {
                var scene = SceneManager.GetActiveScene();
                if (scene != null && !string.IsNullOrWhiteSpace(scene.name) && !string.IsNullOrWhiteSpace(scene.path))
                {
                    UnityEditor.SceneManagement.EditorSceneManager.OpenScene(scene.path);
                }
            }

            EditorGUILayout.EndHorizontal();
        }
    }
}
