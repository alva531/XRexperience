using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.InputSystem;

public class ScanTest : MonoBehaviour
{
    [SerializeField]
    ARRaycastManager m_RaycastManager;
    List<ARRaycastHit> m_hits = new List<ARRaycastHit>();
    [SerializeField]
    GameObject thingtospawn;
    //[SerializeField]
    //GameObject thingtospawn2;
    [SerializeField]
    GameObject ArPlaneMgr;
    Camera ARcam;
    GameObject instanceofThingtoSpawn;
    GameObject instanceofThingtoSpawn2;
    float staleDist = 0;
    int LayerIgnoreRaycast;
    public XRIDefaultInputActions assEt;

    bool hitThingToSpawn = false;

    bool firstSpawn = false;
    
    [SerializeField] 
    private GameObject uiText2;

    [SerializeField] 
    private GameObject moveTutorial;

    [SerializeField] 
    private GameObject rotationTutorial;

    [SerializeField]
    InputAction touchAR;
    [SerializeField]
    InputAction touchCountAR;
    
    [SerializeField]
    InputAction touchPosAR;

    [SerializeField]
    InputAction dragCurrentPos;

    [SerializeField]
    InputAction dragAR;

    [SerializeField]
    InputAction twistStartAR;

    [SerializeField]
    InputAction twistAR;

    float dragSpeed = 2;

    Vector2 LastPos;

    void Awake()
    {
        LastPos = new Vector2(0,0);
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
        dragAR = assEt.FindAction("Drag Delta");
        dragCurrentPos = assEt.FindAction("Drag Current Position");
        twistAR = assEt.FindAction("Twist Delta Rotation");
        twistStartAR = assEt.FindAction("Twist Start Position");
        
        touchAR.Enable();
        touchCountAR.Enable();
        touchPosAR.Enable();
        dragCurrentPos.Enable();
        dragAR.Enable();
        twistStartAR.Enable(); 
        twistAR.Enable();
    }

    void Update()
    {
        //print(touchPosAR.ReadValue<Vector2>() );
        //print(touchAR.IsPressed());

        RaycastHit hit;
        Ray ray = ARcam.ScreenPointToRay(touchPosAR.ReadValue<Vector2>());

        if (touchAR.IsPressed())
        {

            m_RaycastManager.Raycast(ray, m_hits);
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.tag == "Spawnable")
                    {
                        instanceofThingtoSpawn = hit.collider.gameObject;
                        hitThingToSpawn = true;
                    }
                     else if (instanceofThingtoSpawn == null)
                    {
                        SpawnPrefab(m_hits[0].pose.position);
                    }
                }

            // Posición

            if (hitThingToSpawn == true && touchAR.IsPressed())
            {
                if (instanceofThingtoSpawn != null && LastPos != dragCurrentPos.ReadValue<Vector2>())
                {
                    instanceofThingtoSpawn.transform.position += new Vector3(dragAR.ReadValue<Vector2>().normalized.x * dragSpeed * Time.deltaTime, 0, dragAR.ReadValue<Vector2>().normalized.y * dragSpeed * Time.deltaTime);
                }
            }
            LastPos = dragCurrentPos.ReadValue<Vector2>();
            // Rotación
            
            if (instanceofThingtoSpawn != null)
            {
                float twistDelta = twistAR.ReadValue<float>();

                if (Mathf.Abs(twistDelta) > 0.01f)
                {
                    instanceofThingtoSpawn.transform.Rotate(Vector3.up, twistDelta * Time.deltaTime * 100f, Space.World); 
                }
            }

            return;
        }

        if (touchAR.IsPressed() == false)
        {
            hitThingToSpawn = false;
        }

    }

    private void SpawnPrefab(Vector3 spawnPosition)
    {
        instanceofThingtoSpawn = Instantiate(thingtospawn, spawnPosition, Quaternion.identity);
        //instanceofThingtoSpawn2 = Instantiate(thingtospawn2, spawnPosition, Quaternion.identity);

        //foreach (var plane in ArPlaneMgr.GetComponent<ARPlaneManager>().trackables)
        //{
            //plane.gameObject.GetComponent<ARPlaneMeshVisualizer>().enabled = false;
        //}
        firstSpawn = true;

        if (firstSpawn)
        {
            uiText2.GetComponent<Animator>().SetTrigger("Disappear");
            StartCoroutine(Tutorial());
        }

    }

    private IEnumerator Tutorial()
    {
        
        yield return new WaitForSeconds(0.25f);
        
        if (moveTutorial != null)
        {
            moveTutorial.SetActive(true);
        }
        else
        {
            rotationTutorial.SetActive(true);
        }

        yield return new WaitForSeconds(6f);

        rotationTutorial.SetActive(true);
    }

    public void Delete()
    {
        Destroy(instanceofThingtoSpawn);
        hitThingToSpawn = false;
        instanceofThingtoSpawn = null;
    }
}
