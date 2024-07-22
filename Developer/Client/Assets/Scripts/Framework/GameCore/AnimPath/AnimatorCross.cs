/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	AnimatorCross
作    者:	HappLI
描    述:	动作测试混合预览
*********************************************************************/
using UnityEngine;
namespace TopGame.Logic
{
    public class AnimatorCross : MonoBehaviour
    {
        public string strName1;
        public string strName2;
    }
#if UNITY_EDITOR
    [UnityEditor.CanEditMultipleObjects]
    [UnityEditor.CustomEditor(typeof(AnimatorCross), true)]
    public class AnimatorCrossEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            AnimatorCross setting = target as AnimatorCross;

            setting. strName1 = UnityEditor.EditorGUILayout.TextField("动作1", setting.strName1);
            setting.strName2 = UnityEditor.EditorGUILayout.TextField("动作2", setting.strName2);
            if (GUILayout.Button("播放"))
            {
                Animator m_pPlayer = setting.GetComponent<Animator>();
                m_pPlayer.Play(setting.strName1, 0);
                m_pPlayer.Play(setting.strName2, 1);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}
