//auto genreator
namespace Framework.Plugin.AT
{
#if UNITY_EDITOR
	[ATExport("网络模块",true)]
#endif
	public static class AgentTree_NetWork
	{
#if UNITY_EDITOR
		[ATMonoFunc(1169249629,"Connect",typeof(TopGame.Net.NetWork))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.Net.NetWork),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableInt),"type",false, null,typeof(TopGame.Net.ESessionType))]
		[ATMethodArgv(typeof(VariableString),"strIp",false, null,null)]
		[ATMethodArgv(typeof(VariableInt),"nPort",false, null,null)]
		[ATMethodArgv(typeof(VariableDelegate),"onCallback",false, null,null)]
		[ATMethodArgv(typeof(VariableLong),"localConnID",false, null,null)]
		[ATMethodReturn(typeof(VariableUser),"pReturn",typeof(TopGame.Net.AServerSession),null)]
#endif
		public static bool AT_Connect(TopGame.Net.NetWork pPointer, VariableInt type, VariableString strIp, VariableInt nPort, VariableDelegate onCallback, VariableLong localConnID, VariableUser pReturn=null)
		{
			if(type== null || strIp== null || nPort== null || onCallback== null || localConnID== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.Connect((TopGame.Net.ESessionType)type.mValue,strIp.mValue,nPort.mValue,(AServerSessionVar)=>{
				onCallback.mValue=AServerSessionVar;
				onCallback.DoCall();},localConnID.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-77830448,"ConnectAddress",typeof(TopGame.Net.NetWork))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.Net.NetWork),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableInt),"type",false, null,typeof(TopGame.Net.ESessionType))]
		[ATMethodArgv(typeof(VariableString),"strAddress",false, null,null)]
		[ATMethodArgv(typeof(VariableDelegate),"onCallback",false, null,null)]
		[ATMethodArgv(typeof(VariableLong),"localConnID",false, null,null)]
		[ATMethodReturn(typeof(VariableUser),"pReturn",typeof(TopGame.Net.AServerSession),null)]
#endif
		public static bool AT_Connect_1(TopGame.Net.NetWork pPointer, VariableInt type, VariableString strAddress, VariableDelegate onCallback, VariableLong localConnID, VariableUser pReturn=null)
		{
			if(type== null || strAddress== null || onCallback== null || localConnID== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.Connect((TopGame.Net.ESessionType)type.mValue,strAddress.mValue,(AServerSessionVar)=>{
				onCallback.mValue=AServerSessionVar;
				onCallback.DoCall();},localConnID.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(159717930,"发送消息包",typeof(TopGame.Net.NetWork))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.Net.NetWork),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableUser),"pMsg",false, typeof(TopGame.Net.PacketBase),null)]
		[ATMethodReturn(typeof(VariableBool),"pReturn",null,null)]
#endif
		public static bool AT_SendMessage(TopGame.Net.NetWork pPointer, VariableUser pMsg, VariableBool pReturn=null)
		{
			if(pMsg== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = pPointer.SendMessage((TopGame.Net.PacketBase)pMsg.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-845519087,"发送心跳包",typeof(TopGame.Net.NetWork))]
		[ATMethodArgv(typeof(VariableUser),"pPointer",false, typeof(TopGame.Net.NetWork),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableFloat),"fHeartGap",false, null,null)]
#endif
		public static bool AT_SendHeart(TopGame.Net.NetWork pPointer, VariableFloat fHeartGap)
		{
			if(fHeartGap== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			pPointer.SendHeart(fHeartGap.mValue);
			return true;
		}
		public static bool DoAction(VariableUser pUserClass, AgentTreeTask pTask, ActionNode pAction, int functionId = 0)
		{
			int funcId = (functionId!=0)?functionId:(int)pAction.GetExcudeHash();
			switch(funcId)
			{
				case 1169249629:
				{//TopGame.Net.NetWork->Connect
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.Net.NetWork.getInstance();
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=5) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.Net.NetWork)) return true;
					return AgentTree_NetWork.AT_Connect(pUserClass.mValue as TopGame.Net.NetWork, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(3, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableDelegate>(4, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableLong>(5, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableUser>(0, pTask));
				}
				case -77830448:
				{//TopGame.Net.NetWork->Connect_1
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.Net.NetWork.getInstance();
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=4) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.Net.NetWork)) return true;
					return AgentTree_NetWork.AT_Connect_1(pUserClass.mValue as TopGame.Net.NetWork, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(2, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableDelegate>(3, pTask), pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableLong>(4, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableUser>(0, pTask));
				}
				case 159717930:
				{//TopGame.Net.NetWork->SendMessage
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.Net.NetWork.getInstance();
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					if(!(pUserClass.mValue is TopGame.Net.NetWork)) return true;
					return AgentTree_NetWork.AT_SendMessage(pUserClass.mValue as TopGame.Net.NetWork, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableUser>(1, pTask), pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableBool>(0, pTask));
				}
				case -845519087:
				{//TopGame.Net.NetWork->SendHeart
					if(pUserClass == null) return true;
					if(pUserClass.mValue == null)
						pUserClass.mValue = TopGame.Net.NetWork.getInstance();
					if(pUserClass.mValue == null)
						pUserClass.mValue = AgentTreeManager.getInstance().FindUserClass(pUserClass.hashCode, pTask.pAT);
					if(pUserClass.mValue == null) return true;
					if(pAction.inArgvs.Length <=1) return true;
					if(!(pUserClass.mValue is TopGame.Net.NetWork)) return true;
					return AgentTree_NetWork.AT_SendHeart(pUserClass.mValue as TopGame.Net.NetWork, pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(1, pTask));
				}
			}
			return true;
		}
	}
}
