using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicAttack : MonoBehaviour
{
    public float speed = 5f;
    private Vector3 targetPosition;

    public void SetTarget(Vector3 target)
    {
        targetPosition = target;
        StartCoroutine(MoveToTarget());
    }

    private IEnumerator MoveToTarget()
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }

        // Aplicar l�gica de da�o aqu� si es necesario

        Destroy(gameObject); // Destruir el prefab despu�s de alcanzar el objetivo
    }
}
