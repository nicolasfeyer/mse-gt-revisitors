using UnityEngine;

public class WeakSpot : MonoBehaviour
{
    [SerializeField] private ParticleSystem particles;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            particles.Play();
            particles.transform.localPosition = 
            transform.parent.localPosition + Vector3.down * 0.2f;
            transform.parent.gameObject.SetActive(false);
        }
    }
}
