using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

namespace TopGame.ED
{
	public class AvatarMaskGeneratorEditor : EditorWindow
    {
        static AvatarMaskGeneratorEditor ms_Instance = null;
        static List<Motion> motionClips = new List<Motion>();

		//[MenuItem("Tools/AnimationController", false, 10)]
		static void GeneratorAnimationController()
		{
			//string path = AssetDatabase.GetAssetPath(Selection.activeObject);

			//GUIUtility.systemCopyBuffer = path;

			var objects = Selection.objects;

			motionClips.Clear();

			for (int i = 0; i < objects.Length; ++i)
			{
				string path = AssetDatabase.GetAssetPath(objects[i]);

				var assets = AssetDatabase.LoadAllAssetsAtPath(path);

				if (assets.Length == 0)
					continue;

				for (int j = 0; j < assets.Length; ++j)
				{
					if (assets[j] is Motion)
					{
						var clip = assets[j] as Motion;

						if (clip.name.StartsWith("__preview"))
							continue;

						motionClips.Add(clip);
					}
				}
			}

			if (motionClips.Count == 0)
				return;

            if (ms_Instance == null)
            {
                ms_Instance = ScriptableObject.CreateInstance(typeof(AvatarMaskGeneratorEditor)) as AvatarMaskGeneratorEditor;
            }
         //   ms_Instance.controllerPath = AssetDatabase.GetAssetPath(objects)
            ms_Instance.Show();
            ms_Instance.minSize = new Vector2(600, 300);
        }
        //------------------------------------------------------
		[MenuItem("Assets/生成AvatarMask", false, 10)]
		static void GeneratorAvatarMask()
		{
			//string path = AssetDatabase.GetAssetPath(Selection.activeObject);

			//GUIUtility.systemCopyBuffer = path;

			var obj = Selection.activeObject;

			string path = AssetDatabase.GetAssetPath(obj);

			ModelImporter modelImporter  = AssetImporter.GetAtPath(path) as ModelImporter;

            if (modelImporter == null)
            {
                EditorUtility.DisplayDialog("提示", "请选择一个有效模型再操作", "好的");
                return;
            }

			string maskPath = path.Substring(0, path.LastIndexOf('.')) + "_mask.mask";

            AvatarMaskGenerator.GeneratorAvatarMask(modelImporter, maskPath);
			AssetDatabase.Refresh();
		}
        //------------------------------------------------------
        string controllerPath = "";
        void OnGUI()
		{
			if (motionClips.Count == 0)
			{
				Close();
				return;
			}

			if (GUILayout.Button("Create"))
			{
                controllerPath = EditorUtility.SaveFilePanelInProject("控制器目录", "test", "controller", "", controllerPath);
                AvatarMaskGenerator.GeneratorAnimatorController(motionClips, controllerPath);
				AssetDatabase.Refresh();
			}
		}
	}
}
