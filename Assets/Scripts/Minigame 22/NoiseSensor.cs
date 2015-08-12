using UnityEngine;
using System.Collections;

namespace Minigame22
{
    public class NoiseSensor : MonoBehaviour
    {
        #region variables

        public NoiseLevel noiseLevel
        {
            get { return _noiseLevel; }
            set { _noiseLevel = value; UpdateSprite(); }
        }

        public UISprite spriteNoise;
        public float xLocalMin = 0;
        public float xLocalMax = 0;
        public float yLocalMin = 0;
        public float yLocalMax = 0;

        private NoiseLevel _noiseLevel;
        private bool _isInit = false;
        private Collider _lastNoiseColider;

        #endregion

        
        void Start()
        {
            Init();
        }

        void Update()
        {
            if (MiniGame22_Manager.instance.isPlay && UICamera.hoveredObject != null)
            {
                transform.position = new Vector3(UICamera.lastHit.point.x, UICamera.lastHit.point.y, transform.position.z);
                transform.localPosition = new Vector3(Mathf.Clamp(transform.localPosition.x, xLocalMin, xLocalMax), Mathf.Clamp(transform.localPosition.y, yLocalMin, yLocalMax), transform.localPosition.z);
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (!MiniGame22_Manager.instance.isPlay)
                return;

            NoiseSource ns = other.GetComponent<NoiseSource>();
            if (ns != null)
            {
                _lastNoiseColider = other;
                noiseLevel = ns.noiseLevel;
                MiniGame22_Manager.instance.WasFindNoise(ns);
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (!MiniGame22_Manager.instance.isPlay || _lastNoiseColider == null)
                return;

            if (_lastNoiseColider == other)
            {
                noiseLevel = NoiseLevel._0;
                _lastNoiseColider = null;
            }
        }

        public void Reset()
        {
            Init();
            _lastNoiseColider = null;
            _noiseLevel = NoiseLevel._0;
            UpdateSprite();
        }

        public void UpdateSprite()
        {
            Init();
            if (spriteNoise != null)
                switch(_noiseLevel)
                {
                    case NoiseLevel._0:
                        spriteNoise.spriteName = "";
                        break;

                    case NoiseLevel._20:
                        spriteNoise.spriteName = "20";
                        break;

                    case NoiseLevel._40:
                        spriteNoise.spriteName = "40";
                        break;

                    case NoiseLevel._50:
                        spriteNoise.spriteName = "50";
                        break;

                    case NoiseLevel._80:
                        spriteNoise.spriteName = "80";
                        break;

                    case NoiseLevel._90:
                        spriteNoise.spriteName = "90";
                        break;

                    case NoiseLevel._100:
                        spriteNoise.spriteName = "100";
                        break;

                    case NoiseLevel._110:
                        spriteNoise.spriteName = "110";
                        break;

                    case NoiseLevel._120:
                        spriteNoise.spriteName = "120";
                        break;

                    case NoiseLevel._140:
                        spriteNoise.spriteName = "140";
                        break;

                    default:
                        spriteNoise.spriteName = "20";
                        break;
                }
        }

        private void Init()
        {
            if (_isInit)
                return;

            _isInit = true;
        }
    }

    public enum NoiseLevel { _0, _20, _40, _50, _80, _90, _100, _110, _120, _140}
}