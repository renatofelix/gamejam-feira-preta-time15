using UnityEngine;

public class Sky : MonoBehaviour
{
    [SerializeField]
    private Color cloudySkyColor = new Color(0.393093f, 0.3831435f, 0.3962264f, 1);

    [SerializeField]
    private Color sunnySkyColor = new Color(0.6470588f, 0.7411765f, 0.7411765f, 1);

    [SerializeField]
    private Weather weather;

    private Camera cameraComponent;
    
    private DynamicColor currentSkyColor;

    // Start is called before the first frame update
    void Start()
    {
        cameraComponent = GetComponent<Camera>();

        currentSkyColor = sunnySkyColor;        

        weather.OnGotCloudy += (delayInSeconds) => {
            StartCoroutine(currentSkyColor.TransitionTo(cloudySkyColor, delayInSeconds));
        };

        weather.OnGotSunny += (delayInSeconds) => {
            StartCoroutine(currentSkyColor.TransitionTo(sunnySkyColor, delayInSeconds));
        };
    }

    // Update is called once per frame
    void Update()
    {
        cameraComponent.backgroundColor = currentSkyColor;
    }
}
