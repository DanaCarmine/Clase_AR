using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAccessory : MonoBehaviour
{
    public GameObject[] accesorios; // Los 3 modelos
    private int ultimoIndice = -1;

    void Start()
    {
        //elige el primero al azar al arrancar.
        ChangeAccessory_BTN();
    }

    public void ChangeAccessory_BTN()
    {
        if (accesorios.Length == 0) return;

        int nuevoIndice;

        // Elige uno al azar que no sea el actual
        do
        {
            nuevoIndice = Random.Range(0, accesorios.Length);
        } while (nuevoIndice == ultimoIndice && accesorios.Length > 1);

        ultimoIndice = nuevoIndice;
        ActualizarAccesorios(nuevoIndice);
    }

    void ActualizarAccesorios(int indiceActivo)
    {
        for (int i = 0; i < accesorios.Length; i++)
        {
            if (accesorios[i] != null)
            {
                // Solo activa el que coincida con el índice, los demás se apagan
                accesorios[i].SetActive(i == indiceActivo);
            }
        }
    }
}