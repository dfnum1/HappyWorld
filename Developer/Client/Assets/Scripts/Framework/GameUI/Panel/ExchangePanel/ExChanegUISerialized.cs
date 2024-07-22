using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopGame.UI
{
    public class ExChanegUISerialized : UserInterface
    {
        public Vector3 ModelPositionOffset = new Vector3(-0.12f, -1.29f, 3);
        public Vector3 ModelRotationOffset = new Vector3(0, 180, 0);
        public Vector3 RenderSize = new Vector3(512,512,45);

        public List<uint> ShowModelIDs= new List<uint>();
    }
}
