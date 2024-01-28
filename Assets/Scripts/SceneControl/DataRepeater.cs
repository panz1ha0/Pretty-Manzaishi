using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Kuchinashi
{
    public class DataRepeater : MonoBehaviour
    {
        private static DataRepeater _instance;
        public static DataRepeater Instance
        {
            get => _instance ??= new DataRepeater();
            private set => _instance = value;
        }

        public int CurrentLevelId = 0;

        public Element CurrentElements;
        public int CurrentFans;

        public static void Initialize()
        {
            Instance.CurrentFans = 0;
            Instance.CurrentElements = new Element();
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}