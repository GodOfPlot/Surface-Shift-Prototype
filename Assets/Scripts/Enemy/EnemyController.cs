using Cysharp.Threading.Tasks;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private GameObject player;
	private LineRenderer lineRenderer;

	private Vector3 shotVector;

	private readonly Timer shotTimer = new();

	private void Start()
	{
		lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.enabled = false;
	}

	private void Update()
	{
		if (shotTimer.Update(Time.deltaTime))
		{
			Shot();
			shotTimer.Start(1f);
		}
	}

	#region Shot

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
			player = other.gameObject;
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			shotVector = transform.forward;
			player = null;
		}
	}

	private void Shot()
	{
		if (player == null)
			return;

		Vector3 to = VectorToPlayer(transform.position, player.transform.position);

		float rotateSpeed = 0.1f;
		shotVector = Vector3.RotateTowards(shotVector, to, rotateSpeed, 0f);
			
		if (30 > Vector3.Angle(shotVector, to))
			RayShot(shotVector);

	}

	private async void RayShot(Vector3 vector)
	{
		float maxDistance = 50f;
		Ray ray = new Ray(transform.position, vector);
		RaycastHit hit;

		Vector3 endPoint = transform.position + vector * maxDistance;

		if (Physics.Raycast(ray, out hit, maxDistance))
		{
			endPoint = hit.point;
			if (hit.collider.CompareTag("Player"))
			{
				Debug.Log("Player hit!");
			}
		}

		lineRenderer.SetPosition(0, transform.position);
		lineRenderer.SetPosition(1, endPoint);
		lineRenderer.enabled = true;

		await UniTask.Delay(100); 

		lineRenderer.enabled = false;
	}

	private Vector3 VectorToPlayer(Vector3 from, Vector3 to) => (to - from).normalized;

	#endregion
}
