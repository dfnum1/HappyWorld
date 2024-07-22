//auto genreator
namespace Framework.Plugin.AT
{
#if UNITY_EDITOR
	[ATExport("UI系统",true)]
#endif
	public static class AgentTree_UIManager
	{
#if UNITY_EDITOR
		[ATMonoFunc(1670306757,"清理所有",typeof(TopGame.UI.UIManager))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.UI.UIManager),null, "", false, -1, true,false)]
#endif
		public static bool AT_Clear(TopGame.UI.UIManager pPointer)
		{
			pPointer.Clear();
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1547098398,"获取设备型号",typeof(TopGame.UI.UIManager))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.UI.UIManager),null, "", false, -1, true,false)]
		[ATMethodReturn(typeof(VariableString),"pReturn",null,null)]
#endif
		public static bool AT_GetPhoneType(TopGame.UI.UIManager pPointer, VariableString pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetPhoneType();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-754769662,"设置适配边缘偏移",typeof(TopGame.UI.UIManager))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.UI.UIManager),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableFloat),"left",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"top",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"right",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"bottom",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"posZ",false, null,null)]
#endif
		public static bool AT_SetCanvasBorderOffset(VariableFloat left,VariableFloat top,VariableFloat right,VariableFloat bottom,VariableFloat posZ)
		{
			if(left== null || top== null || right== null || bottom== null || posZ== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			TopGame.UI.UIManager.SetCanvasBorderOffset(left.mValue,top.mValue,right.mValue,bottom.mValue,posZ.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1048712169,"UI根节点",typeof(TopGame.UI.UIManager))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.UI.UIManager),null, "", false, -1, true,false)]
		[ATMethodReturn(typeof(VariableObject),"pReturn",typeof(UnityEngine.Transform),null)]
#endif
		public static bool AT_GetRoot(TopGame.UI.UIManager pPointer, VariableObject pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetRoot();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(2006159075,"跟节点",typeof(TopGame.UI.UIManager))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.UI.UIManager),null, "", false, -1, true,false)]
		[ATMethodReturn(typeof(VariableObject),"pReturn",typeof(UnityEngine.Transform),null)]
#endif
		public static bool AT_GetRoot3D(TopGame.UI.UIManager pPointer, VariableObject pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetRoot3D();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1009572667,"界面是否显示",typeof(TopGame.UI.UIManager))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.UI.UIManager),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableInt),"type",false, null,typeof(TopGame.UI.EUIType))]
		[ATMethodReturn(typeof(VariableBool),"pReturn",null,null)]
#endif
		public static bool AT_IsWinShow(TopGame.UI.UIManager pPointer, VariableInt type, VariableBool pReturn=null)
		{
			if(type== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.IsWinShow((ushort)type.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1587046863,"显示所有",typeof(TopGame.UI.UIManager))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.UI.UIManager),null, "", false, -1, true,false)]
#endif
		public static bool AT_ShowAll(TopGame.UI.UIManager pPointer)
		{
			pPointer.ShowAll();
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-2073083593,"显示一个UI",typeof(TopGame.UI.UIManager))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.UI.UIManager),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableInt),"type",false, null,typeof(TopGame.UI.EUIType))]
		[ATMethodReturn(typeof(VariableUser),"pReturn",typeof(Framework.UI.UIHandle),null)]
#endif
		public static bool AT_ShowUI(TopGame.UI.UIManager pPointer, VariableInt type, VariableUser pReturn=null)
		{
			if(type== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.ShowUI((ushort)type.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1550238933,"隐藏一个UI",typeof(TopGame.UI.UIManager))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.UI.UIManager),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableInt),"type",false, null,typeof(TopGame.UI.EUIType))]
#endif
		public static bool AT_HideUI(TopGame.UI.UIManager pPointer, VariableInt type)
		{
			if(type== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.HideUI((ushort)type.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1739952387,"隐藏所有",typeof(TopGame.UI.UIManager))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.UI.UIManager),null, "", false, -1, true,false)]
