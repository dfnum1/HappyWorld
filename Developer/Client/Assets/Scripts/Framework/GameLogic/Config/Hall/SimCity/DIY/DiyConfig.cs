// /********************************************************************
// 生成日期:	1:11:2020 10:06
// 类    名: 	DiyConfig
// 作    者:	HappLI
// 描    述:	DIY配置
// *********************************************************************/
// using Framework.Core;
// using System.Collections.Generic;
// using System.IO;
// using System.Linq;
// using TopGame.Core;
// using UnityEngine;
// 
// namespace TopGame.Logic
// {
//     public class DiyConfig : MonoBehaviour
//     {
//         public Vector2Int size;
//         public int cellSize;
// 
//         [System.Serializable]
//         public class Item : Framework.Core.VariablePoolAble
//         {
//             public int id;
//             public string prefab;
//             public Vector2Int size;
//             public int testCnt;
// 
//             public void Destroy()
//             {
//             }
//         }
//         public Item[] items;
// 
//         static DiyConfig ms_Instance;
//         //------------------------------------------------------
//         private void Awake()
//         {
//             ms_Instance = this;
//         }
//         //------------------------------------------------------
//         private void OnDestroy()
//         {
//             ms_Instance = null;
//         }
//         //------------------------------------------------------
//         public static Transform Root
//         {
//             get
//             {
//                 if (ms_Instance == null) return null;
//                 return ms_Instance.transform;
//             }
//         }
//         //------------------------------------------------------
//         public static Vector3 CenterPosition
//         {
//             get
//             {
//                 if (ms_Instance == null) return Vector3.zero;
//                 return ms_Instance.transform.position;
//             }
//         }
//         //------------------------------------------------------
//         public static Vector3 LTPosition
//         {
//             get
//             {
//                 if (ms_Instance == null) return Vector3.zero;
//                 return ms_Instance.transform.position - new Vector3(ZoomSize.x,0, ZoomSize.y)*0.5f;
//             }
//         }
//         //------------------------------------------------------
//         public static Vector2Int WorldPosToGridPos(Vector3 worldPos, float rotate, Vector2Int size)
//         {
//             if (ms_Instance == null) return Vector2Int.zero;
//             if (((rotate % 360 / 90) % 2) != 0)
//             {
//                 size.x = size.x ^ size.y;
//                 size.y = size.y ^ size.x;
//                 size.x = size.x ^ size.y;
//             }
//             Vector3 gridOffset = worldPos - new Vector3(size.x* CellSize, 0,size.y* CellSize) *0.5f - LTPosition;
//             int grid_x = Mathf.Clamp(Mathf.RoundToInt(gridOffset.x / CellSize),0, DiySize.x);
//             int grid_y = Mathf.Clamp(Mathf.RoundToInt(gridOffset.z / CellSize), 0, DiySize.y);
//             return new Vector2Int(grid_x, grid_y);
//         }
//         //------------------------------------------------------
//         public static Vector3 GridPosToWorldPos(Vector2Int gridPos, float rotate, Vector2Int size)
//         {
//             if (ms_Instance == null) return Vector3.zero;
//             if (((rotate % 360 / 90) % 2) != 0)
//             {
//                 size.x = size.x ^ size.y;
//                 size.y = size.y ^ size.x;
//                 size.x = size.x ^ size.y;
//             }
//             return LTPosition + new Vector3(gridPos.x + size.x * 0.5f, 0, gridPos.y+size.y * 0.5f)* CellSize;
//         }
//         //------------------------------------------------------
//         public static bool CanPut(Vector3 worldPos, float rotate, Vector2Int size)
//         {
//             if (ms_Instance == null) return false;
//             Vector2Int grid = WorldPosToGridPos(worldPos, rotate, size);
//             if (((rotate % 360 / 90) % 2) != 0)
//             {
//                 size.x = size.x ^ size.y;
//                 size.y = size.y ^ size.x;
//                 size.x = size.x ^ size.y;
//             }
//             if (grid.x < 0 || grid.y < 0 || grid.x >= ms_Instance.size.x || grid.y >= ms_Instance.size.y) return false;
//             Vector2Int rb = grid + size;
//             if (rb.x < 0 || rb.y < 0 || rb.x > ms_Instance.size.x || rb.y > ms_Instance.size.y) return false;
//             return true;
//         }
//         //------------------------------------------------------
//         public static Item FindConfig(int id)
//         {
//             if (ms_Instance == null) return null;
//             for(int i=0; i < ms_Instance.items.Length; ++i)
//             {
//                 if (ms_Instance.items[i].id == id) return ms_Instance.items[i];
//             }
//             return null;
//         }
//         //------------------------------------------------------
//         public static Vector2Int DiySize
//         {
//             get
//             {
//                 if (ms_Instance == null) return Vector2Int.zero;
//                 return ms_Instance.size;
//             }
//         }
//         //------------------------------------------------------
//         public static int CellSize
//         {
//             get
//             {
//                 if (ms_Instance == null) return 0;
//                 return ms_Instance.cellSize;
//             }
//         }
//         //------------------------------------------------------
//         public static float HalfCellSize
//         {
//             get
//             {
//                 if (ms_Instance == null) return 0;
//                 return ms_Instance.cellSize*0.5f;
//             }
//         }
//         //------------------------------------------------------
//         public static Vector2Int ZoomSize
//         {
//             get
//             {
//                 if (ms_Instance == null) return Vector2Int.zero;
//                 return ms_Instance.size* ms_Instance.cellSize;
//             }
//         }
//         //------------------------------------------------------
//         public static Item[] DiyItems
//         {
//             get
//             {
//                 if (ms_Instance == null) return null;
//                 return ms_Instance.items;
//             }
//         }
//         //------------------------------------------------------
//         public static void RotateBound(ref Vector2 vMin, ref Vector2 vMax, float fRotate)
//         {
//             if (Mathf.Abs(fRotate) <= 0.01f) return;
//             fRotate = fRotate * Mathf.Deg2Rad;
//             float cosF = Mathf.Cos(fRotate);
//             float sinF = Mathf.Sin(fRotate);
//             Vector2 center = new Vector2( (vMin.x+ vMax.x)*0.5f, (vMin.y + vMax.y) * 0.5f );
// 
//             Vector2 retMin = new Vector2( vMin.x - center.x, vMin.y - center.y);
//             Vector2 retMax = new Vector2( vMax.x - center.x, vMax.y - center.y);
// 
//             vMin.x = (retMin.x * cosF - retMin.y * sinF + center.x);
//             vMin.y= (retMin.x * sinF + retMin.y * cosF + center.y);
// 
//             vMax.x = (retMax.x * cosF - retMax.y * sinF + center.x);
//             vMax.y = (retMax.x * sinF + retMax.y * cosF + center.y);
//         }
//         //------------------------------------------------------
//         public static bool Intersection(Vector2Int vMin0, Vector2Int vSize0, float fRotate0, Vector2Int vMin1, Vector2Int vSize1, float fRotate1, float fFactor =0.1f)
//         {
//             Vector2 vMinTemp0 = new Vector2(vMin0.x + fFactor, vMin0.y+ fFactor);
//             Vector2 vMax0 = new Vector2(vMin0.x + vSize0.x - fFactor, vMin0.y + vSize0.y - fFactor);
// 
//             Vector2 vMinTemp1 = new Vector2(vMin1.x + fFactor, vMin1.y + fFactor);
//             Vector2 vMax1 = new Vector2(vMin1.x + vSize1.x - fFactor, vMin1.y + vSize1.y - fFactor);
// 
//             RotateBound(ref vMinTemp0, ref vMax0, fRotate0);
//             RotateBound(ref vMinTemp1, ref vMax1, fRotate1);
// 
//             return (vMax1.x > vMinTemp0.x &&
//                     vMinTemp1.x < vMax0.x &&
//                     vMax1.y > vMinTemp0.y &&
//                     vMinTemp1.y < vMax0.y);
//         }
//     }
// }
// 
