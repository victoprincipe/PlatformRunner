using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Net.Sockets;
using System.Net;

public class NetworkManager : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    int udpPort = 9082;
    [SerializeField]
    string host = "127.0.0.1";
    UTF8Encoding encoding = new UTF8Encoding();
    WaitForSeconds UdpDelay = new WaitForSeconds(0.3f);
    IPEndPoint UdpEndPoint = null;
    UdpClient client = null;

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
        JSONObject j = new JSONObject(JsonString);
        if(j["ACTION"].str == "SPAWN_PLAYER")
        {
            Instantiate(player, Vector3.up * 10f, Quaternion.identity);
        }
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
        JSONObject j = new JSONObject(JSONObject.Type.OBJECT);
        j.AddField("ACTION", "REGISTER");
        j.AddField("NAME", "Runewather");
        string Jsonstring = j.Print(true);
        byte[] data = encoding.GetBytes(Jsonstring);
        client.Send(data, data.Length, host, udpPort);
    }

    void Start()
    {        
        UdpConnect();
        StartCoroutine(ReceiveData());
        SendCommand();
    }
}
