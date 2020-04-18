using UnityEngine;

public class GameQuitter : MonoBehaviour {
	private void Update() {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit();
		}
	}
}
