/********************************************************************
生成日期:	27:4:2022 11:50
类    名: 	ImageLine
作    者:	hjc
描    述:	直线图片
*********************************************************************/
using TopGame.UI;
using UnityEngine;
using UnityEngine.UI;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TopGame.UI
{
    public class ImageLine : Image
    {
        public float _width;
        public float width 
        {
            get { return _width; }
            set { 
                _width = value;
                SetVerticesDirty();
            }
        }
        public Vector3[] positions;
        //------------------------------------------------------
        public void SetPositions(Vector3[] positions)
        {
            this.positions = positions;
            SetVerticesDirty();
        }
        //------------------------------------------------------
        public void SetPosition(int index, Vector3 position)
        {
            this.positions[index] = position;
            SetVerticesDirty();
        }
        //------------------------------------------------------
        public Vector3 GetPosition(int index)
        {
            return this.positions[index];
        }
        //------------------------------------------------------
        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            toFill.Clear();
            if (positions == null || positions.Length == 0) return;
            Rect rect = GetPixelAdjustedRect();
            float centerX = (rect.xMin + rect.xMax)/2;
            float centerY = (rect.yMin + rect.yMax)/2;
            for (int i = 0; i < positions.Length - 1;)
            {
                float x1 = centerX + positions[i].x;
                float y1 = centerY + positions[i++].y;
                float x2 = centerX + positions[i].x;
                float y2 = centerY + positions[i++].y;
                float z = (float)Math.Sqrt((x2 - x1) * (x2 - x1)  + (y2 - y1) * (y2 - y1));
                if (z == 0) continue;
                float uvx = 1;
                if (this.sprite) uvx = z / this.sprite.rect.width;
                float offsetX = width / 2 * (y2 - y1) / z;
                float offsetY = width / 2 * (x2 - x1) / z;
                Vector3 v1 = new Vector3(x1 + offsetX, y1 - offsetY);
                Vector3 v2 = new Vector3(x1 - offsetX, y1 + offsetY);
                Vector3 v3 = new Vector3(x2 - offsetX, y2 + offsetY);
                Vector3 v4 = new Vector3(x2 + offsetX, y2 - offsetY);
                int verCnt = toFill.currentVertCount;
                toFill.AddVert(v1, this.color, Vector2.zero);
                toFill.AddVert(v2, this.color, Vector2.up);
                toFill.AddVert(v3, this.color, new Vector2(uvx, 1));
                toFill.AddVert(v4, this.color, new Vector2(uvx, 0));
                toFill.AddTriangle(verCnt, verCnt + 1, verCnt + 2);
                toFill.AddTriangle(verCnt, verCnt + 2, verCnt + 3);
            }
        }
    }


#if UNITY_EDITOR
    [CustomEditor(typeof(ImageLine), true)]
    [CanEditMultipleObjects]
    public class ImageLineEditor : UnityEditor.UI.ImageEditor
    {
        
        //------------------------------------------------------
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            ImageLine image = target as ImageLine;
            image.width = EditorGUILayout.FloatField("Width", image.width);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("positions"), new GUIContent("Positions"), true);
            serializedObject.ApplyModifiedProperties();
            base.OnInspectorGUI();
        }
    }
#endif
}
