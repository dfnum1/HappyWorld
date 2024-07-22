/********************************************************************
生成日期:	25:7:2019   14:35
类    名: 	AnimPathData
作    者:	HappLI
描    述:   路径动画数据
*********************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TopGame.Core;
using Framework.Core;
using Framework.Data;
#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;
#endif
#if USE_SERVER
using ScriptableObject = ExternEngine.ScriptableObject;
#endif
namespace TopGame.Data
{
   // [CreateAssetMenu]
    [System.Serializable]
    [ExternEngine.ConfigPath("sos/DamageCoefficientParams.asset")]
    public class DamageCoefficientParams : ScriptableObject
    {
#if UNITY_EDITOR
        [CoefficientParamUse(3), DisplayNameGUI("暴击矫正")]
#endif
        public DamageCoefficientParam CriticalCoefficient;      //暴伤
#if UNITY_EDITOR
        [CoefficientParamUse(5, "5:格挡倍率"), DisplayNameGUI("格挡矫正")]
#endif
        public DamageCoefficientParam BlockCoefficient;         //格挡
#if UNITY_EDITOR
        [CoefficientParamUse(2), DisplayNameGUI("命中矫正")]
#endif
        public DamageCoefficientParam HitCoefficient;           //命中
#if UNITY_EDITOR
        [CoefficientParamUse(3), DisplayNameGUI("效果抵抗矫正")]
#endif
        public DamageCoefficientParam EffectHitCoefficient;     //效果命中
#if UNITY_EDITOR
        [CoefficientParamUse(5), DisplayNameGUI("免伤矫正")]
#endif
        public DamageCoefficientParam DerateDamageCoefficient;  //免伤
#if UNITY_EDITOR
        [CoefficientParamUse(3), DisplayNameGUI("物理穿透")]
#endif
        public DamageCoefficientParam PhysicThornCoefficient;  //物理穿透

#if UNITY_EDITOR
        [CoefficientParamUse(3), DisplayNameGUI("魔法穿透")]
#endif
        public DamageCoefficientParam MagicThornCoefficient;  //魔法穿透

#if UNITY_EDITOR
        [CoefficientParamUse(4), DisplayNameGUI("技能CD")]
#endif
        public DamageCoefficientParam SkillCDCoefficient;  //技能CD

#if UNITY_EDITOR
        [CoefficientParamUse(2), DisplayNameGUI("攻速/移速")]
#endif
        public DamageCoefficientParam SpeedCoefficient;

#if UNITY_EDITOR
        [CoefficientParamUse(2), DisplayNameGUI("治疗")]
#endif
        public DamageCoefficientParam CureCoefficient;

        public AttrOperationParam AttrOperation;

        private void OnEnable()
        {
            Refresh();
        }
        //------------------------------------------------------
        public void Refresh()
        {
            Framework.Base.ConfigUtil.CriticalCoefficient = CriticalCoefficient;
            Framework.Base.ConfigUtil.DerateDamageCoefficient = DerateDamageCoefficient;
            Framework.Base.ConfigUtil.EffectHitCoefficient = EffectHitCoefficient;
            Framework.Base.ConfigUtil.BlockCoefficient = BlockCoefficient;
            Framework.Base.ConfigUtil.HitCoefficient = HitCoefficient;
            Framework.Base.ConfigUtil.PhysicThornCoefficient = PhysicThornCoefficient;
            Framework.Base.ConfigUtil.MagicThornCoefficient = MagicThornCoefficient;
            Framework.Base.ConfigUtil.SkillCDCoefficient = SkillCDCoefficient;
            Framework.Base.ConfigUtil.SpeedCoefficient = SpeedCoefficient;
            Framework.Base.ConfigUtil.CureCoefficient = CureCoefficient;
            Framework.Base.ConfigUtil.AttrOperation = AttrOperation;
        }
    }

#if UNITY_EDITOR
    [UnityEditor.CanEditMultipleObjects]
    [UnityEditor.CustomEditor(typeof(DamageCoefficientParams), true)]
    public class DamageCoefficientParamsEditor : UnityEditor.Editor
    {
        bool m_bExpandCoefficient = false;
        bool m_bExpandAttrOp = false;
        public static void ExportJson(DamageCoefficientParams controller)
        {
            if (controller == null) return;
            if (!System.IO.Directory.Exists(ED.EditorHelp.ServerBinaryRootPath))
                return;
            string strFile = ED.EditorHelp.ServerBinaryRootPath + "DamageCoefficients.json";
            string strJson = UnityEngine.JsonUtility.ToJson(controller, true);
            System.IO.FileStream fs = new System.IO.FileStream(strFile, System.IO.FileMode.OpenOrCreate);
            System.IO.StreamWriter sw = new System.IO.StreamWriter(fs, System.Text.Encoding.UTF8);
            fs.Position = 0;
            fs.SetLength(0);
            sw.Write(strJson);
            sw.Close();
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DamageCoefficientParams controller = target as DamageCoefficientParams;
            m_bExpandCoefficient = UnityEditor.EditorGUILayout.Foldout(m_bExpandCoefficient, "战斗调整因子");
            if (m_bExpandCoefficient)
            {
                FieldInfo[] fields = controller.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                for (int i = 0; i < fields.Length; ++i)
                {
                    System.Object var = fields[i].GetValue(controller);
                    if (var is DamageCoefficientParam)
                    {
                        DamageCoefficientParam param = (DamageCoefficientParam)var;
                        param = param.Draw(controller, fields[i]);
                        fields[i].SetValue(controller, param);
                    }
                }
            }
            m_bExpandAttrOp = UnityEditor.EditorGUILayout.Foldout(m_bExpandAttrOp, "属性计算");
            if(m_bExpandAttrOp)
            {
                if (controller.AttrOperation.attrConvertFactors == null)
                    controller.AttrOperation.attrConvertFactors = new EAttrOperation[255];
                else if(controller.AttrOperation.attrConvertFactors.Length < (int)EBuffAttrType.Count)
                {
                    List<EAttrOperation> ops = new List<EAttrOperation>(controller.AttrOperation.attrConvertFactors);
                    for(int i =  ops.Count; i < (int)EBuffAttrType.Count; ++i)
                        ops.Add(EAttrOperation.AddMuti);
                }
                System.Type enumType = typeof(EBuffAttrType);
                foreach (System.Enum v in System.Enum.GetValues(enumType))
                {
                    int index = System.Convert.ToInt32(v);
                    string strName = ED.EditorHelp.GetEnumFieldDisplayName(v);
                    controller.AttrOperation.attrConvertFactors[index] = (EAttrOperation)Framework.ED.HandleUtilityWrapper.PopEnum(strName, controller.AttrOperation.attrConvertFactors[index]);
                }
            }

            if (GUILayout.Button("刷新"))
            {
                UnityEditor.EditorUtility.SetDirty(target);
                UnityEditor.AssetDatabase.SaveAssets();
                UnityEditor.AssetDatabase.Refresh(UnityEditor.ImportAssetOptions.ForceSynchronousImport);
                ExportJson(controller);
            }
            if (GUILayout.Button("提交"))
            {
                ExportJson(controller);
                UnitySVN.SVNCommit(new string[]{ ED.EditorHelp.ServerBinaryRootPath , AssetDatabase.GetAssetPath(target) });

            }
            if (serializedObject.ApplyModifiedProperties())
            {
                controller.Refresh();
            }
        }
    }
#endif
}

