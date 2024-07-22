/********************************************************************
生成日期:	17:9:2019   16:19
类    名: 	GlobalDefaultResources
作    者:	HappLI
描    述:	全局缺省资源
*********************************************************************/
using System;
using UnityEngine;

namespace TopGame.Data
{
    public class GlobalDefaultResources
    {
        static Texture2D ms_pGrayTexture;
        static Texture2D ms_pTransparencyTexture;
        static Sprite ms_pTransparencySprite;
        static Material ms_UIGrayMat = null;

        static Material[] ms_DefaultMaterials;
        public static Texture2D GrayTexture
        {
            get
            {
                if (ms_pGrayTexture == null)
                {
                    ms_pGrayTexture = new Texture2D(2, 2, TextureFormat.ARGB32, false);
                    for (int i = 0; i < 2; ++i)
                    {
                        for (int j = 0; j < 2; ++j)
                        {
                            ms_pGrayTexture.SetPixel(i, j, new Color(0, 0, 0, 0.95f));
                        }
                    }
                    ms_pGrayTexture.Apply(false);
                }
                return ms_pGrayTexture;
            }
        }
        public static Texture2D TransparencyTexture
        {
            get
            {
                if (ms_pTransparencyTexture == null)
                {
                    ms_pTransparencyTexture = new Texture2D(2, 2, TextureFormat.ARGB32, false);
                    for (int i = 0; i < 2; ++i)
                    {
                        for (int j = 0; j < 2; ++j)
                        {
                            ms_pTransparencyTexture.SetPixel(i, j, new Color(0, 0, 0, 0));
                        }
                    }
                    ms_pTransparencyTexture.Apply(false);
                }
                return ms_pTransparencyTexture;
            }
        }
        //------------------------------------------------------
        public static Sprite TransparencySprite
        {
            get
            {
                if (ms_pTransparencySprite == null)
                    ms_pTransparencySprite = Sprite.Create(TransparencyTexture, new Rect(0, 0, 2, 2), Vector2.zero, 100);
                return ms_pTransparencySprite;
            }
            set
            {
                ms_pTransparencySprite = value;
            }
        }
        //------------------------------------------------------
        public static Material UIGrayMat
        {
            get { return ms_UIGrayMat; }
            set { ms_UIGrayMat = value; }
        }
        //------------------------------------------------------
        public static Material GetDefaultMaterial(int index)
        {
            if (index<0 || ms_DefaultMaterials == null || index >= ms_DefaultMaterials.Length) return null;
            return ms_DefaultMaterials[index];
        }
        //------------------------------------------------------
        public static Material[] DefaultMaterials
        {
            get { return ms_DefaultMaterials; }
            set { ms_DefaultMaterials = value; }
        }
    }
}