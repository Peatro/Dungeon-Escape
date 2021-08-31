using UnityEngine;

public abstract class Enemy : MonoBehaviour, IDamageable
{
    public int Health { get; set; }

    [SerializeField] protected int health;
    [SerializeField] protected float speed;
    [SerializeField] protected int gems;
    [SerializeField] protected Transform pointA, pointB;
    [SerializeField] protected GameObject diamondPrefab;

    protected Vector3 currentTarget;
    protected Animator anim;
    protected SpriteRenderer sprite;
    protected Player player;
    
    protected bool isDead = false;

    protected virtual void Init()
    {
        anim = GetComponentInChildren<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        Health = health;
    }

    protected void Start()
    {
        Init();
    }

   protected virtual void Update()
    {
        if (isDead == false)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                CombatChecker();
                return;
            }
            if (anim.GetBool("InCombat") == true)
            {
                AttackDirection();
                return;
            }

            Movement();
        }        
    }

    protected virtual void Movement()
    {
        SpriteFlip();

        if (transform.position == pointA.position)
        {
            anim.SetTrigger("Idle");
            currentTarget = pointB.position;
        }
        else if (transform.position == pointB.position)
        {
            anim.SetTrigger("Idle");
            currentTarget = pointA.position;
        }

        transform.position = Vector3.MoveTowards(transform.position, currentTarget, speed * Time.deltaTime);
    }

    public virtual void Damage()
    {
        if (isDead == false)
        {
            Debug.Log("Current health : " + Health);
            Health--;
            anim.SetTrigger("Hit");
            anim.SetBool("InCombat", true);

            if (Health <= 0)
            {
                isDead = true;
                anim.SetTrigger("Death");
                GameObject diamond = Instantiate(diamondPrefab, transform.position, Quaternion.identity) as GameObject;
                diamond.GetComponent<Diamond>().gems = gems;
            }
        }        
    }

    protected virtual void SpriteFlip()
    {
        if (currentTarget == pointA.position)
        {
            sprite.flipX = true;
        }
        else if (currentTarget == pointB.position)
        {
            sprite.flipX = false;
        }
    }
    protected virtual void CombatChecker()
    {
        float distance = Vector3.Distance(transform.localPosition, player.transform.localPosition);
        if (distance > 5.0f)
        {
            anim.SetBool("InCombat", false);
        }
    }

    protected virtual void AttackDirection()
    {
        Vector3 attackDirection = player.transform.localPosition - transform.localPosition;

        if (attackDirection.x > 0)
        {
            sprite.flipX = false;
        }
        else if (attackDirection.x < 0)
        {
            sprite.flipX = true;
        }
    }
}
