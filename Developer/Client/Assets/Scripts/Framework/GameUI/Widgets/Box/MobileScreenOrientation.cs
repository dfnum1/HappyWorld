/********************************************************************
生成日期:	11:11:2020 10:06
类    名: 	MobileScreenOrientation
作    者:	JaydenHe
描    述:	陀螺仪
*********************************************************************/
using System;
using UnityEngine;
namespace TopGame.UI
{
    [Serializable]
    public class SetUp
    {
        [Tooltip("敏感度")]
        public float sensitivity = 15f;   //敏感度

        [Tooltip("最大水平移动速度")]
        public float maxturnSpeed = 35f;    // 最大移动速度

        [Tooltip("最大垂直傾斜角移动速度")]
        public float maxTilt = 35f;    // 最大倾斜角

        [Tooltip("位移加成速率")]
        public float posRate = 1.5f;
        [Tooltip("固定轴X值")]
        public float CertainXValue = -1;
        [Tooltip("固定轴Y值")]
        public float CertainYValue = -1;
        [Tooltip("固定轴Z值")]
        public float CertainZValue = -1;
    }

    public class MobileScreenOrientation : MonoBehaviour
    {
        public enum MotionAxial
        {
            All = 1,  //全部轴
            None = 2,
            x = 3,
            y = 4,
            z = 5
        }

        public enum MotionMode
        {
            Postion = 1,   //只是位置辩护
            Rotation = 2,
            All = 3    //全部变化
        }

        public MotionAxial MotionAxial1 = MotionAxial.y;
        public MotionAxial MotionAxial2 = MotionAxial.None;

        public MotionMode Mode = MotionMode.Rotation;   //运动模式

        public SetUp SettingProperty;

        public GameObject Target;     //被移动的对象

        Vector3 m_MobileOrientation;   //手机陀螺仪变化的值

        Vector3 m_tagerTransform;
        Vector3 m_tagerPos;
        public Vector3 ReversePosition = Vector3.one; //基于陀螺仪方向的取反

        private bool m_IsEnabled = true;
        private Vector3 m_DefaultPos;
        private Transform m_Trans;

        public Vector3 m_DefaultRotation;
        private Vector3 m_EnableRotation;
        
        void Awake()
        {
            if (!Target)
            {
                //Debug.LogError("陀螺仪未设置目标");
                return;
            }

            m_Trans = Target.transform;
            m_DefaultPos = m_Trans.localPosition;
            //m_DefaultRotation = m_Trans.eulerAngles;
            m_tagerTransform = Vector3.zero;
            m_tagerPos = Vector3.zero;
        }

        public void DisableGyro()
        {
            m_IsEnabled = false;
        }

        public void Reset()
        {
            return;
            if (m_Trans)
            {
                m_Trans.localEulerAngles = m_DefaultRotation;
                m_Trans.localPosition = m_DefaultPos;
            }

        }


        public void EnableGyro()
        {
#if !UNITY_EDITOR
            m_EnableRotation = m_DefaultRotation;
            Debug.LogError("m_EnableRotation:"+m_EnableRotation.ToString("f3"));
            m_IsEnabled = true;
#endif
        }


