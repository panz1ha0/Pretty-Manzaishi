using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using Unity.VisualScripting;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Data/CardState/Idle", fileName = "CardState_Idle")]
public class CardState_Idle : CardState
{
    private Vector3 currentPosition;
    private Material material;
    private bool onPosition = false;

    [SerializeField] Vector3 position;
    [SerializeField] float speed;
    public override void Enter()
    {
        base.Enter();
        Color currentColor = card.GetComponent<Image>().color;
        currentColor.a = 100;
        card.GetComponent<Image>().color = currentColor;
        material = card.GetComponent<Image>().material;
        card.localScale = Vector3.one;
        currentPosition = card.localPosition;

        material.SetFloat("_BurnAmount", 0);
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        onPosition = currentPosition == position;
        if(onPosition && stateMachine.isPreviewing)
        {
            stateMachine.SwitchState(typeof(CardState_Preview));
        }
        if (stateMachine.termOver && stateMachine.unborn)
        {
            stateMachine.SwitchState(typeof(CardState_Shuffle));
        }
        currentPosition = Vector3.MoveTowards(currentPosition, position, speed * Time.deltaTime);
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        card.localPosition = currentPosition;
    }
}