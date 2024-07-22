using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using TopGame.Base;


namespace TopGame.Core
{
   
    public class AmbientEmitter : MonoBehaviour
    {
        [SerializeField]
        //[EventRef]
        private EventReference m_event;

        private StudioEventEmitter m_pEmitter;
        // Start is called before the first frame update
        void Start()
        {
            if(m_pEmitter == null )
                m_pEmitter = GetComponent<StudioEventEmitter>();
            m_pEmitter.AllowFadeout = true;
           
        }

        // Update is called once per frame
        void Update()
        {
            
            // 主城功能建筑 逻辑 ///////////////////////
            if (gameObject.layer == (int)EMaskLayer.EffectLayer && !m_pEmitter.IsPlaying())
            {
                m_pEmitter.Play();
            }
            else if(gameObject.layer != (int)EMaskLayer.EffectLayer && m_pEmitter.IsPlaying())
            {
                m_pEmitter.Stop();
                
            }
            ////////////////////////////////////////////
        }
    }
}

