using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MineSweeper.Framework.game;
using MineSweeper.Framework.System;

namespace MineSweeper.Framework.UI
{
    public class MenuPresenter : PresenterBase
    {
        #region 필드 변수

        [Header("View 모음")]
        [SerializeField] private DifficultyView m_difficultyView;
        [SerializeField] private ClearView m_clearView;
        [SerializeField] private DefeatView m_defeatView;
        [SerializeField] private PlayView m_playView;

        public float m_time = 0;

        #endregion

        #region 유니티 고유 이벤트

        private void Start()
        {
            StartCoroutine(CheckingTime());
        }

        private void Update()
        {
            // 실시간 시간 변화 (int 형)
            m_playView.TimeText.text = $"Time : {(int)m_time}s";
        }

        #endregion

        #region 함수

        // 게임이 끝났을때 시간 Text 변화
        public void SetTimeText()
        {
            switch (GameManager.Instance.GameState)
            {
                case EGameState.Clear:
                    m_clearView.TimeText.text = $"TIME : {m_time}s";
                    break;
                case EGameState.Defeat:
                    m_defeatView.TimeText.text = $"TIME : {m_time}s";
                    break;
                default:
                    break;
            }
        }

        // 시간초를 세어 주는 코루틴 함수
        public IEnumerator CheckingTime()
        {
            while (true)
            {
                yield return new WaitForSeconds(1.0f);
                m_time++;
            }
        }

        // 게임이 끝나고 Again 버튼을 눌렀을 시 발동되는 함수
        private void ChooseAgain()
        {
            if (m_clearView.gameObject.activeSelf)
            {
                m_clearView.gameObject.SetActive(false);
            }
            if (m_defeatView.gameObject.activeSelf)
            {
                m_defeatView.gameObject.SetActive(false);
            }
            if (m_playView.gameObject.activeSelf)
            {
                m_playView.gameObject.SetActive(false);
            }
            GameManager.Instance.ChangeState(EGameState.Lobby);
            GameManager.Instance.ChangeState(EGameState.Play);
            StartCoroutine(CheckingTime());
            m_time = 0;


            SoundManager.Instance?.Play("UI", ESound.Effect);
        }

        // 게임이 끝나고 Menu 버튼을 눌렀을 시 발동되는 함수
        private void ChooseMenu()
        {
            GameManager.Instance.ChangeState(EGameState.Lobby);
            if (m_clearView.gameObject.activeSelf)
            {
                m_clearView.gameObject.SetActive(false);
            }
            if (m_defeatView.gameObject.activeSelf)
            {
                m_defeatView.gameObject.SetActive(false);
            }
            if (m_playView.gameObject.activeSelf)
            {
                m_playView.gameObject.SetActive(false);
            }
            m_difficultyView.gameObject.SetActive(true);
            StartCoroutine(CheckingTime());


            SoundManager.Instance?.Play("UI", ESound.Effect);
        }

        #endregion

        #region 재정의 함수

        public override void Initialize()
        {
            base.Initialize();

            // 버튼에 함수 할당
            m_clearView.PlayAgainButton.onClick.AddListener(ChooseAgain);
            m_clearView.GoToMenuButton.onClick.AddListener(ChooseMenu);
            m_defeatView.PlayAgainButton.onClick.AddListener(ChooseAgain);
            m_defeatView.GoToMenuButton.onClick.AddListener(ChooseMenu);

            // 게임매니저에 뷰 인스턴스와 자신의 인스턴스 전달
            GameManager.Instance.m_PlayView = m_playView;
            GameManager.Instance.m_ClearView = m_clearView;
            GameManager.Instance.m_DefeatView = m_defeatView;
            GameManager.Instance.m_MenuPresenter = this;
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