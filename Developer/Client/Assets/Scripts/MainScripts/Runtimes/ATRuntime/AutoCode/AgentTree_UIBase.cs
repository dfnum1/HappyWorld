//auto genreator
namespace Framework.Plugin.AT
{
#if UNITY_EDITOR
	[ATExport("UI系统/界面",true)]
#endif
	public static class AgentTree_UIBase
	{
#if UNITY_EDITOR
		[ATMonoFunc(-965493104,"界面类型ID",typeof(TopGame.UI.UIBase))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.UI.UIBase),null)]
		[ATMethodReturn(typeof(VariableInt),"pReturn",null,null)]
#endif
		public static bool AT_GetUIType(TopGame.UI.UIBase pPointer, VariableInt pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = (int)pPointer.GetUIType();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-145863365,"层级",typeof(TopGame.UI.UIBase))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.UI.UIBase),null)]
		[ATMethodReturn(typeof(VariableInt),"pReturn",null,null)]
#endif
		public static bool AT_GetOrder(TopGame.UI.UIBase pPointer, VariableInt pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetOrder();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1158257097,"设置层级",typeof(TopGame.UI.UIBase))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.UI.UIBase),null)]
		[ATMethodArgv(typeof(VariableInt),"order",false, null,null)]
#endif
		public static bool AT_SetOrder(TopGame.UI.UIBase pPointer, VariableInt order)
		{
			if(order== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SetOrder(order.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(396527448,"设置Z轴深度",typeof(TopGame.UI.UIBase))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.UI.UIBase),null)]
		[ATMethodArgv(typeof(VariableInt),"zValue",false, null,null)]
#endif
		public static bool AT_SetZDeepth(TopGame.UI.UIBase pPointer, VariableInt zValue)
		{
			if(zValue== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SetZDeepth(zValue.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(116475848,"设置为常驻",typeof(TopGame.UI.UIBase))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.UI.UIBase),null)]
		[ATMethodArgv(typeof(VariableBool),"bPermanent",false, null,null)]
#endif
		public static bool AT_SetPermanent(TopGame.UI.UIBase pPointer, VariableBool bPermanent)
		{
			if(bPermanent== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SetPermanent(bPermanent.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1102805964,"是否常驻",typeof(TopGame.UI.UIBase))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.UI.UIBase),null)]
		[ATMethodReturn(typeof(VariableBool),"pReturn",null,null)]
#endif
		public static bool AT_GetPermanent(TopGame.UI.UIBase pPointer, VariableBool pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetPermanent();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(710334278,"获取UI逻辑脚本",typeof(TopGame.UI.UIBase))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.UI.UIBase),null)]
		[ATMethodArgv(typeof(VariableInt),"type",false, typeof(System.Type),null)]
		[ATMethodReturn(typeof(VariableUser),"pReturn",typeof(TopGame.UI.UILogic),null)]
#endif
		public static bool AT_FindLogic(TopGame.UI.UIBase pPointer, VariableInt type, VariableUser pReturn=null)
		{
			if(type== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.FindLogic((System.Type)AgentTree_ClassTypes.HashToClassType(type.mValue));
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(70879939,"移除UI逻辑脚本",typeof(TopGame.UI.UIBase))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.UI.UIBase),null)]
		[ATMethodArgv(typeof(VariableUser),"pLogic",false, typeof(TopGame.UI.UILogic),null)]
		[ATMethodArgv(typeof(VariableInt),"hashCode",false, null,null)]
#endif
		public static bool AT_RemoveLogic(TopGame.UI.UIBase pPointer, VariableUser pLogic, VariableInt hashCode)
		{
			if(pLogic== null || hashCode== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.RemoveLogic((TopGame.UI.UILogic)pLogic.mValue,hashCode.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(74244052,"显示",typeof(TopGame.UI.UIBase))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.UI.UIBase),null)]
#endif
		public static bool AT_Show(TopGame.UI.UIBase pPointer)
		{
			pPointer.Show();
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1021039180,"关闭",typeof(TopGame.UI.UIBase))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.UI.UIBase),null)]
		[ATMethodReturn(typeof(VariableBool),"pReturn",null,null)]
#endif
		public static bool AT_Close(TopGame.UI.UIBase pPointer, VariableBool pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.Close();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1949292123,"移动屏幕外",typeof(TopGame.UI.UIBase))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.UI.UIBase),null)]
