/********************************************************************
生成日期:	25:7:2019   14:35
类    名: 	ActionGraphAssets
作    者:	HappLI
描    述:   动作脚本数据
*********************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Core;
#if UNITY_EDITOR
using UnityEditor;
using TopGame.Data;
#endif
namespace Framework.Core
{
#if UNITY_EDITOR
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ActionGraphAssets), true)]
    public class ActionGraphAssetsEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            ActionGraphAssets controller = target as ActionGraphAssets;
            Color color = GUI.color;
            for (int i = 0; i < controller.datas.Length; ++i)
            {
                if (controller.datas[i] == null)
                    GUI.color = Color.red;
                else
                    GUI.color = color;
                controller.datas[i] = EditorGUILayout.ObjectField((i + 1).ToString(), controller.datas[i], typeof(TextAsset)) as TextAsset;
                GUI.color = color;
            }
            if (GUILayout.Button("刷新"))
            {
                RefreshSync(controller);
            }
            if (GUILayout.Button("提交"))
            {
                RefreshSync(controller);
                string strAGFiles = TopGame.ED.EditorHelp.ServerBinaryRootPath + "ActionGraphDatas.agls";
                UnitySVN.SVNCommit(new string[] { strAGFiles, AssetDatabase.GetAssetPath(target) });
            }
            serializedObject.ApplyModifiedProperties();
        }
        //------------------------------------------------------
        public static void RefreshSync(ActionGraphAssets target, bool bReload=true)
        {
            if (target == null) return;
            string[] files = System.IO.Directory.GetFiles(Application.dataPath + ActionStateManager.SAVE_PATH_ROOT, "*.json", System.IO.SearchOption.AllDirectories);

            Dictionary<string, TextAsset> vMapp = new Dictionary<string, TextAsset>();
            for (int i = 0; i < files.Length; ++i)
            {
                string path = files[i].Replace("\\", "/").Replace(Application.dataPath, "Assets");
                if (path.Contains(".meta")) continue;
                TextAsset asset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);
                if (asset == null) continue;
                if (vMapp.ContainsKey(asset.name))
                {
                    EditorUtility.DisplayDialog("提示", "已存在相同名" + asset.name + "的动作脚本, 请修改", "检查");
                    Selection.activeObject = asset;
                }
                else
                    vMapp.Add(asset.name, asset);
            }
            List<TextAsset> vdatas = new List<TextAsset>();
            CsvData_Player Player = Data.DataEditorUtil.GetTable<CsvData_Player>(bReload);
            if (Player != null)
            {
                foreach (var db in Player.datas)
                {
                    if (string.IsNullOrEmpty(db.Value.actionGraph))
                        continue;

                    if (vMapp.ContainsKey(db.Value.actionGraph) && !vdatas.Contains(vMapp[db.Value.actionGraph]))
                    {
                        vdatas.Add(vMapp[db.Value.actionGraph]);
                    }
                    if (vMapp.ContainsKey(db.Value.showActionGraph) && !vdatas.Contains(vMapp[db.Value.showActionGraph]))
                    {
                        vdatas.Add(vMapp[db.Value.showActionGraph]);
                    }
                }
            }
            CsvData_Monster Monster = Data.DataEditorUtil.GetTable<CsvData_Monster>(bReload);
            if (Monster != null)
            {
                foreach (var db in Monster.datas)
                {
                    if (string.IsNullOrEmpty(db.Value.actionGraph))
                        continue;

                    if (vMapp.ContainsKey(db.Value.actionGraph) && !vdatas.Contains(vMapp[db.Value.actionGraph]))
                    {
                        vdatas.Add(vMapp[db.Value.actionGraph]);
                    }
                }
            }
            CsvData_Summon Summon = Data.DataEditorUtil.GetTable<CsvData_Summon>(bReload);
            if (Summon != null)
            {
                foreach (var db in Summon.datas)
                {
                    if (string.IsNullOrEmpty(db.Value.actionGraph))
                        continue;

                    if (vMapp.ContainsKey(db.Value.actionGraph) && !vdatas.Contains(vMapp[db.Value.actionGraph]))
                    {
                        vdatas.Add(vMapp[db.Value.actionGraph]);
                    }
                }
            }
            target.datas = vdatas.ToArray();
            EditorUtility.SetDirty(target);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            string strRoot = TopGame.ED.EditorHelp.ServerBinaryRootPath;
            if (!System.IO.Directory.Exists(strRoot))
                return;
            Dictionary<string, Framework.Data.BinaryUtil> vBinrays = new Dictionary<string, Data.BinaryUtil>();
            for (int i = 0; i < target.datas.Length; ++i)
            {
                try
                {
                    string path = AssetDatabase.GetAssetPath(target.datas[i]);
                    ActionGraphData pGraph = JsonUtility.FromJson<ActionGraphData>(target.datas[i].text);
                    pGraph.strName = System.IO.Path.GetFileNameWithoutExtension(path);
                    Framework.Data.BinaryUtil binarys = Framework.Core.ActionGraphDataBinaryUtil.Write(pGraph, null);
                    if(binarys.GetCur()>0)
                    {
                        vBinrays[System.IO.Path.GetFileNameWithoutExtension(path)] = binarys;
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.LogError(AssetDatabase.GetAssetPath(target.datas[i]) + " 解析失败");
                }
            }
            string strAGFiles = TopGame.ED.EditorHelp.ServerBinaryRootPath + "ActionGraphDatas.agls";
            Framework.Data.BinaryUtil binaryUtil = new Data.BinaryUtil();
            binaryUtil.WriteInt32(vBinrays.Count);
            foreach (var db in vBinrays)
            {
                binaryUtil.WriteInt32(db.Value.GetCur());
                binaryUtil.WriteBuffers(db.Value.GetBuffer(), db.Value.GetCur());
            }
            binaryUtil.SaveTo(strAGFiles);
        }
    }

#endif
}

