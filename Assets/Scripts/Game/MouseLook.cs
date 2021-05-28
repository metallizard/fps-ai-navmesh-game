using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField]
    private Transform _playerRoot;

    [SerializeField]
    private Transform _lookRoot;

    [SerializeField]
    private float _sensitivity = 5;

    [SerializeField]
    private float _rollAngle = 10;

    [SerializeField]
    private float _rollSpeed = 3;

    [SerializeField]
    private Vector2 _verticalLookLimit = new Vector2(-70, 80);

    private Vector2 _lookAngles;

    private Vector2 _currentMouseLook;
    private float _currentRollAngle;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if(!Game.Instance.IsPaused)
        {
            LookAround();
        }
    }

    void LookAround()
    {
        // Get mouse input information.
        _currentMouseLook = new Vector2(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"));

        // Multiply mouse input with sensitivity field.
        _lookAngles.x += -_currentMouseLook.x * _sensitivity;
        _lookAngles.y += _currentMouseLook.y * _sensitivity;

        // Limit max up and min down value of player look.
        _lookAngles.x = Mathf.Clamp(_lookAngles.x, _verticalLookLimit.x, _verticalLookLimit.y);

        _currentRollAngle = Mathf.Lerp(_currentRollAngle, Input.GetAxisRaw("Mouse X") * _rollAngle, Time.deltaTime * _rollSpeed);

        // Apply mouse input data to player.
        _lookRoot.localRotation = Quaternion.Euler(_lookAngles.x, 0, _currentRollAngle);
        _playerRoot.localRotation = Quaternion.Euler(0, _lookAngles.y, 0);
    }
}