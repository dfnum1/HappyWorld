/********************************************************************
生成日期:	12:28:2020 13:16
类    名: 	DragFitImage
作    者:	zdq
描    述:	拖拽适配类型的图片
*********************************************************************/
using System.Collections;
using System.Collections.Generic;
using TopGame.Core;
using Framework.Module;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Framework.Core;
using System;

namespace TopGame.UI
{
    public class DragFitImage : MonoBehaviour, ITouchInput, IPointerDownHandler
    {
        public bool Test;
        public Image FitImg;
        public List<Transform> DragTrasnforms;
        public List<Transform> DragContains;
        [Header("惯性滑动时间")]
        public float m_DragInertiaTime = 0.1f;
        public Vector2 m_Velocity = Vector2.zero;
        [SerializeField]
        [Header("图片边距偏移")]
        Vector2 m_imgBorderLimitOffset = Vector2.zero;
        [Header("惯性最大值")]
        public float m_DragInertiaMaxValue = 30f;
        public bool bEnableDragInertiaLimit = true;

        int m_nTouchID = -1;
        Vector2 m_LastTouch = Vector2.zero;
        bool m_Dragging = false;


        List<Vector3> m_CacheTransformsStartPos = new List<Vector3>();

        Vector2 m_DragDir = Vector2.zero;
        public System.Action OnTouchEndCallback;
        /// <summary>
        /// 惯性结束回调
        /// </summary>
        public System.Action OnInertiaEnd;

        PointerEventData m_PointerEventData;
        Vector2 m_refVelocity = Vector2.zero;


        //------------------------------------------------------
        private void Awake()
        {
            if (FitImg == null)
            {
                FitImg = GetComponent<Image>();
            }
            //Init();
        }
        //------------------------------------------------------
        private void OnEnable()
        {
            Init();
        }
        //------------------------------------------------------
        void Init()
        {
            if (FitImg == null)
            {
                return;
            }
            m_DragDir = Vector2.zero;

            //根据 FitImage 图片的大小进行计算
            Vector2 resolution = UI.UIKits.GetReallyResolution();
            Vector2 imgSize = FitImg.rectTransform.sizeDelta * FitImg.rectTransform.localScale;
            if (imgSize.x > resolution.x && imgSize.y > resolution.y)
            {
                m_DragDir = Vector2.one;
            }
            else if (imgSize.x > resolution.x && imgSize.y < resolution.y)
            {
                m_DragDir = Vector2.right;
            }
            else if (imgSize.x < resolution.x && imgSize.y > resolution.y)
            {
                m_DragDir = Vector2.up;
            }

            SetInputTouchEnable(true);
        }

        //------------------------------------------------------
        //根据界面是否激活设置输入开关
        public void SetInputTouchEnable(bool enable)
        {
            m_Velocity = Vector2.zero;
            if (enable)
            {
                AddTouchInput();
                RecordDragTransformsPos();
            }
            else
            {
                RemoveTouchInput();
                ResetDragTransformsPos();
            }
        }
        //------------------------------------------------------
        void ResetDragTransformsPos()
        {
            for (int i = 0; i < m_CacheTransformsStartPos.Count; i++)
            {
                if (DragTrasnforms[i])
                {
                    DragTrasnforms[i].localPosition = m_CacheTransformsStartPos[i];
                }
            }
            m_CacheTransformsStartPos.Clear();
        }
        //------------------------------------------------------
        void RecordDragTransformsPos()
        {
            m_CacheTransformsStartPos.Clear();
            foreach (var item in DragTrasnforms)
            {
                if (item)
                {
                    m_CacheTransformsStartPos.Add(item.localPosition);
                }
            }
        }
        //------------------------------------------------------
        bool CheckBorder(Image img, Vector2 deltaPos, out Vector2 canOffsetPos, Vector2 imgBorderLimitOffset)
        {
            canOffsetPos = Vector2.zero;
            if (img == null)
            {
                return false;
            }

            //计算坐标是否超出屏幕
            //1.获取屏幕宽度为例
            //2.计算图片当前显示可左右偏移的范围(这边要注意锚点要居中对齐) = (图片宽度 * 缩放 - 屏幕宽度) / 2

            if (img.rectTransform.anchorMin != new Vector2(0.5f, 0.5f) || img.rectTransform.anchorMax != new Vector2(0.5f, 0.5f))
            {
                Framework.Plugin.Logger.Error("图片的锚点不是在中心点,不能进行计算边界操作");
                return false;
            }

            Vector2 resolution = UI.UIKits.GetReallyResolution();

            float screenWidth = resolution.x;
            float imageWidth = img.rectTransform.sizeDelta.x - imgBorderLimitOffset.x;
            float offsetPosX = Mathf.Round((imageWidth * img.rectTransform.localScale.x - screenWidth) / 2);
            if (offsetPosX < 0)//当屏幕超过图片大小后,就不能进行拖动
            {
                offsetPosX = 0;
            }

            float screenHeight = resolution.y;
            float imageHeight = img.rectTransform.sizeDelta.y - imgBorderLimitOffset.y;
            float offsetPosY = Mathf.Round((imageHeight * img.rectTransform.localScale.y - screenHeight) / 2);
            if (offsetPosY < 0)//当屏幕超过图片大小后,就不能进行拖动
            {
                offsetPosY = 0;
            }

            Vector2 offsetMax = new Vector2(offsetPosX, offsetPosY);
            Vector2 offsetMin = new Vector2(-offsetPosX, -offsetPosY);
            if (img.rectTransform.localPosition.x + deltaPos.x > offsetMax.x)
            {
                //计算剩余可以偏移的位置 = 临界值 - 当前值
                canOffsetPos.x = offsetMax.x - img.rectTransform.localPosition.x;
                return false;
            }
            else if (img.rectTransform.localPosition.y + deltaPos.y > offsetMax.y)
            {
                canOffsetPos.y = offsetMax.y - img.rectTransform.localPosition.y;
                return false;
            }
            else if (img.rectTransform.localPosition.x + deltaPos.x < offsetMin.x)
            {
                canOffsetPos.x = offsetMin.x - img.rectTransform.localPosition.x;
                return false;
            }
            else if (img.rectTransform.localPosition.y + deltaPos.y < offsetMin.y)
            {
                canOffsetPos.y = offsetMin.y - img.rectTransform.localPosition.y;
                return false;
            }
            canOffsetPos = deltaPos;

            return true;
        }

