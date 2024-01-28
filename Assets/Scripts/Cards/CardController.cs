using JetBrains.Annotations;
using Kuchinashi;
using Kuchinashi.SceneControl;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    bool TurnEnd;
    bool onTutorial;
    Animator[] crowd_Animator;
    Animator character_Animator;
    const int MAX_TURN = 5;
    int TurnNumber = 0;

    private float startTime;
    private float duration => Time.time - startTime;
    private bool first;
    private bool startFirstTurn => duration >= 2.0f;
    public bool CanPreView;
    public Sprite[] sprites;
    public RectTransform rectTransform;
    public int level;
    private void Awake()
    {
        CanPreView = true;
        RakugoList = GameDesignData.Instance.RakugoList;
        UsedCards = new List<int>(RakugoList.Count);
        cardInput = GetComponent<CardInput>();
        cards = GetComponentsInChildren<CardStateMachine>();
        crowd_Animator = new Animator[] { GameObject.Find("crowds").GetComponent<Animator>(), GameObject.Find("crowds (1)").GetComponent<Animator>(), GameObject.Find("crowds (2)").GetComponent<Animator>() };
        character_Animator = GameObject.Find("Character").GetComponent<Animator>();
        first = true;
        foreach (CardStateMachine item in cards)
        {
            item.Init();
        }
        if (DataRepeater.Instance.CurrentElements == null)
        {
            DataRepeater.Instance.CurrentElements = new Element();
            DataRepeater.Instance.CurrentElements.Ero = 0;
            DataRepeater.Instance.CurrentElements.Hell = 0;
            DataRepeater.Instance.CurrentElements.Cold = 0;
            DataRepeater.Instance.CurrentElements.Nonsense = 0;
        }
        if (GameProgressData.Instance.RoundDataList.Count == 0 || GameProgressData.Instance.RoundDataList == null)
        {
            int radNum = Random.Range(0, 5);
            level = radNum;
            DataRepeater.Instance.CurrentLevelId = radNum;
            onTutorial = true;
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
        if(!onTutorial) CheckHoverInThisCrd(transform.gameObject);
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
            //StartCoroutine(WaitForShuffle());
            StartCoroutine(TurnEndInterval());
            
            Debug.Log(TurnNumber);
            if(TurnNumber == MAX_TURN)
            {
                if (GameProgressData.Instance.RoundDataList.Count < 5)
                {
                    if (checkElement())
                    {
                        SceneControl.SwitchSceneWithoutConfirm("EndScene");
                    }
                    SceneControl.SwitchSceneWithoutConfirm("SettlementScene", () =>
                    {
                        SettlementManager.SettleGame(DataRepeater.Instance.CurrentLevelId, UsedCards);
                    });
                }
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
    public void SetCardContent(Rakugo rakugo, TMP_Text content) => content.SetText($"{rakugo.Content.Substring(0, 18)}...");
    public void SetCardImage(Type type, Image image)
    {
        switch (type)
        {
            case Type.Hell:
                image.sprite = sprites[2];
                break;
            case Type.Cold:
                image.sprite = sprites[0];
                break;
            case Type.Ero:
                image.sprite = sprites[1];
                break;
            case Type.Nonsense:
                image.sprite = sprites[3];
                break;
            default:
                break;
        }
    }

    public void SetDetailedPanel(Rakugo rakugo)
    {
        TMP_Text text = GameObject.Find("DetailedPreview").GetComponentInChildren<TMP_Text>();
        text.SetText($"{rakugo.Content}");
    }
    public void SetDetailedPanel()
    {
        TMP_Text text = GameObject.Find("DetailedPreview").GetComponentInChildren<TMP_Text>();
        text.SetText("");
    }
    public void avtivateCards() => onTutorial = false;

    public bool checkCardAndLevel(Rakugo rakugo)
    {
        if (level == 0) return true;
        if (rakugo.Type == Type.Cold && level == 2) return true;
        else if (rakugo.Type == Type.Ero && level == 3) return true;
        else if (rakugo.Type == Type.Hell && level == 1) return true;
        else if (rakugo.Type == Type.Nonsense && level == 4) return true;
        else return false;
    }
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
    public void CrowdsLaugh()
    {
        foreach (var item in crowd_Animator)
        {
            item.SetTrigger("laugh");
        }
    }
    public void CharacterTalk() => character_Animator.SetTrigger("talk");

    IEnumerator TurnEndInterval()
    {
        while (TurnEnd)
        {
            if(cards[0].turnEndFinished && cards[1].turnEndFinished && cards[2].turnEndFinished)
            {
                TurnNumber += 1;
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
    private bool checkElement()
    {
        Element element = DataRepeater.Instance.CurrentElements;
        Debug.Log(element.Cold + " " + element.Ero + " " + element.Hell + " " + element.Nonsense);
        return (element.Ero < -15 || element.Ero > 15 || element.Cold < -15 || element.Cold > 15 || element.Hell < -15 || element.Hell > 15 || element.Nonsense < -15 || element.Nonsense > 15);
    }
    IEnumerator WaitForShuffle()
    {
        yield return new WaitForSeconds(1.5f);
    }
}
