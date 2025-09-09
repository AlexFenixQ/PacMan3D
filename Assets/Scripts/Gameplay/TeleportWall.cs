using UnityEngine;

public class TeleportWall : MonoBehaviour
{
    public TeleportWall targetWall;   // ������ �� ������ �����
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

        // ���� ����� ������ �����
        Vector3 wallCenter = targetWall.transform.position;

        // ���� ������� ����� (����������� "�����")
        Vector3 wallNormal = targetWall.transform.forward;

        // ��������� ������� ����� (�� ��� Z ����������)
        float halfDepth = targetWall.GetComponent<Collider>().bounds.extents.z;

        // ����� ������: ����� ����� + ������� * (�������/2 + ������)
        Vector3 exitPos = wallCenter + wallNormal * (halfDepth + 1f);

        // ���������� ������
        player.transform.position = exitPos;

        // ������������� ������ ��� ��, ��� ������� �����
        player.transform.forward = wallNormal;

        // ��������� ���������� �������
        targetWall.isTeleporting = true;

        yield return new WaitForSeconds(0.3f);

        isTeleporting = false;
        targetWall.isTeleporting = false;
    }
}