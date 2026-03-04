using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GAG.EasyTangibleTable
{
    public class SpriteAnimator : MonoBehaviour
    {
        [Header("Animation Settings")] [SerializeField]
        SpriteRenderer _targetRenderer;

        [SerializeField] Image _targetImage;
        [SerializeField] List<Sprite> _startSpritesSet;
        [SerializeField] List<Sprite> _middleSpriteSet;
        [SerializeField] List<Sprite> _endSpriteSet;
        [SerializeField] float _frameRate = 10f;
        [SerializeField] bool _loop = true;

        [Header("Events")] public UnityEvent onAnimationFinished;

        int _currentFrame;
        Coroutine _playRoutine;

        void OnEnable()
        {
            Play();
        }

        void OnDisable()
        {
            Stop();
        }

        public void Play()
        {
            if (_playRoutine != null)
                StopCoroutine(_playRoutine);
            _playRoutine = StartCoroutine(PlayAnimation());
        }

        public void Stop()
        {
            if (_playRoutine != null)
            {
                StopCoroutine(_playRoutine);
                _playRoutine = null;
            }
        }

        public void SetLoop(bool shouldLoop)
        {
            _loop = shouldLoop;
        }

        IEnumerator PlayAnimation()
        {
            //_currentFrame = 0;

            //while (_currentFrame < _middleSpriteSet.Count)
            //{
            //    //_targetRenderer.sprite = sprites[_currentFrame];
            //    _targetImage.sprite = _middleSpriteSet[_currentFrame];
            //    yield return new WaitForSeconds(1f / _frameRate);
            //    _currentFrame++;

            //    if (_currentFrame >= _middleSpriteSet.Count)
            //    {
            //        if (_loop)
            //        {
            //            _currentFrame = 0;
            //        }
            //        else
            //        {
            //            onAnimationFinished?.Invoke();
            //            break;
            //        }
            //    }
            //}

            //_playRoutine = null;

            // --- 1️⃣ Play Start Animation ---
            for (int i = 0; i < _startSpritesSet.Count; i++)
            {
                SetSprite(_startSpritesSet[i]);
                yield return new WaitForSeconds(1f / _frameRate);
            }

            // --- 2️⃣ Loop Middle Animation ---
            do
            {
                for (int i = 0; i < _middleSpriteSet.Count; i++)
                {
                    SetSprite(_middleSpriteSet[i]);
                    yield return new WaitForSeconds(1f / _frameRate);
                }
            } while (_loop);

            // --- 3️⃣ Play End Animation ---
            for (int i = 0; i < _endSpriteSet.Count; i++)
            {
                SetSprite(_endSpriteSet[i]);
                yield return new WaitForSeconds(1f / _frameRate);
            }

            // --- 4️⃣ Done ---
            onAnimationFinished?.Invoke();
            _playRoutine = null;
        }

        void SetSprite(Sprite sprite)
        {
            if (_targetRenderer != null)
                _targetRenderer.sprite = sprite;
            if (_targetImage != null)
                _targetImage.sprite = sprite;
        }
    }
}