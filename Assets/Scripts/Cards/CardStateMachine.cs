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
    CardInput playerInput;
    CardController cardController;
    BaseCard baseCard;

    float BurnAmount = 0;
    float BurnSpeed = 0.5f;
    public bool startDissolve;
    Material material;
    public bool unborn = true;
    public bool termOver;
    public bool isPreviewing;
    public bool OneHasDissolved;
    public void StartDissolve(bool flag) => startDissolve = flag;
    public bool DissolveEnd() => !startDissolve;
    public void SetBurnSpeed(float burnSpeed) => BurnSpeed = burnSpeed;
    public CardState[] cardStates;
    private void Awake()
    {
        card = GetComponent<Transform>();
        playerInput = GetComponentInParent<CardInput>();
        cardController = GetComponentInParent<CardController>();
        baseCard = GetComponent<BaseCard>();
        statetable = new Dictionary<System.Type, ICardState>(cardStates.Length);

        foreach (CardState state in cardStates)
        {
            state.Init(this, card, playerInput, cardController, baseCard);
            statetable.Add(state.GetType(), state);
        }
    }
    private void Start()
    {
        Color currentColor = card.GetComponent<Image>().color;
        currentColor.a = 0;
        card.GetComponent<Image>().color = currentColor;
        SwitchOn(statetable[typeof(CardState_Shuffle)]);
    }
    private void FixedUpdate()
    {
        if (unborn && startDissolve)
        {
            card.GetComponent<Image>().material = Resources.Load<Material>("Config/CardState/Custom_DissolveShader");
            material = card.GetComponent<Image>().material;
            unborn = false;
        }
        if (startDissolve && !unborn)
        {
            StartCoroutine(Dissolve());
            startDissolve = material.GetFloat("_BurnAmount") < 1;
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
}