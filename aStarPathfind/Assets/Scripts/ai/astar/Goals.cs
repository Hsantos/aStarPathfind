using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goals {

    public int cost { get; private set; }

    public void UpdateGoals()
    {
        cost++;
    }
}
