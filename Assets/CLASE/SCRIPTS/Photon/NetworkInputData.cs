using UnityEngine;
using Fusion;

/// <summary>
/// Esta estructura debe contener todos los valores que se van a mandar a el servidor. Ojo univcamente los valores.
/// 
/// Esto debe de heredar de INetworkInput para que la estructura sea reconocida por el servidor como una serie de inputs a leer
/// Hicimos una Interface de inputs
/// </summary>

public struct NetworkInputData : INetworkInput
{
    public Vector2 move;
    public Vector2 look;
    public bool isRunning;

    public float yRotation;

    public bool shoot;
}
