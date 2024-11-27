using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorComponent : MonoBehaviour
{
    public Transform _target;
    [SerializeField] bool _targetDetected = false;

    // Update is called once per frame
    void Update()
    {
        if (_targetDetected && _target != null)
        {
            // Calcula a direção para o jogador
            Vector3 direction = (_target.position - transform.position).normalized;

            // Cria uma rotação suave em direção ao jogador
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.parent.rotation = Quaternion.Slerp(transform.parent.rotation, lookRotation, 10f * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _target = other.transform;
            _targetDetected = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _targetDetected = false;
            _target = null;
        }
    }
}
