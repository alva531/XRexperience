using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Unity.Mathematics;

public class ScanTest : MonoBehaviour
{
    [SerializeField]
    ARRaycastManager m_RaycastManager;
    List<ARRaycastHit> m_hits = new List<ARRaycastHit>();
    [SerializeField]
    GameObject thingtospawn;
    [SerializeField]
    GameObject thingtospawn2;
    [SerializeField]
    GameObject ArPlaneMgr;
    Camera ARcam;
    GameObject instanceofThingtoSpawn;
    GameObject instanceofThingtoSpawn2;
    float staleDist = 0;
    int LayerIgnoreRaycast;
    public XRIDefaultInputActions assEt;
    

    [SerializeField]
    InputAction touchAR;
    [SerializeField]
    InputAction touchCountAR;
    
    [SerializeField]
    InputAction touchPosAR;

    [SerializeField]
    InputAction dragAR;
    void Awake()
    {
        instanceofThingtoSpawn = null;
        ARcam = GameObject.Find("Main Camera").GetComponent<Camera>();
        LayerIgnoreRaycast = LayerMask.NameToLayer("Ignore Raycast");
    }

    void Start()
    {
        assEt = new XRIDefaultInputActions();
        touchAR = assEt.FindAction("Spawn Object");
        touchCountAR = assEt.FindAction("Screen Touch Count");
        touchPosAR =  assEt.FindAction("Tap Start Position");
        dragAR = assEt.FindAction("Drag Test");
        
        touchAR.Enable();
        touchCountAR.Enable();
        touchPosAR.Enable();
        dragAR.Enable();
    }

    void Update()
    {
        print(touchPosAR.ReadValue<Vector2>() );
        print(touchAR.IsPressed());

        RaycastHit hit;
        Ray ray = ARcam.ScreenPointToRay(touchPosAR.ReadValue<Vector2>());

        if (m_RaycastManager.Raycast(touchPosAR.ReadValue<Vector2>(), m_hits) && touchAR.IsPressed())
        {
            if (instanceofThingtoSpawn == null)
            {


                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.tag == "Spawnable")
                    {
                        instanceofThingtoSpawn = hit.collider.gameObject;
                        
                    }
                     else
                    {
                    //     Destroy(GameObject.Find("Canvas"));
                         SpawnPrefab(m_hits[0].pose.position);
                    //     ArPlaneMgr.GetComponent<ARPlaneManager>().enabled = false;
                    //     GameObject[] a = GameObject.FindGameObjectsWithTag("Plane");

                    //     foreach (GameObject ca in a )
                    //     {
                    //         ca.GetComponent<LineRenderer>().enabled = false;
                    //         ca.GetComponent<MeshRenderer>().enabled = false;
                    //         ca.GetComponent<ARPlaneMeshVisualizer>().enabled = false;
                    //         ca.layer = LayerIgnoreRaycast;
                    //     }
                    }
                    
                }
            }
/*             else if ( touchAR.IsPressed())
            {
                if (Physics.Raycast(ray, out hit) && (!instanceofThingtoSpawn2.transform.GetChild(5).gameObject.activeSelf && !instanceofThingtoSpawn2.transform.GetChild(9).gameObject.activeSelf))
                {
                    print(hit.collider.gameObject.tag);
                }
            } */

            if (Mathf.Abs(dragAR.ReadValue<Vector2>().x) > 0f)
            {
                if (instanceofThingtoSpawn != null)
                {
                    instanceofThingtoSpawn.transform.position += new Vector3(dragAR.ReadValue<Vector2>().normalized.x * Time.deltaTime, 0, dragAR.ReadValue<Vector2>().normalized.y * Time.deltaTime);
                }
            }

            return;
        }

    }

    private void SpawnPrefab(Vector3 spawnPosition)
    {
        instanceofThingtoSpawn = Instantiate(thingtospawn, spawnPosition, Quaternion.identity);
        instanceofThingtoSpawn2 = Instantiate(thingtospawn2, spawnPosition, Quaternion.identity);
    }
}
