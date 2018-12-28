using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {


    public string[,] board = new string[6, 7]
    {
        {"0","0","0","0","0","0","0"},
        {"0","0","0","0","0","2","0"},
        {"0","0","0","0","0","0","0"},
        {"0","1","0","0","0","0","0"},
        {"0","0","0","0","0","0","0"},
        {"0","0","0","0","0","0","0"}
    };

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
