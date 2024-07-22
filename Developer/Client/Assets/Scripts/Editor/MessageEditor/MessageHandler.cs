using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace TopGame.ED
{
    public class MessageHandler
    {
        struct Message
        {
            public int nCode;
            public string ClassName;
        }
        static string PacketBuilderPath = Application.dataPath + "/Scripts/MainScripts/NetWork/AutoCode/NetHandlerRegister.cs";
        static string PacketLockBuilderPath = Application.dataPath + "/Scripts/MainScripts/NetWork/AutoCode/NetPacketLocker.cs";
        //------------------------------------------------------
        public static void DoBuilder()
        {
            string root = System.IO.Path.GetDirectoryName(PacketBuilderPath);
            if (!Directory.Exists(root)) Directory.CreateDirectory(root);

            Dictionary<Proto3.MID, HashSet<Proto3.MID>> vLockers = new Dictionary<Proto3.MID, HashSet<Proto3.MID>>();
            HashSet<MethodInfo> vMethods = new HashSet<MethodInfo>();
            Assembly assembly = null;
            foreach (var ass in System.AppDomain.CurrentDomain.GetAssemblies())
            {
                assembly = ass;
                Type[] types = assembly.GetTypes();
                for (int i = 0; i < types.Length; ++i)
                {
                    Type tp = types[i];
                    if (tp.IsDefined( typeof(Net.NetHandlerAttribute) ))
                    {
                        MethodInfo[] meths = types[i].GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
                        for (int m = 0; m < meths.Length; ++m)
                        {
                            if (meths[m].IsDefined(typeof(Net.NetResponseAttribute), false))
                            {
                                vMethods.Add(meths[m]);
                            }
                            if (meths[m].IsDefined(typeof(Net.NetLockAttribute), false))
                            {
                                Net.NetLockAttribute[] locker = (Net.NetLockAttribute[])meths[m].GetCustomAttributes<Net.NetLockAttribute>();
                                for (int j = 0; j < locker.Length; ++j)
                                {
                                    HashSet<Proto3.MID> vSets;
                                    if (!vLockers.TryGetValue(locker[j].lockID, out vSets))
                                    {
                                        vSets = new HashSet<Proto3.MID>();
                                        vLockers.Add(locker[j].lockID, vSets);
                                    }
                                    vSets.Add(locker[j].unlockID);
                                }
  
                            }
                        }
                    }
                }
            }

            //handler
            {
                string code = "";
                code += "/********************************************************************\n";
                code += "作    者:	" + "自动生成" + "\n";
                code += "描    述:\n";
                code += "*********************************************************************/\n";

                code += "namespace TopGame.Net\n";
                code += "{\n";
                code += "\tpublic static class NetHandlerRegister\n";
                code += "\t{\n";
                code += "\t\tpublic static void Init(NetHandler handler)\n";
                code += "\t\t{\n";


                HashSet<Proto3.MID> vSets = new HashSet<Proto3.MID>();
                foreach (var method in vMethods)
                {
                    Net.NetResponseAttribute attr = (Net.NetResponseAttribute)method.GetCustomAttribute(typeof(Net.NetResponseAttribute));
                    if (attr == null) continue;

                    if (vSets.Contains(attr.mid))
                    {
                        Debug.LogWarning("具有两个相同的消息码,请检查:[mid=" + attr.mid + "]" + method.DeclaringType.FullName + "::" + method.Name);
                        continue;
                    }

                    vSets.Add(attr.mid);
                    code += "\t\t\thandler.Register((int)Proto3.MID." + attr.mid + "," + method.DeclaringType.FullName + "." + method.Name + ");\r\n";
                }
                code += "\t\t}\n";
                code += "\t}\n";
                code += "}\n";

                if (File.Exists(PacketBuilderPath))
                {
                    File.Delete(PacketBuilderPath);
                }
                FileStream fs = new FileStream(PacketBuilderPath, FileMode.OpenOrCreate);
                StreamWriter writer = new StreamWriter(fs, System.Text.Encoding.UTF8);
                writer.Write(code);
                writer.Close();
            }

            //locker
            {
                string code = "";
                code += "/********************************************************************\n";
                code += "作    者:	" + "自动生成" + "\n";
                code += "描    述:\n";
                code += "*********************************************************************/\n";

                code += "namespace TopGame.Net\n";
                code += "{\n";
                code += "\tpublic partial class NetWork\n";
                code += "\t{\n";

                //unlock
                code += "\t\tpublic int GetLockSize()\n";
                code += "\t\t{\n";
                code += "\t\t\treturn m_vLockMsgs.Count;\r\n";
                code += "\t\t}\n";

                code += "\t\tpublic bool IsTimerLock(int mid, long time = 30000000)\n";
                code += "\t\t{\n";
                code += "\t\t\tlong lastTime;\r\n";
                code += "\t\t\tif (m_vTimerLockMsgs.TryGetValue(mid, out lastTime) && System.DateTime.Now.Ticks - lastTime <= time) return true;\r\n";
                code += "\t\t\treturn false;\r\n";
                code += "\t\t}\n";

                //lock
                code += "\t\tpublic void LockPacket(int mid)\n";
                code += "\t\t{\n";
                code += "\t\t\tswitch(mid)\r\n";
                code += "\t\t\t{\r\n";
                foreach (var method in vLockers)
                {
                    code += "\t\t\t\tcase " + (int)method.Key + ":{m_vLockMsgs.Add(" + (int)method.Key + "); m_vTimerLockMsgs[mid] = System.DateTime.Now.Ticks; return;}//" + method.Key + "\r\n";
                }
                code += "\t\t\t}\r\n";
                code += "\t\t}\n";

                Dictionary<Proto3.MID, HashSet<Proto3.MID>> vUnlock = new Dictionary<Proto3.MID, HashSet<Proto3.MID>>();
                foreach (var method in vLockers)
                {
                    foreach (var ul in method.Value)
                    {
                        HashSet<Proto3.MID> vSet;
                        if(!vUnlock.TryGetValue(ul, out vSet))
                        {
                            vSet = new HashSet<Proto3.MID>();
                            vUnlock.Add(ul, vSet);
                        }
                        vSet.Add(method.Key);
                    }
                }
                //unlock
                code += "\t\tpublic void UnLockPacket(int mid)\n";
                code += "\t\t{\n";
                code += "\t\t\tm_vTimerLockMsgs.Remove(mid);\r\n";
                code += "\t\t\tswitch(mid)\r\n";
                code += "\t\t\t{\r\n";
                foreach (var method in vUnlock)
                {
                    code += "\t\t\t\tcase " + (int)method.Key + "://" + method.Key + "\r\n";
                    foreach (var ul in method.Value)
                    {
                        code += "\t\t\t\tm_vLockMsgs.Remove(" + (int)ul + ");//" + ul + ";\r\n";
                        code += "\t\t\t\tm_vTimerLockMsgs.Remove(" + (int)ul + ");\r\n";
                    }
                    code += "\t\t\t\tbreak;\r\n";
                }
                code += "\t\t\t}\r\n";

                code += "\t\t}\n";

                code += "\t}\n";
                code += "}\n";

                if (File.Exists(PacketLockBuilderPath))
                {
                    File.Delete(PacketLockBuilderPath);
                }
                FileStream fs = new FileStream(PacketLockBuilderPath, FileMode.OpenOrCreate);
                StreamWriter writer = new StreamWriter(fs, System.Text.Encoding.UTF8);
                writer.Write(code);
                writer.Close();
            }
        }
    }

}

