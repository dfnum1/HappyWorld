#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

namespace TopGame.Core.Brush
{
    [CustomEditor(typeof(TerrainFoliageDatas))]
    public class TerrainFoliageDatasEditor : Editor
    {
        //------------------------------------------------------
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            TerrainFoliageDatas brush = (TerrainFoliageDatas)target;
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("foliageMatrial"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("treeMaterial"), true);
            GUILayout.BeginHorizontal();
            GUILayout.Box("预制物件");
            if (GUILayout.Button("新增"))
            {
                brush.brushRes.Add(new BrushRes() { });
            }
            GUILayout.EndHorizontal();
            for(int i = 0; i < brush.brushRes.Count; ++i)
            {
                GameObject asset= AssetDatabase.LoadAssetAtPath<GameObject>(brush.brushRes[i].strFile);

                EditorGUILayout.BeginHorizontal();
                asset = EditorGUILayout.ObjectField(brush.brushRes[i].name + "[" + i + "]", asset, typeof(GameObject), false) as GameObject;
                if(asset != null)
                {
                    brush.brushRes[i].strFile = AssetDatabase.GetAssetPath(asset);
                    brush.brushRes[i].name = asset.name;
                    brush.brushRes[i].guid = Animator.StringToHash(asset.name);
                }
                else
                {
                    EditorGUILayout.HelpBox("丢失", MessageType.Error);
                }
                if(GUILayout.Button("删除", new GUILayoutOption[] { GUILayout.Width(50) }))
                {
                    brush.brushRes.RemoveAt(i);
                    break;
                }
                EditorGUILayout.EndHorizontal();
            }

            GUILayout.BeginHorizontal();
            GUILayout.Box("共享Mesh");
            if (GUILayout.Button("新增"))
            {
                if (brush.shareMeshs == null) brush.shareMeshs = new List<FoliageLodMesh>();
                brush.shareMeshs.Add(new FoliageLodMesh());
            }
            GUILayout.EndHorizontal();
            if (brush.shareMeshs != null)
            {
                for(int i =0; i < brush.shareMeshs.Count; ++i)
                {
                    FoliageLodMesh lodMesh = brush.shareMeshs[i];
                    GUILayout.BeginHorizontal();
                    lodMesh.bExpand = EditorGUILayout.Foldout(lodMesh.bExpand, "ShareMesh[" + i + "]");
                  if (GUILayout.Button("移除", new GUILayoutOption[] { GUILayout.Width(40) }))
                    {
                        if (EditorUtility.DisplayDialog("提示", "删除共享Mesh?", "是", "否"))
                        {
                            brush.shareMeshs.RemoveAt(i);
                            break;
                        }
                    }
                    if (GUILayout.Button("添加", new GUILayoutOption[] { GUILayout.Width(40) }))
                    {
                        if (lodMesh.Lods == null) lodMesh.Lods = new List<FoliageLodMesh.Lod>();
                        lodMesh.Lods.Add(new FoliageLodMesh.Lod());
                        break;
                    }
                    GUILayout.EndHorizontal();
                    if(lodMesh.bExpand)
                    {
                        if (lodMesh.Lods != null)
                        {
                            EditorGUI.indentLevel++;
                            for (int j = 0; j < lodMesh.Lods.Count; ++j)
                            {
                                FoliageLodMesh.Lod lod = lodMesh.Lods[j];
                                EditorGUI.indentLevel++;
                                EditorGUILayout.BeginHorizontal();
                                if (GUILayout.Button("Lod[" + (int)lod.distance + "]", new GUILayoutOption[] { GUILayout.Width(100) }))
                                {
                                    if (EditorUtility.DisplayDialog("提示", "删除LOD?", "是", "否"))
                                    {
                                        lodMesh.Lods.RemoveAt(j);
                                        break;
                                    }
                                }
                                EditorGUILayout.EndHorizontal();
                                lod.distance = EditorGUILayout.FloatField("Distance", lod.distance);
                                lod.billboard = EditorGUILayout.Toggle("Billboard", lod.billboard);
                                lod.mesh = EditorGUILayout.ObjectField("Mesh", lod.mesh, typeof(Mesh), false) as Mesh;
                                EditorGUI.indentLevel--;
                            }
                            EditorGUI.indentLevel--;
                        }
                    }
                }
            }
            GUILayout.BeginHorizontal();
            GUILayout.Box("共享笔刷");
            if (GUILayout.Button("新增"))
            {
                if (brush.brushBuffers == null) brush.brushBuffers = new List<BrushBuffer>();
                brush.brushBuffers.Add(new BrushBuffer());
            }
            GUILayout.EndHorizontal();
            if (brush.brushBuffers!=null)
            {
                for (int i = 0; i < brush.brushBuffers.Count; ++i)
                {
                    BrushBuffer buffer = brush.brushBuffers[i];
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.Foldout(true, buffer.name + "[" + buffer.guid + "]");
                        if(GUILayout.Button("删除",new GUILayoutOption[] { GUILayout.Width(40) }))
                        {
                            if(EditorUtility.DisplayDialog("提示", "删除"+ buffer.name + "笔刷?", "是", "否"))
                            {
                                brush.brushBuffers.RemoveAt(i);
                                break;
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                        buffer.type = (EBrushType)EditorGUILayout.EnumPopup("type", buffer.type);
                        buffer.guid = EditorGUILayout.IntField("guid", buffer.guid);
                        buffer.name = EditorGUILayout.TextField("Name", buffer.name);
                        buffer.shareMesh = EditorGUILayout.IntField("ShareMesh", buffer.shareMesh);
                        buffer.bBillboard= EditorGUILayout.Toggle("Billboard", buffer.bBillboard);
                        buffer.usOffset = EditorGUILayout.Vector2Field("UVOffset", buffer.usOffset);
                    }
                }
            }
            Undo.RecordObject(target, "BrushRes");

            serializedObject.ApplyModifiedProperties();

            if(GUILayout.Button("刷新"))
            {
                brush.Refresh();
                EditorUtility.SetDirty(brush);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            }
        }
    }
}
#endif