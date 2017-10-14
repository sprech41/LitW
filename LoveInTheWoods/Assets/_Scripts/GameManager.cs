using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public bool maeAngery;  //possible public variables accesible all over the game





    //here start the functions that create the DontDestroyOnLoad gameObject and result in this class being dragged through all scenes
    void Awake()
    {
        MakeSingleton();
    }

    private void MakeSingleton()
    {
        if (instance != null) {
            Destroy(gameObject);
        }
        else  {instance = this;
            DontDestroyOnLoad(gameObject);
        }
   }
}
