/********************************************************************
生成日期:	10:7:2019   11:49
类    名: 	MessagePool
作    者:	HappLI
描    述:	消息创建池
*********************************************************************/

using System;
using System.Collections.Generic;

namespace TopGame.Net
{
    public class MessagePool
    {
        [System.Serializable]
        struct CodeMessage
        {
            public int code;
            public string content;
            public bool isPb;
        }

        public static T Create<T>() where T : Google.Protobuf.IMessage, new()
        {
            T newPb = new T();
            return newPb;
        }
        public static PacketBase CreatePB<T>(ESessionType type = ESessionType.Tcp) where T : Google.Protobuf.IMessage, new ()
        {
            T newPb = new T();
            PacketBase msg = new PacketBase();
            int midCode = PacketBuilder.getMessageCode(typeof(T));
            msg.SetMsg(midCode, newPb);
            msg.SetSessionType(type);
            return msg;
        }
        public static string ToJson(Proto3.MID code, Google.Protobuf.IMessage msg)
        {
            CodeMessage codeMsg = new CodeMessage();
            codeMsg.code = (int)code;
            codeMsg.content = Google.Protobuf.JsonFormatter.Default.Format(msg);
            codeMsg.isPb = true;
            return UnityEngine.JsonUtility.ToJson(codeMsg,true);
        }
        public static PacketBase ToPackage(string json)
        {
            if (string.IsNullOrEmpty(json)) return PacketBase.DEF;
            try
            {
                PacketBase pkg = new PacketBase();
                CodeMessage codeMessage = UnityEngine.JsonUtility.FromJson<CodeMessage>(json);
                Google.Protobuf.IMessage message =  PacketBuilder.newMessageByCode(codeMessage.code);
                if (message == null) return PacketBase.DEF;
                message = message.Descriptor.Parser.ParseJson(codeMessage.content);
                pkg.SetMsg(codeMessage.code, message);
                return pkg;
            }
            catch(Exception e)
            {
                UnityEngine.Debug.LogWarning(e.ToString());   
            }
            return PacketBase.DEF;
        }
    }
}
