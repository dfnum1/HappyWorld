/********************************************************************
类    名:   CsvData_SystemConfig
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
    public partial class CsvData_SystemConfig : Data_Base
    {
		public partial class SystemConfigData : BaseData
		{
			public	byte				id;
			public	uint[]				initialHero;
			public	uint[]				initialTeam;
			public	uint[]				initialItem;
			public	uint[]				initialItemValue;
			public	uint				distancePoint;
			public	byte				rebornTimes;
			public	uint				rebornCost;
			public	uint				costNum;
			public	uint				diamondRebornCost;
			public	uint				rebornHPRecover;
			public	uint				rebornCountdown;
			public	byte				adRebornTimes;
			public	int				runSkillTime;
			public	int				runPetTime;
			public	int				runPetLimit;
			public	uint				shopRefreshTime;
			public	uint				goldShopRefreshTime;
			public	uint				bagItemLimit;
			public	uint				fastHangUpNum;
			public	uint				videoHangUpNum;
			public	uint				fastHangUpCost;
			public	uint				fastHangUpRewardTime;
			public	uint				hangUpTimeMax;
			public	uint				hangUpTimeMin;
			public	uint				hangUpItemInterval;
			public	uint				hangUpTipsTime;
			public	uint[]				hangUpGoldShow;
			public	uint				hangUpDefaultTime;
			public	uint				actionPointLimit;
			public	uint				actionPointRecover;
			public	uint				actionPointBuyLimit;
			public	uint				actionPointGetNum;
			public	uint[]				actionPointCost;
			public	uint				actionPointAdLimit;
			public	uint				actionPointAdGetNum;
			public	uint				petHatchPosionLimit1;
			public	uint				petHatchPosionLimit2;
			public	uint				petHatchPosionLimit3;
			public	uint				petHatchSpeedUpCost;
			public	uint[]				commonEquip;
			public	uint[]				defaultPet;
			public	uint				seasonDay;
			public	uint[]				goldMultipleID;
			public	uint				OpenSystemUnock;
			public	uint				OpenAD;
			public	uint				changeNameCost;
			public	uint[]				specialCard;
			public	uint[]				proReduce;
			public	uint				mailTimeLimit;
			public	uint				cardConfigLimit;

			//mapping data
		}
		Dictionary<byte, SystemConfigData> m_vData = new Dictionary<byte, SystemConfigData>();
		//-------------------------------------------
		public Dictionary<byte, SystemConfigData> datas
		{
			get{ return m_vData;}
		}
        //-------------------------------------------
        public CsvData_SystemConfig()
        {
			Framework.Plugin.AT.AgentTreeManager.getInstance().RegisterClass(this);
        }
		//-------------------------------------------
		public SystemConfigData GetData(byte id)
		{
			SystemConfigData outData;
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
				if(!csv[i]["id"].IsValid()) continue;
				
				SystemConfigData data = new SystemConfigData();
				
				data.id = csv[i]["id"].Byte();
				data.initialHero = csv[i]["initialHero"].UintArray();
				data.initialTeam = csv[i]["initialTeam"].UintArray();
				data.initialItem = csv[i]["initialItem"].UintArray();
				data.initialItemValue = csv[i]["initialItemValue"].UintArray();
				data.distancePoint = csv[i]["distancePoint"].Uint();
				data.rebornTimes = csv[i]["rebornTimes"].Byte();
				data.rebornCost = csv[i]["rebornCost"].Uint();
				data.costNum = csv[i]["costNum"].Uint();
				data.diamondRebornCost = csv[i]["diamondRebornCost"].Uint();
				data.rebornHPRecover = csv[i]["rebornHPRecover"].Uint();
				data.rebornCountdown = csv[i]["rebornCountdown"].Uint();
				data.adRebornTimes = csv[i]["adRebornTimes"].Byte();
				data.runSkillTime = csv[i]["runSkillTime"].Int();
				data.runPetTime = csv[i]["runPetTime"].Int();
				data.runPetLimit = csv[i]["runPetLimit"].Int();
				data.shopRefreshTime = csv[i]["shopRefreshTime"].Uint();
				data.goldShopRefreshTime = csv[i]["goldShopRefreshTime"].Uint();
				data.bagItemLimit = csv[i]["bagItemLimit"].Uint();
				data.fastHangUpNum = csv[i]["fastHangUpNum"].Uint();
				data.videoHangUpNum = csv[i]["videoHangUpNum"].Uint();
				data.fastHangUpCost = csv[i]["fastHangUpCost"].Uint();
				data.fastHangUpRewardTime = csv[i]["fastHangUpRewardTime"].Uint();
				data.hangUpTimeMax = csv[i]["hangUpTimeMax"].Uint();
				data.hangUpTimeMin = csv[i]["hangUpTimeMin"].Uint();
				data.hangUpItemInterval = csv[i]["hangUpItemInterval"].Uint();
				data.hangUpTipsTime = csv[i]["hangUpTipsTime"].Uint();
				data.hangUpGoldShow = csv[i]["hangUpGoldShow"].UintArray();
				data.hangUpDefaultTime = csv[i]["hangUpDefaultTime"].Uint();
				data.actionPointLimit = csv[i]["actionPointLimit"].Uint();
				data.actionPointRecover = csv[i]["actionPointRecover"].Uint();
				data.actionPointBuyLimit = csv[i]["actionPointBuyLimit"].Uint();
				data.actionPointGetNum = csv[i]["actionPointGetNum"].Uint();
				data.actionPointCost = csv[i]["actionPointCost"].UintArray();
				data.actionPointAdLimit = csv[i]["actionPointAdLimit"].Uint();
				data.actionPointAdGetNum = csv[i]["actionPointAdGetNum"].Uint();
				data.petHatchPosionLimit1 = csv[i]["petHatchPosionLimit1"].Uint();
				data.petHatchPosionLimit2 = csv[i]["petHatchPosionLimit2"].Uint();
				data.petHatchPosionLimit3 = csv[i]["petHatchPosionLimit3"].Uint();
				data.petHatchSpeedUpCost = csv[i]["petHatchSpeedUpCost"].Uint();
				data.commonEquip = csv[i]["commonEquip"].UintArray();
				data.defaultPet = csv[i]["defaultPet"].UintArray();
				data.seasonDay = csv[i]["seasonDay"].Uint();
				data.goldMultipleID = csv[i]["goldMultipleID"].UintArray();
				data.OpenSystemUnock = csv[i]["OpenSystemUnock"].Uint();
				data.OpenAD = csv[i]["OpenAD"].Uint();
				data.changeNameCost = csv[i]["changeNameCost"].Uint();
				data.specialCard = csv[i]["specialCard"].UintArray();
				data.proReduce = csv[i]["proReduce"].UintArray();
				data.mailTimeLimit = csv[i]["mailTimeLimit"].Uint();
				data.cardConfigLimit = csv[i]["cardConfigLimit"].Uint();
				
				m_vData.Add(data.id, data);
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
