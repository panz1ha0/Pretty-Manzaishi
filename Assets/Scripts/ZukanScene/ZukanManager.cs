using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZukanManager : MonoBehaviour
{
    public GameObject[] cardPrefabs;
    private int currentPage = 0;

    private void Awake()
    {
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
        foreach (var child in transform.GetComponentsInChildren<CardController>())
        {
            Destroy(child.gameObject);
        }

        foreach (var rakugo in GenerateList(currentPage))
        {
            Instantiate(cardPrefabs[(int) rakugo.Type], transform).GetComponent<CardController>().Init(rakugo);
        }
    }
}
