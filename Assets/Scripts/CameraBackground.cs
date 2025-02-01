using UnityEngine;
using UnityEngine.UI;

public class CameraBackground : MonoBehaviour
{
    private WebCamTexture webCamTexture;

    void Start()
    {
        // Obtén las cámaras disponibles en el dispositivo
        WebCamDevice[] devices = WebCamTexture.devices;

        if (devices.Length > 0)
        {
            // Usa la primera cámara disponible
            webCamTexture = new WebCamTexture(devices[0].name);

            // Aplica la textura al RawImage
            RawImage rawImage = GetComponent<RawImage>();
            rawImage.texture = webCamTexture;
            rawImage.material.mainTexture = webCamTexture;

            // Inicia la cámara
            webCamTexture.Play();

            // Ajusta el aspecto y la rotación una vez que la cámara comienza a transmitir
            StartCoroutine(AdjustAspectAndRotation(rawImage));
        }
        else
        {
            Debug.LogError("No se encontró ninguna cámara en el dispositivo.");
        }
    }

    System.Collections.IEnumerator AdjustAspectAndRotation(RawImage rawImage)
    {
        // Espera hasta que la cámara comience a enviar datos
        while (webCamTexture.width <= 16)
        {
            yield return null;
        }

        // Calcula la relación de aspecto de la cámara
        float cameraAspect = (float)webCamTexture.width / webCamTexture.height;
        float screenAspect = (float)Screen.width / Screen.height;

        RectTransform rectTransform = rawImage.rectTransform;

        // Ajusta el tamaño del RawImage para que respete la relación de aspecto
        if (cameraAspect > screenAspect)
        {
            // La cámara es más ancha que la pantalla
            rectTransform.sizeDelta = new Vector2(Screen.height * cameraAspect, Screen.height);
        }
        else
        {
            // La cámara es más alta que la pantalla
            rectTransform.sizeDelta = new Vector2(Screen.width, Screen.width / cameraAspect);
        }

        // Ajusta la rotación del RawImage
        rectTransform.localEulerAngles = new Vector3(0, 0, -webCamTexture.videoRotationAngle);

        // Si la cámara está reflejada (algunas cámaras frontales), voltea horizontalmente
        if (webCamTexture.videoVerticallyMirrored)
        {
            rectTransform.localScale = new Vector3(rectTransform.localScale.x, -rectTransform.localScale.y, rectTransform.localScale.z);
        }
    }

    void OnDestroy()
    {
        // Detén la cámara cuando el objeto se destruya
        if (webCamTexture != null)
        {
            webCamTexture.Stop();
        }
    }
}
