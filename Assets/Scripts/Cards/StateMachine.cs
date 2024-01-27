using System.Collections.Generic;
using UnityEngine;

public class StateMachine: MonoBehaviour
{
    protected ICardState currentState;
    protected Dictionary<System.Type, ICardState> statetable;
    
    public ICardState GetCurrentState() => currentState;

    private void Awake()
    {
        
    }
    private void Update()
    {
        currentState?.LogicUpdate();
        currentState?.PhysicsUpdate();
        //Debug.Log("currentState: " + currentState);
    }
    public void SwitchOn(ICardState state)
    {
        state.Enter();
        currentState = state;
    }
    public void SwitchState(ICardState state)
    {
        currentState?.Exit();
        SwitchOn(state);
    }
    public void SwitchState(System.Type type)
    {
        SwitchState(statetable[type]);
    }
}