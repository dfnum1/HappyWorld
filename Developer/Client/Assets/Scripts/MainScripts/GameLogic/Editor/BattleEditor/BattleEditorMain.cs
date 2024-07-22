
using Framework.Module;
using UnityEngine;
namespace TopGame.ED
{
    public class BattleEditorMain : FrameworkMainEditor
    {
        public float DefaultCameraDistance = 10;
        public Vector3 DefaultCameraEulerAngle = new Vector3(26, 0, 0);
        public float CameraEditorSpeed = 10;
        //------------------------------------------------------
        protected override void Start()
        {
            BattleEditorInstance battleInstance = Framework.Module.ModuleManager.getInstance().RegisterModule<BattleEditorInstance>();
            ModuleManager.mainModule = battleInstance;
            if(ModuleManager.getInstance().Awake(this))
            {
                Data.DataManager.OnLoaded += DoInitStartup;
                Data.DataManager.ReInit();
            }
        }
        //------------------------------------------------------
        protected override void Update()
        {
            base.Update();
           Framework.Core.CameraMode cameraMode = Core.CameraKit.GetCurrentMode();
            if (cameraMode != null)
            {
                Core.EditorCameraMode editor = cameraMode as Core.EditorCameraMode;
                editor.SetMoveSpeed(CameraEditorSpeed);
            }
        }
        //------------------------------------------------------
        void DoInitStartup()
        {
            ModuleManager.getInstance().StartUp(this);
            Logic.FrameworkStartUp.getInstance().SetSection(Logic.EStartUpSection.AppStartUp);
        }
#if UNITY_EDITOR && USE_DIYLEVEL
        //------------------------------------------------------
        [UnityEditor.MenuItem("Tools/RunBattleEditor _F8")]
        static void RunGame()
        {
            ED.EditorUtil.OpenStartUpApplication("Assets/Scenes/Editor/BattleEditor.unity");
        }
#endif
    }
}