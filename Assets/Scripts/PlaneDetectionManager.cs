using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PlaneDetectionManager : MonoBehaviour
{
    [SerializeField] 
    private ARPlaneManager arPlaneManager;

    [SerializeField] 
    private GameObject uiText1;

    [SerializeField] 
    private GameObject uiText2;

    private void OnEnable()
    {
        if (arPlaneManager != null)
        {
            arPlaneManager.planesChanged += OnPlanesChanged;
        }
    }

    private void OnDisable()
    {
        if (arPlaneManager != null)
        {
            arPlaneManager.planesChanged -= OnPlanesChanged;
        }
    }

    private void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        int planeCount = arPlaneManager.trackables.count;

        if (planeCount > 0)
        {
            uiText1.GetComponent<Animator>().SetTrigger("Disappear");
            uiText2.SetActive(true);
        }
    }
}
