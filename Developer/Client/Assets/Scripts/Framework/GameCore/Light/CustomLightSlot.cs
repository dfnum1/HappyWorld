using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace TopGame.Core
{
    [ExecuteInEditMode]
	public class CustomLightSlot : MonoBehaviour
	{
        static bool ms_bDirty = true;
        static List<CustomLightSlot> ms_Slots = new List<CustomLightSlot>();
        public int nCullingMask = 1<< (int)Base.EMaskLayer.UI_3D;
        public LightShadows shadowTypes = LightShadows.Soft;
        public bool bSyncRotate = false;

        public bool syncLightDirection = false;
        //------------------------------------------------------
        private void OnEnable()
        {
            if (!ms_Slots.Contains(this))
            {
                ms_Slots.Add(this);
                ms_bDirty = true;
                DirtyCustomLight();
            }
        }
        //------------------------------------------------------
        private void OnDisable()
        {
            int index = ms_Slots.IndexOf(this);
            if (index >= 0)
            {
                ms_Slots.RemoveAt(index);
                ms_bDirty = true;
                DirtyCustomLight();
            }
        }
#if UNITY_EDITOR
        private void Update()
        {
            if (ms_Slots == null || ms_Slots.Count <= 0) return;
            if(ms_Slots[ms_Slots.Count-1] == this)
            {
                CustomLightSystem.SetSyncSlot(this);
            }
        }
#endif
        //------------------------------------------------------
        private void OnDestroy()
        {
            int index = ms_Slots.IndexOf(this);
            if (index >= 0)
            {
                ms_Slots.RemoveAt(index);
                ms_bDirty = true;
                DirtyCustomLight();
            }
        }
        //------------------------------------------------------
        static void DirtyCustomLight()
        {
            if(ms_bDirty)
            {
                ms_bDirty = false;
                if (ms_Slots.Count > 0)
                    CustomLightSystem.SetSyncSlot(ms_Slots[ms_Slots.Count-1]);
                else
                    CustomLightSystem.SetSyncSlot(null);
            }
        }
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(CustomLightSlot), true)]
    public class CustomLightSlotEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            CustomLightSlot slot = target as CustomLightSlot;

            slot.syncLightDirection = EditorGUILayout.Toggle("同步光朝向", slot.syncLightDirection);
            slot.bSyncRotate = EditorGUILayout.Toggle("角度同步", slot.bSyncRotate);
            slot.nCullingMask = Framework.ED.HandleUtilityWrapper.PopRenderLayerMask("生效层", slot.nCullingMask);
            slot.shadowTypes = (LightShadows)EditorGUILayout.EnumPopup("影子", slot.shadowTypes);
            serializedObject.ApplyModifiedProperties();
            if (GUILayout.Button("保存"))
            {
                EditorUtility.SetDirty(target);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            }
        }
    }
#endif
}
