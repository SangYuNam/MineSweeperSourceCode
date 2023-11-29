using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MineSweeper.Framework.System;
using UnityEngine.AddressableAssets;
using TMPro;

namespace MineSweeper.Framework.UI
{
    public class UIManager : Singleton<UIManager>
    {
        #region 필드 변수
        #endregion


        #region 유니티 고유 이벤트

        #endregion


        #region 함수

        private void CreateCanvas(string key)
        {
            GameObject obj = Resources.Load<GameObject>(key);
            Instantiate(obj, transform);
        }

        #endregion


        #region 재정의 함수

        protected override void OnCreated()
        {
            base.OnCreated();
            CreateCanvas("MainCanvas");
        }

        #endregion


        #region 인터페이스 구현
        #endregion

    }
}
