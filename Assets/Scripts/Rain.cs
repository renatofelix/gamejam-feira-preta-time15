using System.Collections;
using UnityEngine;

public class Rain : MonoBehaviour
{
    [SerializeField]
    private Weather weather;

    private ParticleSystem particleSystemComponent;

    // Start is called before the first frame update
    void Start()
    {
        particleSystemComponent = GetComponent<ParticleSystem>();

        weather.OnStartedRaining += () => {
            particleSystemComponent.Play();
        };

        weather.OnStoppedRaining += () => {
            particleSystemComponent.Stop();
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
