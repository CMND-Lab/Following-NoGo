using UnityEngine;
using UXF;

public class RecordBounds : MonoBehaviour
{
    public string GetPosition() {
        Vector3 position = gameObject.transform.position;
            
        string str_position = position.ToString("F4");
            
        return str_position;
    }

    public string GetSize() {
        Vector3 size;
        if (gameObject.GetComponent<BoxCollider>()) {
            size = gameObject.GetComponent<BoxCollider>().bounds.size;
        } else {
            size = gameObject.GetComponent<MeshRenderer>().bounds.size;
        }

        string str_size = size.ToString("F4");

        return str_size;
    }

    public void Record() {
        string name = gameObject.name;
        Debug.Log("Recording for " + name + "\tPos: " + GetPosition().ToString() + "\tSize: " + GetSize().ToString());

        Session.instance.participantDetails[name + "__position"] = GetPosition();
        Session.instance.participantDetails[name + "__size"] = GetSize();
    }
}