/********************************************************************
类    名:   CsvData_Scene
作    者:	自动生成
描    述:	
*********************************************************************/
using UnityEngine;
using System.Collections.Generic;
using Framework.Data;
using Framework.Core;
using Framework.Base;
namespace TopGame.Data
{
    public partial class CsvData_Scene : Data_Base
    {
		public partial class SceneData : BaseData
		{
			public	ushort				ID;
			public	string				strFile;
			public	string				strName;
			public	string				strSubScene;
			public	Vector3				PositionOffset;
			public	Vector3				LookAtOffset;
			public	Vector3				EulerAngles;
			public	Vector2				ClampAngleX;
			public	Vector2				ClampAngleY;
			public	Vector4				FollowSpeed;
			public	float				FollowDistance;
			public	float				FollowMinDistance;
			public	float				FollowMaxDistance;
			public	float				fLerp;
			public	float				FOV;
			public	uint				nTheme;

			//mapping data
		}
		Dictionary<ushort, SceneData> m_vData = new Dictionary<ushort, SceneData>();
		//-------------------------------------------
		public Dictionary<ushort, SceneData> datas
		{
			get{ return m_vData;}
		}
        //-------------------------------------------
        public CsvData_Scene()
        {
			Framework.Plugin.AT.AgentTreeManager.getInstance().RegisterClass(this);
        }
		//-------------------------------------------
		public SceneData GetData(ushort id)
		{
			SceneData outData;
			if(m_vData.TryGetValue(id, out outData))
				return outData;
			return null;
		}
        //-------------------------------------------
        public override bool LoadData(string strContext,CsvParser csv = null)
        {
			if(csv == null) csv = new CsvParser();
			if(!csv.LoadTableString(strContext))
				return false;
			
			ClearData();
			
			int i = csv.GetTitleLine();
			if(i < 0) return false;
			
			int nLineCnt = csv.GetLineCount();
			for(i++; i < nLineCnt; i++)
			{
				if(!csv[i]["ID"].IsValid()) continue;
				
				SceneData data = new SceneData();
				
				data.ID = csv[i]["ID"].Ushort();
				data.strFile = csv[i]["strFile"].String();
				data.strName = csv[i]["strName"].String();
				data.strSubScene = csv[i]["strSubScene"].String();
				data.PositionOffset = csv[i]["PositionOffset"].Vec3();
				data.LookAtOffset = csv[i]["LookAtOffset"].Vec3();
				data.EulerAngles = csv[i]["EulerAngles"].Vec3();
				data.ClampAngleX = csv[i]["ClampAngleX"].Vec2();
				data.ClampAngleY = csv[i]["ClampAngleY"].Vec2();
				data.FollowSpeed = csv[i]["FollowSpeed"].Vec4();
				data.FollowDistance = csv[i]["FollowDistance"].Float();
				data.FollowMinDistance = csv[i]["FollowMinDistance"].Float();
				data.FollowMaxDistance = csv[i]["FollowMaxDistance"].Float();
				data.fLerp = csv[i]["fLerp"].Float();
				data.FOV = csv[i]["FOV"].Float();
				data.nTheme = csv[i]["nTheme"].Uint();
				
				m_vData.Add(data.ID, data);
				OnAddData(data);
			}
			OnLoadCompleted();
            return true;
        }
        //-------------------------------------------
        public override void ClearData()
        {
			m_vData.Clear();
			base.ClearData();
        }
    }
}
