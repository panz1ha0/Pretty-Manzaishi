using UnityEngine;

[CreateAssetMenu(menuName = "Data/CardState/Drag", fileName = "CardState_Drag")]
public class CardState_Drag : CardState
{
    public override void Enter()
    {
        base.Enter();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (playerInput.releaseCard && card.localPosition.y <= -100)
        {
            stateMachine.SwitchState(typeof(CardState_Idle));
        }
        if (playerInput.releaseCard && card.localPosition.y > -100)
        {
            stateMachine.SwitchState(typeof(CardState_Casted));
        }
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        card.position = Input.mousePosition;
    }
}