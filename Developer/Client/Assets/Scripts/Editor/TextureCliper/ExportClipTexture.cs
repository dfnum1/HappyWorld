#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using TopGame.Data;
namespace TopGame.ED
{
    public class ExportClipTexture : EditorWindow
    {
		public static ExportClipTexture current { get; private set; }
        TextureCliperAssets.Clip m_clip;
        bool firstFrame = true;

        Texture2D m_pTexture;
        Texture2D m_pClipExport;
        Vector2Int m_ExportPos = Vector2Int.zero;
        Vector2Int m_ExportSize = Vector2Int.zero;
        bool m_bConvertSprite = true;

        string m_ExportName;
        public static ExportClipTexture Show(TextureCliperAssets.Clip target, float width = 200) {
            ExportClipTexture window = EditorWindow.GetWindow<ExportClipTexture>(true, "导出 " + target.asset, true);
			if (current != null) current.Close();
			current = window;
			window.m_clip = target;
			window.minSize = new Vector2(width, width);
			window.position = new Rect(0, 0, width, width);
			GUI.FocusControl("ClearAllFocus");
			window.UpdatePositionToMouse();
            window.firstFrame = true;

            if (target.group != null && !string.IsNullOrEmpty(target.group.path))
                window.m_pTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(target.group.path);
            else
                window.m_pTexture = null;
            window.m_ExportSize.x = Mathf.FloorToInt(target.source.width);
            window.m_ExportSize.y = Mathf.FloorToInt(target.source.height);
            window.m_ExportPos.x = Mathf.FloorToInt(target.source.x);
            window.m_ExportPos.y = Mathf.FloorToInt(target.source.y);
            window.m_ExportName = target.asset;
            return window;
		}
        //------------------------------------------------------
        private void OnDisable()
        {
            current = null;
        }
        //------------------------------------------------------
        public static void Hide()
        {
            if (current) current.Close();
        }
        //------------------------------------------------------
        private void UpdatePositionToMouse()
        {
			if (Event.current == null) return;
			Vector3 mousePoint = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
			Rect pos = position;
			pos.x = mousePoint.x - position.width * 0.5f;
			pos.y = mousePoint.y - 10;
			position = pos;
		}
        //------------------------------------------------------
        private void OnLostFocus()
        {
			// Make the popup close on lose focus
			Close();
		}
        //------------------------------------------------------
        private void OnGUI()
        {
			if (firstFrame)
            {
				UpdatePositionToMouse();
				firstFrame = false;
			}
            if(m_pTexture == null)
            {
                this.ShowNotification(new GUIContent("没有原图！"));
                return;
            }
            m_ExportName = EditorGUILayout.TextField("导出名", m_ExportName);
            EditorGUILayout.LabelField("X:" + m_ExportPos.x);
            EditorGUILayout.LabelField("Y:" + m_ExportPos.y);
            m_ExportSize.x = EditorGUILayout.IntField("导出宽度", m_ExportSize.x);
            m_ExportSize.y = EditorGUILayout.IntField("导出高度", m_ExportSize.y);

            m_bConvertSprite = EditorGUILayout.Toggle("自动转为精灵格式", m_bConvertSprite);
            EditorGUI.BeginDisabledGroup(m_ExportSize.x <= 0 || m_ExportSize.y <= 0 || string.IsNullOrEmpty(m_ExportName)) ;
            if(GUILayout.Button("导出"))
            {
                ExportTexture();
            }
            EditorGUI.EndDisabledGroup();
        }
        //------------------------------------------------------
        void ExportTexture()
        {
            Texture2D clipTexture = null;
            TextureConvertLogic.TextureZoomCopy(m_pTexture, m_clip.source, false, true, ref clipTexture);
            if (m_ExportSize.x != (int)m_clip.source.width || m_ExportSize.y != (int)m_clip.source.height)
            {
                m_pClipExport = ScaleTexture(clipTexture, m_ExportSize.x, m_ExportSize.y);
            }
            else
                m_pClipExport = clipTexture;

            string exportRoot = Application.dataPath;
            if(m_clip.group!=null && m_clip.group.catchData!=null)
            {
                TextureCliperGroupCatch.GroupData editData = m_clip.group.catchData as TextureCliperGroupCatch.GroupData;
                if (editData != null && !string.IsNullOrEmpty(editData.saveDir))
                    exportRoot = editData.saveDir;
            }

           string  path = EditorUtility.SaveFilePanel("导出", exportRoot, m_ExportName, "png");
            path = path.Replace("\\", "/");
            if (path.Contains(Application.dataPath) && path.Contains("Assets/DataRef/"))
            {
                EditorUtility.DisplayDialog("提示", "保存的路径不能为引用目录,必须是Assets/Datas下的目录", "好的");
                return;
            }
            System.IO.File.WriteAllBytes(path, m_pClipExport.EncodeToPNG());

            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            if(path.Contains(Application.dataPath))
            {
                path = path.Replace(Application.dataPath, "Assets");
                TextureImporter assetImport = AssetImporter.GetAtPath(path) as TextureImporter;
                if(assetImport!=null)
                {
                    assetImport.isReadable = false;
                    assetImport.mipmapEnabled = false;
                    assetImport.wrapMode = TextureWrapMode.Clamp;

                    TextureImporter sourceImport = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(m_pTexture)) as TextureImporter;
                    if(sourceImport!=null)
                    {
                        TextureImporterPlatformSettings defalut = sourceImport.GetDefaultPlatformTextureSettings();
                        if (defalut != null) assetImport.SetPlatformTextureSettings(defalut);
                        TextureImporterPlatformSettings android = sourceImport.GetPlatformTextureSettings("Android");
                        if (android != null) assetImport.SetPlatformTextureSettings(android);
                        TextureImporterPlatformSettings iphone = sourceImport.GetPlatformTextureSettings("iPhone");
                        if (iphone != null) assetImport.SetPlatformTextureSettings(iphone);
                    }
                    if (m_bConvertSprite)
                    {
                        assetImport.spriteImportMode = SpriteImportMode.Single;
                        assetImport.textureType = TextureImporterType.Sprite;
                    }
                    assetImport.SaveAndReimport();
                    m_clip.texture = path;
                }
            }

            GameObject.DestroyImmediate(m_pClipExport);
            if(clipTexture!= m_pClipExport)
                GameObject.DestroyImmediate(clipTexture);
            clipTexture = null;
            m_pClipExport = null;
        }
        //------------------------------------------------------
        public static Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
        {
            Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, true);
            Color[] rpixels = result.GetPixels(0);
            float incX = ((float)1 / source.width) * ((float)source.width / targetWidth);
            float incY = ((float)1 / source.height) * ((float)source.height / targetHeight);
            for (int px = 0; px < rpixels.Length; px++)
            {
                rpixels[px] = source.GetPixelBilinear(incX * ((float)px % targetWidth), incY * ((float)Mathf.Floor(px / targetWidth)));
            }
            result.SetPixels(rpixels, 0);
            result.Apply();
            return result;
        }
    }
}
#endif