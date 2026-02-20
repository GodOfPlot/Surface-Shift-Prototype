using Cysharp.Threading.Tasks;
using UnityEngine;

[SelectionBase]
public class EnemyController : MonoBehaviour
{
	[SerializeField] private Transform headTransform;

	private GameObject player;
	private LineRenderer lineRenderer;

	private Vector3 shotVector;

	private void Start()
	{
		lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.enabled = false;
		shotVector = headTransform.forward;
	}

	private void Update() => Shot();

	#region Shot

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
			player = other.gameObject;
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
			player = null;
	}

	private void Shot()
	{
		if (player == null)
			return;

		Vector3 to = VectorToPlayer(transform.position, player.transform.position);

		float rotateSpeed = Time.deltaTime * 0.7f;
		shotVector = Vector3.RotateTowards(shotVector, to, rotateSpeed, 0f);
		headTransform.rotation = Quaternion.LookRotation(shotVector);

		if (60 > Vector3.Angle(shotVector, to))
		{
			RayShot(shotVector);
		}
	}

	private async void RayShot(Vector3 vector)
	{
		float maxDistance = 50f;
		Ray ray = new(transform.position, vector);

		Vector3 endPoint = transform.position + vector * maxDistance;

		if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
		{
			endPoint = hit.point;
			if (hit.collider.CompareTag("Player"))
				hit.collider.GetComponentInParent<RestartSystem>().RestartWithEvent();
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
