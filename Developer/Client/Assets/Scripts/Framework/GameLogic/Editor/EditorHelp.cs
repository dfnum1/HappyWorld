#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TopGame.Data;
using TopGame.Core;
using UnityEditor;
using System.Text;
using System.IO;
using Excel;
using System.Data;
using System.Reflection;
using Framework.Data;
using OfficeOpenXml;

namespace TopGame.ED
{
    public class EditorTimer
    {
        public float m_PreviousTime;
        public float deltaTime = 0.02f;
        public float fixedDeltaTime = 0.02f;
        public float m_fDeltaTime = 0f;
        public float m_currentSnap = 1f;

        //-----------------------------------------------------
        public void Update()
        {
            if (Application.isPlaying)
            {
                Application.targetFrameRate = 30;
                deltaTime = Time.deltaTime;
                m_fDeltaTime = (float)(deltaTime * m_currentSnap);
            }
            else
            {
                float curTime = Time.realtimeSinceStartup;
                m_PreviousTime = Mathf.Min(m_PreviousTime, curTime);//very important!!!

                deltaTime = curTime - m_PreviousTime;
                m_fDeltaTime = (float)(deltaTime * m_currentSnap);
            }

            m_PreviousTime = Time.realtimeSinceStartup;
        }
    }

    [Framework.Plugin.EditorCode]
    public class CsvData_MonsterTemplater : Data_Base
    {
        public class Templater : BaseData
        {
            public uint id;
            public string desc;

            public Dictionary<EMoveType, uint[]> groups = new Dictionary<EMoveType, uint[]>();
        }
        Dictionary<uint, Templater> m_vData = new Dictionary<uint, Templater>();
        //-------------------------------------------
        public Dictionary<uint, Templater> datas
        {
            get { return m_vData; }
        }
        //-------------------------------------------
        public CsvData_MonsterTemplater()
        {
        }
        //-------------------------------------------
        public Templater GetData(uint id)
        {
            Templater outData;
            if (m_vData.TryGetValue(id, out outData))
                return outData;
            return null;
        }
        //-------------------------------------------
        public bool LoadExcel(string strFile)
        {
            try
            {
                string strContext = EditorHelp.LoadExcelToCsv(strFile);
                if (string.IsNullOrEmpty(strContext)) return false;
                return LoadData(strContext);
            }
            catch (System.Exception ex)
            {
                EditorUtility.DisplayDialog("解析失败", ex.ToString(), "好的");
                return false;
            }
        }
        //-------------------------------------------
        public override bool LoadData(string strContext, CsvParser csv = null)
        {
            csv = new CsvParser();
            if (!csv.LoadTableString(strContext))
                return false;

            ClearData();

            int i = csv.GetTitleLine();
            if (i < 0) return false;

            int nLineCnt = csv.GetLineCount();
            for (i++; i < nLineCnt; i++)
            {
                if (!csv[i]["id"].IsValid()) continue;

                Templater data = new Templater();

                data.id = csv[i]["id"].Uint();
                data.desc = csv[i]["desc"].String();
                data.groups[EMoveType.Fixed] = csv[i]["fixedGroups"].UintArray();
                data.groups[EMoveType.Rush] = csv[i]["rushGroups"].UintArray();
                data.groups[EMoveType.Wander] = csv[i]["wanderGroups"].UintArray();
                data.groups[EMoveType.Standoff] = csv[i]["standoffGroups"].UintArray();

                m_vData.Add(data.id, data);
            }

            return true;
        }
        //-------------------------------------------
        public override void ClearData()
        {
            m_vData.Clear();
        }
    }

