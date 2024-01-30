using UnityEngine;

public class ClashState : GameState
{
    private Animator playerAnimator;
    private Animator enemyAnimator;

    public ClashState(Animator playerAnim, Animator enemyAnim)
    {
        playerAnimator = playerAnim;
        enemyAnimator = enemyAnim;
    }

    public override void EnterState(StateManager manager)
    {
    }

    public override void ExitState(StateManager manager)
    {
    }

    public override void UpdateState(StateManager manager)
    {
    }

    public void HandleClashSignal(string signal)
    {
        switch (signal)
        {
            case "Single":
                playerAnimator.Play("Player Clash Single");
                enemyAnimator.Play("Enemy Clash Single");
                break;
            case "Double":
                playerAnimator.Play("Player Double");
                enemyAnimator.Play("Enemy Clash Double");
                break;
            case "Hold":
                playerAnimator.Play("Player Hold");
                enemyAnimator.Play("Enemy Clash Hold");
                break;
        }
    }
}
