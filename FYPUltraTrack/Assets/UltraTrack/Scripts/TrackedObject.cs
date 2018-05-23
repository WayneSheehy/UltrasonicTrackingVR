using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TrackedObject : MonoBehaviour {


	public ArduinoDataSource Source;
	public Transform SensorR, SensorM, SensorL;
	 Vector3 P1,P2,P3;
	float r1,r2,r3;
    public GameObject projectile;
    public Transform bulletSpawn;

    public float speed;

	TrilaterationCalculator TriCal = new TrilaterationCalculator ();
	Vector3 ans;
    int i = 0;
    int n = 10;
   Vector3[] tempPositions = new Vector3[10];

	bool full=false;
	Vector3 rollingAverage;
    private float gunCooldown = 0;

    // Use this for initialization
    void Start () {

		P1 = SensorR.position;
		P2 = SensorM.position;
		P3 = SensorL.position;
		r1 = Source.distance1;
		r2 = Source.distance2;
		r3 = Source.distance3;

        

		ans = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		float step = speed * Time.deltaTime;
        if (Source)
        {
            if (Source.HasData)
            {
                if (Source.triggerPulled)
                {
                    if (gunCooldown <= 0)
                    {
                        GameObject bullet = Instantiate(projectile, bulletSpawn.position, Quaternion.identity) as GameObject;
                        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 10;
                        GameObject.Destroy(bullet, 2f);
                        gunCooldown = 0.5f;
                    }
                }
                gunCooldown -= Time.deltaTime;


                r1 = Source.distance1;
                r2 = Source.distance2;
                r3 = Source.distance3;

                Vector3 newReading = TriCal.findIntersection(P1, r1, P2, r2, P3, r3);
                Vector3 difference = (newReading - rollingAverage);


                if (((difference.magnitude <= 0) || (difference.magnitude >= 200))  && full)
                {
                    newReading = rollingAverage;
                }



                if (full)
                {
                    print ("New Reading" + newReading);
                    //print ("Difference" + (newReading - tempPositions [i]));

                    rollingAverage += (1 / (float)n) * (newReading - tempPositions[i]);
                    //print(rollingAverage);
                }


                tempPositions[i] = newReading;



                i++;

                if (i >= n)
                {

                    i = 0;

                    if (!full)
                    {
                        full = true;
                        rollingAverage = GetMeanVector(tempPositions);
                    }

                }


                //Debug.Log (ans);
                //transform.Rotate(Source.OrientationChange.eulerAngles);
                this.transform.position = Vector3.MoveTowards(this.transform.position, rollingAverage, step);
            }
        }

	}


    private Vector3 GetMeanVector(Vector3[] positions)
    {
        if (positions.Length == 0)
            return Vector3.zero;
        float x = 0f;
        float y = 0f;
        float z = 0f;
        foreach (Vector3 pos in positions)
        {
            x += pos.x;
            y += pos.y;
            z += pos.z;
        }
        return new Vector3(x / positions.Length, y / positions.Length, z / positions.Length);
    }




}
