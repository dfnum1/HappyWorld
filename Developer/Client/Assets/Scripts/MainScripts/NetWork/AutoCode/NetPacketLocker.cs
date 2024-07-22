/********************************************************************
作    者:	自动生成
描    述:
*********************************************************************/
namespace TopGame.Net
{
	public partial class NetWork
	{
		public int GetLockSize()
		{
			return m_vLockMsgs.Count;
		}
		public bool IsTimerLock(int mid, long time = 30000000)
		{
			long lastTime;
			if (m_vTimerLockMsgs.TryGetValue(mid, out lastTime) && System.DateTime.Now.Ticks - lastTime <= time) return true;
			return false;
		}
		public void LockPacket(int mid)
		{
		}
		public void UnLockPacket(int mid)
		{
		}
	}
}
