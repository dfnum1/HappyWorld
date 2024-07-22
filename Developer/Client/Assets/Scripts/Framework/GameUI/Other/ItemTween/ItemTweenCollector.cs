/********************************************************************
生成日期:	5:31:2022 14:45
类    名: 	ItemTweenCollector
作    者:	hjc
描    述:	道具收集目标
*********************************************************************/
using Framework.Core;
using TopGame.Core;
using UnityEngine;
#if UNITY_EDITOR
using Framework.ED;
using UnityEditor;
#endif

namespace TopGame.UI
{
    public class ItemTweenCollector : MonoBehaviour
    {
        // 收集的道具id，0为可收集所有道具，-1为不收集
        public int ItemId;
        public Transform ShowRoot;
#if UNITY_EDITOR
        [Framework.ED.DisplayDrawType("TopGame.UI.EUIType")]
#endif
        public int UIType = 0;
        public int Priority;

        //------------------------------------------------------
        private void OnEnable()
        {
            if (ItemId < 0) return;
            ItemCollectUtil.AddCollector((uint)ItemId, this);
        }
        //------------------------------------------------------
        private void OnDisable()
        {
            if (ItemId < 0) return;
            ItemCollectUtil.RemoveCollector((uint)ItemId, this);
        }
        //------------------------------------------------------
        public void SetItemId(int newId)
        {
            if (!this.gameObject.activeInHierarchy)
            {
                ItemId = newId;
                return;
            }
            if (ItemId >= 0) ItemCollectUtil.RemoveCollector((uint)ItemId, this);
            ItemId = newId;
            if (ItemId >= 0) ItemCollectUtil.AddCollector((uint)ItemId, this);
        }
        //------------------------------------------------------
        public Transform GetShowRoot()
        {
            if (ShowRoot != null) return ShowRoot;
            return this.transform;
        }
        //------------------------------------------------------
        public Vector3 GetPosition()
        {
            return this.transform.position;
        }
        //------------------------------------------------------
        public bool IsVisible()
        {
            if (UIType != 0)
            {
                
                var uiBase = UI.UIKits.CastGetUI<Framework.UI.UIHandle>(false, (ushort)UIType);
                if (uiBase == null || !uiBase.IsVisible() || uiBase.IsMoveOut()) return false;
            }
            return true;
        }
    }
}