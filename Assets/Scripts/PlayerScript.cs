using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private CharacterController _characterController;
    private GameObject _headGameObject;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float verticalRotateDump;
    [SerializeField] private float maxInteractionDistance;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _headGameObject = GetComponentInChildren<Camera>().gameObject;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        MovePlayer();
        RotatePlayerView();
        CheckForDoorInteraction();
    }

    private void CheckForDoorInteraction()
    {
        Transform _headTransform = _headGameObject.transform;
        if (Physics.Raycast(_headTransform.position, _headTransform.forward, out RaycastHit hitInfo, maxInteractionDistance))
        {
            if (hitInfo.transform.TryGetComponent<DoorScript>(out DoorScript door) && Input.GetKey(KeyCode.E))
            {
                door.Interact();
            }
        }
    }

    private void RotatePlayerView()
    {
        float mouseX = Input.GetAxis("Mouse X");
        RotatePlayerOnX(mouseX);

        float mouseY = Input.GetAxis("Mouse Y");
        RotatePlayerOnY(mouseY);
    }

    private void RotatePlayerOnX(float mouseX)
    {
        float rotationAmount = mouseX * Time.deltaTime * rotationSpeed;
        transform.Rotate(Vector3.up, rotationAmount);
    }

    private void RotatePlayerOnY(float mouseY)
    {
        float rotationAmount = -mouseY * Time.deltaTime * rotationSpeed;
        float currentRotation = _headGameObject.transform.localRotation.eulerAngles.x;
        float dumpedRotation = CalculateDumpedRotation(currentRotation, rotationAmount);
        _headGameObject.transform.localRotation = Quaternion.Euler(dumpedRotation, 0, 0);
    }

    private float CalculateDumpedRotation(float currentRotation, float rotationAmount)
    {
        float dumpedRotation;

        if (currentRotation < 180)
            dumpedRotation = Mathf.Clamp(currentRotation + rotationAmount, -verticalRotateDump, verticalRotateDump);
        else
            dumpedRotation = Mathf.Clamp(currentRotation - 360f + rotationAmount, -verticalRotateDump,
                verticalRotateDump);
        return dumpedRotation;
    }

    private void MovePlayer()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalInput, 0, verticalInput) * (Time.deltaTime * moveSpeed);
        _characterController.Move(transform.TransformDirection(movement));
    }
}

