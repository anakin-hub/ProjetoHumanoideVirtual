using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (PlayerMovement.Instance._enemyLocked)
        {
            if(other.gameObject.layer == LayerMask.NameToLayer("Atk"))
            {
                PlayerMovement.Instance.SetAttack(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Atk"))
        {
            PlayerMovement.Instance.SetAttack(false);
        }
    }
}
