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

        public int LevelCount = 0;

        public int CurrentLevelId = 0;

        public Element CurrentElements;
        public int CurrentFans;
    }
}