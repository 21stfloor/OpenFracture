using UnityEngine;
using UnityEngine.Events;

public class HybridFracture: Prefracture
{
	public UnityEvent OnActivated;
	void OnTriggerEnter(Collider collider)
	{
		if (triggerOptions.triggerType == TriggerType.Trigger)
		{
			// Colliding object tag must be in the set of allowed collision tags if filtering by tag is enabled
			bool tagAllowed = triggerOptions.IsTagAllowed(collider.gameObject.tag);

			if (triggerOptions.filterCollisionsByTag && tagAllowed)
			{
				callbackOptions.CallOnFracture(collider, gameObject, transform.position);
				ActivatePrefractured(collider.transform.position);
			}
		}
	}

	public void ActivatePrefractured(Vector3 hitPoint)
	{
		var fracturedCopy = transform.parent.Find($"{gameObject.name}Fragments");
		if (fracturedCopy != null)
		{
			fracturedCopy.gameObject.SetActive(true);
			gameObject.SetActive(false);
			var fragment = fracturedCopy.GetComponentInChildren<UnfreezeFragment>();
			if(fragment != null)
			{
                fragment.Unfreeze();
			}
			var firstRigidbody = GetComponent<Rigidbody>();
			firstRigidbody.AddExplosionForce(2f, hitPoint, 2f);
		}
		OnActivated?.Invoke();

    }
}