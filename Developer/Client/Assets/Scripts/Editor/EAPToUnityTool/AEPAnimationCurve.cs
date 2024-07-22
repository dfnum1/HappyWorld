#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace TopGame.AEPToUnity
{
    public class AEPAnimationCurve
    {
        public EditorCurveBinding binding;

        public AnimationCurve curve;

        public AEPAnimationCurve(EditorCurveBinding _binding, AnimationCurve _curve)
        {
            this.binding = _binding;
            this.curve = _curve;
        }
    }
}
#endif