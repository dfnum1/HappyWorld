using System;
using System.Security.Cryptography.X509Certificates;
using System.Linq;
using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TopGame.UI;
using TopGame.Core;
using Framework.Core;
using DG.Tweening;

namespace TopGame
{
    public class RoleTest : MonoBehaviour
    {
        [System.NonSerialized]
        public int curSelectIdx = 0;
        public UnityEngine.UI.Dropdown dropdown;

        public List<RectTransform> scrollContent;

        public GameObject animCell;

        [System.NonSerialized]
        public Animator[] m_Animators;

        List<UIRoleTest.AssetFile> m_vList = null;

        public int panelType; // 0 role 1 boss 2 monster
        public Transform centerStand;
        public Transform leftStand;
        public Transform rightStand;

        AInstanceAble m_pModel = null;
        AInstanceAble m_pHeightModel = null;

        AnimationClip m_pModelPlayClip = null;
        AnimationClip m_pHeightModelPlayClip = null;
        float m_fPlayTime = 0;
        float m_fHeightPlayTime =0;

        private bool bInit = false;

        // Start is called before the first frame update
        void Awake()
        {
            bInit = true;
        }
        //------------------------------------------------------
        private void OnDestroy()
        {
            Clear();
        }
        //------------------------------------------------------
        public void Clear()
        {
            if (m_pModel != null)
            {
                m_pModel.RecyleDestroy();
            }
            if (m_pHeightModel != null) m_pHeightModel.RecyleDestroy();
            m_pModel = null;
            m_pHeightModel = null;
            m_pModelPlayClip = null;
            m_pHeightModelPlayClip = null;
        }
        //------------------------------------------------------
        public void AddDropItem(List<UIRoleTest.AssetFile> vItems)
        {
            if (dropdown == null) return;
            m_vList = vItems;
            dropdown.ClearOptions();
            dropdown.value = -1;
            dropdown.onValueChanged.AddListener(OnSelect);
            for (int i = 0; i < m_vList.Count; ++i)
                dropdown.options.Add(new Dropdown.OptionData(m_vList[i].name));
            OnSelect(0);
        }
        //------------------------------------------------------
        void OnItemCellShow(GameObject item, AnimationClip animationClip, string clipName)
        {
            Text aniNameText = item.transform.Find("name").GetComponent<Text>();
            if (aniNameText)
            {
                aniNameText.text = animationClip.name.Replace(clipName, "");
            }
        }
        //------------------------------------------------------
        /// <summary>
        /// 设置高模低模显示 只对人物生效
        /// </summary>
        /// <param name="type">1 全开 2关低模 3关高模</param>
        public void SetHighLowModel(int type)
        {
            if(type == 1)
            {
                if (m_pModel != null) m_pModel.gameObject.SetActive(true);
                if (m_pHeightModel != null) m_pHeightModel.gameObject.SetActive(true);
            }
            else if (type == 2)
            {
                if (m_pModel != null) m_pModel.gameObject.SetActive(false);
                if (m_pHeightModel != null) m_pHeightModel.gameObject.SetActive(true);
            }
            else if (type == 3)
            {
                if (m_pModel != null) m_pModel.gameObject.SetActive(true);
                if (m_pHeightModel != null) m_pHeightModel.gameObject.SetActive(false);
            }
        }
        //------------------------------------------------------
        void OnSpawnInstance(InstanceOperiaon instOP)
        {
            bool heightModel = false;
            if(instOP.HasData<Variable2>(0))
            {
                heightModel = instOP.GetUserData<Variable2>(0).intVal0!=0;
            }
            if(heightModel)
            {
                if (m_pHeightModel != null) m_pHeightModel.RecyleDestroy();
                m_pHeightModel = instOP.pPoolAble;
                if (m_pHeightModel)
                {
                    m_pHeightModel.SetPosition(instOP.GetUserData<Variable3>(1).ToVector3());
                    m_pHeightModel.SetEulerAngle(instOP.GetUserData<Variable3>(2).ToVector3());
                    m_pHeightModel.SetScale(instOP.GetUserData<Variable3>(3).ToVector3());
                    Animator animator = m_pHeightModel.GetBehaviour<Animator>();
                    SetAnimatorStates(animator, 1);
                }
            }
            else
            {

                if (m_pModel != null) m_pModel.RecyleDestroy();
                m_pModel = instOP.pPoolAble;
                if (m_pModel)
                {
                    m_pModel.SetPosition(instOP.GetUserData<Variable3>(1).ToVector3());
                    m_pModel.SetEulerAngle(instOP.GetUserData<Variable3>(2).ToVector3());
                    m_pModel.SetScale(instOP.GetUserData<Variable3>(3).ToVector3());
                    Animator animator = m_pModel.GetBehaviour<Animator>();
                    SetAnimatorStates(animator, 0);
                }
            }
        }
        //------------------------------------------------------
        void OnSpawnSign(InstanceOperiaon instOP)
        {
            if (!instOP.HasData<Variable2>(0))
            {
                instOP.SetUsed(false);
                return;
            }
            instOP.SetUsed(instOP.GetUserData<Variable2>(0).intVal1 == curSelectIdx);
        }
        //------------------------------------------------------
        public void OnSelect(int val)
        {
            Clear();
            curSelectIdx = val;
            dropdown.value = curSelectIdx;
            dropdown.captionText.text = m_vList[curSelectIdx].name;

            if (!string.IsNullOrEmpty(m_vList[curSelectIdx].file))
            {
                InstanceOperiaon instOP = FileSystemUtil.SpawnInstance(m_vList[curSelectIdx].file, false);
                if (instOP != null)
                {
                    Transform parent = leftStand;
                    if (string.IsNullOrEmpty(m_vList[curSelectIdx].height_file))
                    {
                        parent = centerStand;
                        instOP.SetByParent(centerStand);
                    }
                    else instOP.SetByParent(leftStand);
                    instOP.SetUserData(0, new Variable2() { intVal0 = 0, intVal1 = curSelectIdx });
                    instOP.SetUserData(1, new Variable3() { floatVal0 = parent.position.x, floatVal1 = parent.position.y, floatVal2 = parent.position.z });
                    instOP.SetUserData(2, new Variable3() { floatVal0 = parent.eulerAngles.x, floatVal1 = parent.eulerAngles.y, floatVal2 = parent.eulerAngles.z });
                    instOP.SetUserData(3, new Variable3() { floatVal0 = parent.localScale.x, floatVal1 = parent.localScale.y, floatVal2 = parent.localScale.z });
                    instOP.OnCallback += OnSpawnInstance;
                    instOP.OnSign += OnSpawnSign;
                    instOP.Refresh();
                }
            }
            if (!string.IsNullOrEmpty(m_vList[curSelectIdx].height_file))
            {
                InstanceOperiaon instOP = FileSystemUtil.SpawnInstance(m_vList[curSelectIdx].height_file, false);
                if (instOP != null)
                {
                    Transform parent = rightStand;
                    if (string.IsNullOrEmpty(m_vList[curSelectIdx].file))
                    {
                        parent = centerStand;
                        instOP.SetByParent(centerStand);
                    }
                    else instOP.SetByParent(rightStand);
                    instOP.SetUserData(0, new Variable2() { intVal0 = 1, intVal1 = curSelectIdx });
                    instOP.SetUserData(1, new Variable3() { floatVal0 = parent.position.x, floatVal1 = parent.position.y, floatVal2 = parent.position.z });
                    instOP.SetUserData(2, new Variable3() { floatVal0 = parent.eulerAngles.x, floatVal1 = parent.eulerAngles.y, floatVal2 = parent.eulerAngles.z });
                    instOP.SetUserData(3, new Variable3() { floatVal0 = parent.localScale.x, floatVal1 = parent.localScale.y, floatVal2 = parent.localScale.z });
                    instOP.OnCallback += OnSpawnInstance;
                    instOP.OnSign += OnSpawnSign;
                    instOP.Refresh();
                }
            }

            SetModelRotate(false);
        }
        //------------------------------------------------------
        // Update is called once per frame
        void Update()
        {
            Shader.globalMaximumLOD = 200;
            if(m_pModelPlayClip!=null && m_pModel!=null)
            {
                m_fPlayTime += Time.fixedDeltaTime;
                m_pModelPlayClip.SampleAnimation(m_pModel.gameObject, m_fPlayTime);
                if (m_fPlayTime >= m_pModelPlayClip.length)
                    m_fPlayTime = 0;
            }
            if (m_pHeightModelPlayClip != null && m_pHeightModel != null)
            {
                m_fHeightPlayTime += Time.fixedDeltaTime;
                m_pHeightModelPlayClip.SampleAnimation(m_pHeightModel.gameObject, m_fHeightPlayTime);
                if (m_fHeightPlayTime >= m_pHeightModelPlayClip.length)
                    m_fHeightPlayTime = 0;
            }
        }
        //------------------------------------------------------
        void SetAnimatorStates(Animator animator, int index)
        {
            if (index < 0 || index >= scrollContent.Count) return;
            RectTransform scroll = scrollContent[index];
            if (scroll == null) return;
            int useCnt = 0;
            if(animator!=null)
            {
                RuntimeAnimatorController animatorController = animator.runtimeAnimatorController;
                if (animatorController != null)
                {
                    int animCount = animatorController.animationClips.Length;
                    useCnt = animCount;
                    for (int k = 0; k < animCount; k++)
                    {
                        Transform cell = scroll.Find("animCell" + k.ToString());

                        if (!cell)
                        {
                            cell = GameObject.Instantiate(animCell).transform;
                            EventTriggerListener.Get(cell.gameObject).param1 = new Variable2() { intVal0 = index, intVal1 = k }; //animator
                            EventTriggerListener.Get(cell.gameObject).onClick = SetClickAnimCell;
                        }

                        cell.name = "animCell" + k.ToString();
                        cell.parent = scroll;
                        cell.localScale = Vector3.one;
                        cell.localRotation = Quaternion.Euler(Vector3.zero);
                        cell.gameObject.SetActive(true);
                        OnItemCellShow(cell.gameObject, animatorController.animationClips[k], animator.gameObject.name.Replace("(Clone)", "_"));
                    }
                }
           //     animator.enabled = false;
            }
            
            //隐藏其他动画
            for (int j = useCnt; j < scroll.childCount; j++)
            {
                Transform cell = scroll.Find("animCell" + j.ToString());
                if (cell) cell.gameObject.SetActive(false);
            }
            //动态设置content大小
            scroll.sizeDelta = new Vector2(scroll.sizeDelta.x, useCnt *
                (animCell.GetComponent<RectTransform>().sizeDelta.y +
                scroll.GetComponent<VerticalLayoutGroup>().spacing));
            //归位
            scroll.anchoredPosition = new Vector2(scroll.anchoredPosition.x, 0);
        }
        //------------------------------------------------------
        public void SetModelRotate(bool bRotate)
        {
            PlayTween(leftStand, bRotate);
            PlayTween(centerStand, bRotate);
            PlayTween(rightStand, bRotate);
        }
        //------------------------------------------------------
        void PlayTween(Transform transform, bool bPlay)
        {
            if (transform == null) return;
            DOTweenAnimation[] tweens = transform.GetComponents<DOTweenAnimation>();
            if (tweens == null)
            {
                for (int k = 0; k < tweens.Length; k++)
                {
                    if (bPlay)
                    {
                        tweens[k].DOPlayForward();
                    }
                    else
                    {
                        tweens[k].DOPause();
                    }
                }
            }
        }
        //------------------------------------------------------
        public void SetClickAnimCell(GameObject gameObject, VariablePoolAble[] param)
        {
            Variable2 var = (Variable2)param[0];
            AInstanceAble pSyncAble = null;
            AInstanceAble pAble = null;
            if (var.intVal0 == 0)
            {
                pAble = m_pModel;
                pSyncAble = m_pHeightModel;
            }
            else
            {
                pAble = m_pHeightModel;
                pSyncAble = m_pModel;
            }
            if (pAble == null) return;
            AnimationClip clip = null;
            Animator animator = pAble.GetBehaviour<Animator>();
            if (animator)
            {
                RuntimeAnimatorController animatorController = animator.runtimeAnimatorController;
                if (!animatorController || var.intVal1 >= animatorController.animationClips.Length) return;
                if (var.intVal0 == 0)
                {
                    m_fPlayTime = 0;
                    m_pModelPlayClip = animatorController.animationClips[var.intVal1];
                    clip = m_pModelPlayClip;
                }
                else
                {
                    m_fHeightPlayTime = 0;
                    m_pHeightModelPlayClip = animatorController.animationClips[var.intVal1];
                    clip = m_pHeightModelPlayClip;
                }
            }
            if(pSyncAble && clip)
            {
                animator = pSyncAble.GetBehaviour<Animator>();
                if (animator)
                {
                    RuntimeAnimatorController animatorController = animator.runtimeAnimatorController;
                    if (!animatorController || var.intVal1 >= animatorController.animationClips.Length) return;
                    string strName = clip.name.Replace(pAble.name.Replace("(Clone)", "_"), "");
                    for (int i =0; i < animatorController.animationClips.Length; ++i)
                    {
                        if(animatorController.animationClips[i].name.Contains(strName))
                        {
                            if (var.intVal0 == 0)
                            {
                                m_fHeightPlayTime = 0;
                                m_pHeightModelPlayClip = animatorController.animationClips[i];
                            }
                            else
                            {
                                m_fPlayTime = 0;
                                m_pModelPlayClip = animatorController.animationClips[i];
                            }
                            break;
                        }
                    }
                }
            }
        }
    }
}