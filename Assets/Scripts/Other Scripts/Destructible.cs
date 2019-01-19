using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace H1ddenGames
{
    namespace DestructibleEnvironment
    {
        public class Destructible : MonoBehaviour
        {
            [Header("Force's Power"), Tooltip("How much force should be given to the object's pieces?"), SerializeField] private float _minExplosionForce = 1000f;
            [SerializeField] private float _maxExplosionForce = 1800f;

            [Tooltip("How big is the explosion's radius?"), SerializeField] private float _minExplosionRadius = 3f;
            [SerializeField] private float _maxExplosionRadius = 4.5f;

            [Header("Force's Influence"), Tooltip("How far should the object's pieces be until they stop having a force used on them?"), SerializeField] private float _effectDistance = 4f;

            [Header("Force's Location"), Tooltip("How far from the base of the parent object should the force be applied?"), SerializeField] private float _minExplosionRange = 0.5f;
            [SerializeField] private float _maxExplosionRange = 1f;

            [Space(10), SerializeField] private float _cleanUpTime = 2f;

            [Space(10), SerializeField] private Material _replacementMaterial;

            [SerializeField] private BoxCollider _boxCollider;
            [SerializeField, Space(10)] private List<Rigidbody> _rigidbodies = new List<Rigidbody>();
            [SerializeField] private List<MeshRenderer> _renderers = new List<MeshRenderer>();
            [SerializeField] private List<float> _distances = new List<float>();

            private Vector3 _pos;
            private RaycastHit[] _hits;
            public List<GameObject> _gameObjectsToCleanUp = new List<GameObject>();

            #region Getters and Setters
            public List<Rigidbody> Rigidbodies { get => _rigidbodies; set => _rigidbodies = value; }
            public List<float> Distances { get => _distances; set => _distances = value; }
            public List<MeshRenderer> Renderers { get => _renderers; set => _renderers = value; }
            public BoxCollider BoxCollider { get => _boxCollider; set => _boxCollider = value; }
            public Material ReplacementMaterial { get => _replacementMaterial; set => _replacementMaterial = value; }

            public float MinExplosionForce { get => _minExplosionForce; set => _minExplosionForce = value; }
            public float MaxExplosionForce { get => _maxExplosionForce; set => _maxExplosionForce = value; }
            public float MinExplosionRadius { get => _minExplosionRadius; set => _minExplosionRadius = value; }
            public float MaxExplosionRadius { get => _maxExplosionRadius; set => _maxExplosionRadius = value; }
            public float EffectDistance { get => _effectDistance; set => _effectDistance = value; }
            public float MinExplosionRange { get => _minExplosionRange; set => _minExplosionRange = value; }
            public float MaxExplosionRange { get => _maxExplosionRange; set => _maxExplosionRange = value; }
            public float CleanUpTime { get => _cleanUpTime; set => _cleanUpTime = value; }
            #endregion

            private void Awake()
            {
                if(BoxCollider == null)
                {
                    BoxCollider = gameObject.GetComponent<BoxCollider>();
                }

                if(Rigidbodies == null)
                {
                    FindRigidbodies();
                }
                
                if(Renderers == null)
                {
                    FindMeshRenderers();
                }

                if(Distances == null)
                {
                    UpdateDistances();
                }

                ToggleRigidbodies(true);
            }

            public void LateUpdate()
            {
                UpdateClickableArea();
            }

            public void FindRigidbodies()
            {
                Rigidbodies = gameObject.GetComponentsInChildren<Rigidbody>().ToList();
            }

            public void FindMeshRenderers()
            {
                Renderers = gameObject.GetComponentsInChildren<MeshRenderer>().ToList();
            }

            public void ReplaceMaterials()
            {
                UpdateDistances();

                for (int i = 0; i < Renderers.Count; i++)
                {
                    if(Distances[i] > EffectDistance)
                    {
                        if(Renderers[i].material.Equals(ReplacementMaterial))
                        {
                            return;
                        } 

                        Renderers[i].material = ReplacementMaterial;

                        if(_gameObjectsToCleanUp.Contains(Renderers[i].gameObject))
                        {
                            return;
                        }

                        _gameObjectsToCleanUp.Add(Renderers[i].gameObject);
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

            /// <summary>
            /// Updates the list of distances when the game starts and after every click. 
            /// </summary>
            [ContextMenu("Update Distances")]
            public void UpdateDistances()
            {
                Distances.Clear();

                foreach (var item in Rigidbodies) {
                    Distances.Add(Vector3.Distance(item.transform.position, transform.position));
                }
            }

            private void OnMouseOver()
            {
                if (Input.GetMouseButtonDown(0)) {
                    // Make rigidbodies only react to physics.
                    ToggleRigidbodies(false);

                    // Get current position of each fractured part.
                    UpdateDistances();

                    int i = 0;
                    foreach (var item in Rigidbodies) {
                        // If the distance from the clickable object is greater than the effect's distance, there's nothing to do. 
                        if (Distances[i] > EffectDistance) {
                            i++;
                        }
                        else {
                            i++;
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

                    Coroutine co = StartCoroutine("CleanUp");
                }
            }

            public IEnumerator CleanUp()
            {
                yield return new WaitForSecondsRealtime(CleanUpTime);
                int index = Random.Range(0, _gameObjectsToCleanUp.Count - 1);

                _gameObjectsToCleanUp[index].SetActive(false);
                foreach (var item in Rigidbodies)
                {
                    if(_gameObjectsToCleanUp[index].GetComponent<Rigidbody>().Equals(item))
                    {
                        Rigidbodies.Remove(item);
                        break;
                    } 
                }

                foreach (var item in Renderers)
                {
                    if (_gameObjectsToCleanUp[index].GetComponent<Renderer>().Equals(item))
                    {
                        Renderers.Remove(item);
                        break;
                    }
                }

                Destroy(_gameObjectsToCleanUp[index]);

                _gameObjectsToCleanUp.RemoveAt(index);
                _gameObjectsToCleanUp.TrimExcess();

                if(_gameObjectsToCleanUp.Count > 0)
                {
                    StartCoroutine("CleanUp");
                }
            }


            /// <summary>
            /// Updates the box collider's shape based on a raycast to see if there's any material to manipulate.
            /// </summary>
            public void UpdateClickableArea()
            {
                UpdateArea("top");
                UpdateArea("center");
                UpdateArea("bottom");
            }

            public void UpdateArea(string location)
            {
                // Change the material of the object pieces that are outside of the effect range.
                ReplaceMaterials();

                // Once the collider gets below a certain size, there is no need to have it active anymore.
                if (BoxCollider.size.x < 0.5f || BoxCollider.size.y < 0.5f || BoxCollider.size.z < 0.5f) {
                    BoxCollider.enabled = false;
                    return;
                }

                // The "~" negates the statement after it. So instead of ignoring "Destructible" layer objects, it will ignore everything BUT the "Destructible" layer.
                LayerMask _layerMask = ~LayerMask.NameToLayer("Destructible");
                Vector3 size = new Vector3(BoxCollider.size.x, BoxCollider.size.y / 3, BoxCollider.size.z);
                List<Collider> hitColliders = new List<Collider>();
                hitColliders.Clear();

                if (location.ToLower().Equals("top"))
                {
                    // Top
                    hitColliders = Physics.OverlapBox(BoxCollider.center * (BoxCollider.size.y / 2.5f), size, Quaternion.identity, _layerMask).ToList();
                }
                else if(location.ToLower().Equals("center"))
                {
                    // Center
                    hitColliders = Physics.OverlapBox(BoxCollider.center, size, Quaternion.identity, _layerMask).ToList();
                } else if(location.ToLower().Equals("bottom"))
                {
                    // Bottom
                    hitColliders = Physics.OverlapBox(BoxCollider.center / BoxCollider.size.y, size, Quaternion.identity, _layerMask).ToList();
                }

                // Decrease the size of the collider if there is nothing found within the collider.
                if(hitColliders.Count <= Rigidbodies.Count / 2)
                {
                    BoxCollider.size = new Vector3(BoxCollider.size.x, BoxCollider.size.y / 3, BoxCollider.size.z);
                    BoxCollider.center = new Vector3(BoxCollider.center.x, BoxCollider.size.y / 3, BoxCollider.center.z);
                }
            }

            void OnDrawGizmos()
            {
                Gizmos.color = Color.red;
                Vector3 size = new Vector3(BoxCollider.size.x, BoxCollider.size.y / 3, BoxCollider.size.z);
                Gizmos.DrawWireCube(BoxCollider.center * (BoxCollider.size.y / 2.5f), size); // Top
                Gizmos.DrawWireCube(BoxCollider.center, size); // Center
                Gizmos.DrawWireCube(BoxCollider.center / BoxCollider.size.y, size); // Bottom
            }
        }
    }
}