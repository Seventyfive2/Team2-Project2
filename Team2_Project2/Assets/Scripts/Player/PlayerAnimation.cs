using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [Header("Animation Names")]
    [SerializeField] private string moveFoward;
    [SerializeField] private string moveBackward;
    [SerializeField] private string moveSideways;
    [Space]
    [SerializeField] private string attack;
    [SerializeField] private string special;
    [SerializeField] private string item;
    [Space]
    [SerializeField] private string takeDamage;
    [SerializeField] private string playerDeath;

    [Header("Setup")]
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private PlayerAttack combat;
    [SerializeField] private PlayerStats stats;

    void Start()
    {
        //animator = GetComponent<Animator>();
        //Player Movement
        movement.OnPlayerMove += Movement_OnPlayerMove;
        //Player Combat
        combat.OnPlayerAttack += Combat_OnPlayerAttack;
        combat.OnPlayerSpecial += Combat_OnPlayerSpecial;
        combat.OnPlayerItem += Combat_OnPlayerItem;
        //Player Health
        StartCoroutine(LateStart(.5f));
    }

    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        stats.GetHealthSystem().OnHealthChanged += PlayerAnimation_OnHealthChanged;
        stats.GetHealthSystem().OnDeath += PlayerAnimation_OnDeath;
    }

    private void Movement_OnPlayerMove(object sender, PlayerMovement.PlayerMoveEventArgs e)
    {
        switch (e.curDirection)
        {
            case PlayerMovement.PlayerMoveEventArgs.moveDirection.Foward:
            default:
                Debug.Log("playing " + moveFoward);
                animator.ResetTrigger("move");
                animator.SetTrigger("move");
                break;
            case PlayerMovement.PlayerMoveEventArgs.moveDirection.Backward:
                Debug.Log("playing " + moveBackward);
                break;
            case PlayerMovement.PlayerMoveEventArgs.moveDirection.Sideways:
                Debug.Log("playing " + moveSideways);
                break;
        }
    }

    #region Combat
    private void Combat_OnPlayerAttack(object sender, PlayerAttack.PlayerAttackEventArgs e)
    {
        if(!e.secondary)
        {
            //Primary Attack Animation
            Debug.Log("playing " + attack + " with " + e.weapon);
            animator.ResetTrigger("attack");
            animator.SetTrigger("attack");
        }
        else
        {
            //Secondary Attack Animation


        }

    }
    private void Combat_OnPlayerSpecial(object sender, PlayerAttack.PlayerSpecialEventArgs e)
    {
        Debug.Log("playing " + special + " with " + e.ability);
        animator.ResetTrigger("special");
        animator.SetTrigger("special");
    }
    private void Combat_OnPlayerItem(object sender, PlayerAttack.PlayerItemEventArgs e)
    {
        Debug.Log("playing " + item + " with " + e.item);
    }
    #endregion

    #region Player Take Damage
    private void PlayerAnimation_OnHealthChanged(object sender, System.EventArgs e)
    {
        Debug.Log("playing " + takeDamage);
    }
    private void PlayerAnimation_OnDeath(object sender, System.EventArgs e)
    {
        Debug.Log("playing " + playerDeath);
    }
    #endregion
}
