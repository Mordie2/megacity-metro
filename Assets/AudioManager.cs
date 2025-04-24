using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;


public class AudioManager : MonoBehaviour
{
    [Header("Volume")]
    [Range(0, 1)]
    public float masterVolume = 1;

    private Bus masterBus;

    private List<EventInstance> eventInstances;
    private List<StudioEventEmitter> eventEmitters;


    public static AudioManager instance { get; private set; }

    public EventInstance ambienceEventInstance;


    void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogError("Found more than one Audio Manager in the scene.");
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        eventInstances = new List<EventInstance>();
        eventEmitters = new List<StudioEventEmitter>();

        masterBus = RuntimeManager.GetBus("bus:/");
 
    }

    private void Start()
    {
        //InitializeAmbience(FMODEvents.instance.Ambience1);
        //playerCart = GameObject.Find("Cart");
    }

    private void Update()
    {
        masterBus.setVolume(masterVolume);

    }

    private void InitializeAmbience(EventReference ambienceEventReference)
    {
        ambienceEventInstance = CreateInstance(ambienceEventReference);
        ambienceEventInstance.start();
        ambienceEventInstance.release();
    }

    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public StudioEventEmitter InitializeEventEmitter(EventReference eventReference, GameObject emitterGameObject)
    {
        StudioEventEmitter emitter = emitterGameObject.GetComponent<StudioEventEmitter>();
        emitter.EventReference = eventReference;
        eventEmitters.Add(emitter);
        return emitter;
    }

    private void CleanUp()
    {
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }

        foreach (StudioEventEmitter emitter in eventEmitters)
        {
            emitter.Stop();
        }
    }

    public EventInstance CreateInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        return eventInstance;
    }

    public void SetAmbienceParameter(string parameterName, float parameterValue)
    {
        ambienceEventInstance.setParameterByName(parameterName, parameterValue);
    }

    public void SetInstanceParameter(EventInstance eventInstance, string parameterName, float parameterValue)
    {
        eventInstance.setParameterByName(parameterName, parameterValue);
    }

    public void SetEmitterParameter(StudioEventEmitter emitter, string parameterName, float parameterValue)
    {
        if (emitter != null)
        {
            emitter.EventInstance.setParameterByName(parameterName, parameterValue);
        }
    }

    public static void PlayOneShotWithParameters(EventReference eventReference, Vector3 position, params (string, float)[] parameters)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(position));

        foreach (var parameter in parameters)
        {
            eventInstance.setParameterByName(parameter.Item1, parameter.Item2);
        }

        eventInstance.start();
        eventInstance.release();
    }


    private void OnDestroy()
    {
        CleanUp();
    }
}