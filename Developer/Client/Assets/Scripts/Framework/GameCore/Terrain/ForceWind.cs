/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	Terrain
作    者:	HappLI
描    述:	
*********************************************************************/
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Framework.Core;
namespace TopGame.Core
{
    public class ForceWind : MonoBehaviour
    {
        public float Distance = 0;
        public float Force = 10;
        public float DampingFactor = 1f;
        private void OnEnable()
        {
            TerrainManager.AddForceWind(transform, Force, Distance, DampingFactor);
        }
        //------------------------------------------------------
        private void OnDisable()
        {
            TerrainManager.RemoveForceWind(transform);
        }
    }
    //------------------------------------------------------
#if UNITY_EDITOR
    [CustomEditor(typeof(ForceWind))]
    [CanEditMultipleObjects]
    public class ForceWindEditor : UnityEditor.Editor
    {
        bool m_bRemoved = false;
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            ForceWind test = target as ForceWind;
            float preDistance = test.Distance;
            Vector3 prePos = test.transform.position;
            test.Distance = EditorGUILayout.FloatField("风力半径", test.Distance);
            test.Force = EditorGUILayout.FloatField("风力强度", test.Force);
            test.DampingFactor = EditorGUILayout.FloatField("衰减因子", test.DampingFactor);
            serializedObject.ApplyModifiedProperties();
            if((preDistance != test.Distance || !Framework.Core.CommonUtility.Equal(prePos, test.transform.position, 0.5f)) && !m_bRemoved )
            {
                TerrainManager.AddForceWind(test.transform, test.Force, test.Distance, test.DampingFactor);
            }

            if (GUILayout.Button("刷新"))
            {
                m_bRemoved = false;
                TerrainManager.AddForceWind(test.transform, test.Force, test.Distance, test.DampingFactor);
            }
            if (GUILayout.Button("移除"))
            {
                m_bRemoved = true;
                TerrainManager.RemoveForceWind(test.transform);
            }

        }
        //------------------------------------------------------
        private void OnDestroy()
        {
            if (target == null) return;
            ForceWind test = target as ForceWind;
            TerrainManager.RemoveForceWind(test.transform);
            m_bRemoved = true;
        }
        //------------------------------------------------------
        private void OnEnable()
        {
            if (target == null) return;
            ForceWind test = target as ForceWind;
            TerrainManager.AddForceWind(test.transform, test.Force, test.Distance, test.DampingFactor);
            m_bRemoved = false;
        }
        //------------------------------------------------------
        private void OnDisable()
        {
            if (target == null) return;
            ForceWind test = target as ForceWind;
            TerrainManager.RemoveForceWind(test.transform);
            m_bRemoved = true;
        }
        //------------------------------------------------------
        private void OnSceneGUI()
        {
            ForceWind test = target as ForceWind;
            Handles.CircleHandleCap(0, test.transform.position, Quaternion.Euler(90,0,0), test.Distance, EventType.Repaint);
        }
    }
#endif
}