using UnityEngine;

public class CameraOrbiter : MonoBehaviour {

	[SerializeField] private Transform orbitRelativeTo;

	[SerializeField] private float orbitSpeed;
	[SerializeField] private float initialOrbitDistance;
	[SerializeField] private float orbitDistanceChangeSpeed;
	[SerializeField] private float orbitDistanceChangeReactTime;

	[SerializeField] private float minOrbitDistance;
	[SerializeField] private float maxOrbitDistance;

	private Vector2 _inputPlanetRotationVector;
	private float _inputOrbiterDistanceChange;
	
	private Vector2 _mousePosLastFrame;
	private Vector2 _mousePosThisFrame;
	private Transform _transform;
	private float _desiredOrbitDistance;
	private float _currentOrbitDistance;
	private float _currentOrbitDistanceChangeSpeed;

	private void Start() {
		_transform = transform;
		_currentOrbitDistance = initialOrbitDistance;
		_desiredOrbitDistance = initialOrbitDistance;
		RealignCamera();
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
		ApplyPlanetRotationInput();
		ApplyOrbitDistanceChangeInput();
	}

	private void ApplyPlanetRotationInput() {
		var currentToMaxOrbitDistanceRatio = _currentOrbitDistance / maxOrbitDistance;
		transform.Translate(_inputPlanetRotationVector * (Time.deltaTime * orbitSpeed * currentToMaxOrbitDistanceRatio));
	}

	private void ApplyOrbitDistanceChangeInput() {
		_desiredOrbitDistance += _inputOrbiterDistanceChange * orbitDistanceChangeSpeed;
		_desiredOrbitDistance = Mathf.Clamp(_desiredOrbitDistance, minOrbitDistance, maxOrbitDistance);
		_currentOrbitDistance = Mathf.SmoothDamp(_currentOrbitDistance, _desiredOrbitDistance,
			ref _currentOrbitDistanceChangeSpeed, orbitDistanceChangeReactTime);
	}

	private void RealignCamera() {
		var vectorTowardsPlanet = (orbitRelativeTo.position - _transform.position).normalized;
		_transform.position = vectorTowardsPlanet * (-1 * _currentOrbitDistance);
		_transform.rotation = Quaternion.LookRotation(vectorTowardsPlanet, _transform.up);
	}
}
