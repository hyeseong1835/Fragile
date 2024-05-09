using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class BreakParticle : MonoBehaviour
{
    [SerializeField][Required]
        Texture2D breakTexture;

    public void SpawnParticle(Vector3 pos, Quaternion rot)
    {
        gameObject.SetActive(true);

        ParticleSystem particle = GetComponent<ParticleSystem>();
        ParticleSystem.ShapeModule shapeModule = particle.shape;
        shapeModule.texture = breakTexture;

        particle.transform.parent = null;
        particle.transform.position = pos;
        particle.transform.rotation = rot;
        particle.Play();
        
        Destroy(gameObject, particle.main.duration);
    }
}
