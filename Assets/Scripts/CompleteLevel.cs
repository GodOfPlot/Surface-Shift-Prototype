using UnityEngine;

public class CompleteLevel : MonoBehaviour
{
	[SerializeField] private Stopwatch stopwatch;
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			if (stopwatch != null)
				stopwatch.Complete();
		}
	}
}
