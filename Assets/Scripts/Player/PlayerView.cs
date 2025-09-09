using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerView : MonoBehaviour, IPlayer
{
    CharacterController controller;
    public Joystick joystick; // правильный тип
    PlayerViewModel vm;

    public float speedMultiplier = 1f;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Start()
    {
        vm = new PlayerViewModel(this, 4f);
        tag = "Player";
    }

    void Update()
    {
        if (joystick == null)
        {
            Debug.LogError("Joystick is NOT assigned!");
            return;
        }

        Vector2 dir = joystick.Direction;
        Vector3 move = new Vector3(dir.x, 0, dir.y) * 4f * Time.deltaTime;
        vm.UpdateMovement(move);
    }

    public void Move(Vector3 delta)
    {
        controller.Move(delta);
        if (delta.sqrMagnitude > 0.001f)
            transform.forward = new Vector3(delta.x, 0, delta.z);
    }

    public void SetEnabled(bool enabled)
    {
        this.enabled = enabled;
    }

    public Vector3 GetPosition() => transform.position;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            GameManager.Instance.PlayerDied();
        }
    }
}