        private void LateUpdate()
        {
            if (m_Dragging || Framework.Plugin.Guide.GuideSystem.getInstance().bDoing)
            {
                
                return;
            }

            DragInertia();
        }
        //------------------------------------------------------
        /// <summary>
        /// 拖拽惯性
        /// </summary>
        void DragInertia()
        {
            float deltaTime = Time.unscaledDeltaTime;

            //限制范围
            if (bEnableDragInertiaLimit)
            {
                m_Velocity = Vector2.ClampMagnitude(m_Velocity, m_DragInertiaMaxValue);
                //平滑
                m_Velocity = Vector2.SmoothDamp(m_Velocity, Vector2.zero, ref m_refVelocity, m_DragInertiaTime);
            }
            else
            {
                //设置坐标时,插值移动
                m_Velocity = Vector2.Lerp(m_Velocity, Vector2.zero, 0.5f);
            }
            
            
            

            if (Mathf.Abs(m_Velocity.x) < 1)
            {
                m_Velocity[0] = 0;
            }
            if (Mathf.Abs(m_Velocity.y) < 1)
            {
                m_Velocity[1] = 0;
            }

            if (m_Velocity.magnitude < 0.1f)//过滤掉太小的惯性
            {
                OnInertiaEnd?.Invoke();
                OnInertiaEnd = null;
                return;
            }

            //Debug.Log("m_Velocity:" + m_Velocity.magnitude.ToString() + ",m_refVelocity:" + m_refVelocity);

            Vector2 canOffsetPos = Vector2.zero;
            CheckBorder(FitImg, m_Velocity, out canOffsetPos, m_imgBorderLimitOffset);

            UpdateDragTransformPos(new Vector3(canOffsetPos.x, canOffsetPos.y, 0));
        }
        //------------------------------------------------------
        private void OnDisable()
        {
            RemoveTouchInput();
        }
        //------------------------------------------------------
        private void OnDestroy()
        {
            RemoveTouchInput();
        }
        //------------------------------------------------------
        void AddTouchInput()
        {
            if (Framework.Module.ModuleManager.mainFramework == null) return;
            Framework.Module.ModuleManager.mainFramework.AddTouchInput(this.GetHashCode(),this);//为了让同一个 DragFitImage脚本可以添加多个TouchInput,所以这边使用实例对象的hashcode而不是类的hashcode
        }
        //------------------------------------------------------
        void RemoveTouchInput()
        {
            if (Framework.Module.ModuleManager.mainFramework == null) return;
            Framework.Module.ModuleManager.mainFramework.RemoveTouchInput(this.GetHashCode());
        }
        //------------------------------------------------------
        public void OnPointerDown(PointerEventData eventData)
        {
            m_PointerEventData = eventData;
        }
        //------------------------------------------------------
        public void OnTouchBegin(ATouchInput.TouchData touch)
        {
            if (Framework.Plugin.Guide.GuideSystem.getInstance().bDoing)
            {
                return;
            }

            if(m_nTouchID ==-1)
            {
                m_nTouchID = touch.touchID;
                m_LastTouch = touch.position;
                m_Dragging = CheckOverlap(touch.position);
            }
        }
        //------------------------------------------------------
        public void OnTouchEnd(ATouchInput.TouchData touch)
        {
            if (Framework.Plugin.Guide.GuideSystem.getInstance().bDoing)
            {
                return;
            }

            if (m_nTouchID == touch.touchID)
            {
                m_nTouchID = -1;
                m_LastTouch = touch.position;
                m_Dragging = false;
                m_PointerEventData = null;
                OnTouchEndCallback?.Invoke();
            }
        }
        //------------------------------------------------------
        public bool CheckOverlap(Vector2 pos)
        {
            if (EventSystem.current == null) return false;

            if(DragContains==null || DragContains.Count<=0)
            {
                if (EventSystem.current.currentSelectedGameObject == this.gameObject || EventSystem.current.firstSelectedGameObject == this.gameObject)
                {
                    return true;
                }
            }

            if (EventSystem.current.currentSelectedGameObject != null)
            {
                //Debug.LogError("firstSelectedGameObject:" + EventSystem.current.firstSelectedGameObject.name);
                //UnityEditor.Selection.objects = new GameObject[] { EventSystem.current.currentSelectedGameObject };
                return CheckDraging(EventSystem.current.currentSelectedGameObject.transform);
            }
            else
            {
                //Debug.LogError("firstSelectedGameObject 为null");
            }

            if (EventSystem.current.firstSelectedGameObject != null)
            {
                //Debug.LogError("firstSelectedGameObject:" + EventSystem.current.firstSelectedGameObject.name);
                //UnityEditor.Selection.objects = new GameObject[] { EventSystem.current.currentSelectedGameObject };
                return CheckDraging(EventSystem.current.firstSelectedGameObject.transform);
            }
            else
            {
                //Debug.LogError("firstSelectedGameObject 为null");
            }

            if (m_PointerEventData != null && m_PointerEventData.pointerEnter != null)
            {
                //Debug.Log("click object:" + m_PointerEventData.pointerEnter.name);
                //UnityEditor.Selection.objects = new Object[] { m_PointerEventData.pointerEnter };
                return (CheckDraging(m_PointerEventData.pointerEnter.transform));
            }
            else
            {
                return false;
            }

            //return true;
        }
        //------------------------------------------------------
        bool CheckDraging(Transform pTrans)
        {
            while(pTrans)
            {
                if (DragContains.Contains(pTrans))
                {
                    return true;
                }
                pTrans = pTrans.parent;
            }
            return false;
        }
        //------------------------------------------------------
        public void OnTouchMove(ATouchInput.TouchData touch)
        {
            if (m_nTouchID != touch.touchID) return;
            if (!m_Dragging || Framework.Plugin.Guide.GuideSystem.getInstance().bDoing)
                return;

            Vector2 dragDeltaPos = Vector2.zero;
            dragDeltaPos = (touch.position - m_LastTouch) * m_DragDir;

            Vector2 canOffsetPos = Vector2.zero;
            CheckBorder(FitImg, dragDeltaPos, out canOffsetPos, m_imgBorderLimitOffset);
            UpdateDragTransformPos(new Vector3(canOffsetPos.x, canOffsetPos.y, 0));

            //计算惯性
            bEnableDragInertiaLimit = true;

            float deltaTime = Time.unscaledDeltaTime;
            m_Velocity = dragDeltaPos / deltaTime;

            //记录最后拖拽坐标
            m_LastTouch = touch.position;

            //print("OnTouchMove,dragDeltaPos:" + dragDeltaPos + ",m_DragDir:" + m_DragDir + ",canOffsetPos:" + canOffsetPos);
        }
        //------------------------------------------------------
        public void OnTouchWheel(float wheel, Vector2 mouse)
        {
        }
        //------------------------------------------------------
        public void UpdateDragTransformPos(Vector3 pos)
        {
            foreach (var item in DragTrasnforms)
            {
                item.localPosition += pos;
            }
        }
        //------------------------------------------------------
        public void SetPosition(Vector2 anchoredPosition,Action onInertiaEndAction = null)
        {
            if (onInertiaEndAction != null)
            {
                OnInertiaEnd += onInertiaEndAction;
            }

            Vector2 canOffsetPos = Vector2.zero;
            Vector2 deltaPos = anchoredPosition - FitImg.rectTransform.anchoredPosition;
            CheckBorder(FitImg, deltaPos, out canOffsetPos, m_imgBorderLimitOffset);

            bEnableDragInertiaLimit = false;
            m_Velocity = deltaPos;
            //UpdateDragTransformPos(new Vector3(canOffsetPos.x, canOffsetPos.y, 0));
        }
    }
}
