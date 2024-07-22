//auto genreator
namespace Framework.Plugin.AT
{
#if UNITY_EDITOR
	[ATExport("渲染/材质属性",true)]
#endif
	public static class AgentTree_MaterailBlockUtil
	{
#if UNITY_EDITOR
		[ATMonoFunc(-1902165494,"设置全局浮点",typeof(Framework.Core.MaterailBlockUtil))]
		[ATMethodArgv(typeof(VariableInt),"ClassHashCode",false, typeof(Framework.Core.MaterailBlockUtil),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableString),"propertyName",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"fValue",false, null,null)]
#endif
		public static bool AT_SetGlobalFloat(VariableString propertyName,VariableFloat fValue)
		{
			if(propertyName== null || fValue== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			Framework.Core.MaterailBlockUtil.SetGlobalFloat(propertyName.mValue,fValue.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1456886206,"设置浮点数-Go",typeof(Framework.Core.MaterailBlockUtil))]
		[ATMethodArgv(typeof(VariableInt),"ClassHashCode",false, typeof(Framework.Core.MaterailBlockUtil),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableObject),"go",false, typeof(UnityEngine.GameObject),null)]
		[ATMethodArgv(typeof(VariableString),"propertyName",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"param",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bBlock",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bShare",false, null,null)]
		[ATMethodArgv(typeof(VariableInt),"index",false, null,null)]
#endif
		public static bool AT_SetGoFloat(VariableObject go,VariableString propertyName,VariableFloat param,VariableBool bBlock,VariableBool bShare,VariableInt index)
		{
			if(go== null || propertyName== null || param== null || bBlock== null || bShare== null || index== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			Framework.Core.MaterailBlockUtil.SetGoFloat(go.ToObject<UnityEngine.GameObject>(),propertyName.mValue,param.mValue,bBlock.mValue,bShare.mValue,index.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1580948318,"设置浮点数-Transform",typeof(Framework.Core.MaterailBlockUtil))]
		[ATMethodArgv(typeof(VariableInt),"ClassHashCode",false, typeof(Framework.Core.MaterailBlockUtil),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableObject),"trans",false, typeof(UnityEngine.Transform),null)]
		[ATMethodArgv(typeof(VariableString),"propertyName",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"param",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bBlock",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bShare",false, null,null)]
		[ATMethodArgv(typeof(VariableInt),"index",false, null,null)]
#endif
		public static bool AT_SetTransformFloat(VariableObject trans,VariableString propertyName,VariableFloat param,VariableBool bBlock,VariableBool bShare,VariableInt index)
		{
			if(trans== null || propertyName== null || param== null || bBlock== null || bShare== null || index== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			Framework.Core.MaterailBlockUtil.SetTransformFloat(trans.ToObject<UnityEngine.Transform>(),propertyName.mValue,param.mValue,bBlock.mValue,bShare.mValue,index.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1831898998,"设置浮点数-Renderer",typeof(Framework.Core.MaterailBlockUtil))]
		[ATMethodArgv(typeof(VariableInt),"ClassHashCode",false, typeof(Framework.Core.MaterailBlockUtil),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableObject),"render",false, typeof(UnityEngine.Renderer),null)]
		[ATMethodArgv(typeof(VariableString),"propertyName",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"param",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bBlock",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bShare",false, null,null)]
		[ATMethodArgv(typeof(VariableInt),"index",false, null,null)]
#endif
		public static bool AT_SetRenderFloat(VariableObject render,VariableString propertyName,VariableFloat param,VariableBool bBlock,VariableBool bShare,VariableInt index)
		{
			if(render== null || propertyName== null || param== null || bBlock== null || bShare== null || index== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			Framework.Core.MaterailBlockUtil.SetRenderFloat(render.ToObject<UnityEngine.Renderer>(),propertyName.mValue,param.mValue,bBlock.mValue,bShare.mValue,index.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(30421350,"设置浮点-Renders[]",typeof(Framework.Core.MaterailBlockUtil))]
		[ATMethodArgv(typeof(VariableInt),"ClassHashCode",false, typeof(Framework.Core.MaterailBlockUtil),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableObjectList),"renderers",false, null,null)]
		[ATMethodArgv(typeof(VariableString),"propertyName",false, null,null)]
		[ATMethodArgv(typeof(VariableFloat),"param",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bBlock",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bShare",false, null,null)]
		[ATMethodArgv(typeof(VariableInt),"index",false, null,null)]
#endif
		public static bool AT_SetRendersFloat(VariableObjectList renderers,VariableString propertyName,VariableFloat param,VariableBool bBlock,VariableBool bShare,VariableInt index)
		{
			if(renderers== null || propertyName== null || param== null || bBlock== null || bShare== null || index== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			{//"catch temp"
				if(ms_Renderer_Catch_0 == null) ms_Renderer_Catch_0= new System.Collections.Generic.List<UnityEngine.Renderer>();
				for(int i = 0; i < renderers.GetList().Count; ++i)
				{
					if(renderers.mValue[i]!=null)
						ms_Renderer_Catch_0.Add( (UnityEngine.Renderer)renderers.mValue[i]);
				}
			}
			Framework.Core.MaterailBlockUtil.SetRendersFloat(ms_Renderer_Catch_0,propertyName.mValue,param.mValue,bBlock.mValue,bShare.mValue,index.mValue);
			if(ms_Renderer_Catch_0!=null) ms_Renderer_Catch_0.Clear();
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1489213840,"设置全局Int",typeof(Framework.Core.MaterailBlockUtil))]
		[ATMethodArgv(typeof(VariableInt),"ClassHashCode",false, typeof(Framework.Core.MaterailBlockUtil),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableString),"propertyName",false, null,null)]
		[ATMethodArgv(typeof(VariableInt),"nValue",false, null,null)]
#endif
		public static bool AT_SetGlobalInt(VariableString propertyName,VariableInt nValue)
		{
			if(propertyName== null || nValue== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			Framework.Core.MaterailBlockUtil.SetGlobalInt(propertyName.mValue,nValue.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1117852636,"设置Int-Go",typeof(Framework.Core.MaterailBlockUtil))]
		[ATMethodArgv(typeof(VariableInt),"ClassHashCode",false, typeof(Framework.Core.MaterailBlockUtil),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableObject),"go",false, typeof(UnityEngine.GameObject),null)]
		[ATMethodArgv(typeof(VariableString),"propertyName",false, null,null)]
		[ATMethodArgv(typeof(VariableInt),"param",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bBlock",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bShare",false, null,null)]
		[ATMethodArgv(typeof(VariableInt),"index",false, null,null)]
#endif
		public static bool AT_SetGoInt(VariableObject go,VariableString propertyName,VariableInt param,VariableBool bBlock,VariableBool bShare,VariableInt index)
		{
			if(go== null || propertyName== null || param== null || bBlock== null || bShare== null || index== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			Framework.Core.MaterailBlockUtil.SetGoInt(go.ToObject<UnityEngine.GameObject>(),propertyName.mValue,param.mValue,bBlock.mValue,bShare.mValue,index.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-728132556,"设置Int-Go",typeof(Framework.Core.MaterailBlockUtil))]
		[ATMethodArgv(typeof(VariableInt),"ClassHashCode",false, typeof(Framework.Core.MaterailBlockUtil),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableObject),"trans",false, typeof(UnityEngine.Transform),null)]
		[ATMethodArgv(typeof(VariableString),"propertyName",false, null,null)]
		[ATMethodArgv(typeof(VariableInt),"param",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bBlock",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bShare",false, null,null)]
		[ATMethodArgv(typeof(VariableInt),"index",false, null,null)]
#endif
		public static bool AT_SetTransformInt(VariableObject trans,VariableString propertyName,VariableInt param,VariableBool bBlock,VariableBool bShare,VariableInt index)
		{
			if(trans== null || propertyName== null || param== null || bBlock== null || bShare== null || index== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			Framework.Core.MaterailBlockUtil.SetTransformInt(trans.ToObject<UnityEngine.Transform>(),propertyName.mValue,param.mValue,bBlock.mValue,bShare.mValue,index.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(730283371,"设置Int-Renderer",typeof(Framework.Core.MaterailBlockUtil))]
		[ATMethodArgv(typeof(VariableInt),"ClassHashCode",false, typeof(Framework.Core.MaterailBlockUtil),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableObject),"render",false, typeof(UnityEngine.Renderer),null)]
		[ATMethodArgv(typeof(VariableString),"propertyName",false, null,null)]
		[ATMethodArgv(typeof(VariableInt),"param",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bBlock",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bShare",false, null,null)]
		[ATMethodArgv(typeof(VariableInt),"index",false, null,null)]
#endif
		public static bool AT_SetRenderInt(VariableObject render,VariableString propertyName,VariableInt param,VariableBool bBlock,VariableBool bShare,VariableInt index)
		{
			if(render== null || propertyName== null || param== null || bBlock== null || bShare== null || index== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			Framework.Core.MaterailBlockUtil.SetRenderInt(render.ToObject<UnityEngine.Renderer>(),propertyName.mValue,param.mValue,bBlock.mValue,bShare.mValue,index.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(787976746,"设置Int-Renders[]",typeof(Framework.Core.MaterailBlockUtil))]
		[ATMethodArgv(typeof(VariableInt),"ClassHashCode",false, typeof(Framework.Core.MaterailBlockUtil),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableObjectList),"renderers",false, null,null)]
		[ATMethodArgv(typeof(VariableString),"propertyName",false, null,null)]
		[ATMethodArgv(typeof(VariableInt),"param",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bBlock",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bShare",false, null,null)]
		[ATMethodArgv(typeof(VariableInt),"index",false, null,null)]
#endif
		public static bool AT_SetRendersInt(VariableObjectList renderers,VariableString propertyName,VariableInt param,VariableBool bBlock,VariableBool bShare,VariableInt index)
		{
			if(renderers== null || propertyName== null || param== null || bBlock== null || bShare== null || index== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			{//"catch temp"
				if(ms_Renderer_Catch_1 == null) ms_Renderer_Catch_1= new System.Collections.Generic.List<UnityEngine.Renderer>();
				for(int i = 0; i < renderers.GetList().Count; ++i)
				{
					if(renderers.mValue[i]!=null)
						ms_Renderer_Catch_1.Add( (UnityEngine.Renderer)renderers.mValue[i]);
				}
			}
			Framework.Core.MaterailBlockUtil.SetRendersInt(ms_Renderer_Catch_1,propertyName.mValue,param.mValue,bBlock.mValue,bShare.mValue,index.mValue);
			if(ms_Renderer_Catch_1!=null) ms_Renderer_Catch_1.Clear();
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1787816663,"设置全局向量",typeof(Framework.Core.MaterailBlockUtil))]
		[ATMethodArgv(typeof(VariableInt),"ClassHashCode",false, typeof(Framework.Core.MaterailBlockUtil),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableString),"propertyName",false, null,null)]
		[ATMethodArgv(typeof(VariableVector4),"param",false, null,null)]
#endif
		public static bool AT_SetGlobalVector(VariableString propertyName,VariableVector4 param)
		{
			if(propertyName== null || param== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			Framework.Core.MaterailBlockUtil.SetGlobalVector(propertyName.mValue,param.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-2072906693,"设置向量-Go",typeof(Framework.Core.MaterailBlockUtil))]
		[ATMethodArgv(typeof(VariableInt),"ClassHashCode",false, typeof(Framework.Core.MaterailBlockUtil),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableObject),"go",false, typeof(UnityEngine.GameObject),null)]
		[ATMethodArgv(typeof(VariableString),"propertyName",false, null,null)]
		[ATMethodArgv(typeof(VariableVector4),"param",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bBlock",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bShare",false, null,null)]
		[ATMethodArgv(typeof(VariableInt),"index",false, null,null)]
#endif
		public static bool AT_SetGoVector(VariableObject go,VariableString propertyName,VariableVector4 param,VariableBool bBlock,VariableBool bShare,VariableInt index)
		{
			if(go== null || propertyName== null || param== null || bBlock== null || bShare== null || index== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			Framework.Core.MaterailBlockUtil.SetGoVector(go.ToObject<UnityEngine.GameObject>(),propertyName.mValue,param.mValue,bBlock.mValue,bShare.mValue,index.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-891903529,"设置向量-Go",typeof(Framework.Core.MaterailBlockUtil))]
		[ATMethodArgv(typeof(VariableInt),"ClassHashCode",false, typeof(Framework.Core.MaterailBlockUtil),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableObject),"transform",false, typeof(UnityEngine.Transform),null)]
		[ATMethodArgv(typeof(VariableString),"propertyName",false, null,null)]
		[ATMethodArgv(typeof(VariableVector4),"param",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bBlock",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bShare",false, null,null)]
		[ATMethodArgv(typeof(VariableInt),"index",false, null,null)]
#endif
		public static bool AT_SetTransformVector(VariableObject transform,VariableString propertyName,VariableVector4 param,VariableBool bBlock,VariableBool bShare,VariableInt index)
		{
			if(transform== null || propertyName== null || param== null || bBlock== null || bShare== null || index== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			Framework.Core.MaterailBlockUtil.SetTransformVector(transform.ToObject<UnityEngine.Transform>(),propertyName.mValue,param.mValue,bBlock.mValue,bShare.mValue,index.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1956725408,"设置向量-Renderer",typeof(Framework.Core.MaterailBlockUtil))]
		[ATMethodArgv(typeof(VariableInt),"ClassHashCode",false, typeof(Framework.Core.MaterailBlockUtil),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableObject),"render",false, typeof(UnityEngine.Renderer),null)]
		[ATMethodArgv(typeof(VariableString),"propertyName",false, null,null)]
		[ATMethodArgv(typeof(VariableVector4),"param",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bBlock",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bShare",false, null,null)]
		[ATMethodArgv(typeof(VariableInt),"index",false, null,null)]
#endif
		public static bool AT_SetRenderVector(VariableObject render,VariableString propertyName,VariableVector4 param,VariableBool bBlock,VariableBool bShare,VariableInt index)
		{
			if(render== null || propertyName== null || param== null || bBlock== null || bShare== null || index== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			Framework.Core.MaterailBlockUtil.SetRenderVector(render.ToObject<UnityEngine.Renderer>(),propertyName.mValue,param.mValue,bBlock.mValue,bShare.mValue,index.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-551892727,"设置浮点-Renders[]",typeof(Framework.Core.MaterailBlockUtil))]
		[ATMethodArgv(typeof(VariableInt),"ClassHashCode",false, typeof(Framework.Core.MaterailBlockUtil),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableObjectList),"renderers",false, null,null)]
		[ATMethodArgv(typeof(VariableString),"propertyName",false, null,null)]
		[ATMethodArgv(typeof(VariableVector4),"param",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bBlock",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bShare",false, null,null)]
		[ATMethodArgv(typeof(VariableInt),"index",false, null,null)]
#endif
		public static bool AT_SetRendersVector(VariableObjectList renderers,VariableString propertyName,VariableVector4 param,VariableBool bBlock,VariableBool bShare,VariableInt index)
		{
			if(renderers== null || propertyName== null || param== null || bBlock== null || bShare== null || index== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			{//"catch temp"
				if(ms_Renderer_Catch_2 == null) ms_Renderer_Catch_2= new System.Collections.Generic.List<UnityEngine.Renderer>();
				for(int i = 0; i < renderers.GetList().Count; ++i)
				{
					if(renderers.mValue[i]!=null)
						ms_Renderer_Catch_2.Add( (UnityEngine.Renderer)renderers.mValue[i]);
				}
			}
			Framework.Core.MaterailBlockUtil.SetRendersVector(ms_Renderer_Catch_2,propertyName.mValue,param.mValue,bBlock.mValue,bShare.mValue,index.mValue);
			if(ms_Renderer_Catch_2!=null) ms_Renderer_Catch_2.Clear();
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-715934872,"获取向量-Go",typeof(Framework.Core.MaterailBlockUtil))]
		[ATMethodArgv(typeof(VariableInt),"ClassHashCode",false, typeof(Framework.Core.MaterailBlockUtil),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableObject),"target",false, typeof(UnityEngine.GameObject),null)]
		[ATMethodArgv(typeof(VariableString),"propertyName",false, null,null)]
		[ATMethodReturn(typeof(VariableVector4),"pReturn",null,null)]
#endif
		public static bool AT_GetGoVector(VariableObject target,VariableString propertyName,VariableVector4 pReturn=null)
		{
			if(target== null || propertyName== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = Framework.Core.MaterailBlockUtil.GetGoVector(target.ToObject<UnityEngine.GameObject>(),propertyName.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-637943485,"获取向量-Go",typeof(Framework.Core.MaterailBlockUtil))]
		[ATMethodArgv(typeof(VariableInt),"ClassHashCode",false, typeof(Framework.Core.MaterailBlockUtil),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableObject),"target",false, typeof(UnityEngine.Transform),null)]
		[ATMethodArgv(typeof(VariableString),"propertyName",false, null,null)]
		[ATMethodReturn(typeof(VariableVector4),"pReturn",null,null)]
#endif
		public static bool AT_GetTransformVector(VariableObject target,VariableString propertyName,VariableVector4 pReturn=null)
		{
			if(target== null || propertyName== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = Framework.Core.MaterailBlockUtil.GetTransformVector(target.ToObject<UnityEngine.Transform>(),propertyName.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1899608098,"获取向量-Renderer",typeof(Framework.Core.MaterailBlockUtil))]
		[ATMethodArgv(typeof(VariableInt),"ClassHashCode",false, typeof(Framework.Core.MaterailBlockUtil),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableObject),"render",false, typeof(UnityEngine.Renderer),null)]
		[ATMethodArgv(typeof(VariableString),"propertyName",false, null,null)]
		[ATMethodReturn(typeof(VariableVector4),"pReturn",null,null)]
#endif
		public static bool AT_GetRenderVector(VariableObject render,VariableString propertyName,VariableVector4 pReturn=null)
		{
			if(render== null || propertyName== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			if(pReturn != null)
			{	
				pReturn.mValue = Framework.Core.MaterailBlockUtil.GetRenderVector(render.ToObject<UnityEngine.Renderer>(),propertyName.mValue);
			}	
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1113675273,"设置全局向量数组",typeof(Framework.Core.MaterailBlockUtil))]
		[ATMethodArgv(typeof(VariableInt),"ClassHashCode",false, typeof(Framework.Core.MaterailBlockUtil),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableString),"propertyName",false, null,null)]
		[ATMethodArgv(typeof(VariableVector4List),"values",false, null,null)]
