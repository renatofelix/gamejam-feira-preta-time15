using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weather : MonoBehaviour
{
    [SerializeField]
    private Color sunnyLightColor = new Color(0.9622642f, 0.8914236f, 0.753471f, 1);

    [SerializeField]
    private Color cloudyLightColor = new Color(0.5843138f, 0.3764706f, 0.5764706f, 1);

    [SerializeField]
    private Color lightningColor = new Color(0.5333304f, 0.6962748f, 0.8018868f, 1);

    [SerializeField]
    private Color cloudySkyColor = new Color(0.393093f, 0.3831435f, 0.3962264f, 1);

    [SerializeField]
    private Color darkerGrassColor = new Color(0.4056604f, 0.4056604f, 0.2315326f, 1);

    [SerializeField]
    private Color lighterGrassColor = new Color(0.5878751f, 0.6226414f, 0.2085261f, 1);

    [SerializeField]
    private float sunnyWeatherDuration = 10;

    [SerializeField]
    private ParticleSystem rain;

    [SerializeField]
    private Camera mainCamera;

    [SerializeField]
    private Material grassMaterial;

    private Light lightComponent;

    private float elapsedTimeInSeconds;

    // Start is called before the first frame update
    void Start()
    {
        lightComponent = GetComponent<Light>();
        lightComponent.color = sunnyLightColor;
        rain.gameObject.SetActive(false);
        grassMaterial.SetColor(UnlitShader.BASE_COLOR, lighterGrassColor);
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTimeInSeconds += Time.deltaTime;
        float lerpFactor = (elapsedTimeInSeconds / sunnyWeatherDuration) * Time.deltaTime;
        lightComponent.color = Color.Lerp(lightComponent.color, cloudyLightColor, lerpFactor);
        mainCamera.backgroundColor = Color.Lerp(mainCamera.backgroundColor, cloudySkyColor, lerpFactor);
        grassMaterial.SetColor(UnlitShader.BASE_COLOR, Color.Lerp(grassMaterial.GetColor(UnlitShader.BASE_COLOR), darkerGrassColor, lerpFactor));

        if (elapsedTimeInSeconds > (0.8 * sunnyWeatherDuration)) {
            Flash();
        }
    }

    private void Flash() {
        if (Random.Range(0, 100) < 1) {
            lightComponent.color = lightningColor;
        } else {
            lightComponent.color = cloudyLightColor;
            rain.gameObject.SetActive(true);
        }
    }

    void OnApplicationQuit() {
        grassMaterial.SetColor(UnlitShader.BASE_COLOR, lighterGrassColor);
    }
}
