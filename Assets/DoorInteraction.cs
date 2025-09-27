using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    [Header("Door Settings")]
    public Transform doorHinge;          // Assign the door object (the part that rotates)
    public Vector3 openRotation = new Vector3(0, 90, 0); // Local rotation when open
    public float openSpeed = 3f;

    [Header("Input Settings")]
    public KeyCode interactKey = KeyCode.E;

    private Quaternion closedRot;
    private Quaternion targetRot;
    private bool isOpen = false;
    private bool isPlayerNearby = false;

    void Start()
    {
        if (doorHinge == null) doorHinge = transform; // fallback: rotate self
        closedRot = doorHinge.localRotation;
        targetRot = closedRot;
    }

    void Update()
    {
        // Smoothly rotate toward target
        doorHinge.localRotation = Quaternion.Lerp(doorHinge.localRotation, targetRot, Time.deltaTime * openSpeed);

        // If player presses E nearby → toggle door
        if (isPlayerNearby && Input.GetKeyDown(interactKey))
        {
            if (isOpen)
                targetRot = closedRot;
            else
                targetRot = Quaternion.Euler(openRotation);
            
            isOpen = !isOpen;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            isPlayerNearby = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            isPlayerNearby = false;
    }
}