#endif
		public static bool AT_SetGlobalVectorArray(VariableString propertyName,VariableVector4List values)
		{
			if(propertyName== null || values== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			{//"catch temp"
				if(ms_Vector4_Catch_0 == null) ms_Vector4_Catch_0= new System.Collections.Generic.List<UnityEngine.Vector4>();
				for(int i = 0; i < values.GetList().Count; ++i)
				{
					if(values.mValue[i]!=null)
						ms_Vector4_Catch_0.Add( (UnityEngine.Vector4)values.mValue[i]);
				}
			}
			Framework.Core.MaterailBlockUtil.SetGlobalVectorArray(propertyName.mValue,ms_Vector4_Catch_0);
			if(ms_Vector4_Catch_0!=null) ms_Vector4_Catch_0.Clear();
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-495193053,"设置向量数组-Go",typeof(Framework.Core.MaterailBlockUtil))]
		[ATMethodArgv(typeof(VariableInt),"ClassHashCode",false, typeof(Framework.Core.MaterailBlockUtil),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableObject),"target",false, typeof(UnityEngine.GameObject),null)]
		[ATMethodArgv(typeof(VariableString),"propertyName",false, null,null)]
		[ATMethodArgv(typeof(VariableVector4List),"values",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bBlock",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bShare",false, null,null)]
		[ATMethodArgv(typeof(VariableInt),"index",false, null,null)]
