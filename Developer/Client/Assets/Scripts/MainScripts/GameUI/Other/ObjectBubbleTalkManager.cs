/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	ObjectBubbleTalkManager
作    者:	HappLI
描    述:	对象头顶泡泡对话
*********************************************************************/
using TopGame.Core;
using UnityEngine;
using System.Collections.Generic;
using TopGame.Data;
using Framework.Core;
using System;
using TopGame.Logic;

namespace TopGame.UI
{
    public class ObjectBubbleTalkManager
    {
        private readonly int POOL_MAX = 4;

        struct Bubble
        {
            public Transform pTransform;
            public AWorldNode pBindNode;
            public InstanceAbleHandler pHandle;
            public InstanceOperiaon pCallback;
            public BubbleText bubbleText;
            public Vector3 offset;
            public string strText;
            public float autoHide;
            public bool bInited;
            public Action clickCallback;
            public bool flip;

            public void Destroy()
            {
                if (pHandle != null) pHandle.RecyleDestroy();
                pHandle = null;
                if (pCallback != null) pCallback.Earse();
                pCallback = null;
                bubbleText = null;
                pTransform = null;
                bInited = false;
                autoHide = -1;
                strText = null;
                pBindNode = null;
                offset = Vector3.zero;
            }
            //------------------------------------------------------
            public bool IsValid()
            {
                if (pBindNode != null) return !pBindNode.IsDestroy() && !pBindNode.IsKilled();
                return pTransform != null;
            }
            //------------------------------------------------------
            public Vector3 GetPosition()
            {
                if (pBindNode != null) return pBindNode.GetPosition();
                if (pTransform == null) return Vector3.zero;
                return pTransform.position;
            }
        }

        List<Bubble> m_vData;

