using UnityEngine;
using FMODUnity;

public class FMODTest : MonoBehaviour
{
    [SerializeField] private EventReference testSound;

    void Update()
    {

        AudioManager.instance.PlayOneShot(testSound, transform.position);

        Debug.Log("FMOD one-shot sound triggered!");
    }
}