#endif
		public static bool AT_SetGoVectorArray(VariableObject target,VariableString propertyName,VariableVector4List values,VariableBool bBlock,VariableBool bShare,VariableInt index)
		{
			if(target== null || propertyName== null || values== null || bBlock== null || bShare== null || index== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			{//"catch temp"
				if(ms_Vector4_Catch_1 == null) ms_Vector4_Catch_1= new System.Collections.Generic.List<UnityEngine.Vector4>();
				for(int i = 0; i < values.GetList().Count; ++i)
				{
					if(values.mValue[i]!=null)
						ms_Vector4_Catch_1.Add( (UnityEngine.Vector4)values.mValue[i]);
				}
			}
			Framework.Core.MaterailBlockUtil.SetGoVectorArray(target.ToObject<UnityEngine.GameObject>(),propertyName.mValue,ms_Vector4_Catch_1,bBlock.mValue,bShare.mValue,index.mValue);
			if(ms_Vector4_Catch_1!=null) ms_Vector4_Catch_1.Clear();
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1233976614,"设置向量数组-Renderer",typeof(Framework.Core.MaterailBlockUtil))]
		[ATMethodArgv(typeof(VariableInt),"ClassHashCode",false, typeof(Framework.Core.MaterailBlockUtil),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableObject),"render",false, typeof(UnityEngine.Renderer),null)]
		[ATMethodArgv(typeof(VariableString),"propertyName",false, null,null)]
		[ATMethodArgv(typeof(VariableVector4List),"values",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bBlock",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bShare",false, null,null)]
		[ATMethodArgv(typeof(VariableInt),"index",false, null,null)]
