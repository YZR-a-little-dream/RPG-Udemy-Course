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

    [Header("Ailment color")]
    [SerializeField] private Color[] chillColor;
    [SerializeField] private Color[] igniteColor;
    [SerializeField] private Color[] shockColor;

    private void Start() {
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMat = sr.material;
    }

    public void Maketransparent(bool _transparent)
    {
        if(_transparent)
        {
            sr.color = Color.clear;
        }
        else
        {
            sr.color = Color.white;
        }
    }
    
    //利用协程实现受击闪烁特效
    private IEnumerator FlashFx()
    {
        sr.material = hitMat;

        Color currentColor = sr.color;
        sr.color = Color.white;

        yield return new WaitForSeconds(FlashDuration);

        sr.color = currentColor;

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

    private void CannelColorChange()
    {
        CancelInvoke();
        sr.color = Color.white;
    }

    public void ShockFxFor(float _seconds)
    {
        InvokeRepeating("ShockColorFx",0,0.3f);
        Invoke("CannelColorChange", _seconds);
    }

    public void chillFxFor(float _seconds)
    {
        InvokeRepeating("ChillColorFx",0,0.3f);
        Invoke("CannelColorChange", _seconds);
    }

    public void igniteFxFor(float _seconds)
    {
        InvokeRepeating("IgniteColorFx",0,0.3f);
        Invoke("CannelColorChange", _seconds);
    }

    public void IgniteColorFx()
    {
        if(sr.color != igniteColor[0])
            sr.color = igniteColor[0];
        else
            sr.color = igniteColor[1];
    }

    public void ShockColorFx()
    {
        if(sr.color != shockColor[0])
            sr.color = shockColor[0];
        else
            sr.color = shockColor[1];
    }

    public void ChillColorFx()
    {
        if(sr.color != chillColor[0])
            sr.color = chillColor[0];
        else
            sr.color = chillColor[1];
    }

}
