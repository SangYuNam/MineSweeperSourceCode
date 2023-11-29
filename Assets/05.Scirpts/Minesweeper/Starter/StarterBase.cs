using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using MineSweeper.Framework.game;
using MineSweeper.Framework.UI;

namespace MineSweeper.Framework.System
{
    public class StarterBase : MonoBehaviour
    {
        protected virtual void Start()
        {
            InitSingleton();

        }
        protected virtual void InitSingleton()
        {
            UIManager.Init();
            GameManager.Init();
            SoundManager.Init();
        }
    }
}
