using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer sr;

    [Header("Flash FX")]
    [SerializeField] private float FlashDuration;   //��˸ʱ��
    [SerializeField] private Material hitMat;       //�ܻ�ʱ�Ĳ��ʣ�һ��һ���ģ�
    private Material originalMat;                   //ԭ������

    private void Start() {
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMat = sr.material;
    }
    
    //����Э��ʵ���ܻ���˸��Ч
    private IEnumerator FlashFx()
    {
        sr.material = hitMat;

        yield return new WaitForSeconds(FlashDuration);

        sr.material = originalMat;
    }
    
    //�����˸
    private void RedColorBlick()
    {
        if(sr.color != Color.white)
            sr.color = Color.white;
        else
            sr.color = Color.red;
    }

    private void CannelRedBlink()
    {
        CancelInvoke();
        sr.color = Color.white;
    }
}
