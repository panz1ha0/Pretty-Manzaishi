using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Kuchinashi
{
    public class CanvasGroupHelper
    {
        public static IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float alpha, float speed)
        {
            if (canvasGroup.alpha == alpha) yield break;

            if (alpha == 1f) canvasGroup.blocksRaycasts = true;

            while (!Mathf.Approximately(canvasGroup.alpha, alpha))
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, alpha, speed);
                yield return new WaitForFixedUpdate();
            }
            canvasGroup.alpha = alpha;

            if (alpha == 0f) canvasGroup.blocksRaycasts = false;
        }

        public static IEnumerator FadeCanvasGroupWithButton(CanvasGroup canvasGroup, Button button, float alpha, float speed)
        {
            if (canvasGroup.alpha == alpha) yield break;

            if (alpha == 1f) canvasGroup.blocksRaycasts = true;
            else button.interactable = false;

            while (!Mathf.Approximately(canvasGroup.alpha, alpha))
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, alpha, speed);
                yield return new WaitForFixedUpdate();
            }
            canvasGroup.alpha = alpha;

            if (alpha == 0f) canvasGroup.blocksRaycasts = false;
            else button.interactable = true;
        }
    }

}