using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicWall : MonoBehaviour, IReflectable
{
    GameObject sparks;
   public void Reflect(Vector3 reflectPoint, Vector3 reflectionNormal)
   {
       
       Quaternion rot = Quaternion.FromToRotation(Vector3.up, reflectionNormal);
       sparks = EffectsPooler.Instance.SpawnFromDictionary("sparks",reflectPoint,rot);
       sparks.gameObject.SetActive(true);
       
       Debug.Log(reflectionNormal);
       
   }
}
