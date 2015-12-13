using UnityEngine;
using System.Collections;

public class WIPController : MonoBehaviour {

	private Rigidbody _rigidbody;

	public Transform cardboardHead;

	public float maxForwardVelocity = 4;
	public float minForwardVelocity = 1;
	public float dampingValue = .85f;

	public float stoppingStepTime = 1.3f;
	public float fastestStepTime = .2f;

	private float targetForwardVelocity;

	private float timeSinceLastStep = 5f;

	void Start () {
		_rigidbody = GetComponent<Rigidbody>();
		StepDetector.instance.OnStepDetected += OnStep;
	}
	
	void Update () {
		timeSinceLastStep += Time.deltaTime;
	}

	void FixedUpdate()
	{
		PointTowardsGaze();

		if (timeSinceLastStep > stoppingStepTime)
			targetForwardVelocity = 0;

		//set velocity based on our target
		Vector3 forwardVel = transform.forward * targetForwardVelocity;
		forwardVel.y = _rigidbody.velocity.y;
		_rigidbody.velocity = forwardVel;
		
		//apply damping
		targetForwardVelocity *= dampingValue;
	}

	void OnStep()
	{
		if (timeSinceLastStep > stoppingStepTime)
		{
			targetForwardVelocity = minForwardVelocity;
		}
		else if (timeSinceLastStep < fastestStepTime)
		{
			targetForwardVelocity = maxForwardVelocity;
		}
		else
		{
			float slope = (minForwardVelocity - maxForwardVelocity) / (stoppingStepTime - fastestStepTime);
			float yIntercept = -slope + minForwardVelocity;

			targetForwardVelocity = timeSinceLastStep * slope + yIntercept;
		}

		timeSinceLastStep = 0;
	}

	void PointTowardsGaze()
	{
		Vector3 currentRotation = transform.eulerAngles;
		currentRotation.y = cardboardHead.rotation.eulerAngles.y;
		transform.eulerAngles = currentRotation;
	}
}
