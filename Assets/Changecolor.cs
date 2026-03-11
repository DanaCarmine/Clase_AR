using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Changecolor : MonoBehaviour
{
    public GameObject a1;
    public GameObject a3;
    public GameObject a11;
    public GameObject a12;
    public void ChangeColor_BTN()
    {
        // Asignamos un color aleatorio independiente a cada parte
        a1.GetComponent<Renderer>().material.color = GetRandomColor();
        a3.GetComponent<Renderer>().material.color = GetRandomColor();
        a11.GetComponent<Renderer>().material.color = GetRandomColor();
        a12.GetComponent<Renderer>().material.color = GetRandomColor();
    }
    Color GetRandomColor()
    {
        // Random.value genera un número entre 0.0 y 1.0
        return new Color(Random.value, Random.value, Random.value);
    }
}