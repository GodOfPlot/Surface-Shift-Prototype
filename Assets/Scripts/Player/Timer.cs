public class Timer
{
	private float time;
	private bool running;

	public void Start(float duration)
	{
		time = duration;
		running = true;
	}

	public bool Update(float deltaTime)
	{
		if (!running)
			return false;

		time -= deltaTime;

		if (time <= 0f)
		{
			running = false;
			return true;
		}

		return false;
	}

	public void Stop()
	{
		running = false;
	}

	public bool IsRunning => running;
}