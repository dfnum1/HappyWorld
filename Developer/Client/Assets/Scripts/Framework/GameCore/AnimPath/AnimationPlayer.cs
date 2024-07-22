/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	AnimationEventCallback
作    者:	HappLI
描    述:	动画回调
*********************************************************************/
using UnityEngine;
namespace TopGame.Logic
{
    public class AnimationPlayer : MonoBehaviour
    {
        Framework.Core.AInstanceAble m_pAble = null;
        bool m_bOver = false;
        float m_fTime = 0;
        public bool bClearMaterialBlockOverClip = false;
        public bool unScaleTime = false;
        public AnimationClip Clip;
        public bool bAutoPlay = true;
        //------------------------------------------------------
        private void Awake()
        {
            CheckAble();
            m_bOver = false;
        }
        //------------------------------------------------------
        void OnEnable()
        {
            m_fTime = 0;
            m_bOver = false;
        }
        //------------------------------------------------------
        void OnDisable()
        {
            m_fTime = 0;
            m_bOver = true;
        }
        //------------------------------------------------------
        public void SetTime(float fTime)
        {
            m_fTime = fTime;
            if(Clip) Clip.SampleAnimation(this.gameObject, m_fTime);
        }
        //------------------------------------------------------
        private void Update()
        {
            if (Clip == null || m_bOver) return;
            Clip.SampleAnimation(this.gameObject, m_fTime);
            if(bAutoPlay) m_fTime += unScaleTime?Time.unscaledDeltaTime:Time.deltaTime;
            if (!m_bOver && m_fTime >= Clip.length)
            {
                m_bOver = true;
                if(bClearMaterialBlockOverClip)ClearMaterialBlock();
            }
        }
        //------------------------------------------------------
        void CheckAble()
        {
            if (m_pAble == null)
                m_pAble = GetComponent<Framework.Core.AInstanceAble>();
              if(m_pAble == null)
                m_pAble = GetComponentInParent<Framework.Core.AInstanceAble>();
        }
        //------------------------------------------------------
        void ClearMaterialBlock()
        {
            if (m_pAble != null)
                m_pAble.ClearMaterialBlock();
        }
    }
#if UNITY_EDITOR
    [UnityEditor.CanEditMultipleObjects]
    [UnityEditor.CustomEditor(typeof(AnimationPlayer), true)]
    public class AnimationPlayerEditor : UnityEditor.Editor
    {
        public float m_fTime = 0;
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            AnimationPlayer setting = target as AnimationPlayer;

            GUILayout.BeginHorizontal();
            setting.bClearMaterialBlockOverClip = UnityEditor.EditorGUILayout.Toggle("动画结束后清理材质属性", setting.bClearMaterialBlockOverClip);
            UnityEditor.EditorGUILayout.HelpBox("清理可减少Block中断合批的次数", UnityEditor.MessageType.Warning);
            GUILayout.EndHorizontal();
            setting.Clip = UnityEditor.EditorGUILayout.ObjectField("剪辑动画", setting.Clip, typeof(AnimationClip), false) as AnimationClip;
            setting.unScaleTime = UnityEditor.EditorGUILayout.Toggle("UnScaleTime", setting.unScaleTime);
            if(setting.Clip)
            {
                m_fTime = UnityEditor.EditorGUILayout.Slider("播放进度", m_fTime, 0, setting.Clip.length);
                setting.Clip.SampleAnimation(setting.gameObject, m_fTime);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}