#endif
		public static bool AT_HideAll(TopGame.UI.UIManager pPointer)
		{
			pPointer.HideAll();
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(389852323,"移动一个UI到屏幕外",typeof(TopGame.UI.UIManager))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.UI.UIManager),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableInt),"type",false, null,typeof(TopGame.UI.EUIType))]
#endif
		public static bool AT_MoveOutside(TopGame.UI.UIManager pPointer, VariableInt type)
		{
			if(type== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.MoveOutside((TopGame.UI.EUIType)type.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1680738204,"移动一个UI到屏幕内",typeof(TopGame.UI.UIManager))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.UI.UIManager),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableInt),"type",false, null,typeof(TopGame.UI.EUIType))]
#endif
		public static bool AT_MoveInside(TopGame.UI.UIManager pPointer, VariableInt type)
		{
			if(type== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.MoveInside((TopGame.UI.EUIType)type.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(493131080,"关闭一个UI",typeof(TopGame.UI.UIManager))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.UI.UIManager),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableInt),"type",false, null,typeof(TopGame.UI.EUIType))]
#endif
		public static bool AT_CloseUI(TopGame.UI.UIManager pPointer, VariableInt type)
		{
			if(type== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.CloseUI((ushort)type.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(589444821,"关闭所有",typeof(TopGame.UI.UIManager))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.UI.UIManager),null, "", false, -1, true,false)]
#endif
		public static bool AT_CloseAll(TopGame.UI.UIManager pPointer)
		{
			pPointer.CloseAll();
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1167764636,"获取一个UI",typeof(TopGame.UI.UIManager))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.UI.UIManager),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableInt),"type",false, null,typeof(TopGame.UI.EUIType))]
		[ATMethodArgv(typeof(VariableBool),"bAuto",false, null,null)]
		[ATMethodReturn(typeof(VariableUser),"pReturn",typeof(TopGame.UI.UIBase),null)]
#endif
		public static bool AT_GetUI(TopGame.UI.UIManager pPointer, VariableInt type, VariableBool bAuto, VariableUser pReturn=null)
		{
			if(type== null || bAuto== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetUI((ushort)type.mValue,bAuto.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(207487781,"创建一个UI",typeof(TopGame.UI.UIManager))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.UI.UIManager),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableInt),"type",false, null,typeof(TopGame.UI.EUIType))]
		[ATMethodArgv(typeof(VariableBool),"bShow",false, null,null)]
		[ATMethodReturn(typeof(VariableUser),"pReturn",typeof(Framework.UI.UIHandle),null)]
#endif
		public static bool AT_CreateUI(TopGame.UI.UIManager pPointer, VariableInt type, VariableBool bShow, VariableUser pReturn=null)
		{
			if(type== null || bShow== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.CreateUI((ushort)type.mValue,bShow.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1731505729,"UGUI坐标转屏幕坐标",typeof(TopGame.UI.UIManager))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.UI.UIManager),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableVector3),"uiWorldPos",false, null,null)]
		[ATMethodReturn(typeof(VariableVector3),"pReturn",null,null)]
#endif
		public static bool AT_ConvertUIPosToScreen(TopGame.UI.UIManager pPointer, VariableVector3 uiWorldPos, VariableVector3 pReturn=null)
		{
			if(uiWorldPos== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.ConvertUIPosToScreen(uiWorldPos.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-102619088,"屏幕坐标转UGUI坐标",typeof(TopGame.UI.UIManager))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.UI.UIManager),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableVector3),"screenPos",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bLocal",false, null,null)]
		[ATMethodArgv(typeof(VariableVector3),"point",false, null,null)]
		[ATMethodReturn(typeof(VariableBool),"pReturn",null,null)]
#endif
		public static bool AT_ConvertScreenToUIPos(TopGame.UI.UIManager pPointer, VariableVector3 screenPos, VariableBool bLocal, VariableVector3 point, VariableBool pReturn=null)
		{
			if(screenPos== null || bLocal== null || point== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.ConvertScreenToUIPos(screenPos.mValue,bLocal.mValue,ref point.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-790294378,"世界坐标转UGUI坐标",typeof(TopGame.UI.UIManager))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.UI.UIManager),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableVector3),"worldPos",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bLocal",false, null,null)]
		[ATMethodArgv(typeof(VariableVector3),"point",false, null,null)]
		[ATMethodArgv(typeof(VariableMonoScript),"cam",false, typeof(UnityEngine.Camera),null)]
		[ATMethodReturn(typeof(VariableBool),"pReturn",null,null)]
