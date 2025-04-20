const connection = new signalR.HubConnectionBuilder().withUrl("/callhub").build();
const localVideo = document.getElementById("localVideo");
const remoteVideo = document.getElementById("remoteVideo");

let localStream;
let peerConnection;
let remoteUserId;

const config = {
    iceServers: [
        { urls: "stun:stun.l.google.com:19302" }
    ]
};

connection.on("ReceiveOffer", async (fromUserId, offer) => {
    remoteUserId = fromUserId;
    peerConnection = createPeerConnection();
    await peerConnection.setRemoteDescription(new RTCSessionDescription(JSON.parse(offer)));
    const answer = await peerConnection.createAnswer();
    await peerConnection.setLocalDescription(answer);
    await connection.invoke("SendAnswer", remoteUserId, JSON.stringify(answer));
});

connection.on("ReceiveAnswer", async (fromUserId, answer) => {
    await peerConnection.setRemoteDescription(new RTCSessionDescription(JSON.parse(answer)));
});

connection.on("ReceiveCandidate", async (fromUserId, candidate) => {
    if (candidate) {
        await peerConnection.addIceCandidate(new RTCIceCandidate(JSON.parse(candidate)));
    }
});

function createPeerConnection() {
    const pc = new RTCPeerConnection(config);
    pc.onicecandidate = (event) => {
        if (event.candidate && remoteUserId) {
            connection.invoke("SendCandidate", remoteUserId, JSON.stringify(event.candidate));
        }
    };
    pc.ontrack = (event) => {
        remoteVideo.srcObject = event.streams[0];
    };
    localStream.getTracks().forEach(track => pc.addTrack(track, localStream));
    return pc;
}

async function startCall() {
    localStream = await navigator.mediaDevices.getUserMedia({ video: true, audio: true });
    localVideo.srcObject = localStream;
    peerConnection = createPeerConnection();
    const offer = await peerConnection.createOffer();
    await peerConnection.setLocalDescription(offer);
    await connection.invoke("SendOffer", remoteUserId, JSON.stringify(offer));
}

connection.start().then(() => {
    const urlParams = new URLSearchParams(window.location.search);
    remoteUserId = urlParams.get("with");
    if (remoteUserId) startCall();
});
