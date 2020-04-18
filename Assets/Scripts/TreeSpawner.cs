using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class TreeSpawner : MonoBehaviour {
	[SerializeField] private Transform planet;
	[SerializeField] private Transform treeHolder;
	[SerializeField] private GameObject[] treePrefabs;
	[SerializeField] private float treeAmount;

	private void Start() {
		GenerateTrees();
	}

	private void GenerateTrees() {
		var planetMeshFilter = planet.GetComponentInChildren<MeshFilter>();
		var planetVertices = planetMeshFilter.mesh.vertices;

		for (int i = 0; i < treeAmount; i++) {
			var vertexLocalPos = planetVertices.ElementAt(Random.Range(0, planetVertices.Length));
			var spawnPos = planetMeshFilter.transform.TransformPoint(vertexLocalPos);
			SpawnTree(spawnPos);
		}
	}

	private void SpawnTree(Vector3 spawnPos) {
		var treeToSpawn = treePrefabs.ElementAt(Random.Range(0, treePrefabs.Length));
		
		var tree = Instantiate(treeToSpawn, spawnPos, Quaternion.identity, treeHolder);
		var planetToTreeVector = (spawnPos - planet.position).normalized;

		tree.transform.up = planetToTreeVector;
	}
}
