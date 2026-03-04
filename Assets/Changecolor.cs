/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Changecolor : MonoBehaviour
{
    public GameObject miku;
    public Color color;
    //public Material colorMaterial;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeColor_BTN()
    {
        //miku.GetComponent<Renderer>().material.color = color;
        //colorMaterial.color = color;
      
        Renderer rend = miku.GetComponent<Renderer>();
        Material[] mats = rend.materials;
        mats[3].color = color; 


    }
}*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Changecolor : MonoBehaviour
{
    public GameObject miku;

    private Renderer rend;
    private Material hairMaterial;

    private int colorIndex = 0;
    private Color originalColor;

    void Start()
    {
        rend = miku.GetComponent<Renderer>();

        // material del cabello (element 3)
        hairMaterial = rend.materials[3];

        // guardamos el color original
        originalColor = hairMaterial.color;
    }

    public void ChangeColor_BTN()
    {
        colorIndex++;

        if (colorIndex == 1)
            hairMaterial.color = Color.blue;

        else if (colorIndex == 2)
            hairMaterial.color = new Color(1f, 0.4f, 0.7f); // rosa

        else if (colorIndex == 3)
            hairMaterial.color = Color.green;

        else
        {
            hairMaterial.color = originalColor;
            colorIndex = 0;
        }
    }
}