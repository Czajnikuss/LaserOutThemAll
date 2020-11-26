using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayManager : MonoBehaviour
{
    
    public static PlayManager Instance;
    public List<IHitable> hitablesList;
    public List<IReflectable> reflectableList;
    


    private void Awake() {
        Instance = this;
        FillInterfacesLists(); 
    }

    public void CheckWinLose()
    {
        Debug.Log("checking conditions, count in hitables:" + hitablesList.Count);
    }
    
    
    private void FillInterfacesLists()
    {
        hitablesList = FindObjectsOfType<MonoBehaviour>().OfType<IHitable>().ToList();
        reflectableList = FindObjectsOfType<MonoBehaviour>().OfType<IReflectable>().ToList();
       
    }
}
