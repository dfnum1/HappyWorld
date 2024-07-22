using UnityEngine;
namespace TopGame.Core
{
    [System.Serializable]
    public struct LODRank : Framework.Plugin.IQuickSort<LODRank>
    {
        public int index;
        public float distance;

#if UNITY_EDITOR
        public void OnInspector(System.Object param = null)
        {
            index = UnityEditor.EditorGUILayout.IntField("LOD¼¶±ð", index);
            distance = UnityEditor.EditorGUILayout.FloatField("LOD¾àÀë", distance);
        }
#endif
        //-----------------------------------------------------
        public int CompareTo(int userType, LODRank other)
        {
            if (distance < other.distance) return -1;
            return 1;
        }
        //-----------------------------------------------------
        public void Destroy()
        {
        }
    }

    public class LODNode : LinkListBehaviour<LODNode>
	{
        [SerializeField]
		LODGroup[] LODGroups = null;

		int m_lodIndex;

        private Transform m_pTransform = null;

		public int LodIndex
		{
			get
			{
				return m_lodIndex;
			}
		}
        //------------------------------------------------------
		public void OnInitGroup()
		{
            m_pTransform = transform;
            m_lodIndex = -1;
		}
        //------------------------------------------------------
        public override void Insert()
		{
			base.Insert();
			OnInitGroup();
		}
        //------------------------------------------------------
        public void OnUpdateLOD(int index)
		{
			if (LODGroups == null)
			{
				OnInitGroup();
				if (LODGroups == null)
				{
					return;
				}
			}

            if (m_lodIndex == index) return;

			for (int i = 0; i < LODGroups.Length; ++i)
			{
				if (LODGroups[i] == null)
					continue;

				if (LODGroups[i].lodCount <= index)
					continue;
                LODGroups[i].ForceLOD(index);
			}
			m_lodIndex = index;
		}
        //------------------------------------------------------
        public Vector3 GetPosition()
        {
            if (m_pTransform == null) return Vector3.zero;
            return m_pTransform.position;
        }
    }
}