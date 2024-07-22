using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
#endif

namespace Unity.Animations.SpringBones
{
    public class SpringManager : MonoBehaviour
    {
        [Header("Properties")]
        public string[] activeByActions;
        [System.NonSerialized]
        HashSet<int> m_vActiveByActions;
        [SerializeField]
        public Animator m_pAniamtor;
        bool m_bActiveByAction = true;
        bool m_bInitBone = false;

        [Header("Properties")]
        public bool automaticUpdates = true;
        public bool isPaused = false;
        public int simulationFrameRate = 60;
        [Range(0f, 1f)]
        public float dynamicRatio = 0.5f;
        public Vector3 gravity = new Vector3(0f, -10f, 0f);
        [Range(0f, 1f)]
        public float bounce = 0f;
        [Range(0f, 1f)]
        public float friction = 1f;

        [Header("Constraints")]
        public bool enableAngleLimits = true;
        public bool enableCollision = true;
        public bool enableLengthLimits = true;

        [Header("Ground Collision")]
        public bool collideWithGround = true;
        public float groundHeight = 0f;

        [Header("Bones")]
       // [HideInInspector]
        public SpringBone[] springBones;

        private bool[] boneIsAnimatedStates;
        private List<ForceProvider> m_forceProviders = null;

        private float m_fDelayInit = 1;
        public bool isActiveCollision
        {
            get { return enableCollision && m_bActiveByAction; }
        }

        public void CleanUpBoneNullColliders()
        {
            foreach (var bone in springBones)
            {
                bone.CleanUpBoneNullColliders();
            }
        }

        // Tells the SpringManager which bones are moved by animation.
        // Can be called on a per-animation basis.
        public void UpdateBoneIsAnimatedStates(IList<string> animatedBoneNames)
        {
            if (boneIsAnimatedStates == null
                || boneIsAnimatedStates.Length != springBones.Length)
            {
                boneIsAnimatedStates = new bool[springBones.Length];
            }

            var boneCount = springBones.Length;
            for (int boneIndex = 0; boneIndex < boneCount; boneIndex++)
            {
                boneIsAnimatedStates[boneIndex] = animatedBoneNames.Contains(springBones[boneIndex].name);
            }
        }

        public void UpdateDynamics()
        {
            if (springBones == null) return;
            var boneCount = springBones.Length;

            if (isPaused)
            {
                for (var boneIndex = 0; boneIndex < boneCount; boneIndex++)
                {
                    var springBone = springBones[boneIndex];
                    if (springBone.enabled)
                    {
                        springBone.ComputeRotation(boneIsAnimatedStates[boneIndex] ? dynamicRatio : 1f);
                    }
                }
                return;
            }

            var timeStep = (simulationFrameRate > 0) ?
                (1f / simulationFrameRate) :
                Time.deltaTime;

            for (var boneIndex = 0; boneIndex < boneCount; boneIndex++)
            {
                var springBone = springBones[boneIndex];
                if (springBone.enabled)
                {
                    var sumOfForces = GetSumOfForcesOnBone(springBone);
                    springBone.UpdateSpring(timeStep, sumOfForces);
                    springBone.SatisfyConstraintsAndComputeRotation(this,
                        timeStep, boneIsAnimatedStates[boneIndex] ? dynamicRatio : 1f);
                }
            }
        }

        private Vector3 GetSumOfForcesOnBone(SpringBone springBone)
        {
            var sumOfForces = gravity;
            if(m_forceProviders !=null)
            {
                var providerCount = m_forceProviders.Count;
                for (var providerIndex = 0; providerIndex < providerCount; ++providerIndex)
                {
                    var forceProvider = m_forceProviders[providerIndex];
                    if (forceProvider.isActiveAndEnabled)
                    {
                        sumOfForces += forceProvider.GetForceOnBone(springBone);
                    }
                }
            }
            return sumOfForces;
        }

        private void OnEnable()
        {
            m_fDelayInit = 1;
            CheckActiveAction();
        }

        private void Awake()
        {
            m_bInitBone = false;
            m_bActiveByAction = true;
            m_vActiveByActions = null;
            if (activeByActions != null && activeByActions.Length > 0)
            {
                m_bActiveByAction = false;
                m_vActiveByActions = new HashSet<int>();
                for (int i = 0; i < activeByActions.Length; ++i)
                {
                    m_vActiveByActions.Add(Animator.StringToHash(activeByActions[i]));
                }
            }
        }

