using Gpm.Ui;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


namespace WR.StudyProject.System
{
    public class StudyInfinityScroll : MonoBehaviour
    {
        [Header("ScrollView UI")]
        [SerializeField] private InfiniteScroll m_scrollView = null;
        [Header("Log UI")]
        [SerializeField] private Text m_logText = null;

        private int m_index = 0;
        private List<StudyItemData> DataLists = new List<StudyItemData>();
        private StringBuilder m_log = new StringBuilder();

        void Start()
        {
            m_scrollView.AddSelectCallback((data) =>
            {
                AddLog(string.Format("vertical select data : {0}", ((StudyItemData)data).index.ToString()));
            });
        }

        private void AddLog(string text)
        {
            if(text == null)
            {
                Debug.Log("text is null");
                return;
            }
            m_log.AppendLine(text);
            m_logText.text = m_log.ToString();
        }

        public void InsertData()
        {
            StudyItemData data = new StudyItemData();

            data.index = m_index++;
            DataLists.Add(data);
            m_scrollView.InsertData(data);

            AddLog(string.Format("Insert Data : {0}", m_index - 1));
        }
    }
}
