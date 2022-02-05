using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace WizardsCode.Ink
{
    public class TextBubbleController : MonoBehaviour
    {
        [SerializeField]
        [FormerlySerializedAs("The GUI component that will display the speakers name.")]
        TextMeshProUGUI m_SpeakersName;

        [SerializeField]
        [FormerlySerializedAs("The GUI component that will display the complete text of this chunk.")]
        TextMeshProUGUI m_StoryText;


        [SerializeField]
        [FormerlySerializedAs("Whether or not sounds should be played.")]
        bool m_PlaySpeakingSounds = true;

        [SerializeField]
        [FormerlySerializedAs("An array of sounds that will be played while narration or speech is occuring")]
        AudioClip[] m_SpeechSounds;

        [SerializeField]
        [FormerlySerializedAs("An array of sounds that will be played when punctuation is detected.")]
        AudioClip[] m_PunctuationSounds;

        [SerializeField]
        [FormerlySerializedAs("The audio source for the speech sounds.")]
        AudioSource m_AudioSource_Speech;

        [SerializeField]
        [FormerlySerializedAs("The audio source for the punctuation sounds.")]
        AudioSource m_AudioSourcePunctuation;

        [SerializeField]
        [FormerlySerializedAs("The delay between characters being printed.")]
        internal float m_SecondsBetweenPrintingChars = 0.01f;

        [SerializeField]
        [FormerlySerializedAs("_GrowShrinkSpeed")]
        float m_GrowOrShrinkSpeed = 4.0f;


        float _targetScale = 1.0f;
        float _closeEnough = 0.01f; // small enough to be invisible
        float _prettySmall = 0.1f; // small enough to be able to detect we're aiming for small

        IEnumerator ShowOrHide()
        {
            RectTransform rectTransform = GetComponent<RectTransform>();
            while (!ScaleIsCloseEnough(rectTransform))
            {
                InterpScale(rectTransform);
                yield return null;
            }

            if (_targetScale < _prettySmall)
            {
                rectTransform.transform.localScale = new Vector2(0.0f, 0.0f);
            }
        }

        bool ScaleIsCloseEnough(RectTransform rectTransform)
        {
            return Mathf.Abs(rectTransform.transform.localScale.x - _targetScale) < _closeEnough &&
                   Mathf.Abs(rectTransform.transform.localScale.y - _targetScale) < _closeEnough;
        }


        void InterpScale(RectTransform rectTransform)
        {
            float t = Time.deltaTime * m_GrowOrShrinkSpeed;
            float x = rectTransform.transform.localScale.x * (1 - t) + (_targetScale * t);
            float y = rectTransform.transform.localScale.y * (1 - t) + (_targetScale * t);
            rectTransform.transform.localScale = new Vector2(x, y);
        }

        /** reveal chars, once per pass */
        IEnumerator RevealChars()
        {
            while (m_StoryText.maxVisibleCharacters < m_StoryText.text.Length)
            {
                m_StoryText.maxVisibleCharacters++;
                if (m_PlaySpeakingSounds)
                {
                    ProduceSpeechSound(m_StoryText.text.ToCharArray()[m_StoryText.maxVisibleCharacters - 1]);
                }

                yield return new WaitForSeconds(m_SecondsBetweenPrintingChars);
            }
        }

        /** produce a very short sound based on what type of char is passed in. */
        void ProduceSpeechSound(char c)
        {
            if (char.IsPunctuation(c) && !m_AudioSourcePunctuation.isPlaying)
            {
                m_AudioSource_Speech.Stop();

                if (m_PunctuationSounds != null && m_PunctuationSounds.Length > 0)
                {
                    m_AudioSourcePunctuation.clip = m_PunctuationSounds[Random.Range(0, m_PunctuationSounds.Length)];
                    m_AudioSourcePunctuation.Play();
                }
            }
            else if (char.IsLetter(c) && !m_AudioSource_Speech.isPlaying)
            {
                m_AudioSourcePunctuation.Stop();
                if (m_SpeechSounds != null && m_SpeechSounds.Length > 0)
                {
                    m_AudioSource_Speech.clip = m_SpeechSounds[Random.Range(0, m_SpeechSounds.Length)];
                    m_AudioSource_Speech.Play();
                }
            }
        }



        // ****************************************************************************** public methods
        /** convenience method, detects if _trget_ scale is small */
        public bool IsHidden()
        {
            return _targetScale < _prettySmall;
        }

        /** note: this invokes show widget automatically */
        public void SetText(string speakersName, string text, bool bPlaySpeakingSounds)
        {
            ShowWidget(true);

            m_SpeakersName.gameObject.SetActive(!string.IsNullOrEmpty(speakersName));

            m_SpeakersName.text = speakersName;
            m_StoryText.text = text;

            // this is what lets us show characters one at a time: we increment this later
            m_StoryText.maxVisibleCharacters = 0;

            m_PlaySpeakingSounds = bPlaySpeakingSounds;

            StartCoroutine("RevealChars");
        }

        /** note: this invokes show widget automatically */
        public void SetText(string speakersName, string text, bool bPlaySpeakingSounds, float secondsBetweenPrintingChars)
        {
            ShowWidget(true);
            m_SecondsBetweenPrintingChars = secondsBetweenPrintingChars;
            SetText(speakersName, text, bPlaySpeakingSounds);
        }

        /** convenience method used in test scene in lieu of having a test suite */
        public void TestDisplayingText()
        {
            SetText("buddy", "Yeah, but, that doesn't mean he always gets it right away. Sometimes it takes a couple of shots to capture an entire moment in a single shot.", true);
        }

        public void ClearText()
        {
            m_SpeakersName.text = "";

            m_StoryText.text = "";
        }

        /** if false and currently not hidden, it will call ClearText() automatically */
        public void ShowWidget(bool Value)
        {
            if (Value)
            {
                if (!IsHidden()) return;
                _targetScale = 1.0f;
            }
            else
            {
                if (IsHidden()) return;
                ClearText();
                _targetScale = 0.0f;
            }

            StartCoroutine("ShowOrHide");
        }
    }
}
