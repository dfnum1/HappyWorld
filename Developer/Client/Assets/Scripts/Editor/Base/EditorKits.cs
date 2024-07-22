/********************************************************************
生成日期:	25:7:2019   14:35
类    名: 	EditorKits
作    者:	HappLI
描    述:	编辑器工具集
*********************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using TopGame.Core;
using Framework.Core;
using Framework.ED;
using static TopGame.Data.TextureCliperAssets;

namespace TopGame.ED
{
    //ReorderableList
    //------------------------------------------------------
    //! EditorKits
    //------------------------------------------------------
    public class EditorKits
    {
        
        //------------------------------------------------------
        public static T Clone<T>(T pointer, T cloner )
        {
            FieldInfo[] fields = typeof(T).GetFields(BindingFlags.Instance | BindingFlags.Public);
            for(int i = 0; i < fields.Length; ++i)
            {
                fields[i].SetValue(pointer, fields[i].GetValue(cloner));
            }
            return pointer;
        }
        //------------------------------------------------------
        public static void PlayClip(AudioClip clip, int startSample = 0, bool loop = false)
        {
            EditorUtil.PlayClip(clip, startSample, loop);
        }
        //------------------------------------------------------
        public static void PlayClip(string audioClipFile, int startSample = 0, bool loop = false)
        {
            EditorUtil.PlayClip(audioClipFile, startSample, loop);
        }
        //------------------------------------------------------
        public static void StopAllAudioClips()
        {
            EditorUtil.StopAllAudioClips();
        }
        //-------------------------------------------
        public static bool GetBounds(GameObject pGo, out Bounds bounds)
        {
            bounds = new Bounds();
            Renderer pfiler = pGo.GetComponent<Renderer>();
            if(pfiler == null)
            {
                pfiler = pGo.GetComponent<Renderer>();
            }
            if (pfiler == null) return false;
            bounds = pfiler.bounds;
            return true;
        }
        //-------------------------------------------
        public static bool bRuntimeGame = false;
        public static void NewScene()
        {
            if (bRuntimeGame) return;
            if (!EditorApplication.isPlaying)
            {
                try
                {
                    //   EditorApplication.ExecuteMenuItem("File/New Scene");
                    UnityEngine.SceneManagement.Scene scen = UnityEditor.SceneManagement.EditorSceneManager.NewScene(UnityEditor.SceneManagement.NewSceneSetup.DefaultGameObjects);
                    RenderSettings.ambientSkyColor = Color.white;
//                     Camera[] cameras = GameObject.FindObjectsOfType<Camera>();
//                     if(cameras == null || cameras.Length<=0)
//                     {
//                         GameObject cameraGo = new GameObject("EditorCaemra");
//                         Camera camera = cameraGo.AddComponent<Camera>();
//                         camera.tag = "MainCamera";
//                         cameraGo.hideFlags |= HideFlags.DontSave;
//                     }
                }
                catch (System.Exception ex)
                {
                	
                }
            }
        }
        //------------------------------------------------------
        public static List<T> FindComponents<T>(GameObject pObj, List<T> comps) where T : MonoBehaviour
        {
            T com = pObj.GetComponent<T>();
            if (com)
            {
                comps.Add(com);
                return comps;
            }
            for (int i = 0; i < pObj.transform.childCount; ++i)
                FindComponents(pObj.transform.GetChild(i).gameObject, comps);
            return comps;
        }
        //------------------------------------------------------
        public static void CheckEventCopy()
        {
            DrawEventCore.CheckCopyByClipBoard();
            {
                BaseEventParameter pEvt = Core.CameraSplineEventEditor.BulidCopyCatchSplineEvent();
                if (pEvt != null)
                    DrawEventCore.AddCopyEvent(pEvt);
            }
            {
                BaseEventParameter pEvt = Core.CameraSettingEditor.BuildCatchCameraOffsetEvent();
                if (pEvt != null)
                    DrawEventCore.AddCopyEvent(pEvt);
            }
            {
                BaseEventParameter pEvt = Core.CameraSettingEditor.BuildCatchCameraEventParam();
                if (pEvt != null)
                    DrawEventCore.AddCopyEvent(pEvt);
            }
        }
        //------------------------------------------------------
        private static List<FileInfo> FindDirFiles(string strDir)
        {
            List<FileInfo> vRets = new List<FileInfo>();
            if (!Directory.Exists(strDir))
                return vRets;

            FindDirFiles(strDir, vRets);

            return vRets;

        }
        //------------------------------------------------------
        public static void FindDirFiles(string strDir, List<FileInfo> vRes)
        {
            if (!Directory.Exists(strDir)) return;

            string[] dir = Directory.GetDirectories(strDir);
            DirectoryInfo fdir = new DirectoryInfo(strDir);
            FileInfo[] file = fdir.GetFiles();
            if (file.Length != 0 || dir.Length != 0)
            {
                foreach (FileInfo f in file)
                {
                    vRes.Add(f);
                }
                foreach (string d in dir)
                {
                    FindDirFiles(d, vRes);
                }
            }
        }
        //------------------------------------------------------
        public static void CopyDir(string srcDir, string destDir, HashSet<string> vFilerExtension = null, HashSet<string> vIgoreExtension = null)
        {
            if (srcDir.Length <= 0 || destDir.Length < 0) return;

            try
            {
                //如果目标路径不存在,则创建目标路径
                if (!System.IO.Directory.Exists(destDir))
                {
                    System.IO.Directory.CreateDirectory(destDir);
                }
                string tile = "Copy:" + srcDir + "->" + destDir;
                EditorUtility.DisplayProgressBar(tile, "...", 0);
                //得到原文件根目录下的所有文件
                string[] files = System.IO.Directory.GetFiles(srcDir);
                for (int i =0; i < files.Length; ++i)
                {
                    string extension = Path.GetExtension(files[i]);
                    if (vIgoreExtension != null && vIgoreExtension.Contains(extension)) continue;
                    if (vFilerExtension != null && !vFilerExtension.Contains(extension)) continue;

                    string name = System.IO.Path.GetFileName(files[i]);
                    string dest = System.IO.Path.Combine(destDir, name);
                    System.IO.File.Copy(files[i], dest);//复制文件
                    EditorUtility.DisplayProgressBar(tile, files[i], (float)((float)i / (float)files.Length));
                }
                //得到原文件根目录下的所有文件夹
                string[] folders = System.IO.Directory.GetDirectories(srcDir);
                foreach (string folder in folders)
                {
                    string name = System.IO.Path.GetFileName(folder);
                    string dest = System.IO.Path.Combine(destDir, name);
                    CopyDir(folder, dest, vFilerExtension, vIgoreExtension);//构建目标路径,递归复制文件
                }
                EditorUtility.ClearProgressBar();
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
        //------------------------------------------------------
        public static void CopyFile(string srcFile, string destFile)
        {
            if (srcFile.Length <= 0 || destFile.Length < 0) return;

            srcFile = srcFile.Replace("\\", "/");
            if (!File.Exists(srcFile)) return;

            destFile = destFile.Replace("\\", "/");
            if (!Directory.Exists(Path.GetDirectoryName(destFile)))
                Directory.CreateDirectory(Path.GetDirectoryName(destFile));
            File.Copy(srcFile, destFile, true);
        }
        //------------------------------------------------------
        public static void DeleteFile(string path)
        {
            if (File.Exists(path))
            {
                File.SetAttributes(path, FileAttributes.Normal);
                File.Delete(path);
            }
        }
        //------------------------------------------------------
        public static void DeleteDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                return;
            }
            string[] files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
            string[] dirs = Directory.GetDirectories(path);
            foreach (string file in files)
            {
                DeleteFile(file);
            }
            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }
            Directory.Delete(path);
        }
        //------------------------------------------------------
        public static void ClearDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                return;
            }
            string[] files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
            string[] dirs = Directory.GetDirectories(path);
            foreach (string file in files)
            {
                DeleteFile(file);
            }
        }
        //------------------------------------------------------
        public static void OpenPathInExplorer(string path)
        {
            if (path.Length <= 0f) return;
            System.Diagnostics.Process[] prpgress = System.Diagnostics.Process.GetProcesses();

            string args = "";
            if (!path.Contains(":/") && !path.Contains(":\\"))
            {
                if ((path[0] == '/') || (path[0] == '\\'))
                    path = Application.dataPath.Substring(0, Application.dataPath.Length - "/Assets".Length) + path;
                else
                    path = Application.dataPath.Substring(0, Application.dataPath.Length - "Assets".Length) + path;
            }

            args = path.Replace(":/", ":\\");
            args = args.Replace("/", "\\");
            if (path.Contains("."))
            {
                args = string.Format("/Select, \"{0}\"", args);
            }
            else
            {
                if (args[args.Length - 1] != '\\')
                {
                    args += "\\";
                }
            }
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            System.Diagnostics.Process.Start("Explorer.exe", args);
#elif UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
            Debug.Log("IOS 打包路径: " + path);
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("open", path));
#endif
        }
        //------------------------------------------------------
        public static string DrawUIObjectByPath<T>(string label, string strFile, bool bClear = true, Action onDel = null) where T : UnityEngine.Object
        {
            Color color = GUI.color;
            T asset = AssetDatabase.LoadAssetAtPath<T>(strFile);
            if (asset == null)
            {
                GUI.color = Color.red;
            }
            EditorGUILayout.BeginHorizontal();
            asset = EditorGUILayout.ObjectField(label, asset, typeof(T), false) as T;
            if (asset != null)
                strFile = AssetDatabase.GetAssetPath(asset);
            if (bClear && GUILayout.Button("清除", new GUILayoutOption[] { GUILayout.Width(50) }))
            {
                strFile = "";
            }
            if (onDel != null)
            {
                if (GUILayout.Button("清除", new GUILayoutOption[] { GUILayout.Width(50) }))
                {
                    onDel();
                }
            }
            EditorGUILayout.EndHorizontal();
            GUI.color = color;
            return strFile;
        }
        //------------------------------------------------------
        public static string DrawUIObjectByPathNoLayout<T>(string label, string strFile) where T : UnityEngine.Object
        {
            if (strFile == null) strFile = "";
            Color color = GUI.color;
            T asset = AssetDatabase.LoadAssetAtPath<T>(strFile);
            if (asset == null)
            {
                GUI.color = Color.red;
            }
            asset = EditorGUILayout.ObjectField(label, asset, typeof(T), false) as T;
            if (asset != null)
                strFile = AssetDatabase.GetAssetPath(asset);
            GUI.color = color;
            return strFile;
        }
        //------------------------------------------------------
        public static void PopMessageBox(string title, string context, string ok)
        {
            EditorUtility.DisplayDialog(title, context, ok);
        }
        //------------------------------------------------------
        public static bool PopMessageBox(string title, string context, string ok = "确定", string cancel = "取消")
        {
            return EditorUtility.DisplayDialog(title, context, ok, cancel);
        }
        //------------------------------------------------------
        public static string GetVolumeColor(EVolumeType type)
        {
            return Framework.Core.CommonUtility.GetVolumeColor(type);
        }
        //------------------------------------------------------
        public static Color GetVolumeToColor(EVolumeType type)
        {
            return Framework.Core.CommonUtility.GetVolumeToColor(type);
        }
        //------------------------------------------------------
        public static Vector3 RayHitPos(Vector3 pos, Vector3 dir, float floorY = 0)
        {
            Vector3 vPlanePos = Vector3.zero;
            vPlanePos.y = floorY;

            Vector3 vPlaneNor = Vector3.up;

            float fdot = Vector3.Dot(dir, vPlaneNor);
            if (fdot == 0.0f)
                return Vector3.zero;

            float fRage = ((vPlanePos.x - pos.x) * vPlaneNor.x + (vPlanePos.y - pos.y) * vPlaneNor.y + (vPlanePos.z - pos.z) * vPlaneNor.z) / fdot;

            return pos + dir * fRage;
        }
        //------------------------------------------------------
        public static void DrawWireCube(Vector3 pos, Vector3 size, Color color)
        {
            Color bakcolor = Handles.color;
            Handles.color = color;
            Handles.DrawWireCube(pos, size);
            Handles.color = bakcolor;
        }
        //------------------------------------------------------
        public static void DrawWireSphere(Vector3 pos, float radius, Color color, int controll = 0)
        {
            Color bakcolor = Handles.color;
            Handles.color = color;
            Handles.CircleHandleCap(controll, pos, Quaternion.Euler(0, 0, 0), radius, EventType.Repaint);
            Handles.CircleHandleCap(controll, pos, Quaternion.Euler(90, 0, 0), radius, EventType.Repaint);
            Handles.CircleHandleCap(controll, pos, Quaternion.Euler(0, 180, 0), radius, EventType.Repaint);
            Handles.CircleHandleCap(controll, pos, Quaternion.Euler(0, 90, 0), radius, EventType.Repaint);

            Handles.color = bakcolor;
        }
        //-----------------------------------------------------
        static public AnimatorState[] GetStates(AnimatorStateMachine sm)
        {
            AnimatorState[] stateArray = new AnimatorState[sm.states.Length];
            for (int i = 0; i < sm.states.Length; i++)
            {
                stateArray[i] = sm.states[i].state;
            }
            return stateArray;
        }
        //-----------------------------------------------------
        static public List<AnimatorState> GetStatesRecursive(AnimatorStateMachine sm, bool bChild = true)
        {
            List<AnimatorState> list = new List<AnimatorState>();
            list.AddRange(GetStates(sm));
            if (bChild)
            {
                for (int i = 0; i < sm.stateMachines.Length; i++)
                {
                    list.AddRange(GetStatesRecursive(sm.stateMachines[i].stateMachine));
                }
            }

            return list;
        }
        //-----------------------------------------------------
        static public AnimatorState FindStatesRecursive(AnimatorController controller, string name, bool bChild = true)
        {
            for(int i =0; i < controller.layers.Length; ++i)
            {
                var stateMachine = controller.layers[i].stateMachine;
                for (int j =0; j < stateMachine.stateMachines.Length; ++j)
                {
                    var states= stateMachine.stateMachines[j].stateMachine.states;
                    for(int k = 0; k < states.Length; ++i)
                    {
                        if(states[k].state)
                        {
                            if (states[k].state.name.CompareTo(name) == 0)
                                return states[k].state;
                        }
                    }
                }
            }
            return null;
        }
        //-----------------------------------------------------
        static public bool FindTransforms(Transform pParent, ref List<Transform> vTrans, ref List<string> vNamePops, ref List<string> vFullNamePops, string strParentName = "")
        {
            if (string.IsNullOrEmpty(strParentName))
                strParentName = pParent.name;

            for (int i = 0; i < pParent.childCount; ++i)
            {
                vTrans.Add(pParent.GetChild(i));
                vFullNamePops.Add(strParentName + "/" + pParent.GetChild(i).name);
                vNamePops.Add(pParent.GetChild(i).name);
                FindTransforms(pParent.GetChild(i), ref vTrans, ref vNamePops, ref vFullNamePops, strParentName + "/" + pParent.GetChild(i).name);
            }
            return vTrans.Count>0;
        }
        //-----------------------------------------------------
        static public Transform FindGameObject(Transform root, string name, bool bOnlyChild = false)
        {
            Transform find = null;
            if (root)
            {
                if (!bOnlyChild && name.CompareTo(root.name) == 0)
                {
                    return root;
                }
                for (int i = 0; i < root.childCount; ++i)
                {
                    find = FindGameObject(root.GetChild(i).transform, name);
                    if (find) return find;
                }
            }

            return find;
        }
        //-------------------------------------------------
        public static Transform FindObjectByFullName(Transform pParent, string name)
        {
            string[] paths = name.Split('.');
            if (paths.Length <= 0) return null;

            if (pParent.name.CompareTo(pParent.name) != 0) return null;

            Transform ret = pParent;
            Transform findParent = pParent;
            for (int i = 1; i < paths.Length; ++i)
            {
                Transform temp = null;
                for (int j=0; j < findParent.childCount; ++j)
                {
                    if (findParent.GetChild(j).name.CompareTo(paths[i]) == 0)
                    {
                        temp = findParent.GetChild(j);
                        break;
                    }
                }
                if (temp == null) return null;
                findParent = temp;
                ret = temp;
            }
            return ret;
        }
        //------------------------------------------------------
        public static void CopyToClipboard(string strText)
        {
            TextEditor t = new TextEditor();
            t.text = strText;
            t.OnFocus();
            t.Copy();
        }
        //------------------------------------------------------
        public static void SceneViewLookat(Transform transform)
        {
            Selection.activeObject = transform.gameObject;
            if(SceneView.lastActiveSceneView != null)
                SceneView.lastActiveSceneView.FrameSelected();
        }
        //------------------------------------------------------
        public static void SceneViewLookat(Vector3 transform)
        {
            if (SceneView.lastActiveSceneView != null)
            {
                SceneView.lastActiveSceneView.LookAt(transform);
            }
        }
        //------------------------------------------------------
        public class DropMenuItem
        {
            public string strDisplay;
            public int nValueID;
            public object pValue;
        }
        static List<string> ms_vPop = new List<string>();
        public static object DrawDropMenum(string strLabel, object curData, List<DropMenuItem> vData, float nWidth = 0)
        {
            ms_vPop.Clear();
            int index = -1;
            for(int i = 0; i < vData.Count; ++i)
            {
                ms_vPop.Add(vData[i].strDisplay);
                if (vData[i].pValue == curData)
                {
                    index = i;
                }
            }
            if (nWidth > 0)
                GUILayout.BeginHorizontal(new GUILayoutOption[] { GUILayout.Width(nWidth) });
            if(strLabel.Length>0)
                index = EditorGUILayout.Popup(strLabel, index, ms_vPop.ToArray());
            else
                index = EditorGUILayout.Popup(index, ms_vPop.ToArray());
            if (nWidth > 0)
                GUILayout.EndHorizontal();
                if (index >= 0 && index < vData.Count)
                return vData[index].pValue;
            return null;
        }
        //------------------------------------------------------
        public static int DrawDropMenumByValueID(string strLabel, int valueID, List<DropMenuItem> vData, int defaultID = 0, float nWidth = 0)
        {
            ms_vPop.Clear();
            int index = -1;
            for (int i = 0; i < vData.Count; ++i)
            {
                ms_vPop.Add(vData[i].strDisplay);
                if (vData[i].nValueID == valueID)
                {
                    index = i;
                }
            }
            if (nWidth > 0)
                GUILayout.BeginHorizontal(new GUILayoutOption[] { GUILayout.Width(nWidth) });
            if (strLabel.Length > 0)
                index = EditorGUILayout.Popup(strLabel, index, ms_vPop.ToArray());
            else
                index = EditorGUILayout.Popup(index, ms_vPop.ToArray());

            if (nWidth > 0)
                GUILayout.EndHorizontal();
            if (index >= 0 && index < vData.Count)
                return vData[index].nValueID;
            return defaultID;
        }
        //------------------------------------------------------
        public static int DrawDropMenum(string strLabel, int curIndex, List<DropMenuItem> vData, float nWidth = 0)
        {
            ms_vPop.Clear();
            for (int i = 0; i < vData.Count; ++i)
                ms_vPop.Add(vData[i].strDisplay);
            if (nWidth > 0)
                GUILayout.BeginHorizontal(new GUILayoutOption[] { GUILayout.Width(nWidth) });
            if (strLabel.Length > 0)
                curIndex = EditorGUILayout.Popup(strLabel, curIndex, ms_vPop.ToArray());
            else
                curIndex = EditorGUILayout.Popup(curIndex, ms_vPop.ToArray());

            if (nWidth > 0)
                GUILayout.EndHorizontal();
            return curIndex;
        }
        //------------------------------------------------------
        public static bool DrawProperty(System.Object pointer, ref FieldInfo memInfo, bool bSet = true, int width = -1)
        {
            if(width > 0)
                GUILayout.BeginHorizontal(new GUILayoutOption[] { GUILayout.MinWidth(width) });
            else
                GUILayout.BeginHorizontal();

            bool bDrawed = false;
            if (memInfo.FieldType == typeof(byte))
            {
                bDrawed = true;
                if (bSet)
                    memInfo.SetValue(pointer, (byte)EditorGUILayout.IntField(memInfo.Name, (byte)memInfo.GetValue(pointer)));
                else
                    EditorGUILayout.IntField(memInfo.Name, (byte)memInfo.GetValue(pointer));
            }
            else if (memInfo.FieldType == typeof(short))
            {
                bDrawed = true;
                if (bSet)
                    memInfo.SetValue(pointer, (short)EditorGUILayout.IntField(memInfo.Name, (short)memInfo.GetValue(pointer)));
                else
                    EditorGUILayout.IntField(memInfo.Name, (short)memInfo.GetValue(pointer));
            }
            else if (memInfo.FieldType == typeof(int))
            {
                bDrawed = true;
                if (bSet)
                    memInfo.SetValue(pointer, (int)EditorGUILayout.IntField(memInfo.Name, (int)memInfo.GetValue(pointer)));
                else
                    EditorGUILayout.IntField(memInfo.Name, (int)memInfo.GetValue(pointer));
            }
            else if (memInfo.FieldType == typeof(uint))
            {
                bDrawed = true;
                if (bSet)
                    memInfo.SetValue(pointer, (uint)EditorGUILayout.IntField(memInfo.Name, (int)(uint)memInfo.GetValue(pointer)));
                else
                    EditorGUILayout.IntField(memInfo.Name, (int)(uint)memInfo.GetValue(pointer));
            }
            else if (memInfo.FieldType == typeof(float))
            {
                bDrawed = true;
                if (bSet)
                    memInfo.SetValue(pointer, EditorGUILayout.FloatField(memInfo.Name, (float)memInfo.GetValue(pointer)));
                else
                    EditorGUILayout.FloatField(memInfo.Name, (float)memInfo.GetValue(pointer));
            }
            else if (memInfo.FieldType == typeof(bool))
            {
                bDrawed = true;
                if (bSet)
                    memInfo.SetValue(pointer, EditorGUILayout.Toggle(memInfo.Name, (bool)memInfo.GetValue(pointer)));
                else
                    EditorGUILayout.Toggle(memInfo.Name, (bool)memInfo.GetValue(pointer));
            }
            else if (memInfo.FieldType == typeof(string))
            {
                bDrawed = true;
                if (bSet)
                    memInfo.SetValue(pointer, EditorGUILayout.TextField(memInfo.Name, memInfo.GetValue(pointer).ToString()));
                else
                    EditorGUILayout.TextField(memInfo.Name, memInfo.GetValue(pointer).ToString());
            }
            else if (memInfo.FieldType == typeof(Vector2))
            {
                bDrawed = true;
                if (bSet)
                    memInfo.SetValue(pointer, EditorGUILayout.Vector2Field(memInfo.Name, (Vector2)memInfo.GetValue(pointer)));
                else
                    EditorGUILayout.Vector2Field(memInfo.Name, (Vector2)memInfo.GetValue(pointer));
            }
            else if (memInfo.FieldType == typeof(Vector3))
            {
                bDrawed = true;
                if (bSet)
                    memInfo.SetValue(pointer, EditorGUILayout.Vector3Field(memInfo.Name, (Vector3)memInfo.GetValue(pointer)));
                else
                    EditorGUILayout.Vector3Field(memInfo.Name, (Vector3)memInfo.GetValue(pointer));
            }
            else if (memInfo.FieldType == typeof(Vector2Int))
            {
                bDrawed = true;
                if (bSet)
                    memInfo.SetValue(pointer, EditorGUILayout.Vector2IntField(memInfo.Name, (Vector2Int)memInfo.GetValue(pointer)));
                else
                    EditorGUILayout.Vector2IntField(memInfo.Name, (Vector2Int)memInfo.GetValue(pointer));
            }
            else if (memInfo.FieldType == typeof(Vector3Int))
            {
                bDrawed = true;
                if (bSet)
                    memInfo.SetValue(pointer, EditorGUILayout.Vector3IntField(memInfo.Name, (Vector3Int)memInfo.GetValue(pointer)));
                else
                    EditorGUILayout.Vector3IntField(memInfo.Name, (Vector3Int)memInfo.GetValue(pointer));
            }
            else if (memInfo.FieldType == typeof(Vector4))
            {
                bDrawed = true;
                if (bSet)
                    memInfo.SetValue(pointer, EditorGUILayout.Vector4Field(memInfo.Name, (Vector4)memInfo.GetValue(pointer)));
                else
                    EditorGUILayout.Vector4Field(memInfo.Name, (Vector4)memInfo.GetValue(pointer));
            }
            else if (memInfo.FieldType.IsEnum)
            {
                bDrawed = true;
                System.Object obj = memInfo.GetValue(pointer);
                Enum val = EditorGUILayout.EnumPopup(memInfo.Name, (Enum)Enum.Parse(obj.GetType(), obj.ToString()));
                if (bSet)
                    memInfo.SetValue(pointer, val);
            }

            GUILayout.EndHorizontal();
            return bDrawed;
        }
        //-------------------------------------------------
        static System.Type ms_Type;
        static MethodInfo ms_MetInfo;
        public static long GetStorgeMemory(UnityEngine.Object asset)
        {
            if (asset != null)
            {
                if (asset.GetType() == typeof(UnityEngine.Texture) || asset.GetType().BaseType == typeof(UnityEngine.Texture))
                {
                    ms_Type = System.Reflection.Assembly.Load("UnityEditor.dll").GetType("UnityEditor.TextureUtil");
                    ms_MetInfo = ms_Type.GetMethod("GetStorageMemorySize", BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public);
                    return (long)(int)ms_MetInfo.Invoke(null, new object[] { asset });
                    //    return 0;
                }
                else
                {
                    string filePath = AssetDatabase.GetAssetPath(asset);
                    if (filePath.Length > 0)
                    {
                        FileInfo fileInfo = new FileInfo(filePath);
                        return fileInfo.Length;
                    }
                }

            }
            return 0;
        }
        //------------------------------------------------------
        public static bool IsTextureMipMap(UnityEngine.Object asset)
        {
            if (asset != null)
            {
                if (asset.GetType() == typeof(UnityEngine.Texture) || asset.GetType().BaseType == typeof(UnityEngine.Texture))
                {
                    ms_Type = System.Reflection.Assembly.Load("UnityEditor.dll").GetType("UnityEditor.TextureUtil");
                    ms_MetInfo = ms_Type.GetMethod("HasMipMap", BindingFlags.Static | BindingFlags.Public);
                    return (bool)ms_MetInfo.Invoke(null, new object[] { asset });
                    //    return 0;
                }

            }
            return false;
        }
        //------------------------------------------------------
        public static bool IsReadWriteAble(UnityEngine.Object asset)
        {
            if (asset == null) return false;
            if (asset is MonoScript)
                return false;

            string path = AssetDatabase.GetAssetPath(asset);
            if (asset.GetType() == typeof(UnityEngine.Texture) || asset.GetType().BaseType == typeof(UnityEngine.Texture))
            {
                TextureImporter texture = AssetImporter.GetAtPath(path) as TextureImporter;
                return texture.isReadable;
            }
            else if (path.ToLower().Contains(".fbx"))
            {
                ModelImporter model = AssetImporter.GetAtPath(path) as ModelImporter;
                if (model == null) return false;
                return model.isReadable;
            }
            return false;
        }
        //------------------------------------------------------
        public static bool IsOptimizeMeshVertices(UnityEngine.Object asset)
        {
            if (asset == null) return false;

            string path = AssetDatabase.GetAssetPath(asset);
            if (path.ToLower().Contains(".fbx"))
            {
                ModelImporter model = AssetImporter.GetAtPath(path) as ModelImporter;
                return model.optimizeMeshVertices;
            }
            return false;
        }
        //------------------------------------------------------
        public static bool IsOptimizeMeshPolygon(UnityEngine.Object asset)
        {
            if (asset == null) return false;

            string path = AssetDatabase.GetAssetPath(asset);
            if (path.ToLower().Contains(".fbx"))
            {
                ModelImporter model = AssetImporter.GetAtPath(path) as ModelImporter;
                return model.optimizeMeshPolygons;
            }
            return false;
        }
        //------------------------------------------------------
        public static ModelImporterMeshCompression CompressMesh(UnityEngine.Object asset)
        {
            if (asset == null) return ModelImporterMeshCompression.Off;

            string path = AssetDatabase.GetAssetPath(asset);
            if (path.ToLower().Contains(".fbx"))
            {
                ModelImporter model = AssetImporter.GetAtPath(path) as ModelImporter;
                return model.meshCompression;
            }
            return ModelImporterMeshCompression.Off;
        }
        //------------------------------------------------------
        public class GUIEnabled : IDisposable
        {
            [SerializeField]
            private bool PreviousState
            {
                get;
                set;
            }

            public GUIEnabled(bool newState)
            {
                this.PreviousState = GUI.enabled;
                if (!this.PreviousState)
                {
                    GUI.enabled = false;
                    return;
                }
                GUI.enabled = newState;
            }

            public void Dispose()
            {
                GUI.enabled = this.PreviousState;
            }
        }

        public class GUIColor : IDisposable
        {
            [SerializeField]
            private Color PreviousColor
            {
                get;
                set;
            }

            public GUIColor(Color newColor)
            {
                this.PreviousColor = GUI.color;
                GUI.color = newColor;
            }

            public void Dispose()
            {
                GUI.color = this.PreviousColor;
            }
        }

        public class GUIBackgroundColor : IDisposable
        {
            [SerializeField]
            private Color PreviousColor
            {
                get;
                set;
            }

            public GUIBackgroundColor(Color newColor)
            {
                this.PreviousColor = GUI.color;
                GUI.backgroundColor = newColor;
            }

            public void Dispose()
            {
                GUI.backgroundColor = this.PreviousColor;
            }
        }

        public class GUISkinLabelFontStyle : IDisposable
        {
            [SerializeField]
            private FontStyle PreviousStyle
            {
                get;
                set;
            }

            public GUISkinLabelFontStyle(FontStyle newStyle)
            {
                this.PreviousStyle = GUI.skin.label.fontStyle;
                GUI.skin.label.fontStyle = newStyle;
            }

            public void Dispose()
            {
                GUI.skin.label.fontStyle= this.PreviousStyle;
            }
        }
        public class GUISkinLabelNormalTextColor : IDisposable
        {
            [SerializeField]
            private Color PreviousTextColor
            {
                get;
                set;
            }

            public GUISkinLabelNormalTextColor(Color newColor)
            {
                this.PreviousTextColor = GUI.skin.label.normal.textColor;
                GUI.skin.label.normal.textColor = newColor;
            }

            public void Dispose()
            {
                GUI.skin.label.normal.textColor = this.PreviousTextColor;
            }
        }

        public class GUILayoutBeginHorizontal : IDisposable
        {
            public GUILayoutBeginHorizontal()
            {
                GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            }

            public GUILayoutBeginHorizontal(params GUILayoutOption[] layoutOptions)
            {
                GUILayout.BeginHorizontal(layoutOptions);
            }

            public void Dispose()
            {
                GUILayout.EndHorizontal();
            }
        }

        public class GUILayoutBeginVertical : IDisposable
        {
            public GUILayoutBeginVertical()
            {
                GUILayout.BeginVertical(new GUILayoutOption[0]);
            }

            public GUILayoutBeginVertical(params GUILayoutOption[] layoutOptions)
            {
                GUILayout.BeginVertical(layoutOptions);
            }

            public GUILayoutBeginVertical(GUIStyle style, params GUILayoutOption[] layoutOptions)
            {
                GUILayout.BeginVertical(style, layoutOptions);
            }

            public void Dispose()
            {
                GUILayout.EndVertical();
            }
        }

        public class EditorGUIIndentLevel : IDisposable
        {
            [SerializeField]
            private int PreviousIndent
            {
                get;
                set;
            }

            public EditorGUIIndentLevel(int newIndent)
            {
                this.PreviousIndent = EditorGUI.indentLevel;
                EditorGUI.indentLevel = EditorGUI.indentLevel + newIndent;
            }

            public void Dispose()
            {
                EditorGUI.indentLevel = this.PreviousIndent;
            }
        }

        public struct EditorGUIUtilityLabelWidth : IDisposable
        {
            [SerializeField]
            private float PreviousWidth
            {
                get;
                set;
            }

            public EditorGUIUtilityLabelWidth(float newWidth)
            {
                this.PreviousWidth = EditorGUIUtility.labelWidth;
                EditorGUIUtility.labelWidth = newWidth;
            }

            public void Dispose()
            {
                EditorGUIUtility.labelWidth = this.PreviousWidth;
            }
        }

        public class EditorGUIUtilityFieldWidth : IDisposable
        {
            [SerializeField]
            private float PreviousWidth
            {
                get;
                set;
            }

            public EditorGUIUtilityFieldWidth(float newWidth)
            {
                this.PreviousWidth = EditorGUIUtility.fieldWidth;
                EditorGUIUtility.fieldWidth = newWidth;
            }

            public void Dispose()
            {
                EditorGUIUtility.fieldWidth = this.PreviousWidth;
            }
        }

        public class EditorGUIUtilityFieldAndLabelWidth : IDisposable
        {
            [SerializeField]
            private float PreviousWidth
            {
                get;
                set;
            }
            [SerializeField]
            private float PreviousLabelWidth
            {
                get;
                set;
            }
            public EditorGUIUtilityFieldAndLabelWidth(float newWidth, float labelWidth)
            {
                this.PreviousWidth = EditorGUIUtility.fieldWidth;
                this.PreviousLabelWidth = EditorGUIUtility.fieldWidth;
                EditorGUIUtility.fieldWidth = newWidth;
                EditorGUIUtility.labelWidth = labelWidth;
            }

            public void Dispose()
            {
                EditorGUIUtility.fieldWidth = this.PreviousWidth;
                EditorGUIUtility.labelWidth = this.PreviousLabelWidth;
            }
        }

        public class EditorGUILayoutBeginHorizontal : IDisposable
        {
            public EditorGUILayoutBeginHorizontal()
            {
                EditorGUILayout.BeginHorizontal(new GUILayoutOption[0]);
            }

            public EditorGUILayoutBeginHorizontal(params GUILayoutOption[] layoutOptions)
            {
                EditorGUILayout.BeginHorizontal(layoutOptions);
            }

            public void Dispose()
            {
                EditorGUILayout.EndHorizontal();
            }
        }

        public class EditorGUILayoutBeginVertical : IDisposable
        {
            public EditorGUILayoutBeginVertical()
            {
                EditorGUILayout.BeginVertical(new GUILayoutOption[0]);
            }

            public EditorGUILayoutBeginVertical(params GUILayoutOption[] layoutOptions)
            {
                EditorGUILayout.BeginVertical(layoutOptions);
            }

            public EditorGUILayoutBeginVertical(GUIStyle style, params GUILayoutOption[] layoutOptions)
            {
                EditorGUILayout.BeginVertical(style, layoutOptions);
            }

            public void Dispose()
            {
                EditorGUILayout.EndVertical();
            }
        }
    }

}
