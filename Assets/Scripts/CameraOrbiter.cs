using UnityEngine;

public class CameraOrbiter : MonoBehaviour {

	[SerializeField] private Transform orbitRelativeTo;

	[SerializeField] private float orbitSpeed;
	[SerializeField] private float orbitDistance;

	private Vector2 _planetRotationVector;
	//vector containing mouse position change
	
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
		_mousePosThisFrame = Input.mousePosition;
		if (!Input.GetKey(KeyCode.Mouse0)) {
			_planetRotationVector = Vector2.zero;
		} else {
			_planetRotationVector = _mousePosLastFrame - _mousePosThisFrame;
		}
		_mousePosLastFrame = _mousePosThisFrame;
	}

	private void ApplyUserInput() {
		transform.Translate(_planetRotationVector * (Time.deltaTime * orbitSpeed));
	}

	private void RealignCamera() {
		var vectorTowardsPlanet = (orbitRelativeTo.position - _transform.position).normalized;
		_transform.position = vectorTowardsPlanet * (-1 * orbitDistance);
		_transform.rotation = Quaternion.LookRotation(vectorTowardsPlanet, _transform.up);
	}
}
