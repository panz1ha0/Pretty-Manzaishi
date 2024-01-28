using System.Collections;
using System.Collections.Generic;
using Kuchinashi;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public Sprite[] TutorialImages;
    private CanvasGroup canvasGroup;
    private Button button;
    private Image image;

    private int currentPage = 0;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        button = GetComponent<Button>();
        image = GetComponent<Image>();

        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;

        currentPage = 0;

        if (DataRepeater.Instance.LevelCount == 0)
        {
            image.sprite = TutorialImages[currentPage];
            StartCoroutine(CanvasGroupHelper.FadeCanvasGroupWithButton(canvasGroup, button, 1f, 0.1f));

            button.onClick.AddListener(() => {
                if (currentPage < TutorialImages.Length - 1)
                {
                    image.sprite = TutorialImages[++currentPage];
                }
                else
                {
                    StartCoroutine(CanvasGroupHelper.FadeCanvasGroupWithButton(canvasGroup, button, 0f, 0.1f));
                }
            });
        }
    }
}
