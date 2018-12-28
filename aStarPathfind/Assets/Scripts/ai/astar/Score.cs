using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score  {

	public int calcScore(int cost, int heuristic)
    {
        return cost + heuristic;
    }
}