#endif
		public static bool AT_ConvertWorldPosToUIPos(TopGame.UI.UIManager pPointer, VariableVector3 worldPos, VariableBool bLocal, VariableVector3 point, VariableMonoScript cam, VariableBool pReturn=null)
		{
			if(worldPos== null || bLocal== null || point== null || cam== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.ConvertWorldPosToUIPos(worldPos.mValue,bLocal.mValue,ref point.mValue,cam.ToObject<UnityEngine.Camera>());
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-336781782,"UGUI坐标转世界坐标",typeof(TopGame.UI.UIManager))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.UI.UIManager),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableMonoScript),"cam",false, typeof(UnityEngine.Camera),null)]
		[ATMethodArgv(typeof(VariableVector3),"uiguiPos",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"distance",false, null,null)]
		[ATMethodReturn(typeof(VariableVector3),"pReturn",null,null)]
#endif
		public static bool AT_UGUIPosToWorldPos(TopGame.UI.UIManager pPointer, VariableMonoScript cam, VariableVector3 uiguiPos, VariableFloat distance, VariableVector3 pReturn=null)
		{
			if(cam== null || uiguiPos== null || distance== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.UGUIPosToWorldPos(cam.ToObject<UnityEngine.Camera>(),uiguiPos.mValue,distance.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-2086069240,"连续打开界面",typeof(TopGame.UI.UIManager))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.UI.UIManager),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableIntList),"uis",false, null,null)]
#endif
		public static bool AT_SequenceOpenUI(TopGame.UI.UIManager pPointer, VariableIntList uis)
		{
			if(uis== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			{//"catch temp"
				if(ms_UInt16_Catch_0 == null) ms_UInt16_Catch_0= new System.Collections.Generic.List<System.UInt16>();
				for(int i = 0; i < uis.GetList().Count; ++i)
				{
					if(uis.mValue[i]!=null)
						ms_UInt16_Catch_0.Add( (System.UInt16)uis.mValue[i]);
				}
			}
			pPointer.SequenceOpenUI(ms_UInt16_Catch_0);
			if(ms_UInt16_Catch_0!=null) ms_UInt16_Catch_0.Clear();
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-716547496,"隐藏GameObject",typeof(TopGame.UI.UIManager))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.UI.UIManager),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableObject),"Go",false, typeof(UnityEngine.GameObject),null)]
#endif
		public static bool AT_HideGameObject(TopGame.UI.UIManager pPointer, VariableObject Go)
		{
			if(Go== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.HideGameObject(Go.ToObject<UnityEngine.GameObject>());
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1746047024,"显示GameObject",typeof(TopGame.UI.UIManager))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.UI.UIManager),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableObject),"Go",false, typeof(UnityEngine.GameObject),null)]
#endif
		public static bool AT_ShowGameObject(TopGame.UI.UIManager pPointer, VariableObject Go)
		{
			if(Go== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.ShowGameObject(Go.ToObject<UnityEngine.GameObject>());
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1807452822,"显示GameObjects",typeof(TopGame.UI.UIManager))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.UI.UIManager),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableObjectList),"param",false, null,null)]
#endif
		public static bool AT_ShowGameObjects(TopGame.UI.UIManager pPointer, VariableObjectList param)
		{
			if(param== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			{//"catch temp"
				if(ms_GameObject_Catch_0 == null) ms_GameObject_Catch_0= new System.Collections.Generic.List<UnityEngine.GameObject>();
				for(int i = 0; i < param.GetList().Count; ++i)
				{
					if(param.mValue[i]!=null)
						ms_GameObject_Catch_0.Add( (UnityEngine.GameObject)param.mValue[i]);
				}
			}
			pPointer.ShowGameObjects(ms_GameObject_Catch_0);
			if(ms_GameObject_Catch_0!=null) ms_GameObject_Catch_0.Clear();
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1511308907,"隐藏GameObjects",typeof(TopGame.UI.UIManager))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.UI.UIManager),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableObjectList),"param",false, null,null)]
#endif
		public static bool AT_HideGameObjects(TopGame.UI.UIManager pPointer, VariableObjectList param)
		{
			if(param== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			{//"catch temp"
				if(ms_GameObject_Catch_1 == null) ms_GameObject_Catch_1= new System.Collections.Generic.List<UnityEngine.GameObject>();
				for(int i = 0; i < param.GetList().Count; ++i)
				{
					if(param.mValue[i]!=null)
						ms_GameObject_Catch_1.Add( (UnityEngine.GameObject)param.mValue[i]);
				}
			}
			pPointer.HideGameObjects(ms_GameObject_Catch_1);
			if(ms_GameObject_Catch_1!=null) ms_GameObject_Catch_1.Clear();
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-2014383755,"GameObject是否显示",typeof(TopGame.UI.UIManager))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.UI.UIManager),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableObject),"Go",false, typeof(UnityEngine.GameObject),null)]
		[ATMethodReturn(typeof(VariableBool),"pReturn",null,null)]
