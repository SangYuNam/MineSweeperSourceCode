using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MineSweeper.Framework.System
{
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private static T m_instance;
        private static bool m_isDestroy = false;
        public static T Instance
        {
            get
            {
                if(m_isDestroy == true)
                {
                    Debug.Log($"{typeof(T).Name} Destroy or Not Initailized");
                    return null;
                }
                if (m_instance == null)
                {
                    GameObject obj = GameObject.Find(typeof(T).Name);

                    if (obj != null)
                    {
                        m_instance = obj.GetComponent<T>();
                    }
                }
                if(m_instance == null)
                {
                    GameObject obj = new GameObject(typeof(T).Name);
                    m_instance = obj.AddComponent<T>();
                }

                return m_instance;
            }
        }

        public static void Init()
        {
            Debug.Log($"{typeof(T).Name}Init");
            m_isDestroy = false;
            Instance.OnCreated();
            DontDestroyOnLoad(Instance);
        }
        protected virtual void OnCreated()
        {
            // 재정의 함수
        }

        protected virtual void OnDestroy()
        {
            Debug.Log($"{typeof(T).Name}OnDestroy");
            m_isDestroy = true;
        }
    }
}
