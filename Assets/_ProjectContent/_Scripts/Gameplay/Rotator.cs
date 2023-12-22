using UnityEngine;

namespace Gameplay
{
    public class Rotator : MonoBehaviour
    {
        [SerializeField] private bool randomizeSpeed = true;
        [SerializeField] private float _degreesPerFrame;
        [SerializeField] private bool randomizeAxis = true;
        [SerializeField] private Vector3 _axis;

        private void Start()
        {
            _degreesPerFrame = randomizeSpeed ? Random.Range(180, 360) : _degreesPerFrame;
            _axis = randomizeAxis ? Random.insideUnitSphere : _axis;
        }

        private void LateUpdate()
        {
            transform.RotateAround(transform.position, _axis, _degreesPerFrame * Time.deltaTime);
        }
    }
}