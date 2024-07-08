using System.Collections;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class Animation : MonoBehaviour
{

    public GameObject Din1;
    public GameObject Din2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public  void   animation(int a)
    {
        //switch (a){

        //    case 1 :
        //        Din1.transform.DOLocalMoveY(39662, 1f);
        //        Din2.transform.DOLocalMoveY(39016, 1f);
        //        break;

        //}
        if (a == 1)
        {
            Din1.transform.DOLocalMoveY(39662, 2f);
            Din2.transform.DOLocalMoveY(39016, 2f);
            Debug.Log("111");

        }


        else 
        {
            Din1.transform.DOLocalMoveY(350, 2f);
            Din2.transform.DOLocalMoveY(315, 2f);
            Debug.Log("22");

        }


    } 


}
