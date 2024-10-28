using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer sr;

    [Header("Flash FX")]
    [SerializeField] private float FlashDuration;   //闪烁时间
    [SerializeField] private Material hitMat;       //受击时的材质（一闪一闪的）
    private Material originalMat;                   //原本材质

    private void Start() {
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMat = sr.material;
    }
    
    //利用协程实现受击闪烁特效
    private IEnumerator FlashFx()
    {
        sr.material = hitMat;

        yield return new WaitForSeconds(FlashDuration);

        sr.material = originalMat;
    }
    
    //红白闪烁
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
