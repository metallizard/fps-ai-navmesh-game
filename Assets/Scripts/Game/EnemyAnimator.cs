using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Walk(bool isWalking)
    {
        _animator.SetBool("Walk", isWalking);
    }

    public void Run(bool isRunning)
    {
        _animator.SetBool("Run", isRunning);
    }

    public void Attack()
    {
        _animator.SetTrigger("Attack");
    }
}
