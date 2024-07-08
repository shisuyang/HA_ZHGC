using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poi : MonoBehaviour
{
    public string gateName = "监控";
    public string id;
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!gameObject.activeInHierarchy) return;
        var sceenPos = Camera.main.WorldToScreenPoint(target.position);
        sceenPos.z = 0;
        transform.position = sceenPos;
    }
    public void Clcik()
    {
        GameManager._instance.jsInterface.JSSendGateID(gateName, id);
    }

}
