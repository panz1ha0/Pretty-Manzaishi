using JetBrains.Annotations;
using Kuchinashi;
using Kuchinashi.SceneControl;
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
    int level;
    CardStateMachine[] cards;
    bool TurnEnd;
    const int MAX_TURN = 5;
    int TurnNumber = 0;

    private float startTime;
    private float duration => Time.time - startTime;
    private bool first;
    private bool startFirstTurn => duration >= 2.0f;

    public RectTransform rectTransform;

    private void Awake()
    {
        RakugoList = GameDesignData.Instance.RakugoList;
        UsedCards = new List<int>(RakugoList.Count);
        cardInput = GetComponent<CardInput>();
        cards = GetComponentsInChildren<CardStateMachine>();
        first = true;
        foreach (CardStateMachine item in cards)
        {
            item.Init();
        }
        if (DataRepeater.Instance.CurrentElements == null)
        {
            DataRepeater.Instance.CurrentElements = new Element();
            //DataRepeater.Instance.CurrentElements.Ero = 0;
            //DataRepeater.Instance.CurrentElements.Hell = 0;
            //DataRepeater.Instance.CurrentElements.Cold = 0;
            //DataRepeater.Instance.CurrentElements.Nonsense = 0;
        }
        if (GameProgressData.Instance.RoundDataList.Count == 0)
        {
            int radNum = Random.Range(0, 5);
            DataRepeater.Instance.CurrentLevelId = radNum;
        }
        else
        {
            level = DataRepeater.Instance.CurrentLevelId;
        }
        rectTransform = GetComponent<RectTransform>();
    }
    private void Start()
    {
        startTime = Time.time;
    }
    private void Update()
    {
        if(startFirstTurn && first)
        {
            List<Rakugo> drawCard = Dealer();
            for (int i = 0; i < cards.Length; i++)
            {
                cards[i].Restart(drawCard[i]);
            }
            first = false;
        }
        CheckHoverInThisCrd(transform.gameObject);
        //Debug.Log("isPreviewing: " + isPreviewing);
    }

    private void FixedUpdate()
    {
        if (!startFirstTurn) return;
        foreach (CardStateMachine item in cards)
        {
            if (item.GetCurrentState()?.GetType() == typeof(CardState_Shuffle) && item.GetLastState()?.GetType() == typeof(CardState_Casted)) TurnEnd = true;
        }
        if (TurnEnd)
        {
            foreach (CardStateMachine item in cards)
            {
                item.turnEnd = true;
            }
            StartCoroutine(TurnEndInterval());
            if(TurnNumber == MAX_TURN)
            {
                SceneControl.SwitchSceneWithoutConfirm("SettlementScene", () => { SettlementManager.SettleGame(DataRepeater.Instance.CurrentLevelId, UsedCards); });
            }
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
                //Debug.Log(item.collider.gameObject.name);
                if(item.collider.TryGetComponent<CardStateMachine>(out card))
                {
                    //Debug.Log("Yeah");
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
    public void UpdateElements(Element element) => DataRepeater.Instance.CurrentElements += element;
    private List<Rakugo> Dealer()
    {
        List<int> temp = new List<int>();
        List<Rakugo> rakugos = new List<Rakugo>();
        while (true)
        {
            int a = Random.Range(0, 49);
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

    IEnumerator TurnEndInterval()
    {
        while (TurnEnd)
        {
            if(cards[0].turnEndFinished && cards[1].turnEndFinished && cards[2].turnEndFinished)
            {
                TurnNumber += 1;
                Debug.Log(TurnNumber);
                TurnEnd = false;
                List<Rakugo> drawCard = Dealer();
                for (int i = 0; i < cards.Length; i++)
                {
                    cards[i].Init();
                    cards[i].Restart(drawCard[i]);
                }
            }
            yield return null;
        }
        
    }
}
