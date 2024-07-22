/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	StateAttribute
作    者:	HappLI
描    述:	游戏状态属性
*********************************************************************/

using System;
namespace TopGame.Logic
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ModeBetweenStateCallFlagAttribute : Attribute
    {
        public EMode mode;
        public EGameState state;
        public EGameState toState;
        public ModeBetweenStateCallFlagAttribute(EMode mode, EGameState state, EGameState toState)
        {
            this.mode = mode;
            this.state = state;
            this.toState = toState;
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class StateClearFlagAttribute : Attribute
    {
        public EGameState state;
        public EGameState toState;
        public uint clearFlag = (uint)EStateChangeFlag.All;
        public StateClearFlagAttribute(EGameState state, EGameState toState, EStateChangeFlag clearFlag = EStateChangeFlag.All)
        {
            this.state = state;
            this.toState = toState;
            this.clearFlag = (uint)clearFlag;
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class StateLogicAttribute : Attribute
    {
        public EGameState state;
        public StateLogicAttribute(EGameState state)
        {
            this.state = state;
        }
    }
    //------------------------------------------------------
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class StateAttribute : Attribute
    {
        public EGameState state;
        public ushort nSceneID;
        public bool deltaChange = true;
        public StateAttribute( EGameState state, ushort nSceneID = 0, bool deltaChange= true)
        {
            this.deltaChange = deltaChange;
            this.state = state;
            this.nSceneID = nSceneID;
        }
    }
    //------------------------------------------------------
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ModeAttribute : Attribute
    {
        public EGameState state;
        public EMode mode;
        public ushort sceneId;
        public Base.ELoadingType loadingType;
        public ModeAttribute(EGameState state, EMode mode = EMode.None, ushort sceneId = 0, Base.ELoadingType loadingType  = Base.ELoadingType.ModeTransition)
        {
            this.state = state;
            this.mode = mode;
            this.sceneId = sceneId;
            this.loadingType = loadingType;
        }
    }
    //------------------------------------------------------
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ModeLogicAttribute : Attribute
    {
        public EMode mode;
        public int priority;
        public bool bEditor = false;
        public ModeLogicAttribute(EMode mode, int order = 0, bool bEditor= false)
        {
            this.mode = mode;
            this.priority = order;
            this.bEditor = bEditor;
        }
    }
}
