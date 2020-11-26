using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogsOnTrap : MonoBehaviour
{
    public Rigidbody rb;
    public float timeToDisable;
    bool isRolling;

    public void StartRoling() 
    {
        if(!isRolling)
        {
            rb.useGravity = true;
            StartCoroutine(DisableAfter(timeToDisable));
            isRolling = true;
        }
    }
    private void OnCollisionEnter(Collision other) 
    {
        if(isRolling)
        {
            other.collider.TryGetComponent<IHitable>(out IHitable hitable);
            if(hitable != null)
            {
                hitable.Damage();
            }
        }    
    }
    IEnumerator DisableAfter( float timeToDisable)
    {
        yield return new WaitForSeconds(timeToDisable);
        this.gameObject.SetActive(false);
    }
    

}