    public class EditorHelp
    {
        static bool m_bInited = false;
        static CsvData_MonsterTemplater m_MonsterTemplaterCsv = new CsvData_MonsterTemplater();
        static void Inited()
        {
            if (m_bInited) return;
            m_bInited = true;
            string monsterLayout = Application.dataPath + "/EditorDatas/DungonTemplate/MonsterTemplate.xlsx";
            if (File.Exists(monsterLayout))
            {
                m_MonsterTemplaterCsv.LoadExcel(monsterLayout);
            }
        }
        //------------------------------------------------------
        static string m_BinaryRootPath = "";
        public static string BinaryRootPath
        {
            get
            {
                if (string.IsNullOrEmpty(m_BinaryRootPath))
                    m_BinaryRootPath = Application.dataPath + "/../Binarys/";
                if (!System.IO.Directory.Exists(m_BinaryRootPath))
                    System.IO.Directory.CreateDirectory(m_BinaryRootPath);
                return m_BinaryRootPath;
            }
        }
        //------------------------------------------------------
        static string m_ServerBinaryRootPath = "";
        public static string ServerBinaryRootPath
        {
            get
            {
                if (string.IsNullOrEmpty(m_ServerBinaryRootPath))
                    m_ServerBinaryRootPath = Application.dataPath + "/../../Server/server_bin/datas/binarys/";
                if (!System.IO.Directory.Exists(m_ServerBinaryRootPath))
                    System.IO.Directory.CreateDirectory(m_ServerBinaryRootPath);
                return m_ServerBinaryRootPath;
            }
        }
        //------------------------------------------------------
        public static CsvData_MonsterTemplater.Templater GetMonsterTemplater(uint cur)
        {
            Inited();
            return m_MonsterTemplaterCsv.GetData(cur);
        }
        //------------------------------------------------------
        public static CsvData_MonsterTemplater MonsterTemplaterCsv
        {
            get
            {
                Inited();
                return m_MonsterTemplaterCsv;
            }
        }
        //------------------------------------------------------
        public static Vector3 GetTransfromMinPos(Transform trans)
        {
            Vector3 pos = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);

