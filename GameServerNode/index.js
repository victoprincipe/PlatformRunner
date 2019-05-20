var udp = require ('dgram');

const host = '127.0.0.1'
const udpPort = 3030

var udpServer = udp.createSocket ('udp4').bind (udpPort, host)

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
    let msg = {
        x: 50,
        y: 30
    }
    msg = JSON.stringify(msg)
    udpServer.send( msg, 0, msg.length, remote.port, remote.address )
});

udpServer.on ('listening', function () {
    var address = udpServer.address ()
    console.log ("UDP Listening On IP: " + address.address + " at Port: " + address.port)
});