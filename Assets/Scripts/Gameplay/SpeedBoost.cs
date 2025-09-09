using System.Collections;
using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
    public float multiplier = 1.6f;
    public float duration = 5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var pv = other.GetComponent<PlayerView>();
            if (pv != null) pv.StartCoroutine(ApplyBoost(pv));
            AudioManager.Instance?.PlayPowerup();
            Destroy(gameObject);
        }
    }

    IEnumerator ApplyBoost(PlayerView pv)
    {
        pv.speedMultiplier *= multiplier;
        yield return new WaitForSeconds(duration);
        pv.speedMultiplier /= multiplier;
    }
}