using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Kuchinashi;

public class DetailPanelController : MonoBehaviour
{
    private static DetailPanelController _instance;

    public Sprite[] CardType;
    public Sprite[] ValueType;
    private CanvasGroup canvasGroup;
    private Button button;

    private Image mCardImage;
    private TMP_Text mCardText;
    private TMP_Text mDetail;
    private Image mHellValue;
    private Image mColdValue;
    private Image mEroValue;
    private Image mNonsenseValue;

    private void Awake()
    {
        _instance = this;

        canvasGroup = GetComponent<CanvasGroup>();
        button = GetComponent<Button>();

        mCardImage = transform.Find("Card").GetComponent<Image>();
        mCardText = transform.Find("Card/Text").GetComponent<TMP_Text>();

        mDetail = transform.Find("Detail").GetComponent<TMP_Text>();

        mHellValue = transform.Find("Hell/Value").GetComponent<Image>();
        mColdValue = transform.Find("Cold/Value").GetComponent<Image>();
        mEroValue = transform.Find("Ero/Value").GetComponent<Image>();
        mNonsenseValue = transform.Find("Nonsense/Value").GetComponent<Image>();

        Initialize();
    }

    private void Initialize()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;

        button.onClick.AddListener(() => {
            StartCoroutine(CanvasGroupHelper.FadeCanvasGroupWithButton(canvasGroup, button, 0f, 0.1f));
        });
    }

    public static void ShowDetail(int rakugoId)
    {
        var rakugo = GameDesignData.GetRakugo(rakugoId);
        
        _instance.mCardImage.sprite = _instance.CardType[(int) rakugo.Type];
        _instance.mCardText.SetText($"{rakugo.Content.Substring(0, 18)}...");

        _instance.mDetail.SetText(rakugo.Content);

        if (rakugo.Influence.Hell > 3) _instance.mHellValue.sprite = _instance.ValueType[0];
        else if (rakugo.Influence.Hell > 0) _instance.mHellValue.sprite = _instance.ValueType[1];
        else if (rakugo.Influence.Hell == 0) _instance.mHellValue.sprite = _instance.ValueType[2];
        else if (rakugo.Influence.Hell < 0) _instance.mHellValue.sprite = _instance.ValueType[3];
        else if (rakugo.Influence.Hell < -3) _instance.mHellValue.sprite = _instance.ValueType[4];

        if (rakugo.Influence.Cold > 3) _instance.mColdValue.sprite = _instance.ValueType[0];
        else if (rakugo.Influence.Cold > 0) _instance.mColdValue.sprite = _instance.ValueType[1];
        else if (rakugo.Influence.Cold == 0) _instance.mColdValue.sprite = _instance.ValueType[2];
        else if (rakugo.Influence.Cold < 0) _instance.mColdValue.sprite = _instance.ValueType[3];
        else if (rakugo.Influence.Cold < -3) _instance.mColdValue.sprite = _instance.ValueType[4];

        if (rakugo.Influence.Ero > 3) _instance.mEroValue.sprite = _instance.ValueType[0];
        else if (rakugo.Influence.Ero > 0) _instance.mEroValue.sprite = _instance.ValueType[1];
        else if (rakugo.Influence.Ero == 0) _instance.mEroValue.sprite = _instance.ValueType[2];
        else if (rakugo.Influence.Ero < 0) _instance.mEroValue.sprite = _instance.ValueType[3];
        else if (rakugo.Influence.Ero < -3) _instance.mEroValue.sprite = _instance.ValueType[4];

        if (rakugo.Influence.Nonsense > 3) _instance.mNonsenseValue.sprite = _instance.ValueType[0];
        else if (rakugo.Influence.Nonsense > 0) _instance.mNonsenseValue.sprite = _instance.ValueType[1];
        else if (rakugo.Influence.Nonsense == 0) _instance.mNonsenseValue.sprite = _instance.ValueType[2];
        else if (rakugo.Influence.Nonsense < 0) _instance.mNonsenseValue.sprite = _instance.ValueType[3];
        else if (rakugo.Influence.Nonsense < -3) _instance.mNonsenseValue.sprite = _instance.ValueType[4];

        _instance.StartCoroutine(CanvasGroupHelper.FadeCanvasGroupWithButton(_instance.canvasGroup, _instance.button, 1f, 0.1f));
    }
}
