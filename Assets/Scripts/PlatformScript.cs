using Unity.Cinemachine;
using UnityEngine;

public class PlatformScript : MonoBehaviour
{
	[SerializeField] private CinemachineSplineCart cart;

	[SerializeField] private float speed = 0.05f;

	private int direction = 1;

	private void Update()
	{
		cart.SplinePosition += speed * direction * Time.deltaTime;

		if (cart.SplinePosition >= 1f)
		{
			cart.SplinePosition = 1f;
			direction = -1;
		}
		else if (cart.SplinePosition <= 0f)
		{
			cart.SplinePosition = 0f;
			direction = 1;
		}
	}
}
