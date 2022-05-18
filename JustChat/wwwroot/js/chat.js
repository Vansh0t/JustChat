
var connection = new signalR.HubConnectionBuilder().withUrl("/chat").build();
var selfUser = {}
const chatElem = $("#chat")
const messageTemplate= document.querySelector("#chatMessageTemplate")
const messageSelfTemplate= document.querySelector("#chatMessageSelfTemplate")
const chatInput = $("#chatInput")

const scrollTo=(pos)=>{
    chatElem.scrollTop(pos);
} 

const sendMessage = () => {
    connection.invoke("SendMessage", chatInput.val())
    chatInput.val("")
}

const addChatMessage = (time, sender, text) => {
    let clone;
    console.log(selfUser, sender)
    if(sender == selfUser.username) {
        
        clone = messageSelfTemplate.content.cloneNode(true)
    }
        
    else
        clone = messageTemplate.content.cloneNode(true)
    let localTime = new Date(time).toLocaleString()
    clone.querySelector(".chatSender p").innerText = sender
    let timeSpanElem = clone.querySelector(".messageTime").cloneNode(true)
    timeSpanElem.innerText = localTime
    let msgTextElem = clone.querySelector(".messageText")
    msgTextElem.innerText = text
    msgTextElem.append(timeSpanElem)
    chatElem.append(clone);
}

connection.on("InitChat", (user, messages) =>{
    console.log(messages)
    selfUser = user
    messages.forEach(msg => {
        addChatMessage(msg.time, msg.sender, msg.text);
    });
    
})

connection.on("ReceiveMessage", (time, sender, message) => {
    try {
        addChatMessage(time, sender, message)
        scrollTo(999999999999)
    } catch (error) {
        console.error(error)
    }
    
    console.log(`${time}: ${sender} says ${message}`)
    
    console.log("TT?")
});

connection.on("ReceiveMessages", (messages) => {
    console.log(messages)
});

connection.start().then(function () {
    console.log("Chat connection established")
    connection.invoke("InitChat")
}).catch(function (err) {
    return console.error(err.toString());
});


chatInput.on("keypress", (event) =>{
    if(event.key==="Enter") {
        sendMessage()
    }
});


//document.getElementById("sendButton").addEventListener("click", function (event) {
//    var user = document.getElementById("userInput").value;
//    var message = document.getElementById("messageInput").value;
//    connection.invoke("SendMessage", user, message).catch(function (err) {
//        return console.error(err.toString());
//    });
//    event.preventDefault();
//});