using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpiral : MonoBehaviour
{

    // Controls the radius of the circular motion for X/Y axis.
    public Vector2 Radii = new Vector2(2f, 3f);

    // Controls the direction/speed the spiral translates
    public Vector3 LinearVelocity = new Vector3(0.5f,0, 0f);

    // Controls the rotational speed of the spiral.
    public float AngularVelocity = 1f;

    // Scales the speed of the spiral motion,
    public float TimeScale = 2f;

    float m_Time;

    void Update()
    {
        // Calculate the angle at this time.
        var angle = AngularVelocity * m_Time;

        // Calculate the linear translation at this time.
        var position = LinearVelocity * m_Time;

        // Add in the circular motion.
        position += new Vector3(Mathf.Cos(angle) * Radii.x, 0, Mathf.Sin(angle) * Radii.y);

        // Set the transform to this spiral position.
        transform.position = position;

        // Adjust time.
        m_Time += TimeScale * Time.deltaTime;
    }

}
