using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Kuchinashi;
using Kuchinashi.SceneControl;

public class StartSceneController : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private CanvasGroup creditCanvasGroup;
    private Animator mCanvasAnimator;

    private Button mStart;
    private Animator mStartAnimator;
    private Button mZukan;
    private Animator mZukanAnimator;
    private Button mExit;
    private Animator mExitAnimator;

    private MessageManager mExitConfirm;

    private Button mCredit;
    private Button mCreditClose;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        creditCanvasGroup = transform.Find("CreditPanel").GetComponent<CanvasGroup>();
        mCanvasAnimator = GetComponent<Animator>();

        mStart = transform.Find("Menu/Start").GetComponent<Button>();
        mStartAnimator = mStart.GetComponent<Animator>();
        mZukan = transform.Find("Menu/Zukan").GetComponent<Button>();
        mZukanAnimator = mZukan.GetComponent<Animator>();
        mExit = transform.Find("Menu/Exit").GetComponent<Button>();
        mExitAnimator = mExit.GetComponent<Animator>();

        mExitConfirm = transform.Find("ExitConfirm").GetComponent<MessageManager>();

        mCredit = transform.Find("Credit").GetComponent<Button>();
        mCreditClose = transform.Find("CreditPanel").GetComponent<Button>();

        Initialize();
    }

    private void Initialize()
    {
        canvasGroup.alpha = 1;
        creditCanvasGroup.alpha = 0;

        mStart.interactable = true;
        mStart.onClick.AddListener(() => {
            mStart.interactable = false;
            StartCoroutine(GameStartCoroutine());
        });

        mZukan.interactable = true;
        mZukan.onClick.AddListener(() => {
            mZukan.interactable = false;
            StartCoroutine(ZukanCoroutine());
        });

        mExit.interactable = true;
        mExit.onClick.AddListener(() => {
            mExit.interactable = false;
            mExitAnimator.Play("Pressed");
            mExitConfirm.Show(() => Application.Quit());
            mExit.interactable = true;
        });

        mCredit.onClick.AddListener(() => {
            StartCoroutine(CanvasGroupHelper.FadeCanvasGroupWithButton(creditCanvasGroup, mCreditClose, 1f, 0.1f));
        });
        mCreditClose.onClick.AddListener(() => {
            StartCoroutine(CanvasGroupHelper.FadeCanvasGroupWithButton(creditCanvasGroup, mCreditClose, 0f, 0.1f));
        });
    }

    IEnumerator GameStartCoroutine()
    {
        mCanvasAnimator.speed = 1;

        mStartAnimator.Play("Pressed");
        yield return new WaitForSeconds(0.5f);

        mCanvasAnimator.enabled = true;
        mCanvasAnimator.Play("FadeOut");
        yield return new WaitForSeconds(1.5f);

        mCanvasAnimator.speed = 0;

        SceneControl.SwitchSceneWithoutConfirm("MainScene");
    }

    IEnumerator ZukanCoroutine()
    {
        mCanvasAnimator.speed = 1;

        mZukanAnimator.Play("Pressed");
        yield return new WaitForSeconds(0.5f);

        mCanvasAnimator.enabled = true;
        mCanvasAnimator.Play("FadeOut");
        yield return new WaitForSeconds(1.5f);

        mCanvasAnimator.speed = 0;

        SceneControl.SwitchSceneWithoutConfirm("ZukanScene");
    }
}
