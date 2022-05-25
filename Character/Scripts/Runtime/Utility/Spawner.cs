using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using WizardsCode.Character;
using WizardsCode.Character.WorldState;

namespace WizardsCode.BackgroundAI
{
    /// <summary>
    /// A really simple spawner that will create a number of a given prefab within a defined area.
    /// </summary>
    public class Spawner : MonoBehaviour
    {
        [SerializeField, Tooltip("A name that will be appended to each instance. This will also have a number appended to make each unique.")]
        string m_Name = "Spawned";
        [SerializeField, Tooltip("The rules this spawner should obey when spawning.")]
        SpawnerDefinition m_SpawnDefinition;

        [SerializeField, Tooltip("The number of items to spawn on start." +
            " If this is set to 0 then no items will spawn until the duration of the spawn frequency has passed.")]
        int m_SpawnsOnStart = 0;
        [SerializeField, Tooltip("The maximum number of these items to be spawned in world at any one time. " +
            " That is, if there are this many already in the world no new instances will be spawned." +
            " Note that depending on the Spawn Definition each spawn may be more than one prefab.")]
        protected int m_NumberOfSpawns = 5;
        [SerializeField, Tooltip("The radius within which to spawn")]
        float m_Radius = 10;
        [SerializeField, Tooltip("The frequency at which new instances will be spawned if there are fewer than the maximum allowed number.")]
        protected float m_SpawnFrequency = 5;
        [SerializeField, Tooltip("Should the character only be placed on a NavMesh?")]
        bool onNavMesh = false;
        [HideInInspector, SerializeField, Tooltip("The area mask that indicates NavMesh areas that the spawner can spawn characters into.")]
        public int navMeshAreaMask = NavMesh.AllAreas;

        protected List<Transform> m_Spawned = new List<Transform>();
        protected int m_TotalSpawnedCount = 0;
        protected float m_TimeOfNextSpawn = 0;

        /// <summary>
        /// Get all the objects spawned by this spawner.
        /// </summary>
        public List<Transform> Spawned
        {
            get { return m_Spawned; }
        }

        private void Start()
        {
            ActorManager.Instance.RegisterSpawner(this);

            for (int i = m_SpawnsOnStart; i > 0; i--)
            {
                m_TotalSpawnedCount++;
                Spawn(m_TotalSpawnedCount.ToString());
            }

            m_TimeOfNextSpawn = m_SpawnFrequency;
        }

        protected virtual void Update()
        {
            if (m_Spawned.Count < m_NumberOfSpawns
                && m_TimeOfNextSpawn <= Time.time)
            {
                Spawn(m_TotalSpawnedCount.ToString());
                m_TotalSpawnedCount++;
                m_TimeOfNextSpawn = Time.time + m_SpawnFrequency;
            }
        }

        protected virtual GameObject[] Spawn(string namePostfix)
        {
            Vector3? position = GetPosition();

            if (position != null)
            {
                //Optimization: Use a pool
                GameObject[] spawned = m_SpawnDefinition.InstantiatePrefabs((Vector3)position, $"{m_Name} {namePostfix}");
                for (int idx = 0; idx < spawned.Length; idx++)
                {
                    m_Spawned.Add(spawned[idx].transform);
                }
                return spawned;
            }

            return null;
        }

        /// <summary>
        /// Attempt to find a suitable spawn position for a spawned prefab.
        /// A limited number of attempts will be tried before aborting. This prevents
        /// endless loops. 
        /// </summary>
        /// <param name="attemptNumber">The current attempt number</param>
        /// <param name="maxTries">The maximum number of attempts to find a spawn location before aborting.</param>
        /// <returns></returns>
        private Vector3? GetPosition(int attemptNumber = 0, int maxTries = 10)
        {
            attemptNumber++;
            if (attemptNumber > maxTries)
            {
                Debug.LogWarning("Unable to find a suitable location on the navmesh for " + gameObject.name + ". Check you have baked the NavMesh and that you have at least one allowed area within the spawn radius.");
                return null;
            }

            Vector2 pos2D = Random.insideUnitCircle * m_Radius;
            Vector3 position = transform.position + new Vector3(pos2D.x, 0, pos2D.y);
            Vector3 finalPos = position;
            if (!onNavMesh && Terrain.activeTerrain != null) {
                finalPos.y = Terrain.activeTerrain.SampleHeight(finalPos) + Terrain.activeTerrain.transform.position.y;
            } else if (onNavMesh)
            {
                NavMeshHit hit;
                if (NavMesh.SamplePosition(finalPos, out hit, 2, navMeshAreaMask))
                {
                    finalPos = hit.position;
                } else
                {
                    return GetPosition(attemptNumber);
                }
            }

            return finalPos;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, m_Radius);
        }
    }
}
