using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectHermes
{
    public class SpikeSpawner : MonoBehaviour
    {

        #region Variables

        [Title("Spike Settings")]
        [SerializeField] private Vector2 direction = Vector2.zero;
        [SerializeField] [Range(0, 20)] private float spikeLifespan = 5;
        [SerializeField] private GameObject spikePrefab;

        [Title("Spawner Settings")]
        [SerializeField] [Range(0, 10)] private float spawnRate = 1;
        [SerializeField] private float spawnOffset = 0;

		#endregion

		#region Methods

		private void Awake()
        {
			InvokeRepeating("SpawnSpike", spawnOffset, spawnRate);
        }

        private void SpawnSpike()
		{
            print("Shot");
            GameObject spike = Instantiate(spikePrefab, transform.position, transform.rotation, transform);
            spike.GetComponent<FlyingSpikeScript>().lifespan = spikeLifespan;
            spike.GetComponent<Rigidbody2D>().velocity = direction;

        }

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.red;

            Gizmos.DrawLine(transform.position, transform.position + new Vector3(direction.x, direction.y, 0));
		}

		#endregion

	}
}
