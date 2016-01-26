using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;


public class ZombieController : MonoBehaviour
{
    //This attribute allows a private variable to be 
    //visible in the Editor
    [SerializeField]
    private int _health = 3;
    private bool _isDead;
    private bool _isReadyToMove;
    private Animator _animator;
    private GameController _gameController;
    private SpriteRenderer _spriteRenderer;
    [SerializeField]
    //TODO: recycle/object pool these
    private GameObject _hitPrefab = null;

    private Rigidbody2D _rigidBody;

    //When was the player last spotted? We'll use this so we walk for 
    //a minimum of two seconds when we spot the player.
    private DateTime _playerSpottedTime;
    private Vector2 _lastVelocity;

    // Use this for initialization
    void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.SetBool("Idle", true);

        //We use this to flash it red and to fade it away.
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _gameController = GameController.GetGameControllerInScene();
        _rigidBody = GetComponent<Rigidbody2D>();

        StartCoroutine(ActivateMe());
    }

    IEnumerator ActivateMe()
    {
        //Zombies can't track the main player for a min of three seconds
        yield return new WaitForSeconds(Random.Range(3, 7));
        _isReadyToMove = true;
    }

    public void GotHit()
    {
        if (_isDead) return;

        //Kick off a separate task to flash the zombie
        StartCoroutine(FlashZombieRed());
        //play 'hit' animation.
        _animator.SetTrigger("GotHit");
        _health -= 1;
        Instantiate(_hitPrefab, transform.position, Quaternion.identity);
        if (_health <= 0)
        {
            KillZombie();
        }
    }

    /// <summary>
    /// IEnumerators mean we can call these methods and continue execution from the caller.
    /// Great, normal methods do that you say. Yes, but here we can continue execution and
    /// sleep when we want and resume, while the caller continues on their merry way.
    /// </summary>
    /// <returns></returns>
    IEnumerator FlashZombieRed()
    {
        //Applies a 'flash' to a red color

        //Swap out the color for a damage color
        _spriteRenderer.material.color = Color.red;
        yield return new WaitForSeconds(.2f);
        //restore the color
        _spriteRenderer.material.color = Color.white;

    }
    public void KillZombie()
    {
        if (_isDead) return;
        _isDead = true;
        //if health<=0, state is dead and play 'die'
        //The cleaner way here is to transition between states - ie Zombie.ChangeState(ZombieState.Dead)
        //and that would be in charge of stopping other animations in progress, but this is a bit simpler for lab purposes.
        _animator.SetBool("Die", true);
        _animator.SetBool("Walk", false);
        _animator.SetBool("Idle", false);

        //Zombie can no longer get hit, he's dead jim.
        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;

        //Prevent detecting any collisions now - again, he's dead jim.
        GetComponent<Rigidbody2D>().isKinematic = true;

        //Kick off a separate task to fade the zombie away over two seconds
        StartCoroutine(FadeAway());
    }

    IEnumerator FadeAway()
    {
        yield return new WaitForSeconds(1);
        while (_spriteRenderer.color.a > 0)
        {
            var color = _spriteRenderer.color;
            //color.a is 0 to 1. So .5*time.deltaTime will take 2 seconds to fade out
            color.a -= (.5f * Time.deltaTime);

            _spriteRenderer.color = color;
            //wait for a frame
            yield return null;
        }
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "rocket")
        {
            _gameController.IncrementPoints(10);
            Destroy(collision.gameObject);
            GotHit();
        }
    }


    void FixedUpdate()
    {
        bool foundPlayer = false;
        //I know, are zombies _really_ dead?
        if (_isDead) return;

        //Don't start moving until we've signaled to ourselves its time to go!
        if (!_isReadyToMove) return;


        //Now, let's see if we should move closer and closer to main player.

        //Check for the main playeer. We are using a layer mask to ONLY look for objects assigned to layer 8 (the Player Layer)
        //that was custom defined in the Layers dropdown. You can see an example of this here: http://docs.unity3d.com/Manual/Layers.html
        //This is a bit mask saying layer 8 is set ex a byte has 8 values 00000000 and 1<<8 becomes 10000000.
        //The player is the only object on this layer, hence the only thing we'll hit.
        var hitLeft = Physics2D.Raycast(transform.position, Vector2.left, Mathf.Infinity, (1 << 8));

        if (hitLeft.collider != null)
        {
            //A player was found to the left of us

            //flip character to look to the left, since normally a zombie faces to the right
            if (transform.localScale.x != -1f)
            {
                var scale = transform.localScale;
                scale.x = -1;
                transform.localScale = scale;
            }
            //The left is a vector with values -1,0,0. Multiply by speed and a smoothing value Time.deltaTime
            //so no matter the framerate, we'll move at 75 units a second
            //Also, store the last velocity as we'll keep the zombie moving for two seconds even when we don't see the player

            //Note on using TransformDirection - technically we could move to the left without using TransformDirection. It's a better
            //practice though to use it, as it will always make a character move to ITS left regardless of where it is or rotated
            //There's the concept of world left (think of a grid that never moves) and characters that can move throughout this world.
            //do we want to move to the world's left - or make the player move to its left. This becomes more of a concern in 3d
            //but a good practice to get used to.
            _lastVelocity = transform.TransformDirection(-1 * Time.deltaTime * 75, 0, 0);
            //the above could be written as transform.TransformDirection(Vector3.left * Time.deltaTime * 75);



            _rigidBody.velocity = _lastVelocity;
            foundPlayer = true;
        }
        else
        {
            //Didn't find on the left, now check the right
            var hitRight = Physics2D.Raycast(transform.position, Vector2.right, Mathf.Infinity, (1 << 8));
            if (hitRight.collider != null)
            {
                //Found player to the right

                //Flip towards player if necessary
                if (transform.localScale.x != 1f)
                {
                    var scale = transform.localScale;
                    scale.x = 1;
                    transform.localScale = scale;
                }
                //Move the zombie
                _lastVelocity = transform.TransformDirection(1 * Time.deltaTime * 75, 0, 0);
                _rigidBody.velocity = _lastVelocity;
                foundPlayer = true;
            }
        }
        if (foundPlayer)
        {
            _playerSpottedTime = DateTime.Now;
        }
        else
        {
            //We didn't find the player. If it's been < 2 seconds since we've seen the player, pretend we still see him.
            if (_playerSpottedTime != DateTime.MinValue &&
                    DateTime.Now.Subtract(_playerSpottedTime).TotalSeconds < 2)
            {
                foundPlayer = true;
                //Just keep moving in the direction we were previously moving in.
                _rigidBody.velocity = _lastVelocity;
            }
        }

        if (foundPlayer)
        {
            _animator.SetBool("Walk", true);
            _animator.SetBool("Idle", false);
        }
        else
        {
            //Go idle, no player found
            _animator.SetBool("Idle", true);
            _animator.SetBool("Walk", false);
        }
    }
    
}
