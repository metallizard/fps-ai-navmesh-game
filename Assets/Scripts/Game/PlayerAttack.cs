using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]
    private Animator _weaponAnimator;

    [SerializeField]
    private AudioClip _shootSfx;

    [SerializeField]
    private GameObject _shootVfx;

    void Update()
    {
        if(!Game.Instance.IsPaused)
        {
            Fire();
        }
    }

    void Fire()
    {
        if (!_weaponAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            return;

        // Detect player input (left mouse click)
        if(Input.GetMouseButtonDown(0))
        {
            // Trigger Fire animation.
            _weaponAnimator.SetTrigger("Fire");

            // Play shoot sfx.
            AudioSource.PlayClipAtPoint(_shootSfx, transform.position);

            // Play shoot vfx.
            _shootVfx.SetActive(true);

            StartCoroutine(DeactivateShootSFX());

            // Shoot bullet.
            RaycastHit hit;
            if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit))
            {
                // Hit something.
                if (hit.transform.tag == "Enemy")
                {
                    hit.transform.GetComponent<Health>().GetHit(50);
                }
            }
            else
            {
                // Hit nothing.
            }
        }
    }

    IEnumerator DeactivateShootSFX()
    {
        yield return new WaitForSeconds(0.15f);
        _shootVfx.SetActive(false);
    }
}
