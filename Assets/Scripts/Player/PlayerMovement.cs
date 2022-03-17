using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerMovement : MonoBehaviour
{
    public float jumpHeight = 3.5f;
    public float gravity = 40;
    public Animator animator;
    public AudioClip JumpAudioClip;
    public AudioClip DeathAudioClip;
    public delegate void EventHandler(Guid groundHitID);
    public event EventHandler PlayerLandedEvent;
    public delegate void GameOverEventHandler();
    public event GameOverEventHandler GameOverEvent;

    private float _jumpVerticalVelocity;
    private float _currentVerticalVelocity;
    private int _collisionCount;
    private float _gameSpeed = 1;
    private PlayerState _ps = PlayerState.InAir;
    private static readonly int PlayerOnGround = Animator.StringToHash("PlayerOnGround");
    private static readonly int PlayerJumping = Animator.StringToHash("PlayerJumping");
    private static readonly int PlayerFalling = Animator.StringToHash("PlayerFalling");

    private static readonly int GameSpeedAmplifier = Animator.StringToHash("GameSpeedAmplifier");

    private enum PlayerState
    {
        OnGround,InAir
    }

    private void Start()
    {
        _jumpVerticalVelocity = (float)Math.Sqrt(2 * gravity * jumpHeight);
        _gameSpeed = GameSpeed.GS.GetGameSpeed();
        GameSpeed.GS.GameSpeedChangedEvent += GameSpeedChanged;
        GetComponent<OffScreenable>().OffScreenEvent += OnOffScreen;
    }

    private void FixedUpdate()
    {
        HandleGravity();
    }

    void Update()
    {
        HandlePlayerAnimation();
    }

    private void GameSpeedChanged(float newGameSpeed)
    {
        _gameSpeed = newGameSpeed;
        animator.SetFloat(GameSpeedAmplifier,newGameSpeed);
    }

    private void OnOffScreen()
    {
        OnGameOver();
    }

    protected virtual void OnGameOver()
    {
        GameOverEvent?.Invoke();
        GetComponent<AudioSource>().PlayOneShot(DeathAudioClip);
        StartCoroutine(ExecuteAfterTime(0.75f));
        IEnumerator ExecuteAfterTime(float time)
        {
            yield return new WaitForSeconds(time);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void HandleGravity()
    {
        var deltaTime = Time.deltaTime * _gameSpeed;
        switch (_ps)
        {
            case PlayerState.OnGround:
                break;
            case PlayerState.InAir:
                _currentVerticalVelocity -= gravity * deltaTime;
                break;

        }
        
        //prevent player from skipping through ground at high speeds
        if (_currentVerticalVelocity < 0)
        {
            var hit = Physics2D.Raycast(transform.position + Vector3.down * 0.5f, Vector3.down,
                -1 * _currentVerticalVelocity * deltaTime, ~(1 << 3));
            if(hit)
            {
                transform.position += Vector3.down * hit.distance;
            }else
            {
                transform.position += Vector3.up * _currentVerticalVelocity * deltaTime;
            }
        }
        else
        {
            var hit = Physics2D.Raycast(transform.position + Vector3.up * 0.5f, Vector3.up,
                _currentVerticalVelocity * deltaTime, ~(1 << 3));
            if(hit)
            {
                transform.position += Vector3.up * hit.distance;
            }else
            {
                transform.position += Vector3.up * _currentVerticalVelocity * deltaTime;
            }
        }

    }

    private void HandlePlayerAnimation()
    {
        switch (_ps)
        {
            case PlayerState.InAir:
            {
                animator.SetBool(PlayerOnGround, false);
                if (_currentVerticalVelocity >= 0)
                {
                    animator.SetBool(PlayerJumping, true);
                    animator.SetBool(PlayerFalling, false);
                }
                else
                {
                    animator.SetBool(PlayerJumping, false);
                    animator.SetBool(PlayerFalling, true);
                }

                break;
            }
            case PlayerState.OnGround:
                animator.SetBool(PlayerOnGround, true);
                animator.SetBool(PlayerJumping, false);
                animator.SetBool(PlayerFalling, false);
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        var initialPlayerState = _ps;
        _collisionCount++;

        var contacts = new ContactPoint2D[col.contactCount];
        col.GetContacts(contacts);
        
        //todo eject player sideways onGroundSideTouch
        if (contacts.Any(contact => Vector2.Angle(contact.normal, Vector2.down) < 45f))//headbutt
        {
            _currentVerticalVelocity = 0;
        }
        else if (contacts.Any(contact => Vector2.Angle(contact.normal, Vector2.up) < 45f))//land
        {
            _ps = PlayerState.OnGround;
            if (initialPlayerState == PlayerState.InAir && _ps == PlayerState.OnGround)
            {
                PlayerLanded(col.gameObject.GetComponent<GroundMovement>().GetID());
            }

            _currentVerticalVelocity = 0;
        }
    }

    private void PlayerLanded(Guid groundHitID)
    {
        PlayerLandedEvent?.Invoke(groundHitID);
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (--_collisionCount == 0)
        {
            _ps = PlayerState.InAir;
        }
    }

    public void Jump()//todo: this code is vulnerable to multiple executions at the same time
    {
        if (_ps != PlayerState.OnGround) return;
        _currentVerticalVelocity = _jumpVerticalVelocity;
        GetComponent<AudioSource>().PlayOneShot(JumpAudioClip);
    }
}
