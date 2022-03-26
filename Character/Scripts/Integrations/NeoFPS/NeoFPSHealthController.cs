#if NEOFPS
using NeoFPS;
using NeoSaveGames;
using NeoSaveGames.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WizardsCode.Character.Stats;

namespace WizardsCode.Character.Intergration 
{
    public class NeoFPSHealthController : HealthController, IHealthManager, INeoSerializableComponent
    {
        protected override void OnHealthChanged(float normalizedDelta)
        {
            base.OnHealthChanged(normalizedDelta);
        }

        /// <summary>
        /// NeoFPS paramater, pass through to WizardsCode Health Controller
        /// </summary>
        public bool isAlive => IsAlive;

        /// <summary>
        /// NeoFPS paramater, pass through to WizardsCode Health Controller
        /// </summary>
        public float health
        {
            get { return Health; }
            set { Health = value; }
        }

        /// <summary>
        /// NeoFPS paramater, pass through to WizardsCode Health Controller
        /// </summary>
        public float healthMax
        {
            get { return m_Health.MaxValue; }
            set { m_Health.MaxValue = value; }
        }

        /// <summary>
        /// NeoFPS paramater, pass through to WizardsCode Health Controller
        /// </summary>
        public float normalisedHealth
        {
            get { return m_Health.NormalizedValue; }
            set { m_Health.NormalizedValue = value; }
        }

        /// <summary>
        /// NeoFPS events, pass through to WizardsCode Health Controller
        /// </summary>
        public event HealthDelegates.OnIsAliveChanged onIsAliveChanged;
        /// <summary>
        /// NeoFPS events, pass through to WizardsCode Health Controller
        /// </summary>
        public event HealthDelegates.OnHealthChanged onHealthChanged;
        /// <summary>
        /// NeoFPS events, pass through to WizardsCode Health Controller
        /// </summary>
        public event HealthDelegates.OnHealthMaxChanged onHealthMaxChanged;

        /// <summary>
        /// NeoFPS method, pass through to appropriate WizardsCode method
        /// </summary>
        /// <param name="damage"></param>
        public void AddDamage(float damage)
        {
            TakeDamage(damage);
        }


        /// <summary>
        /// NeoFPS method, pass through to appropriate WizardsCode method.
        /// TODO: Handle criticals
        /// </summary>
        /// <param name="damage"></param>
        public void AddDamage(float damage, bool critical)
        {
            AddDamage(damage);
        }

        /// <summary>
        /// NeoFPS method, pass through to appropriate WizardsCode method.
        /// TODO: Handle criticals, damage source
        /// </summary>
        /// <param name="damage"></param>
        public void AddDamage(float damage, IDamageSource source)
        {
            AddDamage(damage);
        }

        /// <summary>
        /// NeoFPS method, pass through to appropriate WizardsCode method.
        /// TODO: Handle criticals, and raycastHit
        /// </summary>
        /// <param name="damage"></param>
        public void AddDamage(float damage, bool critical, RaycastHit hit)
        {
            AddDamage(damage);
        }

        /// <summary>
        /// NeoFPS method, pass through to appropriate WizardsCode method.
        /// TODO: Handle criticals, damage source
        /// </summary>
        /// <param name="damage"></param>
        public void AddDamage(float damage, bool critical, IDamageSource source)
        {
            AddDamage(damage);
        }

        /// <summary>
        /// NeoFPS method, pass through to appropriate WizardsCode method.
        /// TODO: Handle criticals, damage source and raycastHit
        /// </summary>
        /// <param name="damage"></param>
        public void AddDamage(float damage, bool critical, IDamageSource source, RaycastHit hit)
        {
            AddDamage(damage);
        }

        /// <summary>
        /// NeoFPS method, pass through to appropriate WizardsCode method
        /// </summary>
        /// <param name="damage"></param>
        public void AddHealth(float h)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// NeoFPS method, pass through to appropriate WizardsCode method
        /// </summary>
        /// <param name="damage"></param>
        public void AddHealth(float h, IDamageSource source)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// NeoFPS method, pass through to appropriate WizardsCode method
        /// </summary>
        /// <param name="damage"></param>
        public void ReadProperties(INeoDeserializer reader, NeoSerializedGameObject nsgo)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// NeoFPS method, pass through to appropriate WizardsCode method
        /// </summary>
        /// <param name="damage"></param>
        public void WriteProperties(INeoSerializer writer, NeoSerializedGameObject nsgo, SaveMode saveMode)
        {
            throw new System.NotImplementedException();
        }
    }
}
#endif