        void LateUpdate()
        {
            return;
            if (Target == null || !m_IsEnabled)
                return;

            m_MobileOrientation = Input.acceleration;

            if (MotionAxial1 == MotionAxial.None && MotionAxial2 == MotionAxial.None)   //不操作任何轴
                return;
            else if (MotionAxial1 == MotionAxial.x && MotionAxial2 == MotionAxial.None)   // X轴
            {
                float finalX = m_EnableRotation.x - m_MobileOrientation.x * SettingProperty.maxTilt * ReversePosition.x; ;
                float preX = m_tagerTransform.x;
                m_tagerTransform.x = Mathf.Lerp(finalX, preX, 0.5f);
                m_tagerTransform.y = SettingProperty.CertainYValue != -1 ? SettingProperty.CertainYValue : m_EnableRotation.y;
                m_tagerTransform.z = SettingProperty.CertainZValue != -1 ? SettingProperty.CertainZValue : m_EnableRotation.z;
            }
            else if (MotionAxial1 == MotionAxial.y && MotionAxial2 == MotionAxial.None)   //Y 轴
            {
                float finalY = m_EnableRotation.y - m_MobileOrientation.y * SettingProperty.maxturnSpeed * ReversePosition.y;
                float preY = m_tagerTransform.y;
                m_tagerTransform.y = Mathf.Lerp(finalY, preY, 0.5f);
                m_tagerTransform.x = SettingProperty.CertainXValue != -1 ? SettingProperty.CertainXValue : m_EnableRotation.x ;
                m_tagerTransform.z = SettingProperty.CertainZValue != -1 ? SettingProperty.CertainZValue : m_EnableRotation.z;
            }
            else if (MotionAxial1 == MotionAxial.z && MotionAxial2 == MotionAxial.None)   // z轴
            {
                float finalZ = m_EnableRotation.z - m_MobileOrientation.z * SettingProperty.maxTilt * ReversePosition.z;
                float preZ = m_tagerTransform.z;
                m_tagerTransform.z = Mathf.Lerp(finalZ, preZ, 0.5f);
                m_tagerTransform.x = SettingProperty.CertainXValue != -1 ? SettingProperty.CertainXValue : m_EnableRotation.x;
                m_tagerTransform.y = SettingProperty.CertainYValue != -1 ? SettingProperty.CertainYValue : m_EnableRotation.y;
            }
            else if (MotionAxial1 == MotionAxial.x && MotionAxial2 == MotionAxial.y)   // X和Y轴
            {
                m_tagerTransform.y = m_EnableRotation.y -m_MobileOrientation.x * SettingProperty.maxturnSpeed * ReversePosition.y;
                m_tagerTransform.x = m_EnableRotation.x +  m_MobileOrientation.y * SettingProperty.maxTilt * ReversePosition.x;
                m_tagerTransform.z = SettingProperty.CertainZValue != -1 ? SettingProperty.CertainZValue : m_EnableRotation.z;
            }
            else if (MotionAxial1 == MotionAxial.y && MotionAxial2 == MotionAxial.x) // Y和X轴
            {
                m_tagerTransform.y = m_EnableRotation.y  -m_MobileOrientation.x * SettingProperty.maxturnSpeed * ReversePosition.y;
                m_tagerTransform.x = m_EnableRotation.x +  m_MobileOrientation.y * SettingProperty.maxTilt * ReversePosition.x;
                m_tagerTransform.z = SettingProperty.CertainZValue != -1 ? SettingProperty.CertainZValue : m_EnableRotation.z;
            }
            else if (MotionAxial1 == MotionAxial.x && MotionAxial2 == MotionAxial.z)  // x 和 Z 轴
            {
                m_tagerTransform.x = m_EnableRotation.x + m_MobileOrientation.y * SettingProperty.maxTilt * ReversePosition.x;
                m_tagerTransform.z = m_EnableRotation.z -m_MobileOrientation.z * SettingProperty.maxTilt * ReversePosition.z;
                m_tagerTransform.y = SettingProperty.CertainYValue != -1 ? SettingProperty.CertainYValue : m_EnableRotation.y;
            }
            else if (MotionAxial1 == MotionAxial.z && MotionAxial2 == MotionAxial.x)  // Z 和 X 轴
            {
                m_tagerTransform.x = m_EnableRotation.x +  m_MobileOrientation.y * SettingProperty.maxTilt * ReversePosition.x;
                m_tagerTransform.z = m_EnableRotation.z -m_MobileOrientation.z * SettingProperty.maxTilt * ReversePosition.z;
                m_tagerTransform.y = SettingProperty.CertainYValue != -1 ? SettingProperty.CertainYValue : m_EnableRotation.y;
            }
            else if (MotionAxial1 == MotionAxial.y && MotionAxial2 == MotionAxial.z)   // Y和Z 轴
            {
                m_tagerTransform.y = m_EnableRotation.y -m_MobileOrientation.x * SettingProperty.maxturnSpeed * ReversePosition.y;
                m_tagerTransform.z = m_EnableRotation.z  -m_MobileOrientation.z * SettingProperty.maxTilt * ReversePosition.z;
                m_tagerTransform.x = SettingProperty.CertainXValue != -1 ? SettingProperty.CertainXValue : m_EnableRotation.x;
            }
            else if (MotionAxial1 == MotionAxial.z && MotionAxial2 == MotionAxial.y)   // Z和 Y轴
            {
                m_tagerTransform.y = m_EnableRotation.y  -m_MobileOrientation.x * SettingProperty.maxturnSpeed * ReversePosition.y;
                m_tagerTransform.z = m_EnableRotation.z -m_MobileOrientation.z * SettingProperty.maxTilt * ReversePosition.z;
                m_tagerTransform.x = SettingProperty.CertainXValue != -1 ? SettingProperty.CertainXValue : m_EnableRotation.x;
            }
            else if (MotionAxial1 == MotionAxial.All && MotionAxial2 == MotionAxial.All)   // 所有轴向都运动
            {
                m_tagerTransform.y = m_EnableRotation.y -m_MobileOrientation.x * SettingProperty.maxturnSpeed * ReversePosition.y;
                m_tagerTransform.x = m_EnableRotation.x + m_MobileOrientation.y * SettingProperty.maxTilt * ReversePosition.x;
                m_tagerTransform.z = m_EnableRotation.z + m_MobileOrientation.z * SettingProperty.maxTilt * ReversePosition.z;
            }

            m_tagerPos.x = m_tagerTransform.y;
            m_tagerPos.y = -m_tagerTransform.x;
            m_tagerPos.z = m_tagerTransform.z;

            if (Mode == MotionMode.Postion)
            {
                Target.transform.localPosition = Vector3.Lerp(Target.transform.localPosition, m_tagerPos * SettingProperty.posRate, Time.deltaTime * SettingProperty.sensitivity);
            }
            else if (Mode == MotionMode.Rotation)
            {
                Target.transform.localRotation = Quaternion.Lerp(Target.transform.localRotation, Quaternion.Euler(m_tagerTransform), Time.deltaTime * SettingProperty.sensitivity);
            }
            else
            {
                Target.transform.localPosition = Vector3.Lerp(Target.transform.localPosition, m_tagerPos * SettingProperty.posRate, Time.deltaTime * SettingProperty.sensitivity);
                Target.transform.localRotation = Quaternion.Lerp(Target.transform.localRotation, Quaternion.Euler(m_tagerTransform), Time.deltaTime * SettingProperty.sensitivity);
            }
        }

    }
}