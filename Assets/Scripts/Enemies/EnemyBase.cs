using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyBase : MonoBehaviour
{
    protected NavMeshAgent agent;
    protected Transform player;

    [Header("Speeds")]
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;
    public float frightenedSpeed = 1.5f;

    protected float originalSpeed;
    protected Material originalMat;
    public Material frightenedMat; // assign in inspector

    protected bool isFrightened = false;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        var p = GameObject.FindGameObjectWithTag("Player");
        if (p) player = p.transform;
        originalSpeed = agent.speed;
        var rend = GetComponentInChildren<Renderer>();
        if (rend) originalMat = rend.material;
    }

    protected virtual void OnEnable()
    {
        if (PowerUpManager.Instance != null)
        {
            PowerUpManager.Instance.OnPowerTimeChanged += OnPowerTimeChanged;
            PowerUpManager.Instance.OnPowerEnd += OnPowerEnd;
        }
    }
    protected virtual void OnDisable()
    {
        if (PowerUpManager.Instance != null)
        {
            PowerUpManager.Instance.OnPowerTimeChanged -= OnPowerTimeChanged;
            PowerUpManager.Instance.OnPowerEnd -= OnPowerEnd;
        }
    }

    void OnPowerTimeChanged(float t) { /* можно обновл€ть UI таймер если нужно */ }

    void OnPowerEnd()
    {
        ExitFrightened();
    }

    public virtual void EnterFrightened()
    {
        if (isFrightened) return;
        isFrightened = true;
        agent.speed = frightenedSpeed;
        if (frightenedMat != null)
        {
            var rend = GetComponentInChildren<Renderer>();
            if (rend) rend.material = frightenedMat;
        }
        // помен€ть поведение: идти в уклон или двигатьс€ случайно Ч можно в наследниках переопределить
    }

    public virtual void ExitFrightened()
    {
        if (!isFrightened) return;
        isFrightened = false;
        agent.speed = originalSpeed;
        var rend = GetComponentInChildren<Renderer>();
        if (rend && originalMat != null) rend.material = originalMat;
        // вернуть прежнее поведение
    }

    // ѕример: при триггере, когда игрок в режиме frightened Ч игрок "съедает" врага
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isFrightened)
            {
                // враг умирает (временное удаление)
                AudioManager.Instance?.PlayEatGhost();
                GameManager.Instance?.CollectPellet(200); // за съедение призрака даЄм очки
                VFXManager.Instance?.PlayCollectEffect(transform.position);
                // спр€чем врага на пару секунд
                StartCoroutine(TemporarilyDisable());
            }
            else
            {
                GameManager.Instance?.PlayerDied();
                AudioManager.Instance.PlayDeath();
            }
        }
    }

    System.Collections.IEnumerator TemporarilyDisable()
    {
        // скрываем или телепортируем на респаун
        gameObject.SetActive(false);
        yield return new WaitForSeconds(3f);
        // вернуть на начальную точку или на NavMesh
        gameObject.SetActive(true);
        ExitFrightened();
    }
}