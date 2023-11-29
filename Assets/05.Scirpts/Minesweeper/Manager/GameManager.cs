using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MineSweeper.Framework.game;
using MineSweeper.Framework.System;
using MineSweeper.Framework.UI;
using System;

namespace MineSweeper.Framework.System
{    
    public enum EGameState
    {
        Create = 0, // 최초 화면
        Lobby, // 메인 화면
        Play, // 플레이 화면
        Clear, // 클리어 화면
        Defeat, // 실패 화면
    }

    public enum EGameDifficulty
    {
        None = 0, // 최초 초기화 난이도
        Beginner, // 초급
        Intermediate, // 중급
        Expert // 고급
    }

    public class GameManager : Singleton<GameManager>
    {
        #region 필드 변수

        // State 선언 및 초기화
        public EGameState GameState { get; private set; }
        public EGameDifficulty GameDifficulty = EGameDifficulty.None;

        // 오브젝트 참조
        public PlayView m_PlayView;
        public ClearView m_ClearView;
        public DefeatView m_DefeatView;
        public MenuPresenter m_MenuPresenter = null;
        public game.MineSweeper m_MineSweeper = null;
        public FadePresenter m_FadePresenter = null;
        private SceneChanger m_sceneChanger = null;

        #endregion


        #region 유니티 고유 이벤트

        private void Update()
        {

            // ESC키를 누를 시 게임 종료
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }

        }

        #endregion


        #region 함수

        // 게임 흐름을 관장 할 ChangeState 함수
        public void ChangeState(EGameState state)
        {
            if (GameState == state)
            {
                return;
            }
            switch (state)
            {
                case EGameState.Create:
                    GameState = EGameState.Create;
                    m_sceneChanger.ChangeScene(ESceneType.Intro);
                    Debug.Log("Now State is Create");                    
                    break;
                case EGameState.Lobby:
                    GameState = EGameState.Lobby;
                    Debug.Log("Now State is Lobby");
                    m_sceneChanger.ChangeScene(ESceneType.Lobby);
                    break;
                case EGameState.Play:
                    GameState = EGameState.Play;
                    m_sceneChanger.ChangeScene(ESceneType.Play);
                    m_PlayView.gameObject.SetActive(true);
                    Debug.Log("Now State is Play");                    
                    break;
                case EGameState.Clear:
                    GameState = EGameState.Clear;
                    m_ClearView.gameObject.SetActive(true);
                    m_MenuPresenter.SetTimeText();
                    m_MenuPresenter.StopAllCoroutines();
                    m_MineSweeper.AllClear();
                    Debug.Log("Now State is Clear");


                    SoundManager.Instance?.Play("Win", ESound.Effect);
                    break;
                case EGameState.Defeat:
                    GameState = EGameState.Defeat;
                    m_DefeatView.gameObject.SetActive(true);
                    m_MenuPresenter.SetTimeText();
                    m_MenuPresenter.StopAllCoroutines();
                    Debug.Log("Now State is Deafeat");

                    SoundManager.Instance?.Play("Lose", ESound.Effect);
                    break;
                default:
                    Debug.Log("Doesn't Exist State");
                    break;
            }
        }

        // Fade In
        public void FadeIn(float fadeOutTime, Action callback = null)
        {
            m_FadePresenter?.FadeIn(fadeOutTime, callback);
        }

        // Fade In
        public void FadeOut(float fadeOutTime, Action callback = null)
        {
            m_FadePresenter?.FadeOut(fadeOutTime, callback);
        }

        public void FadeInOut(float fadeInOutTime, Action callback = null)
        {
            m_FadePresenter?.FadeInOut(fadeInOutTime, callback);
        }

        // 씬체인저의 ChangeScene을 가져오기위해 컴포넌트를 Add해준다.
        private void CreateSceneChanger()
        {
            m_sceneChanger = gameObject.AddComponent<SceneChanger>();
        }

        #endregion


        #region 재정의 함수

        // 씬체인저를 생성
        protected override void OnCreated()
        {
            base.OnCreated();
            CreateSceneChanger();
        }

        #endregion


        #region 인터페이스 구현


        #endregion

    }
}
