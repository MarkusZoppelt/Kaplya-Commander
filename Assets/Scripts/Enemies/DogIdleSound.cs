using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogIdleSound : StateMachineBehaviour
{
    [SerializeField] internal AudioClip idleSound;
    [SerializeField] internal float timeBetweenSounds;

    private AudioSource source;
    private float soundTimer;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        soundTimer = 0f;
        source = animator.gameObject.GetComponent<AudioSource>();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        soundTimer -= Time.deltaTime;
        if (soundTimer > 0f)
        {
            return;
        }

        source.PlayOneShot(idleSound);
        soundTimer = timeBetweenSounds;
    }
}
