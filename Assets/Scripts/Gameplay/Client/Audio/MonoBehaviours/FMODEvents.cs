using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    [field: Header("SFX")]
    [field: SerializeField] public EventReference ShipLaser { get; private set; }
    [field: SerializeField] public EventReference ShipEngine { get; private set; }
    [field: SerializeField] public EventReference LaserHit { get; private set; }
    [field: SerializeField] public EventReference ShipKilled { get; private set; }
    [field: SerializeField] public EventReference Shield { get; private set; }
    [field: SerializeField] public EventReference Damage { get; private set; }
    [field: SerializeField] public EventReference TraficVehicle { get; private set; }
    [field: SerializeField] public EventReference Click { get; private set; }

    [field: Header("Music")]
    [field: SerializeField] public EventReference Music { get; private set; }
    public static FMODEvents instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {

            Debug.Log("Found more than one Audio Manager in the scene.");
            Destroy(gameObject);

        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}