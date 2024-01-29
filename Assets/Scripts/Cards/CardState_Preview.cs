using UnityEngine;

[CreateAssetMenu(menuName = "Data/CardState/Preview", fileName = "CardState_Preview")]
public class CardState_Preview : CardState
{
    private Vector3 currentPosition;
    private float targetPositionY;
    //private Canvas canvas;
    const float PREVIEW_SCALE = 1.5f;

    [SerializeField] float upmove;
    [SerializeField] float speed;
    public override void Enter()
    {
        base.Enter();
        AudioManager.Instance.PlaySFX("OnHoverCard");
        cardController.CanPreView = false;
        //canvas = card.GetComponent<Canvas>();
        //canvas.sortingOrder = 1;
        cardController.SetDetailedPanel(baseCard.rakugoData);
        image.transform.localScale = new Vector3(PREVIEW_SCALE, PREVIEW_SCALE, PREVIEW_SCALE);
        content.rectTransform.localScale = new Vector3(PREVIEW_SCALE, PREVIEW_SCALE, PREVIEW_SCALE);
        currentPosition = card.localPosition;
        targetPositionY = currentPosition.y + upmove;
    }
    public override void Exit()
    {
        base.Exit();
        cardController.SetDetailedPanel();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        //currentPosition.y = Mathf.MoveTowards(currentPosition.y, targetPositionY, speed * Time.deltaTime);
        if (!stateMachine.isPreviewing)
        {
            cardController.CanPreView = true;
            stateMachine.SwitchState(typeof(CardState_Idle));
        }
        if (playerInput.dragCard)
        {
            stateMachine.SwitchState(typeof(CardState_Drag));
        }
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        card.localPosition = new Vector3(currentPosition.x, targetPositionY);
    }
}