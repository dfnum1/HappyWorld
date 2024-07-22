using System.Collections;
using System.Collections.Generic;
using TopGame.Data;
using UnityEngine;
using UnityEngine.UI;

namespace TopGame.UI
{
    [UI((ushort)EUIType.GMPanel, UI.EUIAttr.Logic,true)]
    public class GMChapterLogic : UILogic
    {
        ListView m_pList = null;
        //------------------------------------------------------
        public override void OnShow()
        {
            base.OnShow();
            RefreshUI();
        }
        //------------------------------------------------------
        public override void OnHide()
        {
            base.OnHide();
            m_pList = null;
            if (m_pList)
            {
                m_pList.onItemRender.RemoveAllListeners();
            }
        }
        //------------------------------------------------------
        void RefreshUI()
        {
            m_pList = GetWidget<ListView>("ChapterList");
            if (m_pList == null) return;
//             if (m_vChapterDatas == null) m_vChapterDatas = new List<CsvData_Chapter.ChapterData>();
//             foreach(var db in Data.DataManager.getInstance().Chapter.datas)
//             {
//                 m_vChapterDatas.Add(db.Value);
//             }
            m_pList.onItemRender.RemoveAllListeners();
            m_pList.onItemRender.AddListener(OnRenderList);
        //    m_pList.numItems = (uint)m_vChapterDatas.Count;
        }
        //------------------------------------------------------
        void OnRenderList(int index,UISerialized item)
        {
//             if (index <= 0 || index >= m_vChapterDatas.Count) return;
//             var data = m_vChapterDatas[index];
//             EventTriggerListener listener = item.GetWidget<EventTriggerListener>("Bg");
//             if (listener != null)
//             {
//                 listener.param1 = data;
//                 listener.onClick = (g, obj) =>
//                 {
//       //              SvrData.UserManager.MySelf.ProxyDB<SvrData.BattleDB>().SetLevelFile(data.map);
//       //              GameInstance.getInstance().ChangeLocation(SvrData.ELocationState.Run, Base.ELoadingType.Loading);
//                 };
//             }
//             UIUtil.SetLabel(item.GetWidget<Text>("IDText"), data.ID.ToString());
//             UIUtil.SetLabel(item.GetWidget<Text>("StageDes"), data.map);
        }
    }
}