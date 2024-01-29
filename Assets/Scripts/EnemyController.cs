using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Animator bodyAnimator;

    void Start()
    {
        
        bodyAnimator = GetComponent<Animator>();
        
        
        if (bodyAnimator == null)
        {
            Debug.LogError("Missing Animator on Enemy Body");
        }
    }

    // Method to trigger specific animations
    public void TriggerAnimation(string animationTrigger)
    {
        if (bodyAnimator != null)
        {
            bodyAnimator.SetTrigger(animationTrigger);
        }
        else
        {
            Debug.LogError("Animator component not found on the enemy!");
        }
    }
}
/* 
public class GameManager : MonoBehaviour
{
    public EnemyController enemyController;

    public void SignalEnemyDoubleClash()
    {
        enemyController.TriggerAnimation("Enemy Clash Double");
    }

    public void SignalEnemyAttack()
    {
        enemyController.TriggerAnimation("Enemy Attack");
    }

    public void SignalEnemyHoldClash()
    {
        enemyController.TriggerAnimation("Enemy Clash Hold");
    }

    public void SignalEnemyHurt()
    {
        enemyController.TriggerAnimation("Enemy Hurt");
    }

    public void SignalEnemySingleClash()
    {
        enemyController.TriggerAnimation("Enemy Clash Single");
    }

    // ... other signals can be added similarly
}
*/
