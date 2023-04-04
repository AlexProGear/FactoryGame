using System.Collections;
using FactoryGame.Data;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils.Extensions;

namespace FactoryGame.Factory.World
{
    [RequireComponent(typeof(Rigidbody))]
    public class ItemObject : MonoBehaviour
    {
        public ItemData data;
        [ShowInInspector, ReadOnly] public bool CanPickup { get; set; }

        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void OnSpawn()
        {
            AddSpawnImpulse();
            StartCoroutine(PickupCooldown());
        }

        private void AddSpawnImpulse()
        {
            Vector3 impulse = Random.onUnitSphere.WithY(0).normalized;
            impulse *= Random.Range(data.horizontalImpulseMin, data.horizontalImpulseMax);
            impulse.y = data.verticalImpulse;
            _rigidbody.angularVelocity = Random.insideUnitCircle * 10;
            _rigidbody.AddForce(impulse, ForceMode.Impulse);
        }

        private IEnumerator PickupCooldown()
        {
            CanPickup = false;
            yield return new WaitForSeconds(data.pickupCooldown);
            CanPickup = true;
        }

        public void TogglePhysics(bool active)
        {
            _rigidbody.isKinematic = !active;
        }
    }
}