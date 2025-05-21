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
        // ��Ʈ�ڽ��� Ȱ��ȭ�� ������ '�̹� ���� ��' ����� �ʱ�ȭ
        if (hitEnemy == null)
        {
            hitEnemy = new List<Collider>();
        }
        hitEnemy.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hitEnemy.Contains(other)) // "Player" �±״� ����
        {
            return;
        }
        // ��: EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
        // if (enemyHealth != null)
        // {
        //     enemyHealth.TakeDamage(damage);
        // }

        hitEnemy.Add(other);
    }

}
