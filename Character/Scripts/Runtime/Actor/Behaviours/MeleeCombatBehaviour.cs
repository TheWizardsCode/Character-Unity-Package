using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using WizardsCode.Character.AI;
using WizardsCode.Stats;

namespace WizardsCode.Character.AI
{
    public class MeleeCombatBehaviour : GenericActorInteractionBehaviour
    {
        [Header("Melee Combat")]
        [SerializeField, Tooltip("The time after the start if the `OnPerformCue` is first until the character attempts to inflict damage on the enemy.")]
        public float m_TimeUntilDamage = 1;
        [SerializeField, Tooltip("The set of character stats and the influence to apply to enemies this character manages to hit them.")]
        internal StatInfluencerSO[] m_EnemyHitInfluences;

        /// <summary>
        /// Start a coroutine that will perform the damage check at the appropriate time.
        /// </summary>
        protected override void OnPrepareCue()
        {
            StartCoroutine(DealDamageCo());
        }

        private IEnumerator DealDamageCo()
        {
            yield return new WaitForSeconds(m_TimeUntilDamage);

            int attempt = 0;
            StatsTracker target = participants[Random.Range(0, participants.Count)];
            while (target == Brain && attempt < 4)
            {
                attempt++;
                target = participants[Random.Range(0, participants.Count)];
            }

            for (int i = 0; i < m_EnemyHitInfluences.Length; i++)
            {
                target.TryAddInfluencer(m_EnemyHitInfluences[i]);
                Debug.Log($"{DisplayName} hit {target.DisplayName} for {m_EnemyHitInfluences[i].maxChange} {m_EnemyHitInfluences[i].stat.DisplayName}");
                yield return null;
            }
        }
    }
}
