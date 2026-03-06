using UnityEngine;
using UnityEngine.UI;

public class UIChangeSensitivity : MonoBehaviour
{
	[SerializeField] private GameObject _player;

	private CameraControl _cameraControl;
	private Slider _sensitivitySlider;

	private void Start()
	{
		_sensitivitySlider = GetComponent<Slider>();
		_player.TryGetComponent(out _cameraControl);
	}

	public void ChangeSensitivity() => _cameraControl.SetSensitivity = _sensitivitySlider.value;
}
