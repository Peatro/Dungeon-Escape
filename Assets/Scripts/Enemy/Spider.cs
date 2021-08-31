using System.Collections;
using UnityEngine;

public class Spider : Enemy, IDamageable
{
    [SerializeField] private GameObject _acidPrefab;
    [SerializeField] private float _fireRate;
    private bool _canAttack = true;

    protected override void Init()
    {
        base.Init();        
    }

    protected override void Update()
    {
        if (_canAttack && isDead == false)
        {
            anim.SetTrigger("Attack");
            StartCoroutine(AttackCoroutine());
        }
    }

    protected override void Movement()
    {
        //sit still
    }
     
    public override void Damage()
    {
        if (isDead == false)
        {
            Debug.Log("Spider::Damage()");
            health--;

            if (health <= 0)
            {
                anim.SetTrigger("Death");
                isDead = true;
                GameObject diamond = Instantiate(diamondPrefab, transform.position, Quaternion.identity) as GameObject;
                diamond.GetComponent<Diamond>().gems = base.gems;
            }
        }        
    }

    public void Attack()
    {
        if (_acidPrefab != null)
        {
            Instantiate(_acidPrefab, transform.position, Quaternion.identity);
        }
    }

    IEnumerator AttackCoroutine()
    {
        _canAttack = false;
        yield return new WaitForSeconds(_fireRate);
        _canAttack = true;
    }
}
