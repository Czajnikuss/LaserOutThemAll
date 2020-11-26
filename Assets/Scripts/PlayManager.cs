using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayManager : MonoBehaviour
{
    
    public static PlayManager Instance;
    public List<IHitable> hitablesList;
    public List<IReflectable> reflectableList;
    float clickTimer, clickInterval = 0.2f;
    public UnityEvent Fire, MoveLeft, MoveRight;
    private Vector3 mouseStartPos;
    [SerializeField] private float deadZoneInput;



    private void Awake() {
        Instance = this;
        FillInterfacesLists(); 
    }
    private void Update() 
    {
        clickTimer += Time.deltaTime;
        if(Input.GetMouseButtonDown(0))
        {
            clickTimer = 0;
            mouseStartPos = Input.mousePosition;
        }
        if(Input.GetMouseButtonUp(0) && clickTimer< clickInterval && Fire != null)
        {
            Fire.Invoke();
        }
        if(Input.GetMouseButton(0) && clickTimer > clickInterval)
        {
            if(Input.mousePosition.x > mouseStartPos.x + deadZoneInput && MoveRight !=null)
            {
                MoveRight.Invoke();
            }
            else if(Input.mousePosition.x < mouseStartPos.x - deadZoneInput && MoveLeft !=null)
            {
                MoveLeft.Invoke();
            }
        }
    }

    public void CheckWinLose()
    {
        foreach (var item in hitablesList)
        {
            if(item.side == Side.Enemy)
            {
                goto gotEnemy;
            }
            
        }
        Win();
        gotEnemy:;
    }
    public void Lose()
    {
        Debug.Log("Sorry, try next time");
    }
    public void Win()
    {
        Debug.Log("Won!!!!");
    }
    
    private void FillInterfacesLists()
    {
        hitablesList = FindObjectsOfType<MonoBehaviour>().OfType<IHitable>().ToList();
        reflectableList = FindObjectsOfType<MonoBehaviour>().OfType<IReflectable>().ToList();
       
    }
    public void RestartThisLevel()
    {
        Scene scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.name);
    }
}
