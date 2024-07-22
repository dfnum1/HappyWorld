/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	TeamFormationData
作    者:	HappLI
描    述:	队伍阵型数据
*********************************************************************/
using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace TopGame.Data
{
   // [CreateAssetMenu]
    public class TeamFormationData : ScriptableObject
    {
        [System.Serializable]
        public struct SplineFrame : Framework.Plugin.IQuickSort<SplineFrame>
        {
            public float time;
            public Vector3 position;
            public Vector3 inTan;
            public Vector3 outTan;
            //-----------------------------------------------------
            public int CompareTo(int type,SplineFrame other)
            {
                if (time < other.time) return -1;
                if (time > other.time) return 1;
                return 0;
            }
            //-----------------------------------------------------
            public void Destroy()
            {
            }
        }

        [System.Serializable]
        public struct Grid
        {
            public Vector3 Offset;
            public float Scale;
            public short nRow;
        }

        [System.Serializable]
        public class GridConfig
        {
            public Vector3 Offset = Vector3.zero;
            public List<Grid> vGrid = new List<Grid>();
        }

        public float          SwapSpeed = 1f;
        public bool           SwapPoint = false;
        public AnimationCurve SwapForwad;
        public AnimationCurve SwapBack;

        public List<SplineFrame> TurnLeaveSplines = null;
        public List<SplineFrame> TurnEnterSplines = null;

        public Vector3[]          StanceProjectileSpeedApends;
        
        [System.Serializable]
        public class CircleConfig
        {
            public float leaderScale = -1;
            public float memberScale = -1;
            public float radius = 1;
            public float turnSpeed = 1;
            public float turnHeadUISpeed = 0.25f;
            public int partitionCnt = 3;
            public bool showLeaderHalo = true;
            public bool onlyShowLeader = true;
            public bool onlyLeaderDoSkill = true;
            public Vector3 offset;
            public Vector4[] partitionOffsets = null;
#if UNITY_EDITOR
            [System.NonSerialized]
            public bool bExpand = false;
#endif
        }
        public CircleConfig[] circleTeams = new CircleConfig[2];
        public GridConfig gridTeams = new GridConfig();

        static TeamFormationData ms_Instance = null;
        private void OnEnable()
        {
            ms_Instance = this;
        }
        //------------------------------------------------------
        private void OnDestroy()
        {
            ms_Instance = null;
        }
        //------------------------------------------------------
        public static bool IsValid()
        {
            return ms_Instance != null;
        }
        //------------------------------------------------------
        public static bool IsSwapPoint
        {
            get
            {
                if (ms_Instance == null) return false;
                return ms_Instance.SwapPoint;
            }
        }
        //------------------------------------------------------
        public static CircleConfig GetCircleFormat(int index)
        {
                if (ms_Instance == null || index< 0 || ms_Instance.circleTeams == null || index>= ms_Instance.circleTeams.Length) return null;
                return ms_Instance.circleTeams[index];
        }
        //------------------------------------------------------
        public static float GetSwapSpeed()
        {
            if (ms_Instance == null) return 0;
            return ms_Instance.SwapSpeed;
        }
        //------------------------------------------------------
        public static AnimationCurve GetSwapForwadCurve()
        {
            if (ms_Instance == null) return null;
            return ms_Instance.SwapForwad;
        }
        //------------------------------------------------------
        public static AnimationCurve GetSwapBackCurve()
        {
            if (ms_Instance == null) return null;
            return ms_Instance.SwapBack;
        }
        //------------------------------------------------------
        public static int GetMaxCount()
        {
            if (ms_Instance == null || ms_Instance.gridTeams == null) return 0;
            return ms_Instance.gridTeams.vGrid.Count;
        }
        //------------------------------------------------------
        public static Vector3 GetOffset(int index)
        {
            if (ms_Instance == null || ms_Instance.gridTeams == null || index < 0 || index >= ms_Instance.gridTeams.vGrid.Count) return Vector3.zero;
            return ms_Instance.gridTeams.vGrid[index].Offset + ms_Instance.gridTeams.Offset;
        }
        //------------------------------------------------------
        public static float GetScale(int index)
        {
            if (ms_Instance == null || ms_Instance.gridTeams == null || index < 0 || index >= ms_Instance.gridTeams.vGrid.Count) return 1;
            return ms_Instance.gridTeams.vGrid[index].Scale;
        }
        //------------------------------------------------------
        public static short GetRow(int index)
        {
            if (ms_Instance == null || ms_Instance.gridTeams == null || index < 0 || index >= ms_Instance.gridTeams.vGrid.Count) return 0;
            return ms_Instance.gridTeams.vGrid[index].nRow;
        }
        //------------------------------------------------------
        public static Vector3 GetProjectileSpeedApends(int index)
        {
            if (ms_Instance == null || ms_Instance.StanceProjectileSpeedApends == null || index < 0 || index >= ms_Instance.StanceProjectileSpeedApends.Length) return Vector3.zero;
            return ms_Instance.StanceProjectileSpeedApends[index];
        }
        //------------------------------------------------------
        public static int EvaluateLeave(float fTime, ref Vector3 outPos)
        {
            if (ms_Instance == null) return 0;
            return ms_Instance.Evaluate(ms_Instance.TurnLeaveSplines, fTime, ref outPos);
        }
        //------------------------------------------------------
        public static int EvaluateEnter(float fTime, ref Vector3 outPos)
        {
            if (ms_Instance == null) return 0;
            return ms_Instance.Evaluate(ms_Instance.TurnEnterSplines, fTime, ref outPos);
        }
        //------------------------------------------------------
        public int Evaluate(List<SplineFrame> vFrames, float fTime, ref Vector3 outPos)
        {
            if (vFrames ==null || vFrames.Count <= 0) return 0;
            if (fTime <= vFrames[0].time)
            {
                outPos = vFrames[0].position;
                return -1;
            }
            if (fTime >= vFrames[vFrames.Count - 1].time)
            {
                outPos = vFrames[vFrames.Count - 1].position;
                return 1;
            }

            int __len = vFrames.Count;
            int __half;
            int __middle;
            int __first = 0;
            while (__len > 0)
            {
                __half = __len >> 1;
                __middle = __first + __half;

                if (fTime < vFrames[__middle].time)
                    __len = __half;
                else
                {
                    __first = __middle;
                    ++__first;
                    __len = __len - __half - 1;
                }
            }

            int lhs = __first - 1;
            int rhs = Mathf.Min(vFrames.Count - 1, __first);

            if (lhs < 0 || lhs >= vFrames.Count || rhs < 0 || rhs >= vFrames.Count)
                return 0;

            SplineFrame lhsKey = vFrames[lhs];
            SplineFrame rhsKey = vFrames[rhs];

            float dx = rhsKey.time - lhsKey.time;
            Vector3 m1 = Vector3.zero, m2 = Vector3.zero;
            float t;
            if (dx != 0f)
            {
                t = (fTime - lhsKey.time) / dx;
            }
            else
                t = 0;

            m1 = lhsKey.position + lhsKey.outTan;
            m2 = rhsKey.position + rhsKey.inTan;
            outPos = Framework.Core.BaseUtil.Bezier4(t, lhsKey.position, m1, m2, rhsKey.position);
            return 2;
        }
#if UNITY_EDITOR
        //------------------------------------------------------
        float GetMaxTime(List<SplineFrame> vFrames)
        {
            float fTime = 0;
            for (int i = 0; i < vFrames.Count; ++i)
            {
                fTime = Mathf.Max(vFrames[i].time, fTime);
            }
            return fTime;
        }
        //------------------------------------------------------
        public void DrawSpline(List<SplineFrame> vFrames, Color color, int controller = 0, bool bEiditAble = true, float fArrowRadius = 6)
        {
            if (vFrames.Count <= 0) return;
            Vector3 prePos = Vector3.zero;
            float maxTime = GetMaxTime(vFrames);
            Color bak = UnityEditor.Handles.color;
            prePos = vFrames[0].position;
            float time = 0f;
            UnityEditor.Handles.color = color;
            while (time < maxTime)
            {
                Vector3 pos = Vector3.zero;
                Evaluate(vFrames, time, ref pos);

                UnityEditor.Handles.DrawLine(prePos, pos);

                prePos = pos;
                time += 0.01f;
            }
            UnityEditor.Handles.color = bak;
        }
#endif
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(TeamFormationData), true)]
    [CanEditMultipleObjects]
    public class TeamFormationDataEditor : Editor
    {
        float m_fDurationTime;
        float m_fTestTime;
        bool m_bExpandEnterSpline = false;
        bool m_bExpandLeaveSpline = false;
        bool m_bStanceProjectileSpeedApends = false;
        bool m_bExpandGrid = false;
        public void OnEnable()
        {
            SceneView.duringSceneGui += this.OnSceneGUI;
        }
        private void OnDisable()
        {
            SceneView.duringSceneGui -= this.OnSceneGUI;
        }
        GameObject m_pTset = null;

        bool m_bCircleExpand = false;
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            TeamFormationData assets = target as TeamFormationData;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("SwapPoint"), new GUIContent("排与排之间点交换模式"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("SwapSpeed"), new GUIContent("交换速度"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("SwapForwad"), new GUIContent("与前排交换曲线"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("SwapBack"), new GUIContent("与后排交换曲线"));

            m_fDurationTime = EditorGUILayout.Slider("曲线K帧时间", m_fDurationTime, 0, 10);
            GUILayout.BeginHorizontal();
            m_bExpandEnterSpline = EditorGUILayout.Foldout(m_bExpandEnterSpline, "进场曲线");
            if(GUILayout.Button("新建"))
            {
                if (assets.TurnEnterSplines == null) assets.TurnEnterSplines = new List<TeamFormationData.SplineFrame>();
                for(int i = 0; i < assets.TurnEnterSplines.Count; ++i)
                {
                    TeamFormationData.SplineFrame frame = assets.TurnEnterSplines[i];
                    if ( Mathf.Abs(frame.time-m_fDurationTime) <= 0.01f )
                    {
                        return;
                    }
                }
                TeamFormationData.SplineFrame newframe = new TeamFormationData.SplineFrame();
                newframe.time = m_fDurationTime;
                assets.TurnEnterSplines.Add(newframe);
                Framework.Plugin.SortUtility.QuickSortUp<TeamFormationData.SplineFrame>(ref assets.TurnEnterSplines);
            }
            GUILayout.EndHorizontal();
            if(m_bExpandEnterSpline && assets.TurnEnterSplines != null)
            {
                for(int i = 0; i < assets.TurnEnterSplines.Count; ++i)
                {
                    TeamFormationData.SplineFrame frame = assets.TurnEnterSplines[i];
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("[" + i + "]");

                    EditorGUI.BeginChangeCheck();
                    frame.time = EditorGUILayout.FloatField(frame.time);
                    if(EditorGUI.EndChangeCheck())
                    {
                        Framework.Plugin.SortUtility.QuickSortUp<TeamFormationData.SplineFrame>(ref assets.TurnEnterSplines);
                    }
                    GUILayout.EndHorizontal();
                    frame.position = EditorGUILayout.Vector3Field("pos",frame.position);

                    assets.TurnEnterSplines[i] = frame;
                }
            }

            GUILayout.BeginHorizontal();
            m_bExpandLeaveSpline = EditorGUILayout.Foldout(m_bExpandLeaveSpline, "离场曲线");
            if (GUILayout.Button("新建"))
            {
                if (assets.TurnLeaveSplines == null) assets.TurnLeaveSplines = new List<TeamFormationData.SplineFrame>();
                for (int i = 0; i < assets.TurnLeaveSplines.Count; ++i)
                {
                    TeamFormationData.SplineFrame frame = assets.TurnLeaveSplines[i];
                    if (Mathf.Abs(frame.time - m_fDurationTime) <= 0.01f)
                    {
                        return;
                    }
                }
                TeamFormationData.SplineFrame newframe = new TeamFormationData.SplineFrame();
                newframe.time = m_fDurationTime;
                assets.TurnLeaveSplines.Add(newframe);
                Framework.Plugin.SortUtility.QuickSortUp<TeamFormationData.SplineFrame>(ref assets.TurnLeaveSplines);
            }
            GUILayout.EndHorizontal();
            if (m_bExpandLeaveSpline && assets.TurnLeaveSplines!=null)
            {
                for (int i = 0; i < assets.TurnLeaveSplines.Count; ++i)
                {
                    TeamFormationData.SplineFrame frame = assets.TurnLeaveSplines[i];
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("[" + i + "]");

                    EditorGUI.BeginChangeCheck();
                    frame.time = EditorGUILayout.FloatField(frame.time);
                    if (EditorGUI.EndChangeCheck())
                    {
                        Framework.Plugin.SortUtility.QuickSortUp<TeamFormationData.SplineFrame>(ref assets.TurnLeaveSplines);
                    }
                    GUILayout.EndHorizontal();
                    frame.position = EditorGUILayout.Vector3Field("pos", frame.position);

                    assets.TurnLeaveSplines[i] = frame;
                }
            }

            m_bExpandGrid = EditorGUILayout.Foldout(m_bExpandGrid, "格子站位");
            if(m_bExpandGrid)
            {
                EditorGUI.indentLevel++;
                assets.gridTeams.Offset = EditorGUILayout.Vector3Field("起始偏移", assets.gridTeams.Offset);

                for (int i = 0; i < assets.gridTeams.vGrid.Count; ++i)
                {
                    TeamFormationData.Grid foramt = assets.gridTeams.vGrid[i];
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.Foldout(true, "第" + (i + 1).ToString());
                    if (GUILayout.Button("删除"))
                    {
                        if (EditorUtility.DisplayDialog("提示", "是否确认删除", "删除", "取消"))
                        {
                            assets.gridTeams.vGrid.RemoveAt(i);
                            break;
                        }
                    }
                    GUILayout.EndHorizontal();
                    EditorGUI.indentLevel++;
                    EditorGUI.BeginChangeCheck();
                    foramt.Offset = EditorGUILayout.Vector3Field("偏移", foramt.Offset);
                    foramt.nRow = (short)EditorGUILayout.IntField("排", foramt.nRow);
                    foramt.Scale = EditorGUILayout.FloatField("缩放", foramt.Scale);

                    if (EditorGUI.EndChangeCheck())
                    {
                        if (SceneView.lastActiveSceneView != null) SceneView.lastActiveSceneView.Repaint();
                    }
                    EditorGUI.indentLevel--;
                    assets.gridTeams.vGrid[i] = foramt;
                }
                if (GUILayout.Button("新建"))
                {
                    assets.gridTeams.vGrid.Add(new TeamFormationData.Grid() { Offset = Vector3.zero, Scale =1 });
                }
                EditorGUI.indentLevel--;
            }


            m_bCircleExpand = EditorGUILayout.Foldout(m_bCircleExpand, "圆形轮盘站位");
            if(m_bCircleExpand)
            {
                for(int i = 0; i < assets.circleTeams.Length; ++i)
                {
                    DrawCircleTeam(i.ToString(), ref assets.circleTeams[i]);
                }
            }

            GUILayout.BeginHorizontal();
            m_bStanceProjectileSpeedApends = EditorGUILayout.Foldout(m_bStanceProjectileSpeedApends, "普攻站位速度加成");
            if (GUILayout.Button("新建"))
            {
                List<Vector3> vSpeeds = assets.StanceProjectileSpeedApends!=null? new List<Vector3>(assets.StanceProjectileSpeedApends):new List<Vector3>();
                vSpeeds.Add(Vector3.zero);
                assets.StanceProjectileSpeedApends = vSpeeds.ToArray();
            }
            GUILayout.EndHorizontal();
            if (m_bStanceProjectileSpeedApends)
            {
                if (assets.StanceProjectileSpeedApends != null)
                {
                    for(int i = 0; i < assets.StanceProjectileSpeedApends.Length; ++i)
                    {
                        GUILayout.BeginHorizontal();
                        assets.StanceProjectileSpeedApends[i] = EditorGUILayout.Vector3Field("第" + (i + 1).ToString() + "号位速度加成", assets.StanceProjectileSpeedApends[i]);
                        if(GUILayout.Button("删除"))
                        {
                            if(!EditorUtility.DisplayDialog("提示", "是否确认删除，删除之后后面的位置都自动往前挪一位！！！", "取消", "删除"))
                            {
                                List<Vector3> vSpeeds = assets.StanceProjectileSpeedApends != null ? new List<Vector3>(assets.StanceProjectileSpeedApends) : new List<Vector3>();
                                vSpeeds.RemoveAt(i);
                                assets.StanceProjectileSpeedApends = vSpeeds.ToArray();
                                break;
                            }
                        }
                        GUILayout.EndHorizontal();
                    }
                }
            }

            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("刷新保存"))
            {
                EditorUtility.SetDirty(target);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            }

        }
        //------------------------------------------------------
        void DrawCircleTeam(string label, ref TeamFormationData.CircleConfig circleConfig)
        {
            circleConfig.bExpand = EditorGUILayout.Foldout(circleConfig.bExpand, label);
            if (!circleConfig.bExpand) return;
            EditorGUI.indentLevel++;
            circleConfig.leaderScale = EditorGUILayout.FloatField("领导位缩放", circleConfig.leaderScale);
            circleConfig.memberScale = EditorGUILayout.FloatField("成员位缩放", circleConfig.memberScale);
            circleConfig.radius = EditorGUILayout.FloatField("圆形大小", circleConfig.radius);
            circleConfig.turnSpeed = EditorGUILayout.FloatField("轮转速度", circleConfig.turnSpeed);
            circleConfig.turnHeadUISpeed = EditorGUILayout.FloatField("UI头像轮转速度", circleConfig.turnHeadUISpeed);
            circleConfig.offset = EditorGUILayout.Vector3Field("整体偏移", circleConfig.offset);
            circleConfig.showLeaderHalo = EditorGUILayout.Toggle("显示脚底光环", circleConfig.showLeaderHalo);
            circleConfig.onlyShowLeader = EditorGUILayout.Toggle("只显示首领位", circleConfig.onlyShowLeader);
            circleConfig.onlyLeaderDoSkill = EditorGUILayout.Toggle("只有首领位攻击", circleConfig.onlyLeaderDoSkill);

            circleConfig.partitionCnt = EditorGUILayout.IntField("几等分", circleConfig.partitionCnt);
            if (circleConfig.partitionOffsets == null || circleConfig.partitionOffsets.Length != circleConfig.partitionCnt)
            {
                List<Vector4> vOffset = new List<Vector4>();
                if (circleConfig.partitionOffsets != null)
                {
                    for (int i = vOffset.Count; i < circleConfig.partitionOffsets.Length && i < circleConfig.partitionCnt; ++i)
                    {
                        vOffset.Add(circleConfig.partitionOffsets[i]);
                    }
                }
                for (int i = vOffset.Count; i < circleConfig.partitionCnt; ++i)
                {
                    vOffset.Add(Vector4.zero);
                }
                circleConfig.partitionOffsets = vOffset.ToArray();
            }
            for (int i = 0; i < circleConfig.partitionOffsets.Length; ++i)
            {
                circleConfig.partitionOffsets[i] = EditorGUILayout.Vector4Field((i + 1) + "等分位偏移(w:范围随机)", circleConfig.partitionOffsets[i]);
            }
            EditorGUI.indentLevel--;
        }
        //------------------------------------------------------
        private void OnSceneGUI(SceneView view)
        {
            TeamFormationData assets = target as TeamFormationData;

            if(m_bExpandEnterSpline)
                OnDrawSplineGUI(assets, assets.TurnEnterSplines, Color.blue);
            if(m_bExpandLeaveSpline)
                OnDrawSplineGUI(assets, assets.TurnLeaveSplines, Color.red);

            if (assets.gridTeams == null) return;

            Vector3 offset = Vector3.zero;
            for (int i = 0; i < assets.gridTeams.vGrid.Count; ++i)
            {
                TeamFormationData.Grid foramt = assets.gridTeams.vGrid[i];
                offset = assets.gridTeams.Offset + foramt.Offset;
                Handles.CubeHandleCap(0, offset, Quaternion.identity, 0.1f, EventType.Repaint);

                Handles.Label(offset, "第" + (i+1).ToString());
            }
        }
        //-----------------------------------------------------
        private void OnDrawSplineGUI(TeamFormationData controller, List<TeamFormationData.SplineFrame> vFrames, Color color)
        {
            if (vFrames == null) return;
            controller.DrawSpline(vFrames, color);
            UnityEngine.Event currentEvt = UnityEngine.Event.current;
            for (int i = 0; i < vFrames.Count; ++i)
            {
                TeamFormationData.SplineFrame frame = vFrames[i];

                if (currentEvt != null && currentEvt.shift)
                {
                    //  HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

                    frame.inTan = Handles.DoPositionHandle(frame.position + frame.inTan, Quaternion.identity) - frame.position;
                    frame.outTan = Handles.DoPositionHandle(frame.position + frame.outTan, Quaternion.identity) - frame.position;

                    Handles.color = Color.red;
                    Handles.SphereHandleCap(0, frame.position + frame.inTan, Quaternion.identity, 0.1f, EventType.Repaint);
                    Handles.SphereHandleCap(0, frame.position + frame.outTan, Quaternion.identity, 0.1f, EventType.Repaint);
                    Handles.color = color;

                    Handles.SphereHandleCap(0, frame.position, Quaternion.identity, 0.2f, EventType.Repaint);
                }
                else
                {
                    frame.position = Handles.DoPositionHandle(frame.position, Quaternion.identity);
                }


                Handles.Label(frame.position, i.ToString() + "[" + frame.time + "]");
                vFrames[i] = frame;
            }


            for (int i = 0; i < vFrames.Count; ++i)
            {
                TeamFormationData.SplineFrame frame = vFrames[i];
                Vector3 prevPos = frame.position;
                Vector3 nextPos = frame.position;
                Vector3 retPos = Vector3.zero;
                if (i - 1 >= 0 && controller.Evaluate(vFrames, (frame.time + vFrames[i - 1].time) / 2, ref retPos) != 0)
                {
                    prevPos = retPos;
                }
                if (i + 1 < vFrames.Count && controller.Evaluate(vFrames,(frame.time + vFrames[i + 1].time) / 2, ref retPos)!=0)
                {
                    nextPos = retPos;
                }
                if (currentEvt != null && currentEvt.control)
                {
                    Vector2 position2 = HandleUtility.WorldToGUIPoint(frame.position);
                    GUILayout.BeginArea(new Rect(position2.x, position2.y, 40, 25));
                    if (GUILayout.Button("删除"))
                    {
                        if(EditorUtility.DisplayDialog("提示i", "确定删除?", "删除", "取消"))
                        {
                            vFrames.RemoveAt(i);
                            break;
                        }
                    }
                    GUILayout.EndArea();
                }
                if (i - 1 >= 0)
                {
                    Vector2 position2 = HandleUtility.WorldToGUIPoint(prevPos);
                    GUILayout.BeginArea(new Rect(position2.x, position2.y, 40, 25));
                    if (GUILayout.Button("插入"))
                    {
                        float insert = (frame.time + vFrames[i - 1].time) / 2;
                        if (controller.Evaluate(vFrames, insert, ref retPos) != 0)
                        {
                            TeamFormationData.SplineFrame newframe = new TeamFormationData.SplineFrame();
                            newframe.time = insert;
                            newframe.position = retPos;
                            vFrames.Insert(i, newframe);

                            m_fDurationTime = insert;
                            m_fTestTime = insert;
                            break;
                        }
                    }
                    GUILayout.EndArea();
                }
                if (i + 1 < vFrames.Count)
                {
                    Vector2 position2 = HandleUtility.WorldToGUIPoint(nextPos);
                    GUILayout.BeginArea(new Rect(position2.x, position2.y, 40, 25));
                    if (GUILayout.Button("插入"))
                    {
                        float insert = (frame.time + vFrames[i + 1].time) / 2;
                        if (controller.Evaluate(vFrames, insert, ref retPos) != 0)
                        {
                            TeamFormationData.SplineFrame newframe = new TeamFormationData.SplineFrame();
                            newframe.time = insert;
                            newframe.position = retPos;
                            vFrames.Insert(i + 1, newframe);

                            m_fDurationTime = insert;
                            m_fTestTime = insert;
                            break;
                        }
                    }
                    GUILayout.EndArea();
                }
            }
        }
        //------------------------------------------------------
        void DrawLine(Vector3 start, Vector3 end, AnimationCurve curve, Color color , float fGap = 0.01f)
        {
            float fMaxTime = Framework.Core.BaseUtil.GetCurveMaxTime(curve);
            if (fMaxTime <= 0 ||fGap <=0) return;
            float distance = Vector3.Distance(start, end);
            if (distance <= 0) return;

            Color bak = Handles.color;
            Handles.color = color;
            float delta = 0;
            Vector3 pos = start;
            Vector3 dir = end - start;
            while(delta <= distance)
            {
                float fTime = Mathf.Clamp01(delta / distance);
                Vector3 lerp = Vector3.Lerp(start, end, fTime);
                fTime *= fMaxTime;
                Vector3 cur = new Vector3(lerp.x, lerp.y+ curve.Evaluate(fTime), lerp.z);
                Handles.DrawLine(pos, cur);
                if((cur - pos).normalized.sqrMagnitude>0.001f)
                    dir = cur - pos;
                pos = cur;
                delta += fGap;
            }
            Handles.DrawLine(pos, end);
            if ((end - pos).normalized.sqrMagnitude > 0.001f)
                dir = end - pos;
            Handles.ArrowHandleCap(0, end, Quaternion.LookRotation(dir.normalized), 0.1f, EventType.Repaint);

            Handles.color = bak;
        }
    }
#endif
}
