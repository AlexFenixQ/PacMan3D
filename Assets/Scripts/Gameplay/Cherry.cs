using UnityEngine;
using UnityEngine.VFX;

public class Cherry : MonoBehaviour
{
    public int value = 100;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.CollectPellet(value); // или отдельный AddScore
            AudioManager.Instance?.PlayCherry();
            VFXManager.Instance?.PlayCollectEffect(transform.position);
            Destroy(gameObject);
        }
    }
}