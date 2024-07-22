// /********************************************************************
// 生成日期:	3:17:2022  15:05
// 类    名: 	StateConfig
// 作    者:	HappLI
// 描    述:	
// *********************************************************************/
// namespace TopGame.Logic
// {
//     [System.Serializable]
//     public struct StateConfig
//     {
//         public EGameState state;
// 
//         public bool IsValid
//         {
//             get { return state != EGameState.Count; }
//         }
// #if UNITY_EDITOR
//         public void OnInspector(System.Object param = null)
//         {
//             state = (EGameState)UnityEditor.EditorGUILayout.EnumPopup("状态", state);
//         }
// #endif
//         [System.NonSerialized]
//         public static StateConfig DEFAULT = new StateConfig() { state = EGameState.Count };
//     }
// }
