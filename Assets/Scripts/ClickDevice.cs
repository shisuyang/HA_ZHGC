using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickDevice : MonoBehaviour, IPointerDownHandler
{
    public string gateName;
    public string id;


    public Material highlightMat;
    public Material[] oldmaterial;
    public MeshRenderer meshRenderer;
    bool _b;
    public bool isPoi;
    public Toggle poitoggle;


    public Toggle tog;
    // Start is called before the first frame update
    void Start()
    {
        if (!isPoi)
        {
            meshRenderer = GetComponent<MeshRenderer>();
            oldmaterial = meshRenderer.sharedMaterials;
        }
        else

        {
            tog = Instantiate(poitoggle, poitoggle.transform.parent);
            tog.gameObject.SetActive(true);
            tog.onValueChanged.AddListener((b) =>
            {
                if (b)
                {
                    GameManager._instance.jsInterface.JSSendGateID(gateName, id);
                }
            });
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (!isPoi) return;
        if (!tog.gameObject.activeInHierarchy) return;
        var screenPos = Camera.main.WorldToScreenPoint(transform.position);
        if (screenPos.z <= 0)
        {
            screenPos = 100000 * Vector3.left;
        }
        else
        {
            screenPos.z = 0;
        }
        tog.transform.position = screenPos;



    }
    public void SetHighlight(bool b)
    {
        if (isPoi)
        {
            tog.isOn = b;
        }
        else
        {
            meshRenderer.sharedMaterial = b ? highlightMat : oldmaterial[0];
            _b = b;
        }


    }
    public void SetState(int state)
    {
        //  tip.sharedMaterial = materials[state];
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (isPoi) return;
        Debug.Log("点击了 " + name);


        SetHighlight(!_b);

        GameManager._instance.jsInterface.JSSendGateID(gateName, id);
    }


}
