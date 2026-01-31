using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
	[Header("Jump Settings")]
	[SerializeField] private float jumpForce = 1000f;
	[SerializeField] private float fixationTime = 3.0f;

	[Space]
	[SerializeField] private float jumpAttachDelay = 0.1f;

	[Header("Input Actions")]
	[SerializeField] private InputActionReference jump;

	// Timers
	private readonly Timer startedJumpTimer = new();
	private readonly Timer groundedTimer = new();

	// State
	private bool startedJumpCheck = false;
	private bool groundedJumpCheck = true;

	// Components
	private Rigidbody rb;

	private void OnEnable() => jump.action.performed += _ => InputStartJump();
	private void OnDisable() => jump.action.performed -= _ => InputStartJump();

	private void Start() => rb = GetComponent<Rigidbody>();

	private void FixedUpdate()
	{
		if (startedJumpTimer.IsRunning)
			startedJumpCheck = startedJumpTimer.Update(Time.fixedDeltaTime);

		if (groundedTimer.IsRunning && groundedTimer.Update(Time.fixedDeltaTime))
			Unparent();
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (!startedJumpCheck)
			return;

		string tag = collision.collider.tag;

		switch (tag)
		{
			case "Ground":
				ChangeState(collision.transform);
				break;

			case "Wall":
				ChangeState(collision.transform);
				groundedTimer.Start(fixationTime);
				break;
		}
	}

	public void Unparent()
	{
		transform.SetParent(null);
		rb.isKinematic = false;
	}

	private void ChangeState(Transform parent)
	{
		transform.SetParent(parent);
		rb.isKinematic = true;
		startedJumpCheck = false;
	}

	private void InputStartJump()
	{
		if (groundedJumpCheck)
			return;

		Unparent();
		StartJump(Camera.main.transform.forward);

		startedJumpTimer.Start(jumpAttachDelay);
		groundedJumpCheck = false;
	}

	private void StartJump(Vector3 force) => rb.AddForce(force * jumpForce, ForceMode.Acceleration);
}