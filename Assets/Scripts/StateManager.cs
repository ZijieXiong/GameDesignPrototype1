using UnityEngine;

public abstract class GameState
{
    public abstract void EnterState(StateManager manager);
    public abstract void ExitState(StateManager manager);
    public abstract void UpdateState(StateManager manager);
}

public class PlayerAttackState : GameState
{
    public override void EnterState(StateManager manager)
    {
        Debug.Log("Entering Player Attack State");
    }

    public override void ExitState(StateManager manager)
    {
        Debug.Log("Exiting Player Attack State");
    }

    public override void UpdateState(StateManager manager)
    {
    }
}

public class EnemyAttackState : GameState
{
    public override void EnterState(StateManager manager)
    {
        Debug.Log("Entering Enemy Attack State");
    }

    public override void ExitState(StateManager manager)
    {
        Debug.Log("Exiting Enemy Attack State");
    }

    public override void UpdateState(StateManager manager)
    {
    }
}

public class ClashState : GameState
{
    public override void EnterState(StateManager manager)
    {
        Debug.Log("Entering Clash State");
    }

    public override void ExitState(StateManager manager)
    {
        Debug.Log("Exiting Clash State");
    }

    public override void UpdateState(StateManager manager)
    {
    }
}

public class StateManager : MonoBehaviour
{
    private GameState currentState;

    void Start()
    {
        TransitionToState(new PlayerAttackState());
    }

    public void TransitionToState(GameState newState)
    {
        currentState?.ExitState(this);
        currentState = newState;
        newState.EnterState(this);
    }

    void Update()
    {
        currentState?.UpdateState(this);
    }

    public void ReceiveSignal(string signal)
    {
        switch (signal)
        {
            case "playerAttack":
                TransitionToState(new PlayerAttackState());
                break;
            case "enemyAttack":
                TransitionToState(new EnemyAttackState());
                break;
            case "clash":
                TransitionToState(new ClashState());
                break;
            default:
                Debug.LogError("Unknown signal received");
                break;
        }
    }
}
