using UnityEngine;

public class CameraOrbiter : MonoBehaviour {

	[SerializeField] private Transform orbitRelativeTo;

	[SerializeField] private float orbitSpeed;
	[SerializeField] private float orbitDistance;
	[SerializeField] private float orbitDistanceChangeSpeed;

	[SerializeField] private float minOrbitDistance;
	[SerializeField] private float maxOrbitDistance;

	private Vector2 _inputPlanetRotationVector;
	private float _inputOrbiterDistanceChange;
	
	private Vector2 _mousePosLastFrame;
	private Vector2 _mousePosThisFrame;
	private Transform _transform;

	private void Start() {
		_transform = transform;
	}

	private void Update() {
		ReadUserInput();
		ApplyUserInput();
		RealignCamera();
	}

	private void ReadUserInput() {
		ReadPlanetRotationInput();
		ReadOrbiterDistanceChangeInput();
	}
	
	private void ReadPlanetRotationInput() {
		_mousePosThisFrame = Input.mousePosition;
		if (!Input.GetKey(KeyCode.Mouse0)) {
			_inputPlanetRotationVector = Vector2.zero;
		} else {
			_inputPlanetRotationVector = _mousePosLastFrame - _mousePosThisFrame;
		}
		_mousePosLastFrame = _mousePosThisFrame;
	}

	private void ReadOrbiterDistanceChangeInput() {
		_inputOrbiterDistanceChange = -1 * Input.mouseScrollDelta.y;
	}
	
	private void ApplyUserInput() {
		transform.Translate(_inputPlanetRotationVector * (Time.deltaTime * orbitSpeed));
		orbitDistance += _inputOrbiterDistanceChange * orbitDistanceChangeSpeed;
		orbitDistance = Mathf.Clamp(orbitDistance, minOrbitDistance, maxOrbitDistance);
	}

	private void RealignCamera() {
		var vectorTowardsPlanet = (orbitRelativeTo.position - _transform.position).normalized;
		_transform.position = vectorTowardsPlanet * (-1 * orbitDistance);
		_transform.rotation = Quaternion.LookRotation(vectorTowardsPlanet, _transform.up);
	}
}
