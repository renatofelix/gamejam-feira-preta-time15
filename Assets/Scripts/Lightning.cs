using System.Collections;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    [SerializeField]
    private Color lightningColor = new Color(0.5333304f, 0.6962748f, 0.8018868f, 1);

    [SerializeField]
    private Weather weather;

    private Light lightComponent;

    // Start is called before the first frame update
    void Start()
    {
        enabled = false;
        
        lightComponent = GetComponent<Light>();
        lightComponent.color = lightningColor;
        lightComponent.enabled = false;
        
        IEnumerator flashCoroutine = Flash();

        weather.OnStartedLightning += () => {
            StartCoroutine(flashCoroutine);
        };

        weather.OnStoppedLightning += () => {  
            StopCoroutine(flashCoroutine);
            lightComponent.enabled = false;
        };
    }

    private IEnumerator Flash() {
        lightComponent.enabled = true;
        yield return new WaitForSeconds(0.025f);
        lightComponent.enabled = false;

        yield return new WaitForSeconds(0.5f);

        lightComponent.enabled = true;
        yield return new WaitForSeconds(0.025f);
        lightComponent.enabled = false;

        yield return new WaitForSeconds(0.5f);

        lightComponent.enabled = true;
        yield return new WaitForSeconds(0.025f);
        lightComponent.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