        private void Start()
        {
            m_fDelayInit = 1;
            var boneCount = springBones.Length;
            if (boneIsAnimatedStates == null || boneIsAnimatedStates.Length != boneCount)
            {
                boneIsAnimatedStates = new bool[boneCount];
            }
        }

        public void ForceIntialize()
        {
            m_fDelayInit = 1;
        }

        void CheckActiveAction()
        {
            bool preState = m_bActiveByAction;
            m_bActiveByAction = true;
            if (m_vActiveByActions != null)
            {
                m_bActiveByAction = false;

                if(Framework.Core.GraphPlayableUtil.IsGraphPlayable(this.gameObject))
                {
                    foreach (var db in m_vActiveByActions)
                    {
                        if (Framework.Core.GraphPlayableUtil.IsPlaying(this.gameObject, db))
                        {
                            m_bActiveByAction = true;
                            if(m_bInitBone)  m_bInitBone = preState == m_bActiveByAction;
                            break;
                        }
                    }
                }
                else
                {
                    if (!m_bActiveByAction)
                    {
                        if (m_pAniamtor)
                        {
                            AnimatorStateInfo state = m_pAniamtor.GetCurrentAnimatorStateInfo(0);
                            if (state.shortNameHash == 0 || m_vActiveByActions.Contains(state.shortNameHash))
                            {
                                m_bActiveByAction = true;
                                if (m_bInitBone) m_bInitBone = preState == m_bActiveByAction;
                            }
                        }
                    }
                }
            }
        }

        private void LateUpdate()
        {
            Vector3 pos = transform.position;
            if (Mathf.Abs(pos.x) >= 9999f && Mathf.Abs(pos.y) >= 9999f && Mathf.Abs(pos.z) >= 9999f) return;
            if(m_fDelayInit>0)
            {
                m_fDelayInit -= Time.deltaTime;
                return;
            }
            if(m_fDelayInit <0)
            {
                CheckActiveAction();
            }
            if (m_fDelayInit > 0) return;
            if (!m_bInitBone)
            {
                if (isActiveCollision)
                {
                    var boneCount = springBones.Length;
                    for (int boneIndex = 0; boneIndex < boneCount; boneIndex++)
                    {
                        springBones[boneIndex].Initialize();
                    }
                }
                m_bInitBone = true;
            }
            if (automaticUpdates && m_bActiveByAction) { UpdateDynamics(); }
        }

#if UNITY_EDITOR
        private Vector3[] groundPoints;

        private void OnDrawGizmos()
        {
            if (collideWithGround)
            {
                if (groundPoints == null)
                {
                    groundPoints = new Vector3[] {
                        new Vector3(-1f, 0f, -1f),
                        new Vector3( 1f, 0f, -1f),
                        new Vector3( 1f, 0f,  1f),
                        new Vector3(-1f, 0f,  1f)
                    };
                }

                var characterPosition = transform.position;
                var groundOrigin = new Vector3(characterPosition.x, groundHeight, characterPosition.z);
                var pointCount = groundPoints.Length;
                Gizmos.color = Color.yellow;
                for (int pointIndex = 0; pointIndex < pointCount; pointIndex++)
                {
                    var endPointIndex = (pointIndex + 1) % pointCount;
                    Gizmos.DrawLine(
                        groundOrigin + groundPoints[pointIndex],
                        groundOrigin + groundPoints[endPointIndex]);
                }
            }

            DrawBones();
        }

        private List<SpringBone> selectedBones;
        private Vector3[] boneLines;