#endif
		public static bool AT_SetRenderVectorArray(VariableObject render,VariableString propertyName,VariableVector4List values,VariableBool bBlock,VariableBool bShare,VariableInt index)
		{
			if(render== null || propertyName== null || values== null || bBlock== null || bShare== null || index== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			{//"catch temp"
				if(ms_Vector4_Catch_2 == null) ms_Vector4_Catch_2= new System.Collections.Generic.List<UnityEngine.Vector4>();
				for(int i = 0; i < values.GetList().Count; ++i)
				{
					if(values.mValue[i]!=null)
						ms_Vector4_Catch_2.Add( (UnityEngine.Vector4)values.mValue[i]);
				}
			}
			Framework.Core.MaterailBlockUtil.SetRenderVectorArray(render.ToObject<UnityEngine.Renderer>(),propertyName.mValue,ms_Vector4_Catch_2,bBlock.mValue,bShare.mValue,index.mValue);
			if(ms_Vector4_Catch_2!=null) ms_Vector4_Catch_2.Clear();
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-790959705,"设置全局矩阵",typeof(Framework.Core.MaterailBlockUtil))]
		[ATMethodArgv(typeof(VariableInt),"ClassHashCode",false, typeof(Framework.Core.MaterailBlockUtil),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableString),"propertyName",false, null,null)]
		[ATMethodArgv(typeof(VariableFloatList),"param",false, null,null)]
