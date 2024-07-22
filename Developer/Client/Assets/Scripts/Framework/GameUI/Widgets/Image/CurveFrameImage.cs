/********************************************************************
生成日期:	12:28:2020 13:16
类    名: 	CurveFrameImage
作    者:	Happli
描    述:	曲线帧图
*********************************************************************/

using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using Framework.Core;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace TopGame.UI
{
    [UIWidgetExport]
    [ExecuteAlways]
    public class CurveFrameImage : Image
    {
        [System.Serializable]
        public struct Frame
        {
            public float delta;
            public float scale;
            public Graphic graphic;
            //     public Rect uvRect;
        }
        public bool everyRandom = true;
        public float startRandom;
        public Vector3 localOffset;
        public AnimationCurve posX;
        public AnimationCurve posY;
        public AnimationCurve rotate;
        public AnimationCurve aplha;
        public AnimationCurve scale;

        public List<Frame> vFrames = new List<Frame>();
        public bool AutoPlay = false;

        float m_fDelta = 0;
        bool m_bIsPlaying = false;
        float m_fMaxTime = -1;
#if UNITY_EDITOR
        [System.NonSerialized]
        public bool bEditor = false;
#endif
        public int FrameCount
        {
            get
            {
                return vFrames.Count;
            }
        }
        //------------------------------------------------------
        public override Texture mainTexture
        {
            get
            {
                return sprite == null ? Texture2D.grayTexture : sprite.texture;
            }
        }
        //------------------------------------------------------
        protected override void Awake()
        {
            if (AutoPlay)
            {
                Play();
            }
            else
            {
                m_bIsPlaying = false;
            }
            base.Awake();
        }
        //------------------------------------------------------
        public void Play()
        {
            if(!m_bIsPlaying)
            {
                if (startRandom > 0)
                {
                    m_fDelta = -UnityEngine.Random.Range(0,startRandom);
                }
            }

            m_bIsPlaying = true;
        }
        //------------------------------------------------------
        public bool IsPlaying()
        {
            return m_bIsPlaying;
        }
        //------------------------------------------------------
        public void ForceUpdate(float deltaTime)
        {
            if (!m_bIsPlaying || 0 == FrameCount)
            {
                return;
            }
            m_fDelta += deltaTime;
            if (m_fDelta >= GetMaxTime())
            {
                if (everyRandom && startRandom > 0)
                {
                    m_fDelta = -UnityEngine.Random.Range(0, startRandom);
                }
                else m_fDelta = 0;
            }
            Frame frame;
            Graphic graphic = null;
            for (int i = 0; i < vFrames.Count; ++i)
            {
                frame = vFrames[i];
                graphic = frame.graphic;
                if (graphic == null) graphic = this;
                bool validAlpha = false;
                bool validPos = false;
                bool validRotate = false;
                float alpha = 1;
                if (this.aplha != null && Framework.Core.BaseUtil.IsValidCurve(this.aplha))
                {
                    validAlpha = true;
                    alpha = this.aplha.Evaluate(m_fDelta - frame.delta);
                }

                float scale = 1;
                if (this.scale != null && Framework.Core.BaseUtil.IsValidCurve(this.scale)) scale = this.scale.Evaluate(m_fDelta - frame.delta);
                scale *= frame.scale;

                float posX = 0;
                if (this.posX != null && Framework.Core.BaseUtil.IsValidCurve(this.posX))
                {
                    validPos = true;
                    posX = this.posX.Evaluate(m_fDelta - frame.delta);
                }

                float posY = 0;
                if (this.posY != null && Framework.Core.BaseUtil.IsValidCurve(this.posY))
                {
                    validPos = true;
                    posY = this.posY.Evaluate(m_fDelta - frame.delta);
                }

                float rotate = 0;
                if (this.rotate != null && Framework.Core.BaseUtil.IsValidCurve(this.rotate))
                {
                    validRotate = true;
                    rotate = this.rotate.Evaluate(m_fDelta - frame.delta);
                }

                if(validAlpha)
                {
                    Color color = graphic.color;
                    color.a = alpha;
                    graphic.color = color;
                }
                if(validRotate)
                {
                    Vector3 euler = graphic.transform.localEulerAngles;
                    euler.z = rotate;
                    graphic.transform.localEulerAngles = euler;
                }
                graphic.transform.localScale = Vector3.one * scale;
                if(validPos)
                    graphic.rectTransform.localPosition = localOffset + new Vector3(posX, posY,0);
            }
        }
        //------------------------------------------------------
        public float GetTime()
        {
            return m_fDelta;
        }
        //------------------------------------------------------
        public void SetPlayTime(float time)
        {
#if UNITY_EDITOR
            if (0 == FrameCount)
            {
                return;
            }
#else
            if (!m_bIsPlaying || 0 == FrameCount)
            {
                return;
            }
#endif
            m_fDelta = time;
            //  this.SetVerticesDirty();
            //   this.UpdateGeometry();
            ForceUpdate(0);
        }
        //------------------------------------------------------
        public float GetMaxTime()
        {
#if !UNITY_EDITOR
            if (m_fMaxTime >= 0) return m_fMaxTime;
#else
            if(Application.isPlaying)
            {
                if (m_fMaxTime >= 0) return m_fMaxTime;
            }
#endif
            float time = 0;
            time = Mathf.Max(time, Framework.Core.BaseUtil.GetCurveMaxTime(aplha));
            time = Mathf.Max(time, Framework.Core.BaseUtil.GetCurveMaxTime(scale));
            time = Mathf.Max(time, Framework.Core.BaseUtil.GetCurveMaxTime(posX));
            time = Mathf.Max(time, Framework.Core.BaseUtil.GetCurveMaxTime(posY));
            time = Mathf.Max(time, Framework.Core.BaseUtil.GetCurveMaxTime(rotate));
            float frameMax = 0;
            for (int i = 0; i < vFrames.Count; ++i)
            {
                frameMax = Mathf.Max(frameMax, vFrames[i].delta);
            }
            m_fMaxTime = time+ frameMax;
            return m_fMaxTime;
        }
        //------------------------------------------------------
        void Update()
        {
#if UNITY_EDITOR
            if (bEditor) return;
#endif
            ForceUpdate(Time.deltaTime);
        }
        //------------------------------------------------------
        public void Pause()
        {
            m_bIsPlaying = false;
        }
        //------------------------------------------------------
        public void Resume()
        {
            if (!m_bIsPlaying)
            {
                m_bIsPlaying = true;
            }
        }
        //------------------------------------------------------
        public void Stop()
        {
            m_bIsPlaying = false;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(CurveFrameImage), true)]
    [CanEditMultipleObjects]
    public class CurveFrameImageEditor : UnityEditor.UI.GraphicEditor
    {
        bool m_bPreiewing = false;
        float m_fTimer = 0;
        TopGame.ED.EditorTimer m_pTimer = new TopGame.ED.EditorTimer();
        bool m_bExpand = false;
        //------------------------------------------------------
        protected override void OnEnable()
        {
            m_bPreiewing = false;
            CurveFrameImage image = target as CurveFrameImage;
            if (!Application.isPlaying)
                image.bEditor = true;
            EditorApplication.update += Update;
            base.OnEnable();
        }
        //------------------------------------------------------
        protected override void OnDisable()
        {
            CurveFrameImage image = target as CurveFrameImage;
            if (!Application.isPlaying)
                image.bEditor = false;
            EditorApplication.update -= Update;
            base.OnDisable();
        }
        //------------------------------------------------------
        void DrawCurve(string label, AnimationCurve curve)
        {
            GUILayout.BeginHorizontal();
            curve = EditorGUILayout.CurveField(label, curve);
            if(GUILayout.Button("Clear", new GUILayoutOption[] { GUILayout.Width(50) }))
            {
                if (curve != null) curve.keys = null;
            }
            GUILayout.EndHorizontal();
        }
        //------------------------------------------------------
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            CurveFrameImage image = target as CurveFrameImage;
            EditorGUI.BeginChangeCheck();
            image.AutoPlay = EditorGUILayout.Toggle("AutoPlay", image.AutoPlay);
            image.sprite = EditorGUILayout.ObjectField("Sprite", image.sprite, typeof(Sprite),false) as Sprite;

            image.everyRandom = EditorGUILayout.Toggle("Every Random", image.everyRandom);
            image.startRandom = EditorGUILayout.FloatField("random start delay ", image.startRandom);
            image.localOffset = EditorGUILayout.Vector3Field("localOffset", image.localOffset);
            DrawCurve("PosX", image.posX);
            DrawCurve("PosY", image.posY);
            DrawCurve("Rotate", image.rotate);
            DrawCurve("Aplha", image.aplha);
            DrawCurve("Scale", image.scale);

            m_bExpand = EditorGUILayout.Foldout(m_bExpand, "Frames");
            if (m_bExpand)
            {
                EditorGUI.indentLevel++;
                for (int i = 0; i < image.vFrames.Count; ++i)
                {
                    CurveFrameImage.Frame frame = image.vFrames[i];
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Frame" + (i + 1));
                    if (GUILayout.Button("Add", new GUILayoutOption[] { GUILayout.Width(50) }))
                    {
                        image.vFrames.Insert(i + 1, new CurveFrameImage.Frame() { delta = 0, scale = 1 });
                        break;
                    }
                    if (GUILayout.Button("Del", new GUILayoutOption[] { GUILayout.Width(30) }))
                    {
                        image.vFrames.RemoveAt(i);
                        break;
                    }
                    GUILayout.EndHorizontal();
                    frame.delta = EditorGUILayout.FloatField("delta", frame.delta);
                    frame.scale = EditorGUILayout.FloatField("scale", frame.scale);
                    frame.graphic = EditorGUILayout.ObjectField("graphic", frame.graphic, typeof(Graphic), true) as Graphic;
                    //     frame.uvRect = EditorGUILayout.RectField("uv", frame.uvRect);
                    image.vFrames[i] = frame;
                }
                if (image.vFrames.Count <= 0)
                {
                    if (GUILayout.Button("AddFrame", new GUILayoutOption[] { GUILayout.Width(30) }))
                    {
                        image.vFrames.Add(new CurveFrameImage.Frame() { delta = 0, scale = 1 });
                    }
                }
                EditorGUI.indentLevel--;
            }
            if (EditorGUI.EndChangeCheck())
            {
                image.SetMaterialDirty();
                image.SetPlayTime(m_fTimer);
                SceneView.lastActiveSceneView.Repaint();
            }
            serializedObject.ApplyModifiedProperties();
            if(GUILayout.Button("刷新"))
            {
                EditorUtility.SetDirty(target);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            }
            base.OnInspectorGUI();
        }
        //------------------------------------------------------
        private void OnSceneGUI()
        {
            CurveFrameImage image = target as CurveFrameImage;
            GUILayout.BeginArea(new Rect(0, 0, 500, 50));
            EditorGUI.BeginChangeCheck();
            GUILayout.BeginHorizontal();
            m_fTimer = GUILayout.HorizontalSlider(m_fTimer, 0, image.GetMaxTime());
            if (GUILayout.Button("播放", new GUILayoutOption[] { GUILayout.MaxWidth(60) }))
            {
                m_bPreiewing = true;
                image.Play();
            }

            if (GUILayout.Button("停止", new GUILayoutOption[] { GUILayout.MaxWidth(60) }))
            {
                m_bPreiewing = false;
                image.Stop();
            }
            GUILayout.EndHorizontal();
            if (EditorGUI.EndChangeCheck())
            {
                image.SetPlayTime(m_fTimer);
                TopGame.ED.EditorHelp.RepaintPlayModeView();
            }
            GUILayout.EndArea();

        }
        //------------------------------------------------------
        void Update()
        {
            if (Application.isPlaying) return;
            m_pTimer.Update();
            CurveFrameImage image = target as CurveFrameImage;
            if(m_bPreiewing)
            {
                m_fTimer += m_pTimer.deltaTime;
                image.SetPlayTime(m_fTimer);
                if (m_fTimer >= image.GetMaxTime()) m_fTimer = 0;
            }
            TopGame.ED.EditorHelp.RepaintPlayModeView();
            if (SceneView.currentDrawingSceneView) SceneView.currentDrawingSceneView.Repaint();
            SceneView.lastActiveSceneView.Repaint();
        }
    }
#endif
}