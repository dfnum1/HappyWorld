using System;
using System.Collections.Generic;
/********************************************************************
生成日期:	5:30:2022 10:34
类    名: 	IItemTweenManager
作    者:	hjc
描    述:	道具收集效果管理器
*********************************************************************/

using Framework.Core;
using TopGame.Core;
using UnityEngine;

namespace TopGame.UI
{
    public interface IItemTweenManager
    {
        void AddCollector(uint itemId, ItemTweenCollector collector);
        void RemoveCollector(uint itemId, ItemTweenCollector collector);
        void CreateItem(int itemId, int count, Vector3 position, ItemTweenCollector collector = null);
        void Update(float fFrameTime);
        void Destroy();
    }
    public class ItemCollectUtil
    {
        //------------------------------------------------------
        public static IItemTweenManager getMgr()
        {
            if (Framework.Module.ModuleManager.mainFramework == null) return null;
            GameFramework gameFramework = Framework.Module.ModuleManager.mainFramework as GameFramework;
            if (gameFramework != null) return gameFramework.itemTweenMgr;
            return null;
        }
        //------------------------------------------------------
        internal static void AddCollector(uint itemId, ItemTweenCollector collector)
        {
            IItemTweenManager mgr = getMgr();
            if (mgr == null) return;
            mgr.AddCollector(itemId, collector);
        }
        //------------------------------------------------------
        internal static void RemoveCollector(uint itemId, ItemTweenCollector collector)
        {
            IItemTweenManager mgr = getMgr();
            if (mgr == null) return;
            mgr.RemoveCollector(itemId, collector);
        }
        //------------------------------------------------------
        public static void CreateTween(int itemId, int count, Vector3 position, ItemTweenCollector collector = null)
        {
            IItemTweenManager mgr = getMgr();
            if (mgr == null) return;
            mgr.CreateItem(itemId, count, position, collector);
        }
    }

}