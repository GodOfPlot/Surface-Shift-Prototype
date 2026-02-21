using UnityEngine;
using UnityEngine.Events;

public class RestartSystem : MonoBehaviour
{
	[SerializeField] private GameObject spawnpoint;
	[SerializeField] private UnityEvent onRestart;

	private Rigidbody rb;
	private PlayerMovement playerMovement;
	private CameraControl CameraControl;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
		playerMovement = GetComponent<PlayerMovement>();
		CameraControl = GetComponent<CameraControl>();
		Restart();
	}

	public void RestartWithEvent()
	{
		playerMovement.enabled = false;
		CameraControl.enabled = false;
		VisibleTrue();
		onRestart.Invoke();
	}

	public void Restart()
	{
		VisibleFalse();
		rb.position = spawnpoint.transform.position;
		rb.rotation = spawnpoint.transform.rotation;
		playerMovement.enabled = true;
		CameraControl.enabled = true;
		Debug.Log("Restarted");
	}

	public void VisibleFalse()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	private void VisibleTrue()
	{
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}
}
