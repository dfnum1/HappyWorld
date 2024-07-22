/********************************************************************
生成日期:	23:3:2020   13:47
类    名: 	HotScriptsManager
作    者:	HappLI
描    述:	代码热更
*********************************************************************/
using Framework.Core;
using Framework.Module;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
#if USE_ILRUNTIME
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;
#endif
using TopGame.Core;
using UnityEngine;

namespace TopGame.Hot
{
    public class HotScriptsModule
    {
        protected static string ms_strRoot = null;
        protected static string ms_strUpdateRoot = null;
#if USE_ILRUNTIME || USE_ASSEMLYDYNAMICLOAD
        private string _dllPath = "Assets/Datas/Hot/ScriptHots.dll.bytes";
        private string _pdbPath = "Assets/Datas/Hot/ScriptHots.dll.mdb.bytes";
#endif

#if USE_ILRUNTIME
        private AppDomain m_pAppDomain;
        private ILRuntime.Runtime.Intepreter.ILTypeInstance m_pHotFixEntity;
#elif USE_ASSEMLYDYNAMICLOAD
        private Assembly m_pAssembly;
        private object m_pHotFixEntity;
#elif USE_INJECTFIX
        string GetHotScriptFile(string patchName)
        {
            string strFile = Framework.Core.CommonUtility.stringBuilder.Append(HotScriptsModule.ms_strUpdateRoot).Append(patchName).ToString();
            if (!System.IO.File.Exists(strFile))
            {
                strFile = Framework.Core.CommonUtility.stringBuilder.Append(HotScriptsModule.ms_strRoot).Append(patchName).ToString();
            }
            return strFile;
        }
        IFix.Core.VirtualMachine m_pMainVM = null;
        IFix.Core.VirtualMachine m_pScriptVM = null;
#elif USE_HYBRIDCLR
        [System.Serializable]
        public struct HyBridHots
        {
            public List<string> aotDlls;
            public List<string> hotDlls;
            public List<string> diffDlls;
        }
        string GetHotAssembly(string assemblyName)
        {
#if UNITY_EDITOR
#endif
            string strFile = Framework.Core.BaseUtil.stringBuilder.Append(HotScriptsModule.ms_strUpdateRoot).Append(assemblyName).ToString();
#if UNITY_EDITOR
            strFile = Application.dataPath + "/../Library/ScriptAssemblies/" + assemblyName;
#endif
            if (!System.IO.File.Exists(strFile))
            {
                strFile = Framework.Core.BaseUtil.stringBuilder.Append(HotScriptsModule.ms_strRoot).Append(assemblyName).ToString();
            }
            return strFile;
        }      
#endif

#if USE_ILRUNTIME || USE_ASSEMLYDYNAMICLOAD
        private List<Type> m_vHotFixTypes;
        System.IO.MemoryStream m_pDll = null;
        System.IO.MemoryStream m_pPDB = null;
#endif
        public Action OnDestroyAction = null;
#if USE_HYBRIDCLR
        System.Reflection.Assembly m_pMainAssembly = null;
#endif
        //------------------------------------------------------
        public void Awake(FileSystem fileSystem)
        {
            ms_strRoot = fileSystem.StreamBinaryPath + "HotScripts/";
            ms_strUpdateRoot = fileSystem.UpdateDataPath + "HotScripts/";
#if USE_ILRUNTIME || USE_ASSEMLYDYNAMICLOAD
            AssetOperiaon loadOp = FileSystemUtil.BuildAssetOp(_dllPath);
            loadOp.OnCallback = OnDLLLoaded;
            loadOp.Refresh();

            loadOp = FileSystemUtil.BuildAssetOp(_pdbPath);
            loadOp.OnCallback = OnPDBLoaded;
            loadOp.Refresh();
#elif USE_INJECTFIX
            int dataSize = 0;
            byte[] bytes = TopGame.Core.JniPlugin.ReadFileAllBytes(GetHotScriptFile("MainScripts.dll.patch"), ref dataSize, true);
            if (bytes != null && dataSize > 4)
            {
                try
                {
                    int version = BitConverter.ToInt32(bytes, 0);
                    if (Framework.Base.ConfigUtil.ConverVersion(FileSystemUtil.PlublishVersion) <= version)
                    {
                        m_pMainVM = IFix.Core.PatchManager.Load(new System.IO.MemoryStream(bytes, 4, dataSize - 4));
                        UnityEngine.Debug.Log("-----Hot MainScripts Init!!!-----");
                    }
                }
                catch (System.Exception exp)
                {
                    Debug.LogWarning(exp.ToString());
                }
            }
            dataSize = 0;
            bytes = TopGame.Core.JniPlugin.ReadFileAllBytes(GetHotScriptFile("Assembly-CSharp.dll.patch"), ref dataSize, true);
            if (bytes != null && dataSize > 4)
            {
                try
                {
                    int version = BitConverter.ToInt32(bytes, 0);
                    if(Framework.Base.ConfigUtil.ConverVersion(FileSystemUtil.PlublishVersion) <= version)
                    {
                        m_pMainVM = IFix.Core.PatchManager.Load(new System.IO.MemoryStream(bytes, 4, dataSize - 4));
                        UnityEngine.Debug.Log("-----Hot Assembly-CSharp Init!!!-----");
                    }
                }
                catch (System.Exception exp)
                {
                    Debug.LogWarning(exp.ToString());
                }
            }
#elif USE_HYBRIDCLR
            try
            {
                m_pMainAssembly = null;
                int dataSize = 0;
                byte[] bytes = null;
                TextAsset hotdlls = Resources.Load<TextAsset>("hotdlls");
                if (hotdlls != null)
                {
                    HyBridHots hots = JsonUtility.FromJson<HyBridHots>(hotdlls.text);
                    if(hots.aotDlls!=null)
                    {
                        for(int i =0; i < hots.aotDlls.Count; ++i)
                        {
                            dataSize = 0;
                            bytes = TopGame.Core.JniPlugin.ReadFileAllBytes(GetHotAssembly(hots.aotDlls[i]), ref dataSize, true);
                            if (bytes != null && dataSize > 0)
                            {
                                var result = HybridCLR.RuntimeApi.LoadMetadataForAOTAssembly(bytes, HybridCLR.HomologousImageMode.SuperSet);
                                Debug.Log(hots.aotDlls[i] + " AOT Meta "+ result);
                            }
                        }
                    }
//                     if (hots.diffDlls != null)
//                     {
//                         for (int i = 0; i < hots.diffDlls.Count; ++i)
//                         {
//                             bytes = TopGame.Core.JniPlugin.ReadFileAllBytes(GetHotAssembly(hots.diffDlls[i]), ref dataSize, true);
//                             if (bytes != null && dataSize > 0)
//                             {
//                                 byte[] patchBytes = TopGame.Core.JniPlugin.ReadFileAllBytes(GetHotAssembly(hots.diffDlls[i]+".patch"), ref dataSize, true);
//                                 if (dataSize > 0 && patchBytes != null)
//                                 {
//                                     HybridCLR.LoadImageErrorCode errorCode = HybridCLR.RuntimeApi.LoadDifferentialHybridAssembly(bytes, patchBytes);
//                                     System.Reflection.Assembly.Load(bytes);
//                                     if (errorCode == HybridCLR.LoadImageErrorCode.OK)
//                                         Debug.Log(hots.diffDlls[i] + " DH : " + errorCode);
//                                     else
//                                         Debug.LogError(hots.diffDlls[i] + " DH : " + errorCode);
//                                 }
//                             }
//                             else Debug.LogError(hots.diffDlls[i] + "DH load failed!");
//                         }
//                     }
                    if (hots.hotDlls != null)
                    {
                        for (int i = 0; i < hots.hotDlls.Count; ++i)
                        {
                            bytes = TopGame.Core.JniPlugin.ReadFileAllBytes(GetHotAssembly(hots.hotDlls[i]), ref dataSize, true);
                            if (bytes != null && dataSize > 0)
                            {
                                if(hots.hotDlls[i].CompareTo("MainScripts.dll") == 0)
                                {
                                    m_pMainAssembly = System.Reflection.Assembly.Load(bytes);
                                }
                                else
                                    System.Reflection.Assembly.Load(bytes);
                                Debug.Log(hots.hotDlls[i] + " load ok!");
                            }
                            else Debug.LogError(hots.hotDlls[i] +  " load failed!");
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Debug.LogWarning(ex.ToString());
            }
#endif
        }
#if USE_ILRUNTIME || USE_ASSEMLYDYNAMICLOAD
        //------------------------------------------------------
        void OnDLLLoaded(AssetOperiaon op)
        {
            if (op == null || !op.isValid()) return;
            m_pDll = new MemoryStream( (op.GetOrigin() as TextAsset).bytes );
            op.pAsset.Release();
        }
        //------------------------------------------------------
        void OnPDBLoaded(AssetOperiaon op)
        {
            if (op == null || !op.isValid()) return;
            m_pPDB = new MemoryStream((op.GetOrigin() as TextAsset).bytes);
            op.pAsset.Release();
        }
#endif
        //------------------------------------------------------
        public bool Start()
        {
#if USE_ILRUNTIME
            m_pAppDomain = new AppDomain();
            if (m_pDll != null && m_pPDB != null)
            {
                m_pAppDomain.LoadAssembly(m_pDll, m_pPDB, new Mono.Cecil.Pdb.PdbReaderProvider());
            }
            else if(m_pDll!=null)
            {
                m_pAppDomain.LoadAssembly(m_pDll, null, new Mono.Cecil.Pdb.PdbReaderProvider());
            }
            else
            {
                Framework.Plugin.Logger.Error("HotScripts Load Error!");
                return false;
            }

            if (m_pAppDomain != null)
            {
                //! 注册CLR绑定
                //ILRuntime.Runtime.Generated.CLRBindings.Initialize(m_pAppDomain);

                //跨域继承的基类
                m_pAppDomain.RegisterCrossBindingAdaptor(new IMessageAdaptor());
                m_pAppDomain.DelegateManager.RegisterFunctionDelegate<IMessageAdaptor.Adaptor>();
                m_pAppDomain.DelegateManager.RegisterMethodDelegate<System.Object>();
                m_pAppDomain.DelegateManager.RegisterMethodDelegate<System.UInt16, System.Byte[]>();

                m_pAppDomain.RegisterCrossBindingAdaptor(new UI.UILogicAdaptor());
                m_pAppDomain.DelegateManager.RegisterFunctionDelegate<UI.UILogicAdaptor.Adaptor>();
                m_pAppDomain.DelegateManager.RegisterMethodDelegate<UI.UIBase>();

                //这里做一些ILRuntime的注册
                m_pAppDomain.DelegateManager.RegisterMethodDelegate<System.Object, ILRuntime.Runtime.Intepreter.ILTypeInstance>();
//                 m_pAppDomain.DelegateManager.RegisterDelegateConvertor<System.EventHandler<ILRuntime.Runtime.Intepreter.ILTypeInstance>>((act) =>
//                 {
//                     return new System.EventHandler<ILRuntime.Runtime.Intepreter.ILTypeInstance>((sender, e) =>
//                     {
//                         ((Action<System.Object, ILRuntime.Runtime.Intepreter.ILTypeInstance>)act)(sender, e);
//                     });
//                 });

                m_pHotFixEntity = m_pAppDomain.Instantiate("TopGame.Hot.HotScriptInstance");
            }

            if (m_pHotFixEntity == null)
                Framework.Plugin.Logger.Break("热更新实例化失败:HotFix.Taurus.HotFixMode");

#elif USE_ASSEMLYDYNAMICLOAD
            if (m_pDll != null && m_pPDB != null)
                m_pAssembly = Assembly.Load(m_pDll.GetBuffer());
            else
            {
                Framework.Plugin.Logger.Warning("HotScripts Load Error!");  
                return false;
            }
            if(m_pAssembly!=null)
                m_pHotFixEntity =m_pAssembly.CreateInstance("HotFix.Taurus.HotFixMode");
#endif
#if USE_ILRUNTIME || USE_ASSEMLYDYNAMICLOAD
            m_pDll = null;
            m_pPDB = null;
#elif USE_HYBRIDCLR
            if(m_pMainAssembly!=null)
            {
                var appType = m_pMainAssembly.GetType("TopGame.AppMain");
                if(appType!=null)
                {
                    var mainMethod = appType.GetMethod("Main");
                    if (mainMethod != null)
                    {
                        bool bResult = (bool)mainMethod.Invoke(null, null);
                        if (!bResult) return false;
                    }
                    else
                        Debug.LogError("unfind app main method!");
                }
                else
                    Debug.LogError("unfind AppMain class type!");
            }
            else
            {
                Debug.LogError("unfind AppMain class type Assembly!");
            }
#endif
            return true;
        }
#if USE_ILRUNTIME || USE_ASSEMLYDYNAMICLOAD
        //------------------------------------------------------
        public List<Type> GetHotTypes
        {
            get
            {
                if (m_vHotFixTypes == null || m_vHotFixTypes.Count == 0)
                {
                    m_vHotFixTypes = new List<Type>();
#if USE_ILRUNTIME
                    if (m_pAppDomain != null)
					{
						foreach (var item in m_pAppDomain.LoadedTypes.Values)
						{
                            m_vHotFixTypes.Add(item.ReflectionType);
						}
					}
#elif USE_ASSEMLYDYNAMICLOAD
                    if (m_pAssembly != null)
                    {
                        m_vHotFixTypes = new List<Type>(m_pAssembly.GetTypes());
                    }
#endif
                }
                return m_vHotFixTypes;
            }
        }
#endif
        //------------------------------------------------------
        public void Exit()
        {
            if(OnDestroyAction != null) OnDestroyAction();
#if USE_INJECTFIX
            m_pMainVM = null;
            m_pScriptVM = null;
#elif USE_HYBRIDCLR
            m_pMainAssembly = null;
#endif
        }
    }

}
