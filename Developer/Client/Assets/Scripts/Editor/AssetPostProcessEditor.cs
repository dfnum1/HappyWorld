/********************************************************************
生成日期:	1:11:2020 10:09
类    名: 	资源操作进程
作    者:	HappLI
描    述:	资源操作
*********************************************************************/

using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections.Generic;
using UnityEditor.Animations;
using System.IO;

#if UNITY_EDITOR

namespace TopGame.ED
{
    public class FbxPostProcess : AssetPostprocessor
    {
        static void OnPostprocessAllAssets(string[] imported, string[] deleted, string[] moved, string[] movedFrom)
        {
    //        if (Application.isPlaying && Framework.Module.ModuleManager.getInstance() != null)
    //            Framework.Module.ModuleManager.getInstance().PauseJobs();
                
        }
        //-------------------------------------------
        public void OnPreprocessModel()
        {

        }

        public void OnPostprocessModel(GameObject go)
        {
        }
    }

}

#endif