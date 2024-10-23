using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//挂载在背景上当角色移动的时候，让背景随着摄像机一起动
public class ParallaxBackground : MonoBehaviour
{
    private GameObject cam;                          //获取到摄像头

    [SerializeField] private float parallaxEffect;  //视差作用力

    private float xPosition;                        //x轴的位置  

    private float backGroundlength;                 //用于实现循环背景

    private void Start() {
        cam = GameObject.Find("Main Camera");
        xPosition = transform.position.x;

        backGroundlength = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void Update() 
    {
        //相当于摄像机位移和背景位移的距离差值
        float distanceMove = cam.transform.position.x * (1-parallaxEffect);

        float distanceToMove = cam.transform.position.x * parallaxEffect;      
        transform.position = new Vector3(xPosition + distanceToMove,transform.position.y);

        if(distanceMove > xPosition + backGroundlength)
            xPosition = xPosition + backGroundlength;
        else if(distanceMove < xPosition - backGroundlength)
            xPosition = xPosition - backGroundlength;
    }
}
