using UnityEngine;

public class TeleportWall : MonoBehaviour
{
    public TeleportWall targetWall;   // ссылка на вторую стену
    private bool isTeleporting = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isTeleporting && targetWall != null)
        {
            StartCoroutine(Teleport(other));
        }
    }

    private System.Collections.IEnumerator Teleport(Collider player)
    {
        isTeleporting = true;

        // Берём центр второй стены
        Vector3 wallCenter = targetWall.transform.position;

        // Берём нормаль стены (направление "вперёд")
        Vector3 wallNormal = targetWall.transform.forward;

        // Вычисляем толщину стены (по оси Z коллайдера)
        float halfDepth = targetWall.GetComponent<Collider>().bounds.extents.z;

        // Точка выхода: центр стены + нормаль * (толщина/2 + отступ)
        Vector3 exitPos = wallCenter + wallNormal * (halfDepth + 1f);

        // Перемещаем игрока
        player.transform.position = exitPos;

        // Разворачиваем игрока так же, как смотрит стена
        player.transform.forward = wallNormal;

        // Блокируем мгновенный возврат
        targetWall.isTeleporting = true;

        yield return new WaitForSeconds(0.3f);

        isTeleporting = false;
        targetWall.isTeleporting = false;
    }
}