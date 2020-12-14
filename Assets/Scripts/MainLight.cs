using UnityEngine;

public class MainLight : MonoBehaviour
{
    [SerializeField]
    private Color sunnyLightColor = new Color(0.6470588f, 0.7411765f, 0.7137255f, 1);

    [SerializeField]
    private Color cloudyLightColor = new Color(0.5843138f, 0.3764706f, 0.5764706f, 1);
    
    [SerializeField]
    private Weather weather;

    private Light lightComponent;

    private DynamicColor currentLightColor;

    // Start is called before the first frame update
    void Start()
    {
        lightComponent = GetComponent<Light>();

        currentLightColor = sunnyLightColor;
        
        weather.OnGotCloudy += (delayInSeconds) => {
            StartCoroutine(currentLightColor.TransitionTo(cloudyLightColor, delayInSeconds));
        };

        weather.OnGotSunny += (delayInSeconds) => {  
            StartCoroutine(currentLightColor.TransitionTo(sunnyLightColor, delayInSeconds));
        };
    }

    // Update is called once per frame
    void Update()
    {
        lightComponent.color = currentLightColor;
    }
}
