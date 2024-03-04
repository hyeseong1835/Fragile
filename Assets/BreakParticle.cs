using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class BreakParticle : MonoBehaviour
{
    [SerializeField] ParticleSystem particle;
    public void SpawnParticle(Vector3 pos, Quaternion rot)
    {
        particle.transform.parent = null;
        particle.transform.position = pos;
        particle.transform.rotation = rot;
        particle.Play();
        Destroy(gameObject, particle.main.duration);
    }
}
