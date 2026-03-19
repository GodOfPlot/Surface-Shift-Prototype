using UnityEngine;
using UnityEngine.UI;

public class Stopwatch : MonoBehaviour
{
	[SerializeField] private Text timeText;
	[SerializeField] private Text completeTime;
	[SerializeField] private Data data;
	[SerializeField] private float elapsedTime = 0f;

	private bool isRunning = false;

	public float ElapsedTime => elapsedTime;

	private void Start()
	{
		timeText.text = "Time: 0.00 seconds";
		if (data.CompletionTime > 0)
			completeTime.text = $"Completion Time: {data.CompletionTime}";
		else
			completeTime.text = "Completion Time: N/A";
		isRunning = true;
	}

	private void Update()
	{
		if (isRunning)
			elapsedTime += Time.deltaTime;
		timeText.text = $"Time: {elapsedTime} seconds";
	}

	public void StartTime()
	{
		isRunning = true;
		elapsedTime = 0f;
	}

	public void Complete()
	{
		isRunning = false;
		if (data.CompletionTime > elapsedTime || data.CompletionTime == 0)
			data.CompletionTime = elapsedTime;
		completeTime.text = $"Completion Time: {data.CompletionTime}";
	}
}
