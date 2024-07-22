using System;
using System.Collections.Generic;
/********************************************************************
生成日期:	5:30:2022 10:34
类    名: 	ItemTweenManager
作    者:	hjc
描    述:	道具收集效果管理器
*********************************************************************/

using Framework.Core;
using Framework.Plugin.Guide;
using TopGame.Base;
using TopGame.Core;
using TopGame.Data;
using TopGame.SvrData;
using UnityEngine;

namespace TopGame.UI
{
    public class ItemTweenManager : IItemTweenManager
    {

        public struct ItemData
        {
            public uint id;
            public string path;
            public uint count;
            public int type;
            public IContextData configData;
        }

        private uint MaxNumber = 20;
        public struct TweenItem
        {
            public Transform transform;
            public Vector3 startPos;
            public Vector3 endPos;
            public Vector3 initSpeed;
            public Vector3 a;
            public float duration;
            public void UpdateA(float delay)
            {
                float aDuration = Math.Max(0, duration - delay);
                a = new Vector3( ((endPos.x - startPos.x) - initSpeed.x * duration) / (aDuration * aDuration),
                        ((endPos.y - startPos.y) - initSpeed.y * duration) / (aDuration * aDuration), 0);
            }
        }
        public class Tween : VariablePoolAble
        {
            public InstanceAbleHandler pHandle;
            public InstanceOperiaon pCallback;
            public List<ItemData> list;
            public List<Vector3> positions;
            public ItemTweenCollector collector;
            public float timer;
            public float speedTimer;
            public ItemTween tween;
            public List<TweenItem> items;
            public void Destroy()
            {
                if (pHandle != null) pHandle.RecyleDestroy();
                pHandle = null;
                if (pCallback != null) pCallback.Earse();
                pCallback = null;
                if (list != null) list.Clear();
                list = null;
                if (positions != null) positions.Clear();
                timer = 0;
                speedTimer = 0;
                tween = null;
                if (items != null) 
                {
                    for (int i = 0; i < items.Count; i++)
                    {
                        items[i].transform.gameObject.SetActive(false);
                    }
                    items.Clear();
                }
                items = null;
            }
        }
        public struct CollectorData 
        {
            public ItemTweenCollector collector;
            public Transform parent;
            public List<Tween> tweens;
            public void Destroy()
            {
                collector = null;
                parent = null;
                if (tweens != null)
                {
                    tweens.Clear();
                }
                tweens = null;
            }
        }

        List<Tween> m_vData;
        Dictionary<uint, List<ItemTweenCollector>> m_vCollector;
        List<CollectorData> m_vShowingCollector;
        ObjPool<Tween> m_TweenPool;
        List<Tween> m_vWaitTweening;
        
