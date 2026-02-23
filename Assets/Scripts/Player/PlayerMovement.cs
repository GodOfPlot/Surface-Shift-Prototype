using UnityEngine;
using UnityEngine.InputSystem;

[SelectionBase]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(RestartSystem))]
public class PlayerMovement : MonoBehaviour
{
	[Header("Jump Settings")]
	[SerializeField] private float jumpForce = 1000f;
	[SerializeField] private float smallJumpForce = 200f;
	[SerializeField] private float fixationTime = 3.0f;

	[Space]
	[SerializeField] private float jumpAttachDelay = 0.1f;

	[Header("Input Actions")]
	[SerializeField] private InputActionReference jump;
	[SerializeField] private InputActionReference smallJump;

	// Timers
	private readonly Timer startedJumpTimer = new();
	private readonly Timer groundedTimer = new();

	// Surface normal
	private Vector3 normal = Vector3.up;

	// State
	private bool startedJumpCheck = false;
	private bool groundedJumpCheck = true;
	private bool smallJUmpActivated = false;

	// Components
	private Rigidbody rb;
	private RestartSystem restartSystem;

	private void OnEnable()
	{
		jump.action.performed += OnJump;
		smallJump.action.performed += OnSmallJump;
	}

	private void OnDisable()
	{
		jump.action.performed -= OnJump;
		smallJump.action.performed -= OnSmallJump;
	}

	private void Start()
	{
		rb = GetComponent<Rigidbody>();

		restartSystem = GetComponent<RestartSystem>();
	}

	private void FixedUpdate()
	{
		if (startedJumpTimer.IsRunning)
			startedJumpCheck = startedJumpTimer.Update(Time.fixedDeltaTime);

		if (groundedTimer.IsRunning && groundedTimer.Update(Time.fixedDeltaTime))
			Unparent();
	}

	private void OnCollisionEnter(Collision collision)
	{
		string tag = collision.collider.tag;
		normal = collision.contacts[0].normal;

		if (tag == "Enemy")
		{
			restartSystem.RestartWithEvent();
			return;
		}

		if (!startedJumpCheck || smallJUmpActivated)
			return;

		switch (tag)
		{
			case "Ground":
				ChangeStateToFixed(collision.transform);
				break;

			case "Wall":
				ChangeStateToFixed(collision.transform);
				groundedTimer.Start(fixationTime);
				break;
		}
	}

	private void OnJump(InputAction.CallbackContext context) => InputJump();

	private void OnSmallJump(InputAction.CallbackContext context) => InputSmallJump();

	private void CheckSurfaceContact()
	{
		if (rb.isKinematic)
			return;

		groundedJumpCheck = false;

		Vector3 p1 = transform.position;
		Vector3 p2 = transform.position + Vector3.up * 1.0f;
		float radius = 0.6f;

		Collider[] hits = Physics.OverlapCapsule(p1, p2, radius);

		foreach (Collider col in hits)
		{
			if (col.CompareTag("Ground") || col.CompareTag("Wall"))
			{
				groundedJumpCheck = true;
				break;
			}
		}
	}

	public void Unparent()
	{
		transform.SetParent(null);
		rb.isKinematic = false;
		groundedTimer.Stop();
	}

	private void ChangeStateToFixed(Transform parent)
	{
		transform.SetParent(parent);
		rb.isKinematic = true;
		startedJumpCheck = false;
		groundedJumpCheck = true;
	}

	private void InputJump()
	{
		CheckSurfaceContact();
		if (!groundedJumpCheck && !smallJUmpActivated)
			return;

		StartJump(Camera.main.transform.forward, jumpForce);
		groundedJumpCheck = false;
		smallJUmpActivated = false;
	}

	private void InputSmallJump()
	{
		CheckSurfaceContact();
		if (!groundedJumpCheck)
			return;

		StartJump(normal, smallJumpForce);
		smallJUmpActivated = true;
	}

	private void StartJump(Vector3 force, float speed)
	{
		Unparent();
		rb.AddForce(force * speed, ForceMode.Acceleration);
		startedJumpTimer.Start(jumpAttachDelay);
	}
}