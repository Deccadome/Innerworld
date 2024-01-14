using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class OrbController : MonoBehaviour
{
    Transform player;
    Light2D orbLight;
    public float logShape;

    [SerializeField]
    float distance;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        orbLight = gameObject.GetComponent<Light2D>();
    }
    private void Update()
    {
        orbLight.intensity = Mathf.Clamp(1 / (distance + 1) * 25f, 0.25f, 50f);
    }

    private void FixedUpdate()
    {
        GetDistanceToPlayer();
    }

    private void GetDistanceToPlayer()
    {
        distance = Vector3.Distance(player.position, transform.position);
    }
}
