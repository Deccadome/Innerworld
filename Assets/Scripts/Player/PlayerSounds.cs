using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    public AudioSource stoneStep1;
    public AudioSource stoneStep2;

    public void StoneStep1Keyframe() { stoneStep1.Play(); }
    public void StoneStep2Keyframe() { stoneStep2.Play(); }
}
