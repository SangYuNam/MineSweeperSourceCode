using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MineSweeper.Framework.UI
{
    public class ClearView : ViewBase
    {
        #region 필드 변수

        [Header("Button 모음")]
        [SerializeField] private Button m_playAgainButton;
        [SerializeField] private Button m_goToMenuButton;

        [Header("Text 모음")]
        [SerializeField] private TextMeshProUGUI m_timeText;

        public Button PlayAgainButton
        {
            get => m_playAgainButton;
        }
        public Button GoToMenuButton
        {
            get => m_goToMenuButton;
        }

        public TextMeshProUGUI TimeText
        {
            get => m_timeText;
        }

        #endregion

        #region 유니티 고유 이벤트


        #endregion

        #region 함수


        #endregion

        #region 재정의 함수

        public override void Open()
        {
            base.Open();
        }

        public override void Close()
        {
            base.Close();
        }

        #endregion

        #region 인터페이스 구현


        #endregion

    }
}