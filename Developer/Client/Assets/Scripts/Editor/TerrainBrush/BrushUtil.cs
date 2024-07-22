#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace TopGame.Core.Brush
{
    public class BrushUtil
    {
        //------------------------------------------------------
        public static void ExportMeshData(Matrix4x4 world, Mesh mesh)
        {
            if (mesh == null || string.IsNullOrEmpty(mesh.name)) return;
            string root = Application.dataPath + "/Scripts/MainScripts/GameCore/Terrain/Brush/Data/";
            if (System.IO.Directory.Exists(root))
                System.IO.Directory.CreateDirectory(root);

            string strName = mesh.name;
            strName = (strName[0]).ToString().ToUpper() + strName.Substring(1);
            string file = Application.dataPath + "/Scripts/MainScripts/GameCore/Terrain/Brush/Data/" + strName + "BrushData.cs";
            if (System.IO.File.Exists(file))
            {
                System.IO.File.Delete(file);
            }

            string strCode = "";
            strCode += "using UnityEngine;\r\n";
            strCode += "namespace TopGame.Core.Brush\r\n";
            strCode += "{\r\n";
            strCode += "\t[Brush(\"" + strName + "\")]\r\n";
            strCode += "\tpublic class " + strName + "BrushData\r\n";
            strCode += "\t{\r\n";
            strCode += "\t\t[BrushName(\"" + strName + "\")]\r\n";
            strCode += "\t\tpublic static FoliageBuffer " + strName + " = new FoliageBuffer()\r\n";
            strCode += "\t\t{\r\n";
            //! verteices
            strCode += "\t\t\tvertices = new Vector3[]\r\n";
            strCode += "\t\t\t{\r\n";
            for (int i =0; i < mesh.vertices.Length; ++i)
            {
                Vector3 pos = world.MultiplyPoint(mesh.vertices[i]);
                strCode += "\t\t\t\tnew Vector3(" + string.Format("{0}f,{1}f,{2}f", pos.x, pos.y, pos.z) + ")";
                if (i < mesh.vertices.Length - 1)
                    strCode += ",\r\n";
            }
            strCode += "\r\n\t\t\t},\r\n";

            //! uv
            strCode += "\t\t\tuvs = new Vector2[]\r\n";
            strCode += "\t\t\t{\r\n";
            for (int i = 0; i < mesh.uv.Length; ++i)
            {
                strCode += "\t\t\t\tnew Vector2(" + string.Format("{0}f,{1}f", mesh.uv[i].x, mesh.uv[i].y) + ")";
                if (i < mesh.uv.Length - 1)
                    strCode += ",\r\n";
            }
            strCode += "\r\n\t\t\t},\r\n";

            //! triangles
            strCode += "\t\t\ttriangles = new int[] {";
            for (int i = 0; i < mesh.triangles.Length; ++i)
            {
                strCode +=  mesh.triangles[i].ToString();
                if (i < mesh.triangles.Length - 1)
                    strCode += ",";
            }
            strCode += "}\r\n";

            strCode += "\t\t};\r\n";

            strCode += "\t}\r\n";
            strCode += "}\r\n";


            FileStream fs = new FileStream(file, FileMode.OpenOrCreate);
            StreamWriter writer = new StreamWriter(fs, System.Text.Encoding.UTF8);
            writer.Write(strCode);
            writer.Close();
        }
        //------------------------------------------------------
        public static void FindAllBrush(List<EditBrsuhData> vBrush)
        {
            if (vBrush == null) vBrush = new List<EditBrsuhData>();
            vBrush.Clear();

            string[] assets = AssetDatabase.FindAssets("t:TerrainFoliageDatas");
            for(int i =0; i < assets.Length; ++i)
            {
                TerrainFoliageDatas terrainData = AssetDatabase.LoadAssetAtPath<TerrainFoliageDatas>(AssetDatabase.GUIDToAssetPath(assets[i]));
                if (terrainData == null) continue;
                terrainData.Refresh();
                foreach (var db in terrainData.GetBrushs())
                {
                    EditBrsuhData br = new EditBrsuhData();
                    br.brush = db.Value;
                    br.useMatrial = terrainData.FindMaterial(db.Value.type);
                    br.icon = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Scripts/Editor/TerrainBrush/Data/icons/" + db.Value.name + ".png");
                    if (br.brush is BrushRes)
                    {
                        br.icon = AssetDatabase.GetCachedIcon((br.brush as BrushRes).strFile);
                    }
                    if (br.icon == null) br.icon = br.useMatrial.mainTexture;
                    br.uv_tile_offset = new Rect(br.brush.GetUvOffset().x + br.useMatrial.mainTextureOffset.x, br.brush.GetUvOffset().y + br.useMatrial.mainTextureOffset.y, br.useMatrial.mainTextureScale.x, br.useMatrial.mainTextureScale.y);
                    vBrush.Add(br);
                }
            }

        }
    }
}
#endif