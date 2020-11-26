using System.Collections;
using UnityEngine;

public class Sparks : MonoBehaviour, IPooledObject
{
    public void OnObjectPooled()
    {
        StartCoroutine(DisableAfter(0.5f));
    }
    IEnumerator DisableAfter(float timeTo)
    {
        yield return new WaitForSeconds(timeTo);
        gameObject.SetActive(false);
    }
}
