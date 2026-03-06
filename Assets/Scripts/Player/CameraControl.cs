using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControl : MonoBehaviour
{
	[SerializeField] private Transform _cameraT;
	[SerializeField] private float _mouseSensitivity = 2.5f;
	[SerializeField] private float _minY = -90f;
	[SerializeField] private float _maxY = 90f;
	[SerializeField] private InputActionReference _mouse;

	private float _xRotation;
	private float _yRotation;

	private void OnEnable()
	{
		_mouse.action.Enable();
	}

	private void OnDisable()
	{
		_mouse.action.Disable();
	}

	private void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	private void Update()
	{
		Vector2 mouseDelta = _mouse.action.ReadValue<Vector2>();

		float mouseX = mouseDelta.x * _mouseSensitivity * 100f * Time.deltaTime;
		float mouseY = mouseDelta.y * _mouseSensitivity * 100f * Time.deltaTime;

		_yRotation += mouseX;
		_xRotation -= mouseY;
		_xRotation = Mathf.Clamp(_xRotation, _minY, _maxY);

		_cameraT.rotation = Quaternion.Euler(_xRotation, _yRotation, 0f);
	}

	public float SetSensitivity { set => _mouseSensitivity = value; }
}
