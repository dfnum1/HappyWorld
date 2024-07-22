using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TopGame.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace TopGame.ED
{
    public class PriorityTool
    {
        // [MenuItem("UIEditor/层次/最里层 " + Configure.ShortCut.MoveNodeTop)]
        public static void MoveToTopWidget()
        {
            Transform curSelect = Selection.activeTransform;
            if (curSelect != null)
            {
                curSelect.SetAsFirstSibling();
            }
        }
        // [MenuItem("UIEditor/层次/最外层 " + Configure.ShortCut.MoveNodeBottom)]
        public static void MoveToBottomWidget()
        {
            Transform curSelect = Selection.activeTransform;
            if (curSelect != null)
            {
                curSelect.SetAsLastSibling();
            }
        }

        // [MenuItem("UIEditor/层次/往里挤 " + Configure.ShortCut.MoveNodeUp)]
        public static void MoveUpWidget()
        {
            Transform curSelect = Selection.activeTransform;
            if (curSelect != null)
            {
                int curIndex = curSelect.GetSiblingIndex();
                if (curIndex > 0)
                    curSelect.SetSiblingIndex(curIndex - 1);
            }
        }

        // [MenuItem("UIEditor/层次/往外挤 " + Configure.ShortCut.MoveNodeDown)]
        public static void MoveDownWidget()
        {
            Transform curSelect = Selection.activeTransform;
            if (curSelect != null)
            {
                int curIndex = curSelect.GetSiblingIndex();
                int child_num = curSelect.parent.childCount;
                if (curIndex < child_num - 1)
                    curSelect.SetSiblingIndex(curIndex + 1);
            }
        }
    }
    //------------------------------------------------------
    public class UIAlignTool
    {
        // [MenuItem("UIEditor/对齐/左对齐 ←")]
        internal static void AlignInHorziontalLeft()
        {
            float x = Mathf.Min(Selection.gameObjects.Select(obj => obj.transform.localPosition.x).ToArray());

            foreach (GameObject gameObject in Selection.gameObjects)
            {
                gameObject.transform.localPosition = new Vector2(x,
                    gameObject.transform.localPosition.y);
            }
        }

        // [MenuItem("UIEditor/对齐/右对齐 →")]
        public static void AlignInHorziontalRight()
        {
            float x = Mathf.Max(Selection.gameObjects.Select(obj => obj.transform.localPosition.x +
            ((RectTransform)obj.transform).sizeDelta.x).ToArray());
            foreach (GameObject gameObject in Selection.gameObjects)
            {
                gameObject.transform.localPosition = new Vector3(x -
            ((RectTransform)gameObject.transform).sizeDelta.x, gameObject.transform.localPosition.y);
            }
        }

        // [MenuItem("UIEditor/对齐/上对齐 ↑")]
        public static void AlignInVerticalUp()
        {
            float y = Mathf.Max(Selection.gameObjects.Select(obj => obj.transform.localPosition.y).ToArray());
            foreach (GameObject gameObject in Selection.gameObjects)
            {
                gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, y);
            }
        }

        // [MenuItem("UIEditor/对齐/下对齐 ↓")]
        public static void AlignInVerticalDown()
        {
            float y = Mathf.Min(Selection.gameObjects.Select(obj => obj.transform.localPosition.y -
            ((RectTransform)obj.transform).sizeDelta.y).ToArray());

            foreach (GameObject gameObject in Selection.gameObjects)
            {
                gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, y + ((RectTransform)gameObject.transform).sizeDelta.y);
            }
        }


        // [MenuItem("UIEditor/对齐/水平均匀 |||")]
        public static void UniformDistributionInHorziontal()
        {
            int count = Selection.gameObjects.Length;
            float firstX = Mathf.Min(Selection.gameObjects.Select(obj => obj.transform.localPosition.x).ToArray());
            float lastX = Mathf.Max(Selection.gameObjects.Select(obj => obj.transform.localPosition.x).ToArray());
            float distance = (lastX - firstX) / (count - 1);
            var objects = Selection.gameObjects.ToList();
            objects.Sort((x, y) => (int)(x.transform.localPosition.x - y.transform.localPosition.x));
            for (int i = 0; i < count; i++)
            {
                objects[i].transform.localPosition = new Vector3(firstX + i * distance, objects[i].transform.localPosition.y);
            }
        }

        // [MenuItem("UIEditor/对齐/垂直均匀 ☰")]
        public static void UniformDistributionInVertical()
        {
            int count = Selection.gameObjects.Length;
            float firstY = Mathf.Min(Selection.gameObjects.Select(obj => obj.transform.localPosition.y).ToArray());
            float lastY = Mathf.Max(Selection.gameObjects.Select(obj => obj.transform.localPosition.y).ToArray());
            float distance = (lastY - firstY) / (count - 1);
            var objects = Selection.gameObjects.ToList();
            objects.Sort((x, y) => (int)(x.transform.localPosition.y - y.transform.localPosition.y));
            for (int i = 0; i < count; i++)
            {
                objects[i].transform.localPosition = new Vector3(objects[i].transform.localPosition.x, firstY + i * distance);
            }
        }

        // [MenuItem("UIEditor/对齐/一样大 ■")]
        public static void ResizeMax()
        {
            var height = Mathf.Max(Selection.gameObjects.Select(obj => ((RectTransform)obj.transform).sizeDelta.y).ToArray());
            var width = Mathf.Max(Selection.gameObjects.Select(obj => ((RectTransform)obj.transform).sizeDelta.x).ToArray());
            foreach (GameObject gameObject in Selection.gameObjects)
            {
                ((RectTransform)gameObject.transform).sizeDelta = new Vector2(width, height);
            }
        }

        // [MenuItem("UIEditor/对齐/一样小 ●")]
        public static void ResizeMin()
        {
            var height = Mathf.Min(Selection.gameObjects.Select(obj => ((RectTransform)obj.transform).sizeDelta.y).ToArray());
            var width = Mathf.Min(Selection.gameObjects.Select(obj => ((RectTransform)obj.transform).sizeDelta.x).ToArray());
            foreach (GameObject gameObject in Selection.gameObjects)
            {
                ((RectTransform)gameObject.transform).sizeDelta = new Vector2(width, height);
            }
        }
    }
}

