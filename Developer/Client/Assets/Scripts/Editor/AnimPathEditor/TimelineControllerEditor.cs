//

using System.Reflection;
using TopGame.Core;
using UnityEditor;
using UnityEngine;

namespace TopGame.ED
{
    [UnityEditor.CustomEditor(typeof(TimelineController), true)]
    public class TimelineControllerEditor : Editor
    {
        EditorWindow m_pEditorWindow = null;
        private void OnEnable()
        {
            TimelineController assets = target as TimelineController;
            if (assets.playableDirector == null)
            {
                assets.playableDirector = assets.gameObject.GetComponent<UnityEngine.Playables.PlayableDirector>();
            }
            m_pEditorWindow = EditorWindow.GetWindow<EditorWindow>("AnimationWindow");
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            TimelineController assets = target as TimelineController;

            System.Reflection.FieldInfo[] fiels = assets.GetType().GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            for(int i = 0; i < fiels.Length; ++i)
            {
                if (fiels[i].IsNotSerialized) continue;
                EditorGUILayout.PropertyField(serializedObject.FindProperty(fiels[i].Name));
            }
            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("刷新保存"))
            {
                EditorUtility.SetDirty(target);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            }
            {
                if (m_pEditorWindow)
                {
                    int currentFrame = 0;
                    float curFrameTime = 0;
                    AnimationClip clip = null;
                    FieldInfo filed = m_pEditorWindow.GetType().GetField("m_AnimEditor", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    if (filed != null)
                    {
                        object animEditor = filed.GetValue(m_pEditorWindow);
                        if (animEditor != null)
                        {
                            PropertyInfo propField = animEditor.GetType().GetProperty("selection", BindingFlags.Public | BindingFlags.Instance);
                            if (propField != null)
                            {
                                object animationwindowSelectionItem = propField.GetValue(animEditor);
                                if (animationwindowSelectionItem != null)
                                {
                                    PropertyInfo clipField = animationwindowSelectionItem.GetType().GetProperty("animationClip", BindingFlags.Public | BindingFlags.Instance);
                                    if (clipField != null)
                                    {
                                        clip = clipField.GetValue(animationwindowSelectionItem) as AnimationClip;
                                    }
                                }
                            }
                            propField = animEditor.GetType().GetProperty("state", BindingFlags.Public | BindingFlags.Instance);
                            if (propField != null)
                            {
                                object animationwindowState = propField.GetValue(animEditor);
                                if (animationwindowState != null)
                                {
                                    PropertyInfo clipField = animationwindowState.GetType().GetProperty("currentTime", BindingFlags.Public | BindingFlags.Instance);
                                    if (clipField != null)
                                    {
                                        curFrameTime = (float)clipField.GetValue(animationwindowState);
                                    }
                                    clipField = animationwindowState.GetType().GetProperty("currentFrame", BindingFlags.Public | BindingFlags.Instance);
                                    if (clipField != null)
                                    {
                                        currentFrame = (int)clipField.GetValue(animationwindowState);
                                    }
                                }
                            }
                        }
                    }

                    if (clip != null)
                    {
                        if (assets.slots != null)
                        {
                            if (GUILayout.Button("Lerp绑点到第" + currentFrame.ToString() + "帧"))
                            {
                                EditorCurveBinding[] bindings = AnimationUtility.GetCurveBindings(clip);
                                if (bindings != null)
                                {
                                    for (int j = 0; j < assets.slots.Length; ++j)
                                    {
                                        if (assets.slots[j] == null) continue;
                                        if (assets.follow == assets.slots[j]) continue;
                                        string path = Framework.Core.CommonUtility.GetTransformToPath(assets.slots[j], assets.transform, false);
                                        for (int i = 0; i < bindings.Length; ++i)
                                        {
                                            if (bindings[i].path.CompareTo(path) != 0) continue;
                                            AnimationCurve curve = AnimationUtility.GetEditorCurve(clip, bindings[i]);
                                            if (UpdateCurve(bindings[i], assets.slots[j], curFrameTime, curve))
                                            {
                                                AnimationUtility.SetEditorCurve(clip, bindings[i], curve);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (assets.follow != null && CameraController.getInstance() != null && CameraController.getInstance().GetCamera() &&
                            CameraController.getInstance().GetTransform()
                            && Application.isPlaying && GUILayout.Button("Lerp相机到第" + currentFrame.ToString() + "帧"))
                        {
                            string path = Framework.Core.CommonUtility.GetTransformToPath(assets.follow, assets.transform, false);
                            EditorCurveBinding[] bindings = AnimationUtility.GetCurveBindings(clip);
                            if (bindings != null)
                            {
                                Transform pCamera = CameraController.getInstance().GetTransform();
                                for (int i = 0; i < bindings.Length; ++i)
                                {
                                    if (bindings[i].path.CompareTo(path) != 0) continue;
                                    AnimationCurve curve = AnimationUtility.GetEditorCurve(clip, bindings[i]);
                                    if (UpdateCurve(bindings[i], pCamera, curFrameTime, curve))
                                    {
                                        AnimationUtility.SetEditorCurve(clip, bindings[i], curve);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            
        }
        //------------------------------------------------------
        bool UpdateCurve(EditorCurveBinding binding, Transform pSlot, float curFrameTime, AnimationCurve curve)
        {
            bool bDirty = false;
            if (binding.propertyName.CompareTo("m_Position.x") == 0)
            {
                Framework.Core.CommonUtility.AddCurveKey(curve, curFrameTime, pSlot.position.x, 0.001f);
                bDirty = true;
            }
            else if (binding.propertyName.CompareTo("m_Position.y") == 0)
            {
                Framework.Core.CommonUtility.AddCurveKey(curve, curFrameTime, pSlot.position.y, 0.001f);
                bDirty = true;
            }
            else if (binding.propertyName.CompareTo("m_Position.z") == 0)
            {
                Framework.Core.CommonUtility.AddCurveKey(curve, curFrameTime, pSlot.position.z, 0.001f);
                bDirty = true;
            }
            else if (binding.propertyName.CompareTo("m_LocalPosition.x") == 0)
            {
                Framework.Core.CommonUtility.AddCurveKey(curve, curFrameTime, pSlot.localPosition.x, 0.001f);
                bDirty = true;
            }
            else if (binding.propertyName.CompareTo("m_LocalPosition.y") == 0)
            {
                Framework.Core.CommonUtility.AddCurveKey(curve, curFrameTime, pSlot.localPosition.y, 0.001f);
                bDirty = true;
            }
            else if (binding.propertyName.CompareTo("m_LocalPosition.z") == 0)
            {
                Framework.Core.CommonUtility.AddCurveKey(curve, curFrameTime, pSlot.localPosition.z, 0.001f);
                bDirty = true;
            }
            else if (binding.propertyName.CompareTo("eulerAnglesRaw.x") == 0)
            {
                Framework.Core.CommonUtility.AddCurveKey(curve, curFrameTime, pSlot.eulerAngles.x, 0.001f);
                bDirty = true;
            }
            else if (binding.propertyName.CompareTo("eulerAnglesRaw.y") == 0)
            {
                Framework.Core.CommonUtility.AddCurveKey(curve, curFrameTime, ClampAngle(pSlot.eulerAngles.y), 0.001f);
                bDirty = true;
            }
            else if (binding.propertyName.CompareTo("eulerAnglesRaw.z") == 0)
            {
                Framework.Core.CommonUtility.AddCurveKey(curve, curFrameTime, pSlot.eulerAngles.z, 0.001f);
                bDirty = true;
            }
            else if (binding.propertyName.CompareTo("localEulerAnglesRaw.x") == 0)
            {
                Framework.Core.CommonUtility.AddCurveKey(curve, curFrameTime, pSlot.localEulerAngles.x, 0.001f);
                bDirty = true;
            }
            else if (binding.propertyName.CompareTo("localEulerAnglesRaw.y") == 0)
            {
                Framework.Core.CommonUtility.AddCurveKey(curve, curFrameTime, ClampAngle(pSlot.localEulerAngles.y), 0.001f);
                bDirty = true;
            }
            else if (binding.propertyName.CompareTo("localEulerAnglesRaw.z") == 0)
            {
                Framework.Core.CommonUtility.AddCurveKey(curve, curFrameTime, pSlot.localEulerAngles.z, 0.001f);
                bDirty = true;
            }
            return bDirty;
        }
        //------------------------------------------------------
        private static float ClampAngle(float angle)
        {
            if (angle < -360)
                angle += 360;
            if (angle > 360)
                angle -= 360;
            return angle;
        }
    }
}