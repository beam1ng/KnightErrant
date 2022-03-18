using UnityEngine;

public class RandomHandler : MonoBehaviour
{
    public static RandomHandler RH;
    public System.Random Random;

    private void Awake()
    {
        if (RH != null)
        {
            Destroy(this);
        }
        else
        {
            RH = this;
        }

        Random = new System.Random((int) Time.time);
    }
}