#endif
		public static bool AT_MoveOutside(TopGame.UI.UIBase pPointer)
		{
			pPointer.MoveOutside();
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-787964043,"移进屏幕内",typeof(TopGame.UI.UIBase))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.UI.UIBase),null)]
#endif
		public static bool AT_MoveInside(TopGame.UI.UIBase pPointer)
		{
			pPointer.MoveInside();
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1437928308,"隐藏",typeof(TopGame.UI.UIBase))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.UI.UIBase),null)]
#endif
		public static bool AT_Hide(TopGame.UI.UIBase pPointer)
		{
			pPointer.Hide();
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1613203700,"是否显示",typeof(TopGame.UI.UIBase))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.UI.UIBase),null)]
		[ATMethodReturn(typeof(VariableBool),"pReturn",null,null)]
#endif
		public static bool AT_IsVisible(TopGame.UI.UIBase pPointer, VariableBool pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.IsVisible();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(920957374,"是否可以中断引导",typeof(TopGame.UI.UIBase))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.UI.UIBase),null)]
		[ATMethodReturn(typeof(VariableBool),"pReturn",null,null)]
#endif
		public static bool AT_CanBreakGuide(TopGame.UI.UIBase pPointer, VariableBool pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.CanBreakGuide();
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1557574079,"获取一个对象",typeof(TopGame.UI.UIBase))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.UI.UIBase),null)]
		[ATMethodArgv(typeof(VariableString),"strName",false, null,null)]
		[ATMethodReturn(typeof(VariableObject),"pReturn",typeof(UnityEngine.GameObject),null)]
#endif
		public static bool AT_FindOject(TopGame.UI.UIBase pPointer, VariableString strName, VariableObject pReturn=null)
		{
			if(strName== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.FindOject(strName.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(2093585483,"清理",typeof(TopGame.UI.UIBase))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.UI.UIBase),null)]
#endif
		public static bool AT_Clear(TopGame.UI.UIBase pPointer)
		{
			pPointer.Clear();
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-689720064,"资源加载设置",typeof(TopGame.UI.UIBase))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.UI.UIBase),null)]
		[ATMethodArgv(typeof(VariableObject),"pObj",false, null,null)]
		[ATMethodArgv(typeof(VariableString),"strPath",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bPermanent",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bAysnc",false, null,null)]
		[ATMethodArgv(typeof(VariableObject),"defaultSprite",false, typeof(UnityEngine.Sprite),null)]
		[ATMethodReturn(typeof(VariableUser),"pReturn",typeof(Framework.Core.AssetOperiaon),null)]
#endif
		public static bool AT_LoadObjectAsset(TopGame.UI.UIBase pPointer, VariableObject pObj, VariableString strPath, VariableBool bPermanent, VariableBool bAysnc, VariableObject defaultSprite, VariableUser pReturn=null)
		{
			if(pObj== null || strPath== null || bPermanent== null || bAysnc== null || defaultSprite== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.LoadObjectAsset(pObj.mValue,strPath.mValue,bPermanent.mValue,bAysnc.mValue,defaultSprite.ToObject<UnityEngine.Sprite>());
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1292562843,"AddUserParam",typeof(TopGame.UI.UIBase))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.UI.UIBase),null)]
		[ATMethodArgv(typeof(VariableString),"key",false, null,null)]
		[ATMethodArgv(typeof(VariableUser),"param",false, typeof(Framework.Core.VariablePoolAble),null)]
#endif
		public static bool AT_AddUserParam(TopGame.UI.UIBase pPointer, VariableString key, VariableUser param)
		{
			if(key== null || param== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.AddUserParam(key.mValue,(Framework.Core.VariablePoolAble)param.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(255485600,"AddUserIntParam",typeof(TopGame.UI.UIBase))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.UI.UIBase),null)]
		[ATMethodArgv(typeof(VariableString),"key",false, null,null)]
		[ATMethodArgv(typeof(VariableInt),"param",false, null,null)]
#endif
		public static bool AT_AddUserIntParam(TopGame.UI.UIBase pPointer, VariableString key, VariableInt param)
		{
			if(key== null || param== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.AddUserIntParam(key.mValue,param.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1605617938,"AddUserStrParam",typeof(TopGame.UI.UIBase))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.UI.UIBase),null)]
		[ATMethodArgv(typeof(VariableString),"key",false, null,null)]
		[ATMethodArgv(typeof(VariableString),"param",false, null,null)]
#endif
		public static bool AT_AddUserStrParam(TopGame.UI.UIBase pPointer, VariableString key, VariableString param)
		{
			if(key== null || param== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.AddUserStrParam(key.mValue,param.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1704230650,"GetUserParam",typeof(TopGame.UI.UIBase))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.UI.UIBase),null)]
		[ATMethodArgv(typeof(VariableString),"key",false, null,null)]
		[ATMethodReturn(typeof(VariableUser),"pReturn",typeof(Framework.Core.VariablePoolAble),null)]
