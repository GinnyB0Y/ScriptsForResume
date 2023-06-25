using UnityEngine;

public class Rotation : MonoBehaviour
{
    public float rotationSpeed;
    public bool jump;
    public float X;
    public float rotateAngle;

    void Update()
    {
        if (jump)
        {
            transform.Rotate(rotationSpeed * Time.deltaTime, 0, 0);
            rotateAngle += rotationSpeed * Time.deltaTime;
        }

        else
        {
            X = rotateAngle;

            if (X > 0 && X < 90)
            {
                transform.Rotate(rotationSpeed * Time.deltaTime * 5, 0, 0);
                rotateAngle += rotationSpeed * Time.deltaTime * 5;
                
                if (rotateAngle >= 90)
                    {
                        transform.rotation = Quaternion.Euler(90, 0, 0);
                    rotateAngle = 90;
                    }
            }

            if (X > 90 && X < 180)
            {
                transform.Rotate(rotationSpeed * Time.deltaTime * 5, 0, 0);
                rotateAngle += rotationSpeed * Time.deltaTime * 5;
                if (rotateAngle >= 180)
                {
                    transform.rotation = Quaternion.Euler(180, 0, 0);
                    rotateAngle = 180;
                }
            }

            if (X > 180 && X < 270)
            {
                transform.Rotate(rotationSpeed * Time.deltaTime * 5, 0, 0);
                rotateAngle += rotationSpeed * Time.deltaTime * 5;
                if (rotateAngle >= 270)
                {
                    transform.rotation = Quaternion.Euler(270, 0, 0);
                    rotateAngle = 270;
                }
            }

            if (X > 270)
            {
                transform.Rotate(rotationSpeed * Time.deltaTime * 5, 0, 0);
                rotateAngle += rotationSpeed * Time.deltaTime * 5;
                if (rotateAngle > 360)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    rotateAngle = 0;
                }
            }
        }
        if(rotateAngle >= 360)
            rotateAngle = 0;
    }
}
