using System.Collections;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour, IDamageable
{
    private Rigidbody2D _playerRigid;
    private PlayerAnimation _playerAnim;
    private SpriteRenderer _playerSpriteRenderer;
    private SpriteRenderer _swordArcSpriteRenderer;
    private bool _resetJump = false;
    private bool _isDead = false;

    [SerializeField] private int _gems = 0;
    [SerializeField] private bool _grounded;
    [SerializeField] private float _speed = 5.0f;
    [SerializeField] private float _jumpForce = 6.4f;
    [SerializeField] private int _health = 4;

    public int Health { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        _playerRigid = GetComponent<Rigidbody2D>();
        _playerAnim = GetComponent<PlayerAnimation>();
        _playerSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _swordArcSpriteRenderer = transform.GetChild(1).GetComponent<SpriteRenderer>();
        Health = _health;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isDead)
            return;
        Movement();
        Attack();
    }

    void Attack()
    {
        if (CrossPlatformInputManager.GetButtonDown("A_Button") && _grounded == true)
        {
            _playerAnim.Attack();
        }
    }

    void Movement()
    {
        float move = CrossPlatformInputManager.GetAxisRaw("Horizontal");

        SpriteFlip(move);
        _grounded = IsGrounded();

        if ((Input.GetButtonDown("Jump") || CrossPlatformInputManager.GetButtonDown("B_Button")) && _grounded == true)
        {
            _playerRigid.velocity = new Vector2(_playerRigid.velocity.x, _jumpForce);
            StartCoroutine(JumpCoroutine());
            _playerAnim.Jump(true);
        }
        
        _playerRigid.velocity = new Vector2(move * _speed, _playerRigid.velocity.y);
        _playerAnim.Move(move);       
    }

    bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.6f, 1 << 8);

        if (hit.collider != null)
        {
            if (_resetJump == false)
            {
                _playerAnim.Jump(false);
                return true;
            }
        }
        return false;
    }

    void SpriteFlip(float direction)
    {
        if (direction == 0)
            return;
        if (direction > 0)
        {
            _playerSpriteRenderer.flipX = false;
            _swordArcSpriteRenderer.flipX = false;
            _swordArcSpriteRenderer.flipY = false;

            Vector3 newPos = _swordArcSpriteRenderer.transform.localPosition;
            newPos.x = 1.01f;
            _swordArcSpriteRenderer.transform.localPosition = newPos;
        }
        else
        {
            _playerSpriteRenderer.flipX = true;
            _swordArcSpriteRenderer.flipX = true;
            _swordArcSpriteRenderer.flipY = true;

            Vector3 newPos = _swordArcSpriteRenderer.transform.localPosition;
            newPos.x = -1.01f;
            _swordArcSpriteRenderer.transform.localPosition = newPos;
        }
    }

    public void Damage()
    {
        if (_isDead)
            return;

        Debug.Log("Player::Damage()");
        Health--;
        _playerAnim.Hit();
        UIManager.Instance.UpdateLives(Health);

        if (Health <= 0)
        {
            _isDead = true;
            _playerAnim.Death();
        }
    }

    public void GemsCollected(int amount)
    {
        _gems += amount;
        UIManager.Instance.UpdateGemCount(_gems);
    }

    public int GemsCount()
    {
        return _gems;
    }

    public void Purchase(int price)
    {
        _gems -= price;
        UIManager.Instance.UpdateGemCount(_gems);
        UIManager.Instance.OpenShop(_gems);
    }

    IEnumerator JumpCoroutine()
    {
        _resetJump = true;
        yield return new WaitForSeconds(0.1f);
        _resetJump = false;
    }
}