#endif
		public static bool AT_GetUserParam(TopGame.UI.UIBase pPointer, VariableString key, VariableUser pReturn=null)
		{
			if(key== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetUserParam(key.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1403380325,"GetUserIntParam",typeof(TopGame.UI.UIBase))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.UI.UIBase),null)]
		[ATMethodArgv(typeof(VariableString),"key",false, null,null)]
		[ATMethodReturn(typeof(VariableInt),"pReturn",null,null)]
#endif
		public static bool AT_GetUserIntParam(TopGame.UI.UIBase pPointer, VariableString key, VariableInt pReturn=null)
		{
			if(key== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetUserIntParam(key.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(472602996,"GetUserStrParam",typeof(TopGame.UI.UIBase))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.UI.UIBase),null)]
		[ATMethodArgv(typeof(VariableString),"key",false, null,null)]
		[ATMethodReturn(typeof(VariableString),"pReturn",null,null)]
#endif
		public static bool AT_GetUserStrParam(TopGame.UI.UIBase pPointer, VariableString key, VariableString pReturn=null)
		{
			if(key== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.GetUserStrParam(key.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1299280658,"get_view",typeof(TopGame.UI.UIBase), true)]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false,  typeof(TopGame.UI.UIBase),null)]
		[ATMethodReturn(typeof(VariableUser),"pReturn",typeof(TopGame.UI.UIView),null)]
