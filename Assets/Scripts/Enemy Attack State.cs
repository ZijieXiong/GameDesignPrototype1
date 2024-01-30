using UnityEngine;
using System.Collections;

public class EnemyAttackState : GameState
{
    private Animator playerAnimator;
    private Animator enemyAnimator;
    private int attackCounter = 0;

    public EnemyAttackState(Animator playerAnim, Animator enemyAnim)
    {
        playerAnimator = playerAnim;
        enemyAnimator = enemyAnim;
    }

    public override void EnterState(StateManager manager)
    {
        attackCounter++;
        manager.StartCoroutine(AttackSequence());
    }

    public override void ExitState(StateManager manager)
    {
    }

    public override void UpdateState(StateManager manager)
    {
    }

    private IEnumerator AttackSequence()
    {
        
        enemyAnimator.Play("Enemy Attack");

        // Wait for half a second
        yield return new WaitForSeconds(0.5f);

        // Play "Player Hurt" animation
        playerAnimator.Play("Player Hurt");

        // Play "Player Health" animations based on the attackCounter
        if (attackCounter == 1)
        {
            playerAnimator.Play("Player Health 1");
        }
        else if (attackCounter == 2)
        {
            playerAnimator.Play("Player Health 1");
            playerAnimator.Play("Player Health 2");
        }
        else if (attackCounter >= 3)
        {
            playerAnimator.Play("Player Health 1");
            playerAnimator.Play("Player Health 2");
            playerAnimator.Play("Player Health 3");
        }
    }
}
