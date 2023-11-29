using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MineSweeper.Framework.System;

namespace MineSweeper.Framework.UI
{
    public class TitlePresenter : PresenterBase
    {
        #region 필드 변수

        [SerializeField] private TitleView m_titleView;
        [SerializeField] private DifficultyPresenter m_difficultyMenuPresenter;

        #endregion


        #region 유니티 고유 이벤트


        #endregion


        #region 함수

        private void MoveLobbyScene()
        {
            GameManager.Instance.ChangeState(EGameState.Lobby);
            m_titleView.gameObject.SetActive(false);
            if(m_difficultyMenuPresenter != null && !m_difficultyMenuPresenter.gameObject.activeSelf) 
            {
                m_difficultyMenuPresenter.gameObject.SetActive(true);

                SoundManager.Instance?.Play("Start", ESound.Effect);
            }
            else
            {
                Debug.Log("m_difficultyMenuPresenter is Null or activeSelf is True");
            }
        }

        #endregion


        #region 재정의 함수

        public override void Initialize()
        {
            base.Initialize();

            m_titleView.StartButton.onClick.AddListener(MoveLobbyScene);
        }

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