#endif
		public static bool AT_SetGlobalVectorArray_1(VariableString propertyName,VariableFloatList param)
		{
			if(propertyName== null || param== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
UnityEngine.Matrix4x4 param_temp = UnityEngine.Matrix4x4.identity;
			if(param.mValue !=null && param.mValue.Count == 16) 
			{
				for(int i =0; i < 16; ++i) param_temp[i] = param.mValue[i];
			}
			Framework.Core.MaterailBlockUtil.SetGlobalVectorArray(propertyName.mValue,param_temp);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1987853416,"设置矩阵",typeof(Framework.Core.MaterailBlockUtil))]
		[ATMethodArgv(typeof(VariableInt),"ClassHashCode",false, typeof(Framework.Core.MaterailBlockUtil),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableObject),"render",false, typeof(UnityEngine.Renderer),null)]
		[ATMethodArgv(typeof(VariableString),"propertyName",false, null,null)]
		[ATMethodArgv(typeof(VariableFloatList),"param",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bBlock",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bShare",false, null,null)]
		[ATMethodArgv(typeof(VariableInt),"index",false, null,null)]
#endif
		public static bool AT_SetMatrix(VariableObject render,VariableString propertyName,VariableFloatList param,VariableBool bBlock,VariableBool bShare,VariableInt index)
		{
			if(render== null || propertyName== null || param== null || bBlock== null || bShare== null || index== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
UnityEngine.Matrix4x4 param_temp = UnityEngine.Matrix4x4.identity;
			if(param.mValue !=null && param.mValue.Count == 16) 
			{
				for(int i =0; i < 16; ++i) param_temp[i] = param.mValue[i];
			}
			Framework.Core.MaterailBlockUtil.SetMatrix(render.ToObject<UnityEngine.Renderer>(),propertyName.mValue,param_temp,bBlock.mValue,bShare.mValue,index.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1048496418,"设置全局颜色",typeof(Framework.Core.MaterailBlockUtil))]
		[ATMethodArgv(typeof(VariableInt),"ClassHashCode",false, typeof(Framework.Core.MaterailBlockUtil),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableString),"propertyName",false, null,null)]
		[ATMethodArgv(typeof(VariableColor),"param",false, null,null)]
#endif
		public static bool AT_SetGlobalColor(VariableString propertyName,VariableColor param)
		{
			if(propertyName== null || param== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			Framework.Core.MaterailBlockUtil.SetGlobalColor(propertyName.mValue,param.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1940242792,"设置纹理参数-Renderer",typeof(Framework.Core.MaterailBlockUtil))]
		[ATMethodArgv(typeof(VariableInt),"ClassHashCode",false, typeof(Framework.Core.MaterailBlockUtil),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableObject),"render",false, typeof(UnityEngine.Renderer),null)]
		[ATMethodArgv(typeof(VariableString),"propertyName",false, null,null)]
		[ATMethodArgv(typeof(VariableVector4),"param",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bBlock",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bShare",false, null,null)]
		[ATMethodArgv(typeof(VariableInt),"index",false, null,null)]
