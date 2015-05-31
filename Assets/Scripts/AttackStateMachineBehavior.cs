using UnityEngine;

public class AttackStateMachineBehavior : StateMachineBehaviour
{
    public GameObject particle;
    private bool _launchedProjectile = false;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Debug.Log("OnStateEnter");
       
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Destroy(clone);
        _launchedProjectile = false;
        // Debug.Log("OnStateExit");
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //   var bip = animator.GetAnimatorTransitionInfo(layerIndex);
        if (stateInfo.normalizedTime > .1 && !_launchedProjectile)
        {
            var projectilePosition = animator.rootPosition;

            //Move the projectile over just a bit when creating it.
          
           

            //See if we should flip it to the right or left
            //Setting a scale to -1 in the x flips the object (same as rotating by 180 degrees)
            if (animator.gameObject.transform.localScale.x == -1)
            {
                //Vampire is looking right (opposite of default orientation)
                projectilePosition.x += 2.5f;
                projectilePosition.y -= .75f;
                var projectile = Instantiate(particle, projectilePosition, Quaternion.identity) as GameObject;
                var temp = projectile.transform.localScale;
                temp.x = -1;
                projectile.transform.localScale = temp;

                //flip the particle system's game object by 180
                var particleSystem = projectile.GetComponentInChildren<ParticleSystem>();
                //Rotation is stored as a quaternion, get a eulerAngle (ie degrees)
                particleSystem.transform.localRotation = Quaternion.Euler( new Vector3(0, 270, -90));
               
            }
            else
            {
                //Vampire is looking left (default character orientation)
                projectilePosition.x -= 2.5f;
                projectilePosition.y -= .75f;
                Instantiate(particle, projectilePosition, Quaternion.identity);
            }
           
            _launchedProjectile = true;
        }
       // Debug.Log("OnStateUpdate:" + stateInfo.normalizedTime.ToString());
    }
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       // Debug.Log("OnStateMove");
    }
    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       // Debug.Log("OnStateIK");
    }
}
