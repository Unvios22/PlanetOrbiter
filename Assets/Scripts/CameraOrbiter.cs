using UnityEngine;

public class CameraOrbiter : MonoBehaviour {
	[SerializeField] private Transform orbitingTarget;

	[SerializeField] private float orbitTranslationSpeed;
	[SerializeField] private float orbitTranslationReactionTime;
	
	[SerializeField] private float orbitDistanceChangeSpeed;
	[SerializeField] private float orbitDistanceChangeReactionTime;

	[SerializeField] private float initialOrbitDistance;
	[SerializeField] private float minOrbitDistance;
	[SerializeField] private float maxOrbitDistance;

	private Vector2 _cameraStrafingMoveVector;
	private float _inputOrbitingDistanceChange;
	
	private Vector2 _mousePosThisFrame;
	private Vector2 _mousePosLastFrame;
	
	private float _desiredOrbitDistance;
	private float _currentOrbitDistance;
	private float _currentOrbitDistanceChangeSpeed;

	private Vector2 _desiredOrbitTranslation;
	private Vector2 _currentOrbitTranslation;
	private Vector2 _currentOrbitTranslationSpeed;
	
	private Transform _transform;

	private void Start() {
		CacheAndInitializeValues();
		RepositionAndRealignCameraToTarget();
	}

	private void CacheAndInitializeValues() {
		_transform = transform;
		_currentOrbitDistance = initialOrbitDistance;
		_desiredOrbitDistance = initialOrbitDistance;
	}

	private void Update() {
		ReadUserInput();
		ApplyUserInput();
	}

	private void ReadUserInput() {
		ReadCameraStrafingInput();
		ReadOrbitingDistanceChangeInput();
	}
	
	private void ReadCameraStrafingInput() {
		_mousePosThisFrame = Input.mousePosition;
		if (!Input.GetKey(KeyCode.Mouse0)) {
			_cameraStrafingMoveVector = Vector2.zero;
		} else {
			_cameraStrafingMoveVector = _mousePosLastFrame - _mousePosThisFrame;
		}
		_mousePosLastFrame = _mousePosThisFrame;
	}

	private void ReadOrbitingDistanceChangeInput() {
		_inputOrbitingDistanceChange = -1 * Input.mouseScrollDelta.y;
	}
	
	private void ApplyUserInput() {
		ApplyCameraStrafingInput();
		CalculateOrbitDistance();
		RepositionAndRealignCameraToTarget();
	}

	private void ApplyCameraStrafingInput() {
		var currentToMaxOrbitDistanceRatio = _currentOrbitDistance / maxOrbitDistance;
		_desiredOrbitTranslation =
			_cameraStrafingMoveVector * (orbitTranslationSpeed * currentToMaxOrbitDistanceRatio);
		
		_currentOrbitTranslation = Vector2.SmoothDamp(_currentOrbitTranslation, _desiredOrbitTranslation,
			ref _currentOrbitTranslationSpeed, orbitTranslationReactionTime);
		
		_transform.Translate(_currentOrbitTranslation);
	}

	private void CalculateOrbitDistance() {
		_desiredOrbitDistance += _inputOrbitingDistanceChange * orbitDistanceChangeSpeed;
		_desiredOrbitDistance = Mathf.Clamp(_desiredOrbitDistance, minOrbitDistance, maxOrbitDistance);
		_currentOrbitDistance = Mathf.SmoothDamp(_currentOrbitDistance, _desiredOrbitDistance,
			ref _currentOrbitDistanceChangeSpeed, orbitDistanceChangeReactionTime);
	}

	private void RepositionAndRealignCameraToTarget() {
		var targetPosition = orbitingTarget.position;
		var vectorTowardsTarget = (targetPosition - _transform.position).normalized;
		
		var targetToDesiredOrbitPositionVector = vectorTowardsTarget * (-1 * _currentOrbitDistance);
		
		_transform.position = targetPosition + targetToDesiredOrbitPositionVector;
		_transform.rotation = Quaternion.LookRotation(vectorTowardsTarget, _transform.up);
	}
}
