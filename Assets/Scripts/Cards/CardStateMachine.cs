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
        SwitchOn(statetable[typeof(CardState_Shuffle)]);
    }
    private void FixedUpdate()
    {
        if (unborn && startDissolve)
        {
            image.material = Resources.Load<Material>("Config/CardState/Custom_DissolveShader");
            material = image.material;
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