using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ProjectileScript : MonoBehaviour {

    public GameObject _particalSystem;

    private float _speed;
    private Vector3 _dir;

    public void Start()
    {
        Destroy(gameObject, 3);
    }

    public void SetProjectile(Vector3 dir, float speed = 50.0f)
    {
        _speed = speed;
        _dir = dir.normalized;

        gameObject.GetComponent<Rigidbody>().velocity = _dir * _speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject part = Instantiate(_particalSystem, collision.transform.position, Quaternion.identity);
        Destroy(part, 1);
        Destroy(gameObject);
    }
}
