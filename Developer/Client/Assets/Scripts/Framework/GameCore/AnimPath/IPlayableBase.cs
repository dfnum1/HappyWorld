/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	基础播放类型
作    者:	HappLI
描    述:	
*********************************************************************/
using Framework.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace TopGame.Core
{
    public enum EPathUseEndType
    {
        [Framework.Data.DisplayNameGUI("使用位置")]
        Position = 1<<0,
        [Framework.Data.DisplayNameGUI("使用朝向")]
        EulerAngle = 1<<1,
        [Framework.Data.DisplayNameGUI("使用广角")]
        Fov = 1<<2,
        [Framework.Data.DisplayNameGUI("将玩家位置设置到跟随点")]
        ReplaceRunPosition = 1 << 3,
        [Framework.Data.DisplayNameGUI("使用玩家位置")]
        UseRunPosition = 1 << 4,

        [Framework.Data.DisplayNameGUI("同步镜头模式")]
        SyncMode = 1 << 5,
    }
    public struct KeyFrame
    {
        public float time;
        public Vector3 position;
        public Vector3 eulerAngle;
        public float fov;

        public static KeyFrame Epsilon = new KeyFrame() { time = -1 };

        public void Init()
        {
            time = -1;
            fov = 0;
            position = eulerAngle = Vector3.zero;
        }

        public bool isValid
        {
            get { return time >= 0; }
        }
    }
    public enum EPlayableCallbackType
    {
        Position,
        EuleraAngle,
        Lookat,
        VecUp,
        FOV,
        FollowPosition,
        Tick,
        End,
    }

    public interface IPlayableCallback
    {
        bool OnTimelineCallback(IPlayableBase palyable, EPlayableCallbackType valueType, Variable3 value);
    }

    public struct PlayableBindSlot
    {
        public string strName;
        public AInstanceAble pAble;
        public Transform pSlot;
        public Framework.Plugin.AT.IUserData pUserAT;

        public TopGame.Timeline.IUserTrackAsset playableAsset;
        public bool bGenericBinding;
        public UnityEngine.Playables.PlayableBinding binding;
        public UnityEngine.Object source;

        public static PlayableBindSlot DEFAULT = new PlayableBindSlot() { pAble = null, pSlot = null, pUserAT = null, playableAsset = null };
    }

    [Framework.Plugin.AT.ATExportMono("路径动画系统/IPlayableBase")]
    public interface IPlayableBase : Framework.Plugin.AT.IUserData
    {
        void Stop();
        void Pause();
        void Resume();
        //   void Destroy();

        float GetPlayTime();
        float GetDuration();
        float GetCanSkipTime();

        bool IsEndCallbacked();
        void SetEndCallbacked(bool bEndCallbacked);

        bool CanSkip();
        void SetSkipTitle(bool bCanSkip, float skipTime);
        void SkipTo(float fSkipTime);
        void SkipDo();
        int GetGuid();
        string GetAssetName();
        void SetAssetName(string strAssetName);
        void SetMirror(bool bMirror);
        void SetCamera(Camera pCamera);
        ushort getUseEnd();
        bool isOver();
        void EnableEvent(bool bEnable);
        void TriggerEvent();
        Transform GetTarget();
        void SetTarget(Transform pTrans);
        Transform GetTrigger();
        void SetTrigger(Transform pTrans);
        KeyFrame GetLastKey();
        bool Update(float fFrame);
        List<BaseEventParameter> GetEvents();
        void SetOffsetPosition(Vector3 pos);
        void SetOffsetEulerAngle(Vector3 eulerAngle);
        Vector3 GetOffsetPosition();
        Vector3 GetOffsetEulerAngle();

        Vector3 GetPosition();
        Vector3 GetEulerAngle();
        Vector3 GetLookAt();
        float GetFov();

        Camera GetCamera();
        Transform GetSlot(int index);
        Transform GetSlot(string strName);

        bool HasBinding(Framework.Plugin.AT.IUserData pAT, string strName = null);
        bool HasBinding(AInstanceAble pAble, string strName = null);
        PlayableBindSlot GetPlayaleSlot(string strName);
        List<PlayableBindSlot> GetPlayableSlots();

        void BindTrackObject(string trackName, AInstanceAble pObj, bool bBinding, Framework.Plugin.AT.IUserData pAT = null);
        void Register(IPlayableCallback callback);
        void UnRegister(IPlayableCallback callback);
    }
}