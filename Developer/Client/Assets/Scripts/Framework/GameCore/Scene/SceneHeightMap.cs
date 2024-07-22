/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	SceneHeightMap
作    者:	HappLI
描    述:	场景高度图
*********************************************************************/
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace TopGame.Core
{
    public class SceneHeightMap : MonoBehaviour
    {
        public Texture2D heightTexture;
    }

#if UNITY_EDITOR
    [CanEditMultipleObjects]
    [CustomEditor(typeof(SceneHeightMap), true)]
    public class SceneHeightMapEditor : Editor
    {
        int m_nWidth = 256;
        int m_nHeight= 256;
        Mesh m_pMesh;
        Material m_pDrawMat;
        //---------------------------------------
        public void OnEnable()
        {
            m_pMesh = new Mesh();
            m_pMesh.hideFlags = HideFlags.DontSave;
            m_pMesh.vertices = new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 1, 0), new Vector3(1, 1, 0), new Vector3(1, 0, 0) };
            m_pMesh.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
            m_pMesh.uv = new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0) };
            m_pDrawMat = new Material(Shader.Find("SD/Particles/SD_Alpha_Blended"));
            m_pDrawMat.hideFlags = HideFlags.DontSave;
            m_pDrawMat.SetTexture("_MainTex", Resources.Load("event_trigger") as Texture2D);

            SceneHeightMap height = target as SceneHeightMap;
            if (height.heightTexture)
            {
                m_nWidth = height.heightTexture.width;
                m_nHeight = height.heightTexture.height;
            }
        }
        private void OnDisable()
        {
            if (m_pMesh) GameObject.DestroyImmediate(m_pMesh);
            if (m_pDrawMat) GameObject.DestroyImmediate(m_pDrawMat);
        }
        //---------------------------------------
        public override void OnInspectorGUI()
        {
            SceneHeightMap height = target as SceneHeightMap;
            m_nWidth = EditorGUILayout.IntField("Width", m_nWidth);
            m_nHeight = EditorGUILayout.IntField("Height", m_nHeight);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("heightTexture"), true);
            if (m_nWidth>0 && m_nHeight > 0 && GUILayout.Button("Bake"))
            {
                if(height.heightTexture == null || height.heightTexture.width != m_nWidth || height.heightTexture.height != m_nHeight)
                {
                    height.heightTexture = new Texture2D(m_nWidth, m_nHeight, TextureFormat.RHalf, false);
                }
                for(int x = 0; x < m_nWidth; ++x)
                {
                    for(int z= 0; z< m_nHeight; ++z)
                    {
                        RaycastHit rayHit;
                        if( Physics.Raycast( new Vector3(x-m_nWidth/2,100, z-m_nHeight/2), Vector3.down, out rayHit, 1000))
                        {
                            height.heightTexture.SetPixel(x, z, new Color(rayHit.point.y, 0, 0, 0));
                        }
                        else
                            height.heightTexture.SetPixel(x, z, new Color(0, 0, 0, 0));
                    }
                }
                height.heightTexture.Apply();
                ED.EditorHelp.SaveToPng(height.heightTexture, Application.dataPath + "/heightMap.png");
            }
        }
        //---------------------------------------
        void OnSceneGUI()
        {
            if (m_pMesh == null || m_pDrawMat == null) return;
            SceneHeightMap height = target as SceneHeightMap;
            Camera camera = null;
            if (SceneView.currentDrawingSceneView) camera = SceneView.currentDrawingSceneView.camera;
            else if(SceneView.lastActiveSceneView) camera = SceneView.lastActiveSceneView.camera;
            if (camera == null) return;
            m_pDrawMat.SetTexture("_MainTex", height.heightTexture);
            Graphics.DrawMesh(m_pMesh, Matrix4x4.TRS(height.transform.position + new Vector3(m_nWidth, 0.1f, m_nHeight/2), height.transform.rotation, new Vector3(m_nWidth/2,0.1f, m_nHeight/2)), m_pDrawMat, 0,camera );
        }
    }
#endif
}
