using System.Collections.Generic;
using TopGame.ED;
using UnityEditor;
using UnityEngine;

namespace TopGame.ED
{
    public class MissScriptChecker
    {
        [MenuItem("Tools/检测去除丢失脚本对象")]
        static void CheckMissingComponent()
        {
            int nDirtyCnt = 0;
            string[] objects = AssetDatabase.FindAssets("t:Prefab");
            EditorUtility.DisplayProgressBar("检测", "", 0);
            for (int j = 0; j < objects.Length; ++j)
            {
                string path = AssetDatabase.GUIDToAssetPath(objects[j]);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                EditorUtility.DisplayProgressBar("检测", path, (float)((float)j / (float)objects.Length));

                if (RemoveRecursively(prefab))
                {
                    Debug.Log(path);
                    nDirtyCnt++;
                    EditorUtility.SetDirty(prefab);
                    Debug.Log(path);
                }
            }
            EditorUtility.ClearProgressBar();
            if(nDirtyCnt>0)
            {
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            }
        }

        private static bool RemoveRecursively(GameObject g)
        {
            int cnt = GameObjectUtility.RemoveMonoBehavioursWithMissingScript(g);

            foreach (Transform childT in g.transform)
            {
                if (RemoveRecursively(childT.gameObject)) cnt++;
            }

            return cnt > 0;
        }
        static bool DeleteMissingScripts(Transform target)
        {
            SerializedObject so = new SerializedObject(target.gameObject);
            var soProperties = so.FindProperty("m_Component");
            var components = target.GetComponents<Component>();
            int propertyIndex = 0;
            foreach (var c in components)
            {
                if (c == null)
                {
                    soProperties.DeleteArrayElementAtIndex(propertyIndex);
                }
                else
                {
                    ++propertyIndex;
                }
            }

            return so.ApplyModifiedProperties();
        }

        static bool DeleteChildMissingScripts(Transform target)
        {
            bool bDeling = false;
            foreach (Transform item in target)
            {
                if (DeleteMissingScripts(item))
                    bDeling = true;
                DeleteChildMissingScripts(item);
            }
            return bDeling;
        }
    }
}