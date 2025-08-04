using UnityEngine;
using TMPro;

public class AddLetterToCode : MonoBehaviour
{
    public JoinRoomCode joinRoom;
    public string Letter;
    public TextMeshPro ST;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HandTag"))
        {
            if (joinRoom.code.Length < 12)
            {
                joinRoom.code += Letter;
                ST.text = joinRoom.code;
            }
        }
    }

    private void FixedUpdate()
    {
        ST.text = joinRoom.code;
    }
}
