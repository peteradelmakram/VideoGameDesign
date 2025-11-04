using UnityEngine;

public class FlashingLight : MonoBehaviour
{
    public Light pointLight;        // Reference to the light component
    public float flashSpeed = 5f;   // How fast it flashes
    public float flashIntensity = 2f; // Max intensity during flash
    public bool playerNearby = false;

    private float baseIntensity;
    private float timer;

    void Start()
    {
        if (pointLight == null)
            pointLight = GetComponent<Light>();

        baseIntensity = pointLight.intensity;
    }

    void Update()
    {
        if (playerNearby)
        {
            // Create a flashing (sin wave) effect
            timer += Time.deltaTime * flashSpeed;
            pointLight.intensity = baseIntensity + Mathf.PerlinNoise(Time.time * flashSpeed, 0f) * flashIntensity;
        }
        else
        {
            // Gradually return to base intensity
            pointLight.intensity = Mathf.Lerp(pointLight.intensity, baseIntensity, Time.deltaTime * 2f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerNearby = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerNearby = false;
    }
}