            pos = Vector3.Min(trans.position, pos);
            for (int i = 0; i < trans.childCount; ++i)
            {
                pos = Vector3.Min(pos, GetTransfromMinPos(trans.GetChild(i)));
            }
            return pos;
        }
        //------------------------------------------------------
        public static Vector3 GetTransfromMaxPos(Transform trans)
        {
            Vector3 pos = new Vector3(float.MinValue, float.MinValue, float.MinValue);
            pos = Vector3.Max(trans.position, pos);
            for (int i = 0; i < trans.childCount; ++i)
            {
                pos = Vector3.Max(pos, GetTransfromMaxPos(trans.GetChild(i)));
            }
            return pos;
        }
        //------------------------------------------------------
        public static bool ContainBounds(Bounds bounds, Bounds inside)
        {
            bounds.center = Vector3.zero;
            inside.center = Vector3.zero;
            if (!bounds.Contains(new Vector3(inside.min.x, inside.min.y, inside.min.z))) return false;
            if (!bounds.Contains(new Vector3(inside.min.x, inside.min.y, inside.max.z))) return false;
            if (!bounds.Contains(new Vector3(inside.max.x, inside.min.y, inside.max.z))) return false;
            if (!bounds.Contains(new Vector3(inside.max.x, inside.min.y, inside.min.z))) return false;

            if (!bounds.Contains(new Vector3(inside.min.x, inside.max.y, inside.min.z))) return false;
            if (!bounds.Contains(new Vector3(inside.min.x, inside.max.y, inside.max.z))) return false;
            if (!bounds.Contains(new Vector3(inside.max.x, inside.max.y, inside.max.z))) return false;
            if (!bounds.Contains(new Vector3(inside.max.x, inside.max.y, inside.min.z))) return false;
            return true;
        }
        //------------------------------------------------------
        public static bool BuildBounds(Transform transform, ref Bounds bounds)
        {
            Renderer[] renders = transform.GetComponentsInChildren<Renderer>();
            if (renders.Length > 0)
            {
                Vector3 min = Vector3.one * 10000;
                Vector3 max = -Vector3.one * 10000;
                for (int i = 0; i < renders.Length; ++i)
                {
                    if (!renders[i] || renders[i].bounds.size.magnitude >= 1000000) continue;
                    min = Vector3.Min(renders[i].bounds.min, min);
                    max = Vector3.Max(renders[i].bounds.max, max);
                }
                Renderer render = transform.gameObject.GetComponent<Renderer>();
                if (render)
                {
                    min = Vector3.Min(render.bounds.min, min);
                    max = Vector3.Max(max, render.bounds.max);
                }
                if ((max - min).sqrMagnitude <= 0 || (max - min).sqrMagnitude >= 1000000)
                {
                    min = GetTransfromMinPos(transform);
                    max = GetTransfromMaxPos(transform);
                    float sqrSize = (max - min).sqrMagnitude;
                    if (sqrSize <= 0 || sqrSize >= 1000000) return false;
                }
                min -= Vector3.one * 0.01f;
                max += Vector3.one * 0.01f;
                bounds.SetMinMax(min, max);
                return true;
            }
            else
            {
                Renderer render = transform.gameObject.GetComponent<Renderer>();
                if (render)
                {
                    Vector3 min = Vector3.one * 10000;
                    Vector3 max = -Vector3.one * 10000;
                    min = Vector3.Min(render.bounds.min, min);
                    max = Vector3.Max(max, render.bounds.max);
                    float sqrSize = (max - min).sqrMagnitude;
                    if (sqrSize <= 0 || sqrSize >= 1000000)
                    {
                        min = GetTransfromMinPos(transform);
                        max = GetTransfromMaxPos(transform);
                        sqrSize = (max - min).sqrMagnitude;
                        if (sqrSize <= 0 || sqrSize >= 1000000)
                            return false;
                    }
                    min -= Vector3.one * 0.01f;
                    max += Vector3.one * 0.01f;
                    bounds.SetMinMax(min, max);

                    return true;
                }
                else
                {
                    Vector3 min = Vector3.one * 10000;
                    Vector3 max = -Vector3.one * 10000;
                    min = GetTransfromMinPos(transform);
                    max = GetTransfromMaxPos(transform);
                    float sqrSize = (max - min).sqrMagnitude;
                    if (sqrSize <= 0 || sqrSize >= 1000000) return false;

                    min -= Vector3.one * 0.01f;
                    max += Vector3.one * 0.01f;
                    bounds.SetMinMax(min, max);
                }
            }
            return false;
        }
        //------------------------------------------------------
        public static string GetEnumFieldDisplayName(System.Enum curVar)
        {
            System.Type enumType = curVar.GetType();
            System.Reflection.FieldInfo fi = enumType.GetField(curVar.ToString());
            string strTemName = curVar.ToString();
            if (fi != null && fi.IsDefined(typeof(Framework.Plugin.PluginDisplayAttribute)))
            {
                strTemName = fi.GetCustomAttribute<Framework.Plugin.PluginDisplayAttribute>().displayName;
            }
            return strTemName;
        }
        //------------------------------------------------------
        public static Ray MouseToRay(Event evt, Camera camera = null)
        {
            return MouseToRay(evt.mousePosition, camera);
        }
        //------------------------------------------------------
        public static Ray MouseToRay(Vector3 screenMousePos, Camera camera = null)
        {
            if (camera == null && UnityEditor.SceneView.lastActiveSceneView) camera = UnityEditor.SceneView.lastActiveSceneView.camera;
            if (camera == null) return new Ray(Vector3.zero, Vector3.zero);
            float mult = 1;
#if UNITY_5_4_OR_NEWER
            mult = EditorGUIUtility.pixelsPerPoint;
#endif
            screenMousePos.y = camera.pixelHeight - screenMousePos.y * mult;
            screenMousePos.x *= mult;
            return camera.ScreenPointToRay(screenMousePos);
        }
        //------------------------------------------------------
        public static void SaveToPng(Texture2D texture, string saveTo)
        {
            byte[] bytes = texture.EncodeToPNG();
            string dir = System.IO.Path.GetDirectoryName(saveTo);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            FileStream file = File.Open(saveTo, FileMode.Create);
            BinaryWriter writer = new BinaryWriter(file);
            writer.Write(bytes);
            file.Close();
        }
        //------------------------------------------------------
        public static void SaveToPng(RenderTexture rt, string saveto)
        {
            RenderTexture backupRt=  RenderTexture.active;
            RenderTexture.active = rt;

            Texture2D png = new Texture2D(rt.width, rt.height, TextureFormat.ARGB32, true);
            png.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);

            byte[] bytes = png.EncodeToPNG();

