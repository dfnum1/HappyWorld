/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	ProjectorLight
作    者:	HappLI
描    述:	探照灯
*********************************************************************/
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace TopGame.Core
{
    public class ProjectorLightCatch
    {
        static List<ProjectorLight> ms_Projectors = new  List<ProjectorLight>(16);
        //------------------------------------------------------
        public static void Add(ProjectorLight light)
        {
            if (light.boundRadius2 <= 0 || light.projectors == null) return;
            int id = light.GetInstanceID();
            if (!ms_Projectors.Contains(light))
                ms_Projectors.Add(light);
        }
        //------------------------------------------------------
        public static void Remove(ProjectorLight light)
        {
            if (light.boundRadius2 <= 0 || light.projectors == null) return;
            ms_Projectors.Remove(light);
        }
        //------------------------------------------------------
        public static bool HasProjector()
        {
            return ms_Projectors.Count > 0;
        }
        //------------------------------------------------------
        public static bool ScanProjector(Vector3 actorPos, out Color lightColor)
        {
            lightColor = Color.white;
            if (ms_Projectors == null) return false;

            float check = 0;
            Vector3 projPos;
            ProjectorLight.Projector projector;
            for (int i = 0; i < ms_Projectors.Count; ++i)
            {
                projPos = ms_Projectors[i].GetPosition();
                check = (projPos - actorPos).sqrMagnitude;
                if (check > ms_Projectors[i].boundRadius2 || ms_Projectors[i].projectors == null) continue;
                for(int j=0; j < ms_Projectors[i].projectors.Length; ++j)
                {
                    projector = ms_Projectors[i].projectors[j];
                    if((projector.offset + projPos - actorPos).sqrMagnitude <= projector.fRadius2)
                    {
                        lightColor = projector.color;
                        return true;
                    }
                }
            }
            return false;
        }
    }

    public class ProjectorLight : MonoBehaviour
    {
        [System.Serializable]
        public struct Projector
        {
            public Vector3 offset;
            public float fRadius2;
            public Color color;
#if UNITY_EDITOR
            [System.NonSerialized]
            public bool bExpand;
#endif
        }
        public Projector[] projectors;
        public float boundRadius2 = 0;
        Transform m_Transform;
        public void Awake()
        {
            m_Transform = transform;
        }
        //------------------------------------------------------
        public Vector3 GetPosition()
        {
            return m_Transform.position;
        }
        //------------------------------------------------------
        private void OnEnable()
        {
            ProjectorLightCatch.Add(this);
        }
        //------------------------------------------------------
        private void OnDisable()
        {
            ProjectorLightCatch.Remove(this);
        }
        //------------------------------------------------------
        private void OnDestroy()
        {
            ProjectorLightCatch.Remove(this);
        }
    }
#if UNITY_EDITOR
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ProjectorLight), true)]
    public class ProjectorLightEditor : Editor
    {
        List<ProjectorLight.Projector> m_vProjectors = null;
        private void OnEnable()
        {
            ProjectorLight controller = target as ProjectorLight;
            ProjectorLightCatch.Add(controller);
            if (controller.projectors != null)
                m_vProjectors = new List<ProjectorLight.Projector>(controller.projectors);
            else
                m_vProjectors = new List<ProjectorLight.Projector>();
        }
        //---------------------------------------
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.HelpBox("生效，需要对应的物件挂载接受器组件：\r\nProjectorLightReciver", MessageType.Warning);
            ProjectorLight controller = target as ProjectorLight;

            EditorGUI.BeginChangeCheck();
            for(int i = 0; i < m_vProjectors.Count; ++i)
            {
                ProjectorLight.Projector projector = m_vProjectors[i];
                EditorGUILayout.BeginHorizontal();
                projector.bExpand = EditorGUILayout.Foldout(projector.bExpand, "探针[" + i + "]");
                if(GUILayout.Button("删除"))
                {
                    Undo.RecordObject(controller, "projectors");
                    m_vProjectors.RemoveAt(i);
                    break;
                }
                EditorGUILayout.EndHorizontal();
                if (projector.bExpand)
                {
                    EditorGUI.indentLevel++;
                    projector.offset = EditorGUILayout.Vector3Field("偏移坐标", projector.offset);
                    float radius = EditorGUILayout.FloatField("半径", Mathf.Sqrt(projector.fRadius2));
                    projector.fRadius2 = radius * radius;
                    projector.color = EditorGUILayout.ColorField("探针颜色", projector.color);
                    EditorGUI.indentLevel--;
                }

                m_vProjectors[i] = projector;
            }
            if (EditorGUI.EndChangeCheck())
            {
                controller.projectors = m_vProjectors.ToArray();
                if (SceneView.lastActiveSceneView)
                    SceneView.lastActiveSceneView.Repaint();
            }

            if(GUILayout.Button("添加"))
            {
                Vector3 offset = Vector3.zero;
                if(m_vProjectors.Count>0)  offset = m_vProjectors[m_vProjectors.Count - 1].offset + Vector3.forward*5;
                m_vProjectors.Add(new ProjectorLight.Projector() { offset = offset, color = Color.gray, fRadius2 = 0 });
                controller.projectors = m_vProjectors.ToArray();
            }

            Vector3 min = Vector3.one * float.MaxValue;
            Vector3 max = Vector3.one * float.MinValue;
            for (int i= 0; i < m_vProjectors.Count; ++i)
            {
                ProjectorLight.Projector projector = m_vProjectors[i];
                float halfRadius = Mathf.Sqrt(projector.fRadius2)/2;
                min = Vector3.Min(projector.offset-new Vector3(halfRadius, 0, halfRadius), min);
                max = Vector3.Max(projector.offset+ new Vector3(halfRadius, 0, halfRadius), max);
            }
            max.y = min.y;
            controller.boundRadius2 = ((max - min)/2).sqrMagnitude;

            serializedObject.ApplyModifiedProperties();
        }
        //---------------------------------------
        void OnSceneGUI()
        {
            Vector3 min = Vector3.one * float.MaxValue;
            Vector3 max = Vector3.one * float.MinValue;
            ProjectorLight controller = target as ProjectorLight;
            Color color = Handles.color;

            for (int i = 0; i < m_vProjectors.Count; ++i)
            {
                ProjectorLight.Projector projector = m_vProjectors[i];
                float halfRadius = Mathf.Sqrt(projector.fRadius2) / 2;
                Vector3 pos = controller.transform.position + projector.offset;
                Handles.color = new Color(projector.color.r, projector.color.g, projector.color.b, 0.8f);
                Handles.SphereHandleCap(0, pos, Quaternion.identity, Mathf.Sqrt(projector.fRadius2), EventType.Repaint);
                projector.offset = Handles.PositionHandle(pos, Quaternion.identity) - controller.transform.position;
                m_vProjectors[i] = projector;

                min = Vector3.Min(pos - new Vector3(halfRadius, 0, halfRadius), min);
                max = Vector3.Max(pos + new Vector3(halfRadius, 0, halfRadius), max);
            }

            Handles.color = Color.red;

            Handles.DrawWireCube((max + min)/2, max - min);
            Handles.CircleHandleCap(0, (max + min) / 2, Quaternion.Euler(90, 0, 0), Mathf.Sqrt(controller.boundRadius2), EventType.Repaint);

            for (int i = 0; i < m_vProjectors.Count; ++i)
            {
                ProjectorLight.Projector projector = m_vProjectors[i];
                Vector3 pos = controller.transform.position + projector.offset;
                Vector2 position2 = HandleUtility.WorldToGUIPoint(pos);
                GUILayout.BeginArea(new Rect(position2.x - 100, position2.y, 200, 30));
                float size = EditorGUILayout.Slider(Mathf.Sqrt(projector.fRadius2), 0, 200);
                projector.fRadius2 = size * size;
                GUILayout.EndArea();
                m_vProjectors[i] = projector;
            }
            Handles.color = color;
        }
    }
#endif
}
