using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Animations;
using System.Linq;

namespace H1ddenGames
{
    namespace DestructibleEnvironment
    {
        public class Destructible : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
        {
            [Header("Force's Power"), Tooltip("How much force should be given to the object's pieces?"), SerializeField] private float _minExplosionForce = 1000f;
            [SerializeField] private float _maxExplosionForce = 1800f;
            [SerializeField] private int _hitsToBreak = 1;
            [SerializeField] private bool _isExplosive = false;

            [Tooltip("How big is the explosion's radius?"), SerializeField] private float _minExplosionRadius = 3f;
            [SerializeField] private float _maxExplosionRadius = 4.5f;

            [Header("Force's Influence"), Tooltip("How far should the object's pieces be until they stop having a force used on them?"), SerializeField] private float _effectDistance = 4f;

            [Header("Force's Location"), Tooltip("How far from the base of the parent object should the force be applied?"), SerializeField] private float _minExplosionRange = 0.5f;
            [SerializeField] private float _maxExplosionRange = 1f;

            [Space(10), SerializeField] private float _cleanUpTime = 2f;

            [Space(10), SerializeField] private Material _replacementMaterial;

            private BoxCollider _boxCollider;
            private List<Rigidbody> _rigidbodies = new List<Rigidbody>();
            private List<MeshRenderer> _renderers = new List<MeshRenderer>();
            private List<float> _distances = new List<float>();

            private Vector3 _pos;
            private RaycastHit[] _hits;
            private List<GameObject> _gameObjectsToCleanUp = new List<GameObject>();
            private Coroutine co;

            public Animator _animator; 

            #region Getters and Setters
            public List<Rigidbody> Rigidbodies { get => _rigidbodies; set => _rigidbodies = value; }
            public List<float> Distances { get => _distances; set => _distances = value; }
            public List<MeshRenderer> Renderers { get => _renderers; set => _renderers = value; }
            public BoxCollider BoxCollider { get => _boxCollider; set => _boxCollider = value; }
            public Material ReplacementMaterial { get => _replacementMaterial; set => _replacementMaterial = value; }

            public int HitsToBreak { get => _hitsToBreak; set => _hitsToBreak = value; }
            public float MinExplosionForce { get => _minExplosionForce; set => _minExplosionForce = value; }
            public float MaxExplosionForce { get => _maxExplosionForce; set => _maxExplosionForce = value; }
            public float MinExplosionRadius { get => _minExplosionRadius; set => _minExplosionRadius = value; }
            public float MaxExplosionRadius { get => _maxExplosionRadius; set => _maxExplosionRadius = value; }
            public float EffectDistance { get => _effectDistance; set => _effectDistance = value; }
            public float MinExplosionRange { get => _minExplosionRange; set => _minExplosionRange = value; }
            public float MaxExplosionRange { get => _maxExplosionRange; set => _maxExplosionRange = value; }
            public float CleanUpTime { get => _cleanUpTime; set => _cleanUpTime = value; }
            public bool IsExplosive { get => _isExplosive; set => _isExplosive = value; }
            #endregion

            private void Awake()
            {
                if(BoxCollider == null) { BoxCollider = gameObject.GetComponent<BoxCollider>(); }
                if(Rigidbodies == null || Rigidbodies.Count == 0) { Rigidbodies = gameObject.GetComponentsInChildren<Rigidbody>().ToList(); }
                if(Renderers == null || Renderers.Count == 0) { Renderers = gameObject.GetComponentsInChildren<MeshRenderer>().ToList(); }
                if(Distances == null || Distances.Count == 0) { UpdateDistances(); }

                ToggleRigidbodies(true);
                _animator = transform.GetComponentInParent<Animator>();
            }

            public void LateUpdate()
            {
                ReplaceMaterials();
            }

            /// <summary>
            /// Updates the list of distances when the game starts and after every click. 
            /// </summary>
            [ContextMenu("Update Distances")]
            public void UpdateDistances()
            {
                Distances.Clear();

                foreach (var item in Rigidbodies)
                {
                    Distances.Add(Vector3.Distance(item.transform.localPosition, BoxCollider.center));
                }
            }

