/********************************************************************
生成日期:	25:7:2019   14:35
类    名: 	ActionBatchCreateDefault
作    者:	HappLI
描    述:	动作批量缺省绑定
*********************************************************************/
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using System.Linq;
using System.IO;
using TopGame.Data;
using TopGame.Core;
using Framework.Core;
using Framework.ED;
using Framework.Data;
using Framework.Module;
using ExternEngine;

namespace TopGame.ED
{
    public class AnimatorStateData
    {
        public int layer;
        public string layerName;
        public AnimatorControllerLayer layerPtr;
        public AnimatorState state;
    }

    public class ActionBatchCreateDefault
    {
        //[MenuItem("Assets/创建缺省动作绑定器")]
        //public static void CreateDefault()
        //{
        //    var objects = Selection.gameObjects;
        //    if (objects == null || objects.Length <= 0)
        //        return;
        //    for (int i =0; i < objects.Length; ++i)
        //    {
        //        EditorUtility.DisplayProgressBar("创建", "", (float)i / (float)objects.Length);
        //        BuildDefault(objects[i]);
        //    }
        //    AssetDatabase.SaveAssets();
        //    AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
        //    EditorUtility.ClearProgressBar();
        //}
        ////------------------------------------------------------
        //static void BuildDefault(GameObject pObject)
        //{
        //    if (pObject == null) return;
        //    ActionGraphBinder pBinder = pObject.GetComponent<ActionGraphBinder>();
        //    if (pBinder == null)
        //        pBinder = pObject.AddComponent<ActionGraphBinder>();
        //    else
        //    {
        //        if (!pBinder.usePlayableGraph)
        //            return;
        //    }
        //    if (pBinder.playables != null && pBinder.playables.Length>0)
        //        return;

        //    pBinder.usePlayableGraph = true;

        //    var states = RefreshLocalAnimation(pBinder);

        //    pBinder.playables = null;
        //    HashSet<int> vHashs = new HashSet<int>();
        //    List<PlayableState> vStates = pBinder.playables != null ? new List<PlayableState>(pBinder.playables) : new List<PlayableState>();
        //    foreach (var db in vStates)
        //    {
        //        vHashs.Add(db.nameHash);
        //    }
        //    pBinder.playables = vStates.ToArray();
        //    Animator animator = pObject.GetComponent<Animator>();
        //    if (animator != null)
        //    {
        //        animator.runtimeAnimatorController = null;
        //    }
        //    EditorUtility.SetDirty(pObject);
        //}
        //-----------------------------------------------------
        public static List<AnimatorStateData> RefreshLocalAnimation(AActionGraphBinder binder, string prefabName = null)
        {
            List<AnimatorStateData> vStates = new List<AnimatorStateData>();
            if (binder == null) return vStates;

            if (string.IsNullOrEmpty(prefabName))
                prefabName = binder.gameObject.name;

            string sourcePath = null;
            Animator animator = binder.GetComponent<Animator>();
            if (animator != null && animator.runtimeAnimatorController != null)
            {
                sourcePath = AssetDatabase.GetAssetPath(animator.runtimeAnimatorController);
            }

            if (!string.IsNullOrEmpty(sourcePath))
            {
                if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(sourcePath).Replace("\\", "/") + "/animations"))
                {
                    sourcePath = null;
                }
            }

            if (string.IsNullOrEmpty(sourcePath))
            {
                var renderers = binder.GetComponentsInChildren<Renderer>();
                for (int i = 0; i < renderers.Length; ++i)
                {
                    if (renderers[i] is MeshRenderer)
                    {
                        sourcePath = AssetDatabase.GetAssetPath((renderers[i] as MeshRenderer).additionalVertexStreams);
                        break;
                    }
                    if (renderers[i] is SkinnedMeshRenderer)
                    {
                        sourcePath = AssetDatabase.GetAssetPath((renderers[i] as SkinnedMeshRenderer).sharedMesh);
                        break;
                    }
                }
                if (!string.IsNullOrEmpty(sourcePath))
                {
                    if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(sourcePath).Replace("\\", "/") + "/animations"))
                    {
                        sourcePath = null;
                    }
                }
            }

            if (string.IsNullOrEmpty(sourcePath)) return vStates;
            string prefabNameRef = System.IO.Path.GetFileNameWithoutExtension(sourcePath);
            sourcePath = System.IO.Path.GetDirectoryName(sourcePath).Replace("\\", "/") + "/animations";
            string[] clips = AssetDatabase.FindAssets("t:AnimationClip", new string[] { sourcePath });
            for (int i = 0; i < clips.Length; ++i)
            {
                AnimationClip clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(AssetDatabase.GUIDToAssetPath(clips[i]));
                if (clip != null /*&& (clip.name.Contains(prefabName) || clip.name.Contains(prefabNameRef))*/)
                {
                    AnimatorStateData action = new AnimatorStateData();
                    action.layerName = "BaseLayer";
                    action.layer = 0;
                    action.state = new AnimatorState();
                    action.state.name = clip.name.Replace(prefabName + "_", "").Replace(prefabNameRef + "_", "");
                    action.state.motion = clip;

                    vStates.Add(action);
                }
            }
            return vStates;
        }
    }
}