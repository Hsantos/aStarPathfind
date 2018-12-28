using System;
using UnityEngine;

public class Node
{
    public Vector3 pos { get; private set; }
    public string value { get; private set; }
    public int line { get; set; }
    public int collumn{ get; set; }

    public int score;

    public Node(string value,int line, int collumn,  Vector3 pos)
    {
        this.value = value;
        this.line = line;
        this.collumn = collumn;
        this.pos = pos;
    }
}

