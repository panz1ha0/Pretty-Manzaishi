using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CardController: MonoBehaviour
{
    List<Rakugo> RakugoList;
    List<int> UsedCards;
    CardInput cardInput;
    CardStateMachine[] cards;
    bool TermOver;

    public RectTransform rectTransform;

    private void Awake()
    {
        RakugoList = GameDesignData.Instance.RakugoList;
        UsedCards = new List<int>(RakugoList.Count);
        cardInput = GetComponent<CardInput>();
        cards = GetComponentsInChildren<CardStateMachine>();
        foreach (Rakugo item in Dealer())
        {
            Debug.Log(item.Id);
        }

        rectTransform = GetComponent<RectTransform>();
    }
    private void Update()
    {
        CheckHoverInThisCrd(transform.gameObject);
        //Debug.Log("isPreviewing: " + isPreviewing);
    }

    private void FixedUpdate()
    {
        foreach (CardStateMachine item in cards)
        {
            if (item.GetCurrentState()?.GetType() == typeof(CardState_Casted)) TermOver = true;
        }
        if (TermOver)
        {
            //Debug.Log("A");
            foreach (CardStateMachine item in cards)
            {
                item.termOver = true;
            }
            foreach (CardStateMachine item in cards)
            {
                Debug.Log(item.gameObject.name + ": " + item.unborn);
            }
            TermOver = false;
        }
    }
    private void CheckHoverInThisCrd(GameObject canvas)
    {
        CardStateMachine card;
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        Vector2 rayTarget = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D[] results = Physics2D.RaycastAll(rayTarget, Vector2.zero, 100f, LayerMask.GetMask("Card"));

        GraphicRaycaster gr = canvas.GetComponent<GraphicRaycaster>();
        // List<RaycastResult> results = new List<RaycastResult>();
        // gr.Raycast(pointerEventData, results);

        if (results.Length != 0)
        {
            foreach (var item in results)
            {
                //Debug.Log(item.gameObject.name);
                if(item.collider.TryGetComponent<CardStateMachine>(out card))
                {
                    Debug.Log("Yeah");
                    card.isPreviewing = true;
                }
            }
        }
        else
        {
            foreach (CardStateMachine item in cards)
            {
                item.isPreviewing = false;
            }
        }
    }
    public void MarkUsedCard(int id) => UsedCards.Add(id);
    public List<Rakugo> Dealer()
    {
        List<int> temp = new List<int>();
        List<Rakugo> rakugos = new List<Rakugo>();
        while (true)
        {
            int a = Random.Range(0, 48);
            foreach (int item in UsedCards)
            {
                if (a == item) continue;
            }
            foreach (int item in temp)
            {
                if (a == item) continue;
            }
            temp.Add(a);
            rakugos.Add(RakugoList[a]);
            if (rakugos.Count == 3) return rakugos;
        }
    }
}
