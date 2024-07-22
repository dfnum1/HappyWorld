/********************************************************************
生成日期:	2020-6-16
类    名: 	IBuildingInstance
作    者:	happli
描    述:	建筑实体对象
*********************************************************************/
using Framework.Core;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
namespace TopGame.Logic
{
    [DisallowMultipleComponent]
    public abstract class ABuildingInstance : MonoBehaviour
    {
        public static System.Action<ABuildingInstance> OnAddBuilding;
        public static System.Action<ABuildingInstance> OnDelBuilding;

        public CityZoom ownerZoom;
        [System.Serializable]
        public struct StateEffect
        {
            [Framework.Data.DisplayNameGUI("状态")]
#if UNITY_EDITOR
            [Framework.ED.DisplayDrawType("TopGame.SvrData.EBuildingChunkState")]
#endif   
            public int state;
            [Framework.Data.DisplayNameGUI("特效")]
            [Framework.Data.StringViewGUI(typeof(GameObject))]
            public string effect;
        }    

        [Framework.Data.DisplayNameGUI("状态标志")]
        [Framework.Data.DisplayEnumBitGUI("TopGame.Logic.EBuidingMarkerType", true)]
        public int MarkerFlags = -1;
        public List<StateEffect> stateEffects;
        public int SortOrder = 0;
        public int nBuildingID = -1;
        public float cameraLerp = 0;
        public Vector3 cameraPosition = Vector3.zero;
        public Vector3 cameraLookAt = Vector3.zero;

        public Animator actionAnimator = null;
        public string viewInAction = "ViewIn";
        public string viewOutAction = "ViewOut";

        public FMODUnity.EventReference fmodEvent;

        public CityWayPoints wayPoints = null;

        private int m_nDefaultLayerFlag = 0;
        Transform m_pTransform = null;

        ISound m_pSound = null;
        //-------------------------------------------
        protected virtual void Awake()
        {
            m_pTransform = transform;
            if (OnAddBuilding != null) OnAddBuilding(this);
        }
        //-------------------------------------------
        protected virtual void OnDestroy()
        {
            m_pTransform = null;
            m_nDefaultLayerFlag = gameObject.layer;
            if (OnDelBuilding != null) OnDelBuilding(this);
            if (m_pSound != null)
                m_pSound.Stop();
            m_pSound = null;
        }
        //-------------------------------------------
        public Transform GetTransform()
        {
            if (m_pTransform == null) m_pTransform = transform;
            return m_pTransform;
        }
        //-------------------------------------------
        public void Clear()
        {

        }
        //-------------------------------------------
        public virtual BuildingCrowdInstance GetOwnerCrowd()
        {
            return null;
        }
        //-------------------------------------------
        protected virtual void OnEnable()
        {
            if (m_pSound != null) m_pSound.Stop();
            m_pSound = TopGame.Core.AudioManager.PlayEvent(fmodEvent);
            if (m_nDefaultLayerFlag == 0)
                m_nDefaultLayerFlag = gameObject.layer;
        }
        //-------------------------------------------
        protected virtual void OnDisable()
        {
            if (m_pSound!=null)
            {
                m_pSound.Stop();
                m_pSound = null;
            }
        }
        //------------------------------------------------------
        public int GetDefaultLayer()
        {
            return m_nDefaultLayerFlag;
        }
        //------------------------------------------------------
        public int GetLayer()
        {
            return gameObject.layer;
        }
        //------------------------------------------------------
        public virtual void ResetLayer()
        {
            if (gameObject.layer != m_nDefaultLayerFlag)
            {
                Framework.Core.BaseUtil.SetRenderLayer(gameObject, m_nDefaultLayerFlag);
            }
        }
        //------------------------------------------------------
        public void SetRenderLayer(int layer)
        {
            if (layer == -1) return;
            if (gameObject.layer != layer)
            {
                if (m_nDefaultLayerFlag == 0) m_nDefaultLayerFlag = gameObject.layer;
                Framework.Core.BaseUtil.SetRenderLayer(gameObject, layer);
            }
        }
    }
}
