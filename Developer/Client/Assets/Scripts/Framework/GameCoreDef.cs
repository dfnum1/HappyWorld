/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	GameCoreDef
作    者:	HappLI
描    述:	
*********************************************************************/
using Framework.Core;

namespace TopGame.Core
{
    public interface IAnimPathCallback
    {
        void OnAnimPathBegin(IPlayableBase playAble);
        void OnAnimPathEnd(IPlayableBase playAble);
        void OnAnimPathUpdate(IPlayableBase playAble);
    }
    public interface IParticleCallback
    {
        void OnParticleStop(AInstanceAble pObject);
    }
    public interface ITimelineCallback
    {
        void OnTimelineStop(AInstanceAble pObject);
    }
    public interface ISoundCallback
    {
        void OnSoundChannelStop(uint pAudio);
    }

    public interface ITweenEffectCallback
    {
        void OnTweenEffectCompleted(VariablePoolAble pAble);
    }
}

