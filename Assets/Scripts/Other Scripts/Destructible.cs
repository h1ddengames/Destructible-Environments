using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    [Header("Forces"), Tooltip("How much force should be given to the object's pieces.")] public float explosionForce;
    [Tooltip("How big is the explosion's radius")] public float explosionRadius;

    [Tooltip("How far should the object's pieces be until they stop having a force used on them.")]public float effectDistance; 

    [SerializeField] private List<Rigidbody> rigidbodies = new List<Rigidbody>();
    [SerializeField] private List<float> distances = new List<float>();

    public List<Rigidbody> Rigidbodies { get => rigidbodies; set => rigidbodies = value; }
    public List<float> Distances { get => distances; set => distances = value; }

    private Vector3 pos;
    private BoxCollider boxCollider;

    private void Start()
    {
        boxCollider = gameObject.GetComponent<BoxCollider>();

        // Make rigidbodies only react to script.
        ToggleRigidbodies(true);
        UpdateDistances();
    }

    public void Update()
    {
        UpdateClickableArea();
    }

    /// <summary>
    /// Updates the box collider's shape based on a raycast to see if there's any material to manipulate.
    /// </summary>
    public void UpdateClickableArea()
    {
        // Create a raycast from each cardinal direction pointing towards the center of the object.
        // Bottom
        //Vector3 bottom = transform.TransformDirection(Vector3.up) * 0.2f;
        //Debug.DrawRay(transform.position, bottom, Color.green);

        // Top-Bottom
        pos = new Vector3(0, boxCollider.size.y, 0);
        Vector3 top = transform.TransformDirection(Vector3.down) * 0.2f;
        Debug.DrawRay(pos, top, Color.red);

        RaycastHit hit;
        RaycastHit[] hits;
        Physics.Raycast(pos, top, out hit);

        // Change to fractureObject layer RaycastAll instead of ignoring parent object by name. 
        hits = Physics.RaycastAll(pos, top);

        foreach (var item in hits)
        {
            if(item.collider.gameObject.name.CompareTo("fractured_tall_block") == 0)
            {
                return;
            } else {
                Debug.Log(hit.collider.gameObject.name);
            }  
        }

        //Debug.Log(hit.collider.gameObject.name);

        // Forward-Backward
        pos = new Vector3(0, boxCollider.size.y / 2f, boxCollider.size.z * 0.5f);
        Vector3 forward = transform.TransformDirection(Vector3.back) * 0.2f;
        Debug.DrawRay(pos, forward, Color.magenta);

        // Left-Right
        pos = new Vector3(boxCollider.size.x * 0.5f, boxCollider.size.y / 2f, 0);
        Vector3 right = transform.TransformDirection(Vector3.left) * 0.2f;
        Debug.DrawRay(pos, right, Color.cyan);


        // If the raycast hits anything within 1cm to return since there's nothing to do.

        // Otherwise shrink the collider until the raycast hits.
    }

    /// <summary>
    /// Used to toggle the isKinematic boolean on each fractured object.
    /// </summary>
    /// <param name="b">true means isKinematic is checked, false is unchecked</param>
    public void ToggleRigidbodies(bool b)
    {
        foreach (var item in Rigidbodies)
        {
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

        foreach (var item in Rigidbodies)
        {
            Distances.Add(Vector3.Distance(item.transform.position, transform.position));
        }
    }

    private void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(0))
        {
            // Make rigidbodies only react to physics.
            ToggleRigidbodies(false);

            // Get current position of each fractured part.
            UpdateDistances();
            
            int i = 0;
            foreach (var item in Rigidbodies)
            {
                // If the distance from the clickable object is greater than the effect's distance, there's nothing to do. 
                if(distances[i] > effectDistance)
                {
                    i++;
                    return;
                } else
                {
                    i++;
                    // Otherwise, get the rigidbody and add an explosion force.
                    item.AddExplosionForce(explosionForce, this.transform.position, explosionRadius);
                } 
            }
        }
    }
}