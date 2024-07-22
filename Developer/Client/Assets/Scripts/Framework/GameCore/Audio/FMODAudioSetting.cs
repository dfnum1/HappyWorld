// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using FMODUnity;
// using FMOD.Studio;
// using System;
// using UnityEditor;
// 
// namespace TopGame.Core
// {
//     public enum UI_SOUND_TYPE
//     {
//         NULL,
//         CLICK,
//         CLOSE,
//         TRANSFORM,
//         BUTTON_CLICK,
//         ITEM_POP,
//         ITEM_CANCEL,
//         ORDER_OPEN,
//         ORDER_CLOSE,
//         REWARD_POP,
//         REWARD_ITEM_POP,
//         BUILD_RECLAMATION,
//         BUILD_CONSTRUCTION,
//         BUILD_CONSTRUCTION_VC,
//         BUILD_ITEM_METAL_AXE,
//         DRAG_BEGIN,
//         DRAG_END,
//         HERO_UP,
//         HERO_DOWN,
//     }
// 
//     public enum UI_BUILD_ITEM
//     {
//         ITEM_METAL_AXE ,
//     }
//     public class FMODAudioSetting : MonoBehaviour
//     {
// 
//         
//         [MinMaxAttributeFloat(0f, 1f)]
//         public float m_volumeMainCity = 1;
// 
// 
//         [MinMaxAttributeFloat(0f, 1f)]
//         public float volumeBGM = 0.2f;
//         [MinMaxAttributeFloat(0f, 1f)]
//         public float volumeAmbient = 0.25f;
//         [MinMaxAttributeFloat(0f, 1f)]
//         public float volumeBuildingBGS = 0.4f;
//         [MinMaxAttributeFloat(0f, 1f)]
//         public float volumeUI = 0.5f;
// 
//         public bool m_bPlayTestBGM = false;
//         static int ms_nBGParameter = -1;
//         public static FMOD.Studio.EventInstance ms_eventBGM;
// 
//         public FMOD.Studio.Bus m_pBusBGM;
//         public FMOD.Studio.Bus m_pBusAmbient;
//         public FMOD.Studio.Bus m_pBusBuildingBGS;
//         public FMOD.Studio.Bus m_pBusUI;
// 
// 
// 
//         public FMOD.Studio.EventInstance m_pBGMMainCityEvent;
//         public int m_minDB = -70;
//         public int m_maxDB = 10;
// 
//         private static FMODAudioSetting pInstance = null ;
// 
// 
//         private Dictionary<FMOD.GUID, FMOD.Studio.EventInstance> m_vEventInstance = null;
// 
//         public static FMODAudioSetting GetInstance()
//         {
//             return pInstance;
//         }
// 
//         private void Awake()
//         {
//             pInstance = this;
//             volumeBGM = 0.2f;
//             volumeAmbient = 0.25f;
//             volumeBuildingBGS = 0.4f;       
//             volumeUI = 0.5f;
//         //m_pBGMMainCityEvent = FMODUnity.RuntimeManager.CreateInstance("event:/BuildingBGS/BigOrder");
// 
//             m_pBusBGM = FMODUnity.RuntimeManager.GetBus("bus:/BGM");
//             m_pBusAmbient = FMODUnity.RuntimeManager.GetBus("bus:/Ambient");
//             m_pBusBuildingBGS = FMODUnity.RuntimeManager.GetBus("bus:/BuildingBGS");
//             m_pBusUI = FMODUnity.RuntimeManager.GetBus("bus:/UI");
//         }
// 
//         // Start is called before the first frame update
//         void Start()
//         {
//             FMODUnity.RuntimeManager.LoadBank("Master.bank");
//             PlayBGM("event:/Music/BGM");
//             m_bPlayTestBGM = true; 
// 
//         }
//         //------------------------------------------------------
//         private void OnDestroy()
//         {
//             if (m_vEventInstance == null) return;
//             EventInstance evtInst;
//             foreach (var db in m_vEventInstance)
//             {
//                 evtInst = db.Value;
//                 if (evtInst.isValid())
//                 {
//                     evtInst.stop(STOP_MODE.IMMEDIATE);
//                     evtInst.release();
//                     evtInst.clearHandle();
//                 }
//             }
//             m_vEventInstance.Clear();
//         }
//         //------------------------------------------------------
//         // Update is called once per frame
//         void Update()
//         {
//             m_pBusBGM.setVolume(volumeBGM);
//             m_pBusAmbient.setVolume(volumeAmbient);
//             m_pBusBuildingBGS.setVolume(volumeBuildingBGS);
//             m_pBusUI.setVolume(volumeUI);
// 
//             if (Input.GetKeyUp(KeyCode.Space))
//             {
//                 //SFXVolumeLevel(0.5f);
//                 PlayEvent("event:/Common_UI/Click");
//             }
//             if(m_bPlayTestBGM)
//             {
// 
//             }
//             
//         }
//         //------------------------------------------------------
//         void AddEventInstance(FMOD.GUID guid, FMOD.Studio.EventInstance instacne)
//         {
//             if (instacne.isValid())
//             {
//                 if (m_vEventInstance == null) m_vEventInstance = new Dictionary<FMOD.GUID, EventInstance>();
//                 else
//                 {
//                     FMOD.Studio.EventInstance existEvent;
//                     if (m_vEventInstance.TryGetValue(guid, out existEvent) && existEvent.isValid())
//                     {
//                         existEvent.stop(STOP_MODE.ALLOWFADEOUT);
//                         existEvent.release();
//                         existEvent.clearHandle();
//                     }
//                 }
//                 m_vEventInstance[guid] = instacne;
//             }
//         }
//         //------------------------------------------------------
//         void DelEventInstance(FMOD.GUID guid, STOP_MODE mode)
//         {
//             if (m_vEventInstance == null) return;
//             FMOD.Studio.EventInstance instance;
//             if(m_vEventInstance.TryGetValue(guid, out instance))
//             {
//                 instance.stop(mode);
//                 instance.release();
//                 instance.clearHandle();
//                 m_vEventInstance.Remove(guid);
//             }
//         }
//         //------------------------------------------------------
//         public void SFXVolumeLevel(float newSFXVolume)
//         {
//             m_volumeMainCity = newSFXVolume;
//             FMOD.Studio.PLAYBACK_STATE pbState;
//             m_pBGMMainCityEvent.getPlaybackState(out pbState);
//             if (pbState != FMOD.Studio.PLAYBACK_STATE.PLAYING)
//             {
//                 m_pBGMMainCityEvent.start();
//             }
//         }
// 
//         public static void SetBGVolumeLevel(float value)
//         {
//             if (pInstance == null) return;
//             pInstance.volumeBGM = value;
//             pInstance.volumeBuildingBGS = value;
//             pInstance.volumeAmbient = value;
//         }
// 
//         public static void SetEffectVolumeLevel(float value)
//         {
//             if (pInstance == null) return;
//             pInstance.volumeUI = value;
//         }
// 
//         public static string GetEventPath(UI_SOUND_TYPE type)
//         {
//             string path = "";
//             switch (type)
//             {
//                 case UI_SOUND_TYPE.CLICK:               path = "event:/Common_UI/Click";        break;
//                 case UI_SOUND_TYPE.CLOSE:               path = "event:/Common_UI/Close";        break;
//                 case UI_SOUND_TYPE.TRANSFORM:           path = "event:/Common_UI/Transform";    break;
//                 case UI_SOUND_TYPE.BUTTON_CLICK:        path = "event:/Common_UI/ButtonClick";  break;
//                 case UI_SOUND_TYPE.ITEM_POP:            path = "event:/Common_UI/ItemPop";      break;
//                 case UI_SOUND_TYPE.ITEM_CANCEL:         path = "event:/Common_UI/ItemCancel";   break;
//                 case UI_SOUND_TYPE.ORDER_OPEN:          path = "event:/Common_UI/OrderOpen";    break;
//                 case UI_SOUND_TYPE.ORDER_CLOSE:         path = "event:/Common_UI/OrderClose";   break;
//                 case UI_SOUND_TYPE.REWARD_POP:          path = "event:/Common_UI/RewardPop";    break;
//                 case UI_SOUND_TYPE.REWARD_ITEM_POP:     path = "event:/Common_UI/RewardItemPop"; break;
//                 case UI_SOUND_TYPE.BUILD_RECLAMATION:   path = "event:/UI_Build/Reclamation";   break;
//                 case UI_SOUND_TYPE.BUILD_CONSTRUCTION:  path = "event:/UI_Build/Construction";  break;
//                 case UI_SOUND_TYPE.BUILD_ITEM_METAL_AXE:path = "event:/UI_Build/Item_Metal";    break;
//                 case UI_SOUND_TYPE.DRAG_BEGIN:path = "event:/UI_PrepareWar/MoveUp";    break;
//                 case UI_SOUND_TYPE.DRAG_END:path = "event:/UI_PrepareWar/MoveDown";    break;
//                 case UI_SOUND_TYPE.HERO_DOWN:path = "event:/UI_PrepareWar/Down";    break;
//                 case UI_SOUND_TYPE.HERO_UP:path = "event:/UI_PrepareWar/Up";    break;
//                 case UI_SOUND_TYPE.NULL:            break;
//             }
//             return path ;
//         }
// 
//         public static void PlayUISound(UI_SOUND_TYPE type)
//         {          
//             string path = GetEventPath(type) ;
//             PlayEvent(path);            
//         }
//         //------------------------------------------------------
//         //播放3D音效
//         public void Play3DEvent(UI_SOUND_TYPE type, GameObject go , float minDistance , float maxDistance)
//         {
//             string path = GetEventPath(type);
//             FMODUnity.RuntimeManager.PlayOneShotAttached(path , go);
//         }
//         //------------------------------------------------------
//         public static FMOD.Studio.EventInstance PlayEvent(string eventPath,float fDelay = 0)
//         {
//             if (string.IsNullOrEmpty(eventPath))
//                 return FMOD.Studio.EventInstance.DEFAULT;
// #if UNITY_EDITOR
//             if(!Application.isPlaying)
//             {
//                 EditorEventRef eventRef = FMODUnity.EventCache.GetEventRef(eventPath);
//                 if (eventRef != null)
//                 {
//                     EditorUtils.LoadPreviewBanks();
//                     FMODUnity.EditorUtils.PreviewEvent(eventRef, new Dictionary<string, float>());
//                 }
//                 return FMOD.Studio.EventInstance.DEFAULT;
//             }
// #endif
// 
//             FMOD.Studio.EventInstance testAudio = FMODUnity.RuntimeManager.CreateInstance(eventPath);
//             if (testAudio.isValid())
//             {
//                 FMOD.Studio.EVENT_CALLBACK callback;
//                 callback = new FMOD.Studio.EVENT_CALLBACK(BeatEventCallback);
//                 testAudio.setCallback(callback, FMOD.Studio.EVENT_CALLBACK_TYPE.STOPPED);
//                 if(fDelay>0) testAudio.setProperty(EVENT_PROPERTY.SCHEDULE_DELAY, fDelay * 30000);   //fps 30
//                 testAudio.start();
//                 if (pInstance != null)
//                 {
//                     pInstance.AddEventInstance(FMODUnity.RuntimeManager.PathToGUID(eventPath), testAudio);
//                 }
// #if UNITY_EDITOR
//                 Debug.Log(eventPath);
// #endif
//             }
//             return testAudio;
//         }
//         //------------------------------------------------------
//         public static FMOD.Studio.EventInstance PlayBGM(string eventPath, float fDelay = 0)
//         {
//             if (string.IsNullOrEmpty(eventPath))
//                 return FMOD.Studio.EventInstance.DEFAULT;
// #if UNITY_EDITOR
//             if (!Application.isPlaying)
//             {
//                 EditorEventRef eventRef = FMODUnity.EventCache.GetEventRef(eventPath);
//                 if (eventRef != null)
//                 {
//                     EditorUtils.LoadPreviewBanks();
//                     FMODUnity.EditorUtils.PreviewEvent(eventRef, new Dictionary<string, float>());
//                 }
//                 return FMOD.Studio.EventInstance.DEFAULT;
//             }
// #endif
//             if(ms_eventBGM.isValid())
//             {
//                 ms_nBGParameter = -1;
//                 ms_eventBGM.stop(STOP_MODE.ALLOWFADEOUT);
//             }
//             
//             ms_eventBGM = FMODUnity.RuntimeManager.CreateInstance(eventPath);
//             if (ms_eventBGM.isValid())
//             {
//                 FMOD.Studio.EVENT_CALLBACK callback;
//                 callback = new FMOD.Studio.EVENT_CALLBACK(BeatEventCallback);
//                 ms_eventBGM.setCallback(callback, FMOD.Studio.EVENT_CALLBACK_TYPE.STOPPED);
//                 if (fDelay > 0) ms_eventBGM.setProperty(EVENT_PROPERTY.SCHEDULE_DELAY, fDelay * 30000);   //fps 30
//                 ms_eventBGM.start();
//                 if (pInstance != null)
//                 {
//                     pInstance.AddEventInstance(FMODUnity.RuntimeManager.PathToGUID(eventPath), ms_eventBGM);
//                 }
// #if UNITY_EDITOR
//                 Debug.Log(eventPath);
// #endif
//             }
//             return ms_eventBGM;
//         }
//         //------------------------------------------------------
//         public static FMOD.Studio.EventInstance PlayEvent(EventReference eventRef, float fDelay =0)
//         {
//             if (eventRef.IsNull)
//                 return FMOD.Studio.EventInstance.DEFAULT;
//             FMOD.Studio.EventInstance evtInst = RuntimeManager.CreateInstance(eventRef);
//             if(evtInst.isValid())
//             {
//                 FMOD.Studio.EVENT_CALLBACK callback;
//                 callback = new FMOD.Studio.EVENT_CALLBACK(BeatEventCallback);
//                 evtInst.setCallback(callback, FMOD.Studio.EVENT_CALLBACK_TYPE.STOPPED);
//                 if (fDelay > 0) evtInst.setProperty(EVENT_PROPERTY.SCHEDULE_DELAY, fDelay * 30000);   //fps 30
//                 evtInst.start();
//                 if(pInstance!=null)
//                 {
//                     pInstance.AddEventInstance(eventRef.Guid, evtInst);
//                 }
//                 return evtInst;
//             }
//             return FMOD.Studio.EventInstance.DEFAULT;
//         }
//         //------------------------------------------------------
//         public static void StopEvents(string eventPath, FMOD.Studio.STOP_MODE stopMode = STOP_MODE.ALLOWFADEOUT)
//         {
//             if (string.IsNullOrEmpty(eventPath))
//                 return;
// #if UNITY_EDITOR
//             if (!Application.isPlaying)
//             {
//                 EditorEventRef eventRef = FMODUnity.EventCache.GetEventRef(eventPath);
//                 if (eventRef != null)
//                 {
//                     FMODUnity.EditorUtils.PreviewStop(eventRef);
//                 }
//                 return;
//             }
// #endif
//             if(pInstance == null)
//             {
//                 FMOD.Studio.EventDescription testAudio = FMODUnity.RuntimeManager.GetEventDescription(eventPath);
//                 if (testAudio.isValid())
//                 {
//                     testAudio.releaseAllInstances();
//                 }
//             }
//             else
//             {
//                 pInstance.DelEventInstance(FMODUnity.RuntimeManager.PathToGUID(eventPath), stopMode);
//             }
//         }
//         //------------------------------------------------------
//         [AOT.MonoPInvokeCallback(typeof(FMOD.Studio.EVENT_CALLBACK))]
//         static FMOD.RESULT BeatEventCallback(FMOD.Studio.EVENT_CALLBACK_TYPE type, IntPtr instancePtr, IntPtr parameterPtr)
//         {
//             FMOD.Studio.EventInstance instance = new FMOD.Studio.EventInstance(instancePtr);
//             instance.release();
//             return FMOD.RESULT.OK;
//         }
//         //------------------------------------------------------
//         public void StopAllSound(STOP_MODE stopMode = STOP_MODE.ALLOWFADEOUT)
//         {
//             m_pBusBuildingBGS.stopAllEvents(stopMode);
//         }
//         //------------------------------------------------------
//         public void StopBGMSound(STOP_MODE stopMode = STOP_MODE.ALLOWFADEOUT)
//         {
//             m_pBusBGM.stopAllEvents(stopMode);
//         }
//         //------------------------------------------------------
//         public static void SetBGParameter(int param)
//         {
//             if (!ms_eventBGM.isValid()) return;
//             if (ms_nBGParameter == param) return;
//             ms_nBGParameter = param;
//             ms_eventBGM.setParameterByName("Parameter", ms_nBGParameter);
//         }
//     }
// 
// #if UNITY_EDITOR
// 
//     [CustomEditor(typeof(FMODAudioSetting))]
//     [CanEditMultipleObjects]
//     public class FMODAudioSettingEditor : UnityEditor.Editor
//     {
//         FMODAudioSetting m_fmodAudio;
//         string m_btnText = "";
//         bool[] toggleArr = { false, false, false, false, false };
//         float value = 0.8f;
//         public override void OnInspectorGUI()
//         {
//             m_fmodAudio = this.target as FMODAudioSetting;
//             base.OnInspectorGUI();
//             
//             GUILayout.Label("参数:");
// 
//             if(m_fmodAudio.m_bPlayTestBGM)
//             {
//                 m_btnText = "暂停";
//                 //fmodAudio.playTestBGM = GUILayout.Button(btnText);
//             }
//             else
//             { 
//                 m_btnText = "播放";
//                 //fmodAudio.playTestBGM = GUILayout.Button(btnText);
//             }
//             if(GUILayout.Button(m_btnText))
//             {              
//                 if (m_fmodAudio.m_bPlayTestBGM)
//                 {
//                     //value = (m_fmodAudio.m_maxDB - m_fmodAudio.m_minDB) * value + m_fmodAudio.m_minDB;
//                     FMODAudioSetting.ms_eventBGM.setVolume(0);
//                     m_fmodAudio.m_bPlayTestBGM = false;
//                 }
//                 else
//                 {
//                     FMODAudioSetting.ms_eventBGM.setVolume(10);
//                     m_fmodAudio.m_bPlayTestBGM = true ;
//                 }
//                 
//             }
// 
//             
//             if(GUILayout.Toggle(toggleArr[0], "登录"))
//             {                
//                 freshToggle(0);
//                 FMODAudioSetting.ms_eventBGM.setParameterByName("Parameter", 0);
//             }            
//             if( GUILayout.Toggle(toggleArr[1], "备战"))
//             {
//                 freshToggle(1);
//                 FMODAudioSetting.ms_eventBGM.setParameterByName("Parameter", 1);
//             }
//             if( GUILayout.Toggle(toggleArr[2], "主城"))
//             {
//                 freshToggle(2);
//                 FMODAudioSetting.ms_eventBGM.setParameterByName("Parameter", 2);
//             }
//             if(GUILayout.Toggle(toggleArr[3], "战斗"))
//             {
//                 freshToggle(3);
//                 FMODAudioSetting.ms_eventBGM.setParameterByName("Parameter", 3);
//             }
//             if( GUILayout.Toggle(toggleArr[4], "Boss"))
//             {
//                 freshToggle(4);
//                 FMODAudioSetting.ms_eventBGM.setParameterByName("Parameter", 4);
//             }
//         }
// 
//         
//         public bool freshToggle( int indexTrue )
//         {
//             for(int i = 0; i< toggleArr.Length; i++)
//             {
//                 if (i == indexTrue)
//                 {
//                     toggleArr[i] = true;
//                 }   
//                 else
//                     toggleArr[i] = false;
//             }
//             return toggleArr[indexTrue];            
//         }
//     }
// #endif
// 
// }
// 
// 
