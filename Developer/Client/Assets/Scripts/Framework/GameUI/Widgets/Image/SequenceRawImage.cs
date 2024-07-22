/********************************************************************
生成日期:	12:28:2020 13:16
类    名: 	SequenceRawImage
作    者:	Happli
描    述:	序列帧
*********************************************************************/

using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace TopGame.UI
{
    [UIWidgetExport]
    public class SequenceRawImage : RawImage
    {
        [System.Serializable]
        public struct Frame
        {
            public Texture2D texture;
            public Rect rect;
        }
        public List<Frame> vFrames = new List<Frame>();
        public bool AutoPlay = false;
        public bool         bLoop = true;
        public int         Fps = 30;

        int m_nCurFrame = 0;
        float m_fDelta = 0;
        float m_fInvFps = 0.0333f;
        bool m_bIsPlaying = false;
        bool m_bFoward = true;
        public int FrameCount
        {
            get
            {
                return vFrames.Count;
            }
        }
        //------------------------------------------------------
        protected override void Start()
        {
            if (Fps > 0) m_fInvFps = 1 / Fps;
            else m_fInvFps = 0.03333f;
            if (AutoPlay)
            {
                PlayFoward();
            }
            else
            {
                m_bIsPlaying = false;
            }
            base.Start();
        }
        //------------------------------------------------------
        public void PlayFoward()
        {
            if (Fps > 0) m_fInvFps = 1 / Fps;
            else m_fInvFps = 0.03333f;
            m_bIsPlaying = true;
            m_bFoward = true;
        }
        //------------------------------------------------------
        public void PlayBack()
        {
            if (Fps > 0) m_fInvFps = 1 / Fps;
            else m_fInvFps = 0.03333f;
            m_bIsPlaying = true;
            m_bFoward = false;
        }
        //------------------------------------------------------
        public void ForceUpdate(float deltaTime)
        {
            if (!m_bIsPlaying || 0 == FrameCount)
            {
                return;
            }
            m_fDelta += deltaTime;
            if (m_fDelta >= m_fInvFps)
            {
                m_fDelta = 0;
                if (m_bFoward) m_nCurFrame++;
                else m_nCurFrame--;

                if (m_nCurFrame >= FrameCount)
                {
                    if (bLoop)
                    {
                        m_nCurFrame = 0;
                    }
                    else
                    {
                        m_bIsPlaying = false;
                        return;
                    }
                }
                else if (m_nCurFrame < 0)
                {
                    if (bLoop)
                    {
                        m_nCurFrame = FrameCount - 1;
                    }
                    else
                    {
                        m_bIsPlaying = false;
                        return;
                    }
                }
                SetSprite(m_nCurFrame);
            }
        }
        //------------------------------------------------------
        void Update()
        {
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
            m_nCurFrame = 0;
            SetSprite(m_nCurFrame);
            m_bIsPlaying = false;
        }
        //------------------------------------------------------
        public void Rewind()
        {
            m_nCurFrame = 0;
            SetSprite(m_nCurFrame);
            PlayFoward();
        }
        //------------------------------------------------------
        void SetSprite(int frame)
        {
            this.texture = vFrames[frame].texture;
            this.uvRect = vFrames[frame].rect;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(SequenceRawImage), true)]
    [CanEditMultipleObjects]
    public class SequenceRawImageEditor : UnityEditor.UI.RawImageEditor
    {
        TopGame.ED.EditorTimer m_pTimer = new TopGame.ED.EditorTimer();
        bool m_bExpand = false;
        //------------------------------------------------------
        protected override void OnEnable()
        {
            EditorApplication.update += Update;
            base.OnEnable();
        }
        //------------------------------------------------------
        protected override void OnDisable()
        {
            EditorApplication.update -= Update;
            base.OnDisable();
        }
        //------------------------------------------------------
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            SequenceRawImage image = target as SequenceRawImage;
            image.AutoPlay = EditorGUILayout.Toggle("AutoPlay", image.AutoPlay);
            image.bLoop = EditorGUILayout.Toggle("Loop", image.bLoop);
            image.Fps = EditorGUILayout.IntField("Fps", image.Fps);

            m_bExpand = EditorGUILayout.Foldout(m_bExpand, "Frames");
            if(m_bExpand)
            {
                EditorGUI.indentLevel++;
                for (int i = 0; i < image.vFrames.Count; ++i)
                {
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("Insert", new GUILayoutOption[] { GUILayout.Width(50) }))
                    {
                        if (i - 1 >= 0)
                            image.vFrames.Insert(i - 1, new SequenceRawImage.Frame());
                        else image.vFrames.Insert(0, new SequenceRawImage.Frame());
                        break;
                    }
                    GUILayout.Label("Frame" + (i + 1));
                    if (GUILayout.Button("Insert", new GUILayoutOption[] { GUILayout.Width(50) }))
                    {
                        image.vFrames.Insert(i,new SequenceRawImage.Frame());
                        break;
                    }
                    if (GUILayout.Button("Del", new GUILayoutOption[] { GUILayout.Width(30) }))
                    {
                        image.vFrames.RemoveAt(i);
                        break;
                    }
                    GUILayout.EndHorizontal();
                    SequenceRawImage.Frame frame = image.vFrames[i];
                    frame.texture = EditorGUILayout.ObjectField("Texture", frame.texture, typeof(Texture2D), false) as Texture2D;
                    frame.rect = EditorGUILayout.RectField("UV", frame.rect);
                    image.vFrames[i] = frame;
                }
                if(image.vFrames.Count<=0)
                {
                    if (GUILayout.Button("AddFrame", new GUILayoutOption[] { GUILayout.Width(30) }))
                    {
                        image.vFrames.Add(new SequenceRawImage.Frame());
                    }
                }
                EditorGUI.indentLevel--;
            }
            if(GUILayout.Button("播放"))
            {
                image.PlayFoward();
            }

            if (GUILayout.Button("停止"))
            {
                image.Stop();
            }
            serializedObject.ApplyModifiedProperties();
            base.OnInspectorGUI();
        }
        //------------------------------------------------------
        void Update()
        {
            m_pTimer.Update();
            SequenceRawImage image = target as SequenceRawImage;
            image.ForceUpdate(m_pTimer.deltaTime);
        }
    }
#endif
}