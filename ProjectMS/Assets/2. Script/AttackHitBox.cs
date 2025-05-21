using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

public class AttackHitBox : MonoBehaviour
{
    public int damage = 1;

    private List<Collider> hitEnemy;
    // Start is called before the first frame update

    void OnEnable()
    {
        // 히트박스가 활성화될 때마다 '이미 맞은 적' 목록을 초기화
        if (hitEnemy == null)
        {
            hitEnemy = new List<Collider>();
        }
        hitEnemy.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hitEnemy.Contains(other)) // "Player" 태그는 예시
        {
            return;
        }
        // 예: EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
        // if (enemyHealth != null)
        // {
        //     enemyHealth.TakeDamage(damage);
        // }

        hitEnemy.Add(other);
    }

}
