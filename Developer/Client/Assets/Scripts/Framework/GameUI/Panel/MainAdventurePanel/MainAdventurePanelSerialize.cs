using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopGame.UI
{
    public class MainAdventurePanelSerialize : UserInterface
    {
        [Header("进入战斗时的动画时长")]
        public float EnterBattleAniTime = 0.3f;

        [Header("材料关头像向上偏移坐标")]
        public Vector2 Type4HeadOffsetPosition;

        [Header("Boss关头像向上偏移坐标")]
        public Vector2 Type3HeadOffsetPosition;

        [Header("精英关头像向上偏移坐标")]
        public Vector2 Type2HeadOffsetPosition;
    }
}