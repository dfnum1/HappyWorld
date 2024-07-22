using System.Collections.Generic;
using TopGame.Core;
using UnityEngine;
using UnityEngine.Playables;
namespace TopGame.Timeline
{
    public enum EPlayableParamType
    {
        [Framework.Plugin.PluginDisplay("播放动作(技能)")]
        ActorAction  =0,
        [Framework.Plugin.PluginDisplay("击杀")]
        ActorKill = 1,
        [Framework.Plugin.PluginDisplay("移除")]
        ActorDestroy = 2,
        [Framework.Plugin.PluginDisplay("抖动")]
        Shake = 3,
        [Framework.Plugin.PluginDisplay("ShadowDistance")]
        ShadowDistance = 4,

        [Framework.Plugin.DisableGUI]
        Count,
    }
    public interface BasePlayableParam 
    {
        EPlayableParamType GetParamType();
        void Reset();
        bool OnExcude(IUserTrackAsset userTrack, float time, float duration, bool bEditor);
        void OnStart(IUserTrackAsset userTrack);
        void OnStop(IUserTrackAsset userTrack);
        bool FormString(string strCmd);
        void Destroy();
    }

    public class PlayableParamUtl
    {
        public static BasePlayableParam NewParam(string strCmd)
        {
            if (string.IsNullOrEmpty(strCmd)) return null;
            string head = "";
            strCmd = strCmd.Trim();
            for (int i = 0; i < strCmd.Length; ++i)
            {
                if (strCmd[i] == ':')
                {
                    i++;
                    strCmd = strCmd.Substring(i);
                    break;
                }
                if (strCmd[i] == ' ' || strCmd[i] == '\t' || strCmd[i] == '\r' || strCmd[i] == '\n') continue;
                head += strCmd[i];
            }
            if (strCmd == null) return null;
            int headInt = 0;
            if (!int.TryParse(head, out headInt) || headInt < 0 || headInt >= (int)EPlayableParamType.Count) return null;
            BasePlayableParam param = NewParam((EPlayableParamType)headInt);
            if (param != null && param.FormString(strCmd)) return param;
            return null;
        }

        public static BasePlayableParam NewParam(EPlayableParamType type)
        {
            switch(type)
            {
                case EPlayableParamType.ActorAction: return new ActorActionParam();
                case EPlayableParamType.ActorKill: return new ActorKillParam();
                case EPlayableParamType.ActorDestroy: return new ActorDestroyParam();
                case EPlayableParamType.Shake: return new ShakeParam();
                case EPlayableParamType.ShadowDistance: return new ShadowDistanceParam();
            }
            return null;
        }
    }
}