        //------------------------------------------------------
        public ItemTweenManager()
        {
            m_vData = new List<Tween>();
            m_vCollector = new Dictionary<uint, List<ItemTweenCollector>>();
            m_vShowingCollector = new List<CollectorData>();
            m_TweenPool = new ObjPool<Tween>(null, t => t.Destroy());
            m_vWaitTweening = new List<Tween>();
            switch(GameQuality.Qulity)
            {
                case EGameQulity.High: MaxNumber = 50;break;
                case EGameQulity.Middle: MaxNumber = 30;break;
                default: MaxNumber = 20;break;
            }
            UI.UIBase.OnGlobalHideUI += OnHideUI;
            UI.UIBase.OnGlobalMoveOutUI += OnHideUI;
        }
        //------------------------------------------------------
        private void OnHideUI(UIBase ui)
        {
            for (int i = 0; i < m_vShowingCollector.Count; i++)
            {
                if (m_vShowingCollector[i].collector.UIType == ui.GetUIType())
                {
                    for (int j = 0; j < m_vShowingCollector[i].tweens.Count; j++)
                    {
                        RemoveData(m_vShowingCollector[i].tweens[j]);
                    }
                }
            }
        }
        //------------------------------------------------------
        public void AddCollector(uint itemId, ItemTweenCollector collector)
        {
            List<ItemTweenCollector> list;
            if (!m_vCollector.TryGetValue(itemId, out list))
            {
                list = new List<ItemTweenCollector>();
                m_vCollector.Add(itemId, list);
            }
            list.Add(collector);
        }
        //------------------------------------------------------
        public void RemoveCollector(uint itemId, ItemTweenCollector collector)
        {
            List<ItemTweenCollector> list;
            if (!m_vCollector.TryGetValue(itemId, out list))
            {
                return;
            }
            list.Remove(collector);
        }
        //------------------------------------------------------
        public ItemTweenCollector GetCollector(ItemData item)
        {
            ItemTweenCollector collector = null;
            if (m_vCollector.ContainsKey(item.id))
            {
                var list = m_vCollector[item.id];
                if (list.Count > 0)
                {
                    for (int i = list.Count - 1; i >= 0; i--)
                    {
                        if (list[i].IsVisible() && 
                            (!collector || list[i].Priority > collector.Priority))
                            collector = list[i];
                    }
                }
            }
            if (collector) return collector;
            if (m_vCollector.ContainsKey(0))
            {
                var list = m_vCollector[0];
                if (list.Count > 0)
                {
                    for (int i = list.Count - 1; i >= 0; i--)
                    {
                        if (list[i].IsVisible() && 
                            (!collector || list[i].Priority > collector.Priority))
                            collector = list[i];
                    }
                }
            }
            return collector;
        }
        //------------------------------------------------------
        private bool IsCheckTweening(Tween tweenData)
        {
            for (int i = 0; i < tweenData.list.Count; i++)
            {
                ItemTweenCollector collector = tweenData.collector ? tweenData.collector : GetCollector(tweenData.list[i]);
                if (collector && GuideUtility.IsCheckTweening(collector.transform))
                {
                    return true;
                }
            }
            return false;
        }
        //------------------------------------------------------
        public void CreateItems(List<ItemData> list, List<Vector3> positions, ItemTweenCollector collector = null)
        {
            string strFile = Data.PermanentAssetsUtil.GetAssetPath(Data.EPathAssetType.ItemTween);
            if (string.IsNullOrEmpty(strFile)) return;
            Tween tween = m_TweenPool.Get();
            tween.list = list;
            tween.positions = positions;
            tween.collector = collector;
            tween.pCallback = FileSystemUtil.SpawnInstance(strFile);
            if (tween.pCallback != null)
            {
                tween.pCallback.userData0 = (VariablePoolAble)tween;
                tween.pCallback.pByParent = UI.UIManager.GetAutoUIRoot();
                tween.pCallback.OnCallback = OnSpawnCallback;
                tween.pCallback.OnSign = OnCallSign;
            }
        }
        //------------------------------------------------------
        public void CreateItem(int itemId, int count, Vector3 position, ItemTweenCollector collector = null)
        {
            if (count <= 0 || itemId == 0) return;
            var pData = Data.DataManager.getInstance().Item.GetData((uint)itemId);
            if (pData == null) return;
            ItemData itemData = new ItemData();
            itemData.count = (uint)count;
            itemData.configData = pData;
            itemData.id = (uint)itemId;
            itemData.path = UIUtil.GetAssetPath(pData.icon);

            CreateItem(itemData, position, collector);
        }
        //------------------------------------------------------
        public void CreateItem(ItemData item, Vector3 position, ItemTweenCollector collector = null)
        {
            string strFile = Data.PermanentAssetsUtil.GetAssetPath(Data.EPathAssetType.ItemTween);
            if (string.IsNullOrEmpty(strFile)) return;
            Tween tween = m_TweenPool.Get();
            if (tween.list == null) tween.list = new List<ItemData>(1);
            tween.list.Add(item);
            if (tween.positions == null) tween.positions = new List<Vector3>();
            tween.positions.Add(position);
            tween.collector = collector;
            tween.pCallback = FileSystemUtil.SpawnInstance(strFile);
            if (tween.pCallback != null)
            {
                tween.pCallback.userData0 = (VariablePoolAble)tween;
                tween.pCallback.pByParent = UI.UIManager.GetAutoUIRoot();
                tween.pCallback.OnCallback = OnSpawnCallback;
                tween.pCallback.OnSign = OnCallSign;
            }
        }
        //------------------------------------------------------
        void OnCallSign(InstanceOperiaon pCb)
        {
            if (pCb.userData0 == null || !(pCb.userData0 is Tween) )
            {
                pCb.bUsed = false;
                return;
            }
            pCb.bUsed = true;
        }
        //------------------------------------------------------
        void OnSpawnCallback(InstanceOperiaon pCb)
        {
            if(pCb.pPoolAble == null) return;
            InstanceAbleHandler handle = pCb.pPoolAble as InstanceAbleHandler;
            ItemTween tween = handle.GetWidget<ItemTween>("ItemTween");
            if(handle == null || pCb.userData0 == null || !(pCb.userData0 is Tween) || tween == null)
            {
                pCb.pPoolAble.RecyleDestroy();
                return;
            }
            Tween tweenData = (Tween)pCb.userData0;
            tweenData.pHandle = handle;
            tweenData.tween = tween;
            if (IsCheckTweening(tweenData))
            {
                m_vWaitTweening.Add(tweenData);
                return;
            }
            ShowTween(tweenData);
        }
        //------------------------------------------------------
        private void ShowTween(Tween tweenData)
        {
            tweenData.pHandle.SetScale(Vector3.one);
            tweenData.pHandle.SetPosition(Vector3.zero, true);
            tweenData.items = new List<TweenItem>();
            long totalNum = 0;
            for (int i = 0; i < tweenData.list.Count; i++)
            {
                totalNum += tweenData.list[i].count;
            }
            int[] numArray = new int[tweenData.list.Count];
            int totalShowNum = 0;
            for (int i = 0; i < tweenData.list.Count; i++)
            {
                int num = (int)Math.Round((float)tweenData.list[i].count / totalNum * MaxNumber);
                num = Math.Min(Math.Max(1, num), (int)tweenData.list[i].count);
                numArray[i] = num;
                totalShowNum += num;
            }
            var children = tweenData.tween.GetChildren(totalShowNum);
            int childrenIndex = 0;
            for (int i = 0; i < tweenData.list.Count; i++)
            {
                ItemData itemData = tweenData.list[i];
                if (string.IsNullOrEmpty(itemData.path)) continue;
                ItemTweenCollector collector = tweenData.collector ? tweenData.collector : GetCollector(itemData);
                if (collector == null) continue;
                Vector3 pos = tweenData.positions[i];
                for (int j = 0; j < numArray[i]; j++)
                {
                    if (childrenIndex >= children.Count) break;
                    UISerialized object1 = children[childrenIndex++];
                    object1.gameObject.SetActive(true);
                    RawImageEx icon = object1.GetWidget<RawImageEx>("Icon");
                    icon.SetAssetByPath(itemData.path, null);
                    RawImageEx qualityFrame = object1.GetWidget<RawImageEx>("QualityFrame");
                 //   Util.SetActive(qualityFrame.gameObject, itemData.itemType == Proto3.ItemTypeCode.Hero);
                    TweenItem tweenItem = new TweenItem();
                    tweenItem.transform = object1.transform;
                    tweenItem.startPos = pos;
                    tweenItem.endPos = collector.GetPosition();
                    tweenItem.duration = tweenData.tween.RandomDuration();
                    tweenItem.initSpeed = tweenData.tween.RandomSpeed();
                    tweenItem.UpdateA(tweenData.tween.Delay);
                    tweenData.items.Add(tweenItem);
                }
                int k = 0;
                for (; k < m_vShowingCollector.Count; k++)
                {
                    if (m_vShowingCollector[k].collector == collector)
                    {
                        break;
                    }
                }
                if (k == m_vShowingCollector.Count)
                {
                    CollectorData data = new CollectorData();
                    data.collector = collector;
                    Transform root = collector.GetShowRoot();
                    data.parent = root.parent;
                    root.SetParent(tweenData.pHandle.transform, true);
                    root.SetAsFirstSibling();
                    data.tweens = new List<Tween>();
                    m_vShowingCollector.Add(data);
                }
                if (!m_vShowingCollector[k].tweens.Contains(tweenData))
                {
                    m_vShowingCollector[k].tweens.Add(tweenData);
                }
            }
            tweenData.timer = 0;
            tweenData.speedTimer = 0;
            m_vData.Add(tweenData);
        }
        //------------------------------------------------------
        public void Update(float fFrame)
        {
            for (int i = 0; i < m_vWaitTweening.Count;)
            {
                Tween tweenData = m_vWaitTweening[i];
                if (IsCheckTweening(tweenData))
                {
                    i++;
                    continue;
                }
                m_vWaitTweening.RemoveAt(i);
                ShowTween(tweenData);
            }
            for (int i = 0; i < m_vData.Count;)
            {
                Tween tween = m_vData[i];

                float timeScale = 1;
                float scale = 1;
                if(tween.tween.SpeedCurve != null || tween.tween.ScaleCurve != null)
                {
                    tween.speedTimer += fFrame;
                    if (tween.tween.SpeedCurve != null)
                    {
                        timeScale = tween.tween.SpeedCurve.Evaluate(tween.speedTimer);
                    }
                    if (tween.tween.ScaleCurve != null)
                    {
                        scale = tween.tween.ScaleCurve.Evaluate(tween.speedTimer);
                    }
                }
                tween.timer += fFrame * timeScale;

                TweenItem item;
                for (int j = 0; j < tween.items.Count;)
                {
                    item = tween.items[j];
                    if (tween.timer >= item.duration)
                    {
                        item.transform.gameObject.SetActive(false);
                        tween.items.RemoveAt(j);
                    }
                    else if (tween.timer >= tween.tween.Delay)
                    {
                        float aDuration = tween.timer - tween.tween.Delay;
                        item.transform.position = item.startPos + (item.initSpeed*tween.timer + item.a*aDuration*aDuration);
                        item.transform.localScale = new Vector3(scale, scale, scale);
                        j++;
                    }
                    else
                    {
                        item.transform.position = item.startPos + (item.initSpeed*tween.timer);
                        item.transform.localScale = new Vector3(scale, scale, scale);
                        j++;
                    }
                }

                if (tween.items.Count <= 0)
                {
                    RemoveData(i);
                }
                else
                {
                    i++;
                }
            }
            for (int j = 0; j < m_vShowingCollector.Count; j++)
            {
                if (m_vShowingCollector[j].tweens.Count == 0)
                {
                    m_vShowingCollector[j].Destroy();
                    m_vShowingCollector.RemoveAt(j);
                    j--;
                }
            }
        }
        //------------------------------------------------------
        public void AddData(Tween tween)
        {
            m_vData.Add(tween);
        }
        //------------------------------------------------------
        private void RemoveData(Tween tween)
        {
            int index = m_vData.IndexOf(tween);
            if (index >= 0) RemoveData(index);
        }
        //------------------------------------------------------
        private void RemoveData(int i)
        {
            Tween tween = m_vData[i];
            for (int j = 0; j < m_vShowingCollector.Count; j++)
            {
                int index = m_vShowingCollector[j].tweens.IndexOf(tween);
                if (index >= 0)
                {
                    m_vShowingCollector[j].tweens.RemoveAt(index);
                    if (m_vShowingCollector[j].tweens.Count > 0)
                    {
                        Transform root = m_vShowingCollector[j].collector.GetShowRoot();
                        root.SetParent(m_vShowingCollector[j].tweens[m_vShowingCollector[j].tweens.Count - 1].pHandle.transform, true);
                        root.SetAsFirstSibling();
                    }
                    else
                    {
                        m_vShowingCollector[j].collector.GetShowRoot().SetParent(m_vShowingCollector[j].parent, true);
                    }
                }
            }
            m_vData.RemoveAt(i);
            m_TweenPool.Release(tween);
        }
        //------------------------------------------------------
        public bool IsPlaying()
        {
            return m_vData.Count > 0;
        }
        //------------------------------------------------------
        public void Destroy()
        {
            for (int i = 0; i < m_vData.Count;++i)
            {
                m_vData[i].Destroy();
            }
            m_vData.Clear();
            m_vCollector.Clear();
            m_vShowingCollector.Clear();
            m_vWaitTweening.Clear();
        }
    }
}