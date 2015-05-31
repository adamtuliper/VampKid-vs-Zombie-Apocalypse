using UnityEngine;
using System.Collections;

public class VampController : MonoBehaviour
{
    private Rigidbody2D _rigidBody;
    private Animator _animator;

    private int health;
    private bool _grounded;
    private bool _dead;
    private GameController _gameController;

    [SerializeField]
    private GameObject _batBurst;

    // Use this for initialization
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        _gameController = GameController.GetGameControllerInScene();

    }

    // Update is called once per frame
    void Update()
    {
        //reading key press or mouse input ? DO IT HERE
        //Reading Input.GetAxis? Here or FixedUpdate
        if (Input.GetButtonDown("Jump"))
        {
            //Edit/Project Settings/Input shows jump default is spacebar
            //_jump = Input.GetButtonDown("Jump");
            //We can prevent multiple jumps in a row via several ways:
            //1. we can limit our y position
            //2. we can limit how high we can apply a force
            //3. we can apply an edge collider to top of world
            //4. we can only jump when grounded
            //if (transform.position.y < 40)
            Debug.Log("Jumping..? Grounded:" + _grounded);
            if (_grounded)
            {
                _rigidBody.AddForce(new Vector2(0, 1200));
                _animator.SetTrigger("Jump");
                _grounded = false;
                _animator.SetBool("Grounded", false);
            }
        }

        if (Input.GetButtonDown("Fire1"))
        {
            //Edit/Project Settings/Input shows jump default is spacebar
            //_fire = Input.GetButtonDown("Fire1");
            _animator.SetTrigger("Attack");

            //Note that we could instantiate the projectile here, thats 
            //common when we shoot things out in Unity
            //var projectile = Instantiate(_weapon, projectilePosition, Quaternion.identity);
            //However, I'm using a feature of Unity 5 called a StateMachineBehavior and instead
            //calling code (in /Scripts/AttackStateMachineBehavior) from the vamp_run state 
            //on the vampire's animation controller.
        }
    }


    void FixedUpdate()
    {


        if (_dead)
        {
            return;
        }


        var horizontal = Input.GetAxis("Horizontal");
        var localScale = transform.localScale;

        //flips the character left if the input is < 0 and, right if >0 
        if (horizontal < 0)
        {
            //transform.rotation.y=180;
            //transform.localScale = -1;
            localScale.x = 1;
            // localScale is a Vector 3, which means it contains x,y,z
        }
        else if (horizontal > 0f)
        {
            //transform.localScale = 0;
            localScale.x = -1;
        }


        transform.localScale = localScale;

        if (horizontal != 0)
        {
            _animator.SetBool("Run", true);
        }
        else
        {
            _animator.SetBool("Run", false);
        }

        _rigidBody.velocity = new Vector2(horizontal * 20, _rigidBody.velocity.y);


    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "platform")
        {
            _grounded = true;
            _animator.SetBool("Grounded", true);

        }

    }

    /// <summary>
    /// I decided to use a trigger AND a collider for two reasons - and thats on the  ZOMBIE.
    /// The circle collider gives a nice movement across a surface, doesn't catch
    /// as many edges as a box collider would. So the CircleCollider2D on the zombie uses that
    /// An OnTriggerEnter2D will get called on both objects that get hit, so I'm only detecting
    /// if we run into a zombie from the player, as that's a game ending experience.
    /// 
    /// The circle collider if it was the size of the zombie is pretty big and causes collisions with our 
    /// rocket to happen outside our actual Zombie - they look weird. Now, with using a BoxCollider2D on the 
    /// zombie as our hit detection region, we can have a more realistic looking collision in a better boxy area
    /// since when we used the circle it was a lot bigger than our zombie's trim shape. IF it was a fat zombie
    /// it may have worked :)
    /// </summary>
    /// <param name="collision"></param>
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "zombie")
        {
            //What to do when we hit the zombie? We die!

            //Disable our physics
            _rigidBody.isKinematic = true;

            //Disable the vamp image
            GetComponent<SpriteRenderer>().enabled = false;

            //Enable the explosion particle effects. 
            _batBurst.SetActive(true);

            //While we're at it, let's kill the zombie we just hit :)
            //Fixes the silly issue of having the zombies have a 
            //rigidbody and a collider and they get pushed when we hit them.
            //We can get the zombie from collision.gameObject, then we just need its ZombieController.
            collision.gameObject.GetComponent<ZombieController>().KillZombie();

            //Tell the game controller we are all gone, let it decide what to do next.
            _gameController.PlayerDied();

        }
    }
}
