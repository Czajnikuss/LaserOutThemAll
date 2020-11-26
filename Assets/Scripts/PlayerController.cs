using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof (LineRenderer))]
public class PlayerController : MonoBehaviour, IHitable
{
    [Tooltip("Fire point game object's Transform")]  
    [SerializeField] private Transform hitPointTransform;  
    //the attached line renderer  
    private LineRenderer lineRenderer; 

    //a RaycastHit variable, to gather informartion about the ray's collision  
    private RaycastHit rayHit;  
    private bool isBeeingHit, fire;
    
  
    //reflection direction  
    private Vector3 inDirection;  
  
    private List<Vector3> rayHitPointsList;
    
    public Side side {get {return this.thisObjectSide;} set {this.thisObjectSide = value;} }
    [SerializeField] private Side thisObjectSide;
 
    [Tooltip("maximim distance to with we are checking for colision")]
    [SerializeField] private float maxHitDistance = 100f;  
    

    [SerializeField] float rotationSpeed;
    
    [SerializeField] SkinnedMeshRenderer playerMeshRenderer;
    [SerializeField] Shader standardShader, hitShader, damageShader;
    Animator animator;
    
    void Awake ()  
    {  
          
        //get the attached LineRenderer component  
        lineRenderer = this.GetComponent<LineRenderer>();  
        animator = this.GetComponent<Animator>();
    }  
    private void Start() 
    {   
        PlayManager.Instance.Fire.AddListener(FireThisUpdate);
        PlayManager.Instance.MoveLeft.AddListener(MovingLeft);
        PlayManager.Instance.MoveRight.AddListener(MovingRight);
        
        
        side =  thisObjectSide;
        
    }
    private void Update() 
    {
        
        //I'll define platform specyfic method of this name to efficiently use touch input and easly test in editor
        //In it we are changing Player rotation acording to mouse of touch movement
        //Movement();
        StartARay();
        
    }
    void LateUpdate()
    {
        
        if(isBeeingHit)
        {
            if(fire)
            {
                fire=false;
                Damage();
            }
            playerMeshRenderer.materials[0].shader = hitShader;
            isBeeingHit = false;
            
        }
        else
        {
            playerMeshRenderer.materials[0].shader = standardShader;
        }
        fire = false;
        
    }
    
    public void Hit()
    {
        isBeeingHit = true;
    }    
    public void Damage()
    {
        StartCoroutine(DamageWithDisable());
    }
    public void FireThisUpdate()
    {
    
        fire = true;
    }
    public void MovingLeft()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
    public void MovingRight()
    {
        transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
    }

    private IEnumerator DamageWithDisable()
    {
        playerMeshRenderer.materials[0].shader = damageShader;
        playerMeshRenderer.materials[1].shader = damageShader;
        animator.Play("TakeDamage");
        yield return new WaitForSeconds(1);
        PlayManager.Instance.hitablesList.Remove(this);
        if(side==Side.Player)
        {
            PlayManager.Instance.Lose();
        }
        else
        {
            PlayManager.Instance.CheckWinLose();
        }
        
        this.gameObject.SetActive(false);

    }
     
#region Ray
//starting a new ray, need to clean list and give first vector.
    private void StartARay() 
    {
        rayHitPointsList = new List<Vector3>();
        ShootARay(hitPointTransform.position, hitPointTransform.forward, maxHitDistance);
    }
    

    
    private void ShootARay(Vector3 start, Vector3 direction, float maxLeanght)
    {
        IReflectable reflectable;
        IHitable hitable;
        //this is called only when there is something to add, so let's add.
        rayHitPointsList.Add(start);
        //we hit something
        if(Physics.Raycast(start,direction, out rayHit, maxHitDistance) )
        {
            
            rayHit.collider.TryGetComponent<IReflectable>(out reflectable);
            if(reflectable!= null)
            {
                reflectable.Reflect(rayHit.point, rayHit.normal);
                
                //configure next possible ray
                inDirection = Vector3.Reflect(rayHit.point-start, rayHit.normal); 
                ShootARay(rayHit.point, inDirection, maxHitDistance);
            }
            rayHit.collider.TryGetComponent<IHitable>(out hitable);
            if(hitable != null)
            {
                rayHitPointsList.Add(rayHit.point);
                lineRenderer.positionCount = rayHitPointsList.Count;
                lineRenderer.SetPositions(rayHitPointsList.ToArray());
                
                hitable.Hit();
            }
        }
        else
        {
            //no collision, so we draw line to max lenght as last element
            if(rayHitPointsList.Count<2)
                {
                    rayHitPointsList.Add(direction * maxHitDistance);
                    lineRenderer.positionCount = rayHitPointsList.Count;
                    lineRenderer.SetPositions(rayHitPointsList.ToArray());
                }
            else
                {
                    rayHitPointsList.Add(inDirection * maxHitDistance);
                    lineRenderer.positionCount = rayHitPointsList.Count;
                    lineRenderer.SetPositions(rayHitPointsList.ToArray());
                }
        }
        
        
    }
#endregion 

#region Player Movement
/*#if UNITY_EDITOR
private void Movement() 
{
    if(Input.GetMouseButton(0))
    {
    transform.Rotate(0, (Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime), 0, Space.World);
    }
    
}
//#endif 
/*#if UNITY_ANDROID
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
    }
}
#endif */
#endregion

}
