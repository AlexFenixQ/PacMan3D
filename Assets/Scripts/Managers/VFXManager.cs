using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public static VFXManager Instance;
    public GameObject collectPrefab;
    void Awake() { if (Instance == null) Instance = this; else Destroy(gameObject); }

    public void PlayCollectEffect(Vector3 pos)
    {
        if (collectPrefab) Instantiate(collectPrefab, pos, Quaternion.identity);
    }
}