            public void ReplaceMaterials()
            {
                UpdateDistances();

                for (int i = 0; i < Renderers.Count; i++) {
                    if(Distances[i] > EffectDistance) {
                        if(Renderers[i].material != ReplacementMaterial) { Renderers[i].material = ReplacementMaterial; }

                        if(!_gameObjectsToCleanUp.Contains(Renderers[i].gameObject)) { _gameObjectsToCleanUp.Add(Renderers[i].gameObject); }
                    } 
                }
            }

            /// <summary>
            /// Used to toggle the isKinematic boolean on each fractured object.
            /// </summary>
            /// <param name="b">true means isKinematic is checked, false is unchecked</param>
            public void ToggleRigidbodies(bool b)
            {
                foreach (var item in Rigidbodies) {
                    item.isKinematic = b;
                }
            }

            public void OnPointerDown(PointerEventData eventData) {

                if(eventData.clickCount == 0)
                {
                    HitsToBreak -= 1;
                } else
                {
                    HitsToBreak -= eventData.clickCount;
                }

                // Get current position of each fractured part.
                UpdateDistances();

                if (HitsToBreak > 0)
                {
                    _animator.SetTrigger("isHit");
                    return;
                }
                else if (HitsToBreak == 0)
                {
                    // Make rigidbodies only react to physics.
                    ToggleRigidbodies(false);

                    if (IsExplosive == true)
                    {
                        foreach (var item in Rigidbodies)
                        {
                            // Otherwise, get the rigidbody and add an explosion force.
                            Vector3 location = new Vector3(
                                transform.position.x + Random.Range(MinExplosionRange, MaxExplosionRange),
                                transform.position.y + Random.Range(MinExplosionRange, MaxExplosionRange),
                                transform.position.z + Random.Range(MinExplosionRange, MaxExplosionRange)
                            );

                            item.AddExplosionForce(
                                Random.Range(MinExplosionForce, MaxExplosionForce),
                                location,
                                Random.Range(MinExplosionRadius, MaxExplosionRadius)
                            );
                        }
                    }
                    else
                    {
                        // Otherwise, get the rigidbody and add an explosion force.
                        Vector3 location = new Vector3(
                            transform.position.x + Random.Range(MinExplosionRange, MaxExplosionRange),
                            transform.position.y + Random.Range(MinExplosionRange, MaxExplosionRange),
                            transform.position.z + Random.Range(MinExplosionRange, MaxExplosionRange)
                        );

                        Rigidbodies[Random.Range(0, Rigidbodies.Count - 1)].AddExplosionForce(
                            Random.Range(MinExplosionForce, MaxExplosionForce),
                            location,
                            Random.Range(MinExplosionRadius, MaxExplosionRadius)
                        );
                    }

                    _animator.SetTrigger("isBroken");
                    _animator.StopPlayback();
                    UpdateClickableArea();
                }

                co = StartCoroutine("CleanUp");
            }

            public void OnPointerUp(PointerEventData eventData)
            {
                
            }

            public IEnumerator CleanUp()
            {
                yield return new WaitForSecondsRealtime(CleanUpTime);
                CleanUpEnv();
            }

            public void CleanUpEnv()
            {
                _gameObjectsToCleanUp = _gameObjectsToCleanUp.FindAll(x => x != null);
                if (_gameObjectsToCleanUp.Count == 0 || _gameObjectsToCleanUp[0] == null) { return; }
                _gameObjectsToCleanUp.TrimExcess();

                _gameObjectsToCleanUp.First(x => x != null).SetActive(false);

                foreach (var item in Rigidbodies) {
                    if (_gameObjectsToCleanUp.First().GetComponent<Rigidbody>().Equals(item)) {
                        Rigidbodies.Remove(item);
                        break;
                    }
                }

                foreach (var item in Renderers) {
                    if (_gameObjectsToCleanUp.First().GetComponent<Renderer>().Equals(item)) {
                        Renderers.Remove(item);
                        break;
                    }
                }

                Destroy(_gameObjectsToCleanUp.First());

                if (_gameObjectsToCleanUp.Count > 0) {
                    Coroutine co = StartCoroutine("CleanUp");
                }
            }

            public void UpdateClickableArea()
            {
                if(HitsToBreak <= 0) {
                    BoxCollider.enabled = false;
                } else { return; }
            }
        }
    }
}