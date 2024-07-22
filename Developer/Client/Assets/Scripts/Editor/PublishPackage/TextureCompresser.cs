/********************************************************************
生成日期:	24:7:2019   11:12
类    名: 	TextureCompresser
作    者:	HappLI
描    述:	TextureCompresser
*********************************************************************/
using System.Collections.Generic;
using System.IO;
using TopGame.Base;
using TopGame.Data;
using UnityEditor;
using UnityEngine;

namespace TopGame.ED
{
    public class TextureCompresser : EditorWindow
    {
        [System.Serializable]
        public class Item
        {
            public string path = "";
            public int maxSize = 1024;
            public bool mipMapEnable = false;
            public int mipMapBaise = 0;
            public bool streamTexture = false;
            public bool convertASTC = true;
            public TextureImporterFormat astcFormat = TextureImporterFormat.ASTC_6x6;
            public bool expand = false;
        }
        [System.Serializable]
        public class SymbolData
        {
            public string symbol;
            public TextureImporterFormat format = TextureImporterFormat.ASTC_6x6;
            public int maxSize = 0;
            public bool expand = false;

            [System.NonSerialized]
            private List<string> m_vSymbols;
            public bool IsContain(string text)
            {
                if(m_vSymbols == null || m_vSymbols.Count<=0)
                {
                    m_vSymbols = new List<string>(symbol.Split(';'));
                }
                else
                {
                    string strTemp = "";
                    for (int i = 0; i < m_vSymbols.Count; ++i) strTemp += m_vSymbols[i] + ";";
                    if(strTemp.CompareTo(symbol+";")!=0)
                    {
                        m_vSymbols = new List<string>(symbol.Split(';'));
                    }
                }
                for(int i=0; i < m_vSymbols.Count; ++i)
                {
                    if (!text.Contains(m_vSymbols[i]))
                        return false;
                }
                return true;
            }
        }
        [System.Serializable]
        public class Compress
        {
            public bool expandItems;
            public bool expandSymbol;
            public List<Item> datas = new List<Item>();
            public List<SymbolData> symbolDatas = new List<SymbolData>();

