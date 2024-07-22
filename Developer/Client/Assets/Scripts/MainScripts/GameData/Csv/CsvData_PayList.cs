/********************************************************************
生成日期:	25:7:2023   11:13
类    名: 	CsvData_PayList
作    者:	Ywm
描    述:	CsvData_PayList
*********************************************************************/

using System.Collections.Generic;
using Framework.Data;
using Framework.Plugin.AT;

namespace TopGame.Data
{
    public partial class CsvData_PayList : Data_Base
    {
        //------------------------------------------------------
        static Dictionary<string, Dictionary<uint,PayListData>> m_vPayLists = new Dictionary<string, Dictionary<uint,PayListData>>();
        //------------------------------------------------------
        protected override void OnAddData(IUserData baseData)
        {
            base.OnAddData(baseData);

            var data = baseData as PayListData;
            if (data == null)
            {
                return;
            }

            var channel = "";
            if (data.type != "")
                channel = data.channel + "_" + data.type;
            else
                channel = data.channel;
            Dictionary<uint, PayListData> dic;
            if (!m_vPayLists.TryGetValue(channel, out dic))
            {
                dic = new Dictionary<uint, PayListData>();
                m_vPayLists.Add(channel, dic);
            }
            dic.Add(data.payid, data);
        }
        //------------------------------------------------------
        public PayListData GetPayListByChannel(string channel, uint pos)
        {
            Dictionary<uint, PayListData> dic;
            if (m_vPayLists.TryGetValue(channel, out dic))
            {
                PayListData outData;
                if (dic.TryGetValue(pos, out outData))
                {
                    return outData;
                }
            }
            //找不到时，使用默认渠道商品
            if (m_vPayLists.TryGetValue("default", out dic))
            {
                PayListData outData;
                if (dic.TryGetValue(pos, out outData))
                {
                    return outData;
                }
            }
            return null;
        }
        //------------------------------------------------------
        public Dictionary<uint, PayListData> GetPayListByChannel(string channel)
        {
            Dictionary<uint, PayListData> dic;
            if (m_vPayLists.TryGetValue(channel, out dic) && dic.Count>0)
            {
                return dic;
            }
            //找不到时，使用默认渠道商品
            if (m_vPayLists.TryGetValue("default", out dic))
            {
                return dic;
            }
            return null;
        }		
        //------------------------------------------------------
        protected override void OnClearData()
        {
            base.OnClearData();
            m_vPayLists?.Clear();
        }
    }
}