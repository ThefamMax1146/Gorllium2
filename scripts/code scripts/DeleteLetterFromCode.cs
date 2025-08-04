using UnityEngine;

public class DeleteLetterFromCode : MonoBehaviour
{
    public JoinRoomCode JoinRoom;
    public string HandTag;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(HandTag) && JoinRoom.code.Length > 0)
        {
            JoinRoom.code = JoinRoom.code.Remove(JoinRoom.code.Length - 1);
        }
    }
}
