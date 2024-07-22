//auto generator
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using TopGame.Data;
using TopGame.Core;
using Framework.Data;
using Framework.ED;
using Framework.Core;

namespace TopGame.ED
{
	[Framework.Plugin.PluginInspector(typeof(MoveTypeEventParameter))]
	public class MoveTypeEventInspector
    {
		public static void DrawInspector(MoveTypeEventParameter param)
        {
            param.moveType = (EMoveType)HandleUtilityWrapper.PopEnum("移动类型", param.moveType);
            param.fSpeedScale = EditorGUILayout.Slider("移动速度", param.fSpeedScale, 0, 10);

            bool bCanRadomRoade = true;
            if (param.moveType == EMoveType.Standoff)
            {
                param.userData.x = EditorGUILayout.FloatField("对峙距离", param.userData.x);
                param.userData.y = EditorGUILayout.Toggle("高度同步", param.userData.y>0)?1:0;
                param.userData.z = EditorGUILayout.FloatField("时长(<=0一直对峙)", param.userData.z);
                param.targetOffestPos = EditorGUILayout.Vector3Field("对峙偏移", param.targetOffestPos);
                if (param.fSpeedScale > 0)
                {
                    bCanRadomRoade = false;
                }
            }
            else if (param.moveType == EMoveType.Wander)
            {
                param.userData.x = EditorGUILayout.FloatField("出生点随机半径", param.userData.x);
                param.userData.y = EditorGUILayout.FloatField("目标点随机半径", param.userData.y);
                param.targetOffestPos = EditorGUILayout.Vector3Field("目标位置", param.targetOffestPos);
                float minD = EditorGUILayout.FloatField("呆顿-Min", param.userData.z);
                float maxD = EditorGUILayout.FloatField("呆顿-Max", param.userData.w);
                param.userData.z = Mathf.Min(minD, maxD);
                param.userData.w = Mathf.Max(minD, maxD);
            }
            else if (param.moveType == EMoveType.Rush)
            {

            }

            if (bCanRadomRoade)
            {
                int roadeBit = param.rodeRandom.x;
                EditorGUILayout.LabelField("随机赛道");
                EditorGUI.indentLevel++;
                bool bLeft = EditorGUILayout.Toggle("左道", (roadeBit & (1 << 0)) != 0);
                bool bMidle = EditorGUILayout.Toggle("中道", (roadeBit & (1 << 1)) != 0);
                bool bRight = EditorGUILayout.Toggle("右道", (roadeBit & (1 << 2)) != 0);
                if (bLeft) roadeBit |= 1 << 0;
                else roadeBit &= ~(1 << 0);
                if (bMidle) roadeBit |= 1 << 1;
                else roadeBit &= ~(1 << 1);
                if (bRight) roadeBit |= 1 << 2;
                else roadeBit &= ~(1 << 2);
                EditorGUI.indentLevel--;
                param.rodeRandom.x = roadeBit;
                param.rodeRandom.y = EditorGUILayout.IntField("赛道移动速度(百分位)", param.rodeRandom.y);
                param.rodeRandom.z = EditorGUILayout.IntField("赛道停顿时长(毫秒)", param.rodeRandom.z);
            }
        }
	}
}
