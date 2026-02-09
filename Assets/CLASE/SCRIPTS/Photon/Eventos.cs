using System;
using UnityEngine;

public class Eventos : MonoBehaviour
{
    public event Action eventos; //guarda un metodo, son como los eventos pero mas optimizado

    private void Start()
    {
        eventos.Invoke();
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }
}
