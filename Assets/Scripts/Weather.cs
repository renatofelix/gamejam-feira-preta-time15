using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Weather : MonoBehaviour
{
    [SerializeField]
    private float sunnyTransitionDelayInSeconds = 5;

    [SerializeField]
    private float cloudyTransitionDelayInSeconds = 5;

    [SerializeField]
    private float lightingStartDelayInSeconds = 4;

    [SerializeField]
    private float lightingStopDelayInSeconds = 4;

    [SerializeField]
    private float startDelayInSeconds = 10;

    [SerializeField]
    private float stopDelayInSeconds = 10;

    public Action<float> OnGotCloudy;

    public Action<float> OnGotSunny;

    public Action OnStartedRaining;

    public Action OnStoppedRaining;

    public Action OnStartedLightning;

    public Action OnStoppedLightning;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(NotifyWeatherChanges());
    }

    private IEnumerator NotifyWeatherChanges() {
        yield return new WaitForSeconds(startDelayInSeconds);
        OnGotCloudy(cloudyTransitionDelayInSeconds);
        yield return new WaitForSeconds(lightingStartDelayInSeconds);
        OnStartedLightning();
        yield return new WaitForSeconds(lightingStopDelayInSeconds);
        OnStoppedLightning();
        OnStartedRaining();
        yield return new WaitForSeconds(stopDelayInSeconds);
        OnStoppedRaining();
        OnGotSunny(sunnyTransitionDelayInSeconds);
    }

}
