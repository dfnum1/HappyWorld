#if UNITY_EDITOR
using Framework.Core;
using System.Collections.Generic;
using UnityEngine;
namespace TopGame.ED
{
    public class GridWireBounds
    {
        static float m_fInnerRange = 0.38f;
        private Vector3[] m_verticeData = new Vector3[]
        {
        new Vector3(0.38f, -0.50f, 0.38f),  new Vector3(-0.38f, -0.50f, 0.38f), new Vector3(-0.38f, -0.50f, -0.38f),
        new Vector3(0.38f, -0.50f, -0.38f), new Vector3(0.50f, -0.50f, 0.50f), new Vector3(0.50f, -0.50f, -0.50f),
        new Vector3(-0.50f, -0.50f, -0.50f),new Vector3(-0.50f, -0.50f, 0.50f),new Vector3(0.38f, 0.50f, 0.38f),
        new Vector3(0.38f, 0.50f, -0.38f),  new Vector3(-0.38f, 0.50f, -0.38f), new Vector3(-0.38f, 0.50f, 0.38f),
        new Vector3(0.50f, 0.50f, 0.50f),   new Vector3(-0.50f, 0.50f, 0.50f),  new Vector3(-0.50f, 0.50f, -0.50f),
        new Vector3(0.50f, 0.50f, -0.50f),  new Vector3(0.38f, -0.38f, 0.50f),  new Vector3(0.38f, 0.38f, 0.50f),
        new Vector3(-0.38f, 0.38f, 0.50f),  new Vector3(-0.38f, -0.38f, 0.50f),  new Vector3(0.50f, -0.50f, 0.50f),
        new Vector3(-0.50f, -0.50f, 0.50f),  new Vector3(-0.50f, 0.50f, 0.50f),  new Vector3(0.50f, 0.50f, 0.50f),
        new Vector3(-0.50f, -0.38f, 0.38f),  new Vector3(-0.50f, 0.38f, 0.38f), new Vector3(-0.50f, 0.38f, -0.38f),
        new Vector3(-0.50f, -0.38f, -0.38f), new Vector3(-0.50f, -0.50f, 0.50f),  new Vector3(-0.50f, -0.50f, -0.50f),
        new Vector3(-0.50f, 0.50f, -0.50f),  new Vector3(-0.50f, 0.50f, 0.50f),  new Vector3(-0.38f, -0.38f, -0.50f),
        new Vector3(-0.38f, 0.38f, -0.50f),  new Vector3(0.38f, 0.38f, -0.50f),  new Vector3(0.38f, -0.38f, -0.50f),
        new Vector3(-0.50f, -0.50f, -0.50f),  new Vector3(0.50f, -0.50f, -0.50f),  new Vector3(0.50f, 0.50f, -0.50f),
        new Vector3(-0.50f, 0.50f, -0.50f), new Vector3(0.50f, -0.38f, -0.38f),  new Vector3(0.50f, 0.38f, -0.38f),
        new Vector3(0.50f, 0.38f, 0.38f),   new Vector3(0.50f, -0.38f, 0.38f),  new Vector3(0.50f, -0.50f, -0.50f),
        new Vector3(0.50f, -0.50f, 0.50f),  new Vector3(0.50f, 0.50f, 0.50f),  new Vector3(0.50f, 0.50f, -0.50f)
        };

        private Vector2[] m_uvData = new Vector2[]
        {
        new Vector2(0.88f, 0.12f), new Vector2(0.12f, 0.12f), new Vector2(0.12f, 0.88f),
        new Vector2(0.88f, 0.88f), new Vector2(1.00f, 0.00f), new Vector2(1.00f, 1.00f),
        new Vector2(0.00f, 1.00f), new Vector2(0.00f, 0.00f), new Vector2(0.12f, 0.12f),
        new Vector2(0.12f, 0.88f), new Vector2(0.88f, 0.88f), new Vector2(0.88f, 0.12f),
        new Vector2(0.00f, 0.00f), new Vector2(1.00f, 0.00f), new Vector2(1.00f, 1.00f),
        new Vector2(0.00f, 1.00f), new Vector2(0.12f, 0.12f), new Vector2(0.12f, 0.88f),
        new Vector2(0.88f, 0.88f), new Vector2(0.88f, 0.12f), new Vector2(0.00f, 0.00f),
        new Vector2(1.00f, 0.00f), new Vector2(1.00f, 1.00f), new Vector2(0.00f, 1.00f),
        new Vector2(0.12f, 0.12f), new Vector2(0.12f, 0.88f), new Vector2(0.88f, 0.88f),
        new Vector2(0.88f, 0.12f), new Vector2(0.00f, 0.00f), new Vector2(1.00f, 0.00f),
        new Vector2(1.00f, 1.00f), new Vector2(0.00f, 1.00f), new Vector2(0.12f, 0.12f),
        new Vector2(0.12f, 0.88f), new Vector2(0.88f, 0.88f), new Vector2(0.88f, 0.12f),
        new Vector2(0.00f, 0.00f), new Vector2(1.00f, 0.00f), new Vector2(1.00f, 1.00f),
        new Vector2(0.00f, 1.00f), new Vector2(0.12f, 0.12f), new Vector2(0.12f, 0.88f),
        new Vector2(0.88f, 0.88f), new Vector2(0.88f, 0.12f), new Vector2(0.00f, 0.00f),
        new Vector2(1.00f, 0.00f), new Vector2(1.00f, 1.00f), new Vector2(0.00f, 1.00f)
        };

