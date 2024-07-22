// /********************************************************************
// 生成日期:	1:11:2020 10:06
// 类    名: 	UGCLoopBrick
// 作    者:	HappLI
// 描    述:	UGC 循环组件
// *********************************************************************/
// using Framework.Core;
// using System;
// namespace Framework.UGC
// {
//     [System.Serializable]
//     public struct UGCLoopBrick : UGCBrick
//     {
//         public int id;
//         public int loopCnt;
//         public int[] nextBricks;
// 
//         //------------------------------------------------------
//         public int GetID() {  return id; }
//         //------------------------------------------------------
//         int UGCBrick.GetType()
//         {
//             return (int)EUGCBrickType.Loop;
//         }
//         //------------------------------------------------------
//         public void DrawUI()
//         {
//         }
//         //------------------------------------------------------
//         public bool Excude(VariablePoolAble[] inputParams)
//         {
//             return true;
//         }
//         //------------------------------------------------------
//         public int[] GetNexts()
//         {
//             return null;
//         }
//         //------------------------------------------------------
//         public int GetParamCount()
//         {
//             return 0;
//         }
//         //------------------------------------------------------
//         public VariablePoolAble GetParam(int index)
//         {
//         }
//     }
// }
// 
