#if UNITY_EDITOR
/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	ProjectileDebug
作    者:	HappLI
描    述:	调试数据
*********************************************************************/
using UnityEngine;
using UnityEditor;
using TopGame.Core;
using Framework.Core;

namespace TopGame.Logic
{
    public class ProjectileDebugEditor : IWorldNodeDebugerEditor
    {
        public void OnEnable(AWorldNode pNode) { }
        public void OnDisable(AWorldNode pNode) { }
        public void OnInspectorGUI(AWorldNode pNode)
        {
            Projectile proj = pNode as Projectile;
            EditorGUILayout.LabelField("launch_id:" + proj.GetInstanceID());
            EditorGUILayout.LabelField("config id:" + proj.GetConfigID());
            if(proj.owner_actor!=null) EditorGUILayout.LabelField("owner id:" + proj.owner_actor.GetInstanceID());
            EditorGUILayout.LabelField("延时:"+proj.delta);
            EditorGUILayout.LabelField("targetPosition:" + proj.targetPosition);
            EditorGUILayout.LabelField("extern_speed:" + proj.extern_speed);
            EditorGUILayout.LabelField("acceleration:" + proj.acceleration);
            EditorGUILayout.LabelField("speed:" + proj.speed);
            EditorGUILayout.LabelField("remain_life_time:" + proj.remain_life_time);
            EditorGUILayout.LabelField("remain_hit_count:" + proj.remain_hit_count);
            EditorGUILayout.LabelField("max_oneframe_hit:" + proj.max_oneframe_hit);
            EditorGUILayout.LabelField("hit_step_delta:" + proj.hit_step_delta);
            EditorGUILayout.LabelField("damage_id:" + proj.damage_id);
            EditorGUILayout.LabelField("damage_power:" + proj.damage_power);
            EditorGUILayout.LabelField("remain_bound_count:" + proj.remain_bound_count);
            EditorGUILayout.LabelField("eFollowType:" + proj.eFollowType);
            EditorGUILayout.LabelField("track_end:" + proj.track_end);
            EditorGUILayout.LabelField("FollowOffset:" + proj.FollowOffset);
            EditorGUILayout.LabelField("FollowType" + proj.eFollowType);
            if (proj.trackTransform != null) EditorGUILayout.LabelField("trackTransform:" + proj.trackTransform.ToString());
            if (proj.pTargetNode!=null) EditorGUILayout.LabelField("pTargetNode:" + proj.pTargetNode.ToString());
        }
        //------------------------------------------------------
        public void OnSceneGUI(AWorldNode proj)
        {
            Core.Projectile projeNode = proj as Core.Projectile;
            if (projeNode == null || projeNode.projectile == null) return;
            TestDrawLineTrace(proj as Core.Projectile);
        }
        //------------------------------------------------------
        void TestDrawLineTrace(Core.Projectile projectile)
        {
            if (projectile == null) return;
            Color color = Gizmos.color;
            if (projectile.remain_life_time <= 0 )
            {
                Gizmos.color = color;
                return;
            }
            Gizmos.color = Color.yellow;
            Vector3 max_speed = projectile.maxSpeed;
            Vector3 curSpeed = projectile.speed;
            Vector3 curAcceleration = projectile.acceleration;
            float fDuration = 0;
            Vector3 duration_speed = projectile.speed;
            Vector3 curPos = projectile.position;
            Vector3 lastPos = curPos;
            float fDelta = Mathf.Max(Time.deltaTime, projectile.remain_life_time / 20);
            while (fDuration <= projectile.remain_life_time)
            {
                duration_speed.x += projectile.acceleration.x * fDelta;
                duration_speed.y += projectile.acceleration.y * fDelta;
                duration_speed.z += projectile.acceleration.z * fDelta;

                if (max_speed.x > 0) duration_speed.x = Mathf.Min(duration_speed.x, max_speed.x);
                if (max_speed.y > 0) duration_speed.y = Mathf.Min(duration_speed.y, max_speed.y);
                if (max_speed.z > 0) duration_speed.z = Mathf.Min(duration_speed.z, max_speed.z);

                curPos = curPos + duration_speed * fDelta;
                Handles.DrawLine(lastPos, curPos);
                lastPos = curPos;
                if (curPos.y < 0)
                {
                    break;
                }
                fDuration += fDelta;
            }
            Gizmos.color = color;
        }
    }
}
#endif