        private int[] m_triData = new int[]
        {
        0, 1, 2, 0, 2, 3, 4, 0, 3, 4, 3, 5, 5, 3, 2, 5, 2, 6, 6, 2, 1, 6, 1, 7, 7, 1, 0, 7, 0, 4, 8,
        9, 10, 8, 10, 11, 12, 8, 11, 12, 11, 13, 13, 11, 10, 13, 10, 14, 14, 10, 9, 14, 9, 15, 15, 9, 8, 15, 8, 12, 16,
        17, 18, 16, 18, 19, 20, 16, 19, 20, 19, 21, 21, 19, 18, 21, 18, 22, 22, 18, 17, 22, 17, 23, 23, 17, 16, 23, 16, 20, 24,
        25, 26, 24, 26, 27, 28, 24, 27, 28, 27, 29, 29, 27, 26, 29, 26, 30, 30, 26, 25, 30, 25, 31, 31, 25, 24, 31, 24, 28, 32,
        33, 34, 32, 34, 35, 36, 32, 35, 36, 35, 37, 37, 35, 34, 37, 34, 38, 38, 34, 33, 38, 33, 39, 39, 33, 32, 39, 32, 36, 40,
        41, 42, 40, 42, 43, 44, 40, 43, 44, 43, 45, 45, 43, 42, 45, 42, 46, 46, 42, 41, 46, 41, 47, 47, 41, 40, 47, 40, 44,
        };

        private Material m_pMaterial;
        private Mesh m_pMesh;

        public float InnerRange = 0.25f;

