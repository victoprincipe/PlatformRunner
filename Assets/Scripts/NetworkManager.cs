using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Net.Sockets;
using System.Net;

public class NetworkManager : MonoBehaviour
{

    [SerializeField]
    int udpPort = 9082;
    [SerializeField]
    string host = "127.0.0.1";
    UTF8Encoding encoding = new UTF8Encoding();
    WaitForSeconds UdpDelay = new WaitForSeconds(0.3f);
    IPEndPoint UdpEndPoint = null;
    UdpClient client = null;

    [System.Serializable]
    public class JsonObject : System.Object
    {
        public string[] values;

        public JsonObject(string[] array)
        {
            values = array;
        }
    }

    public void UdpConnect()
    {
        client = new UdpClient();
    }

    public void UdpDisconnect()
    {
        client.Close();
    }

    void ParseDataToJson(byte[] data)
    {
        string JsonString = encoding.GetString(data);
        JsonObject Json = JsonUtility.FromJson<JsonObject>(JsonString);
    }

    IEnumerator ReceiveData()
    {
        byte[] data = null;
        while (true)
        {
            if (client.Available > 0)
            {
                yield return null;
                data = client.Receive(ref UdpEndPoint);
                ParseDataToJson(data);
                data = null;
            }
            yield return UdpDelay;
        }
    }

    public void SendCommand(params string[] values)
    {
        JsonObject Json = new JsonObject(values);
        string Jsonstring = JsonUtility.ToJson(Json);
        byte[] data = encoding.GetBytes(Jsonstring);
        client.Send(data, data.Length, host, udpPort);
    }

    void Start()
    {        
        UdpConnect();
        StartCoroutine(ReceiveData());
    }
}
