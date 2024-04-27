using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class BreakParticle : MonoBehaviour
{
    ParticleSystem particle;

    [SerializeField][Required]
        Texture2D breakTexture;

    void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }
    public void SpawnParticle(Vector3 pos, Quaternion rot)
    {
        ParticleSystem.ShapeModule shapeModule = particle.shape;
        shapeModule.texture = breakTexture;

        particle.transform.parent = null;
        particle.transform.position = pos;
        particle.transform.rotation = rot;
        particle.Play();
        Destroy(gameObject, particle.main.duration);
    }
}
