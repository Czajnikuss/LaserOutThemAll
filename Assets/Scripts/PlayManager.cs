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
    
    float clickTimer, clickInterval = 0.2f;
    public UnityEvent Fire, MoveLeft, MoveRight;
    private Vector3 mouseStartPos;
    [SerializeField] private float deadZoneInput;



    private void Awake() {
        Instance = this;
        //variable initialization
        hitablesList = FindObjectsOfType<MonoBehaviour>().OfType<IHitable>().ToList();
        Time.timeScale = 1;
    }
    private void Update() 
    {
        //lets check for desired input and fire related events
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
    //as of now only log info, in future there should be sone screens 
    public void Lose()
    {
        Debug.Log("Sorry, try next time");
        Time.timeScale = 0;
    }
    public void Win()
    {
        Debug.Log("Won!!!!");
        Time.timeScale = 0;
    }
    
    public void RestartThisLevel()
    {
        Time.timeScale = 1;
        Scene scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.name);
    }
}
