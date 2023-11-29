using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MineSweeper.Framework.System
{
    public enum ESceneType
    {
        Intro = 0, // 최초 씬
        Lobby, // 메인 씬
        Play, // 플레이 씬
    }
    public class SceneChanger : MonoBehaviour
    {
        #region 필드 변수

        private ESceneType m_currentScene;

        #endregion


        #region 유니티 고유 이벤트
        #endregion


        #region 함수

        public void ChangeScene(ESceneType sceneType)
        {
            if (m_currentScene == sceneType)
            {
                return;
            }

            switch (sceneType)
            {
                case ESceneType.Intro:
                    SceneManager.LoadScene(0);
                    break;
                case ESceneType.Lobby:
                    SceneManager.LoadScene(1);
                    break;
                case ESceneType.Play:
                    SceneManager.LoadScene(2);
                    break;
                default:
                    Debug.Log("Doesn't Exist State");
                    break;
            }
        }

        #endregion


        #region 재정의 함수
        #endregion


        #region 인터페이스 구현
        #endregion

    }
}
