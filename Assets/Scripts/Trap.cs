using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour, IHitable
{
    public Side side {get {return this.thisObjectSide;} set {this.thisObjectSide = value;} }
    [SerializeField] private Side thisObjectSide;
    [SerializeField] Shader standardShader, hitShader;
    [SerializeField] private MeshRenderer meshRenderer;
    public float timeToDiasable;
    public bool isTimeOnLogsSameAsTrap;
    public LogsOnTrap[] allLogsOnThisTrap;
    
    private bool isBeeingHit, fire;

    private void Awake() {
        meshRenderer.enabled = true;
        
        
    }
    private void Start() 
    {
        PlayManager.Instance.Fire.AddListener(FireThisUpdate);
       
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
            meshRenderer.material.shader = hitShader;
            isBeeingHit = false;
            
        }
        else
        {
            meshRenderer.material.shader = standardShader;
        }
        fire = false;
        
    }
    public void Hit()
    {
        isBeeingHit = true;
    }    
    public void Damage()
    {
        StartCoroutine(DamageWithDisable(timeToDiasable));
    }
    private IEnumerator DamageWithDisable(float timeToDiasable)
    {
        foreach (var item in allLogsOnThisTrap)
        {
            if(isTimeOnLogsSameAsTrap)
            {
                item.timeToDisable = timeToDiasable;
            }
            item.StartRoling();
        }
        
        yield return new WaitForSeconds(timeToDiasable);
        PlayManager.Instance.hitablesList.Remove(this);
        
        this.gameObject.SetActive(false);
        
    }
    public void FireThisUpdate()
    {
    
        fire = true;
    }
}
