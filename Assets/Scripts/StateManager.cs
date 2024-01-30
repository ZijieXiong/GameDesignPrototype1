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
        playerAnimator = GameObject.Find("Player Body").GetComponent<Animator>();
        enemyAnimator = GameObject.Find("Enemy Body").GetComponent<Animator>();
        if (playerAnimator == null || enemyAnimator == null)
        {
            Debug.LogError("Animator references not set in StateManager");
            return;
        }
        TransitionToState(new PlayerAttackState(playerAnimator, enemyAnimator));
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
                TransitionToState(new PlayerAttackState(playerAnimator, enemyAnimator));
                break;
            case "enemyAttack":
                TransitionToState(new EnemyAttackState(playerAnimator, enemyAnimator));
                break;
            case "clash":
                TransitionToState(new ClashState(playerAnimator, enemyAnimator));
                break;
            default:
                Debug.LogError("Unknown signal received");
                break;
        }
    }
}