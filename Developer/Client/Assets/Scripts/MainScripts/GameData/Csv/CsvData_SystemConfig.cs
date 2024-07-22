/********************************************************************
类    名:   CsvData_Dungons
作    者:	
描    述:	
*********************************************************************/
using UnityEngine;
using System.Collections.Generic;
using Framework.Data;
namespace TopGame.Data
{
    public partial class CsvData_SystemConfig : Data_Base
    {
        protected static SystemConfigData ms_SysConfig = null;

        public static SystemConfigData sysConfig
        {
            get
            {
                if(ms_SysConfig == null && DataManager.getInstance()!=null && DataManager.getInstance().SystemConfig!=null)
                {
                    ms_SysConfig = DataManager.getInstance().SystemConfig.GetData(1);
                }
                return ms_SysConfig;
            }
        }
    }
}
