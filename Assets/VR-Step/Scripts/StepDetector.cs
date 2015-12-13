using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public delegate void StepHandler();

public class StepDetector : MonoBehaviour
{

    private const int filterNumSamplesToAverage = 4;

    private int filterNumSamples = 0; //number of samples to average to pass them into the step detection
    private float filterTotalX = 0;

    private const int dynamicThresholdNumSamplesToUpdate = 50; //number of samples to update the dynamic threshold.
    private int dynamicThresholdNumSamples = 0;
    private float curMin = Single.PositiveInfinity; //minimum value of the last dynamicThresholdNumSamples samples
    private float curMax = Single.NegativeInfinity; //maximum value of the last dynamicThresholdNumSamples samples
    private float dynamicThreshold = 0; //the changing threshold to detect a step

    public float stepMinThreshold = .35f; //minimum threshold for a sample to be passed into the step detection stuff
    private float stepSampleOld = 0; //the last valid sample
    private float stepSampleNew = 0; //the potentially new valid sample, might = old sample if the input sample is below the minimum threshold

    private float timeSinceLastDetectedStep = 10;
    //private float stepIntervalMax = 1.5f;
    private const float stepIntervalMin = .2f;

    public StepHandler OnStepDetected;

	public bool SimulateStepsWithUpArrow = true;

	public static StepDetector instance;

	void Awake()
	{
		instance = this;
	}

	// Use this for initialization
	void Start ()
	{
		
	    bool gyoBool = SystemInfo.supportsGyroscope;
	    if (gyoBool)
	    {
	        Input.gyro.enabled = true;
			Input.compass.enabled = true;
	    }
	}
	
	// Update is called once per frame
	void Update () {
		//update step counter time
		timeSinceLastDetectedStep += Time.deltaTime;

		float upVector = GetUpwardsAcceleration(Input.gyro.userAcceleration);

		//sample once per fixed update, sum 4 samples together and average them to smooth out noise, use the average value of every 4 sums in the step detection
		if (filterNumSamples < filterNumSamplesToAverage)
		{
			filterTotalX += upVector;
			//filterTotalY += Input.acceleration.y;
			//filterTotalZ += Input.acceleration.z;
			filterNumSamples++;
		}
		else
		{
			//use the largest of the 3 accelerations for step detection, since the phone might be in a lot of different orientations
			float totalX = filterTotalX / filterNumSamplesToAverage;
			//float totalY = filterTotalY / filterNumSamplesToAverage;
			//float totalZ = filterTotalZ / filterNumSamplesToAverage;

			//float largest = ReturnLargest(totalX, ReturnLargest(totalY, totalZ));

			StepDetection(totalX);

			//reset filter counter and values.
			filterNumSamples = 0;
			filterTotalX = 0;
		}

		//update the dynamic threshold every dynamicThresholdNumSamplesToUpdate steps
		if (dynamicThresholdNumSamples < dynamicThresholdNumSamplesToUpdate)
		{

			if (upVector > curMax) curMax = upVector;
			if (upVector < curMin) curMin = upVector;

			dynamicThresholdNumSamples++;
		}
		else
		{
			dynamicThreshold = (curMax + curMin) / 2;

			//accelText.text = dynamicThreshold.ToString();

			curMin = Single.PositiveInfinity;
			curMax = Single.NegativeInfinity;
			dynamicThresholdNumSamples = 0;
		}


		if (Input.GetKeyDown(KeyCode.UpArrow) && SimulateStepsWithUpArrow)
		{
			OnStepDetected();
		}
	}

    void FixedUpdate()
    {
		
       

    }

    void StepDetection(float stepSampleResult)
    {
        stepSampleOld = stepSampleNew; //the old threshold value is always updated with whatever was in "new" before

        //Check to see if the difference in acceleration is at least some threshold, if not, stepSampleNew remains unchanged.
        if (Math.Abs(stepSampleResult - stepSampleNew) > stepMinThreshold)
        {
            stepSampleNew = stepSampleResult;

            //we detect a step if we have a negative slope when acceleration crosses below the dynamic threshold
            if (stepSampleNew < dynamicThreshold && stepSampleNew < stepSampleOld)
            {
                //check to see how long ago our last detected step was so that we avoid unrealistic detections
                if (timeSinceLastDetectedStep > stepIntervalMin)
                {
                    //Step detected!
                    timeSinceLastDetectedStep = 0;
                    OnStepDetected();
                }
            }
        }
    }

    float ReturnLargest(float a, float b)
    {
        if (a > b) return a;
        else return b;
    }

    float ReturnSmallest(float a, float b)
    {
        if (a < b) return a;
        else return b;
    }

    private float GetUpwardsAcceleration(Vector3 rawAccel)
    {
        Vector3 up = -Input.gyro.gravity;
        //Debug.Log(up);
        return Vector3.Dot(rawAccel, up);
    }
}
