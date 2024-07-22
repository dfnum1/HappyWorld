/********************************************************************
生成日期:	1:11:2020 13:07
类    名: 	SceneMgr
作    者:	HappLI
描    述:	
*********************************************************************/
using Framework.Core;
namespace TopGame.Core
{
    public class SceneMgr : ASceneMgr
    {
        private ASceneTheme m_pSceneTheme = null;

        //------------------------------------------------------
        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (m_pSceneTheme != null) m_pSceneTheme.Clear();
        }
        //------------------------------------------------------
        public void SetThemer(ASceneTheme sceneTheme)
        {
            m_pSceneTheme = sceneTheme;
        }
        //------------------------------------------------------
        public static ASceneTheme GetThemer()
        {
            if (GameInstance.getInstance() == null || GameInstance.getInstance().sceneMgr == null) return null;
            return GameInstance.getInstance().sceneManager.m_pSceneTheme;
        }
        //------------------------------------------------------
        public bool LoadScene(ushort sceneId, float fPopDelay = 0, ESceneSignType load = ESceneSignType.PopAll, ESceneSignType unload = ESceneSignType.PopAll)
        {
            SceneParam sceParam = new SceneParam();
            sceParam.sceneID = sceneId;
            sceParam.isCompled = false;
            Data.CsvData_Scene.SceneData sceneData = Data.DataManager.getInstance().Scene.GetData(sceneId);
            if (sceneData == null)
            {
                DoCallback(sceParam);
                return false;
            }
            sceParam.sceneFile = null;
            sceParam.sceneName = sceneData.strName;
            sceParam.load = load;
            sceParam.unload = unload;
            return LoadScene(sceParam, fPopDelay);
        }
        //------------------------------------------------------
        protected override void InnerUpdate(float fFrame)
        {
            if (m_pSceneTheme != null)
                m_pSceneTheme.Update(fFrame);
        }
    }
}

