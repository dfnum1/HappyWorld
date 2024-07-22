#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;

namespace TopGame.Core.Brush
{
    public struct StandBrushRuntime
    {
        public Vector3 position;
        public Vector3 eulerAngle;
        public Vector3 scale;
    }

    public class BrushDrawer
    {
        EditBrsuhData m_pEditData = null;
        GameObject m_pInstace = null;

        Vector3 m_oldBrushPos;
        Vector3 m_Mouse3DPos;
        Vector2 m_BrushLightUV;
        Color m_BrushLightColor = Color.white;

        public TerrainBrushEditor m_pEditor = null;
        List<StandBrushRuntime> m_vStandDraw = new List<StandBrushRuntime>();
        //------------------------------------------------------
        public void Select(EditBrsuhData data)
        {
            m_pEditData = data;
            if(m_pInstace!=null)
            {
                GameObject.DestroyImmediate(m_pInstace);
                m_pInstace = null;
            }
            if (m_pEditData == null) return;

            if(m_pEditData.brush is BrushRes)
            {
                BrushRes res = m_pEditData.brush as BrushRes;
                GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>(res.strFile);
                if(obj!=null)
                {
                    m_pInstace = GameObject.Instantiate(obj);
                    m_pInstace.hideFlags = HideFlags.DontSave | HideFlags.HideInInspector | HideFlags.HideInInspector;
                }
            }
            else if (m_pEditData.brush is BrushBuffer)
            {
                BrushBuffer res = m_pEditData.brush as BrushBuffer;
                RefreshBrush();
            }
        }
        //------------------------------------------------------
        public void RefreshBrush()
        {
            m_vStandDraw.Clear();
            {
                StandBrushRuntime stand = new StandBrushRuntime();
                stand.position = Vector3.zero;
                stand.eulerAngle = Vector3.up * UnityEngine.Random.Range(0, 360);
                stand.scale = Vector3.one * UnityEngine.Random.Range(0.5f, 2);
                m_vStandDraw.Add(stand);
            }
        }
        //------------------------------------------------------
        public void OnSceneGUI(TerrainBrushEditor pBrushEditor, Camera camera, EBrushMode brushMode, Vector3 brushPos, Vector2 lgihtuv, Color lightColor, Event pEvent = null)
        {
            if (pEvent == null) pEvent = Event.current;
             TerrainBrushEditorRuntime pBrush = pBrushEditor.GetRuntime();
            TerrainBrush terrainBrush = pBrush.GetTerrainBrush();
            m_Mouse3DPos = brushPos;
            m_BrushLightUV = lgihtuv;
            m_BrushLightColor = lightColor;
            Color guiBrushColor = BrushRreferences.GetSettings().guiBrushColor;
            Vector2 randomAngle = BrushRreferences.GetSettings().brushBeginAngle;
            Vector2 randomScale = BrushRreferences.GetSettings().brushScale;
            float brushSpacing = BrushRreferences.GetSettings().brushSpacing;
            float delBrushSize = BrushRreferences.GetSettings().dummyBrushSize;
            float brsuhFlow = BrushRreferences.GetSettings().brsuhFlow;
            if (m_pEditData == null)
            {
                if (pEvent.type == EventType.MouseUp && pEvent.button == 0) m_oldBrushPos = new Vector3(-65000, 0, -65000);
                if (!(pEvent.type == EventType.MouseDown || pEvent.type == EventType.MouseDrag) || pEvent.button != 0) return;

                if (pEvent.type == EventType.MouseDrag && brushSpacing > 0.001f && (brushPos - new Vector3(m_oldBrushPos.x, brushPos.y, m_oldBrushPos.z)).magnitude < brushSpacing * delBrushSize)
                    return;
                if (pEvent.alt)
                {
                    return;
                }
                if (pEvent.shift || brushMode == EBrushMode.Remove)
                {
                    //remove mode
                    Handles.Label(brushPos, "移除模式...");
                    Foliage[] old = pBrush.GetFoliages() != null ? pBrush.GetFoliages().ToArray() : null;
                    if (pBrush.RemoveFoliages(brushPos, BrushRreferences.GetSettings().dummyBrushSize, null))
                    {
                        if (m_pEditor != null && old != null) m_pEditor.RecordUndo(new List<Foliage>(old));
                    }
                    return;
                }
                if (brushMode == EBrushMode.BrushColor)
                {
                    Handles.Label(brushPos, "涂抹颜色...");
                    Foliage[] old = pBrush.GetFoliages() != null ? pBrush.GetFoliages().ToArray() : null;
                    if (pBrush.BrushColor(brushPos, BrushRreferences.GetSettings().dummyBrushSize, BrushRreferences.GetSettings().guiBrushColor, null))
                    {
                        if (m_pEditor != null && old != null) m_pEditor.RecordUndo(new List<Foliage>(old));
                    }
                    return;
                }
                if (brushMode == EBrushMode.RandomSize)
                {
                    Handles.Label(brushPos, "大小笔刷...");
                    Foliage[] old = pBrush.GetFoliages() != null ? pBrush.GetFoliages().ToArray() : null;
                    if (pBrush.BrushSize(brushPos, BrushRreferences.GetSettings().dummyBrushSize, Mathf.Min(randomScale.x, randomScale.y), Mathf.Max(randomScale.x, randomScale.y), null))
                    {
                        if (m_pEditor != null && old != null) m_pEditor.RecordUndo(new List<Foliage>(old));
                    }
                    return;
                }
                m_oldBrushPos = brushPos;
                return;
            }           

            float guiBrushThickness = BrushRreferences.GetSettings().guiBrushThickness;
            int guiBrushNumCorners = BrushRreferences.GetSettings().guiBrushNumCorners;


            if (pEvent.type == EventType.MouseUp && pEvent.button == 0) m_oldBrushPos = new Vector3(-65000, 0, -65000);
            if (!(pEvent.type == EventType.MouseDown || pEvent.type == EventType.MouseDrag) || pEvent.button != 0) return;

            float editSize = m_pEditData.brush.GetSize();
            if (pEvent.shift) editSize = delBrushSize;

            if (pEvent.type == EventType.MouseDrag && brushSpacing > 0.001f && (brushPos - new Vector3(m_oldBrushPos.x, brushPos.y, m_oldBrushPos.z)).magnitude < brushSpacing * editSize)
                return;
            if (pEvent.alt)
            {
                return;
            }
            if (pEvent.shift || brushMode == EBrushMode.Remove)
            {
                //remove mode
                Handles.Label(brushPos, "移除模式...");
                Foliage[] old = pBrush.GetFoliages() != null ? pBrush.GetFoliages().ToArray() : null;
                if (pBrush.RemoveFoliages(brushPos, BrushRreferences.GetSettings().dummyBrushSize, m_pEditData.brush, brsuhFlow))
                {
                    if (m_pEditor != null && old!=null) m_pEditor.RecordUndo(new List<Foliage>(old));
                }
                return;
            }
            if(brushMode == EBrushMode.BrushColor)
            {
                Handles.Label(brushPos, "涂抹颜色...");
                Foliage[] old = pBrush.GetFoliages() != null ? pBrush.GetFoliages().ToArray() : null;
                if (pBrush.BrushColor(brushPos, BrushRreferences.GetSettings().dummyBrushSize, BrushRreferences.GetSettings().guiBrushColor, m_pEditData.brush))
                {
                    if (m_pEditor != null && old != null) m_pEditor.RecordUndo(new List<Foliage>(old));
                }
                return;
            }
            if (brushMode == EBrushMode.RandomSize)
            {
                Handles.Label(brushPos, "大小笔刷...");
                Foliage[] old = pBrush.GetFoliages() != null ? pBrush.GetFoliages().ToArray() : null;
                if (pBrush.BrushSize(brushPos, BrushRreferences.GetSettings().dummyBrushSize, Mathf.Min(randomScale.x, randomScale.y), Mathf.Max(randomScale.x, randomScale.y), m_pEditData.brush))
                {
                    if (m_pEditor != null && old != null) m_pEditor.RecordUndo(new List<Foliage>(old));
                }
                return;
            }
            long key = Mathf.RoundToInt(brushPos.x*10)*100000000 +Mathf.RoundToInt(brushPos.y * 10) *10000 + Mathf.RoundToInt(brushPos.z * 10);
            m_oldBrushPos = brushPos;

            if(!pBrush.HasFoliages(brushPos, BrushRreferences.GetSettings().dummyBrushSize/2, m_pEditData.brush))
            {
                if(delBrushSize<=1)
                {
                    bool bAdd = true;
                    if(UnityEngine.Random.Range(0,100) > brsuhFlow)
                    {
                        bAdd = false;
                    }
                    if(bAdd)
                    {
                        Foliage runtime = new Foliage();
                        runtime.brush = m_pEditData.brush;
                        runtime.useIndex = m_pEditData.brush.guid;
                        runtime.position = brushPos - terrainBrush.transform.position;
                        runtime.lightmap = lightColor.r;
                        runtime.color = new Vector3(guiBrushColor.r, guiBrushColor.g, guiBrushColor.b);
                        runtime.eulerAngle = Vector3.zero + Vector3.up * UnityEngine.Random.Range(Mathf.Min(randomAngle.x, randomAngle.y), Mathf.Max(randomAngle.x, randomAngle.y));
                        runtime.scale = Vector3.one * UnityEngine.Random.Range(Mathf.Min(randomScale.x, randomScale.y), Mathf.Max(randomScale.x, randomScale.y));

                        Foliage[] olds = pBrush.GetFoliages() != null ? pBrush.GetFoliages().ToArray() : null;
                        if (pBrush.AddRuntime(runtime))
                        {
                            if (m_pEditor != null && olds != null) m_pEditor.RecordUndo(new List<Foliage>(olds));
                        }
                    }
                    
                }
                else
                {
                    float cellSize = m_pEditData.brush.GetSize();
                    for (float x = -delBrushSize; x <= (int)delBrushSize; x+= cellSize)
                    {
                        for (float z = -delBrushSize; z <= (int)delBrushSize; z += cellSize)
                        {
                            Vector3 addPos = brushPos + new Vector3(x, 0, z);
                            if ((addPos - brushPos).sqrMagnitude <= delBrushSize * delBrushSize)
                            {
                                bool bAdd = true;
                                if (UnityEngine.Random.Range(0, 100) > brsuhFlow)
                                {
                                    bAdd = false;
                                }
                                if(bAdd)
                                {
                                    Vector2 lightMap1;
                                    Color lightColor1;
                                    if (!pBrushEditor.GetLightColor(camera, ref addPos, out lightMap1, out lightColor1))
                                    {
                                        lightColor1 = lightColor;
                                    }
                                    Foliage runtime = new Foliage();
                                    runtime.brush = m_pEditData.brush;
                                    runtime.useIndex = m_pEditData.brush.guid;
                                    runtime.position = addPos - terrainBrush.transform.position;
                                    runtime.lightmap = lightColor1.r;
                                    runtime.color = new Vector3(guiBrushColor.r, guiBrushColor.g, guiBrushColor.b);
                                    runtime.eulerAngle = Vector3.zero + Vector3.up * UnityEngine.Random.Range(Mathf.Min(randomAngle.x, randomAngle.y), Mathf.Max(randomAngle.x, randomAngle.y));
                                    runtime.scale = Vector3.one * UnityEngine.Random.Range(Mathf.Min(randomScale.x, randomScale.y), Mathf.Max(randomScale.x, randomScale.y));

                                    Foliage[] olds = pBrush.GetFoliages() != null ? pBrush.GetFoliages().ToArray() : null;
                                    if (pBrush.AddRuntime(runtime))
                                    {
                                        if (m_pEditor != null && olds != null) m_pEditor.RecordUndo(new List<Foliage>(olds));
                                    }
                                }
                                
                            }
                        }
                    }

                }

            }  
        }
        //------------------------------------------------------
        Matrix4x4[] matrixls = new Matrix4x4[1023];
        Vector4[] lightcolors = new Vector4[1023];
        Vector4[] uvoffsets = new Vector4[1023];
        MaterialPropertyBlock m_MaterialProp = null;
        CommandBuffer m_CommandBuffer;
        public void Update(Camera camera, TerrainBrushEditorRuntime pBrush)
        {
            if (m_MaterialProp == null) m_MaterialProp = new MaterialPropertyBlock();
            if (m_pEditData != null)
            {
                if(m_pEditor !=null && m_pEditor.GetEditorMode() == EEdtitMode.Brush)
                {
                    if (m_vStandDraw.Count > 0 && m_pEditData.brush is BrushBuffer && m_pEditData.useMatrial)
                    {
                        BrushBuffer buffer = m_pEditData.brush as BrushBuffer;
                        FoliageLodMesh.Lod lod = m_pEditData.brush.GetShareMesh(0);
                        if (lod == null || lod.mesh == null)
                            return;
                        if (pBrush.useCommandBuffer)
                        {
                            if (m_CommandBuffer == null)
                                m_CommandBuffer = new CommandBuffer();
                            m_CommandBuffer.Clear();
                        }

                        int cont = 0;
                        for (int i = 0; i < m_vStandDraw.Count; ++i)
                        {
                            matrixls[cont] = Matrix4x4.TRS(m_vStandDraw[i].position + m_Mouse3DPos, Quaternion.Euler(m_vStandDraw[i].eulerAngle), m_vStandDraw[i].scale);
                            lightcolors[cont] = new Vector4(BrushRreferences.GetSettings().guiBrushColor.r * m_BrushLightColor.r, BrushRreferences.GetSettings().guiBrushColor.g * m_BrushLightColor.r, BrushRreferences.GetSettings().guiBrushColor.b * m_BrushLightColor.r, 1);
                            uvoffsets[cont] = new Vector4(m_pEditData.brush.GetUvOffset().x, m_pEditData.brush.GetUvOffset().y);
                            if (cont >= 1023)
                            {
                                m_MaterialProp.SetVectorArray("_ExternFactor", lightcolors);
                                m_MaterialProp.SetVectorArray("_ExternParam", uvoffsets);
                                if (pBrush.useCommandBuffer) m_CommandBuffer.DrawMeshInstanced(lod.mesh, 0, m_pEditData.useMatrial, 0, matrixls, cont, m_MaterialProp);
                                else Graphics.DrawMeshInstanced(lod.mesh, 0, m_pEditData.useMatrial, matrixls, cont, m_MaterialProp, UnityEngine.Rendering.ShadowCastingMode.Off, false, 0, camera);
                                cont = 0;
                            }
                            else
                                cont++;

                        }
                        if (cont > 0 && m_pEditData.useMatrial)
                        {
                            m_MaterialProp.SetVectorArray("_ExternFactor", lightcolors);
                            m_MaterialProp.SetVectorArray("_ExternParam", uvoffsets);
                            //
                            if (pBrush.useCommandBuffer) m_CommandBuffer.DrawMeshInstanced(lod.mesh, 0, m_pEditData.useMatrial, 0, matrixls, cont, m_MaterialProp);
                            else Graphics.DrawMeshInstanced(lod.mesh, 0, m_pEditData.useMatrial, matrixls, cont, m_MaterialProp, UnityEngine.Rendering.ShadowCastingMode.Off, false, 0, camera);
                            cont = 0;
                        }
                        if (pBrush.useCommandBuffer)
                        {
                            Graphics.ExecuteCommandBuffer(m_CommandBuffer);
                            m_CommandBuffer.Clear();
                        }
                    }
                }
                
                if (m_pInstace != null)
                    m_pInstace.transform.position = m_Mouse3DPos;
            }
            pBrush.DrawInstance(camera);
        }
        //------------------------------------------------------
        public void Clear()
        {
            m_vStandDraw.Clear();
            if(m_CommandBuffer!=null) m_CommandBuffer.Clear();
        }
        //------------------------------------------------------
        public void Destroy()
        {
            if (m_pInstace != null)
            {
                GameObject.DestroyImmediate(m_pInstace);
                m_pInstace = null;
            }
        }
    }
}
#endif