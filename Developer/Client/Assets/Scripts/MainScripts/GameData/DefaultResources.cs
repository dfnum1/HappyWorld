/********************************************************************
生成日期:	1:11:2020 10:05
类    名: 	DefaultResources
作    者:	HappLI
描    述:	游戏缺省资源
*********************************************************************/

using System;
using UnityEngine;

namespace TopGame.Data
{
    public  class DefaultResources
    {
        public static Texture2D GrayTexture
        {
            get
            {
                return GlobalDefaultResources.GrayTexture;
            }
        }
        public static Texture2D TransparencyTexture
        {
            get
            {
                return GlobalDefaultResources.TransparencyTexture;
            }
        }
        //------------------------------------------------------
        public static Sprite TransparencySprite
        {
            get
            {
                return GlobalDefaultResources.TransparencySprite;
            }
            set
            {
                GlobalDefaultResources.TransparencySprite = value;
            }
        }
        //------------------------------------------------------
        public static string DefaultLoading
        {
            get { return "Assets/Datas/Texture/loading/loading.png"; }
        }
        //------------------------------------------------------
        public static string StartUpLoading
        {
            get { return "Assets/Datas/Texture/loading/loading.png"; }
        }
    }
}
