using UnityEngine;

public class PlanetRotator : MonoBehaviour {
	[SerializeField] private Transform planetTransform;
	[SerializeField] private Vector3 rotationAxis;
	[SerializeField] private float rotationSpeed;

	private void Update() {
		planetTransform.RotateAround(planetTransform.position, rotationAxis, rotationSpeed * Time.deltaTime);
	}
}
