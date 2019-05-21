var udp = require ('dgram');

const host = 'localhost'
const udpPort = 8080

var udpServer = udp.createSocket ('udp4').bind (udpPort)

let clients = []

udpServer.on ('close', function (err) {
    console.log ("UDP close")
})

udpServer.on ('error', function (err) {
    console.log ("UDP Error: " + err.toString ())
})

udpServer.on ('message', function (data, remote) {
    console.log ("UDP received")
    console.log("DATA: " + data)
    console.log("IP: " + remote.address)
    let sendMsg
    let rcvMsg = JSON.parse(data)
    if(rcvMsg.ACTION == 'REGISTER') {
        let action = {
            ACTION: "SPAWN_PLAYER"
        }
        sendMsg = JSON.stringify(action)
        clients.push({ name : rcvMsg.NAME, ip: remote.address, port : remote.port })
    }
    udpServer.send(sendMsg, 0, sendMsg.length, remote.port, remote.address )
    console.log(clients)
});

udpServer.on ('listening', function () {
    var address = udpServer.address ()
    console.log ("UDP Listening On IP: " + address.address + " at Port: " + address.port)
});