#endif
		public static bool AT_IsGameObjectShow(TopGame.UI.UIManager pPointer, VariableObject Go, VariableBool pReturn=null)
		{
			if(Go== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.IsGameObjectShow(Go.ToObject<UnityEngine.GameObject>());
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1884369391,"获取UI资源文件",typeof(TopGame.UI.UIManager))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.UI.UIManager),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableInt),"type",false, null,typeof(TopGame.UI.EUIType))]
		[ATMethodReturn(typeof(VariableString),"pReturn",null,null)]
#endif
		public static bool AT_GetUIAssetFile(TopGame.UI.UIManager pPointer, VariableInt type, VariableString pReturn=null)
		{
			if(type== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetUIAssetFile((ushort)type.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-410715753,"预实例化UI资源",typeof(TopGame.UI.UIManager))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.UI.UIManager),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableIntList),"uis",false, null,typeof(TopGame.UI.EUIType))]
		[ATMethodArgv(typeof(VariableBool),"bAsync",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bFrontQueue",false, null,null)]
#endif
		public static bool AT_PreSpawnUI(TopGame.UI.UIManager pPointer, VariableIntList uis, VariableBool bAsync, VariableBool bFrontQueue)
		{
			if(uis== null || bAsync== null || bFrontQueue== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			{//"catch temp"
				if(ms_UInt16_Catch_1 == null) ms_UInt16_Catch_1= new System.Collections.Generic.List<System.UInt16>();
				for(int i = 0; i < uis.GetList().Count; ++i)
				{
					if(uis.mValue[i]!=null)
						ms_UInt16_Catch_1.Add( (System.UInt16)uis.mValue[i]);
				}
			}
			pPointer.PreSpawnUI(ms_UInt16_Catch_1,bAsync.mValue,bFrontQueue.mValue);
			if(ms_UInt16_Catch_1!=null) ms_UInt16_Catch_1.Clear();
			return true;
		}
		private static System.Collections.Generic.List<System.UInt16> ms_UInt16_Catch_0;
		private static System.Collections.Generic.List<System.UInt16> ms_UInt16_Catch_1;
		private static System.Collections.Generic.List<UnityEngine.GameObject> ms_GameObject_Catch_0;
		private static System.Collections.Generic.List<UnityEngine.GameObject> ms_GameObject_Catch_1;
		public static bool DoAction(VariableUser pUserClass, AgentTreeTask pTask, ActionNode pAction, int functionId = 0)
		{
			int funcId = (functionId!=0)?functionId:(int)pAction.GetExcudeHash();
			switch(funcId)
			{
				case 1670306757:
				{//TopGame.UI.UIManager->Clear
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.GameInstance.getInstance().uiManager;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(!(pUserClass.mValue is TopGame.UI.UIManager)) return true;
					return AgentTree_UIManager.AT_Clear(pUserClass.mValue as TopGame.UI.UIManager);
				}
				case 1547098398:
				{//TopGame.UI.UIManager->GetPhoneType
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.GameInstance.getInstance().uiManager;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.UI.UIManager)) return true;
					return AgentTree_UIManager.AT_GetPhoneType(pUserClass.mValue as TopGame.UI.UIManager, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableString>(0, pTask));
				}
				case -754769662:
				{//TopGame.UI.UIManager->SetCanvasBorderOffset
					if(pAction.inArgvs.Length <=5) return true;
					return AgentTree_UIManager.AT_SetCanvasBorderOffset(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(1, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(2, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(3, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(4, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(5, pTask));
				}
				case 1048712169:
				{//TopGame.UI.UIManager->GetRoot
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.GameInstance.getInstance().uiManager;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.UI.UIManager)) return true;
					return AgentTree_UIManager.AT_GetRoot(pUserClass.mValue as TopGame.UI.UIManager, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableObject>(0, pTask));
				}
				case 2006159075:
				{//TopGame.UI.UIManager->GetRoot3D
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.GameInstance.getInstance().uiManager;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.UI.UIManager)) return true;
					return AgentTree_UIManager.AT_GetRoot3D(pUserClass.mValue as TopGame.UI.UIManager, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableObject>(0, pTask));
				}
				case 1009572667:
				{//TopGame.UI.UIManager->IsWinShow
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.GameInstance.getInstance().uiManager;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.UI.UIManager)) return true;
					return AgentTree_UIManager.AT_IsWinShow(pUserClass.mValue as TopGame.UI.UIManager, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableBool>(0, pTask));
				}
				case 1587046863:
				{//TopGame.UI.UIManager->ShowAll
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.GameInstance.getInstance().uiManager;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(!(pUserClass.mValue is TopGame.UI.UIManager)) return true;
					return AgentTree_UIManager.AT_ShowAll(pUserClass.mValue as TopGame.UI.UIManager);
				}
				case -2073083593:
				{//TopGame.UI.UIManager->ShowUI
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.GameInstance.getInstance().uiManager;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.UI.UIManager)) return true;
					return AgentTree_UIManager.AT_ShowUI(pUserClass.mValue as TopGame.UI.UIManager, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableUser>(0, pTask));
				}
				case -1550238933:
				{//TopGame.UI.UIManager->HideUI
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.GameInstance.getInstance().uiManager;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is TopGame.UI.UIManager)) return true;
					return AgentTree_UIManager.AT_HideUI(pUserClass.mValue as TopGame.UI.UIManager, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask));
				}
				case -1739952387:
				{//TopGame.UI.UIManager->HideAll
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.GameInstance.getInstance().uiManager;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(!(pUserClass.mValue is TopGame.UI.UIManager)) return true;
					return AgentTree_UIManager.AT_HideAll(pUserClass.mValue as TopGame.UI.UIManager);
				}
				case 389852323:
				{//TopGame.UI.UIManager->MoveOutside
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.GameInstance.getInstance().uiManager;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is TopGame.UI.UIManager)) return true;
					return AgentTree_UIManager.AT_MoveOutside(pUserClass.mValue as TopGame.UI.UIManager, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask));
				}
				case 1680738204:
				{//TopGame.UI.UIManager->MoveInside
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.GameInstance.getInstance().uiManager;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is TopGame.UI.UIManager)) return true;
					return AgentTree_UIManager.AT_MoveInside(pUserClass.mValue as TopGame.UI.UIManager, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask));
				}
				case 493131080:
				{//TopGame.UI.UIManager->CloseUI
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.GameInstance.getInstance().uiManager;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is TopGame.UI.UIManager)) return true;
					return AgentTree_UIManager.AT_CloseUI(pUserClass.mValue as TopGame.UI.UIManager, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask));
				}
				case 589444821:
				{//TopGame.UI.UIManager->CloseAll
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.GameInstance.getInstance().uiManager;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(!(pUserClass.mValue is TopGame.UI.UIManager)) return true;
					return AgentTree_UIManager.AT_CloseAll(pUserClass.mValue as TopGame.UI.UIManager);
				}
				case 1167764636:
				{//TopGame.UI.UIManager->GetUI
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.GameInstance.getInstance().uiManager;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=2) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.UI.UIManager)) return true;
					return AgentTree_UIManager.AT_GetUI(pUserClass.mValue as TopGame.UI.UIManager, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(2, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableUser>(0, pTask));
				}
				case 207487781:
				{//TopGame.UI.UIManager->CreateUI
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.GameInstance.getInstance().uiManager;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=2) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.UI.UIManager)) return true;
					return AgentTree_UIManager.AT_CreateUI(pUserClass.mValue as TopGame.UI.UIManager, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(2, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableUser>(0, pTask));
				}
				case 1731505729:
				{//TopGame.UI.UIManager->ConvertUIPosToScreen
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.GameInstance.getInstance().uiManager;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.UI.UIManager)) return true;
					return AgentTree_UIManager.AT_ConvertUIPosToScreen(pUserClass.mValue as TopGame.UI.UIManager, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableVector3>(1, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableVector3>(0, pTask));
				}
				case -102619088:
				{//TopGame.UI.UIManager->ConvertScreenToUIPos
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.GameInstance.getInstance().uiManager;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=3) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.UI.UIManager)) return true;
					return AgentTree_UIManager.AT_ConvertScreenToUIPos(pUserClass.mValue as TopGame.UI.UIManager, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableVector3>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableVector3>(3, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableBool>(0, pTask));
				}
				case -790294378:
				{//TopGame.UI.UIManager->ConvertWorldPosToUIPos
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.GameInstance.getInstance().uiManager;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=4) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.UI.UIManager)) return true;
					return AgentTree_UIManager.AT_ConvertWorldPosToUIPos(pUserClass.mValue as TopGame.UI.UIManager, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableVector3>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableVector3>(3, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableMonoScript>(4, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableBool>(0, pTask));
				}
				case -336781782:
				{//TopGame.UI.UIManager->UGUIPosToWorldPos
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.GameInstance.getInstance().uiManager;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=3) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.UI.UIManager)) return true;
					return AgentTree_UIManager.AT_UGUIPosToWorldPos(pUserClass.mValue as TopGame.UI.UIManager, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableMonoScript>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableVector3>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(3, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableVector3>(0, pTask));
				}
				case -2086069240:
				{//TopGame.UI.UIManager->SequenceOpenUI
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.GameInstance.getInstance().uiManager;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is TopGame.UI.UIManager)) return true;
					return AgentTree_UIManager.AT_SequenceOpenUI(pUserClass.mValue as TopGame.UI.UIManager, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableIntList>(1, pTask));
				}
				case -716547496:
				{//TopGame.UI.UIManager->HideGameObject
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.GameInstance.getInstance().uiManager;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is TopGame.UI.UIManager)) return true;
					return AgentTree_UIManager.AT_HideGameObject(pUserClass.mValue as TopGame.UI.UIManager, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableObject>(1, pTask));
				}
				case -1746047024:
				{//TopGame.UI.UIManager->ShowGameObject
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.GameInstance.getInstance().uiManager;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is TopGame.UI.UIManager)) return true;
					return AgentTree_UIManager.AT_ShowGameObject(pUserClass.mValue as TopGame.UI.UIManager, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableObject>(1, pTask));
				}
				case -1807452822:
				{//TopGame.UI.UIManager->ShowGameObjects
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.GameInstance.getInstance().uiManager;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is TopGame.UI.UIManager)) return true;
					return AgentTree_UIManager.AT_ShowGameObjects(pUserClass.mValue as TopGame.UI.UIManager, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableObjectList>(1, pTask));
				}
				case 1511308907:
				{//TopGame.UI.UIManager->HideGameObjects
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.GameInstance.getInstance().uiManager;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is TopGame.UI.UIManager)) return true;
					return AgentTree_UIManager.AT_HideGameObjects(pUserClass.mValue as TopGame.UI.UIManager, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableObjectList>(1, pTask));
				}
				case -2014383755:
				{//TopGame.UI.UIManager->IsGameObjectShow
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.GameInstance.getInstance().uiManager;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.UI.UIManager)) return true;
					return AgentTree_UIManager.AT_IsGameObjectShow(pUserClass.mValue as TopGame.UI.UIManager, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableObject>(1, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableBool>(0, pTask));
				}
				case -1884369391:
				{//TopGame.UI.UIManager->GetUIAssetFile
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.GameInstance.getInstance().uiManager;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.UI.UIManager)) return true;
					return AgentTree_UIManager.AT_GetUIAssetFile(pUserClass.mValue as TopGame.UI.UIManager, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableString>(0, pTask));
				}
				case -410715753:
				{//TopGame.UI.UIManager->PreSpawnUI
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.GameInstance.getInstance().uiManager;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=3) return true;
					if(!(pUserClass.mValue is TopGame.UI.UIManager)) return true;
					return AgentTree_UIManager.AT_PreSpawnUI(pUserClass.mValue as TopGame.UI.UIManager, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableIntList>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(3, pTask));
				}
			}
			return true;
		}
	}
}
