using UnityEngine;
namespace TopGame.Core.Brush
{
	[Brush("GrassData")]
	public class GrassData
	{
		[BrushName("※草", "brush_grass_icon")]
		public static FoliageBuffer ZhujiemianUI_zhiwu001 = new FoliageBuffer()
		{
            vertices = new Vector3[]
            {
                new Vector3(-0.4956131f,1.119251f,0.006811486f),
                new Vector3(0.4956207f,1.119251f,-0.1601583f),
                new Vector3(0.4956207f,0.564903f,-0.1601582f),
                new Vector3(-0.4956169f,0.5649036f,0.006818228f),
                new Vector3(-0.4956207f,-5.863526E-05f,0.006824494f),
                new Vector3(0.4956207f,-5.867007E-05f,-0.1601582f),
                new Vector3(0.4477654f,1.330237f,0.4083322f),
                new Vector3(-0.2145596f,1.340268f,-0.592773f),
                new Vector3(-0.2460289f,0.6791664f,-0.578582f),
                new Vector3(0.4163074f,0.6691352f,0.4225254f),
                new Vector3(0.3848457f,-0.0001671688f,0.4367178f),
                new Vector3(-0.2774925f,0.0002645476f,-0.5643921f),
                new Vector3(0.1773987f,1.336153f,-0.5792263f),
                new Vector3(-0.08226204f,1.336153f,0.5927739f),
                new Vector3(-0.08226204f,0.6741509f,0.5927739f),
                new Vector3(0.1773987f,0.6741509f,-0.5792271f),
                new Vector3(0.1773987f,9.899829E-06f,-0.5792277f),
                new Vector3(-0.08226204f,9.965036E-06f,0.592774f)
            },
            uvs = new Vector2[]
            {
                new Vector2(0.2579424f,0.4146466f),
                new Vector2(0.6606281f,0.4146466f),
                new Vector2(0.6606281f,0.2133036f),
                new Vector2(0.2579424f,0.2133036f),
                new Vector2(0.2579424f,0.01196064f),
                new Vector2(0.6606281f,0.01196064f),
                new Vector2(0.2579424f,0.4146466f),
                new Vector2(0.6606281f,0.4146466f),
                new Vector2(0.6606281f,0.2133036f),
                new Vector2(0.2579424f,0.2133036f),
                new Vector2(0.2579424f,0.01196064f),
                new Vector2(0.6606281f,0.01196064f),
                new Vector2(0.2579424f,0.4146466f),
                new Vector2(0.6606281f,0.4146466f),
                new Vector2(0.6606281f,0.2133036f),
                new Vector2(0.2579424f,0.2133036f),
                new Vector2(0.2579424f,0.01196064f),
                new Vector2(0.6606281f,0.01196064f)
            },
            triangles = new int[] { 0, 1, 2, 0, 2, 3, 4, 3, 2, 4, 2, 5, 6, 7, 8, 6, 8, 9, 10, 9, 8, 10, 8, 11, 12, 13, 14, 12, 14, 15, 16, 15, 14, 16, 14, 17 }
        };
	}
}
