using UnityEngine;

namespace TopGame.UI
{
    public class InstitutePanelSerialized : UserInterface
    {
        public int ClassicLineNum = 5;
        public float ClassicMinWidth = 400;
        public float ClassicMaxWidth = 510;
        public float ClassicPadding = 70;
        public Vector3 TreeStartPos = new Vector3(0, -300, 0);
        public Vector2 SmallIconSize = new Vector2(34, 34);
        public Vector2 LargeIconSize = new Vector2(87, 87);
        public Vector3 SmallEffectScale = new Vector3(0.4f, 0.4f, 1f);
        public Vector3 MidEffectScale = new Vector3(1f, 1f, 1f);
        public Vector3 LargeEffectScale = new Vector3(1f, 1f, 1f);

    }
}