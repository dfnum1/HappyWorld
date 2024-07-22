/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	AStateAPIFlag
作    者:	HappLI
描    述:	状态ipi 调用标志
*********************************************************************/
namespace TopGame.Logic
{
    public enum EAPICallType
    {
        None = 0,
        Awake = 1 << 0,
        PreStart = 1 << 1,
        Start = 1 << 2,
        Destroy = 1 << 3,
        AllNoDestroy = Awake | PreStart | Start,
        All = Awake | PreStart | Start | Destroy,
    }
    public class AAPICallFlag
    {
        private uint m_nAPIFlag = 0;
        protected AAPICallFlag()
        {
            m_nAPIFlag = 0;
        }
        //------------------------------------------------------
        public void resetAPICalled()
        {
            m_nAPIFlag = 0;
        }
        //------------------------------------------------------
        public bool isAPICalled(EAPICallType type)
        {
            return (m_nAPIFlag & ((uint)type)) != 0;
        }
        //------------------------------------------------------
        protected void setAPICalled(EAPICallType type)
        {
            m_nAPIFlag |= (uint)type;
        }
        //------------------------------------------------------
        protected void unAPICalled(EAPICallType type)
        {
            m_nAPIFlag &= ~(uint)type;
        }
    }
}
