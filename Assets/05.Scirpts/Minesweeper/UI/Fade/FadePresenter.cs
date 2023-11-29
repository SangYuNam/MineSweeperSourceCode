using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace MineSweeper.Framework.UI
{
    public class FadePresenter : PresenterBase
    {
        [SerializeField] private FadeView m_currentView;

        public void FadeIn(float fadeOutTime, Action callback = null)
        {
            StartCoroutine(StartFadeIn(fadeOutTime, callback));
        }

        public void FadeOut(float fadeOutTime, Action callback = null)
        {
            StartCoroutine(StartFadeOut(fadeOutTime, callback));
        }

        public void FadeInOut(float fadeOutTime, Action callback = null)
        {
            StartCoroutine(StartFadeInOut(fadeOutTime, callback));
        }

        // 투명 -> 불투명
        private IEnumerator StartFadeIn(float fadeOutTime, Action callback = null)
        {
            m_currentView.FadeImage.gameObject.SetActive(true);

            Image image = m_currentView.FadeImage;
            Color tempColor = image.color;
            while (tempColor.a < 1f)
            {
                tempColor.a += Time.deltaTime / fadeOutTime;
                image.color = tempColor;

                if (tempColor.a >= 1f) tempColor.a = 1f;

                yield return null;
            }

            image.color = tempColor;

            // 콜백
            if (callback != null)
            {
                callback();
            }
        }

        // 불투명 -> 투명
        private IEnumerator StartFadeOut(float fadeOutTime, Action callback = null)
        {
            Image image = m_currentView.FadeImage;
            Color tempColor = image.color;
            while (tempColor.a > 0f)
            {
                tempColor.a -= Time.deltaTime / fadeOutTime;
                image.color = tempColor;

                if (tempColor.a <= 0f) tempColor.a = 0f;

                yield return null;
            }

            image.color = tempColor;

            // 콜백
            if (callback != null)
            {
                callback();
            }

            m_currentView.FadeImage.gameObject.SetActive(false);
        }

        private IEnumerator StartFadeInOut(float fadeInOutTime, Action callback = null)
        {
            m_currentView.FadeImage.gameObject.SetActive(true);

            Image image = m_currentView.FadeImage;
            Color tempColor = image.color;
            while (tempColor.a < 1f)
            {
                tempColor.a += Time.deltaTime / fadeInOutTime;
                image.color = tempColor;

                if (tempColor.a >= 1f) tempColor.a = 1f;

                yield return null;
            }

            // 콜백
            if (callback != null)
            {
                callback();
            }

            while (tempColor.a > 0f)
            {
                tempColor.a -= Time.deltaTime / fadeInOutTime;
                image.color = tempColor;

                if (tempColor.a <= 0f) tempColor.a = 0f;

                yield return null;
            }

            image.color = tempColor;
            m_currentView.FadeImage.gameObject.SetActive(false);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Open()
        {
            base.Open();
        }

        public override void Close()
        {
            base.Close();
        }
    }
}