            public SymbolData FindSymbol(string text)
            {
                for (int i=0; i < symbolDatas.Count; ++i)
                {
                    if (string.IsNullOrEmpty(symbolDatas[i].symbol)) continue;
                    if (symbolDatas[i].IsContain(text))
                        return symbolDatas[i];
                }
                return null;
            }
        }
        Compress m_pCompress = new Compress();
        Vector2 m_Scroll = Vector2.zero;
        static string[] TEXTURE_SIZE_POP = new string[] { "2048", "1024", "512", "256", "128", "64", "32", "16", "8", "4", "2", "0" };
        //------------------------------------------------------
        static TextureCompresser ms_pInstance = null;
        [MenuItem("Tools/图片压缩配置")]
        public static void ShowPop()
        {
            if (EditorApplication.isCompiling)
            {
                EditorUtility.DisplayDialog("警告", "请等待编辑器完成编译再执行此功能", "确定");
                return;
            }
            if (ms_pInstance == null)
            {
                TextureCompresser window = EditorWindow.GetWindow<TextureCompresser>();
                window.titleContent = new GUIContent("图片压缩配置");
            }
        }
        //------------------------------------------------------
        private void OnEnable()
        {
            m_pCompress = LoadData();
            if (m_pCompress == null) m_pCompress = new Compress();
        }
        //------------------------------------------------------
        private void OnDisable()
        {
            SaveData(m_pCompress);
        }
        //------------------------------------------------------
        private void OnGUI()
        {
            m_Scroll = EditorGUILayout.BeginScrollView(m_Scroll);
            m_pCompress.expandSymbol = EditorGUILayout.Foldout(m_pCompress.expandSymbol, "特殊标志");
            if (m_pCompress.expandSymbol)
            {
                if (GUILayout.Button("新建", new GUILayoutOption[] { GUILayout.Height(30) }))
                {
                    m_pCompress.symbolDatas.Add(new SymbolData() { symbol="" });
                }
                for (int i = 0; i < m_pCompress.symbolDatas.Count; ++i)
                {
                    SymbolData symbol = m_pCompress.symbolDatas[i];
                    GUILayout.BeginHorizontal();
                    symbol.expand = EditorGUILayout.Foldout(symbol.expand, "[" + symbol.symbol + "]");
                    if (GUILayout.Button("移除", new GUILayoutOption[] { GUILayout.Width(80) }))
                    {
                        m_pCompress.symbolDatas.RemoveAt(i);
                        break;
                    }
                    if(m_pCompress.symbolDatas.Count>1)
                    {
                        if (i >= 1 && i+1 < m_pCompress.symbolDatas.Count)
                        {
                            if (GUILayout.Button("↑", new GUILayoutOption[] { GUILayout.MaxWidth(20) }))
                            {
                                var temp = m_pCompress.symbolDatas[i - 1];
                                m_pCompress.symbolDatas[i - 1] = symbol;
                                m_pCompress.symbolDatas[i] = temp;
                                break;
                            }
                            if (GUILayout.Button("↓", new GUILayoutOption[] { GUILayout.MaxWidth(20) }))
                            {
                                var temp = m_pCompress.symbolDatas[i + 1];
                                m_pCompress.symbolDatas[i + 1] = symbol;
                                m_pCompress.symbolDatas[i] = temp;
                                break;
                            }
                        }
                        else
                        {
                            if (i == 0 && i + 1 < m_pCompress.symbolDatas.Count)
                            {
                                if (GUILayout.Button("↓", new GUILayoutOption[] { GUILayout.MaxWidth(20) }))
                                {
                                    var temp = m_pCompress.symbolDatas[i + 1];
                                    m_pCompress.symbolDatas[i + 1] = symbol;
                                    m_pCompress.symbolDatas[i] = temp;
                                    break;
                                }
                            }
                            else if (i == m_pCompress.symbolDatas.Count - 1)
                            {
                                if (GUILayout.Button("↑", new GUILayoutOption[] { GUILayout.MaxWidth(20) }))
                                {
                                    var temp = m_pCompress.symbolDatas[i - 1];
                                    m_pCompress.symbolDatas[i - 1] = symbol;
                                    m_pCompress.symbolDatas[i] = temp;
                                    break;
                                }
                            }
                        }
                    }
                    
                    GUILayout.EndHorizontal();
                    if(symbol.expand)
                    {
                        symbol.symbol = EditorGUILayout.TextField("标识符", symbol.symbol);
                        symbol.format = (TextureImporterFormat)EditorGUILayout.EnumPopup("转化格式", symbol.format);

                        int index = System.Array.IndexOf(TEXTURE_SIZE_POP, symbol.maxSize.ToString());
                        if (index < 0) index = TEXTURE_SIZE_POP.Length - 1;
                        index = EditorGUILayout.Popup("图片最大大小", index, TEXTURE_SIZE_POP);
                        if (index >= 0 && index < TEXTURE_SIZE_POP.Length)
                            symbol.maxSize = int.Parse(TEXTURE_SIZE_POP[index]);
                    }
                }
            }
            m_pCompress.expandItems = EditorGUILayout.Foldout(m_pCompress.expandItems,"压缩列表");
            if(m_pCompress.expandItems)
            {
                if (GUILayout.Button("新建", new GUILayoutOption[] { GUILayout.Height(30) }))
                {
                    m_pCompress.datas.Add(new Item() { path = "", maxSize = 1024 });
                }
                for (int i = 0; i < m_pCompress.datas.Count; ++i)
                {
                    Item item = m_pCompress.datas[i];
                    string name = "";
                    UnityEngine.Object assetObj = null;
                    if (!string.IsNullOrEmpty(item.path))
                    {
                        assetObj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(item.path);
                        if (assetObj != null && AssetDatabase.IsValidFolder(item.path))
                            name = "目录";
                    }
                    GUILayout.BeginHorizontal();
                    item.expand = EditorGUILayout.Foldout(item.expand, "压缩项[" + i + "]" + name);
                    if (GUILayout.Button("移除", new GUILayoutOption[] { GUILayout.Width(80) }))
                    {
                        m_pCompress.datas.RemoveAt(i);
                        break;
                    }
                    GUILayout.EndHorizontal();
                    if (item.expand)
                    {
                        EditorGUI.indentLevel++;

                        assetObj = EditorGUILayout.ObjectField("路径/目录", assetObj, typeof(UnityEngine.Object), false);
                        if (assetObj != null) item.path = AssetDatabase.GetAssetPath(assetObj);
                        else item.path = "";
                        int index = System.Array.IndexOf(TEXTURE_SIZE_POP, item.maxSize.ToString());
                        if (index < 0) index = TEXTURE_SIZE_POP.Length - 1;
                        index = EditorGUILayout.Popup("图片最大大小", index, TEXTURE_SIZE_POP);
                        if (index >= 0 && index < TEXTURE_SIZE_POP.Length)
                            item.maxSize = int.Parse(TEXTURE_SIZE_POP[index]);

                        item.mipMapEnable = EditorGUILayout.Toggle("MipMap", item.mipMapEnable);
                        if (item.mipMapEnable)
                            item.mipMapBaise = EditorGUILayout.IntField("MipMapBize", item.mipMapBaise);
                        item.streamTexture = EditorGUILayout.Toggle("Stream", item.streamTexture);
                        item.convertASTC = EditorGUILayout.Toggle("ASTC", item.convertASTC);
                        if(item.convertASTC)
                            item.astcFormat = (TextureImporterFormat)EditorGUILayout.EnumPopup("转化格式", item.astcFormat);
                        EditorGUI.indentLevel--;
                    }
                }
            }
            EditorGUILayout.EndScrollView();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("执行", new GUILayoutOption[] { GUILayout.Height(80) }))
            {
                Compresser(m_pCompress);
                SaveData(m_pCompress);
            }
            if(GUILayout.Button("提交配置", new GUILayoutOption[] { GUILayout.Height(80) }))
            {
                List<string> files = new List<string>();
                files.Add(Application.dataPath + "/../EditorData/TextureCompress.json");
                UnitySVN.SVNCommit(files.ToArray());
            }
            GUILayout.EndHorizontal();
        }
        //------------------------------------------------------
        public static void Compresser(Compress compress = null)
        {
            if(compress == null)  compress = LoadData();
            if (compress == null) return;
            Dictionary<string, SymbolData> symbols = new Dictionary<string, SymbolData>();
            for (int i = 0; i < compress.symbolDatas.Count; ++i)
            {
                if (string.IsNullOrEmpty(compress.symbolDatas[i].symbol)) continue;
                symbols[compress.symbolDatas[i].symbol] = compress.symbolDatas[i];
            }
            Dictionary<TextureImporter, Item> vCompress = new Dictionary<TextureImporter, Item>();
            for (int i = 0; i < compress.datas.Count; ++i)
            {
                if (string.IsNullOrEmpty(compress.datas[i].path)) continue;
                UnityEngine.Object assetObj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(compress.datas[i].path);
                if (assetObj == null) continue;
                if (AssetDatabase.IsValidFolder(compress.datas[i].path))
                {
                    string[] textures = AssetDatabase.FindAssets("t:Texture", new string[] { compress.datas[i].path });
                    for (int j = 0; j < textures.Length; ++j)
                    {
                        AssetImporter assetImport = AssetImporter.GetAtPath(AssetDatabase.GUIDToAssetPath(textures[j]));
                        if (assetImport is TextureImporter)
                            vCompress[assetImport as TextureImporter] = compress.datas[i];
                    }
                }
                else if (assetObj is Texture)
                {
                    AssetImporter assetImport = AssetImporter.GetAtPath(compress.datas[i].path);
                    if (assetImport is TextureImporter)
                        vCompress[assetImport as TextureImporter] = compress.datas[i];
                }
            }
            EditorUtility.DisplayProgressBar("图片压缩", "", 0);
            float deal = 0;
            foreach (var db in vCompress)
            {
                deal++;
                EditorUtility.DisplayProgressBar("图片压缩", db.Key.assetPath, deal / (float)vCompress.Count);
                TextureImporter import = db.Key;
                if (import.textureShape == TextureImporterShape.TextureCube) continue;
                Item config = db.Value;
                bool bDirtry = false;
                if (import.mipmapEnabled != config.mipMapEnable)
                {
                    bDirtry = true;
                    import.mipmapEnabled = config.mipMapEnable;
                }
                if (import.mipmapEnabled)
                {
                    if (import.mipMapBias != config.mipMapBaise)
                    {
                        bDirtry = true;
                        import.mipMapBias = config.mipMapBaise;
                    }
                }
                if(import.streamingMipmaps != config.streamTexture)
                {
                    bDirtry = true;
                    import.streamingMipmaps = config.streamTexture;
                }
                if(import.isReadable)
                {
                    import.isReadable = false;
                    bDirtry = true;
                }
                SymbolData symbolData = compress.FindSymbol(import.assetPath);

                int maxSize = config.maxSize;
                if (symbolData != null && symbolData.maxSize > 0) maxSize = symbolData.maxSize;

                bool bFormatCheck = false;
                TextureImporterFormat importFormat = TextureImporterFormat.Automatic;
                if (config.convertASTC)
                {
                    bFormatCheck = true;
                    importFormat = config.astcFormat;
                    if (symbolData != null)
                        importFormat = symbolData.format;
                }
                Texture texture = AssetDatabase.LoadAssetAtPath<Texture>(import.assetPath);
                TextureImporterPlatformSettings android = import.GetPlatformTextureSettings("Android");
                if (android != null)
                {
               //     if(importFormat == TextureImporterFormat.Automatic)
               //         importFormat = TextureImporterFormat.ETC2_RGB4
                    if (texture.width > maxSize && texture.height > maxSize && android.maxTextureSize != maxSize)
                    {
                        android.maxTextureSize = maxSize;
                        android.overridden = true;
                        bDirtry = true;
                        import.SetPlatformTextureSettings(android);
                    }
                    if (bFormatCheck && importFormat != TextureImporterFormat.Automatic)
                    {
                        if (android.format != importFormat)
                        {
                            android.format = importFormat;
                            android.overridden = true;
                            bDirtry = true;
                            import.SetPlatformTextureSettings(android);
                        }
                    }
                }

                TextureImporterPlatformSettings iphone = import.GetPlatformTextureSettings("iPhone");
                if (iphone != null)
                {
                    if (texture.width > maxSize && texture.height > maxSize && iphone.maxTextureSize != maxSize)
                    {
                        iphone.maxTextureSize = maxSize;
                        iphone.overridden = true;
                        bDirtry = true;
                        import.SetPlatformTextureSettings(iphone);
                    }
                    if (bFormatCheck && importFormat != TextureImporterFormat.Automatic)
                    {
                        if (iphone.format != importFormat)
                        {
                            iphone.format = importFormat;
                            iphone.overridden = true;
                            bDirtry = true;
                            import.SetPlatformTextureSettings(iphone);
                        }
                    }
                }

                if (bDirtry)
                {
                    import.SaveAndReimport();
                }
            }
            EditorUtility.ClearProgressBar();
        }
        //------------------------------------------------------
        static Compress LoadData()
        {
            Compress compress = null;
            string strFile = Application.dataPath + "/../EditorData/TextureCompress.json";
            try
            {
                if (File.Exists(strFile))
                {
                    compress = JsonUtility.FromJson<Compress>(File.ReadAllText(strFile));
                }
            }
            catch(System.Exception ex)
            {

            }
            return compress;
        }
        //------------------------------------------------------
        static void SaveData(Compress compress)
        {
            if(!Directory.Exists(Application.dataPath + "/../EditorData/"))
            {
                Directory.CreateDirectory(Application.dataPath + "/../EditorData/");
            }
            string strFile = Application.dataPath + "/../EditorData/TextureCompress.json";
            if(File.Exists(strFile))
            {
                File.Delete(strFile);
            }
            StreamWriter sw;
            sw = File.CreateText(strFile);
            sw.Write(JsonUtility.ToJson(compress, true));
            sw.Close();
        }
    }
}