        struct Box
        {
            public Transform follow;
            public Mesh mesh;
            public Vector3 size;
            public Vector3 offset;
            public bool visiable;
        }
        List<Box> m_vBoxs = new List<Box>();
        //-----------------------------------------------------
        public void Init(Color color)
        {
       //     m_pMesh = new Mesh();
       //     m_pMesh.vertices = m_verticeData;
       //     m_pMesh.uv = m_uvData;
       //     m_pMesh.triangles = m_triData;

            m_pMaterial = new Material(Shader.Find("SD/Particles/SD_Alpha_Blended"));
            m_pMaterial.SetTexture("_MainTex", Resources.Load<Texture>("GreenBroad"));
            m_pMaterial.SetColor("_TintColor", color);
        }
        //-----------------------------------------------------
        public void SetColor(Color color)
        {
            m_pMaterial.SetColor("_TintColor", color);
        }
        //-----------------------------------------------------
        ~GridWireBounds()
        {
            Destroy();
        }
        //-----------------------------------------------------
        public void Destroy()
        {
            for(int i = 0; i < m_vBoxs.Count; ++i)
            {
                Base.Util.Desytroy(m_vBoxs[i].mesh);
            }
            m_vBoxs.Clear();

            Base.Util.Desytroy(m_pMaterial);
            m_pMaterial = null;

            Base.Util.Desytroy(m_pMesh);
            m_pMesh = null;
        }
        //-----------------------------------------------------
        public void DelBox(Transform transform)
        {
            Box box;
            for (int i = 0; i < m_vBoxs.Count; ++i)
            {
                box = m_vBoxs[i];
                if (box.follow == transform)
                {
                    Base.Util.Desytroy(box.mesh);
                    m_vBoxs.RemoveAt(i);
                    break;
                }
            }
        }
        //-----------------------------------------------------
        public void AddBox(Transform transform, Vector3 size, Vector3 offset)
        {
            Box box;
            for(int i = 0; i < m_vBoxs.Count; ++i)
            {
                box = m_vBoxs[i];
                if (box.follow == transform)
                {
                    if((box.size-size).sqrMagnitude>0)
                    {
                        box.size = size;
                        SetPosSize(ref box);
                    }
                    box.offset = offset;
                    m_vBoxs[i] = box;
                    return;
                }
            }
            box = new Box();
            box.follow = transform;
            box.size = size;
            box.offset = offset;
            box.visiable = true;
            SetPosSize(ref box);
            m_vBoxs.Add(box);
        }
        //-----------------------------------------------------
        void SetPosSize(ref Box box, float yExtend = 0f)
        {
            if (box.size.x < 0f || box.size.y < 0f || box.size.z < 0f) return;
            if(box.mesh == null)
            {
                box.mesh = new Mesh();
                box.mesh.hideFlags = HideFlags.DontSave;
            }
            float halfX = box.size.x * 0.5f;
            float halfY = box.size.y * 0.5f;
            float halfZ = box.size.z * 0.5f;

            Vector3[] vertics = new Vector3[m_verticeData.Length];

            for (int i = 0; i < m_verticeData.Length; ++i)
            {
                vertics[i] = m_verticeData[i];
                //top
                if (Mathf.Abs(m_verticeData[i].y - 0.5f) <= 0.01f && m_verticeData[i].y > 0f)
                {
                    vertics[i].y = halfY;
                }
                else if (Mathf.Abs(m_verticeData[i].y - m_fInnerRange) <= 0.01f && m_verticeData[i].y > 0f)
                {
                    vertics[i].y = halfY - InnerRange;
                }

                //bottom
                if (Mathf.Abs(m_verticeData[i].y + 0.5f) <= 0.01f && m_verticeData[i].y < 0f)
                {
                    vertics[i].y = -halfY;
                }
                else if (Mathf.Abs(m_verticeData[i].y + m_fInnerRange) <= 0.01f && m_verticeData[i].y < 0f)
                {
                    vertics[i].y = -halfY + InnerRange;
                }

                //right
                if (Mathf.Abs(m_verticeData[i].x - 0.5f) <= 0.01f && m_verticeData[i].x > 0f)
                {
                    vertics[i].x = halfX;
                }
                else if (Mathf.Abs(m_verticeData[i].x - m_fInnerRange) <= 0.01f && m_verticeData[i].x > 0f)
                {
                    vertics[i].x = halfX - InnerRange;
                }

                //left
                if (Mathf.Abs(m_verticeData[i].x + 0.5f) <= 0.01f && m_verticeData[i].x < 0f)
                {
                    vertics[i].x = -halfX;
                }
                else if (Mathf.Abs(m_verticeData[i].x + m_fInnerRange) <= 0.01f && m_verticeData[i].x < 0f)
                {
                    vertics[i].x = -halfX + InnerRange;
                }

                //front
                if (Mathf.Abs(m_verticeData[i].z - 0.5f) <= 0.01f && m_verticeData[i].z > 0f)
                {
                    vertics[i].z = halfZ;
                }
                else if (Mathf.Abs(m_verticeData[i].z - m_fInnerRange) <= 0.01f && m_verticeData[i].z > 0f)
                {
                    vertics[i].z = halfZ - InnerRange;
                }

                if (Mathf.Abs(m_verticeData[i].z + 0.5f) <= 0.01f && m_verticeData[i].z < 0f)
                {
                    vertics[i].z = -halfZ;
                }
                else if (Mathf.Abs(m_verticeData[i].z + m_fInnerRange) <= 0.01f && m_verticeData[i].z < 0f)
                {
                    vertics[i].z = -halfZ + InnerRange;
                }
            }

            box.mesh.vertices = vertics;
            box.mesh.uv = m_uvData;
            box.mesh.triangles = m_triData;
        }
        //-----------------------------------------------------
        public void Render(Camera camera = null)
        {
            if (m_pMaterial == null) return;
            Box ins;
            for (int i = 0; i < m_vBoxs.Count;)
            {
                ins = m_vBoxs[i];
                if(ins.follow == null)
                {
                    Base.Util.Desytroy(ins.mesh);
                    m_vBoxs.RemoveAt(i);
                    continue;
                }
                if(ins.visiable && ins.follow && ins.follow.gameObject.activeSelf)
                {
                    Matrix4x4 mt = ins.follow.localToWorldMatrix;
                    Framework.Core.CommonUtility.OffsetPosition(ref mt, ins.offset);
                    if(camera)
                        Graphics.DrawMesh(ins.mesh, mt, m_pMaterial, ins.follow.gameObject.layer, camera);
                    else
                        Graphics.DrawMesh(ins.mesh, mt, m_pMaterial, ins.follow.gameObject.layer);
                }
                ++i;
            }
        }
        //-----------------------------------------------------
        public void RenderToPos(Vector3 pos,Camera camera = null)
        {
            if (m_pMaterial == null) return;
            Box ins;
            for (int i = 0; i < m_vBoxs.Count;)
            {
                ins = m_vBoxs[i];
                if (ins.follow == null)
                {
                    Base.Util.Desytroy(ins.mesh);
                    m_vBoxs.RemoveAt(i);
                    continue;
                }
                if (ins.visiable && ins.follow && ins.follow.gameObject.activeSelf)
                {
                    Matrix4x4 mt = Matrix4x4.TRS(pos, ins.follow.rotation, Vector3.one);
                    if (camera)
                        Graphics.DrawMesh(ins.mesh, mt, m_pMaterial, ins.follow.gameObject.layer, camera);
                    else
                        Graphics.DrawMesh(ins.mesh, mt, m_pMaterial, ins.follow.gameObject.layer);
                }
                ++i;
            }
        }
    }
}
#endif