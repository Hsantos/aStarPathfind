using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Startup : MonoBehaviour {


    private Game game;

    void Awake()
    {
        gameObject.AddComponent<Game>();   
    }
}
