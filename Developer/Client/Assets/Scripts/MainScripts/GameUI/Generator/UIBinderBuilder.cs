//auto generator
namespace TopGame.UI
{
	public partial class UIBinderBuilder
	{
		public static UIBase Create(ushort type)
		{
			switch(type)
			{
				case 255:
				{
					UIBase pUI = new TopGame.UI.UICommonTip();
					TopGame.UI.UICommonTipView pView = new TopGame.UI.UICommonTipView();
					pUI.SetView( pView );
					return pUI;
				}
				case 12:
				{
					UIBase pUI = new TopGame.UI.DialogPanel();
					TopGame.UI.DialogView pView = new TopGame.UI.DialogView();
					pUI.SetView( pView );
					return pUI;
				}
				case 2:
				{
					UIBase pUI = new TopGame.UI.UIFullScreenFillPanel();
					TopGame.UI.UIFullScreenFillView pView = new TopGame.UI.UIFullScreenFillView();
					pUI.SetView( pView );
					return pUI;
				}
				case 13:
				{
					UIBase pUI = new TopGame.UI.UIGMPanel();
					{
						TopGame.UI.GMChapterLogic pLogic = new TopGame.UI.GMChapterLogic();
						pUI.AddLogic( pLogic );
					}
					TopGame.UI.UIGMPanelView pView = new TopGame.UI.UIGMPanelView();
					pUI.SetView( pView );
					return pUI;
				}
				case 11:
				{
					UIBase pUI = new TopGame.UI.UIGameInfo();
					TopGame.UI.UIGameInfoView pView = new TopGame.UI.UIGameInfoView();
					pUI.SetView( pView );
					return pUI;
				}
				case 254:
				{
					UIBase pUI = new TopGame.UI.GuidePanel();
					TopGame.UI.UIGuideView pView = new TopGame.UI.UIGuideView();
					pUI.SetView( pView );
					return pUI;
				}
				case 1:
				{
					UIBase pUI = new TopGame.UI.UILoading();
					TopGame.UI.UILoadingView pView = new TopGame.UI.UILoadingView();
					pUI.SetView( pView );
					return pUI;
				}
				case 10:
				{
					UIBase pUI = new TopGame.UI.UILogin();
					{
						TopGame.UI.UILoginSDKLogic pLogic = new TopGame.UI.UILoginSDKLogic();
						pUI.AddLogic( pLogic );
					}
					TopGame.UI.UILoginView pView = new TopGame.UI.UILoginView();
					pUI.SetView( pView );
					return pUI;
				}
				case 3:
				{
					UIBase pUI = new TopGame.UI.TransitionPanel();
					TopGame.UI.TransitionView pView = new TopGame.UI.TransitionView();
					pUI.SetView( pView );
					return pUI;
				}
				case 256:
				{
					UIBase pUI = new TopGame.UI.UIVideo();
					TopGame.UI.UIVideoView pView = new TopGame.UI.UIVideoView();
					pUI.SetView( pView );
					return pUI;
				}
				case 200:
				{
					UIBase pUI = new TopGame.UI.WaitPanel();
					TopGame.UI.WaitView pView = new TopGame.UI.WaitView();
					pUI.SetView( pView );
					return pUI;
				}
			}
			return null;
		}
		static System.Collections.Generic.Dictionary<System.Type, int> ms_vUITypeMaps = null;
		static void CheckTypeMapping()
		{
			if(ms_vUITypeMaps == null)
			{
				ms_vUITypeMaps = new System.Collections.Generic.Dictionary<System.Type, int>(11);
				ms_vUITypeMaps[typeof(TopGame.UI.UICommonTip)] = 255;
				ms_vUITypeMaps[typeof(TopGame.UI.DialogPanel)] = 12;
				ms_vUITypeMaps[typeof(TopGame.UI.UIFullScreenFillPanel)] = 2;
				ms_vUITypeMaps[typeof(TopGame.UI.UIGMPanel)] = 13;
				ms_vUITypeMaps[typeof(TopGame.UI.UIGameInfo)] = 11;
				ms_vUITypeMaps[typeof(TopGame.UI.GuidePanel)] = 254;
				ms_vUITypeMaps[typeof(TopGame.UI.UILoading)] = 1;
				ms_vUITypeMaps[typeof(TopGame.UI.UILogin)] = 10;
				ms_vUITypeMaps[typeof(TopGame.UI.TransitionPanel)] = 3;
				ms_vUITypeMaps[typeof(TopGame.UI.UIVideo)] = 256;
				ms_vUITypeMaps[typeof(TopGame.UI.WaitPanel)] = 200;
			}
		}
		public static int GetTypeToUIType(System.Type type)
		{
			CheckTypeMapping();
			int result =0;
			if(ms_vUITypeMaps.TryGetValue(type, out result)) return result;
			return 0;
		}
	}
}
