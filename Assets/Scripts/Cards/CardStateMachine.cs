using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class CardStateMachine: StateMachine
{
    private Transform card;
    private Image image;
    private TMP_Text content;
    private CardState lastState = null;
    CardInput playerInput;
    CardController cardController;
    BaseCard baseCard;

    float BurnAmount = 0;
    float BurnSpeed = 0.5f;
    public bool startDissolve;
    Material material;
    public bool unburn = true;
    public bool turnEnd;
    public bool turnEndFinished;
    public bool isPreviewing;
    public bool OneHasDissolved;
    public void StartDissolve(bool flag) => startDissolve = flag;
    public bool DissolveEnd() => !startDissolve;
    public void SetBurnSpeed(float burnSpeed) => BurnSpeed = burnSpeed;
    public CardState[] cardStates;

    

    public void Restart(Rakugo rakugo)
    {
        LoadCard(rakugo);
        cardController.SetCardImage(rakugo.Type, image);
        cardController.SetCardContent(rakugo, content);
        SwitchState(typeof(CardState_Shuffle));
    }
    public void Init()
    {
        unburn = true;
        turnEnd = false;
        turnEndFinished = false;
        isPreviewing = false;
        OneHasDissolved = false;
        startDissolve = false;
        image.material = null;
        BurnAmount = 0;
        baseCard.Init();
    }
    private void Awake()
    {
        card = GetComponent<Transform>();
        image = GetComponentInChildren<Image>();
        content = GetComponentInChildren<TMP_Text>();
        playerInput = GetComponentInParent<CardInput>();
        cardController = GetComponentInParent<CardController>();
        baseCard = GetComponent<BaseCard>();
        statetable = new Dictionary<System.Type, ICardState>(cardStates.Length);

        foreach (CardState state in cardStates)
        {
            state.Init(this, card, image, content, playerInput, cardController, baseCard);
            statetable.Add(state.GetType(), state);
        }
    }
    private void Start()
    {
        Color currentColor = image.color;
        currentColor.a = 0;
        image.color = currentColor;
        //SwitchOn(statetable[typeof(CardState_Shuffle)]);
    }
    private void FixedUpdate()
    {
        if (unburn && startDissolve)
        {
            material = Resources.Load<Material>("Config/CardState/Custom_DissolveShader");
            image.material = material;
            unburn = false;
        }
        if (startDissolve && !unburn)
        {
            Debug.Log(transform.gameObject.name + " disslove");
            StartCoroutine(Dissolve());
            startDissolve = material.GetFloat("_BurnAmount") < 1;
            Debug.Log(DissolveEnd());
        }
    }
    public void LoadCard(Rakugo rakugo)
    {
        baseCard.rakugoData = rakugo;
    }
    IEnumerator Dissolve()
    {
        while (BurnAmount < 1)
        {
            BurnAmount += BurnSpeed * Time.deltaTime;
            material.SetFloat("_BurnAmount", BurnAmount);
            content.gameObject.GetComponent<CanvasGroup>().alpha -= 2 * BurnSpeed * Time.deltaTime;
            yield return null;
        }
    }
    public CardState GetLastState()
    {
        return lastState;
    }
    public void SetLastState(CardState cardState)
    {
        this.lastState = cardState;
    }
}