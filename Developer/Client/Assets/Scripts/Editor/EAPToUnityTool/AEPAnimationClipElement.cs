#if UNITY_EDITOR
using TopGame.AEPToUnity;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace TopGame.AEPToUnity
{
    public class AEPAnimationClipElement
    {
        public List<AEPAnimationCurve> listCurve = new List<AEPAnimationCurve>();

        public static GameObject GetRefenreceObject(string path, GameObject aepRoot)
        {
            GameObject result;
            if (path.Length < 1)
            {
                result = aepRoot;
            }
            else
            {
                string[] array = path.Split(new char[]
                {
                '/'
                });
                GameObject gameObject = aepRoot;
                for (int i = 0; i < array.Length; i++)
                {
                    Transform transform = gameObject.transform.Find(array[i]);
                    if (!(transform != null))
                    {
                        result = null;
                        return result;
                    }
                    gameObject = transform.gameObject;
                }
                result = gameObject;
            }
            return result;
        }

        public void AddTranformAnimation(AEPBoneAnimationElement animInfo, string path, GameObject aepRoot, AEPJsonFinal jsonFinal)
        {
            GameObject refenreceObject = AEPAnimationClipElement.GetRefenreceObject(path, aepRoot);
            if (refenreceObject == null)
            {
                Debug.LogError("Not have object reference:" + path);
            }
            else
            {
                if (animInfo.translate != null)
                {
                    Vector3 zero = Vector3.zero;
                    EditorCurveBinding binding = default(EditorCurveBinding);
                    AnimationCurve animationCurve = new AnimationCurve();
                    binding.type = (typeof(Transform));
                    binding.path = path;
                    binding.propertyName = "m_LocalPosition.x";
                    for (int i = 0; i < animInfo.translate.Count; i++)
                    {
                        AEPAnimationTranslate aEPAnimationTranslate = animInfo.translate[i];
                        animationCurve.AddKey(KeyframeUtil.GetNew(aEPAnimationTranslate.time, aEPAnimationTranslate.x + zero.x, KeyframeUtil.GetTangleMode(aEPAnimationTranslate.tangentType)));
                    }
                    AEPAnimationCurve item = new AEPAnimationCurve(binding, animationCurve);
                    this.listCurve.Add(item);
                    EditorCurveBinding binding2 = default(EditorCurveBinding);
                    AnimationCurve animationCurve2 = new AnimationCurve();
                    binding2.type = (typeof(Transform));
                    binding2.path = path;
                    binding2.propertyName = "m_LocalPosition.y";
                    for (int j = 0; j < animInfo.translate.Count; j++)
                    {
                        AEPAnimationTranslate aEPAnimationTranslate2 = animInfo.translate[j];
                        animationCurve2.AddKey(KeyframeUtil.GetNew(aEPAnimationTranslate2.time, aEPAnimationTranslate2.y + zero.y, KeyframeUtil.GetTangleMode(aEPAnimationTranslate2.tangentType)));
                    }
                    AEPAnimationCurve item2 = new AEPAnimationCurve(binding2, animationCurve2);
                    this.listCurve.Add(item2);
                    EditorCurveBinding binding3 = default(EditorCurveBinding);
                    AnimationCurve animationCurve3 = new AnimationCurve();
                    binding3.type = (typeof(Transform));
                    binding3.path = path;
                    binding3.propertyName = "m_LocalPosition.z";
                    for (int k = 0; k < animInfo.translate.Count; k++)
                    {
                        AEPAnimationTranslate aEPAnimationTranslate3 = animInfo.translate[k];
                        animationCurve3.AddKey(KeyframeUtil.GetNew(aEPAnimationTranslate3.time, zero.z, KeyframeUtil.GetTangleMode(aEPAnimationTranslate3.tangentType)));
                    }
                    AEPAnimationCurve item3 = new AEPAnimationCurve(binding3, animationCurve3);
                    this.listCurve.Add(item3);
                }
                if (animInfo.rotate != null)
                {
                    float num = 0f;
                    for (int l = 0; l < animInfo.rotate.Count; l++)
                    {
                        if (l == 0)
                        {
                            num = animInfo.rotate[l].angle;
                            animInfo.rotate[l].angleChange = num;
                        }
                        else
                        {
                            float num2 = animInfo.rotate[l].angle;
                            if (num2 - num > 180f)
                            {
                                num2 -= 360f;
                            }
                            else if (num - num2 > 180f)
                            {
                                num2 += 360f;
                            }
                            float angleChange = num2 - num;
                            num = animInfo.rotate[l].angle;
                            animInfo.rotate[l].angleChange = angleChange;
                        }
                    }
                    BoneElement boneElement = jsonFinal.GetBoneElement(animInfo.name);
                    if (boneElement != null)
                    {
                        float rotation = boneElement.rotation;
                        EditorCurveBinding binding4 = default(EditorCurveBinding);
                        AnimationCurve animationCurve4 = new AnimationCurve();
                        binding4.type = (typeof(Transform));
                        binding4.path = path;
                        binding4.propertyName = "m_LocalRotation.x";
                        for (int m = 0; m < animInfo.rotate.Count; m++)
                        {
                            AEPAnimationRotate aEPAnimationRotate = animInfo.rotate[m];
                            animationCurve4.AddKey(KeyframeUtil.GetNew(aEPAnimationRotate.time, 0f, KeyframeUtil.GetTangleMode(aEPAnimationRotate.tangentType)));
                        }
                        AEPAnimationCurve item4 = new AEPAnimationCurve(binding4, animationCurve4);
                        this.listCurve.Add(item4);
                        EditorCurveBinding binding5 = default(EditorCurveBinding);
                        AnimationCurve animationCurve5 = new AnimationCurve();
                        binding5.type = (typeof(Transform));
                        binding5.path = path;
                        binding5.propertyName = "m_LocalRotation.y";
                        for (int n = 0; n < animInfo.rotate.Count; n++)
                        {
                            AEPAnimationRotate aEPAnimationRotate2 = animInfo.rotate[n];
                            animationCurve5.AddKey(KeyframeUtil.GetNew(aEPAnimationRotate2.time, 0f, KeyframeUtil.GetTangleMode(aEPAnimationRotate2.tangentType)));
                        }
                        AEPAnimationCurve item5 = new AEPAnimationCurve(binding5, animationCurve5);
                        this.listCurve.Add(item5);
                        EditorCurveBinding binding6 = default(EditorCurveBinding);
                        AnimationCurve animationCurve6 = new AnimationCurve();
                        binding6.type = (typeof(Transform));
                        binding6.path = path;
                        binding6.propertyName = "m_LocalRotation.z";
                        float num3 = 0f;
                        for (int num4 = 0; num4 < animInfo.rotate.Count; num4++)
                        {
                            AEPAnimationRotate aEPAnimationRotate3 = animInfo.rotate[num4];
                            float num5 = num3 + aEPAnimationRotate3.angleChange + rotation;
                            num3 += aEPAnimationRotate3.angleChange;
                            animationCurve6.AddKey(KeyframeUtil.GetNew(aEPAnimationRotate3.time, Mathf.Sin((180f + num5 / 2f) * 0.0174532924f), KeyframeUtil.GetTangleMode(aEPAnimationRotate3.tangentType)));
                        }
                        AEPAnimationCurve item6 = new AEPAnimationCurve(binding6, animationCurve6);
                        this.listCurve.Add(item6);
                        EditorCurveBinding binding7 = default(EditorCurveBinding);
                        AnimationCurve animationCurve7 = new AnimationCurve();
                        binding7.type = (typeof(Transform));
                        binding7.path = path;
                        binding7.propertyName = "m_LocalRotation.w";
                        float num6 = 0f;
                        for (int num7 = 0; num7 < animInfo.rotate.Count; num7++)
                        {
                            AEPAnimationRotate aEPAnimationRotate4 = animInfo.rotate[num7];
                            float num8 = num6 + aEPAnimationRotate4.angleChange + rotation;
                            num6 += aEPAnimationRotate4.angleChange;
                            animationCurve7.AddKey(KeyframeUtil.GetNew(aEPAnimationRotate4.time, Mathf.Cos((180f + num8 / 2f) * 0.0174532924f), KeyframeUtil.GetTangleMode(aEPAnimationRotate4.tangentType)));
                        }
                        AEPAnimationCurve item7 = new AEPAnimationCurve(binding7, animationCurve7);
                        this.listCurve.Add(item7);
                    }
                    else
                    {
                        Debug.LogWarning("Can not find reference object for Rotation:" + boneElement.name);
                    }
                }
                if (animInfo.scale != null)
                {
                    BoneElement boneElement2 = jsonFinal.GetBoneElement(animInfo.name);
                    if (boneElement2 != null)
                    {
                        Vector2 vector = new Vector2(boneElement2.scaleX, boneElement2.scaleY);
                        EditorCurveBinding binding8 = default(EditorCurveBinding);
                        AnimationCurve animationCurve8 = new AnimationCurve();
                        binding8.type = (typeof(Transform));
                        binding8.path = path;
                        binding8.propertyName = "m_LocalScale.x";
                        for (int num9 = 0; num9 < animInfo.scale.Count; num9++)
                        {
                            AEPAnimationScale aEPAnimationScale = animInfo.scale[num9];
                            animationCurve8.AddKey(KeyframeUtil.GetNew(aEPAnimationScale.time, aEPAnimationScale.x * vector.x, KeyframeUtil.GetTangleMode(aEPAnimationScale.tangentType)));
                        }
                        AEPAnimationCurve item8 = new AEPAnimationCurve(binding8, animationCurve8);
                        this.listCurve.Add(item8);
                        EditorCurveBinding binding9 = default(EditorCurveBinding);
                        AnimationCurve animationCurve9 = new AnimationCurve();
                        binding9.type = (typeof(Transform));
                        binding9.path = path;
                        binding9.propertyName = "m_LocalScale.y";
                        for (int num10 = 0; num10 < animInfo.scale.Count; num10++)
                        {
                            AEPAnimationScale aEPAnimationScale2 = animInfo.scale[num10];
                            animationCurve9.AddKey(KeyframeUtil.GetNew(aEPAnimationScale2.time, aEPAnimationScale2.y * vector.y, KeyframeUtil.GetTangleMode(aEPAnimationScale2.tangentType)));
                        }
                        AEPAnimationCurve item9 = new AEPAnimationCurve(binding9, animationCurve9);
                        this.listCurve.Add(item9);
                        EditorCurveBinding binding10 = default(EditorCurveBinding);
                        AnimationCurve animationCurve10 = new AnimationCurve();
                        binding10.type = (typeof(Transform));
                        binding10.path = path;
                        binding10.propertyName = "m_LocalScale.z";
                        for (int num11 = 0; num11 < animInfo.scale.Count; num11++)
                        {
                            AEPAnimationScale aEPAnimationScale3 = animInfo.scale[num11];
                            animationCurve10.AddKey(KeyframeUtil.GetNew(aEPAnimationScale3.time, 1f, KeyframeUtil.GetTangleMode(aEPAnimationScale3.tangentType)));
                        }
                        AEPAnimationCurve item10 = new AEPAnimationCurve(binding10, animationCurve10);
                        this.listCurve.Add(item10);
                    }
                    else
                    {
                        Debug.LogWarning("Can not find reference object for Scale:" + boneElement2.name);
                    }
                }
            }
        }

        public void AddAttactmentAnimation(AEPSlotAnimationElement animInfo, string path, GameObject aepRoot, AEPJsonFinal jsonFinal, SpriteType buildSpriteType)
        {
            GameObject refenreceObject = AEPAnimationClipElement.GetRefenreceObject(path, aepRoot);
            if (refenreceObject == null)
            {
                Debug.LogError("Not have object reference:" + path);
            }
            else
            {
                if (animInfo.attachment != null)
                {
                    EditorCurveBinding binding = default(EditorCurveBinding);
                    AnimationCurve animationCurve = new AnimationCurve();
                    binding.type = (typeof(GameObject));
                    binding.path = path;
                    binding.propertyName = "m_IsActive";
                    if (animInfo.attachment.Count > 0)
                    {
                        Keyframe keyframe = default(Keyframe);
                        keyframe.tangentMode = (31);
                        keyframe.time=(0f);
                        keyframe.value = (1f);
                        animationCurve.AddKey(KeyframeUtil.GetNew(0f, keyframe.value, TangentMode.Stepped));
                        AEPAnimationCurve item = new AEPAnimationCurve(binding, animationCurve);
                        this.listCurve.Add(item);
                    }
                    for (int i = 0; i < refenreceObject.transform.childCount; i++)
                    {
                        GameObject gameObject = refenreceObject.transform.GetChild(i).gameObject;
                        bool flag = false;
                        if (buildSpriteType == SpriteType.SpriteRenderer)
                        {
                            if (gameObject.GetComponent<SpriteRenderer>() != null)
                            {
                                flag = true;
                            }
                        }
                        else if (gameObject.GetComponent<Image>() != null)
                        {
                            flag = true;
                        }
                        if (flag)
                        {
                            string path2 = path + "/" + gameObject.name;
                            EditorCurveBinding binding2 = default(EditorCurveBinding);
                            AnimationCurve animationCurve2 = new AnimationCurve();
                            binding2.type = (typeof(GameObject));
                            binding2.path = path2;
                            binding2.propertyName = "m_IsActive";
                            if (animInfo.attachment.Count > 0)
                            {
                                if (animInfo.attachment[0].time > 0f)
                                {
                                    Keyframe keyframe2 = default(Keyframe);
                                    keyframe2.tangentMode = (31);
                                    keyframe2.time = (0f);
                                    keyframe2.value = (0f);
                                    animationCurve2.AddKey(KeyframeUtil.GetNew(0f, keyframe2.value, TangentMode.Stepped));
                                }
                            }
                            for (int j = 0; j < animInfo.attachment.Count; j++)
                            {
                                AEPAnimationAttachment aEPAnimationAttachment = animInfo.attachment[j];
                                Keyframe keyframe3 = default(Keyframe);
                                keyframe3.tangentMode = (31);
                                keyframe3.time = (aEPAnimationAttachment.time);
                                if (aEPAnimationAttachment.name == null || aEPAnimationAttachment.name.Length < 1)
                                {
                                    keyframe3.value = (0f);
                                }
                                else if (aEPAnimationAttachment.name == gameObject.name)
                                {
                                    keyframe3.value = (1f);
                                }
                                else
                                {
                                    keyframe3.value = (0f);
                                }
                                animationCurve2.AddKey(KeyframeUtil.GetNew(aEPAnimationAttachment.time, keyframe3.value, TangentMode.Stepped));
                            }
                            AEPAnimationCurve item2 = new AEPAnimationCurve(binding2, animationCurve2);
                            this.listCurve.Add(item2);
                        }
                    }
                }
                if (animInfo.color != null)
                {
                    for (int k = 0; k < refenreceObject.transform.childCount; k++)
                    {
                        GameObject gameObject2 = refenreceObject.transform.GetChild(k).gameObject;
                        bool flag2 = false;
                        if (buildSpriteType == SpriteType.SpriteRenderer)
                        {
                            if (gameObject2.GetComponent<SpriteRenderer>() != null)
                            {
                                flag2 = true;
                            }
                        }
                        else if (gameObject2.GetComponent<Image>() != null)
                        {
                            flag2 = true;
                        }
                        if (flag2)
                        {
                            string path3 = path + "/" + gameObject2.name;
                            EditorCurveBinding binding3 = default(EditorCurveBinding);
                            AnimationCurve animationCurve3 = new AnimationCurve();
                            if (buildSpriteType == SpriteType.SpriteRenderer)
                            {
                                binding3.type = (typeof(SpriteRenderer));
                            }
                            else
                            {
                                binding3.type = (typeof(Image));
                            }
                            binding3.path = path3;
                            binding3.propertyName = "m_Color.r";
                            for (int l = 0; l < animInfo.color.Count; l++)
                            {
                                Color color = EditorUtil.HexToColor(animInfo.color[l].color);
                                animationCurve3.AddKey(KeyframeUtil.GetNew(animInfo.color[l].time, color.r, KeyframeUtil.GetTangleMode(animInfo.color[l].tangentType)));
                            }
                            AEPAnimationCurve item3 = new AEPAnimationCurve(binding3, animationCurve3);
                            this.listCurve.Add(item3);
                            EditorCurveBinding binding4 = default(EditorCurveBinding);
                            AnimationCurve animationCurve4 = new AnimationCurve();
                            if (buildSpriteType == SpriteType.SpriteRenderer)
                            {
                                binding4.type = (typeof(SpriteRenderer));
                            }
                            else
                            {
                                binding4.type = (typeof(Image));
                            }
                            binding4.path = path3;
                            binding4.propertyName = "m_Color.b";
                            for (int m = 0; m < animInfo.color.Count; m++)
                            {
                                Color color2 = EditorUtil.HexToColor(animInfo.color[m].color);
                                animationCurve4.AddKey(KeyframeUtil.GetNew(animInfo.color[m].time, color2.b, KeyframeUtil.GetTangleMode(animInfo.color[m].tangentType)));
                            }
                            AEPAnimationCurve item4 = new AEPAnimationCurve(binding4, animationCurve4);
                            this.listCurve.Add(item4);
                            EditorCurveBinding binding5 = default(EditorCurveBinding);
                            AnimationCurve animationCurve5 = new AnimationCurve();
                            if (buildSpriteType == SpriteType.SpriteRenderer)
                            {
                                binding5.type = (typeof(SpriteRenderer));
                            }
                            else
                            {
                                binding5.type = (typeof(Image));
                            }
                            binding5.path = path3;
                            binding5.propertyName = "m_Color.gapAngle";
                            for (int n = 0; n < animInfo.color.Count; n++)
                            {
                                Color color3 = EditorUtil.HexToColor(animInfo.color[n].color);
                                animationCurve5.AddKey(KeyframeUtil.GetNew(animInfo.color[n].time, color3.g, KeyframeUtil.GetTangleMode(animInfo.color[n].tangentType)));
                            }
                            AEPAnimationCurve item5 = new AEPAnimationCurve(binding5, animationCurve5);
                            this.listCurve.Add(item5);
                            EditorCurveBinding binding6 = default(EditorCurveBinding);
                            AnimationCurve animationCurve6 = new AnimationCurve();
                            if (buildSpriteType == SpriteType.SpriteRenderer)
                            {
                                binding6.type = (typeof(SpriteRenderer));
                            }
                            else
                            {
                                binding6.type =(typeof(Image));
                            }
                            binding6.path = path3;
                            binding6.propertyName = "m_Color.a";
                            for (int num = 0; num < animInfo.color.Count; num++)
                            {
                                Color color4 = EditorUtil.HexToColor(animInfo.color[num].color);
                                animationCurve6.AddKey(KeyframeUtil.GetNew(animInfo.color[num].time, color4.a, KeyframeUtil.GetTangleMode(animInfo.color[num].tangentType)));
                            }
                            AEPAnimationCurve item6 = new AEPAnimationCurve(binding6, animationCurve6);
                            this.listCurve.Add(item6);
                        }
                    }
                }
            }
        }

        public void AddStartVisible(string boneName, string path, GameObject aepRoot, AEPJsonFinal jsonFinal, bool isVisible)
        {
            EditorCurveBinding binding = default(EditorCurveBinding);
            AnimationCurve animationCurve = new AnimationCurve();
            binding.type = typeof(GameObject);
            binding.path = path;
            binding.propertyName = "m_IsActive";
            if (isVisible)
            {
                animationCurve.AddKey(KeyframeUtil.GetNew(0f, 1f, TangentMode.Stepped));
            }
            else
            {
                animationCurve.AddKey(KeyframeUtil.GetNew(0f, 0f, TangentMode.Stepped));
            }
            AEPAnimationCurve item = new AEPAnimationCurve(binding, animationCurve);
            this.listCurve.Add(item);
        }
    }
}
#endif