        private void DrawBones()
        {
            // Draw each item by color to reduce Material.SetPass calls
            var boneCount = springBones.Length;
            IList<SpringBone> bonesToDraw = springBones;
            if (onlyShowSelectedBones)
            {
                if (selectedBones == null) { selectedBones = new List<SpringBone>(boneCount); }
                selectedBones.Clear();
                var selection = UnityEditor.Selection.gameObjects;
                for (int boneIndex = 0; boneIndex < boneCount; boneIndex++)
                {
                    var bone = springBones[boneIndex];
                    var pivot = (bone.pivotNode != null) ? bone.pivotNode.gameObject : null;
                    if (selection.Contains(bone.gameObject) || selection.Contains(pivot))
                    {
                        selectedBones.Add(springBones[boneIndex]);
                    }
                }
                bonesToDraw = selectedBones;
                boneCount = bonesToDraw.Count;
            }

            UnityEditor.Handles.color = new Color(0.2f, 1f, 0.2f);
            for (int boneIndex = 0; boneIndex < boneCount; boneIndex++)
            {
                var bone = bonesToDraw[boneIndex];
                bone.DrawAngleLimits(bone.yAngleLimits, angleLimitDrawScale);
            }

            UnityEditor.Handles.color = new Color(0.7f, 0.7f, 1f);
            for (int boneIndex = 0; boneIndex < boneCount; boneIndex++)
            {
                var bone = bonesToDraw[boneIndex];
                bone.DrawAngleLimits(bone.zAngleLimits, angleLimitDrawScale);
            }

            var linePointCount = boneCount * 2;
            if (boneLines == null || boneLines.Length != linePointCount)
            {
                boneLines = new Vector3[linePointCount];
            }

            var pointIndex = 0;
            for (int boneIndex = 0; boneIndex < boneCount; boneIndex++)
            {
                var bone = bonesToDraw[boneIndex];
                var origin = bone.transform.position;
                var pivotForward = -bone.GetPivotTransform().right;
                boneLines[pointIndex] = origin;
                boneLines[pointIndex + 1] = origin + angleLimitDrawScale * pivotForward;
                pointIndex += 2;
            }
            UnityEditor.Handles.color = Color.gray;
            UnityEditor.Handles.DrawLines(boneLines);

            pointIndex = 0;
            for (int boneIndex = 0; boneIndex < boneCount; boneIndex++)
            {
                var bone = bonesToDraw[boneIndex];
                boneLines[pointIndex] = bone.transform.position;
                boneLines[pointIndex + 1] = bone.EditorComputeChildPosition();
                pointIndex += 2;
            }
            UnityEditor.Handles.color = boneColor;
            UnityEditor.Handles.DrawLines(boneLines);

            if (showBoneSpheres)
            {
                Gizmos.color = new Color(0f, 0f, 0f, 0f);
                for (int boneIndex = 0; boneIndex < boneCount; boneIndex++)
                {
                    bonesToDraw[boneIndex].DrawSpringBoneCollision();
                }
            }

            for (int boneIndex = 0; boneIndex < boneCount; boneIndex++)
            {
                bonesToDraw[boneIndex].MarkCollidersForDrawing();
            }

            if (showBoneNames)
            {
                for (int boneIndex = 0; boneIndex < boneCount; boneIndex++)
                {
                    var bone = bonesToDraw[boneIndex];
                    UnityEditor.Handles.Label(bone.transform.position, bone.name);
                }
            }
        }

        [Header("Gizmos")]
        public Color boneColor = Color.yellow;
        public Color colliderColor = Color.gray;
        public Color collisionColor = Color.red;
        public Color groundCollisionColor = Color.green;
        public float angleLimitDrawScale = 0.05f;

        public static bool onlyShowSelectedBones = true;
        public static bool onlyShowSelectedColliders = false;
        public static bool showBoneSpheres = true;
        public static bool showBoneNames = false;

        // Can't declare as const...
        public static Color DefaultBoneColor { get { return Color.yellow; } }
        public static Color DefaultColliderColor { get { return Color.gray; } }
        public static Color DefaultCollisionColor { get { return Color.red; } }
        public static Color DefaultGroundCollisionColor { get { return Color.green; } }

        public void FindSpringBones(bool includeInactive = false)
        {
            var unsortedSpringBones = GetComponentsInChildren<SpringBone>(includeInactive);
            var boneDepthList = unsortedSpringBones.Select(bone => new { bone, depth = GetObjectDepth(bone.transform) })
                .ToList();
            boneDepthList.Sort((a, b) => a.depth.CompareTo(b.depth));
            springBones = boneDepthList.Select(item => item.bone).ToArray();
        }

        // Get the depth of an object (number of consecutive parents)
        private static int GetObjectDepth(Transform inObject)
        {
            var depth = 0;
            var currentObject = inObject;
            while (currentObject != null)
            {
                currentObject = currentObject.parent;
                ++depth;
            }
            return depth;
        }
#endif
    }
#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(SpringManager), true)]
    class SpringManagerEditor : UnityEditor.Editor
    {
        private void OnEnable()
        {
            SpringManager sprite = target as SpringManager;
            if (sprite.m_pAniamtor == null) sprite.m_pAniamtor = sprite.GetComponent<Animator>();
            sprite.FindSpringBones(true);
        }
        public override void OnInspectorGUI()
        {
            SpringManager sprite = target as SpringManager;
            serializedObject.Update();
            base.OnInspectorGUI();
            if (GUILayout.Button("刷新保存"))
            {
                if (sprite.m_pAniamtor == null) sprite.m_pAniamtor = sprite.GetComponent<Animator>();
                sprite.FindSpringBones(true);
                EditorUtility.SetDirty(target);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}
