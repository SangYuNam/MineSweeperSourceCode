using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MineSweeper.Framework.game
{
    public class Block : MonoBehaviour
    {
        public int Index = 0;
        public int NumCount = 0;
        public int Width = 0;
        public int Height = 0;
        public bool ChangeBlock = false;
        public bool IsFlag = false;

        Animator myAnim = null;

        private void Start()
        {
            myAnim = GetComponent<Animator>();
        }

        public void ClickBlock()
        {
            myAnim.SetTrigger("isClick");
        }
    }
}

