using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Kuchinashi;
using Kuchinashi.SceneControl;
using Unity.Mathematics;
using Random = UnityEngine.Random;

public partial class SettlementSceneController : MonoBehaviour
{
    private static SettlementSceneController _instance;

    private CanvasGroup canvasGroup;
    private CanvasGroup scoreCanvasGroup;
    private CanvasGroup fansCanvasGroup;
    private CanvasGroup tachieCanvasGroup;
    private CanvasGroup tachieWordsCanvasGroup;
    private CanvasGroup nextStageCanvasGroup;
    private CanvasGroup nextButtonCanvasGroup;

    private TMP_Text mScore;
    private TMP_Text mFans;
    private TMP_Text mDeltaFans;
    private TMP_Text mTachieWords;
    private TMP_Text mNextLevelName;
    private TMP_Text mNextLevelDescription;
    private Button mNextLevelButton;

    private void Awake()
    {
        _instance = this;

        canvasGroup = GetComponent<CanvasGroup>();
        scoreCanvasGroup = transform.Find("Score").GetComponent<CanvasGroup>();
        fansCanvasGroup = transform.Find("Fans").GetComponent<CanvasGroup>();
        tachieCanvasGroup = transform.Find("Tachie").GetComponent<CanvasGroup>();
        tachieWordsCanvasGroup = transform.Find("Tachie/TextBox").GetComponent<CanvasGroup>();
        nextStageCanvasGroup = transform.Find("NextStage").GetComponent<CanvasGroup>();
        nextButtonCanvasGroup = transform.Find("Next").GetComponent<CanvasGroup>();

        mScore = transform.Find("Score/Value").GetComponent<TMP_Text>();
        mFans = transform.Find("Fans/Value").GetComponent<TMP_Text>();
        mDeltaFans = transform.Find("Fans/DeltaValue").GetComponent<TMP_Text>();
        mTachieWords = transform.Find("Tachie/TextBox/Text").GetComponent<TMP_Text>();
        mNextLevelName = transform.Find("NextStage/Name").GetComponent<TMP_Text>();
        mNextLevelDescription = transform.Find("NextStage/Description").GetComponent<TMP_Text>();

        mNextLevelButton = transform.Find("Next").GetComponent<Button>();

        Initialize();
    }

    private void Initialize()
    {
        canvasGroup.alpha = 0;
        scoreCanvasGroup.alpha = 0;
        fansCanvasGroup.alpha = 0;
        tachieCanvasGroup.alpha = 0;
        tachieWordsCanvasGroup.alpha = 0;
        nextStageCanvasGroup.alpha = 0;
        nextButtonCanvasGroup.alpha = 0;

        mScore.SetText("");
        mFans.SetText("");
        mDeltaFans.SetText("");
        mDeltaFans.color = Color.white;
        mTachieWords.SetText("");
        mNextLevelName.SetText("");
        mNextLevelDescription.SetText("");

        mNextLevelButton.onClick.RemoveAllListeners();
    }

    public static void Action(int score, int fans, int deltaFans, string tachieWords, int nextLevelId)
    {
        _instance.StartCoroutine(_instance.ActionCoroutine(score, fans, deltaFans, tachieWords, nextLevelId));

        _instance.mNextLevelButton.interactable = true;
        _instance.mNextLevelButton.onClick.AddListener(() =>
        {
            AudioManager.Instance.SwitchMusic();
            _instance.mNextLevelButton.interactable = false;
            var animator = _instance.mNextLevelButton.GetComponent<Animator>();
            animator.enabled = true;
            animator.Play("Pressed");
            SceneControl.SwitchSceneWithoutConfirm("MainScene", () => { DataRepeater.Instance.CurrentLevelId = nextLevelId; });
        });
    }

    IEnumerator ActionCoroutine(int score, int fans, int deltaFans, string tachieWords, int nextLevelId)
    {
        yield return new WaitForSeconds(2f);

        yield return CanvasGroupHelper.FadeCanvasGroup(canvasGroup, 1, 0.2f);

        yield return new WaitForSeconds(1f);

        yield return CanvasGroupHelper.FadeCanvasGroup(scoreCanvasGroup, 1, 0.2f);
        yield return new WaitForSeconds(0.3f);
        yield return StepNumber(mScore, 0, score);

        yield return new WaitForSeconds(0.5f);

        mFans.SetText(fans.ToString());
        yield return CanvasGroupHelper.FadeCanvasGroup(fansCanvasGroup, 1, 0.2f);
        yield return new WaitForSeconds(0.3f);
        yield return FadeDeltaFans(deltaFans);
        yield return new WaitForSeconds(0.3f);
        yield return StepNumber(mFans, fans, fans + deltaFans);

        yield return new WaitForSeconds(0.5f);

        yield return CanvasGroupHelper.FadeCanvasGroup(tachieCanvasGroup, 1, 0.05f);
        yield return new WaitForSeconds(0.5f);
        yield return CanvasGroupHelper.FadeCanvasGroup(tachieWordsCanvasGroup, 1, 0.05f);
        yield return new WaitForSeconds(0.3f);
        yield return TypeText(mTachieWords, tachieWords);

        yield return new WaitForSeconds(0.5f);

        yield return CanvasGroupHelper.FadeCanvasGroup(nextStageCanvasGroup, 1, 0.1f);
        yield return new WaitForSeconds(0.3f);
        yield return TypeText(mNextLevelName, GameDesignData.GetLevel(nextLevelId).Name);
        yield return new WaitForSeconds(0.3f);
        yield return TypeText(mNextLevelDescription, GameDesignData.GetLevel(nextLevelId).Description);

        yield return new WaitForSeconds(1f);

        yield return CanvasGroupHelper.FadeCanvasGroup(nextButtonCanvasGroup, 1, 0.05f);
    }
}

public partial class SettlementSceneController
{
    IEnumerator FadeDeltaFans(int value)
    {
        mDeltaFans.SetText(value > 0 ? $"+{value}" : $"{value}");
        mDeltaFans.color = value > 0 ? Color.green : Color.red;

        while (!Mathf.Approximately(mDeltaFans.alpha, 1f))
        {
            mDeltaFans.alpha = Mathf.MoveTowards(mDeltaFans.alpha, 1f, 0.2f);
            yield return new WaitForFixedUpdate();
        }

        mDeltaFans.alpha = 1f;
        yield break;
    }
    
    IEnumerator TypeText(TMP_Text text, string target)
    {
        text.SetText("");

        var len = target.Length;
        var currentText = "";
        var speed = 1 / 12f;
        
        for (var i = 0; i < len; i++)
        {
            currentText += target[i];
            text.SetText(currentText);

            yield return new WaitForSeconds(speed);
        }

        yield break;
    }

    IEnumerator StepNumber(TMP_Text text, int origin, int target)
    {
        text.SetText(origin.ToString());
        var currentValue = origin;

        if (currentValue < target)
        {
            while (Mathf.Abs(currentValue / (float) target) <= 0.95f)
            {
                text.SetText(currentValue.ToString());

                currentValue = Mathf.RoundToInt(Mathf.Lerp(currentValue, target, 0.1f));
                yield return new WaitForFixedUpdate();
            }
        }
        else
        {
            while (Mathf.Abs((float) target / currentValue) <= 0.95f)
            {
                text.SetText(currentValue.ToString());

                currentValue = Mathf.RoundToInt(Mathf.Lerp(currentValue, target, 0.1f));
                yield return new WaitForFixedUpdate();
            }
        }

        text.SetText(target.ToString());
        yield break;
    }
}