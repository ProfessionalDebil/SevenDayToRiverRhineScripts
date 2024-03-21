using UnityEngine;
using System;
using System.Collections.Generic;

public class ProjectileManager : MonoBehaviour {
    private static ProjectileManager objReference;
    private Dictionary<int, ProjectilePool> registeredPools;

    public void Awake() {
        objReference = this;
        registeredPools = new Dictionary<int, ProjectilePool>();
    }
    
    public static void SpawnProjectile(GameObject projectilePrefab, Vector3 position, Quaternion rotation) {
        int projectileID = projectilePrefab.GetInstanceID();
        if (!objReference.HasPool(projectileID)) {
            objReference.CreatePool(projectilePrefab, projectileID);
        }
        //ProjectilePool pool = registeredPools[projectileID];
        objReference.registeredPools[projectileID].Spawn(position, rotation);
    }

    public static void DespawnProjectile(Projectile projectileToRemove, int prefabID) {
        ProjectilePool pool = objReference.GetPool(prefabID);
        pool.Despawn(projectileToRemove);
    }

    public static void SpawnProjectile(Weapon weapon, Vector3 position, Quaternion rotation) {
        SpawnProjectile(weapon.values.projectilePrefab, position, rotation);
    }

    private bool HasPool(int projectileID) {
        return registeredPools.ContainsKey(projectileID);
    }

    private ProjectilePool GetPool(int prefabID) {
        if (objReference.HasPool(prefabID)) {
            return objReference.registeredPools[prefabID];
        }
        return null;
    }

    private ProjectilePool CreatePool(GameObject projectilePrefab, int projectileID) {
        ProjectilePool newPool = new ProjectilePool();
        newPool.Initialize(projectilePrefab, projectileID);
        registeredPools.Add(projectileID, newPool);
        return newPool;
    }

    public class ProjectilePool {
        private Stack<Projectile> unusedProjectiles;
        private GameObject projectilePrefab;
        private int prefabID;

        public Projectile Spawn(Vector3 position, Quaternion rotation) {
            Projectile projectileToUse;
            if (unusedProjectiles.Count > 0) {
                projectileToUse = unusedProjectiles.Pop();
                projectileToUse.gameObject.SetActive(true);
                projectileToUse.transform.position = position;
                projectileToUse.transform.rotation = rotation;
            }
            else {
                projectileToUse = Instantiate(projectilePrefab, position, rotation).GetComponent<Projectile>();
            }
            projectileToUse.Initialize(prefabID);
            return projectileToUse;
        }

        public void Despawn(Projectile proj) {
            proj.gameObject.SetActive(false);
            unusedProjectiles.Push(proj);
        }

        public void Initialize(GameObject prefab, int id) {
            unusedProjectiles = new Stack<Projectile>();
            projectilePrefab = prefab;
            prefabID = id;
        }
    }
}