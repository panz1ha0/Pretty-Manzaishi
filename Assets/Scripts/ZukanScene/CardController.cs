using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Kuchinashi
{
    public class CardController : MonoBehaviour
    {
        private Rakugo data;
        private Button button;
        private TMP_Text content;

        private void Awake()
        {
            button = GetComponent<Button>();
            content = GetComponentInChildren<TMP_Text>();
        }

        public void Init(Rakugo rakugo)
        {
            data = rakugo;
            content.SetText($"{rakugo.Content.Substring(0, 18)}...");

            button.onClick.AddListener(() => DetailPanelController.ShowDetail(data.Id));
        }
    }

}