            FileStream file = File.Open(saveto, FileMode.OpenOrCreate);
            BinaryWriter writer = new BinaryWriter(file);
            writer.Write(bytes);
            file.Close();
            writer.Close();
            RenderTexture.active = backupRt;
            GameObject.DestroyImmediate(png);
            png = null;
        }
        //------------------------------------------------------
        public static void RepaintPlayModeView()
        {
            var unityEditorAssembly = typeof(AudioImporter).Assembly;
            var audioUtilClass = unityEditorAssembly.GetType("UnityEditor.PlayModeView");
            var method = audioUtilClass.GetMethod("RepaintAll", BindingFlags.Static | BindingFlags.Public);
            if (method == null) method = audioUtilClass.GetMethod("RepaintAll", BindingFlags.Static | BindingFlags.NonPublic);
            if (method == null) return;
            method.Invoke(null,null);
        }
        //-----------------------------------------------------
        public static string GetDisplayName(System.Enum curVar, string strDefault = null)
        {
            if (string.IsNullOrEmpty(strDefault)) strDefault = curVar.ToString();
            FieldInfo fi = curVar.GetType().GetField(curVar.ToString());
            if (fi == null) return strDefault;
            if (fi.IsDefined(typeof(Framework.Plugin.PluginDisplayAttribute)))
                strDefault = fi.GetCustomAttribute<Framework.Plugin.PluginDisplayAttribute>().displayName;
            return strDefault;
        }
        //-----------------------------------------------------
        public static void OpenStartUpApplication(string strSceneFile)
        {
            if (EditorApplication.isPlaying)
            {
                //if(Event.current!=null && Event.current.control)如果按住ctrl +f5 那么不会执行这个函数,so
               // EditorApplication.isPlaying = false;
            }
            else
            {
                Framework.Module.ModuleManager.getInstance().ShutDown();
                //保存当前场景
                var scene = UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene();
                if (scene != null && scene.isDirty && !string.IsNullOrWhiteSpace(scene.name) && !string.IsNullOrWhiteSpace(scene.path))
                {
                    if (UnityEditor.EditorUtility.DisplayDialog("提示", "当前场景未保存,是否保存?", "保存", "不保存"))
                    {
                        UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);
                    }
                }
                UnityEditor.SceneManagement.EditorSceneManager.OpenScene(strSceneFile);
                EditorApplication.isPlaying = true;
            }
        }
        //------------------------------------------------------
        public static void SaveExcel(string strFile, List<string> title, List<List<string>> rows)
        {
            try
            {
                FileInfo newFile = new FileInfo(strFile);

                if (newFile.Exists)
                {
                    newFile.Delete();

                    newFile = new FileInfo(strFile);
                }
                using (ExcelPackage package = new ExcelPackage(newFile))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("attr");
                    for (int i = 0; i < title.Count; ++i)
                        worksheet.Cells[1, i+1].Value = title[i];

                    for (int i = 0; i < rows.Count; i++)
                    {
                        for(int j =0; j < rows[i].Count; ++j)
                        {
                            worksheet.Cells[i+2,j+1].Value = rows[i][j];
                        }
                    }
                    package.Save();
                }
            }
            catch (System.Exception ex)
            {
                EditorUtility.DisplayDialog("保存失败", ex.ToString(), "好的");
            }
        }
        //------------------------------------------------------
        public static string LoadExcelToCsv(string strFile)
        {
            try
            {
                FileStream mStream = File.Open(strFile, FileMode.Open, FileAccess.Read, FileShare.Read);
                IExcelDataReader mExcelReader = ExcelReaderFactory.CreateOpenXmlReader(mStream);
                System.Data.DataSet mResultSet = mExcelReader.AsDataSet();
                if (mResultSet == null)
                {
                    EditorUtility.DisplayDialog("解析失败", "不是一个有效的excel 文件", "好的");
                    return "";
                }
                if (mResultSet.Tables.Count < 1)
                {
                    EditorUtility.DisplayDialog("解析失败", "没有数据", "好的");
                    return "";
                }
                DataTable mSheet = mResultSet.Tables[0];
                if (mSheet.Rows.Count < 1)
                    return "";
                int rowStart = 0;

                int rowCount = mSheet.Rows.Count;
                int colCount = mSheet.Columns.Count;
                string[] dataTypes = new string[colCount];
                for (int i = 0; i < colCount; ++i)
                {
                    string dataType = mSheet.Rows[1][i].ToString();
                    if (string.IsNullOrEmpty(dataType))
                    {
                        dataTypes[i] = "";
                        continue;
                    }

                    dataTypes[i] = dataType;
                }
                string[] defaults = new string[colCount];
                string[] externLines = new string[colCount];
                for (int i = 0; i < colCount; ++i)
                {
                    string dataType = mSheet.Rows[2][i].ToString();
                    if (string.IsNullOrEmpty(dataType))
                    {
                        externLines[i] = "";
                        defaults[i] = "";
                        continue;
                    }

                    externLines[i] = dataType;

                    int defaultLeft = dataType.IndexOf('#');
                    int defaultRight = dataType.LastIndexOf('#');
                    if (defaultLeft < defaultRight && defaultRight > 0)
                    {
                        externLines[i] = dataType.Substring(0, defaultLeft);
                        defaults[i] = dataType.Substring(defaultLeft + 1, defaultRight - defaultLeft - 1);
                    }
                    else
                        defaults[i] = "";
                }
                StringBuilder stringBuilder = new StringBuilder();
                //读取数据
                for (int i = rowStart; i < rowCount; i++)
                {
                    for (int j = 0; j < colCount; j++)
                    {
                        if (string.IsNullOrEmpty(dataTypes[j])) continue;
                        string strLabel = mSheet.Rows[i][j].ToString();
                        //使用","分割每一个数值
                        if (i >= 4)
                        {
                            if (!string.IsNullOrEmpty(defaults[j]) && string.IsNullOrEmpty(strLabel))
                                strLabel = defaults[j];
                            //data zooms
                            if (dataTypes[j].ToLower().Contains("bit|"))
                            {
                                int value = 0;
                                string[] vals = strLabel.Split('|');
                                for (int b = 0; b < vals.Length; ++b)
                                {
                                    string temp = vals[b].Trim();
                                    int tempVal = 0;
                                    if (temp.Length > 0 && int.TryParse(temp, out tempVal))
                                    {
                                        value |= 1 << tempVal;
                                    }
                                }
                                strLabel = value.ToString();
                            }
                        }
                        else if (i == 1) //data type
                        {
                            strLabel = dataTypes[j];
                            if (strLabel.Trim().Contains("bit|"))
                                strLabel = strLabel.Replace("bit|", "");
                        }
                        else if (i == 2) //extern desc
                        {
                            strLabel = externLines[j];
                        }
                        if (i >= 4 && dataTypes[j].ToLower().CompareTo("string") == 0)
                            stringBuilder.Append("\"" + strLabel + "\",");
                        else
                            stringBuilder.Append(strLabel + ",");
                    }
                    //使用换行符分割每一行
                    stringBuilder.Append("\r\n");
                }
                return stringBuilder.ToString();
            }
            catch (System.Exception ex)
            {
                EditorUtility.DisplayDialog("解析失败", ex.ToString(), "好的");
                return "";
            }
        }
        //-----------------------------------------------------
        [System.Serializable]
        class PublishABSetting
        {
            public List<string> buildDirs = new List<string>();
            public List<string> unbuildDirs = new List<string>();
        }
        static PublishABSetting ms_pABSetting = null;
        public static bool CanAssetBundlePath(string strPath, bool notifyPop = true, UnityEngine.Transform objTran = null, bool bCsvConfig = false)
        {
            if (string.IsNullOrEmpty(strPath))
                return false;
            if(ms_pABSetting == null)
            {
                ms_pABSetting = new PublishABSetting();
                string strPublishSetting = Application.dataPath + "/../Publishs/Setting.json";
                if (System.IO.File.Exists(strPublishSetting))
                {
                    ms_pABSetting = JsonUtility.FromJson<PublishABSetting>(System.IO.File.ReadAllText(strPublishSetting));
                }
            }
            bool bCanABLoad = true;
            if (strPath.Contains("Assets/Datas/"))
            {
                List<string> unbuildDirs = ms_pABSetting.unbuildDirs;
                for (int i = 0; i < unbuildDirs.Count; ++i)
                {
                    if (strPath.Contains(unbuildDirs[i]))
                    {
                        bCanABLoad = false;
                        break;
                    }
                }
            }
            else
            {
                if (ms_pABSetting.buildDirs != null && ms_pABSetting.buildDirs.Count > 0)
                {
                    bCanABLoad = false;
                    for (int i = 0; i < ms_pABSetting.buildDirs.Count; i++)
                    {
                        if (strPath.Contains(ms_pABSetting.buildDirs[i]))
                        {
                            bCanABLoad = true;
                            break;
                        }
                    }
                }
            }
            if(!bCanABLoad && notifyPop)
            {
                string strFullPath = "";
                while(objTran!=null)
                {
                    if(string.IsNullOrEmpty(strFullPath))
                        strFullPath = objTran.name;
                    else
                        strFullPath = objTran.name + "." + strFullPath;
                    objTran = objTran.parent;
                }
                Debug.LogError(strPath + " 不支持动态加载\r\n" + strFullPath);
                EditorUtility.DisplayDialog(bCsvConfig?"表格配置错误":"错误提示", strPath + " 不支持动态加载\r\n" + strFullPath, "请检查");
            }

            return bCanABLoad;
        }
    }
}
#endif