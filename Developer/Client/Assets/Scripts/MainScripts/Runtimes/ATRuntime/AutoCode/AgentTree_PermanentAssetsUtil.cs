//auto genreator
namespace Framework.Plugin.AT
{
#if UNITY_EDITOR
	[ATExport("常驻资源",true)]
#endif
	public static class AgentTree_PermanentAssetsUtil
	{
#if UNITY_EDITOR
		[ATMonoFunc(871160379,"获取资源",typeof(TopGame.Data.PermanentAssetsUtil))]
		[ATMethodArgv(typeof(VariableInt),"ClassHashCode",false, typeof(TopGame.Data.PermanentAssetsUtil),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableInt),"type",false, null,typeof(TopGame.Data.EPermanentAssetType))]
		[ATMethodReturn(typeof(VariableObject),"pReturn",null,null)]
#endif
		public static bool AT_GetAsset(VariableInt type,VariableObject pReturn=null)
		{
			if(type== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = TopGame.Data.PermanentAssetsUtil.GetAsset((TopGame.Data.EPermanentAssetType)type.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-561002723,"获取元素效果材质",typeof(TopGame.Data.PermanentAssetsUtil))]
		[ATMethodArgv(typeof(VariableInt),"ClassHashCode",false, typeof(TopGame.Data.PermanentAssetsUtil),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableInt),"type",false, null,typeof(Framework.Core.EElementType))]
		[ATMethodReturn(typeof(VariableObject),"pReturn",typeof(UnityEngine.Material),null)]
#endif
		public static bool AT_GetElementEffectMaterial(VariableInt type,VariableObject pReturn=null)
		{
			if(type== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = TopGame.Data.PermanentAssetsUtil.GetElementEffectMaterial((Framework.Core.EElementType)type.mValue);
			}	
			return true;
		}
		public static bool DoAction(VariableUser pUserClass, AgentTreeTask pTask, ActionNode pAction, int functionId = 0)
		{
			int funcId = (functionId!=0)?functionId:(int)pAction.GetExcudeHash();
			switch(funcId)
			{
				case 871160379:
				{//TopGame.Data.PermanentAssetsUtil->GetAsset
					if(pAction.inArgvs.Length <=1) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					return AgentTree_PermanentAssetsUtil.AT_GetAsset(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask),pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableObject>(0, pTask));
				}
				case -561002723:
				{//TopGame.Data.PermanentAssetsUtil->GetElementEffectMaterial
					if(pAction.inArgvs.Length <=1) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					return AgentTree_PermanentAssetsUtil.AT_GetElementEffectMaterial(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask),pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableObject>(0, pTask));
				}
			}
			return true;
		}
	}
}
