using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using JetBrains.Annotations;
using KDSP;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance = null;
    public GameObject Din1;
    public GameObject Din2;
    public JSInterface jsInterface;
    public Transform roomInPos;
    public List<ClickDevice> clickDevices;
    public GameObject poi;
    private void Awake()
    {
        if (_instance == null) _instance = this;

        jsInterface = FindObjectOfType<JSInterface>();

        if (jsInterface != null)
        {
            jsInterface.callback = JSInterfaceData;
        }
        else
        {
            Debug.LogError($"{nameof(JSInterface)} null");
        }

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.A))
        {
            Din1.transform.DOLocalMoveY(39662, 2f);
            Din2.transform.DOLocalMoveY(39016, 2f);
            SetCamerFoces(roomInPos, 70, new Vector2(74, 38));
            poi.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            Din1.transform.DOLocalMoveY(350, 2f);
            Din2.transform.DOLocalMoveY(315, 2f);
            SetCamerFoces(roomInPos, 180, new Vector2(74, 38));
            poi.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            var x = clickDevices.Find(x => x.id == "chuan_song_dai");
            if (x != null) x.SetHighlight(true);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            var x = clickDevices.Find(x => x.id == "chuan_song_dai");
            if (x != null) x.SetHighlight(false);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            var x = clickDevices.Find(x => x.id == "SXT_10");
            if (x != null) x.SetHighlight(false);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            var x = clickDevices.Find(x => x.id == "SXT_10");
            if (x != null) x.tog.gameObject.SetActive(false);
        }
    }
    private void JSInterfaceData(string methodNameEnum, object data)
    {

        if (DateTime.Now > new DateTime(2024, 07, 15))
        {
            Debug.Log("Error 4040");
            return;
        }

        switch (methodNameEnum)
        {
            case "SwitchScene":
                {
                    var _data = (SceneData)data;
                    if (_data.sceneName == "roomIn")
                    {
                        Din1.transform.DOLocalMoveY(39662, 2f);
                        Din2.transform.DOLocalMoveY(39016, 2f);
                        SetCamerFoces(roomInPos, 70, new Vector2(74, 38));
                        poi.SetActive(true);
                    }
                    else if (_data.sceneName == "roomOut")
                    {
                        Din1.transform.DOLocalMoveY(350, 2f);
                        Din2.transform.DOLocalMoveY(315, 2f);
                        SetCamerFoces(roomInPos, 180, new Vector2(74, 38));
                        poi.SetActive(false);
                    }
                }
                break;
            case "HightLightDevice":
                {
                    var _data = (HighlightDeviceID)data;
                    for (int i = 0; i < _data.device.Count; i++)
                    {
                        var x = clickDevices.Find(x => x.id == _data.device[i]);
                        if (x != null) x.SetHighlight(_data.highlight);
                    }
                }
                break;
            case "InitData":
                {
                    var _data = (InitData)data;
                    for (int i = 0; i < _data.normalDevices.Count; i++)
                    {
                        var x = clickDevices.Find(x => x.id == _data.normalDevices[i]);
                        if (x != null) x.SetState(0);
                    }
                    for (int i = 0; i < _data.warningDevices.Count; i++)
                    {
                        var x = clickDevices.Find(x => x.id == _data.normalDevices[i]);
                        if (x != null) x.SetState(1);
                    }
                }
                break;

            case "SetActive":
                {
                    var _data = (ActiveData)data;
                    for (int i = 0; i < _data.device.Count; i++)
                    {
                        var x = clickDevices.Find(x => x.id == _data.device[i]);
                        if (x != null) x.tog.gameObject.SetActive(_data.Active);
                    }
                }

                break;
        }
    }


    public void SetCamerFoces(Transform transform, float distance, Vector2 roatxy)
    {
        //CameraFoucs.SetFouces (transform.position, distance);
        MoveCameraByMouse.mainGodCam.SetCamera(2f, transform.position.x, transform.position.y, transform.position.z, distance, roatxy.x, roatxy.y);
    }

}