#endif
		public static bool AT_SetTextureScale(VariableObject render,VariableString propertyName,VariableVector4 param,VariableBool bBlock,VariableBool bShare,VariableInt index)
		{
			if(render== null || propertyName== null || param== null || bBlock== null || bShare== null || index== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			Framework.Core.MaterailBlockUtil.SetTextureScale(render.ToObject<UnityEngine.Renderer>(),propertyName.mValue,param.mValue,bBlock.mValue,bShare.mValue,index.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(1071214589,"设置颜色-Go",typeof(Framework.Core.MaterailBlockUtil))]
		[ATMethodArgv(typeof(VariableInt),"ClassHashCode",false, typeof(Framework.Core.MaterailBlockUtil),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableObject),"go",false, typeof(UnityEngine.GameObject),null)]
		[ATMethodArgv(typeof(VariableString),"propertyName",false, null,null)]
		[ATMethodArgv(typeof(VariableColor),"color",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bBlock",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bShare",false, null,null)]
		[ATMethodArgv(typeof(VariableInt),"index",false, null,null)]
#endif
		public static bool AT_SetGoColor(VariableObject go,VariableString propertyName,VariableColor color,VariableBool bBlock,VariableBool bShare,VariableInt index)
		{
			if(go== null || propertyName== null || color== null || bBlock== null || bShare== null || index== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			Framework.Core.MaterailBlockUtil.SetGoColor(go.ToObject<UnityEngine.GameObject>(),propertyName.mValue,color.mValue,bBlock.mValue,bShare.mValue,index.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(239362692,"设置颜色-Transform",typeof(Framework.Core.MaterailBlockUtil))]
		[ATMethodArgv(typeof(VariableInt),"ClassHashCode",false, typeof(Framework.Core.MaterailBlockUtil),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableObject),"transform",false, typeof(UnityEngine.Transform),null)]
		[ATMethodArgv(typeof(VariableString),"propertyName",false, null,null)]
		[ATMethodArgv(typeof(VariableColor),"color",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bBlock",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bShare",false, null,null)]
		[ATMethodArgv(typeof(VariableInt),"index",false, null,null)]
#endif
		public static bool AT_SetTransformColor(VariableObject transform,VariableString propertyName,VariableColor color,VariableBool bBlock,VariableBool bShare,VariableInt index)
		{
			if(transform== null || propertyName== null || color== null || bBlock== null || bShare== null || index== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			Framework.Core.MaterailBlockUtil.SetTransformColor(transform.ToObject<UnityEngine.Transform>(),propertyName.mValue,color.mValue,bBlock.mValue,bShare.mValue,index.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1399844080,"设置全局纹理",typeof(Framework.Core.MaterailBlockUtil))]
		[ATMethodArgv(typeof(VariableInt),"ClassHashCode",false, typeof(Framework.Core.MaterailBlockUtil),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableString),"propertyName",false, null,null)]
		[ATMethodArgv(typeof(VariableObject),"param",false, typeof(UnityEngine.Texture),null)]
#endif
		public static bool AT_SetGlobalTexture(VariableString propertyName,VariableObject param)
		{
			if(propertyName== null || param== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			Framework.Core.MaterailBlockUtil.SetGlobalTexture(propertyName.mValue,param.ToObject<UnityEngine.Texture>());
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(387815761,"设置纹理-Renderer",typeof(Framework.Core.MaterailBlockUtil))]
		[ATMethodArgv(typeof(VariableInt),"ClassHashCode",false, typeof(Framework.Core.MaterailBlockUtil),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableObject),"render",false, typeof(UnityEngine.Renderer),null)]
		[ATMethodArgv(typeof(VariableString),"propertyName",false, null,null)]
		[ATMethodArgv(typeof(VariableObject),"tex",false, typeof(UnityEngine.Texture),null)]
		[ATMethodArgv(typeof(VariableBool),"bBlock",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bShare",false, null,null)]
		[ATMethodArgv(typeof(VariableInt),"index",false, null,null)]
#endif
		public static bool AT_SetTexture(VariableObject render,VariableString propertyName,VariableObject tex,VariableBool bBlock,VariableBool bShare,VariableInt index)
		{
			if(render== null || propertyName== null || tex== null || bBlock== null || bShare== null || index== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			Framework.Core.MaterailBlockUtil.SetTexture(render.ToObject<UnityEngine.Renderer>(),propertyName.mValue,tex.ToObject<UnityEngine.Texture>(),bBlock.mValue,bShare.mValue,index.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1482375214,"设置颜色-Renderer",typeof(Framework.Core.MaterailBlockUtil))]
		[ATMethodArgv(typeof(VariableInt),"ClassHashCode",false, typeof(Framework.Core.MaterailBlockUtil),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableObject),"render",false, typeof(UnityEngine.Renderer),null)]
		[ATMethodArgv(typeof(VariableString),"propertyName",false, null,null)]
		[ATMethodArgv(typeof(VariableColor),"color",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bBlock",false, null,null)]
		[ATMethodArgv(typeof(VariableBool),"bShare",false, null,null)]
		[ATMethodArgv(typeof(VariableInt),"index",false, null,null)]
#endif
		public static bool AT_SetRenderColor(VariableObject render,VariableString propertyName,VariableColor color,VariableBool bBlock,VariableBool bShare,VariableInt index)
		{
			if(render== null || propertyName== null || color== null || bBlock== null || bShare== null || index== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			Framework.Core.MaterailBlockUtil.SetRenderColor(render.ToObject<UnityEngine.Renderer>(),propertyName.mValue,color.mValue,bBlock.mValue,bShare.mValue,index.mValue);
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(397644477,"清除属性修改器-Renderer",typeof(Framework.Core.MaterailBlockUtil))]
		[ATMethodArgv(typeof(VariableInt),"ClassHashCode",false, typeof(Framework.Core.MaterailBlockUtil),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableObject),"render",false, typeof(UnityEngine.Renderer),null)]
#endif
		public static bool AT_ClearBlock(VariableObject render)
		{
			if(render== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			Framework.Core.MaterailBlockUtil.ClearBlock(render.ToObject<UnityEngine.Renderer>());
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1322343569,"清除属性修改器-Renderers[]",typeof(Framework.Core.MaterailBlockUtil))]
		[ATMethodArgv(typeof(VariableInt),"ClassHashCode",false, typeof(Framework.Core.MaterailBlockUtil),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableObjectList),"renders",false, null,null)]
#endif
		public static bool AT_ClearRendersBlock(VariableObjectList renders)
		{
			if(renders== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			{//"catch temp"
				if(ms_Renderer_Catch_3 == null) ms_Renderer_Catch_3= new System.Collections.Generic.List<UnityEngine.Renderer>();
				for(int i = 0; i < renders.GetList().Count; ++i)
				{
					if(renders.mValue[i]!=null)
						ms_Renderer_Catch_3.Add( (UnityEngine.Renderer)renders.mValue[i]);
				}
			}
			Framework.Core.MaterailBlockUtil.ClearRendersBlock(ms_Renderer_Catch_3);
			if(ms_Renderer_Catch_3!=null) ms_Renderer_Catch_3.Clear();
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1845800469,"清除属性修改器-Go",typeof(Framework.Core.MaterailBlockUtil))]
		[ATMethodArgv(typeof(VariableInt),"ClassHashCode",false, typeof(Framework.Core.MaterailBlockUtil),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableObject),"go",false, typeof(UnityEngine.GameObject),null)]
#endif
		public static bool AT_ClearaGOBlock(VariableObject go)
		{
			if(go== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			Framework.Core.MaterailBlockUtil.ClearaGOBlock(go.ToObject<UnityEngine.GameObject>());
			return true;
		}
#if UNITY_EDITOR
		[ATMonoFunc(-1713501713,"设置全局LOD",typeof(Framework.Core.MaterailBlockUtil))]
		[ATMethodArgv(typeof(VariableInt),"ClassHashCode",false, typeof(Framework.Core.MaterailBlockUtil),null, "", false, -1, true,false)]
		[ATMethodArgv(typeof(VariableInt),"lod",false, null,null)]
#endif
		public static bool AT_SetGlobalLOD(VariableInt lod)
		{
			if(lod== null)
			{
				AgentTreeUtl.LogWarning("argv is null...");
				return true;
			}
			Framework.Core.MaterailBlockUtil.SetGlobalLOD(lod.mValue);
			return true;
		}
		private static System.Collections.Generic.List<UnityEngine.Renderer> ms_Renderer_Catch_0;
		private static System.Collections.Generic.List<UnityEngine.Renderer> ms_Renderer_Catch_1;
		private static System.Collections.Generic.List<UnityEngine.Renderer> ms_Renderer_Catch_2;
		private static System.Collections.Generic.List<UnityEngine.Renderer> ms_Renderer_Catch_3;
		private static System.Collections.Generic.List<UnityEngine.Vector4> ms_Vector4_Catch_0;
		private static System.Collections.Generic.List<UnityEngine.Vector4> ms_Vector4_Catch_1;
		private static System.Collections.Generic.List<UnityEngine.Vector4> ms_Vector4_Catch_2;
		public static bool DoAction(VariableUser pUserClass, AgentTreeTask pTask, ActionNode pAction, int functionId = 0)
		{
			int funcId = (functionId!=0)?functionId:(int)pAction.GetExcudeHash();
			switch(funcId)
			{
				case -1902165494:
				{//Framework.Core.MaterailBlockUtil->SetGlobalFloat
					if(pAction.inArgvs.Length <=2) return true;
					return AgentTree_MaterailBlockUtil.AT_SetGlobalFloat(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(2, pTask));
				}
				case -1456886206:
				{//Framework.Core.MaterailBlockUtil->SetGoFloat
					if(pAction.inArgvs.Length <=6) return true;
					return AgentTree_MaterailBlockUtil.AT_SetGoFloat(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableObject>(1, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(2, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(3, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(4, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(5, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(6, pTask));
				}
				case -1580948318:
				{//Framework.Core.MaterailBlockUtil->SetTransformFloat
					if(pAction.inArgvs.Length <=6) return true;
					return AgentTree_MaterailBlockUtil.AT_SetTransformFloat(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableObject>(1, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(2, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(3, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(4, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(5, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(6, pTask));
				}
				case -1831898998:
				{//Framework.Core.MaterailBlockUtil->SetRenderFloat
					if(pAction.inArgvs.Length <=6) return true;
					return AgentTree_MaterailBlockUtil.AT_SetRenderFloat(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableObject>(1, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(2, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(3, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(4, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(5, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(6, pTask));
				}
				case 30421350:
				{//Framework.Core.MaterailBlockUtil->SetRendersFloat
					if(pAction.inArgvs.Length <=6) return true;
					return AgentTree_MaterailBlockUtil.AT_SetRendersFloat(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableObjectList>(1, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(2, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloat>(3, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(4, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(5, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(6, pTask));
				}
				case 1489213840:
				{//Framework.Core.MaterailBlockUtil->SetGlobalInt
					if(pAction.inArgvs.Length <=2) return true;
					return AgentTree_MaterailBlockUtil.AT_SetGlobalInt(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(2, pTask));
				}
				case -1117852636:
				{//Framework.Core.MaterailBlockUtil->SetGoInt
					if(pAction.inArgvs.Length <=6) return true;
					return AgentTree_MaterailBlockUtil.AT_SetGoInt(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableObject>(1, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(2, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(3, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(4, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(5, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(6, pTask));
				}
				case -728132556:
				{//Framework.Core.MaterailBlockUtil->SetTransformInt
					if(pAction.inArgvs.Length <=6) return true;
					return AgentTree_MaterailBlockUtil.AT_SetTransformInt(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableObject>(1, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(2, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(3, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(4, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(5, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(6, pTask));
				}
				case 730283371:
				{//Framework.Core.MaterailBlockUtil->SetRenderInt
					if(pAction.inArgvs.Length <=6) return true;
					return AgentTree_MaterailBlockUtil.AT_SetRenderInt(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableObject>(1, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(2, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(3, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(4, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(5, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(6, pTask));
				}
				case 787976746:
				{//Framework.Core.MaterailBlockUtil->SetRendersInt
					if(pAction.inArgvs.Length <=6) return true;
					return AgentTree_MaterailBlockUtil.AT_SetRendersInt(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableObjectList>(1, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(2, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(3, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(4, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(5, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(6, pTask));
				}
				case -1787816663:
				{//Framework.Core.MaterailBlockUtil->SetGlobalVector
					if(pAction.inArgvs.Length <=2) return true;
					return AgentTree_MaterailBlockUtil.AT_SetGlobalVector(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableVector4>(2, pTask));
				}
				case -2072906693:
				{//Framework.Core.MaterailBlockUtil->SetGoVector
					if(pAction.inArgvs.Length <=6) return true;
					return AgentTree_MaterailBlockUtil.AT_SetGoVector(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableObject>(1, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(2, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableVector4>(3, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(4, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(5, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(6, pTask));
				}
				case -891903529:
				{//Framework.Core.MaterailBlockUtil->SetTransformVector
					if(pAction.inArgvs.Length <=6) return true;
					return AgentTree_MaterailBlockUtil.AT_SetTransformVector(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableObject>(1, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(2, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableVector4>(3, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(4, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(5, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(6, pTask));
				}
				case -1956725408:
				{//Framework.Core.MaterailBlockUtil->SetRenderVector
					if(pAction.inArgvs.Length <=6) return true;
					return AgentTree_MaterailBlockUtil.AT_SetRenderVector(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableObject>(1, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(2, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableVector4>(3, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(4, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(5, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(6, pTask));
				}
				case -551892727:
				{//Framework.Core.MaterailBlockUtil->SetRendersVector
					if(pAction.inArgvs.Length <=6) return true;
					return AgentTree_MaterailBlockUtil.AT_SetRendersVector(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableObjectList>(1, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(2, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableVector4>(3, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(4, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(5, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(6, pTask));
				}
				case -715934872:
				{//Framework.Core.MaterailBlockUtil->GetGoVector
					if(pAction.inArgvs.Length <=2) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					return AgentTree_MaterailBlockUtil.AT_GetGoVector(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableObject>(1, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(2, pTask),pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableVector4>(0, pTask));
				}
				case -637943485:
				{//Framework.Core.MaterailBlockUtil->GetTransformVector
					if(pAction.inArgvs.Length <=2) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					return AgentTree_MaterailBlockUtil.AT_GetTransformVector(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableObject>(1, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(2, pTask),pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableVector4>(0, pTask));
				}
				case -1899608098:
				{//Framework.Core.MaterailBlockUtil->GetRenderVector
					if(pAction.inArgvs.Length <=2) return true;
					if(pAction.outArgvs ==null || pAction.outArgvs.Length <1) return true;
					return AgentTree_MaterailBlockUtil.AT_GetRenderVector(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableObject>(1, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(2, pTask),pAction.GetOutVariableByIndex<Framework.Plugin.AT.VariableVector4>(0, pTask));
				}
				case 1113675273:
				{//Framework.Core.MaterailBlockUtil->SetGlobalVectorArray
					if(pAction.inArgvs.Length <=2) return true;
					return AgentTree_MaterailBlockUtil.AT_SetGlobalVectorArray(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableVector4List>(2, pTask));
				}
				case -495193053:
				{//Framework.Core.MaterailBlockUtil->SetGoVectorArray
					if(pAction.inArgvs.Length <=6) return true;
					return AgentTree_MaterailBlockUtil.AT_SetGoVectorArray(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableObject>(1, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(2, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableVector4List>(3, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(4, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(5, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(6, pTask));
				}
				case 1233976614:
				{//Framework.Core.MaterailBlockUtil->SetRenderVectorArray
					if(pAction.inArgvs.Length <=6) return true;
					return AgentTree_MaterailBlockUtil.AT_SetRenderVectorArray(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableObject>(1, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(2, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableVector4List>(3, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(4, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(5, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(6, pTask));
				}
				case -790959705:
				{//Framework.Core.MaterailBlockUtil->SetGlobalVectorArray_1
					if(pAction.inArgvs.Length <=2) return true;
					return AgentTree_MaterailBlockUtil.AT_SetGlobalVectorArray_1(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloatList>(2, pTask));
				}
				case -1987853416:
				{//Framework.Core.MaterailBlockUtil->SetMatrix
					if(pAction.inArgvs.Length <=6) return true;
					return AgentTree_MaterailBlockUtil.AT_SetMatrix(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableObject>(1, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(2, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableFloatList>(3, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(4, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(5, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(6, pTask));
				}
				case 1048496418:
				{//Framework.Core.MaterailBlockUtil->SetGlobalColor
					if(pAction.inArgvs.Length <=2) return true;
					return AgentTree_MaterailBlockUtil.AT_SetGlobalColor(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableColor>(2, pTask));
				}
				case -1940242792:
				{//Framework.Core.MaterailBlockUtil->SetTextureScale
					if(pAction.inArgvs.Length <=6) return true;
					return AgentTree_MaterailBlockUtil.AT_SetTextureScale(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableObject>(1, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(2, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableVector4>(3, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(4, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(5, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(6, pTask));
				}
				case 1071214589:
				{//Framework.Core.MaterailBlockUtil->SetGoColor
					if(pAction.inArgvs.Length <=6) return true;
					return AgentTree_MaterailBlockUtil.AT_SetGoColor(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableObject>(1, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(2, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableColor>(3, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(4, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(5, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(6, pTask));
				}
				case 239362692:
				{//Framework.Core.MaterailBlockUtil->SetTransformColor
					if(pAction.inArgvs.Length <=6) return true;
					return AgentTree_MaterailBlockUtil.AT_SetTransformColor(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableObject>(1, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(2, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableColor>(3, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(4, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(5, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(6, pTask));
				}
				case -1399844080:
				{//Framework.Core.MaterailBlockUtil->SetGlobalTexture
					if(pAction.inArgvs.Length <=2) return true;
					return AgentTree_MaterailBlockUtil.AT_SetGlobalTexture(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(1, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableObject>(2, pTask));
				}
				case 387815761:
				{//Framework.Core.MaterailBlockUtil->SetTexture
					if(pAction.inArgvs.Length <=6) return true;
					return AgentTree_MaterailBlockUtil.AT_SetTexture(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableObject>(1, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(2, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableObject>(3, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(4, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(5, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(6, pTask));
				}
				case -1482375214:
				{//Framework.Core.MaterailBlockUtil->SetRenderColor
					if(pAction.inArgvs.Length <=6) return true;
					return AgentTree_MaterailBlockUtil.AT_SetRenderColor(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableObject>(1, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableString>(2, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableColor>(3, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(4, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableBool>(5, pTask),pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(6, pTask));
				}
				case 397644477:
				{//Framework.Core.MaterailBlockUtil->ClearBlock
					if(pAction.inArgvs.Length <=1) return true;
					return AgentTree_MaterailBlockUtil.AT_ClearBlock(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableObject>(1, pTask));
				}
				case -1322343569:
				{//Framework.Core.MaterailBlockUtil->ClearRendersBlock
					if(pAction.inArgvs.Length <=1) return true;
					return AgentTree_MaterailBlockUtil.AT_ClearRendersBlock(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableObjectList>(1, pTask));
				}
				case -1845800469:
				{//Framework.Core.MaterailBlockUtil->ClearaGOBlock
					if(pAction.inArgvs.Length <=1) return true;
					return AgentTree_MaterailBlockUtil.AT_ClearaGOBlock(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableObject>(1, pTask));
				}
				case -1713501713:
				{//Framework.Core.MaterailBlockUtil->SetGlobalLOD
					if(pAction.inArgvs.Length <=1) return true;
					return AgentTree_MaterailBlockUtil.AT_SetGlobalLOD(pAction.GetInVariableByIndex<Framework.Plugin.AT.VariableInt>(1, pTask));
				}
			}
			return true;
		}
	}
}
