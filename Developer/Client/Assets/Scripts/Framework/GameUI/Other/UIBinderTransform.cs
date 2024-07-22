/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	UIBinderTransform
作    者:	happli
描    述:	绑定绑点
*********************************************************************/
using Framework.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TopGame.UI
{
    [UI.UIWidgetExport]
    public class UIBinderTransform : MonoBehaviour
    {
        public RectTransform binderTrans;
        public string binderName;

        bool m_ConvertUI = false;
        public Transform m_pBinder;
        public bool isUpdate = true;
        public Transform BinderTran
        {
            get { return m_pBinder?.GetComponent<SkinnedMeshRenderer>().rootBone; }
        }


        private void OnEnable()
        {
            if (!string.IsNullOrEmpty(binderName))
                m_pBinder = DyncmicTransformCollects.FindTransformByName(binderName);
            else
                m_pBinder = binderTrans;
            m_ConvertUI = !(m_pBinder is RectTransform);
        }
        private void OnDisable()
        {
            m_pBinder = null;
        }
        private void LateUpdate()
        {
            if (m_pBinder == null || !isUpdate) return;
            if(m_ConvertUI)
            {
                Vector3 uiPos = Vector3.zero;
                if(UI.UIKits.WorldPosToUIPos(BinderTran.position, false, ref uiPos))
                {
                    transform.position = uiPos;
                }
            }
            else
            {
                transform.position = m_pBinder.position;
            }
        }
    }
}
