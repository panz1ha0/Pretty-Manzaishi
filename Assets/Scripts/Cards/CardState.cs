using TMPro;
using UnityEngine;
using UnityEngine.UI;

public interface ICardState
{
    void Enter();
    void Exit();
    void LogicUpdate();
    void PhysicsUpdate();
}

public class CardState : ScriptableObject, ICardState
{
    protected CardStateMachine stateMachine;
    protected Transform card;
    protected Image image;
    protected TMP_Text content;
    protected CardInput playerInput;
    protected CardController cardController;
    protected BaseCard baseCard;
    public void Init(CardStateMachine stateMachine, Transform card, Image image, TMP_Text content, CardInput playerInput, CardController cardController, BaseCard baseCard)
    {
        this.stateMachine = stateMachine;
        this.card = card;
        this.image = image;
        this.content = content;
        this.playerInput = playerInput;
        this.cardController = cardController;
        this.baseCard = baseCard;
    }
    public virtual void Enter()
    {
        //Debug.Log(card.gameObject.name + " enter state: " + this);
    }

    public virtual void Exit()
    {
        stateMachine.SetLastState(this);
    }

    public virtual void LogicUpdate()
    {
        
    }

    public virtual void PhysicsUpdate()
    {
        
    }
}