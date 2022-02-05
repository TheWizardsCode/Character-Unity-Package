using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using WizardsCode.Character;
using static WizardsCode.Character.ActorCueAnimator;
using System;
using UnityEngine.Serialization;

namespace WizardsCode.Character
{
    /// <summary>
    /// An actor cue that extends the basic ActorCue with Animator control features.
    /// </summary>
    [CreateAssetMenu(fileName = "Animation ActorCue", menuName = "Wizards Code/Actor/Animation Cue")]
    public class ActorCueAnimator : ActorCue
    {
        [Header("Animation Layers")]
        [SerializeField, Tooltip("The name of the layer to control the weight of. An emptry field means the layer weight has no effect.")]
        string m_LayerName = "";
        [SerializeField, Range(0f, 1), Tooltip("The weight of the layer")]
        float m_LayerWeight = 1;
        [SerializeField, Range(0f, 20), Tooltip("The time in seconds that it will take to reach the new layer weight.")]
        float m_LayerWeightChangeTime = 0.5f;

        public enum ParameterType { Float, Int, Bool, Trigger }
        [Header("Animation Parameters")]
        [SerializeField, Tooltip("A set of animation parameter changes to make.")]
        AnimationParameter[] m_AnimationParams;

        [Header("Animation Clips")]
        [SerializeField, Tooltip("Tha name of the animation clip to play.")]
        string animationClipName;
        [SerializeField, Tooltip("The normalized time from which to start the animation.")]
        float animationNormalizedTime = 0;

        private int m_LayerIndex;

        public float layerWeightChangeTime
        {
            get { return m_LayerWeightChangeTime; }
        }

        private void ProcessAnimationLayerWeights()
        {
            if (m_Actor.Animator != null)
            {
                m_LayerIndex = m_Actor.Animator.GetLayerIndex(m_LayerName);
            }
        }

        public override IEnumerator Prompt(BaseActorController actor)
        {
            m_Actor = actor;

            ProcessAnimationLayerWeights();
            ProcessAnimationParameters();

            return base.Prompt(actor);
        }

        protected override IEnumerator UpdateCoroutine()
        {
            // Update Layers
            if (m_Actor.Animator != null && m_LayerIndex >= 0 && m_Actor.Animator.GetLayerWeight(m_LayerIndex) != m_LayerWeight)
            {
                float originalWeight = m_Actor.Animator.GetLayerWeight(m_LayerIndex);
                float time = 0;
                while (!Mathf.Approximately(m_Actor.Animator.GetLayerWeight(m_LayerIndex), m_LayerWeight))
                {
                    time += Time.deltaTime;
                    m_Actor.Animator.SetLayerWeight(m_LayerIndex,
                        Mathf.Lerp(originalWeight, m_LayerWeight, time / m_LayerWeightChangeTime));
                    yield return new WaitForEndOfFrame();
                }
            }

            if (!string.IsNullOrWhiteSpace(animationClipName))
            {
                m_Actor.Animator.Play(animationClipName, m_LayerIndex, animationNormalizedTime);
            }

            yield return base.UpdateCoroutine();
        }
        /// <summary>
        /// If this cue has any animation parameter changes then have an actor make those changes.
        /// </summary>
        /// <param name="m_Actor">The actor to enact the animation changes.</param>
        private void ProcessAnimationParameters()
        {
            for (int i = 0; i < m_AnimationParams.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(m_AnimationParams[i].paramName))
                {
                    switch (m_AnimationParams[i].paramType)
                    {
                        case ParameterType.Trigger:
                            m_Actor.Animator.SetTrigger(m_AnimationParams[i].paramName);
                            break;
                        case ParameterType.Bool:
                            m_Actor.Animator.SetBool(m_AnimationParams[i].paramName, m_AnimationParams[i].paramBoolValue);
                            break;
                        case ParameterType.Int:
                            m_Actor.Animator.SetInteger(m_AnimationParams[i].paramName, m_AnimationParams[i].paramIntValue);
                            break;
                        case ParameterType.Float:
                            m_Actor.Animator.SetBool(m_AnimationParams[i].paramName, m_AnimationParams[i].paramBoolValue);
                            break;
                    }
                }
            }
        }
    }

    [Serializable]
    struct AnimationParameter
    {
        [SerializeField, Tooltip("The name of the animation parameter to set the value to.")]
        public string paramName;
        [SerializeField, Tooltip("The type of parameter to set.")]
        public ParameterType paramType;
        [SerializeField, Tooltip("The float value of the parameter, value is ignored if parameter is not a float")]
        public float paramFloatValue;
        [SerializeField, Tooltip("The int value of the parameter, value is ignored if parameter is not a int")]
        public int paramIntValue;
        [SerializeField, Tooltip("The bool value of the parameter, value is ignored if parameter is not a bool")]
        public bool paramBoolValue;
    }
}
