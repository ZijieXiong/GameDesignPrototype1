using UnityEngine;
using System.Collections;

public class PlayerAttackState : GameState
{
    private Animator playerAnimator;
    private Animator enemyAnimator;
    private int attackCounter = 0;

    public PlayerAttackState(Animator playerAnim, Animator enemyAnim)
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
        playerAnimator.Play("Player Attack");
        yield return new WaitForSeconds(0.5f);
        enemyAnimator.Play("Enemy Hurt");

        if (attackCounter == 1)
        {
            enemyAnimator.Play("Enemy Health 1");
        }
        else if (attackCounter == 2)
        {
            enemyAnimator.Play("Enemy Health 1");
            enemyAnimator.Play("Enemy Health 2");
        }
        else if (attackCounter >= 3)
        {
            enemyAnimator.Play("Enemy Health 1");
            enemyAnimator.Play("Enemy Health 2");
            enemyAnimator.Play("Enemy Health 3");
        }
    }
}
