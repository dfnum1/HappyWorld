/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	ILogic
作    者:	HappLI
描    述:	状态模式 ，具体逻辑
*********************************************************************/

using Framework.Core;
using Framework.Logic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;
using Framework.Data;
using Framework.Plugin.AT;
using Framework.BattlePlus;

namespace TopGame.Logic
{
    public enum EModeResult
    {
        None =0,
        Victory = 1,
        Defeated,
        Deuce,

        GM_Victory = 10,
        GM_Defeated,
        GM_Deuce,

        Over = 20,
    }
    public enum EModeStatus : byte
    {
        None = 0,
        In,
        Out,
    }
    public interface IMode : Framework.Plugin.AT.IUserData
    {
        EMode GetMode();
        AState GetState();
        void BindState(AState pState);
        void Awake();
        void PreStart();
        void Start();
        void Update(float fFrame);
        void DrawGizmos();
        void Enable(bool bEnable);
        bool IsEnabled();
        bool IsLocked();
        void RefreshPlayers(int group=0);
        List<Actor> GetPlayers(int group = 0);
        Actor GetCurrentPlayer();
        Wingman GetWingman(int group = 0);
        Actor AddPlayer(byte attackGroup, VariablePoolAble contextData, bool bCheckExist = true);

        bool OnFillPassData(PassCondition passCondition, VariablePoolAble  userData);
        VariablePoolAble GetUserData();
        bool OnNetResponse(Net.PacketBase msg);
        void OnBattleWorldCallback(BattleWorld pWorld, EBattleWorldCallbackType type, VariablePoolAble takeData = null);
        void OnBattleResultCheckEvent(ref EBattleResultStatus current);
        void OnActorAttrChange(Actor pActor, EAttrType type, float fValue, float fOriginal, bool bPlus);
        bool OnLockHitTargetCondition(AWorldNode pTrigger, AWorldNode pTarget, ELockHitType hitType, ELockHitCondition condition, Vector3Int conditionParam);

        void RegisterLogic(AModeLogic logic);
        T RegisterLogic<T>() where T : AModeLogic ,new();
        void UnRegisterLogic(AModeLogic logic);
        T GetLogic<T>() where T : AModeLogic;

        Framework.Module.AFramework GetFramework();
        World GetWorld();
        SvrData.User GetUser();
        uint GetSceneTheme();
        BattleWorld GetBattleWorld();
        T GetBattleLogic<T>() where T : ABattleLogic;
        void EnableBattleLogic<T>(bool bEnable) where T : ABattleLogic;

        bool ExecuteCustom(ushort enterType, IUserData pParam = null);
        bool ExecuteCustom(GameObject pGo, IUserData pParam = null);
        bool ExecuteCustom(string strName, IUserData pParam = null);
        bool ExecuteCustom(int nID, IUserData pParam = null);

        void Preload(string strFile);
        void PreloadInstance(string strFile, int insCnt = 1);

#if UNITY_EDITOR
        string Print();
#endif
    }
}