#endif
		public static bool AT_get_view(TopGame.UI.UIBase pPointer, VariableUser pReturn=null)
		{
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.view;
			}	
			return true;
		}
		public static bool DoAction(VariableUser pUserClass, AgentTreeTask pTask, ActionNode pAction, int functionId = 0)
		{
			int funcId = (functionId!=0)?functionId:(int)pAction.GetExcudeHash();
			switch(funcId)
			{
				case -965493104:
				{//TopGame.UI.UIBase->GetUIType
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.UI.UIBase)) return true;
					return AgentTree_UIBase.AT_GetUIType(pUserClass.mValue as TopGame.UI.UIBase, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableInt>(0, pTask));
				}
				case -145863365:
				{//TopGame.UI.UIBase->GetOrder
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.UI.UIBase)) return true;
					return AgentTree_UIBase.AT_GetOrder(pUserClass.mValue as TopGame.UI.UIBase, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableInt>(0, pTask));
				}
				case 1158257097:
				{//TopGame.UI.UIBase->SetOrder
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is TopGame.UI.UIBase)) return true;
					return AgentTree_UIBase.AT_SetOrder(pUserClass.mValue as TopGame.UI.UIBase, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask));
				}
				case 396527448:
				{//TopGame.UI.UIBase->SetZDeepth
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is TopGame.UI.UIBase)) return true;
					return AgentTree_UIBase.AT_SetZDeepth(pUserClass.mValue as TopGame.UI.UIBase, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask));
				}
				case 116475848:
				{//TopGame.UI.UIBase->SetPermanent
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is TopGame.UI.UIBase)) return true;
					return AgentTree_UIBase.AT_SetPermanent(pUserClass.mValue as TopGame.UI.UIBase, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(1, pTask));
				}
				case 1102805964:
				{//TopGame.UI.UIBase->GetPermanent
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.UI.UIBase)) return true;
					return AgentTree_UIBase.AT_GetPermanent(pUserClass.mValue as TopGame.UI.UIBase, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableBool>(0, pTask));
				}
				case 710334278:
				{//TopGame.UI.UIBase->FindLogic
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.UI.UIBase)) return true;
					return AgentTree_UIBase.AT_FindLogic(pUserClass.mValue as TopGame.UI.UIBase, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableUser>(0, pTask));
				}
				case 70879939:
				{//TopGame.UI.UIBase->RemoveLogic
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=2) return true;
					if(!(pUserClass.mValue is TopGame.UI.UIBase)) return true;
					return AgentTree_UIBase.AT_RemoveLogic(pUserClass.mValue as TopGame.UI.UIBase, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableUser>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(2, pTask));
				}
				case 74244052:
				{//TopGame.UI.UIBase->Show
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(!(pUserClass.mValue is TopGame.UI.UIBase)) return true;
					return AgentTree_UIBase.AT_Show(pUserClass.mValue as TopGame.UI.UIBase);
				}
				case 1021039180:
				{//TopGame.UI.UIBase->Close
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.UI.UIBase)) return true;
					return AgentTree_UIBase.AT_Close(pUserClass.mValue as TopGame.UI.UIBase, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableBool>(0, pTask));
				}
				case -1949292123:
				{//TopGame.UI.UIBase->MoveOutside
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(!(pUserClass.mValue is TopGame.UI.UIBase)) return true;
					return AgentTree_UIBase.AT_MoveOutside(pUserClass.mValue as TopGame.UI.UIBase);
				}
				case -787964043:
				{//TopGame.UI.UIBase->MoveInside
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(!(pUserClass.mValue is TopGame.UI.UIBase)) return true;
					return AgentTree_UIBase.AT_MoveInside(pUserClass.mValue as TopGame.UI.UIBase);
				}
				case -1437928308:
				{//TopGame.UI.UIBase->Hide
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(!(pUserClass.mValue is TopGame.UI.UIBase)) return true;
					return AgentTree_UIBase.AT_Hide(pUserClass.mValue as TopGame.UI.UIBase);
				}
				case -1613203700:
				{//TopGame.UI.UIBase->IsVisible
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.UI.UIBase)) return true;
					return AgentTree_UIBase.AT_IsVisible(pUserClass.mValue as TopGame.UI.UIBase, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableBool>(0, pTask));
				}
				case 920957374:
				{//TopGame.UI.UIBase->CanBreakGuide
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.UI.UIBase)) return true;
					return AgentTree_UIBase.AT_CanBreakGuide(pUserClass.mValue as TopGame.UI.UIBase, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableBool>(0, pTask));
				}
				case 1557574079:
				{//TopGame.UI.UIBase->FindOject
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.UI.UIBase)) return true;
					return AgentTree_UIBase.AT_FindOject(pUserClass.mValue as TopGame.UI.UIBase, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableObject>(0, pTask));
				}
				case 2093585483:
				{//TopGame.UI.UIBase->Clear
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(!(pUserClass.mValue is TopGame.UI.UIBase)) return true;
					return AgentTree_UIBase.AT_Clear(pUserClass.mValue as TopGame.UI.UIBase);
				}
				case -689720064:
				{//TopGame.UI.UIBase->LoadObjectAsset
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=5) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.UI.UIBase)) return true;
					return AgentTree_UIBase.AT_LoadObjectAsset(pUserClass.mValue as TopGame.UI.UIBase, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableObject>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(3, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(4, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableObject>(5, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableUser>(0, pTask));
				}
				case -1292562843:
				{//TopGame.UI.UIBase->AddUserParam
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=2) return true;
					if(!(pUserClass.mValue is TopGame.UI.UIBase)) return true;
					return AgentTree_UIBase.AT_AddUserParam(pUserClass.mValue as TopGame.UI.UIBase, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableUser>(2, pTask));
				}
				case 255485600:
				{//TopGame.UI.UIBase->AddUserIntParam
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=2) return true;
					if(!(pUserClass.mValue is TopGame.UI.UIBase)) return true;
					return AgentTree_UIBase.AT_AddUserIntParam(pUserClass.mValue as TopGame.UI.UIBase, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(2, pTask));
				}
				case -1605617938:
				{//TopGame.UI.UIBase->AddUserStrParam
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=2) return true;
					if(!(pUserClass.mValue is TopGame.UI.UIBase)) return true;
					return AgentTree_UIBase.AT_AddUserStrParam(pUserClass.mValue as TopGame.UI.UIBase, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(2, pTask));
				}
				case -1704230650:
				{//TopGame.UI.UIBase->GetUserParam
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.UI.UIBase)) return true;
					return AgentTree_UIBase.AT_GetUserParam(pUserClass.mValue as TopGame.UI.UIBase, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableUser>(0, pTask));
				}
				case -1403380325:
				{//TopGame.UI.UIBase->GetUserIntParam
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.UI.UIBase)) return true;
					return AgentTree_UIBase.AT_GetUserIntParam(pUserClass.mValue as TopGame.UI.UIBase, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableInt>(0, pTask));
				}
				case 472602996:
				{//TopGame.UI.UIBase->GetUserStrParam
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.UI.UIBase)) return true;
					return AgentTree_UIBase.AT_GetUserStrParam(pUserClass.mValue as TopGame.UI.UIBase, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableString>(0, pTask));
				}
				case -1299280658:
				{//TopGame.UI.UIBase->view
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.UI.UIBase)) return true;
					return AgentTree_UIBase.AT_get_view(pUserClass.mValue as TopGame.UI.UIBase, pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableUser>(0, pTask));
				}
			}
			return true;
		}
	}
}
