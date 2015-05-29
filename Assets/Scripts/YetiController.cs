using UnityEngine;
using System.Collections;

public class YetiController : MonoBehaviour
{
    private Rigidbody2D _rigidBody;
    private Animator _animator;

    private int health;
    //sprivate bool _jump;
    //private bool _fire;
    private bool _grounded;
    private bool _dead;
    private GameController _gameController;

    [SerializeField]
    private GameObject _weapon;
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
            //If we are scaled to 1, ensure we fly to left.
            //if we are scaled to -1, we are now facing right, ensure projectile goes to right.
            //var projectilePosition = transform.position;

            ////Move the projectile over just a bit when creating it.
            //projectilePosition.x += 2f;
            //projectilePosition.y -= .75f;

            //var projectile = Instantiate(_weapon, projectilePosition, Quaternion.identity);
            
            ////Make our projectile fit the scale (ie rotation since we're changing scale to -1 to 
            ////rotate our object) we have
            //if (transform.localScale.x == -1)
            //{
            //    var temp = ((GameObject)projectile).transform.localScale;
            //    temp.x = -1;
            //    ((GameObject)projectile).transform.localScale = temp;;
            //}
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {

        //Did we fall off the world?
        if (collider.gameObject.tag == "Border")
        {
            StartCoroutine(CoVampireDie());
            _dead = true;
        }

        if (collider.gameObject.tag == "Zombie")
        {
            //What to do when we hit the zombie?
        }
    }
    

    IEnumerator CoVampireDie()
    {
        yield return new WaitForSeconds(2);

        _rigidBody.isKinematic = true;
        _gameController.PlayerDied();
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
}
