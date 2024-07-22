/********************************************************************
生成日期:	12:28:2020 13:16
类    名: 	InvSlicedImage
作    者:	Happli
描    述:	逆向九宫
*********************************************************************/

using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;

namespace TopGame.UI
{
    [UIWidgetExport]
    [ExecuteAlways]
    public class InvSlicedImage : Image
    {
        public float horizontalCenter = 0.5f;
        public float verticalCenter = 0.5f;
        [NonSerialized]
        private Sprite m_OverrideSprite;
        private Sprite activeSprite { get { return m_OverrideSprite != null ? m_OverrideSprite : sprite; } }
        static readonly Vector2[] s_VertScratch = new Vector2[4];
        static readonly Vector2[] s_UVScratch = new Vector2[4];
        static void AddQuad(VertexHelper vertexHelper, Vector2 posMin, Vector2 posMax, Color32 color, Vector2 uvMin, Vector2 uvMax)
        {
            int startIndex = vertexHelper.currentVertCount;

            vertexHelper.AddVert(new Vector3(posMin.x, posMin.y, 0), color, new Vector2(uvMin.x, uvMin.y));
            vertexHelper.AddVert(new Vector3(posMin.x, posMax.y, 0), color, new Vector2(uvMin.x, uvMax.y));
            vertexHelper.AddVert(new Vector3(posMax.x, posMax.y, 0), color, new Vector2(uvMax.x, uvMax.y));
            vertexHelper.AddVert(new Vector3(posMax.x, posMin.y, 0), color, new Vector2(uvMax.x, uvMin.y));

            vertexHelper.AddTriangle(startIndex, startIndex + 1, startIndex + 2);
            vertexHelper.AddTriangle(startIndex + 2, startIndex + 3, startIndex);
        }
        private Vector4 GetAdjustedBorders(Vector4 border, Rect adjustedRect)
        {
            Rect originalRect = rectTransform.rect;

            for (int axis = 0; axis <= 1; axis++)
            {
                float borderScaleRatio;

                // The adjusted rect (adjusted for pixel correctness)
                // may be slightly larger than the original rect.
                // Adjust the border to match the adjustedRect to avoid
                // small gaps between borders (case 833201).
                if (originalRect.size[axis] != 0)
                {
                    borderScaleRatio = adjustedRect.size[axis] / originalRect.size[axis];
                    border[axis] *= borderScaleRatio;
                    border[axis + 2] *= borderScaleRatio;
                }

                // If the rect is smaller than the combined borders, then there's not room for the borders at their normal size.
                // In order to avoid artefacts with overlapping borders, we scale the borders down to fit.
                float combinedBorders = border[axis] + border[axis + 2];
                if (adjustedRect.size[axis] < combinedBorders && combinedBorders != 0)
                {
                    borderScaleRatio = adjustedRect.size[axis] / combinedBorders;
                    border[axis] *= borderScaleRatio;
                    border[axis + 2] *= borderScaleRatio;
                }
            }
            return border;
        }
        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            if (overrideSprite == null)
            {
                base.OnPopulateMesh(toFill);
                return;
            }
            if(this.type == Type.Sliced && hasBorder)
            {
                Vector4 vector4_1;
                Vector4 vector4_2;
                Vector4 vector4_3;
                Vector4 vector4_4;
                if ((UnityEngine.Object) this.activeSprite != (UnityEngine.Object) null)
                {
                    vector4_1 = UnityEngine.Sprites.DataUtility.GetOuterUV(this.activeSprite);
                    vector4_2 = UnityEngine.Sprites.DataUtility.GetInnerUV(this.activeSprite);
                    vector4_3 = UnityEngine.Sprites.DataUtility.GetPadding(this.activeSprite);
                    vector4_4 = this.activeSprite.border;
                }
                else
                {
                    vector4_1 = Vector4.zero;
                    vector4_2 = Vector4.zero;
                    vector4_3 = Vector4.zero;
                    vector4_4 = Vector4.zero;
                }
                Rect pixelAdjustedRect = this.GetPixelAdjustedRect();
                Vector4 adjustedBorders = this.GetAdjustedBorders(vector4_4 / this.pixelsPerUnit, pixelAdjustedRect);
                Vector4 vector4_5 = vector4_3 / this.pixelsPerUnit;
            
                float outWidth = pixelAdjustedRect.width - vector4_5.x - vector4_5.z - this.activeSprite.rect.width + adjustedBorders.x + adjustedBorders.z;
                float leftWidth = outWidth * horizontalCenter;
                float rightWidth = outWidth - leftWidth;
            
                float outHeight = pixelAdjustedRect.height - vector4_5.y - vector4_5.w - this.activeSprite.rect.height + adjustedBorders.y + adjustedBorders.w;
                float topHeight = outHeight * verticalCenter;
                float bottomHeight = outHeight - topHeight;
                
                
                s_VertScratch[0] = new Vector2(vector4_5.x, vector4_5.y);
                s_VertScratch[3] = new Vector2(pixelAdjustedRect.width - vector4_5.z, pixelAdjustedRect.height - vector4_5.w);
                s_VertScratch[1].x = leftWidth;//adjustedBorders.x;
                s_VertScratch[1].y = topHeight;//adjustedBorders.y;
                s_VertScratch[2].x = pixelAdjustedRect.width - rightWidth;//adjustedBorders.z;
                s_VertScratch[2].y = pixelAdjustedRect.height - bottomHeight;//adjustedBorders.w;
                for (int index = 0; index < 4; ++index)
                {
                    s_VertScratch[index].x += pixelAdjustedRect.x;
                    s_VertScratch[index].y += pixelAdjustedRect.y;
                }
                s_UVScratch[0] = new Vector2(vector4_1.x, vector4_1.y);
                s_UVScratch[1] = new Vector2(vector4_2.x, vector4_2.y);
                s_UVScratch[2] = new Vector2(vector4_2.z, vector4_2.w);
                s_UVScratch[3] = new Vector2(vector4_1.z, vector4_1.w);
                toFill.Clear();
                for (int index1 = 0; index1 < 3; ++index1)
                {
                    int index2 = index1 + 1;
                    for (int index3 = 0; index3 < 3; ++index3)
                    {
                        int index4 = index3 + 1;
                        AddQuad(toFill, 
                        new Vector2(s_VertScratch[index1].x, s_VertScratch[index3].y), 
                        new Vector2(s_VertScratch[index2].x, s_VertScratch[index4].y), 
                        (Color32) this.color, 
                        new Vector2(s_UVScratch[index1].x, s_UVScratch[index3].y), 
                        new Vector2(s_UVScratch[index2].x, s_UVScratch[index4].y)
                        );
                    }
                }
                return;
            }
            base.OnPopulateMesh(toFill);
        }
    }
}