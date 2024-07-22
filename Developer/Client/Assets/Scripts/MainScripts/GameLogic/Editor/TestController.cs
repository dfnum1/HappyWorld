/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	AnimatorStateCallback
作    者:	HappLI
描    述:	动画动作及回调
*********************************************************************/
using System;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.Rendering;
using TopGame.Data;
using Framework.Core;

namespace TopGame.Logic
{
    public class TestController
    {
        enum ETab
        {
            Ctl,
            SystemInfo,
            Count,
        }
        static bool m_bExpand = false;
        static Vector2 m_Scroll = Vector2.zero;

        static ETab m_eTab = ETab.Ctl;
        static bool m_bPostprocessEnabled = true;

        static string m_strSystemInfoText = null;
        static float m_fDevResoulution = 1;
        static float m_fShadowDistance = 18;
        static float m_fShadowDepthBias = 1;
        static float m_fShadowNormalBias = 1;
        static float m_fRenderScale = 1;

        static int m_nOneFrameCost = -1;
        static int m_nOneFrameMaxInstance = -1;
        static int m_nOneFrameDestroyDelta = -1;
        public static void OnGUI()
        {
            if (GUI.Button(new Rect(0, Screen.height / 2 - 120, 100, 80), m_bExpand ? "-" : "+"))
            {
                m_bExpand = !m_bExpand;
            }
            if(m_bExpand)
            {
                GUI.ModalWindow(0, new Rect(0, 0, Screen.width, Screen.height), OnController, "控制台");
            }
        }
        //------------------------------------------------------
        static GUILayoutOption[] ms_LayoutItem = null;
        static void OnController(int id)
        {
            if (GUILayout.Button("关闭", new GUILayoutOption[] { GUILayout.Height(80) }))
            {
                m_bExpand = false;
            }
            GUILayout.BeginHorizontal();
            for (int i = 0; i < (int)ETab.Count; ++i)
            {
                if(GUILayout.Button(((ETab)i).ToString(), new GUILayoutOption[] { GUILayout.Height(80) }))
                {
                    m_eTab = (ETab)i;
                }
            }
            GUILayout.EndHorizontal();
            if (ms_LayoutItem == null)
                ms_LayoutItem = new GUILayoutOption[] { GUILayout.Height(80) };



            m_Scroll = GUILayout.BeginScrollView(m_Scroll);

            if(m_eTab == ETab.Ctl)
            {
                DrawController(ms_LayoutItem);
            }
            else if(m_eTab == ETab.SystemInfo)
            {
                DrawSystemInfo(ms_LayoutItem);
            }
            

            GUILayout.EndScrollView();
        }
        //------------------------------------------------------
        static void DrawSystemInfo(GUILayoutOption[] LayoutItem)
        {
            if(m_strSystemInfoText == null)
            {
                Type type = typeof(SystemInfo);
                PropertyInfo[] propertyInfos = type.GetProperties(BindingFlags.Static | BindingFlags.Public | BindingFlags.GetProperty);

                StringBuilder sb = new StringBuilder();
                int index = 0;
                {
                    sb.AppendFormat("UUID = {0}", GameInstance.getInstance().GetDeviceUDID());
                    sb.AppendLine();
                }
                sb.AppendLine("total: " + propertyInfos.Length + " items");
                {
                    sb.AppendFormat("{0:D2}. {1,-50} = ", index++, "FP16");
                    sb.Append(SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf)?"Support" : "UnSupport");
                    sb.AppendLine();
                }
                {
                    sb.AppendFormat("{0:D2}. {1,-50} = ", index++, "VSync");
                    sb.Append(QualitySettings.vSyncCount);
                    sb.AppendLine();
                }
                {
                    sb.AppendFormat("{0:D2}. {1,-50} = ", index++, "R11G11B10");
                    sb.Append(SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RGB111110Float) ? "Support" : "UnSupport");
                    sb.AppendLine();
                }
                for (int i = 0; i < propertyInfos.Length; i++)
                {
                    index++;
                    sb.AppendFormat("{0:D2}. {1,-50} = ", index, propertyInfos[i].Name);
                    sb.Append(type.InvokeMember(propertyInfos[i].Name, BindingFlags.Static | BindingFlags.Public | BindingFlags.GetProperty, null, null, null));
                    sb.AppendLine();
                }
                m_strSystemInfoText = sb.ToString();
            }
            GUILayout.TextArea(m_strSystemInfoText);
        }
        //------------------------------------------------------
        static void DrawController(GUILayoutOption[] LayoutItem)
        {
            //！ 相机镜头
            if (Core.CameraController.getInstance() != null)
            {
                Core.CameraController ctl = (Core.CameraController)Core.CameraController.getInstance();
                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("背景镜头" + (ctl.GetCamera(ECameraType.Background).enabled ? "关闭" : "开启"), LayoutItem))
                    {
                        ctl.GetCamera(ECameraType.Background).enabled = !ctl.GetCamera(ECameraType.Background).enabled;
                    }
                    if (GUILayout.Button("近景镜头" + (ctl.GetCamera(ECameraType.Force).enabled ? "关闭" : "开启"), LayoutItem))
                    {
                        ctl.GetCamera(ECameraType.Force).enabled = !ctl.GetCamera(ECameraType.Force).enabled;
                    }
                    if (GUILayout.Button("特效镜头" + (ctl.GetCamera(ECameraType.Effect).enabled ? "关闭" : "开启"), LayoutItem))
                    {
                        ctl.GetCamera(ECameraType.Effect).enabled = !ctl.GetCamera(ECameraType.Effect).enabled;
                    }
                    if (GUILayout.Button("UI镜头" + (GameInstance.getInstance().uiManager.GetUICamera().enabled ? "关闭" : "开启"), LayoutItem))
                    {
                        GameInstance.getInstance().uiManager.GetUICamera().enabled = !GameInstance.getInstance().uiManager.GetUICamera().enabled;
                    }
                }
                GUILayout.EndHorizontal();

             //   GUILayout.BeginHorizontal();
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("AsyncUploadTime:");
                    string asyncloadTime = GUILayout.TextField(QualitySettings.asyncUploadTimeSlice.ToString());
                    int temp = 0;
                    if (int.TryParse(asyncloadTime, out temp))
                        QualitySettings.asyncUploadTimeSlice = temp;
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("AsyncUploadBuffer:");
                    asyncloadTime = GUILayout.TextField(QualitySettings.asyncUploadBufferSize.ToString());
                    temp = 0;
                    if (int.TryParse(asyncloadTime, out temp))
                        QualitySettings.asyncUploadBufferSize = temp;
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("SreamReadBufferSize:");
                    asyncloadTime = GUILayout.TextField(FileSystemUtil.GetFileSystem().GetSreamReadBufferSize().ToString());
                    temp = 0;
                    if (int.TryParse(asyncloadTime, out temp))
                        FileSystemUtil.GetFileSystem().SetSreamReadBufferSize((uint)temp);
                    GUILayout.EndHorizontal();

                }
                //     GUILayout.EndHorizontal();

                UnityEngine.Rendering.Volume postProcessVolume = ctl.GetPostProcessVolume();
                if (postProcessVolume != null && postProcessVolume.sharedProfile)
                {
                    if (postProcessVolume.sharedProfile.components != null)
                    {
                        for (int i = 0; i < postProcessVolume.sharedProfile.components.Count; ++i)
                        {
                            VolumeComponent comp = postProcessVolume.sharedProfile.components[i];
                            string name = comp.name;
                            bool bIsBloom = false;
                            if(comp is UnityEngine.Rendering.Universal.Bloom)
                            {
                                bIsBloom = true;
                            }
                            if (bIsBloom)
                            {
                                GUILayout.BeginHorizontal();
                                UnityEngine.Rendering.Universal.Bloom bloom = comp as UnityEngine.Rendering.Universal.Bloom;
                                bloom.highQualityFiltering.value = GUILayout.Toggle(bloom.highQualityFiltering.value, "高品质", LayoutItem);
                            }
                            if (GUILayout.Button(name + (comp.active ? "关闭" : "开启"), LayoutItem))
                            {
                                comp.active = !comp.active;
                            }
                            if (bIsBloom)
                            {
                                GUILayout.EndHorizontal();
                            }
                        }
                    }
                }
            }
            {
                GUILayout.BeginHorizontal();
                string num = GUILayout.TextField(m_fDevResoulution.ToString(), LayoutItem);
                if (!float.TryParse(num, out m_fDevResoulution))
                    m_fDevResoulution = 1;
                if (GUILayout.Button("DevRes", LayoutItem))
                {
                 //   ScalableBufferManager.ResizeBuffers(m_fDevResoulution, m_fDevResoulution);
                    Screen.SetResolution(Mathf.RoundToInt(GameQuality.DefaultResolution.x * m_fDevResoulution), Mathf.RoundToInt(GameQuality.DefaultResolution.y * m_fDevResoulution), true);
                }
                GUILayout.EndHorizontal();

                UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset urpAsset = UnityEngine.Rendering.GraphicsSettings.renderPipelineAsset as UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset;

                if(urpAsset!=null)
                {
                    GUILayout.BeginHorizontal();
                    string shadow = GUILayout.TextField(m_fShadowDistance.ToString(), LayoutItem);
                    if (!float.TryParse(shadow, out m_fShadowDistance))
                        m_fShadowDistance = 18;
                    if (GUILayout.Button(urpAsset.name + "-ShadowDis", LayoutItem))
                    {
                        urpAsset.shadowDistance = m_fShadowDistance;
                    }
                    GUILayout.EndHorizontal();


                    GUILayout.BeginHorizontal();
                    string depth = GUILayout.TextField(m_fShadowDepthBias.ToString(), LayoutItem);
                    if (!float.TryParse(depth, out m_fShadowDepthBias))
                        m_fShadowDepthBias = 1;
                    if (GUILayout.Button(urpAsset.name + "-ShadowDepth", LayoutItem))
                    {
                            urpAsset.shadowDepthBias = m_fShadowDepthBias;
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    string bais = GUILayout.TextField(m_fShadowNormalBias.ToString(), LayoutItem);
                    if (!float.TryParse(bais, out m_fShadowNormalBias))
                        m_fShadowNormalBias = 1;
                    if (GUILayout.Button(urpAsset.name + "-ShadowNormal", LayoutItem))
                    {
                            urpAsset.shadowNormalBias = m_fShadowNormalBias;
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    string renderScale = GUILayout.TextField(m_fRenderScale.ToString(), LayoutItem);
                    if (!float.TryParse(renderScale, out m_fRenderScale))
                        m_fRenderScale = 1;
                    if (GUILayout.Button(urpAsset.name + "-RenderScale", LayoutItem))
                    {
                        urpAsset.renderScale = m_fRenderScale;
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    urpAsset.supportsCameraDepthTexture = GUILayout.Toggle(urpAsset.supportsCameraDepthTexture,"CameraDepth", LayoutItem);
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    urpAsset.supportsCameraOpaqueTexture = GUILayout.Toggle(urpAsset.supportsCameraOpaqueTexture, "CameraOpaque", LayoutItem);
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    urpAsset.supportsHDR = GUILayout.Toggle(urpAsset.supportsHDR, "CameraHDR", LayoutItem);
                    GUILayout.EndHorizontal();
                }

                {
                    if (m_nOneFrameCost < 0) m_nOneFrameCost = GameQuality.OneFrameCost;
                    if (m_nOneFrameMaxInstance < 0) m_nOneFrameMaxInstance = GameQuality.MaxInstanceCount;
                    if (m_nOneFrameDestroyDelta < 0) m_nOneFrameCost = GameQuality.DestroyDelayTime;
                    {
                        GUILayout.BeginHorizontal();
                        string temp = GUILayout.TextField(m_nOneFrameCost.ToString(), LayoutItem);
                        if (!int.TryParse(temp, out m_nOneFrameCost))
                            m_nOneFrameCost = GameQuality.OneFrameCost;
                        if (GUILayout.Button("单帧耗时分帧-毫秒", LayoutItem))
                        {
                            FileSystemUtil.SetCapability(m_nOneFrameCost, m_nOneFrameMaxInstance, m_nOneFrameDestroyDelta);
                        }
                        GUILayout.EndHorizontal();
                    }
                    {
                        GUILayout.BeginHorizontal();
                        string temp = GUILayout.TextField(m_nOneFrameMaxInstance.ToString(), LayoutItem);
                        if (!int.TryParse(temp, out m_nOneFrameMaxInstance))
                            m_nOneFrameMaxInstance = GameQuality.MaxInstanceCount;
                        if (GUILayout.Button("单帧最大实例化个数", LayoutItem))
                        {
                            FileSystemUtil.SetCapability(m_nOneFrameCost, m_nOneFrameMaxInstance, m_nOneFrameDestroyDelta);
                        }
                        GUILayout.EndHorizontal();
                    }
                    {
                        GUILayout.BeginHorizontal();
                        string temp = GUILayout.TextField(m_nOneFrameDestroyDelta.ToString(), LayoutItem);
                        if (!int.TryParse(temp, out m_nOneFrameDestroyDelta))
                            m_nOneFrameDestroyDelta = GameQuality.DestroyDelayTime;
                        if (GUILayout.Button("缓冲池延迟时长-毫秒", LayoutItem))
                        {
                            FileSystemUtil.SetCapability(m_nOneFrameCost, m_nOneFrameMaxInstance, m_nOneFrameDestroyDelta);
                        }
                        GUILayout.EndHorizontal();
                    }
                }

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("FPS 30", LayoutItem))
                {
                    //Application.targetFrameRate = 30;
                    GameInstance.getInstance().TargetFrameRate = 30;
                }
                if (GUILayout.Button("FPS 45", LayoutItem))
                {
                    //Application.targetFrameRate = 45;
                    GameInstance.getInstance().TargetFrameRate = 45;
                }
                if (GUILayout.Button("FPS 60", LayoutItem))
                {
                    // Application.targetFrameRate = 60;
                    GameInstance.getInstance().TargetFrameRate = 60;
                }
                if (GUILayout.Button("FPS 无限制", LayoutItem))
                {
                     Application.targetFrameRate = 1000;
                }
                GUILayout.EndHorizontal();
                //                 if (GUILayout.Button("720", LayoutItem))
                //                 {
                //                     Screen.SetResolution(GameQuality.DefaultResolution.x * 720 / GameQuality.DefaultResolution.y, 720, true);
                //                 }
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("VSync 关闭", LayoutItem))
                {
                    QualitySettings.vSyncCount = 0;
                }
                if (GUILayout.Button("VSync 开启", LayoutItem))
                {
                    QualitySettings.vSyncCount = 1;
                }
                GUILayout.EndHorizontal();
                if (GUILayout.Button("FreeGC", LayoutItem))
                {
                   FileSystemUtil.Free();
                }

                if (GUILayout.Button("MSAA x0", LayoutItem))
                {
                    QualitySettings.antiAliasing = 0;
                    if (urpAsset) urpAsset.msaaSampleCount = 0;
                }
                if (GUILayout.Button("MSAA x2", LayoutItem))
                {
                    QualitySettings.antiAliasing = 2;
                    if (urpAsset) urpAsset.msaaSampleCount = 2;
                }
                if (GUILayout.Button("MSAA x4", LayoutItem))
                {
                    QualitySettings.antiAliasing = 4;
                    if (urpAsset) urpAsset.msaaSampleCount = 4;
                }
                if (GUILayout.Button("MSAA x8", LayoutItem))
                {
                    QualitySettings.antiAliasing = 8;
                    if (urpAsset) urpAsset.msaaSampleCount = 8;
                }
            }
        }
    }
}
