using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Piece : MonoBehaviour {

    public Node node;

    public Image icon;
    public Color defaultColor;
    void Awake()
    {
        icon = GetComponent<Image>();
        defaultColor = icon.color;
    }
}
