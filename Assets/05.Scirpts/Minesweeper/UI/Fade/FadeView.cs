using MineSweeper.Framework.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MineSweeper.Framework.UI
{
    public class FadeView : ViewBase
    {
        [SerializeField] private Image m_fadeImage;

        public Image FadeImage
        {
            get => m_fadeImage;
            set => m_fadeImage = value; 
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