        private bool m_bEnable = true;
        public ObjectBubbleTalkManager()
        {
            m_vData = new List<Bubble>(4);
        }
        //------------------------------------------------------
        public void Enable(bool bEnable)
        {
            m_bEnable = bEnable;
            if (!m_bEnable)
            {
                Destroy();
            }
        }
        //------------------------------------------------------
        public void CreeateBubbleFile(Transform pTrans, string strFile, string talkText, Vector3 offset, float autoHide = -1, Action clickCallback = null, bool flip = false)
        {
            if (string.IsNullOrEmpty(strFile)) return;
            CreeateBubble(strFile, pTrans, null, talkText, offset, autoHide, clickCallback, flip);
        }
        //------------------------------------------------------
        public void CreeateBubbleFile(AWorldNode pNode, string strFile, string talkText, Vector3 offset, float autoHide = -1, Action clickCallback = null, bool flip = false)
        {
            if (string.IsNullOrEmpty(strFile)) return;
            CreeateBubble(strFile, null, pNode, talkText, offset, autoHide, clickCallback, flip);
        }
        //------------------------------------------------------
        public void CreeateBubble(Transform pTrans, string talkText, Vector3 offset, float autoHide = -1, Action clickCallback = null, bool flip = false)
        {
            if (string.IsNullOrEmpty(talkText)) return;
            string strFile = Data.PermanentAssetsUtil.GetAssetPath(EPathAssetType.BubbleTalk);
            CreeateBubble(strFile, pTrans, null, talkText, offset, autoHide, clickCallback, flip);
        }
        //------------------------------------------------------
        public void CreeateBubble(AWorldNode pNode, string talkText, Vector3 offset, float autoHide = -1, Action clickCallback = null, bool flip = false)
        {
            if (string.IsNullOrEmpty(talkText)) return;
            string strFile = Data.PermanentAssetsUtil.GetAssetPath(EPathAssetType.BubbleTalk);
            CreeateBubble(strFile, null, pNode, talkText, offset, autoHide, clickCallback, flip);
        }
        //------------------------------------------------------
        void CreeateBubble(string strFile, Transform pTrans, AWorldNode pBindNode, string talkText, Vector3 offset, float autoHide = -1, 
            Action clickCallback = null, bool flip = false)
        {
            if (string.IsNullOrEmpty(strFile)) return;
            if (pTrans == null && pBindNode == null) return;
            Bubble bubble;
            for (int i = 0; i < m_vData.Count; ++i)
            {
                bubble = m_vData[i];
                if(pTrans!=null)
                {
                    if (bubble.pTransform == pTrans)
                    {
                        return;
                    }
                }
                if(pBindNode !=null)
                {
                    if (bubble.pBindNode == pBindNode)
                    {
                        return;
                    }
                }
            }

            bubble = new Bubble();
            bubble.bInited = false;
            bubble.offset = offset;
            bubble.strText = talkText;
            bubble.pTransform = pTrans;
            bubble.pBindNode = pBindNode;
            bubble.autoHide = autoHide;
            bubble.clickCallback = clickCallback;
            bubble.flip = flip;

            bubble.pCallback = FileSystemUtil.SpawnInstance(strFile);
            if(bubble.pCallback!=null)
            {
                bubble.pCallback.userData0 = new VariableTransform() { pTransform = pTrans };
                bubble.pCallback.userData1 = pBindNode;
                bubble.pCallback.pByParent = UI.UIManager.GetAutoUIRoot(1);
                bubble.pCallback.OnCallback = OnSpawnCallback;
                bubble.pCallback.OnSign = OnCallSign;
                m_vData.Add(bubble);
            }
        }
        //------------------------------------------------------
        public void Preload(int cnt)
        {
            string strFile = Data.PermanentAssetsUtil.GetAssetPath(EPathAssetType.BubbleTalk);
            if (!string.IsNullOrEmpty(strFile))
            {
                for(int i = 0; i < cnt; ++i)
                    FileSystemUtil.PreSpawnInstance(strFile);
            }
        }
        //------------------------------------------------------
        void OnCallSign(InstanceOperiaon pCb)
        {
            if (!m_bEnable || (pCb.userData0 == null && pCb.userData1 == null) )
            {
                pCb.bUsed = false;
                return;
            }
            Transform pTrans = null;
            AWorldNode pBindNode = null;
            if (pCb.userData0!=null && pCb.userData0 is VariableTransform)
            {
                pTrans = ((VariableTransform)pCb.userData0).pTransform;
            }
            if (pCb.userData1 != null && pCb.userData1 is AWorldNode)
            {
                pBindNode = ((AWorldNode)pCb.userData1);
            }
            pCb.bUsed = false;
            if (pTrans == null && pBindNode == null) return;
            for (int i = 0; i < m_vData.Count; ++i)
            {
                if (m_vData[i].pTransform == pTrans)
                {
                    pCb.bUsed = true;
                    break;
                }
                if (m_vData[i].pBindNode == pBindNode)
                {
                    pCb.bUsed = true;
                    break;
                }
            }
        }
        //------------------------------------------------------
        void OnSpawnCallback(InstanceOperiaon pCb)
        {
            if(pCb.pPoolAble != null)
            {
                InstanceAbleHandler handle = pCb.pPoolAble as InstanceAbleHandler;
                if(handle == null || (pCb.userData0 == null && pCb.userData1 == null) || handle.GetWidget<BubbleText>("bubbleTalk") == null)
                {
                    pCb.pPoolAble.RecyleDestroy();
                    return;
                }

                Transform pTrans = null;
                AWorldNode pBindNode = null;
                if (pCb.userData0 != null && pCb.userData0 is VariableTransform)
                {
                    pTrans = ((VariableTransform)pCb.userData0).pTransform;
                }
                if (pCb.userData1 != null && pCb.userData1 is AWorldNode)
                {
                    pBindNode = ((AWorldNode)pCb.userData1);
                }
                bool bHas = false;
                Bubble bubble;
                for (int i = 0; i < m_vData.Count; ++i)
                {
                    bubble = m_vData[i];
                    if (bubble.pTransform == pTrans && bubble.pBindNode == pBindNode)
                    {
                        bubble.pCallback = null;
                        bubble.bInited = true;
                        bubble.pHandle = handle;
                        handle.SetScale(Vector3.one);
                        bubble.bubbleText = handle.GetWidget<BubbleText>("bubbleTalk");
                        bubble.bubbleText.SetText(bubble.strText, bubble.autoHide);
                        if(handle.transform.parent)
                        {
                            handle.transform.SetSiblingIndex(0);
                        }
                        if (bubble.clickCallback != null)
                        {
                            EventTriggerListener listener = handle.GetWidget<EventTriggerListener>("click");
                            if (listener != null)
                            {
                                Action action = bubble.clickCallback;
                                listener.onClick = (g, p)=>
                                {
                                    action?.Invoke();
                                };
                            }
                        }
                        RectTransform root = handle.GetWidget<RectTransform>("Root");
                        if (root)
                        {
                            root.localScale = bubble.flip ? new Vector3(-1, 1, 1) : Vector3.one;
                        }
//                         UnityEngine.UI.Button dialog = handle.GetWidget<UnityEngine.UI.Button>("dialog");
//                         if (dialog != null)
//                         {
//                             dialog.onClick.AddListener(() =>
//                             {
//                                 UI.UIManager.ShowUI(EUIType.LittleGamePanel);
//                             });
//                         }
                        m_vData[i] = bubble;
                        bHas = true;
                        break;
                    }
                }
                if(!bHas)
                {
                    pCb.pPoolAble.RecyleDestroy();
                }
            }
        }
        //------------------------------------------------------
        public void Destroy()
        {
            for (int i = 0; i < m_vData.Count;++i)
            {
                m_vData[i].Destroy();
            }
            m_vData.Clear();
        }
        //------------------------------------------------------
        public void Update(float fFrame)
        {
            if (!m_bEnable) return;
            Bubble bubble;
            Vector3 uiPos = Vector3.zero;
            bool bLocal = false;
            for (int i = 0; i < m_vData.Count;)
            {
                bubble = m_vData[i];
                if(!bubble.IsValid())
                {
                    bubble.Destroy();
                    m_vData.RemoveAt(i);
                    continue;
                }
                if(!bubble.bInited)
                {
                    ++i;
                    continue;
                }
                if (bubble.pHandle == null || bubble.bubbleText == null || bubble.pHandle.IsRecyled() || !bubble.pHandle.isActiveAndEnabled)
                {
                    bubble.Destroy();
                    m_vData.RemoveAt(i);
                    continue;
                }
                if (!bubble.bubbleText.IsShowing())
                {
                    bubble.Destroy();
                    m_vData.RemoveAt(i);
                }
                else
                {
                    ++i;
                    if(UI.UIUtil.CanShowDynamicMarker())
                    {
                        if(bubble.pTransform!=null && bubble.pTransform is RectTransform)
                        {
                            bubble.bubbleText.transform.position = bubble.pTransform.position;
                        }
                        else
                        {
                            if (UI.UIKits.WorldPosToUIPos(bubble.GetPosition() + bubble.offset, bLocal, ref uiPos))
                            {
                                Transform pBubbleTrans = bubble.bubbleText.transform;
                                pBubbleTrans.position = uiPos;
                            }
                        }
                    }
                    else
                    {
                        bubble.bubbleText.transform.position = Framework.Core.CommonUtility.INVAILD_POS;
                    }
                }
            }
        }
        //------------------------------------------------------
        public void ClearByNode(AWorldNode pNode)
        {
            if (pNode == null) return;
            for (int i = 0; i < m_vData.Count;)
            {
                if (m_vData[i].pBindNode == pNode)
                {
                    m_vData[i].Destroy();
                    m_vData.RemoveAt(i);
                    continue;
                }
                i++;
            }
        }
        //------------------------------------------------------
        public void ClearByTransform(Transform transform)
        {
            for (int i = 0; i < m_vData.Count;)
            {
                if(m_vData[i].pTransform == transform)
                {
                    m_vData[i].Destroy();
                    m_vData.RemoveAt(i);
                    continue;
                }
                i++;
            }
        }
        //------------------------------------------------------
        public InstanceAbleHandler GetHandler(AWorldNode node)
        {
            for (int i = 0; i < m_vData.Count; i++)
            {
                if(m_vData[i].pBindNode == node)
                {
                    return m_vData[i].pHandle;
                }
            }
            return null;
        }
    }
}
