using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Kuchinashi.SceneControl;

public class ZukanManager : MonoBehaviour
{
    public GameObject[] cardPrefabs;
    private int currentPage = 0;

    private Button mHome;
    private Button mNextPage;
    private Button mPrevPage;
    private TMP_Text mPageNumber;

    private Transform mContent;

    private void Awake()
    {
        currentPage = 0;

        mHome = transform.Find("Home").GetComponent<Button>();
        mNextPage = transform.Find("Next").GetComponent<Button>();
        mPrevPage = transform.Find("Prev").GetComponent<Button>();

        mPageNumber = transform.Find("Page").GetComponent<TMP_Text>();

        mContent = transform.Find("Content");

        mHome.onClick.AddListener(() => {
            SceneControl.SwitchSceneWithoutConfirm("StartScene");
        });
        mNextPage.onClick.AddListener(() => {
            currentPage = currentPage < GameDesignData.Instance.RakugoList.Count / 8 ? currentPage + 1 : currentPage;
            GenerateCards();
        });
        mPrevPage.onClick.AddListener(() => {
            currentPage = currentPage > 0 ? currentPage - 1 : currentPage;
            GenerateCards();
        });

        GenerateCards();
    }

    private List<Rakugo> GenerateList(int page)
    {
        var list = new List<Rakugo>();
        for (var i = page * 8; i < (GameDesignData.Instance.RakugoList.Count > ((page + 1) * 8) ? ((page + 1) * 8) : GameDesignData.Instance.RakugoList.Count); i++)
        {
            if (GameDesignData.GetRakugo(i) == null) break;
            list.Add(GameDesignData.GetRakugo(i));
        }

        return list;
    }

    private void GenerateCards()
    {
        mNextPage.gameObject.SetActive(currentPage < GameDesignData.Instance.RakugoList.Count / 8);
        mPrevPage.gameObject.SetActive(currentPage > 0);

        mPageNumber.text = $"{currentPage + 1} / {GameDesignData.Instance.RakugoList.Count / 8 + 1}";

        var count = mContent.childCount;
        for (int i = 0; i < count; i++)
        {
            Destroy(mContent.GetChild(i).gameObject);
        }

        foreach (var rakugo in GenerateList(currentPage))
        {
            if (GameProgressData.Instance.UnlockedRakugo.TryGetValue(rakugo.Id, out bool value))
            {
                if (value)
                {
                    Instantiate(cardPrefabs[(int) rakugo.Type], mContent).GetComponent<Kuchinashi.CardController>().Init(rakugo);
                    continue;
                }
            }
            Instantiate(cardPrefabs[4], mContent);
        }
    }
}
