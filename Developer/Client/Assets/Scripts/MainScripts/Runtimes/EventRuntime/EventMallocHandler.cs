//auto generator
namespace Framework.Core
{
	public class EventMallocHandler
	{
		public static BaseEventParameter NewEvent(int type)
		{
			switch(type)
			{
			case 188: return new Framework.Core.ChangeTargetElementEventParameter();
			case 70: return new Framework.Core.PostRenderEventParameter();
			case 2: return new Framework.Core.ProjectileEventParameter();
			case 41: return new Framework.Core.PropertyEffectParameter();
			case 60: return new Framework.Core.RenderBlendEventParameter();
			case 8: return new Framework.Core.RenderCurveEventParameter();
			case 401: return new Framework.Core.RenderLayerEventParameter();
			case 10: return new Framework.Core.RunerSpeedEventParameter();
			case 12: return new Framework.Core.SkillCDEventParameter();
			case 40: return new Framework.Core.SkillEventParameter();
			case 1: return new Framework.Core.ParticleEventParameter();
			case 300: return new Framework.Core.ActionStatePropertyEventParameter();
			case 303: return new Framework.Core.SpawnSplineEventParameter();
			case 7: return new Framework.Core.SummonEventParameter();
			case 22: return new Framework.Core.SwapTeamEventParameter();
			case 11: return new Framework.Core.TakeSlotSkillEventParameter();
			case 21: return new Framework.Core.TargetPathEventParameter();
			case 100: return new Framework.Core.TimerLockHitTargetEventParameter();
			case 15: return new Framework.Core.TimeScaleEventParamenter();
			case 50: return new Framework.Core.TrackBindEventParameter();
			case 19: return new Framework.Core.TriggerEventParameter();
			case 3: return new Framework.Core.SoundEventParameter();
			case 16: return new Framework.Core.ObjScaleEventParameter();
			case 302: return new Framework.Core.MoveTypeEventParameter();
			case 301: return new Framework.Core.MoveToTargetEventParameter();
			case 14: return new Framework.Core.ActionSwitchEventParameter();
			case 6: return new Framework.Core.BuffEventParameter();
			case 27: return new Framework.Core.CameraCloseUpEventParameter();
			case 9: return new Framework.Core.CameraCurveEventParameter();
			case 28: return new Framework.Core.CameraEventParameter();
			case 24: return new Framework.Core.CameraOffsetEventParameter();
			case 4: return new Framework.Core.CameraShakeParameter();
			case 23: return new Framework.Core.CameraSplineEventParameter();
			case 20: return new Framework.Core.CameraToggleEventParameter();
			case 17: return new Framework.Core.UIEventParameter();
			case 29: return new Framework.Core.CameraZoomLockEventParameter();
			case 304: return new Framework.Core.FollowToTargetEventParameter();
			case 26: return new Framework.Core.GrabThrowEventParameter();
			case 400: return new Framework.Core.InstanceEventParameter();
			case 13: return new Framework.Core.InvincibleEventParameter();
			case 230: return new Framework.Core.LevelChangeEventParameter();
			case 186: return new Framework.Core.LineLinkEventParameter();
			case 18: return new Framework.Core.LockEventParameter();
			case 5: return new Framework.Core.LockHitTargetEventParameter();
			case 305: return new Framework.Core.LockMovePointEventParameter();
			case 187: return new Framework.Core.DamageLinkEventParameter();
			case 200: return new Framework.Core.VariableEventParameter();
			}
			return null;
		}
	}
}
