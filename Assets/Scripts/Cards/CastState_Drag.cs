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

    Vector3 m_Offset;
    Vector3 m_TargetScreenVec;

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        // Debug.Log($"{Input.mousePosition}, {Camera.main.ScreenToWorldPoint(Input.mousePosition)}, {Camera.main.ScreenToViewportPoint(Input.mousePosition)}");

        RectTransformUtility.ScreenPointToLocalPointInRectangle(cardController.rectTransform, Input.mousePosition, Camera.main, out var pos);
        card.GetComponent<RectTransform>().anchoredPosition = pos;

        // Debug.Log($"{pos}, {pos * 2}");

        // card.localPosition = card.GetComponentInParent<Transform>().InverseTransformPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        // m_TargetScreenVec = Camera.main.WorldToScreenPoint(card.transform.position);
        // m_Offset = card.transform.position - Camera.main.ScreenToWorldPoint(new Vector3
        //     (Input.mousePosition.x, Input.mousePosition.y, 1f));

        // while (Input.GetMouseButton(0))
        // {
        //     Vector3 res = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
        //         Input.mousePosition.y, 1f)) + m_Offset;
            
        //     card.transform.position = res;
        // }
    }
}