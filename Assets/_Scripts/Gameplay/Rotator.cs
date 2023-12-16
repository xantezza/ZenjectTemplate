using UnityEngine;

namespace Gameplay
{
    public class Rotator : MonoBehaviour
    {
        private float _speed;
        private Vector3 _axis;

        private void Start()
        {
            _speed = Random.Range(180, 360);
            _axis = Random.insideUnitSphere;
        }

        private void LateUpdate()
        {
            transform.RotateAround(transform.position, _axis, _speed * Time.deltaTime);
        }
    }
}