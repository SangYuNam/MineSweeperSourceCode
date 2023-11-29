using Gpm.Ui;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

namespace WR.StudyProject.System
{
    public class StudyItemData : InfiniteScrollData
    {
        public int index = 0;
        public string description = string.Empty;
    }

    public class StudyItem : InfiniteScrollItem
    {
        [SerializeField] private Text m_text = null;

        public override void UpdateData(InfiniteScrollData scrollData)
        {
            if(scrollData == null)
            {
                Debug.Log("scrollData is null");
                return;
            }
            StringBuilder sb = new StringBuilder();
            
            base.UpdateData(scrollData);
            StudyItemData itemData = (StudyItemData)scrollData;
            sb.Append(string.Format("Item : {0} ", itemData.index));
            sb.Append(itemData.description);
            m_text.text = sb.ToString();
        }

        public void ClickButton()
        {
            OnSelect();
        }
    }
}