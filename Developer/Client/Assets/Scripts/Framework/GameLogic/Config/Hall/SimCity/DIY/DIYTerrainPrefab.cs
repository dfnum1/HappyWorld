/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	DIYTerrainPrefab
作    者:	HappLI
描    述:	DIYTerrainPrefab
*********************************************************************/
using UnityEngine;
using System.Collections.Generic;
using Framework.Core;
using TopGame.Core;
namespace TopGame.Logic
{
    public class DIYTerrainPrefab : AInstanceAble
    {
        [Framework.Data.DisplayNameGUI("diy地表文件"), Framework.Data.SelectFileGUI("/DatasRef/DIY/BlockTerrains", "BlockTerrains", "blocks")]
        public string diyBlockFile = "";
        public UnityEngine.AI.NavMeshData navMesh;


    }
}

