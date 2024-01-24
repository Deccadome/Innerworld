using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Dome
{
    public class OrbSound : MonoBehaviour
    {
        AudioSource glowSound;
        Light2D orbLight;
        public float soundLength = 2.88f;
        private bool isPlaying = false;

        private void Start()
        {
            glowSound = GetComponent<AudioSource>();
            orbLight = GetComponent<Light2D>();
        }

        private void Update()
        {
            HandleSound();
        }

        void HandleSound()
        {
            glowSound.volume = Mathf.Clamp(orbLight.intensity / 10f, 0f, 0.5f);
            if (!isPlaying)
            {
                isPlaying = true;
                StartCoroutine(PlaySound());

            }
        }

        IEnumerator PlaySound()
        {
            glowSound.Play();
            yield return new WaitForSeconds(soundLength);
            isPlaying = false;
        }
    }
}
