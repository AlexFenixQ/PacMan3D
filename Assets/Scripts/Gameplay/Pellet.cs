using UnityEngine;

public class Pellet : MonoBehaviour
{
    public int value = 10;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.Instance != null)
                GameManager.Instance.CollectPellet(value);
            AudioManager.Instance.PlayPellet();
            Destroy(gameObject);
        }
    }
}
