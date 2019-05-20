using UnityEngine;
 using System.Text;
 using System.Collections;
 using System.Net.Sockets;
 using System.Net;
 
 public class Web : MonoBehaviour {
 
     [SerializeField]
     int UdpPort = 9082;
     [SerializeField]
     string host = "127.0.0.1";
     [SerializeField]
     UTF8Encoding encoding = new UTF8Encoding ();
     [SerializeField]
     WaitForSeconds UdpDelay = new WaitForSeconds (.3f);
     [SerializeField]
     IPEndPoint UdpEndPoint = null;
     [SerializeField]
     UdpClient client = null;
 
     [System.Serializable]
     public class JsonObject : System.Object {
         public string[] values;
 
         public JsonObject (string[] array) {
             values = array;
         }
     }
 
     public void UdpConnect () {
         client = new UdpClient ();
     }
 
     public void UdpDisconnect () {
         client.Close ();
     }
 
     void UdpReceive(byte[] data){
         string JsonString = encoding.GetString (data);
         JsonObject Json = JsonUtility.FromJson <JsonObject> (JsonString);
     }
 
     IEnumerator UdpCoro() {
         byte[] data = null;
         while (true){
             if (client.Available > 0) {
                 yield return null;
                 data = client.Receive (ref UdpEndPoint);
                 UdpReceive (data);
                 data = null;
             }
             yield return UdpDelay;
         }
     }
 
     public void UdpSend (params string[] values){
         JsonObject Json = new JsonObject (values);
         string Jsonstring = JsonUtility.ToJson (Json);
         byte[] data = encoding.GetBytes (Jsonstring);
         client.Send (data, data.Length, host, UdpPort);
     }
 
     void Start (){
         UdpConnect ();
         UdpSend ("Test");
     }
 }