//auto generator
using UnityEngine;
namespace TopGame.Logic
{
	public partial class StateFactory 
	{
		static EGameState GetState(System.Type type)
		{
			if(typeof(TopGame.Logic.Battle) == type)
				return EGameState.Battle;
			if(typeof(TopGame.Logic.Hall) == type)
				return EGameState.Hall;
			if(typeof(TopGame.Logic.Login) == type)
				return EGameState.Login;
			return EGameState.Count;
		}
		static System.Type GetType(EGameState state)
		{
			if(EGameState.Battle == state)
				return typeof(TopGame.Logic.Battle);
			if(EGameState.Hall == state)
				return typeof(TopGame.Logic.Hall);
			if(EGameState.Login == state)
				return typeof(TopGame.Logic.Login);
			return null;
		}
		static AState NewState(EGameState state)
		{
			if(EGameState.Battle == state)
				return new TopGame.Logic.Battle();
			if(EGameState.Hall == state)
				return new TopGame.Logic.Hall();
			if(EGameState.Login == state)
				return new TopGame.Logic.Login();
			return null;
		}
		static ushort GetSceneID(EGameState state, EMode mode = EMode.None)
		{
			if(mode != EMode.None)
			{
				int stateModeHash = ((int)state)*100 + (int)mode;
				switch(stateModeHash)
				{
					case 701: return 100;//Battle   PVE
				}
			}
			if(EGameState.Battle == state)
				return 3;
			if(EGameState.Hall == state)
				return 2;
			if(EGameState.Login == state)
				return 1;
			return 0;
		}
		static uint GetClearFlags(EGameState state,EGameState toState)
		{
			int key = ((int)state)<<16|((int)toState);
			if(key == 327685)				return 13;
			if(key == 65539)				return 60;
			return (uint)TopGame.Logic.EStateChangeFlag.All;
		}
		public static bool DeltaChangeState(EGameState state)
		{
			if(EGameState.Battle == state)
				return true;
			if(EGameState.Hall == state)
				return true;
			if(EGameState.Login == state)
				return true;
			return false;
		}
		public static Base.ELoadingType GetModeLoadingType(EMode mode)
		{
			switch(mode)
			{
				case EMode.PVE: return Base.ELoadingType.ModeTransition;
			}
			return Base.ELoadingType.Loading;
		}
		void RegisterMode(AState pState, EMode mode)
		{
			AbsMode pMode = CreateMode(mode);
			if(pMode!=null) pState.SwitchMode(pMode);
		}
		AbsMode CreateMode(EMode mode)
		{
			switch(mode)
			{
				case EMode.PVE:
				{
					AbsMode pMode;
					if(!m_vModes.TryGetValue(mode, out pMode) && pMode == null)
					{
						pMode = new TopGame.Logic.PVE();
						pMode.SetMode(EMode.PVE);
						m_vModes.Add(mode,pMode);
					}
					pMode.RegisterLogic<TopGame.Logic.PVECamera>();
					pMode.RegisterLogic<TopGame.Logic.PVELevel>();
					pMode.RegisterLogic<TopGame.Logic.PVEPlayer>();
					return pMode;
				}
			}
			return null;
		}
		static void RegisterStateLogic(AState pState, EGameState state)
		{
			switch(state)
			{
				case EGameState.Login:
				{
					pState.GetLogic<TopGame.Logic.LoginLogic>();
				}
				break;
			}
		}
	}
}
