using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlundererCombat : MonoBehaviour
{
    private Animator anim;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public static bool isAttacking;
    private bool coroutineStarted;
    public GameObject plundererParent;

    private void Start()
    {
        isAttacking = false;
        coroutineStarted = false;
        anim = this.GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && !anim.GetCurrentAnimatorStateInfo(0).IsTag("attack"))
        {
            //Debug.Log(isAttacking);
            isAttacking = true;
            Attack();
        }
    }

    private void Attack()
    {
        isAttacking = true;
        StartCoroutine(MoveAfterAttackRoutine());
        //play attack animation
        anim.SetTrigger("attack");

        //detect all enemies in range of attack
        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        //Debug.Log(enemiesHit.Length);
        //loop over enemies in array and add damage to each
        foreach(Collider2D enemy in enemiesHit)
        {
            Destroy(enemy.gameObject);
            Debug.Log("HIT ACHIEVED!");
        }
    }

    IEnumerator MoveAfterAttackRoutine()
    {
        yield return new WaitForSeconds(.5f);
        if(!anim.GetCurrentAnimatorStateInfo(0).IsTag("attack"))
        {
            isAttacking = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if(attackPoint != null)
        {
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
        else
        {
            return;
        }
    }
}
