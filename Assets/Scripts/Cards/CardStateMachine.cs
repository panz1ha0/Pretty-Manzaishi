using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.VersionControl.Asset;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using System.Collections;
using UnityEngine.UI;

public class CardStateMachine: StateMachine
{
    private Transform card;
    private Image image;
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

    public void LoadCard(Rakugo rakugo)
    {
        baseCard.rakugoData = rakugo;
    }

    public void Restart(Rakugo rakugo)
    {
        LoadCard(rakugo);
        SwitchState(typeof(CardState_Shuffle));
    }

    private void Awake()
    {
        card = GetComponent<Transform>();
        image = GetComponentInChildren<Image>();
        playerInput = GetComponentInParent<CardInput>();
        cardController = GetComponentInParent<CardController>();
        baseCard = GetComponent<BaseCard>();
        statetable = new Dictionary<System.Type, ICardState>(cardStates.Length);

        foreach (CardState state in cardStates)
        {
            state.Init(this, card, image, playerInput, cardController, baseCard);
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
            StartCoroutine(Dissolve());
            startDissolve = material.GetFloat("_BurnAmount") < 1;
            Debug.Log(DissolveEnd());
        }
    }
    IEnumerator Dissolve()
    {
        while (BurnAmount < 1)
        {
            BurnAmount += BurnSpeed * Time.deltaTime;
            material.SetFloat("_BurnAmount", BurnAmount);
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