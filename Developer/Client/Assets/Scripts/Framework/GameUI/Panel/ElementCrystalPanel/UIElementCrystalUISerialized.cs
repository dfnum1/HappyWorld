using System.Collections;
using System.Collections.Generic;
using TopGame.UI;
using UnityEngine;
using UnityEngine.UI;

public class UIElementCrystalUISerialized : UserInterface
{
    [Header("滚动缩放限制,x最大值,y最小值")]
    public Vector2 ElementCellScaleClamp = new Vector2(1f, 0.9f);

    [Header("元素的宽度")]
    public float ElementCellWidth = 280;
    [Header("元素之间的间隔")]
    public float ElementCellSpaceWidth = 25;
    [Header("屏幕同时显示元素个数")]
    public int ScreenShowElementCount = 2;
    [Header("是否开启无限滚动效果")]
    public bool IsInfiniteScroll = false;
}
