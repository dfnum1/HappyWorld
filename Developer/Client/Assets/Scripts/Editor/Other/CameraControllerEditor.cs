#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using TopGame.Logic;
using System.IO;
using Framework.Core;

namespace TopGame.Core
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(CameraSetting), true)]
    public class CameraSettingEditor : Editor
    {
        bool m_bRecodeCameraCurve = false;
        protected float m_fTestKeyTime = 0;
        protected float m_fMaxTime = 10;
        protected float m_fCurTime = 0;
        protected float m_fRealMaxTime = 0;
        bool m_bSyncViewAndGame = false;

        bool m_bLockLookat = false;
        Transform m_pLookAtTransform = null;

        Vector3 m_BlendOffset = Vector3.zero;
        Vector3 m_BlendAixs = Vector3.zero;
        CameraCurveEventParameter m_pEventCurve = null;

        bool m_bExpandLockOffsetParam = false;

        bool m_bExpandShake = false;
        CameraShakeParameter m_ShakeParam = new CameraShakeParameter();

        CameraOffsetEventParameter m_CameraOffset = new CameraOffsetEventParameter() { fLerp = 1, bAmount = true };
        bool m_bExpandOffset = false;
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            CameraSetting cameraSetting = target as CameraSetting;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("URPCamera"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("postProcessVolume"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_arCamera"), true);
            //  EditorGUILayout.PropertyField(serializedObject.FindProperty("m_fTweenDeltaScale"), true);

            CameraController controller = CameraController.getInstance() as CameraController;
            if(controller == null)
            {
                serializedObject.ApplyModifiedProperties();
                return;
            }

            CameraMode pMode = controller.GetCurrentMode();
            controller.m_bEditor = EditorGUILayout.Toggle("编辑模式", controller.m_bEditor);
            if (controller.m_bEditor)
            {
                bool bPreSync = m_bSyncViewAndGame;
                m_bSyncViewAndGame = EditorGUILayout.Toggle("同步相机", m_bSyncViewAndGame);
                controller.m_zoomSpeed = EditorGUILayout.Slider("编辑缩放速度", controller.m_zoomSpeed, 0.01f, 50);
                controller.m_dragSpeed = EditorGUILayout.Slider("编辑拖拽速度", controller.m_dragSpeed, 0.01f, 50);
                controller.m_rotateSpeed = EditorGUILayout.Slider("编辑旋转速度", controller.m_rotateSpeed, 0.01f, 50);
                if (pMode != null)
                    pMode.SetCurrentFov(EditorGUILayout.Slider("编辑Fov", pMode.GetCurrentFov(), 10, 179), false, 0);
                if (m_bSyncViewAndGame && SceneView.lastActiveSceneView)
                {
                    if (!bPreSync)
                    {
                        SceneView.lastActiveSceneView.MoveToView(cameraSetting.transform);
                    }
                }
            }
            if (pMode != null)
            {
                if (controller.IsEditorMode())
                {
                    pMode.Update(0);
                    pMode.Blance();
                    pMode.ResetLockOffsets();
                    pMode.SetLookFocusScatter(Vector3.zero, 0, 0);
                }
                EditorGUILayout.BeginHorizontal();
                m_bExpandShake = EditorGUILayout.Foldout(m_bExpandShake, "调试震屏");
                if (GUILayout.Button("测试"))
                {
                    controller.Shake(m_ShakeParam.shake_duration, m_ShakeParam.intense, m_ShakeParam.hertz, m_ShakeParam.damping);
                }
                if (m_ShakeParam.shake_duration > 0 && GUILayout.Button("复制"))
                {
                    TextEditor t = new TextEditor();
                    t.text = m_ShakeParam.ToString();
                    t.OnFocus();
                    t.Copy();
                }
                if (!string.IsNullOrEmpty(GUIUtility.systemCopyBuffer) && GUILayout.Button("黏贴"))
                {
                    CameraShakeParameter shake = BuildEventUtl.BuildEvent(null, GUIUtility.systemCopyBuffer) as CameraShakeParameter;
                    if (shake == null)
                    {
                        EditorUtility.DisplayDialog("提示", "不是一个有效的事件参数数据", "Ok");
                    }
                    else
                        m_ShakeParam = shake;
                }
                EditorGUILayout.EndHorizontal();
                if (m_bExpandShake)
                {
                    m_ShakeParam.shake_duration = EditorGUILayout.FloatField("震动时长", m_ShakeParam.shake_duration);
                    m_ShakeParam.intense = EditorGUILayout.Vector3Field("震动强度", m_ShakeParam.intense);
                    m_ShakeParam.hertz = EditorGUILayout.Vector3Field("震动振幅(sin角度)", m_ShakeParam.hertz);
                    if (m_ShakeParam.damping == null)
                        m_ShakeParam.damping = new AnimationCurve();
                    m_ShakeParam.damping = EditorGUILayout.CurveField("衰减值", m_ShakeParam.damping);
                }

            }
            if (pMode is BattleCameraMode)
            {
                BattleCameraMode pBattle = pMode as BattleCameraMode;
                //           if (pBattle.GetLookAtActor() != null)
                {
                    //先去除
                    //                     Runer pRuner = AState.CastCurrentMode<Runer>();
                    //                     CameraFollow pFollow = Logic.Battle.CastCurrentModeLogic<Logic.CameraFollow>();
                    //                     if (pRuner !=null && pFollow != null && GUILayout.Button("还原关卡参数"))
                    //                     {
                    //                         ushort sceneId = (ushort)(pRuner.IsStarted() ? 6 : 3);
                    //                         CameraFollow follow = AState.CastCurrentModeLogic<CameraFollow>();
                    //                         if (follow != null) follow.Clear();
                    //                         pFollow.ApplayCamera(sceneId, !pRuner.IsStarted(), true, false);
                    //                     }
                    if (controller.m_bEditor)
                        pBattle.Blance();
                    float fCurDis = pBattle.GetFollowDistance();
                    float fMin = pBattle.GetMinDistance();
                    float fMax = pBattle.GetMaxDistance();
                    fMin = EditorGUILayout.FloatField("最小距离", fMin);
                    fMax = EditorGUILayout.FloatField("最大距离", fMax);

                    if (controller.m_bEditor)
                    {
                        pBattle.SetFollowLimit(fMin, fMax);

                        pBattle.SetFollowDistance(EditorGUILayout.FloatField("当前视距", fCurDis), false);
                    }
                    Vector3 eulerAngle = Framework.Core.CommonUtility.ClampAngle(controller.GetEulerAngle());// EditorGUILayout.Vector3Field("当前角度",Framework.Core.CommonUtility.ClampAngle(controller.GetEulerAngle()));
                    Vector3 looatpos_offset = pBattle.GetCurrentLookAtOffset();
                    controller.SetEditor(controller.m_bEditor);

                    if (controller.m_bEditor)
                    {
                        if (GUILayout.Button("使用"))
                        {
                            pBattle.ResetLockOffsets();
                            controller.SetEditor(false);
                            ApplayBattleCamera(controller, pBattle);
                        }
                        if (GUILayout.Button("复制"))
                        {
                            controller.SetEditor(false);
                            ApplayBattleCamera(controller, pBattle);
                            eulerAngle = Framework.Core.CommonUtility.ClampAngle(controller.GetEulerAngle());
                            looatpos_offset = pBattle.GetCurrentLookAtOffset();
                            controller.SetEditor(true);
                            string strText = pBattle.GetMinDistance().ToString() + "|";
                            strText += pBattle.GetMaxDistance().ToString() + "|";
                            strText += pBattle.GetFollowDistance() + "|";
                            strText += string.Format("{0:f6}|{1:f6}|{2:f6}|", looatpos_offset.x, looatpos_offset.y, looatpos_offset.z);
                            strText += string.Format("{0:f6}|{1:f6}|{2:f6}|", eulerAngle.x, eulerAngle.y, eulerAngle.z);
                            strText += pBattle.GetCurrentFov();
                            TextEditor t = new TextEditor();
                            t.text = strText;
                            t.OnFocus();
                            t.Copy();

                            CameraEventParameter eventParam = new CameraEventParameter();
                            eventParam.fLerp = 10;
                            eventParam.fov = pBattle.GetCurrentFov();
                            eventParam.distance = pBattle.GetFollowDistance();
                            eventParam.eulerAngle = eulerAngle;
                            eventParam.lookOffset = looatpos_offset;
                            eventParam.transOffset = Vector3.zero;
                            eventParam.level = 0;
                            SaveCameraEventParamCatch(eventParam);
                        }
                        DrawCameraOffsetParam(controller, pBattle);
                    }
                }
            }
            else if (pMode is HallCameraMode)
            {
                HallCameraMode pHall = pMode as HallCameraMode;
                {
                    m_bLockLookat = EditorGUILayout.Toggle("锁定目标", m_bLockLookat);
                    EditorGUILayout.BeginHorizontal();
                    Vector3 curPos = EditorGUILayout.Vector3Field("相机位置", controller.GetPosition());
                    if (GUILayout.Button("复制"))
                    {
                        TextEditor t = new TextEditor();
                        t.text = string.Format("{0:F2}|{1:F2}|{2:F2}", curPos.x, curPos.y, curPos.z);
                        t.OnFocus();
                        t.Copy();
                    }
                    EditorGUILayout.EndHorizontal();

                    Vector3 curLookat = GetCurrentLookAt(curPos, controller.GetDir(), pHall.GetFollowLookAtPosition().y);

                    float curDist = pHall.GetFollowDistance();
                    Vector3 curLookAtOffset = Vector3.zero;
                    if (m_bLockLookat)
                    {

                        EditorGUILayout.BeginHorizontal();
                        curDist = EditorGUILayout.FloatField("当前视距", curDist);
                        pHall.SetFollowDistance(curDist, true, false);
                        if (GUILayout.Button("复制"))
                        {
                            TextEditor t = new TextEditor();
                            t.text = string.Format("{0:F2}", curDist);
                            t.OnFocus();
                            t.Copy();
                        }
                        EditorGUILayout.EndHorizontal();


                        pHall.SetCurrentLookAtOffset(Vector3.zero);
                        curLookat = pHall.GetFollowLookAtPosition();
                        controller.GetTransform().position = curLookat - controller.GetDir() * curDist;

                        EditorGUILayout.BeginHorizontal();
                        curLookat = EditorGUILayout.Vector3Field("视点偏移", curLookat);
                        if (GUILayout.Button("复制"))
                        {
                            TextEditor t = new TextEditor();
                            t.text = string.Format("{0:F2}|{1:F2}|{2:F2}", curLookat.x, curLookat.y, curLookat.z);
                            t.OnFocus();
                            t.Copy();
                        }
                        pHall.SetFollowLookAtPosition(curLookat);
                        EditorGUILayout.EndHorizontal();

                        if (Event.current.type == EventType.ScrollWheel)
                        {
                            curDist += Event.current.delta.y;
                            pHall.SetFollowDistance(curDist, true, false);
                        }
                    }
                    else
                    {
                        EditorGUILayout.BeginHorizontal();
                        curLookAtOffset = curLookat - pHall.GetFollowLookAtPosition();
                        curLookAtOffset = EditorGUILayout.Vector3Field("视点偏移", curLookAtOffset);
                        if (GUILayout.Button("复制"))
                        {
                            TextEditor t = new TextEditor();
                            t.text = string.Format("{0:F2}|{1:F2}|{2:F2}", curLookAtOffset.x, curLookAtOffset.y, curLookAtOffset.z);
                            t.OnFocus();
                            t.Copy();
                        }
                        EditorGUILayout.EndHorizontal();
                    }


                    if (!m_bLockLookat)
                    {
                        EditorGUILayout.BeginHorizontal();
                        curDist = (curLookat - curPos).magnitude;
                        curDist = EditorGUILayout.FloatField("当前视距", curDist);
                        if (GUILayout.Button("复制"))
                        {
                            TextEditor t = new TextEditor();
                            t.text = string.Format("{0:F2}", curDist);
                            t.OnFocus();
                            t.Copy();
                        }
                        EditorGUILayout.EndHorizontal();

                    }
                    EditorGUILayout.BeginHorizontal();
                    Vector3 eulerAngle = EditorGUILayout.Vector3Field("当前角度", controller.GetEulerAngle());
                    if (GUILayout.Button("复制"))
                    {
                        TextEditor t = new TextEditor();
                        t.text = string.Format("{0:F2}|{1:F2}|{2:F2}", eulerAngle.x, eulerAngle.y, eulerAngle.z);
                        t.OnFocus();
                        t.Copy();
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    Vector2 axisX = EditorGUILayout.Vector2Field("X 角度限制", pHall.GetAxisXClamp());
                    pHall.SetAxisXClamp(axisX);
                    if (GUILayout.Button("复制"))
                    {
                        TextEditor t = new TextEditor();
                        t.text = string.Format("{0:F2}|{1:F2}", axisX.x, axisX.y);
                        t.OnFocus();
                        t.Copy();
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    Vector2 axisY = EditorGUILayout.Vector2Field("Y 角度限制", pHall.GetAxisYClamp());
                    pHall.SetAxisYClamp(axisY);
                    if (GUILayout.Button("复制"))
                    {
                        TextEditor t = new TextEditor();
                        t.text = string.Format("{0:F2}|{1:F2}", axisY.x, axisY.y);
                        t.OnFocus();
                        t.Copy();
                    }
                    EditorGUILayout.EndHorizontal();

                    controller.SetEditor(controller.m_bEditor);

                    if (controller.m_bEditor)
                    {
                        EditorGUILayout.BeginHorizontal();
                        m_pLookAtTransform = EditorGUILayout.ObjectField("相对参考点", m_pLookAtTransform, typeof(Transform)) as Transform;
                        if (m_pLookAtTransform != null && GUILayout.Button("复制聚焦参数"))
                        {
                            Vector3 cpLook = GetCurrentLookAt(controller.GetPosition(), controller.GetDir(), m_pLookAtTransform.position.y) - m_pLookAtTransform.position;
                            Vector3 cpPos = controller.GetPosition() - m_pLookAtTransform.position;
                            string strText = "cameraview:";
                            strText += string.Format("{0:f2}|{1:f2}|{2:f3}|", cpLook.x, cpLook.y, cpLook.z);
                            strText += string.Format("{0:f2}|{1:f2}|{2:f3}|", cpPos.x, cpPos.y, cpPos.z);
                            TextEditor t = new TextEditor();
                            t.text = strText;
                            t.OnFocus();
                            t.Copy();
                        }
                        EditorGUILayout.EndHorizontal();
                        if (m_pLookAtTransform != null)
                        {
                            Vector3 cpLook = GetCurrentLookAt(controller.GetPosition(), controller.GetDir(), m_pLookAtTransform.position.y) - m_pLookAtTransform.position;
                            EditorGUILayout.Vector3Field("相对目标偏移", cpLook);
                            EditorGUILayout.Vector3Field("相对目标位置", controller.GetPosition() - m_pLookAtTransform.position);
                        }

                        if (GUILayout.Button("使用"))
                        {
                            controller.SetEditor(false);
                            pHall.ResetLockOffsets();
                            pHall.SetCurrentTransOffset(Vector3.zero, false);
                            pHall.SetCurrentLookAtOffset(curLookAtOffset);
                            pHall.SetFollowDistance(curDist, true, false);
                            pHall.SetCurrentEulerAngle(controller.GetEulerAngle());
                        }
                    }
                }
            }
            else if (pMode == null)
            {
                controller.m_bEditor = EditorGUILayout.Toggle("编辑模式", controller.m_bEditor);
                EditorGUILayout.BeginHorizontal();

                Vector3 position_offset = EditorGUILayout.Vector3Field("相机位置偏移", controller.GetPosition());
                if (GUILayout.Button("复制"))
                {
                    TextEditor t = new TextEditor();
                    t.text = string.Format("{0:F2}|{1:F2}|{2:F2}", position_offset.x, position_offset.y, position_offset.z);
                    t.OnFocus();
                    t.Copy();
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                Vector3 eulerAngle = EditorGUILayout.Vector3Field("当前角度", controller.GetEulerAngle());
                if (GUILayout.Button("复制"))
                {
                    TextEditor t = new TextEditor();
                    t.text = string.Format("{0:F2}|{1:F2}|{2:F2}", eulerAngle.x, eulerAngle.y, eulerAngle.z);
                    t.OnFocus();
                    t.Copy();
                }
                EditorGUILayout.EndHorizontal();

                controller.SetEditor(controller.m_bEditor);
            }

            if (controller.m_bEditor)
            {
                if (Base.GlobalShaderController.Instance)
                {
                    EditorGUI.BeginChangeCheck();
                    m_BlendAixs.x = EditorGUILayout.FloatField("横向曲线渲染", m_BlendAixs.x);
                    m_BlendAixs.y = EditorGUILayout.FloatField("纵向曲线渲染", m_BlendAixs.y);
                    m_BlendOffset.z = EditorGUILayout.FloatField("深度曲线渲染偏移", m_BlendOffset.z);
                    if (EditorGUI.EndChangeCheck())
                    {
                        Base.GlobalShaderController.SetBlend(m_BlendAixs, m_BlendOffset);
                    }
                }


                if (GUILayout.Button("相机曲线编辑"))
                {
                    CameraSplineEventEditor.EditFrame(controller.GetCamera());
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
        //------------------------------------------------------
        static Vector3 GetCurrentLookAt(Vector3 pos, Vector3 dir, float floorY)
        {
            Vector3 curLookAt = Framework.Core.CommonUtility.RayHitPos(pos, dir, floorY);
            if (Vector3.Dot(dir, curLookAt - pos) < 0)
            {
                float dist = Vector3.Distance(curLookAt, pos);
                curLookAt = pos + (pos - curLookAt).normalized * dist;
            }
            return curLookAt;
        }
        //------------------------------------------------------
        void DrawCameraOffsetParam(CameraController controller, CameraMode mode)
        {
            if (controller.GetCamera() == null) return;
            Vector3 modePos = mode.GetCurrentTrans();
            Vector3 modeLookAt = mode.GetCurrentLookAt();
            float modeDist = Vector3.Distance(modePos, modeLookAt);
            Vector3 modeEuler = Framework.Core.CommonUtility.ClampAngle(mode.GetCurrentEulerAngle());
            float modeFov = mode.GetCurrentFov();

            Vector3 followPos = mode.GetFollowLookAtPosition();

            Vector3 curPos = controller.GetPosition();
            Vector3 curLookAt = GetCurrentLookAt(curPos, controller.GetDir(), mode.GetFollowLookAtPosition().y);

            Vector3 curEuler = Framework.Core.CommonUtility.ClampAngle(controller.GetEulerAngle());
            float curDist = Vector3.Distance(curLookAt, curPos);
            float curFov = controller.GetCamera().fieldOfView;

            GUILayout.BeginHorizontal();
            m_bExpandOffset = EditorGUILayout.Foldout(m_bExpandOffset, "偏移参数");
            if (GUILayout.Button("构建相机偏移事件"))
            {
                m_CameraOffset.useBit = 0;
                m_CameraOffset.bAmount = true;
                if ((curLookAt - modeLookAt).magnitude > 0.01f)
                {
                    m_CameraOffset.useBit |= (int)CameraOffsetEventParameter.EType.LookAtOffset;
                    m_CameraOffset.lookOffset = curLookAt - modeLookAt;
                }
                if (Mathf.Abs(curDist - modeDist) > 0.01f)
                {
                    m_CameraOffset.useBit |= (int)CameraOffsetEventParameter.EType.Distance;
                    m_CameraOffset.distance = curDist - modeDist;
                }
                if (Mathf.Abs(Framework.Core.CommonUtility.ClampAngle(modeEuler - curEuler).x) > 0.01f)
                {
                    m_CameraOffset.useBit |= (int)CameraOffsetEventParameter.EType.Yaw;
                    m_CameraOffset.yaw = Framework.Core.CommonUtility.ClampAngle(modeEuler - curEuler).x;
                }
                if (Mathf.Abs(Framework.Core.CommonUtility.ClampAngle(curEuler - modeEuler).y) > 0.01f)
                {
                    m_CameraOffset.useBit |= (int)CameraOffsetEventParameter.EType.Pitch;
                    m_CameraOffset.pitch = (curEuler - modeEuler).y;
                }
                if (Mathf.Abs(Framework.Core.CommonUtility.ClampAngle(curEuler - modeEuler).z) > 0.01f)
                {
                    m_CameraOffset.useBit |= (int)CameraOffsetEventParameter.EType.Roll;
                    m_CameraOffset.roll = Framework.Core.CommonUtility.ClampAngle(curEuler - modeEuler).z;
                }
                if (Mathf.Abs((curFov - modeFov)) > 0.01f)
                {
                    m_CameraOffset.useBit |= (int)CameraOffsetEventParameter.EType.Fov;
                    m_CameraOffset.fov = curFov - modeFov;
                }

                SaveCameraOffsetCatch(m_CameraOffset);
            }
            if (CatchCopyCatchCameraOffsetDatas() && GUILayout.Button("测试"))
            {
                controller.m_bEditor = false;
                GameInstance.getInstance().OnTriggerEvent(m_CameraOffset);
            }
            GUILayout.EndHorizontal();
            if (m_bExpandOffset)
            {
                m_CameraOffset.bAmount = true;
                m_CameraOffset.fLerp = EditorGUILayout.FloatField("过渡速度", m_CameraOffset.fLerp);
                GUILayout.BeginHorizontal();
                EditorGUILayout.Vector3Field("当前位置偏移", curPos - modePos);
                if (GUILayout.Button("复制"))
                {
                    TextEditor t = new TextEditor();
                    Vector3 diff = (curPos - modePos);
                    t.text = string.Format("{0:F2}|{1:F2}|{2:F2}", diff.x, diff.y, diff.z);
                    t.OnFocus();
                    t.Copy();
                }
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                EditorGUILayout.FloatField("当前视距", curDist);
                if (GUILayout.Button("复制"))
                {
                    TextEditor t = new TextEditor();
                    t.text = curDist.ToString();
                    t.OnFocus();
                    t.Copy();
                }
                GUILayout.EndHorizontal();


                GUILayout.BeginHorizontal();
                EditorGUILayout.FloatField("当前视距偏移", curDist - modeDist);
                if (GUILayout.Button("复制"))
                {
                    TextEditor t = new TextEditor();
                    t.text = (curDist - modeDist).ToString();
                    t.OnFocus();
                    t.Copy();
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                EditorGUILayout.Vector3Field("当前视点位置偏移", curLookAt - followPos);
                if (GUILayout.Button("复制"))
                {
                    TextEditor t = new TextEditor();
                    Vector3 diff = (curLookAt - followPos);
                    t.text = string.Format("{0:F2}|{1:F2}|{2:F2}", diff.x, diff.y, diff.z);
                    t.OnFocus();
                    t.Copy();
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                EditorGUILayout.FloatField("当前仰俯角偏移", Framework.Core.CommonUtility.ClampAngle(modeEuler - curEuler).x);
                if (GUILayout.Button("复制"))
                {
                    TextEditor t = new TextEditor();
                    t.text = Framework.Core.CommonUtility.ClampAngle(modeEuler - curEuler).x.ToString();
                    t.OnFocus();
                    t.Copy();
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                EditorGUILayout.Vector3Field("当前角度", curEuler);
                if (GUILayout.Button("复制"))
                {
                    TextEditor t = new TextEditor();
                    t.text = string.Format("{0:F2}|{1:F2}|{2:F2}", curEuler.x, curEuler.y, curEuler.z);
                    t.OnFocus();
                    t.Copy();
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                EditorGUILayout.FloatField("当前偏航角偏移", Framework.Core.CommonUtility.ClampAngle(curEuler - modeEuler).y);
                if (GUILayout.Button("复制"))
                {
                    TextEditor t = new TextEditor();
                    t.text = Framework.Core.CommonUtility.ClampAngle(curEuler - modeEuler).y.ToString();
                    t.OnFocus();
                    t.Copy();
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                EditorGUILayout.FloatField("当前翻滚角偏移", Framework.Core.CommonUtility.ClampAngle(curEuler - modeEuler).z);
                if (GUILayout.Button("复制"))
                {
                    TextEditor t = new TextEditor();
                    t.text = Framework.Core.CommonUtility.ClampAngle(curEuler - modeEuler).z.ToString();
                    t.OnFocus();
                    t.Copy();
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                EditorGUILayout.FloatField("当前广角", curFov);
                if (GUILayout.Button("复制"))
                {
                    TextEditor t = new TextEditor();
                    t.text = curFov.ToString();
                    t.OnFocus();
                    t.Copy();
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                EditorGUILayout.FloatField("当前广角偏移", curFov - modeFov);
                if (GUILayout.Button("复制"))
                {
                    TextEditor t = new TextEditor();
                    t.text = (curFov - modeFov).ToString();
                    t.OnFocus();
                    t.Copy();
                }
                GUILayout.EndHorizontal();
            }
        }
        //------------------------------------------------------
        public static bool CatchCopyCatchCameraOffsetDatas()
        {
            string strTempFile = Application.dataPath + "/../EditorData/TempCameraOffset.json";
            return File.Exists(strTempFile);
        }
        //------------------------------------------------------
        public static CameraOffsetEventParameter BuildCatchCameraOffsetEvent()
        {
            string strTempFile = Application.dataPath + "/../EditorData/TempCameraOffset.json";
            if (!File.Exists(strTempFile)) return null;
            try
            {
                CameraOffsetEventParameter data = JsonUtility.FromJson<CameraOffsetEventParameter>(File.ReadAllText(strTempFile));
                return data;

            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.ToString());
                return null;
            }
        }
        //------------------------------------------------------
        public static void SaveCameraOffsetCatch(CameraOffsetEventParameter cameraOffset)
        {
            string strTempFile = Application.dataPath + "/../EditorData/TempCameraOffset.json";
            if (Directory.Exists(Application.dataPath + "/../EditorData/"))
                Directory.CreateDirectory(Application.dataPath + "/../EditorData");
            if (File.Exists(strTempFile))
                File.Delete(strTempFile);
            FileStream fs = new FileStream(strTempFile, FileMode.OpenOrCreate);
            StreamWriter writer = new StreamWriter(fs, System.Text.Encoding.UTF8);
            writer.Write(JsonUtility.ToJson(cameraOffset, true));
            writer.Close();
        }
        //------------------------------------------------------
        public static void SaveCameraEventParamCatch(CameraEventParameter cameraEvent)
        {
            string strTempFile = Application.dataPath + "/../EditorData/TempCameraEventParam.json";
            if (Directory.Exists(Application.dataPath + "/../EditorData/"))
                Directory.CreateDirectory(Application.dataPath + "/../EditorData");
            if (File.Exists(strTempFile))
                File.Delete(strTempFile);
            FileStream fs = new FileStream(strTempFile, FileMode.OpenOrCreate);
            StreamWriter writer = new StreamWriter(fs, System.Text.Encoding.UTF8);
            writer.Write(JsonUtility.ToJson(cameraEvent, true));
            writer.Close();
        }
        //------------------------------------------------------
        public static CameraEventParameter BuildCatchCameraEventParam()
        {
            string strTempFile = Application.dataPath + "/../EditorData/TempCameraEventParam.json";
            if (!File.Exists(strTempFile)) return null;
            try
            {
                CameraEventParameter data = JsonUtility.FromJson<CameraEventParameter>(File.ReadAllText(strTempFile));
                return data;

            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.ToString());
                return null;
            }
        }
        //---------------------------------------
        public static void ApplayBattleCamera(ICameraController controll, BattleCameraMode mode)
        {
            Vector3 pos = controll.GetPosition();
            Vector3 forward = controll.GetEulerAngle();
            Vector3 up = controll.GetUp();
            ApplayBattleCamera(pos, forward, up, mode);
        }
        //---------------------------------------
        public static void ApplayBattleCamera(Vector3 pos, Vector3 eulerAngle, Vector3 up, BattleCameraMode mode)
        {
            if (mode == null) return;

            Vector3 lookat = GetCurrentLookAt(pos, Framework.Core.CommonUtility.EulersAngleToDirection(eulerAngle), mode.GetFollowLookAtPosition().y);

            mode.ResetLockOffsets();
            mode.SetCurrentLookAtOffset(lookat - mode.GetFollowLookAtPosition());
            mode.SetCurrentTransOffset(Vector3.zero);
            mode.SetFollowDistance(Vector3.Distance(lookat, pos), true);
            mode.SetCurrentEulerAngle(eulerAngle);
            mode.Start();
        }
        //---------------------------------------
        void OnSceneGUI()
        {
            CameraSetting camreaSetting = target as CameraSetting;
            CameraController controller = CameraController.getInstance() as CameraController;
            if (controller == null) return;
            CameraMode pMode = controller.GetCurrentMode();
            if (pMode == null) return;

            if (controller.m_bEditor && Event.current.type == EventType.MouseUp)
            {
                controller.SyncEditPos(camreaSetting.transform.position);
                controller.SyncEditEuler(camreaSetting.transform.eulerAngles);
            }
            if (m_bRecodeCameraCurve)
            {
                m_pEventCurve.DrawLine(m_fRealMaxTime, pMode.GetCurrentLookAt(), pMode.GetCurrentTrans(), Color.white, Color.red, Color.yellow);
            }
            Color color = Handles.color;
            Handles.color = Color.red;
            Vector3 lookat = GetCurrentLookAt(controller.GetPosition(), controller.GetDir(), pMode.GetFollowLookAtPosition().y);
            Handles.SphereHandleCap(0, lookat, Quaternion.identity, Mathf.Min(0.5f, HandleUtility.GetHandleSize(lookat)), EventType.Repaint);
            Handles.DrawLine(controller.GetPosition(), lookat);
            Handles.color = color;

            if (m_bSyncViewAndGame && controller.m_bEditor && SceneView.currentDrawingSceneView)
            {
                Camera camera = SceneView.currentDrawingSceneView.camera;
                camreaSetting.transform.position = camera.transform.position;
                camreaSetting.transform.rotation = camera.transform.rotation;
                controller.SyncEditPos(camera.transform.position);
                controller.SyncEditEuler(camera.transform.eulerAngles);
            }
        }
    }
}
#endif