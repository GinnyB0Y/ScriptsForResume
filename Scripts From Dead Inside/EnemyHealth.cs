using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] AudioSource enemyDeath;

    [SerializeField] int damage = 20;
    [SerializeField] int health = 60;

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
            StartCoroutine(Death());
    }
    IEnumerator Death()
    {
        yield return new WaitForSeconds(0f);
        animator.Play("EnemyDeath");
        enemyDeath.Play();
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
