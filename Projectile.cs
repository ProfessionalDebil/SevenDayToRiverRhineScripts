using UnityEngine;
using System;

public class Projectile : MonoBehaviour {
    public float speed;
    public float gravityEffect = 1;
    [NonSerialized]
    int prefabID;

    void Update() {
        Move();
    }
    void Move() {
        float move = Time.deltaTime * speed;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, move, 71)) {
            OnHit(hit);
        }
        transform.position += transform.forward * move;
        transform.position += Time.deltaTime * gravityEffect * Physics.gravity;
    }

    public void Initialize(int id) {
        prefabID = id;
    }

    void OnHit(RaycastHit objectHit) {
        // REMOVE THIS
        
        if (objectHit.transform.gameObject.name == "Target") {
            objectHit.transform.position -= new Vector3(0, 2, 0);
        }

        ProjectileManager.DespawnProjectile(this, prefabID);
    }
}