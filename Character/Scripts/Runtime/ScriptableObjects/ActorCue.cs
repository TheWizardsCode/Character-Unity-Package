using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace WizardsCode.Character
{
    /// <summary>
    /// A basic actor cue contains details of what an actor should do at upon a signal from the director.
    /// </summary>
    [CreateAssetMenu(fileName = "ActorCue", menuName = "Wizards Code/Actor/Cue")]
    public class ActorCue : ScriptableObject
    {
        [TextArea, SerializeField, Tooltip("A description of this actor cue.")]
        string m_Description;

        [SerializeField, Tooltip("Duration of this phase of this cue action. If 0 then it is unlimited.")]
        public float Duration = 5;

        [Header("Movement")]
        [SerializeField, Tooltip("The name of the mark the actor should move to on this cue.")]
        string markName;
        [SerializeField, Tooltip("Stop movement upon recieving this cue. Note that this will override the markName setting above, that is if this is set and markName is set then no movement will occur.")]
        bool m_StopMovement = false;

        [Header("Sound")]
        [SerializeField, Tooltip("Audio files for spoken lines")]
        public AudioClip audioClip;

        protected BaseActorController m_Actor;
        internal NavMeshAgent m_Agent;
        internal bool m_AgentEnabled;

        /// <summary>
        /// Get or set the mark name, that is the name of an object in the scene the character should move to when this cue is prompted.
        /// Note that changing the Mark during execution of this cue will
        /// have no effect until it is prompted the next time.
        /// </summary>
        public string Mark
        {
            get { return markName; }
            set { markName = value; }
        }

        /// <summary>
        /// Prompt and actor to enact the actions identified in this cue.
        /// </summary>
        /// <returns>An optional coroutine that shouold be started by the calling MonoBehaviour</returns>
        public virtual IEnumerator Prompt(BaseActorController actor)
        {
            m_Actor = actor;

            ProcessMove();
            ProcessAudio();

            return UpdateCoroutine();
        }

        protected virtual IEnumerator UpdateCoroutine()
        {
            if (m_Agent != null)
            {
                while (m_Agent.pathPending || (m_Agent.hasPath && m_Agent.remainingDistance > m_Agent.stoppingDistance))
                {
                    yield return new WaitForSeconds(0.3f);
                }
                m_Agent.enabled = m_AgentEnabled;
            }
        }

        /// <summary>
        /// If this cue has a mark defined move to it.
        /// </summary>
        protected virtual void ProcessMove()
        {
            if (m_StopMovement)
            {
                m_Actor.StopMoving();
                return;
            }

            if (!string.IsNullOrWhiteSpace(markName))
            {
                m_Agent = m_Actor.GetComponent<NavMeshAgent>();
                m_AgentEnabled = m_Agent.enabled;
                m_Agent.enabled = true;

                GameObject go = GameObject.Find(markName);
                if (go != null)
                {
                    m_Agent.SetDestination(go.transform.position);
                } else
                {
                    Debug.LogWarning(m_Actor.name + "  has a mark set, but the mark doesn't exist in the scene. The name set is " + markName);
                }
            }
        }

        protected virtual void ProcessAudio()
        {
            if (audioClip != null)
            {
                AudioSource source = m_Actor.GetComponent<AudioSource>();
                source.clip = audioClip;
                source.Play();
            }
        }
    }
}
