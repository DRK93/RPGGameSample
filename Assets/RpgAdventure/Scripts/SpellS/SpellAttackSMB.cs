using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RpgAdventure;

    public class SpellAttackSMB : StateMachineBehaviour
    {
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
        int splNumber = GameObject.Find("Player").GetComponent<PlayerInput>().spellNumber;
        animator.GetComponent<SpellSpawner>().CreateSpell(splNumber);
        animator.SetBool("SpellCasting", true);
        }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
        int splNumber = GameObject.Find("Player").GetComponent<PlayerInput>().spellNumber;
        if (splNumber == 1)
            animator.ResetTrigger("SpellAttack");
        if (splNumber == 2)
            animator.ResetTrigger("SpellAttack2");
        if (splNumber == 3)
            animator.ResetTrigger("SpellAttack3");
        if (splNumber == 4)
        {
            animator.ResetTrigger("SpellAttack4");
        }

        animator.SetBool("SpellCasting", false);
        }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
    }


