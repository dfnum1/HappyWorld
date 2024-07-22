/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	TextureCliperAssets
作    者:	HappLI
描    述:	图片裁切器
*********************************************************************/
using System.Collections.Generic;
using UnityEngine;
using Framework.Core;
namespace TopGame.Data
{
    [CreateAssetMenu]
    public class TextureCliperAssets : ScriptableObject
    {
        [System.Serializable]
        public class Clip
        {
            [Framework.Data.DisplayNameGUI("描述")]
            public string asset;
            [Framework.Data.DisplayNameGUI("裁剪区域")]
            public Rect source;

            [Framework.Data.DisplayNameGUI("指定图片(不走裁剪)")]
            [Framework.Data.StringViewGUI(typeof(Texture2D))]
            public string texture;

            [System.NonSerialized]
            [Framework.Plugin.DisableGUI]
            public ClipGroup group;
        }

        [System.Serializable]
        public class ClipGroup
        {
            [Framework.Data.DisplayNameGUI("id")]
            public uint clipId;
            [Framework.Data.DisplayNameGUI("组名")]
            public string name;
            [Framework.Data.DisplayNameGUI("图片")]
            [Framework.Data.StringViewGUI(typeof(Texture2D))]
            public string path;
            [Framework.Plugin.DisableGUI]
            public Clip[] clips;

            [System.NonSerialized]
            private Framework.Core.Asset m_pAssetRef;
            private int m_nUseRef = 0;
            public void Grab()
            {
                m_nUseRef++;
            }
            public void Release()
            {
                m_nUseRef--;
                if(m_nUseRef<=0)
                {
                    //!TODO release texture
                    if (m_pAssetRef != null)
                        m_pAssetRef.Release();
                    m_pAssetRef = null;
                }
            }

#if UNITY_EDITOR
            [System.NonSerialized]
            public Texture2D texture;
            [System.NonSerialized]
            public System.Object catchData;
#endif
        }

        static TextureCliperAssets ms_Instance = null;
        //------------------------------------------------------
        public ClipGroup[] groups;

        Dictionary<uint, Dictionary<string, Clip>> m_vClips = new Dictionary<uint, Dictionary<string, Clip>>();
        private void OnEnable()
        {
            ms_Instance = this;
            if(groups != null && groups.Length>0)
            {
                m_vClips = new Dictionary<uint, Dictionary<string, Clip>>();
                for (int i = 0; i < groups.Length; ++i)
                {
                    if(string.IsNullOrEmpty(groups[i].name) || string.IsNullOrEmpty(groups[i].path))  continue;
                    Dictionary<string, Clip> vClips;
                    if(!m_vClips.TryGetValue(groups[i].clipId, out vClips))
                    {
                        vClips = new Dictionary<string, Clip>(groups[i].clips.Length);
                        m_vClips.Add(groups[i].clipId, vClips);
                    }
                    for (int j = 0; j < groups[i].clips.Length; ++j)
                    {
                        Clip clip = groups[i].clips[j];
                        clip.group = groups[i];
                        vClips[groups[i].clips[j].asset] = clip;
                    }
                }
            }
        }
        //------------------------------------------------------
        private void OnDestroy()
        {
            ms_Instance = null;
        }
        //------------------------------------------------------
        public static Clip GetClipAsset(uint clipId, string subClip)
        {
            if (ms_Instance == null || ms_Instance.m_vClips == null || string.IsNullOrEmpty(subClip)) return null;
            Dictionary<string, Clip> vClips;
            if (!ms_Instance.m_vClips.TryGetValue(clipId, out vClips)) return null;

            Clip clip;
            if (!vClips.TryGetValue(subClip, out clip))
            {

                return clip;
            }
            return null;
        }
        //------------------------------------------------------
    }
}
