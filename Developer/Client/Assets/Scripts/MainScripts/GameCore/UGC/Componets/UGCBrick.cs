/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	UGCBrick
作    者:	HappLI
描    述:	UGC基础积木
*********************************************************************/
using System;
namespace Framework.UGC
{
    public enum EUGCBrickType
    {
        Loop =0,
        Random,
    }
    public interface UGCBrick
    {
        void SetPipeline(UGCPipeline pipeline);
        int GetID();
        int GetType();
        int[] GetNexts();
        int GetParamCount();
        Framework.Core.VariablePoolAble GetParam(int index);
        bool Excude(Framework.Core.VariablePoolAble[] inputParams);

        void DrawUI();
        void Read(ref Framework.Data.BinaryUtil reader);
        void Write(ref Framework.Data.BinaryUtil writer);
    }
}

