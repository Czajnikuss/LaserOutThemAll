using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (LineRenderer))]
public class PlayerController : MonoBehaviour, IHitable
{
    [Tooltip("Fire point game object's Transform")]  
    [SerializeField] private Transform hitPointTransform;  
    //the attached line renderer  
    private LineRenderer lineRenderer; 

    //a RaycastHit variable, to gather informartion about the ray's collision  
    private RaycastHit hit;  
    private bool isBeeingHit, markForHit;
    private Color startColor;
  
    //reflection direction  
    private Vector3 inDirection;  
  
    private List<Vector3> hitPointsList;
  
    [Tooltip("maximim distance to with we are checking for colision")]
    [SerializeField] private float maxHitDistance = 100f;  
    

    [SerializeField] float rotationSpeed;
    MeshRenderer meshRenderer;
    [SerializeField] Material standardMateril, redTintMaterial;
    void Awake ()  
    {  
          
        //get the attached LineRenderer component  
        lineRenderer = this.GetComponent<LineRenderer>();  
        meshRenderer = GetComponent<MeshRenderer>();
    }  
    private void Start() 
    {
        //let us show begining ray so player know what we are doing
        StartARay();
        markForHit=false;
    }
    private void Update() 
    {
        
        //I'll define platform specyfic method of this name to efficiently use touch input and easly test in editor
        //In it we are changing Player rotation acording to mouse of touch movement
        Movement();
        
    }
    private void LateUpdate() {
            if(!isBeeingHit&& !markForHit)meshRenderer.material = standardMateril;
    
    }
    public void Hit(Vector3 hitPoint)
    {
        isBeeingHit = true;
        markForHit = true;
        meshRenderer.material= redTintMaterial;
    }

#region Ray
//starting a new ray, need to clean list and give first vector.
    private void StartARay() {
        isBeeingHit = false;
        

        hitPointsList = new List<Vector3>();
        ShootARay(hitPointTransform.position, hitPointTransform.forward, maxHitDistance);
    }
    

    
    private void ShootARay(Vector3 start, Vector3 direction, float maxLeanght)
    {
        IReflectable reflectable;
        IHitable hitable;
        //this is called only when there is something to add, so let's add.
        hitPointsList.Add(start);
        //we hit something
        if(Physics.Raycast(start,direction, out hit, maxHitDistance) )
        {
            reflectable = hit.collider.GetComponent<IReflectable>();
            if(reflectable!= null)
            {
                reflectable.Reflect(hit.point, hit.normal);
            //configure next possible ray
            inDirection = Vector3.Reflect(hit.point-start, hit.normal); 
            ShootARay(hit.point, inDirection, maxHitDistance);
            }
            hitable = hit.collider.GetComponent<IHitable>();
            if(hitable != null)
            {
                hitPointsList.Add(hit.point);
                lineRenderer.positionCount = hitPointsList.Count;
                lineRenderer.SetPositions(hitPointsList.ToArray());
                
                hitable.Hit(hit.point);
            }
        }
        else
        {
            //no collision problem
            if(hitPointsList.Count<2)
                {
                    hitPointsList.Add(direction * maxHitDistance);
                    lineRenderer.positionCount = hitPointsList.Count;
                    lineRenderer.SetPositions(hitPointsList.ToArray());
                }
            else
                {
                    hitPointsList.Add(inDirection * maxHitDistance);
                    lineRenderer.positionCount = hitPointsList.Count;
                    lineRenderer.SetPositions(hitPointsList.ToArray());
                }
        }
        
        
    }
#endregion 

#region Player Movement
#if UNITY_EDITOR
private void Movement() 
{
    if(Input.GetMouseButton(0))
    {
    transform.Rotate(0, (Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime), 0, Space.World);
    StartARay();
    }
}

#elif UNITY_ANDROID
Touch touch;
float startingPosition;
private void Movement() 
{
    if(Input.touchCount>0)
    {
        switch (touch.phase)
        {
            case TouchPhase.Began:
                startingPosition = touch.position.x;
                break;
            case TouchPhase.Moved:
                if (startingPosition > touch.position.x)
                {
                    transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
                }
                else if (startingPosition < touch.position.x)
                {
                    transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
                }
                break;
            case TouchPhase.Ended:
                Debug.Log("Touch Phase Ended.");
                break;
        }
    StartARay();
    }
}
#endif
#endregion

}
