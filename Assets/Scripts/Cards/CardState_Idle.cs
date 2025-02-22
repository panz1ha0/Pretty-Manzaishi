using UnityEngine;

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
        content.gameObject.GetComponent<CanvasGroup>().alpha = 1;
        material = image.material;
        image.transform.localScale = Vector3.one;
        content.rectTransform.localScale = Vector3.one;
        currentPosition = card.localPosition;

        material.SetFloat("_BurnAmount", 0);
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        onPosition = currentPosition == position;
        if(onPosition && stateMachine.isPreviewing && cardController.CanPreView)
        {
            stateMachine.SwitchState(typeof(CardState_Preview));
        }
        if (stateMachine.turnEnd && stateMachine.unburn)
        {
            stateMachine.SwitchState(typeof(CardState_Shuffle));
        }
        
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (stateMachine.GetLastState().GetType() != typeof(CardState_Shuffle))
        {
            currentPosition = Vector3.MoveTowards(currentPosition, position, speed * Time.deltaTime);
            card.localPosition = currentPosition;
        }
        else
        {
            if (duration >= 1.5f)
            {
                currentPosition = Vector3.MoveTowards(currentPosition, position, speed * Time.deltaTime);
                card.localPosition = currentPosition;
            }
        }
    }
}