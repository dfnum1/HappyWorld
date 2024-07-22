/********************************************************************
生成日期:	2020-6-16
类    名: 	IslandMaskFogRoot
作    者:	happli
描    述:	岛屿迷雾配置
*********************************************************************/
using UnityEngine;
using Framework.Core;
using TopGame.Core;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace TopGame.Logic
{
    public class IslandMaskFogRoot : ComSerialized
    {
        public Material fogMaterial;
        public Mesh CloudMesh;
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(IslandMaskFogRoot))]
    [CanEditMultipleObjects]
    public class IslandMaskFogRootEditor : AComSerializedEditor
    {
        protected override void OnInnerInspectorGUI()
        {
            IslandMaskFogRoot root = target as IslandMaskFogRoot;
            root.fogMaterial = EditorGUILayout.ObjectField("材质", root.fogMaterial, typeof(Material), false) as Material;
            root.CloudMesh = EditorGUILayout.ObjectField("云模型", root.CloudMesh, typeof(Mesh), false) as Mesh;
        }
    }
#endif
}
