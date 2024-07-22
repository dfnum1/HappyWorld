/********************************************************************
生成日期:	4:5:2022  10:15
类    名: 	BubbleRocker
作    者:	HappLI
描    述:	泡泡摇杆
*********************************************************************/
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace TopGame.UI
{
    [ExecuteInEditMode,Framework.Core.ComSerializedExport]
    public class BubbleJoystick : Image
    {
        bool m_bPress = false;
        public float dragMaxDistance = 400;
        public int subDivision = 36;
        Vector3 m_LastPress = Vector3.zero;
        Vector3 m_CurPress = Vector3.zero;
        public RectTransform rockerHandle;
        private System.Action<bool, Vector3, Vector3> m_OnJoystick;
        //------------------------------------------------------
        protected override void Awake()
        {
            this.raycastTarget = false;
            if (rockerHandle)
                rockerHandle.localScale = Vector3.zero;
        }
        //------------------------------------------------------
        public void AddCallback(System.Action<bool, Vector3, Vector3> callback)
        {
            m_OnJoystick += callback;
        }
        //------------------------------------------------------
        public void RemoveCallback(System.Action<bool, Vector3, Vector3> callback)
        {
            m_OnJoystick -= callback;
        }
        //------------------------------------------------------
        public void ClearCallbacks()
        {
            m_OnJoystick = null;
        }
        //------------------------------------------------------
        private void Update()
        {
            if(Input.GetMouseButtonDown(0))
            {
                if (!m_bPress)
                {
                    if (Core.TouchInput.IsUITouching())
                        return;

                    Vector3 temp = Vector3.zero;
                    UI.UIKits.ScreenPosToUIPos(Input.mousePosition, false, ref temp);
                    this.transform.position = temp;
                    m_LastPress = Input.mousePosition;
                }
                m_bPress = true;
                if (rockerHandle)
                    rockerHandle.localScale = Vector3.one;
                this.SetVerticesDirty();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                m_bPress = false;
                if (rockerHandle)
                    rockerHandle.localScale = Vector3.zero;
                this.SetVerticesDirty();
                if (m_OnJoystick != null)
                    m_OnJoystick(m_bPress, m_CurPress, m_LastPress);
            }
            if (m_bPress)
            {
                m_CurPress = Input.mousePosition;
                Vector3 vDir = m_CurPress - m_LastPress;
                if (vDir.sqrMagnitude >= dragMaxDistance * dragMaxDistance)
                    m_CurPress = m_LastPress + vDir.normalized * dragMaxDistance;
                if (rockerHandle)
                {
                    Vector3 temp = Vector3.zero;
                    UI.UIKits.ScreenPosToUIPos(m_CurPress, false, ref temp);
                    rockerHandle.position = temp;
                }
                this.SetVerticesDirty();
                if (m_OnJoystick != null)
                    m_OnJoystick(m_bPress, m_CurPress, m_LastPress);
            }
        }
        //------------------------------------------------------
        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            toFill.Clear();
            Vector3 vDir = m_CurPress - m_LastPress;
             if (vDir.sqrMagnitude <= 0 || !m_bPress)
                 return;
            float fLength = vDir.magnitude;
            if(fLength > 0.001f) vDir /= fLength;
            fLength /= UI.UIKits.GetCanvasScaler();
            Rect rect = GetPixelAdjustedRect();
            Vector3 vStart = Vector3.zero;// m_LastPress;

            Vector3 vRight = Vector3.Cross(vDir, Vector3.forward);
            float fBoxHalf = rect.width/2;


            int nSubDivisionX = 2;
            int nSubDivisionY = subDivision;

            if(fLength <= fBoxHalf)
            {
                for(int j =0; j < nSubDivisionY; ++j)
                {
                    float fVFactor = (float)j / (float)nSubDivisionY;
                    float fVFactorNext = (float)(j+1) / (float)nSubDivisionY;

                    Vector3 vBasePoint = vDir * fBoxHalf * 2 * fVFactor + new Vector3(vStart.x, vStart.y, 0) - vDir * fBoxHalf;
                    Vector3 vBasePointNext = vDir*fBoxHalf*2*fVFactorNext + new Vector3(vStart.x, vStart.y, 0) - vDir * fBoxHalf;

                    for(int i =0; i < nSubDivisionX; ++i)
                    {
                        int cnt = toFill.currentVertCount;
                        {
                            UIVertex ui = new UIVertex();
                            ui.position = vBasePoint + vRight * ((float)(i - nSubDivisionX / 2)) * fBoxHalf * 2f / (float)nSubDivisionX;
                            ui.color = this.color;
                            ui.uv0 = new Vector2((float)i / (float)nSubDivisionX, (float)j / (float)nSubDivisionY);
                            toFill.AddVert(ui);
                        }
                        {
                            UIVertex ui = new UIVertex();
                            ui.position = vBasePoint + vRight * ((float)(i + 1 - nSubDivisionX / 2)) * fBoxHalf * 2f / (float)nSubDivisionX;
                            ui.color = this.color;
                            ui.uv0 = new Vector2((float)(i + 1) / (float)nSubDivisionX, (float)j / (float)nSubDivisionY);
                            toFill.AddVert(ui);
                        }
                        {
                            UIVertex ui = new UIVertex();
                            ui.position = vBasePointNext + vRight * ((float)(i - nSubDivisionX / 2)) * fBoxHalf * 2f / (float)nSubDivisionX;
                            ui.color = this.color;
                            ui.uv0 = new Vector2((float)i / (float)nSubDivisionX, (float)(j+1) / (float)nSubDivisionY);
                            toFill.AddVert(ui);
                        }
                        {
                            UIVertex ui = new UIVertex();
                            ui.position = vBasePointNext + vRight * ((float)(i + 1 - nSubDivisionX / 2)) * fBoxHalf * 2f / (float)nSubDivisionX;
                            ui.color = this.color;
                            ui.uv0 = new Vector2((float)(i+1) / (float)nSubDivisionX, (float)(j+1) / (float)nSubDivisionY);
                            toFill.AddVert(ui);
                        }
                        toFill.AddTriangle(cnt+2, cnt + 0, cnt + 3);
                        toFill.AddTriangle(cnt + 0, cnt + 1, cnt + 3);
                    }
                }
            }
            else
            {
                float halfPI = Mathf.PI * 0.5f;
                float fLengthScale = fLength / (fBoxHalf * 2) + 0.5f;
                for(int j =0; j < nSubDivisionY; ++j)
                {
                    float fVFactor = ((float)j / (float)nSubDivisionY);
                    float fVFactorNext = ((float)(j + 1) / (float)nSubDivisionY);
                    float fTan1 = Mathf.Tan(((float)j / (float)(nSubDivisionY - 1)) * halfPI * 0.5f);
                    float fTan2 = Mathf.Tan(((float)(j + 1) / (float)(nSubDivisionY - 1)) * halfPI * 0.5f);
                    float fBiasedLengthScale = 1f + fTan1 * fTan1 * (fLengthScale - 1f);
                    float fBiasedLengthScaleNext = 1f + fTan2 * fTan2 * (fLengthScale - 1f);
                    Vector3 vBasePoint = vDir * fBoxHalf * 2f * fVFactor * fBiasedLengthScale + new Vector3(vStart.x, vStart.y, 0f) - vDir * fBoxHalf;
                    Vector3 vBasePointNext = vDir * fBoxHalf * 2f * fVFactorNext * fBiasedLengthScaleNext + new Vector3(vStart.x, vStart.y, 0f) - vDir * fBoxHalf;

                    float fSliceLength1 = fBoxHalf * 2f * fVFactor * fBiasedLengthScale;
                    float fScaleFactor1 = 1f - Mathf.Tan(Mathf.Min(1f, (Mathf.Max(0f, fSliceLength1 - fBoxHalf * 2f * fVFactor) / 256f)) * halfPI * 0.5f);
                    float fHalf = fBoxHalf * (fScaleFactor1 * 0.5f + 0.5f);
                    float fSliceLength2 = fBoxHalf * 2f * fVFactorNext * fBiasedLengthScaleNext;
                    float fScaleFactor2 = 1f - Mathf.Tan(Mathf.Min(1f, (Mathf.Max(0f, fSliceLength2 - fBoxHalf * 2f * fVFactorNext) / 256f)) * halfPI * 0.5f);
                    float fHalfNext = fBoxHalf * (fScaleFactor2 * 0.5f + 0.5f);

                    for (int i = 0; i < nSubDivisionX; i++)
                    {
                        int cnt = toFill.currentVertCount;
                        {
                            UIVertex ui = new UIVertex();
                            ui.position = vBasePoint + vRight * ((float)(i - nSubDivisionX / 2)) * fHalf * 2f / (float)nSubDivisionX;
                            ui.color = this.color;
                            ui.uv0 = new Vector2((float)i / (float)nSubDivisionX, (float)j / (float)nSubDivisionY);
                            toFill.AddVert(ui);
                        }
                        {
                            UIVertex ui = new UIVertex();
                            ui.position = vBasePoint + vRight * ((float)(i + 1 - nSubDivisionX / 2)) * fHalf * 2f / (float)nSubDivisionX;
                            ui.color = this.color;
                            ui.uv0 = new Vector2((float)(i + 1) / (float)nSubDivisionX, (float)j / (float)nSubDivisionY);
                            toFill.AddVert(ui);
                        }
                        {
                            UIVertex ui = new UIVertex();
                            ui.position = vBasePointNext + vRight * ((float)(i - nSubDivisionX / 2)) * fHalfNext * 2f / (float)nSubDivisionX;
                            ui.color = this.color;
                            ui.uv0 = new Vector2((float)i / (float)nSubDivisionX, (float)(j + 1) / (float)nSubDivisionY);
                            toFill.AddVert(ui);
                        }
                        {
                            UIVertex ui = new UIVertex();
                            ui.position = vBasePointNext + vRight * ((float)(i + 1 - nSubDivisionX / 2)) * fHalfNext * 2f / (float)nSubDivisionX;
                            ui.color = this.color;
                            ui.uv0 = new Vector2((float)(i + 1) / (float)nSubDivisionX, (float)(j + 1) / (float)nSubDivisionY);
                            toFill.AddVert(ui);
                        }
                        toFill.AddTriangle(cnt + 2, cnt + 0, cnt + 3);
                        toFill.AddTriangle(cnt + 0, cnt + 1, cnt + 3);
                    }
                }
            }

            // briliance

        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(BubbleJoystick), true)]
    [CanEditMultipleObjects]
    public class BubbleRockerEditor : UnityEditor.UI.ImageEditor
    {
        //------------------------------------------------------
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            BubbleJoystick image = target as BubbleJoystick;

            EditorGUILayout.PropertyField(serializedObject.FindProperty("rockerHandle"),true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("subDivision"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("dragMaxDistance"), true);
            serializedObject.ApplyModifiedProperties();
            base.OnInspectorGUI();
        }
    }
#endif
}
