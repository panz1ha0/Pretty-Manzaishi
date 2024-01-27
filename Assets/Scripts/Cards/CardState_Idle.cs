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
        Color currentColor = image.color;
        currentColor.a = 100;
        image.color = currentColor;
        Color textColor = content.color;
        textColor.a = 100;
        content.color = textColor;
        material = image.material;
        image.transform.localScale = Vector3.one;
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
        if (stateMachine.turnEnd && stateMachine.unburn)
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