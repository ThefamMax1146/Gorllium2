using UnityEngine;
public class Colorer : MonoBehaviour
{
    public Color YourColor;
    public string Handtag;
    private void OnTriggerEnter(Collider other)
    {

        if (other.transform.tag == Handtag)
        {
            Color myColour = YourColor;
            NetworkManager.Instance.SetPlayerColor(myColour);
        }

    }
}
