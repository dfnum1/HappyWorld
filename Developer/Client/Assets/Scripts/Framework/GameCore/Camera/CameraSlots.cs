/********************************************************************
生成日期:	1:11:2020 10:09
类    名: 	CameraSlots
作    者:	HappLI
描    述:	camera slot
*********************************************************************/
using UnityEngine;
using TopGame.Core;
namespace Framework.Core
{
    public class CameraSlots : ACameraSlots
    {
#if UNITY_EDITOR
        [System.NonSerialized]
       public Camera pPreviewCamera;
#endif
    }
}