/********************************************************************
生成日期:	4:12:2021 17:06
类    名: 	Bind3DTo2D
作    者:	JaydenHe
描    述:	3D转2D跟随组件
*********************************************************************/
using System.Collections;
using System.Collections.Generic;
using TopGame;
using UnityEngine;

public class Bind3DTo2D : MonoBehaviour
{
    public Transform MovingObj;
    public RectTransform FollowingObj;
    public Canvas RootCanvas;
    public Vector3 Offset;

    void Update()
    {
        if (!MovingObj || !FollowingObj) return;
        FollowingObj.position = MovingObj.position+Offset;
    }
}
