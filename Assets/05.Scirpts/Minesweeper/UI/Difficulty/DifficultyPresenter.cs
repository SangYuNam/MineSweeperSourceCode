using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MineSweeper.Framework.game;
using MineSweeper.Framework.System;

namespace MineSweeper.Framework.UI
{
    public class DifficultyPresenter : PresenterBase
    {
        #region 필드 변수
        [SerializeField] private DifficultyView m_difficultyView;
        [SerializeField] MenuPresenter m_menuPresenter;
        #endregion


        #region 유니티 고유 이벤트


        #endregion


        #region 함수

        // Beginner 버튼을 눌렀을때
        private void ChooseBeginner()
        {
            if (m_difficultyView == null)
            {
                Debug.Log("m_difficultyMenuview is Null");
                return;
            }

            SoundManager.Instance?.Play("UI", ESound.Effect);

            GameManager.Instance.FadeInOut(1.5f, () =>
            {
                GameManager.Instance.GameDifficulty = EGameDifficulty.Beginner;
                GameManager.Instance.ChangeState(EGameState.Play);
                m_difficultyView.gameObject.SetActive(false);
                m_menuPresenter.m_time = 0;
            });
        }

        // Intermediate 버튼을 눌렀을때
        private void ChooseIntermediate()
        {
            if (m_difficultyView == null)
            {
                Debug.Log("m_difficultyMenuview is Null");
                return;
            }

            SoundManager.Instance?.Play("UI", ESound.Effect);

            GameManager.Instance.FadeInOut(1.5f, () =>
            {
                GameManager.Instance.GameDifficulty = EGameDifficulty.Intermediate;
                GameManager.Instance.ChangeState(EGameState.Play);
                m_difficultyView.gameObject.SetActive(false);
                m_menuPresenter.m_time = 0;
            });
        }

        // Expert 버튼을 눌렀을때
        private void ChooseExpert()
        {
            if (m_difficultyView == null)
            {
                Debug.Log("m_difficultyMenuview is Null");
                return;
            }

            SoundManager.Instance?.Play("UI", ESound.Effect);

            GameManager.Instance.FadeInOut(1.5f, () =>
            {
                GameManager.Instance.GameDifficulty = EGameDifficulty.Expert;
                GameManager.Instance.ChangeState(EGameState.Play);
                m_difficultyView.gameObject.SetActive(false);
                m_menuPresenter.m_time = 0;
            });
        }

        #endregion


        #region 재정의 함수

        public override void Initialize()
        {
            base.Initialize();

            // 버튼에 함수 할당
            m_difficultyView.BeginnerButton.onClick.AddListener(ChooseBeginner);
            m_difficultyView.IntermediateButton.onClick.AddListener(ChooseIntermediate);
            m_difficultyView.ExpertButton.onClick.AddListener(ChooseExpert);
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
