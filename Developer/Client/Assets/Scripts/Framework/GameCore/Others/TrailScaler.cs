/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	TrailScaler
作    者:	HappLI
描    述:	拖尾缩放控制
*********************************************************************/
using UnityEngine;
namespace TopGame.Core
{
    [ExecuteAlways]
    [RequireComponent(typeof(TrailRenderer))]
    public class TrailScaler : MonoBehaviour
    {
        public TrailRenderer trailRender;
        public Transform follow;
#if UNITY_EDITOR
        private void OnEnable()
        {
            if (trailRender == null) trailRender = GetComponent<TrailRenderer>();
        }
#endif
        //------------------------------------------------------
        void Update()
        {
            return;
            if (trailRender)
            {
                Vector3 scale = follow ? follow.localScale:transform.localScale;
                float widthMultiplier = (scale.x + scale.y + scale.z) / 3;
                trailRender.startWidth = widthMultiplier;
                trailRender.endWidth = widthMultiplier;
            }
        }
    }
}
