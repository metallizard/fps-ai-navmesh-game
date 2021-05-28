using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPoint : MonoBehaviour
{
    [SerializeField]
    private float _damage = 25;
    [SerializeField]
    private float _radius = 1;
    [SerializeField]
    private LayerMask _targetLayer;

    void Update()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _radius, _targetLayer);

        if(hits.Length > 1)
        {
            foreach(var hit in hits)
            {
                Debug.Log(hit.gameObject);
            }
        }
    }
}
