using System.Collections;
using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Data/CardState/Casted", fileName = "CardState_Casted")]
public class CardState_Casted : CardState
{
    [SerializeField] float DissolveSpeed;
    [SerializeField] Vector3 position;
    [SerializeField] float Scale;
    public override void Enter()
    {
        base.Enter();
        stateMachine.SetBurnSpeed(DissolveSpeed);
        card.localScale = new Vector3(Scale, Scale, Scale);
        stateMachine.StartDissolve(true);
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (stateMachine.termOver && !stateMachine.unborn)
        {
            if (stateMachine.DissolveEnd())
            {
                cardController.MarkUsedCard(baseCard.rakugoData.Id);
                stateMachine.SwitchState(typeof(CardState_Shuffle));
            }
        }
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}