#if UNITY_EDITOR
/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	StateCodeGenerator
作    者:	HappLI
描    述:	游戏状态码对应代码生成
*********************************************************************/

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;
using System.IO;

namespace TopGame.Logic
{
    public class StateCodeGenerator
    {
        class ModeLogicData
        {
            public int priority;
            public Type type;
            public bool bEditor;
        }
        class StateLogicData
        {
            public int priority;
            public Type type;
        }
        class ModeData
        {
            public EGameState state;
            public EMode mode;
            public ushort SceneID;
            public Type type;
            public bool actived;
            public Base.ELoadingType loadingType;

            public List<ModeLogicData> logics = new List<ModeLogicData>();
        }

        class StateData
        {
            public Type type;
            public StateClearFlagAttribute clearFlagsAttr;
            public StateAttribute stateAttr;
            public List<StateLogicData> logics = new List<StateLogicData>();
        }

        public static string GeneratorCode = "/Scripts/MainScripts/GameLogic/States/Base/StateFactory_Mapping.cs";
        public static void Build()
        {
            Assembly assembly = null;
            foreach (var ass in System.AppDomain.CurrentDomain.GetAssemblies())
            {
                if (ass.GetName().Name == "MainScripts")
                {
                    assembly = ass;
                    List<Type> vShareResources = new List<Type>();
                    Dictionary<int, ModeBetweenStateCallFlagAttribute> vModeClears = new Dictionary<int, ModeBetweenStateCallFlagAttribute>();
                    Dictionary<EMode, ModeData> vModes = new Dictionary<EMode, ModeData>();
                    Dictionary<EGameState, StateData> vState = new Dictionary<EGameState, StateData>();
                    Type[] types = assembly.GetTypes();
                    for (int i = 0; i < types.Length; ++i)
                    {
                        Type tp = types[i];
                        if(tp.GetInterface(typeof(AStateShareResource).FullName)!=null)
                        {
                            vShareResources.Add(tp);
                            continue;
                        }
                        if (tp.IsDefined(typeof(StateAttribute), false))
                        {
                            StateAttribute[] attr = (StateAttribute[])tp.GetCustomAttributes(typeof(StateAttribute), false);
                            for(int j =0; j < attr.Length; ++j)
                            {
                                if (attr[j].state < EGameState.Count)
                                {
                                    if (vState.ContainsKey(attr[j].state))
                                    {
                                        Debug.LogError("出现重复绑定状态:" + tp.FullName);
                                        continue;
                                    }
                                    StateData attrData = new StateData();
                                    attrData.type = tp;
                                    attrData.stateAttr = attr[j];
                                    attrData.clearFlagsAttr = (StateClearFlagAttribute)tp.GetCustomAttribute(typeof(StateClearFlagAttribute), false);
                                    vState.Add(attr[j].state, attrData);
                                }
                            }
         
                        }
                        else if (tp.IsDefined(typeof(ModeAttribute), false))
                        {
                            ModeAttribute[] attr = (ModeAttribute[])tp.GetCustomAttributes(typeof(ModeAttribute), false);
                            for(int j = 0; j < attr.Length; ++j)
                            {
                                if (attr[j].state < EGameState.Count)
                                {
                                    ModeData st = new ModeData();
                                    st.state = attr[j].state;
                                    st.mode = attr[j].mode;
                                    st.type = tp;
                                    st.loadingType = attr[j].loadingType;
                                    st.SceneID = attr[j].sceneId;
                                    vModes[st.mode] = st;
                                }
                            }

                            if (tp.IsDefined(typeof(ModeBetweenStateCallFlagAttribute), false))
                            {
                                ModeBetweenStateCallFlagAttribute[] modeClears = (ModeBetweenStateCallFlagAttribute[])tp.GetCustomAttributes(typeof(ModeBetweenStateCallFlagAttribute), false);
                                for (int j = 0; j < modeClears.Length; ++j)
                                {
                                    int key = ((int)modeClears[j].mode)<<16|(((int)modeClears[j].state)<<8|(int)modeClears[j].toState);
                                    vModeClears[key] = modeClears[j];
                                }
                            }
                        }
                    }
                    //build logics
                    for (int i = 0; i < types.Length; ++i)
                    {
                        Type tp = types[i];
                        if (tp.GetInterface(typeof(AStateShareResource).FullName) != null)
                        {
                            continue;
                        }
                        if (tp.IsDefined(typeof(ModeLogicAttribute), false))
                        {
                            ModeLogicAttribute[] logis =  (ModeLogicAttribute[])tp.GetCustomAttributes<ModeLogicAttribute>(false);
                            for(int j = 0; j < logis.Length; ++j)
                            {
                                if(logis[j].mode != EMode.None)
                                {
                                    ModeData mode;
                                    if(vModes.TryGetValue(logis[j].mode, out mode))
                                    {
                                        ModeLogicData data = new ModeLogicData();
                                        data.priority = logis[j].priority;
                                        data.type = tp;
                                        data.bEditor = logis[j].bEditor;

                                        mode.logics.Add(data);
                                        mode.logics.Sort((x, y) => (int)(x.priority - y.priority));
                                    }
                                }
                            }
                        }
                        if (tp.IsDefined(typeof(StateLogicAttribute), false))
                        {
                            StateLogicAttribute[] logis = (StateLogicAttribute[])tp.GetCustomAttributes<StateLogicAttribute>(false);
                            for (int j = 0; j < logis.Length; ++j)
                            {
                                StateLogicData data = new StateLogicData();
                              //  data.priority = logis[j].priority;
                                data.type = tp;
                                StateData stateData = null;
                                if (vState.TryGetValue(logis[j].state, out stateData))
                                {
                                    stateData.logics.Add(data);
                                }
                            }
                        }
                    }
                    BuildCode(Application.dataPath + GeneratorCode, vShareResources, vState, vModes, vModeClears, "TopGame.Logic");
                    break;
                }
                //else if (ass.GetName().Name == "MainScripts")
                //{
                //    assembly = ass;
                //    Dictionary<ushort, UIGene> vTypes = new Dictionary<ushort, UIGene>();
                //    Type[] types = assembly.GetTypes();
                //    for (int i = 0; i < types.Length; ++i)
                //    {
                //        Type tp = types[i];
                //        if (tp.IsDefined(typeof(Hot.UI.UIAttribute), false))
                //        {
                //            UIAttribute attr = (UIAttribute)tp.GetCustomAttribute(typeof(UIAttribute), false);
                //            if (attr.uiType > 0)
                //            {
                //                UIGene pui;
                //                if (!vTypes.TryGetValue(attr.uiType, out pui))
                //                {
                //                    pui = new UIGene();
                //                    vTypes.Add(attr.uiType, pui);
                //                }
                //                if (attr.attri == EUIAttr.Logic)
                //                {
                //                    if (pui.Logic != null)
                //                        Debug.LogError("存在相同属性类型的逻辑操作句柄!");
                //                    pui.Logic = tp;
                //                }
                //                else if (attr.attri == EUIAttr.View)
                //                {
                //                    if (pui.View != null)
                //                        Debug.LogError("存在相同属性类型的视图操作句柄!");
                //                    pui.View = tp;
                //                }
                //                else
                //                    Debug.LogError("未定义的界面属性类型");
                //            }
                //        }
                //    }

                //    string genePath = Application.dataPath + "/" + UIGeneratorCode;
                //    BuildCode(genePath, vTypes, "Hot.UI");
                //}
            }
        }
        //------------------------------------------------------
        static void BuildCode(string strPath, List<Type> vShareResources, Dictionary<EGameState, StateData> vState, Dictionary<EMode, ModeData> vModes, Dictionary<int, ModeBetweenStateCallFlagAttribute> vModeClears, string strNameSpace)
        {
            if (File.Exists(strPath))
            {
                File.Delete(strPath);
            }
            string code = "//auto generator\r\n";
            code += "using UnityEngine;\r\n";
            code += "namespace " + strNameSpace + "\r\n";
            code += "{\r\n";
            code += "\tpublic partial class StateFactory \r\n";
            code += "\t{\r\n";
            code += "\t\tstatic EGameState GetState(System.Type type)\r\n";
            code += "\t\t{\r\n";
            foreach(var db in vState)
            {
                code += "\t\t\tif(typeof("+ db.Value.type.FullName + ") == type)" + "\r\n";
                code += "\t\t\t\treturn EGameState." + db.Value.stateAttr.state + ";\r\n";
            }
            code += "\t\t\treturn EGameState.Count;\r\n";
            code += "\t\t}\r\n";

            code += "\t\tstatic System.Type GetType(EGameState state)\r\n";
            code += "\t\t{\r\n";
            foreach (var db in vState)
            {
                code += "\t\t\tif(EGameState." + db.Value.stateAttr.state + " == state)" + "\r\n";
                code += "\t\t\t\treturn typeof(" + db.Value.type.FullName + ");\r\n";
            }
            code += "\t\t\treturn null;\r\n";
            code += "\t\t}\r\n";


            code += "\t\tstatic AState NewState(EGameState state)\r\n";
            code += "\t\t{\r\n";
            foreach (var db in vState)
            {
                code += "\t\t\tif(EGameState." + db.Value.stateAttr.state + " == state)" + "\r\n";
                code += "\t\t\t\treturn new " + db.Value.type.FullName + "();\r\n";
            }
            code += "\t\t\treturn null;\r\n";
            code += "\t\t}\r\n";

            code += "\t\tstatic ushort GetSceneID(EGameState state, EMode mode = EMode.None)\r\n";
            code += "\t\t{\r\n";

            string state_mode_switch = "";
            HashSet<int> vStateMode = new HashSet<int>();
            foreach (var db in vModes)
            {
                int state_mode_hash = ((int)db.Value.state) * 100 + (int)db.Value.mode;
                if (vStateMode.Contains(state_mode_hash)) continue;
                vStateMode.Add(state_mode_hash);
                if (db.Value.SceneID > 0)
                {
                    state_mode_switch += "\t\t\t\t\tcase " + state_mode_hash + ": return " + db.Value.SceneID + ";";
                    state_mode_switch += "//" + db.Value.state + "   " + db.Value.mode + "\r\n";
                }
            }
            if(!string.IsNullOrEmpty(state_mode_switch))
            {
                code += "\t\t\tif(mode != EMode.None)\r\n";
                code += "\t\t\t{\r\n";
                code += "\t\t\t\tint stateModeHash = ((int)state)*100 + (int)mode;\r\n";
                code += "\t\t\t\tswitch(stateModeHash)\r\n";
                code += "\t\t\t\t{\r\n";
                code += state_mode_switch;
                code += "\t\t\t\t}\r\n";
                code += "\t\t\t}\r\n";
            }


            foreach (var db in vState)
            {
                code += "\t\t\tif(EGameState." + db.Value.stateAttr.state + " == state)" + "\r\n";
                code += "\t\t\t\treturn " + db.Value.stateAttr.nSceneID + ";\r\n";
            }
            code += "\t\t\treturn 0;\r\n";
            code += "\t\t}\r\n";

            code += "\t\tstatic uint GetClearFlags(EGameState state,EGameState toState)\r\n";
            code += "\t\t{\r\n";
            code += "\t\t\tint key = ((int)state)<<16|((int)toState);\r\n";
            foreach (var db in vState)
            {
                if(db.Value.clearFlagsAttr != null)
                {
                    int key = (int)db.Value.clearFlagsAttr.state<<16| (int)db.Value.clearFlagsAttr.toState;
                    code += "\t\t\tif(key == " + key + ")";
                    code += "\t\t\t\treturn " + db.Value.clearFlagsAttr.clearFlag + ";\r\n";
                }

            }
            code += "\t\t\treturn (uint)TopGame.Logic.EStateChangeFlag.All;\r\n";
            code += "\t\t}\r\n";

            code += "\t\tpublic static bool DeltaChangeState(EGameState state)\r\n";
            code += "\t\t{\r\n";
            foreach (var db in vState)
            {
                code += "\t\t\tif(EGameState." + db.Value.stateAttr.state + " == state)" + "\r\n";
                code += "\t\t\t\treturn " + db.Value.stateAttr.deltaChange.ToString().ToLower() + ";\r\n";
            }
            code += "\t\t\treturn false;\r\n";
            code += "\t\t}\r\n";

            code += "\t\tpublic static Base.ELoadingType GetModeLoadingType(EMode mode)\r\n";
            code += "\t\t{\r\n";
            code += "\t\t\tswitch(mode)\r\n";
            code += "\t\t\t{\r\n";
            foreach (var db in vModes)
            {
                code += "\t\t\t\tcase EMode." + db.Key + ": return Base.ELoadingType." + db.Value.loadingType.ToString() + ";\r\n";
            }
            code += "\t\t\t}\r\n";
            code += "\t\t\treturn Base.ELoadingType.Loading;\r\n";
            code += "\t\t}\r\n";

            //! RegisterMode
            code += "\t\tvoid RegisterMode(AState pState, EMode mode)\r\n";
            code += "\t\t{\r\n";
            code += "\t\t\tAbsMode pMode = CreateMode(mode);\r\n";
            code += "\t\t\tif(pMode!=null) pState.SwitchMode(pMode);\r\n";
            code += "\t\t}\r\n";

            //! CreateMode
            code += "\t\tAbsMode CreateMode(EMode mode)\r\n";
            code += "\t\t{\r\n";
            code += "\t\t\tswitch(mode)\r\n";
            code += "\t\t\t{\r\n";
            foreach (var db in vModes)
            {
                code += "\t\t\t\tcase EMode." + db.Key.ToString() + ":\r\n";
                code += "\t\t\t\t{" + "\r\n";
                code += "\t\t\t\t\tAbsMode pMode;\r\n";
                code += "\t\t\t\t\tif(!m_vModes.TryGetValue(mode, out pMode) && pMode == null)\r\n";
                code += "\t\t\t\t\t{\r\n";
                code += "\t\t\t\t\t\tpMode = new " + db.Value.type.FullName + "();\r\n";
                code += "\t\t\t\t\t\tpMode.SetMode(EMode." + db.Key + ");\r\n";
                code += "\t\t\t\t\t\tm_vModes.Add(mode,pMode);\r\n";
                code += "\t\t\t\t\t}\r\n";
                for (int j = 0; j < db.Value.logics.Count; ++j)
                {
                    if(db.Value.logics[j].bEditor)
                    {
                        code += "\t\t\t\t\t#if UNITY_EDITOR\r\n";
                        code += "\t\t\t\t\t\tpMode.RegisterLogic<" + db.Value.logics[j].type.FullName + ">();\r\n";
                        code += "\t\t\t\t\t#endif\r\n";
                    }
                    else
                        code += "\t\t\t\t\tpMode.RegisterLogic<" + db.Value.logics[j].type.FullName + ">();\r\n";
                }
                code += "\t\t\t\t\treturn pMode;\r\n";
                code += "\t\t\t\t}" + "\r\n";
            }
            code += "\t\t\t}\r\n";
            code += "\t\t\treturn null;\r\n";
            code += "\t\t}\r\n";

            //! RegisterResource
            code += "\t\tstatic void RegisterStateLogic(AState pState, EGameState state)\r\n";
            code += "\t\t{\r\n";
            code += "\t\t\tswitch(state)\r\n";
            code += "\t\t\t{\r\n";

            foreach (var db in vState)
            {
                if (db.Value.logics.Count <= 0) continue;
                code += "\t\t\t\tcase EGameState." + db.Key.ToString() + ":\r\n";
                code += "\t\t\t\t{" + "\r\n";
                foreach (var ms in db.Value.logics)
                {
                    code += "\t\t\t\t\tpState.GetLogic<" + ms.type.FullName + ">();\r\n";
                }
                code += "\t\t\t\t}" + "\r\n";
                code += "\t\t\t\tbreak;\r\n";
            }
            code += "\t\t\t}\r\n";
            code += "\t\t}\r\n";

            code += "\t}\r\n";
            code += "}\r\n";

            FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate);
            StreamWriter writer = new StreamWriter(fs, System.Text.Encoding.UTF8);
            writer.Write(code);
            writer.Close();
        }
    }
}
#endif