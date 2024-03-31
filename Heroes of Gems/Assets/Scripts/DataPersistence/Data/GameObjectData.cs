using UnityEngine;

[System.Serializable]
public class GameObjectData {
    public string name;
    //public Vector3 position;
    //public Quaternion rotation;
    //public Vector3 scale;

    public GameObjectData(GameObject obj) {
        name = obj.name;
        //position = obj.transform.position;
        //rotation = obj.transform.rotation;
        //scale = obj.transform.localScale;
    }
}