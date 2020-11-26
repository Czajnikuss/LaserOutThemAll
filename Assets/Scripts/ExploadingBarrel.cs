using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploadingBarrel : MonoBehaviour, IHitable
{
    public Side side {get {return this.thisObjectSide;} set {this.thisObjectSide = value;} }
    [SerializeField] private Side thisObjectSide;
    [SerializeField] Shader standardShader, hitShader;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private ParticleSystem boomSystem;
    [SerializeField] float blastRadius;
    private bool isBeeingHit, fire;

    private void Awake() {
        meshRenderer.enabled = true;
        boomSystem.gameObject.SetActive(false);
        
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
        StartCoroutine(DamageWithDisable());
    }
    private IEnumerator DamageWithDisable()
    {
        meshRenderer.enabled = false;
        boomSystem.gameObject.SetActive(true);
        boomSystem.Play();
        yield return new WaitForSeconds(0.5f);
        PlayManager.Instance.hitablesList.Remove(this);
        RaycastHit[] tempHits = Physics.SphereCastAll(transform.position, blastRadius, Vector3.forward);
        foreach (var item in tempHits)
        {
            item.collider.TryGetComponent<IHitable>(out var component);
            component?.Damage();
        }
        this.gameObject.SetActive(false);
        
    }
    public void FireThisUpdate()
    {
    
        fire = true;
    }
}
