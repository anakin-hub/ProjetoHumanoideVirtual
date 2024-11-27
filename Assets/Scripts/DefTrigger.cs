using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (PlayerMovement.Instance._enemyLocked)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Def"))
            {
                PlayerMovement.Instance.SetDefense(true);
                Debug.Log("defesa!");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Def"))
        {
            PlayerMovement.Instance.SetDefense(false);
        }
    }
}
