using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (LineRenderer))]
public class PlayerController : MonoBehaviour
{
    [Tooltip("Fire point game object's Transform")]  
    [SerializeField] private Transform hitPointTransform;  
    //the attached line renderer  
    private LineRenderer lineRenderer; 

    //a RaycastHit variable, to gather informartion about the ray's collision  
    private RaycastHit hit;  
  
    //reflection direction  
    private Vector3 inDirection;  
  
    private List<Vector3> hitPointsList;
  
    //maximim distance to with we are checking for colision
    private float maxHitDistance = 100f;  
  
    void Awake ()  
    {  
          
        //get the attached LineRenderer component  
        lineRenderer = this.GetComponent<LineRenderer>();  
    }  
    private void Start() 
    {
        StartARay();
    }
    private void StartARay() {
        hitPointsList = new List<Vector3>();
        ShootARay(hitPointTransform.position, hitPointTransform.forward, maxHitDistance);
    }
    
    
    private void ShootARay(Vector3 start, Vector3 direction, float maxLeanght)
    {
        //this is called only when there is something to add, so let's add.
        hitPointsList.Add(start);
        //we hit something
        if(Physics.Raycast(start,direction, out hit, maxHitDistance))
        {
            //configure next possible ray
            inDirection = Vector3.Reflect(hit.point-start,hit.normal); 
            ShootARay(hit.point, inDirection, maxHitDistance);
            
        }
        //no hit on first try
        else
        {
            hitPointsList.Add(inDirection * maxHitDistance);
            lineRenderer.positionCount = hitPointsList.Count;
            lineRenderer.SetPositions(hitPointsList.ToArray());
        }
        
        
    }
 
}
