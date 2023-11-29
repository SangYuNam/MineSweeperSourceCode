using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MineSweeper.Framework.UI
{
    public class TitleView : ViewBase
    {
        #region 필드 변수

        public Button StartButton
        {
            get => m_startButton;
        }
        [SerializeField] private Button m_startButton;

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

