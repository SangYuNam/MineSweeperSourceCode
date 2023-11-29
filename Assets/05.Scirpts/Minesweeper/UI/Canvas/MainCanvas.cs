using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using MineSweeper.Framework.System;

namespace MineSweeper.Framework.UI
{
    public class MainCanvas : UICanvasBase
    {
        #region 필드 변수

        [SerializeField] private TitlePresenter m_titlePresenter;
        [SerializeField] private DifficultyPresenter m_difficultyPresenter;
        [SerializeField] private MenuPresenter m_menuPresenter;
        [SerializeField] private FadePresenter m_fadePresenter;


        #endregion

        #region 유니티 고유 이벤트

        private void Start()
        {
            m_titlePresenter.Initialize();
            m_difficultyPresenter.Initialize();
            m_menuPresenter.Initialize();
            m_fadePresenter.Initialize();

            GameManager.Instance.m_FadePresenter = m_fadePresenter;
        }

        #endregion

        #region 함수


        #endregion

        #region 재정의 함수


        #endregion

        #region 인터페이스 구현


        #endregion

    }
}
