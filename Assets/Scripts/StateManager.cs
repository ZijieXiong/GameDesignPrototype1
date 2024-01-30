using UnityEngine;

public abstract class GameState
{
    public abstract void EnterState(StateManager manager);
    public abstract void ExitState(StateManager manager);
    public abstract void UpdateState(StateManager manager);
}

public class StateManager : MonoBehaviour
{
    private GameState currentState;
    private Animator playerAnimator;
    private Animator enemyAnimator;

    void Start()
    {
        playerAnimator = GameObject.Find("Player Body 1 1").GetComponent<Animator>();
        enemyAnimator = GameObject.Find("Enemy Body 1 1").GetComponent<Animator>();
        if (playerAnimator == null || enemyAnimator == null)
        {
            Debug.LogError("Animator references not set in StateManager");
            return;
        }
        //TransitionToState(new PlayerAttackState(playerAnimator, enemyAnimator));
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
        ClashState clashState;

        switch (signal)
        {
            case "0":
                clashState = new ClashState(playerAnimator, enemyAnimator);
                TransitionToState(clashState);
                clashState.HandleClashSignal("Single");
                break;
            case "1":
                clashState = new ClashState(playerAnimator, enemyAnimator);
                TransitionToState(clashState);
                clashState.HandleClashSignal("Hold");
                break;
            case "2":
                clashState = new ClashState(playerAnimator, enemyAnimator);
                TransitionToState(clashState);
                clashState.HandleClashSignal("Double");
                break;
            case "playerAttack":
                TransitionToState(new PlayerAttackState(playerAnimator, enemyAnimator));
                break;
            case "enemyAttack":
                TransitionToState(new EnemyAttackState(playerAnimator, enemyAnimator));
                break;
            default:
                Debug.LogError("Unknown signal received");
                break;
        }
    }
}
