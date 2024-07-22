// /********************************************************************
// 生成日期:	1:11:2020 10:06
// 类    名: 	GameInstance
// 作    者:	HappLI
// 描    述:	游戏主线程
// *********************************************************************/
// 
// using System.Collections.Generic;
// using TopGame.Base;
// using TopGame.Core;
// using TopGame.Data;
// using TopGame.Logic;
// using Framework.Module;
// using UnityEngine;
// using TopGame.Net;
// using System;
// using Framework.Plugin.AT;
// using Framework.Plugin.AI;
// using Framework.Plugin.Guide;
// using TopGame.UI;
// using TopGame.SvrData;
// using Framework.Core;
// 
// namespace TopGame
// {
//     public partial class GameInstance
//     {
//         //-------------------------------------------------
//         public void OnTouchBegin(ATouchInput.TouchData touh)
//         {
//             if (IsTouchLock()) return;
//             ModuleManager.getInstance().OnTouchBegin(touh);
//             GameDelegate.GameTouchBegin(touh.touchID, touh.position);
//         }
//         //-------------------------------------------------
//         public void OnTouchMove(ATouchInput.TouchData touh)
//         {
//             if (IsTouchLock()) return;
//             ModuleManager.getInstance().OnTouchMove(touh);
//             GameDelegate.GameOnTouchMove(touh.touchID, touh.position);
//         }
//         //-------------------------------------------------
//         public void OnTouchWheel(float wheel, Vector2 mouse)
//         {
//             if (IsTouchLock()) return;
//             ModuleManager.getInstance().OnTouchWheel(wheel, mouse);
//             GameDelegate.GameOnMouseWheel(wheel);
//         }
//         //-------------------------------------------------
//         public void OnTouchEnd(ATouchInput.TouchData touh)
//         {
//             if (IsTouchLock()) return;
//             ModuleManager.getInstance().OnTouchEnd(touh);
//             GameDelegate.GameOnTouchEnd(touh.touchID, touh.position);
//         }
//         //-------------------------------------------------
//         public void OnKeyDown(KeyCode code)
//         {
//             if (IsTouchLock()) return;
//             ModuleManager.getInstance().OnKeyDown(code);
//         }
//         //-------------------------------------------------
//         public void OnKeyUp(KeyCode code)
//         {
//             if (IsTouchLock()) return;
//             ModuleManager.getInstance().OnKeyUp(code);
//         }
//     }
// }
