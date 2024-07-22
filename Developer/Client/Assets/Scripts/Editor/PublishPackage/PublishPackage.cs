/********************************************************************
生成日期:	25:7:2019   9:51
类    名: 	PublishPackage
作    者:	HappLI
描    述:	发布包体
*********************************************************************/

using UnityEngine;
using UnityEditor;

namespace TopGame.ED
{
    public class PublishPackage : EditorWindow//, IActiveBuildTargetChanged
    {
        public enum ETab
        {
            Publish,
            A_B,
            Packge,
        }
        static string[] TAB_NAMES = { "发布控制面板", "A/B分体包面板", "包信息面板" };
        static PublishPackage ms_pInstance;

        ETab m_eTab = ETab.Publish;

        PublishPanel m_PublishPanel = new PublishPanel();
        PublishA_BPanel m_pA_BPanel = new PublishA_BPanel();
        PackagePanel m_pPackagePanel = new PackagePanel();
        //------------------------------------------------------
        [MenuItem("Tools/发布")]
        public static void Publish()
        {
            if (EditorApplication.isCompiling)
            {
                EditorUtility.DisplayDialog("警告", "请等待编辑器完成编译再执行此功能", "确定");
                return;
            }
            if (ms_pInstance == null)
            {
                PublishPackage window = EditorWindow.GetWindow<PublishPackage>();
                window.titleContent = new GUIContent("发布");
            }
        }
        //------------------------------------------------------
        private void OnDisable()
        {
            m_PublishPanel.OnDisable();
            m_pA_BPanel.OnDisable();
            m_pPackagePanel.OnDisable();
        }
        //------------------------------------------------------
        private void OnEnable()
        {
            this.minSize = new Vector2(800,600);
            m_PublishPanel.OnEnable();
            m_pA_BPanel.OnEnable(m_PublishPanel.getBuildSetting());
            m_pPackagePanel.OnEnable(m_PublishPanel.getBuildSetting());
        }
        //------------------------------------------------------
        private void OnGUI()
        {
            GUILayout.BeginHorizontal();
            Color color = GUI.color;
            for(int i = 0; i < TAB_NAMES.Length; ++i)
            {
                if (m_eTab == (ETab)i)
                    GUI.color = Color.red;
                else
                    GUI.color = color;
                if(GUILayout.Button(TAB_NAMES[i]))
                {
                    if(m_eTab != (ETab)i)
                    {
                        m_eTab = (ETab)i;
                        if(m_eTab == ETab.Packge)
                            m_pPackagePanel.Refresh(m_PublishPanel.GetUpdateVersion());
                        m_PublishPanel.Save();
                        m_pA_BPanel.Refresh(m_PublishPanel.getBuildSetting());
                    }
                }
            }
            GUI.color = color;
            GUILayout.EndHorizontal();

            switch(m_eTab)
            {
                case ETab.Publish:
                    m_PublishPanel.OnGUI(position);
                    break;
                case ETab.Packge:
                    m_pPackagePanel.OnGUI(position);
                    break;
                case ETab.A_B:
                    m_pA_BPanel.OnGUI(position);
                    break;
            }
        }
        //------------------------------------------------------
        private void Update()
        {
            if(m_eTab == ETab.Packge)
                m_pPackagePanel.Update();
        }
    }
}
