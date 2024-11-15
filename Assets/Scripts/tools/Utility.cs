using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public static class Utility 
{
    /// <summary>
    /// Ѱ����Ŀ������ĵ���
    /// </summary>
    /// <param name="_transform">Ѱ�ҵ��˵ĳ�ʼλ��</param>
    /// <returns></returns>
    public static Transform FindClosestEnemy(Transform _transform)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_transform.position,25);

        float closestDistance = math.INFINITY;
        Transform closestEnemy = null;

        foreach(var hit in colliders)
        {
            if(hit.GetComponent<Enemy>() != null)
            {
            float distanceToEnemy = Vector2.Distance(_transform.position, hit.transform.position);
                if(distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }
        }

        return closestEnemy;
    }
}
