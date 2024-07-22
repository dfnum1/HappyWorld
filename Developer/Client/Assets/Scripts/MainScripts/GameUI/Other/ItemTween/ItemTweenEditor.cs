#if UNITY_EDITOR

/********************************************************************
生成日期:	5:30:2022 10:57
类    名: 	ItemTween
作    者:	hjc
描    述:	道具收集效果
*********************************************************************/
using UnityEngine;
using System;
using TopGame.Core;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEditor;

namespace TopGame.UI
{
    [CustomEditor(typeof(ItemTween), true)]
    public class ItemTweenEditor : Editor
    {
        Framework.ED.EditorTimer m_pTimer = new Framework.ED.EditorTimer();
        UI.ItemTweenManager m_pItemTweenMgr = null;
        public UI.ItemTweenManager itemTweenMgr
        {
            get
            {
                if (m_pItemTweenMgr == null)
                    m_pItemTweenMgr = new UI.ItemTweenManager();

                return m_pItemTweenMgr;
            }
        }
        private void OnEnable()
        {
            EditorApplication.update += OnPreviewUpdate;
        }
        private void OnDisable()
        {
            EditorApplication.update -= OnPreviewUpdate;
        }
        //------------------------------------------------------
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            ItemTween effector = target as ItemTween;
            effector.Icon = UnityEditor.EditorGUILayout.ObjectField("实例化对象", effector.Icon, typeof(UISerialized), true) as UISerialized;
            effector.MinSpeed = UnityEditor.EditorGUILayout.FloatField("随机初始速度最小值", effector.MinSpeed);
            effector.MaxSpeed = UnityEditor.EditorGUILayout.FloatField("随机初始速度最大值", effector.MaxSpeed);
            effector.MinDuration = UnityEditor.EditorGUILayout.FloatField("随机时长最小值", effector.MinDuration);
            effector.MaxDuration = UnityEditor.EditorGUILayout.FloatField("随机时长最大值", effector.MaxDuration);
            effector.Delay = UnityEditor.EditorGUILayout.FloatField("延迟", effector.Delay);
            effector.SpeedCurve = UnityEditor.EditorGUILayout.CurveField("速度曲线", effector.SpeedCurve);
            effector.ScaleCurve = UnityEditor.EditorGUILayout.CurveField("缩放曲线", effector.ScaleCurve);
            effector.PreviewEndPos = UnityEditor.EditorGUILayout.ObjectField("预览目标位置", effector.PreviewEndPos, typeof(Transform), true) as Transform;
            effector.PreviewNumber = UnityEditor.EditorGUILayout.IntField("预览数量", effector.PreviewNumber);
            if (GUILayout.Button("预览"))
            {
                InstanceAbleHandler ui = effector.GetComponent<InstanceAbleHandler>();
                ItemTweenManager.Tween tween = new ItemTweenManager.Tween();
                tween.tween = effector;
                tween.timer = 0;
                tween.speedTimer = 0;
                tween.items = new List<ItemTweenManager.TweenItem>();
                for (int i = 0; i < effector.PreviewNumber; i++)
                {
                    UISerialized object1 = GameObject.Instantiate(effector.Icon, effector.PreviewEndPos);
                    object1.gameObject.SetActive(true);
                    // RawImageEx默认会变透明，预览临时改成ImageEx
                    var go = new GameObject("", typeof(RectTransform), typeof(ImageEx));
                    go.transform.SetParent(object1.transform);
                    go.transform.localPosition = Vector3.zero;
                    ((RectTransform)go.transform).sizeDelta = ((RectTransform)object1.transform).sizeDelta;
                    var image = go.GetComponent<ImageEx>();
                    image.SetAssetByPath(object1.GetWidget<RawImageEx>("Icon").texturePath, null);
                    ItemTweenManager.TweenItem tweenItem = new ItemTweenManager.TweenItem();
                    tweenItem.transform = object1.transform;
                    tweenItem.startPos = effector.transform.position;
                    tweenItem.endPos = effector.PreviewEndPos.position;
                    tweenItem.duration = effector.RandomDuration();
                    tweenItem.initSpeed = effector.RandomSpeed();
                    tweenItem.UpdateA(effector.Delay);
                    tween.items.Add(tweenItem);
                }
                itemTweenMgr.AddData(tween);
            }
            if (GUILayout.Button("刷新保存"))
            {
                EditorUtility.SetDirty(target);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            }
            serializedObject.ApplyModifiedProperties();
        }
        //------------------------------------------------------
        void OnPreviewUpdate()
        {
            if (Application.isPlaying)
                return;
            m_pTimer.Update();
            if (m_pItemTweenMgr != null)
            {
                itemTweenMgr.Update(m_pTimer.deltaTime);
                if (!itemTweenMgr.IsPlaying())
                {
                    ItemTween effector = target as ItemTween;
                    if (!effector.PreviewEndPos) return;
                    for (int i = 0; i < effector.PreviewEndPos.childCount; i++)
                    {
                        GameObject.DestroyImmediate(effector.PreviewEndPos.GetChild(i).gameObject);
                    }
                }
            }
        }
    }
}
#endif