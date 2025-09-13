using UnityEngine;
using System.Collections.Generic;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance;

    public GameObject bulletPrefab;
    public int poolSize = 20;

    private List<Bullet> bullets = new List<Bullet>();

    void Awake()
    {
        Instance = this;

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(bulletPrefab);
            obj.SetActive(false);
            bullets.Add(obj.GetComponent<Bullet>());
        }
    }

    public void SpawnBullet(Vector3 position, Quaternion rotation, Vector2 direction, float speed)
    {
        foreach (var bullet in bullets)
        {
            if (!bullet.gameObject.activeInHierarchy)
            {
                bullet.transform.position = position;
                bullet.transform.rotation = rotation;
                bullet.Shoot(direction, speed);
                return;
            }
        }
        
        GameObject obj = Instantiate(bulletPrefab, position, rotation);
        Bullet newBullet = obj.GetComponent<Bullet>();
        newBullet.Shoot(direction, speed);
        bullets.Add(newBullet);
    }
}