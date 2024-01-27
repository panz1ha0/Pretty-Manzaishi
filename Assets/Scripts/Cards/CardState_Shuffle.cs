using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Data/CardState/Shuffle", fileName = "CardState_Shuffle")]
public class CardState_Shuffle: CardState
{
    private Vector3 currentPosition;
    private bool onPosition = false;

    [SerializeField] Vector3 position;
    [SerializeField] float speed;
    public override void Enter()
    {
        base.Enter();
        currentPosition = card.localPosition;
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        currentPosition = Vector3.MoveTowards(currentPosition, position, speed * Time.deltaTime);
        onPosition = currentPosition == position;
        if (onPosition)
        {
            Color currentColor = image.color;
            currentColor.a = 0;
            image.color = currentColor;
        }
        if (!stateMachine.termOver && stateMachine.unborn &&onPosition)
        {
            stateMachine.SwitchState(typeof(CardState_Idle));
        }
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        card.localPosition = currentPosition;
    }
}