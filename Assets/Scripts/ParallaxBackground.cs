using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�����ڱ����ϵ���ɫ�ƶ���ʱ���ñ������������һ��
public class ParallaxBackground : MonoBehaviour
{
    private GameObject cam;                          //��ȡ������ͷ

    [SerializeField] private float parallaxEffect;  //�Ӳ�������

    private float xPosition;                        //x���λ��  

    private float backGroundlength;                 //����ʵ��ѭ������

    private void Start() {
        cam = GameObject.Find("Main Camera");
        xPosition = transform.position.x;

        backGroundlength = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void Update() 
    {
        //�൱�������λ�ƺͱ���λ�Ƶľ����ֵ
        float distanceMove = cam.transform.position.x * (1-parallaxEffect);

        float distanceToMove = cam.transform.position.x * parallaxEffect;      
        transform.position = new Vector3(xPosition + distanceToMove,transform.position.y);

        if(distanceMove > xPosition + backGroundlength)
            xPosition = xPosition + backGroundlength;
        else if(distanceMove < xPosition - backGroundlength)
            xPosition = xPosition - backGroundlength;
    }
}
