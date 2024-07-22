/********************************************************************
生成日期:	10:7:2019   11:49
类    名: 	INetWork
作    者:	HappLI
描    述:	网络消息
*********************************************************************/


namespace TopGame.Net
{
    public interface INetWork
    {
        AServerSession Connect(ESessionType type, string strIp, int nPort, System.Action<AServerSession> onCallback = null, long localConn=0);
        void Disconnect(ESessionType type, int index =0);
        bool SendMessage(PacketBase pMsg);
        bool SendMessage(ESessionType type, int nCode, Google.Protobuf.IMessage pMsg, bool isCheckNet = true);
        void OnSessionState(AServerSession serverSession, ESessionState eState);
        void OnReConnect(Framework.Core.VariablePoolAble userData = null);
        ESessionState GetSessionState(ESessionType type, int index=0);
        void Register(int code, OnRevicePacketMsg onHandler);
        void UnRegister(int code);
        void OnPackage(PacketBase pMsg);
        void SetSeed(uint nSeed);
        void ResetReceiveMessageTime(float fLastReceiveMessageTime);
    }
}
