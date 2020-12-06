using UnityEngine;

namespace DumontStudios.General
{
    public class UnityUtils : MonoBehaviour
    {
        public static UnityUtils instance;

        public AudioSource genericAudioSource2D;
        public AudioSource genericAudioSource;
        public AudioSource mixerAudiSource;

        //General
        public void Awake()
        {
            if(instance)
            {
                Destroy(gameObject);

                return;
            }

            instance = this;
        }

        //Utils
        public static void PlayAudio(AudioClip audio, float volume = 0.3f)
        {
            if(audio == null)
            {
                return;
            }

            instance.genericAudioSource2D.clip = audio;
            instance.genericAudioSource2D.volume = volume;

            instance.genericAudioSource2D.Play();
        }

        public static AudioSource PlayAudioAtPosition(AudioClip audio, Vector3 position, float duration = 5, float volume = 1, float minDistance = 0, float maxDistance = 10)
        {
            if(audio == null)
            {
                return null;
            }

            AudioSource audioSource = (AudioSource)MonoBehaviour.Instantiate(instance.genericAudioSource, position, Quaternion.identity);
            audioSource.clip = audio;
            audioSource.volume = volume;
            audioSource.maxDistance = maxDistance;

            audioSource.Play();

            MonoBehaviour.Destroy(audioSource.gameObject, duration);

            return audioSource;
        }

        public static T RandomArg<T>(params T[] values)
        {
            return values[UnityEngine.Random.Range(0, values.Length)];
        }
    }
}
