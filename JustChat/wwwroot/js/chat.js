const setUIError=(error) => {
    chatInput.prop("disabled", true)
    chatErrorMsg.text(error)
    chatErrorMsg.prop("hidden", false)
}
const unsetUIError = ()=> {
    chatInput.prop("disabled", false)
    chatErrorMsg.text("")
    chatErrorMsg.prop("hidden", true)
}


const chatErrorMsg = $("#chatError")
const chatInput = $("#chatInput")
var initConnAttempts = 3;
var connection = new signalR.HubConnectionBuilder()
    .withUrl("/chat")
    .withAutomaticReconnect()
    .build();

connection.onreconnecting(error => {
    setUIError("Connection lost. Reconnecting...")
    console.error(error);
});
connection.onreconnected(connId => {
    unsetUIError()
    console.debug("chat connection restored");
});
connection.onclose(error=> {
    setUIError("Chat connection lost.")
    console.error(error);
})

var selfUser = {}
const chatElem = $("#chat")
const messageTemplate= document.querySelector("#chatMessageTemplate")
const messageSelfTemplate= document.querySelector("#chatMessageSelfTemplate")

var firstMsg
var batchRequested = false;
var allMessagesLoaded = false;
const messages = []

const scrollTo=(pos)=>{
    chatElem.scrollTop(pos);
} 

const sendMessage = () => {
    connection.invoke("SendMessage", chatInput.val())
    chatInput.val("")
}
const renderStyle = ()=> {
    for (let i = 0; i < messages.length; i++) {
        const cur = messages[i]
        if(cur.styleRendered) 
            continue
        if(i!==messages.length-1) {
            const next = messages[i+1]
            if(next.sender===cur.sender) {
                cur.element.querySelector(".chatMessage").classList.remove("arrow")
                cur.element.querySelector(".chatSender").classList.add("invisible")
            }
            else {
                cur.element.querySelector(".chatMessage").classList.add("arrow")
            }
            cur.styleRendered=true
        }
        else {
            cur.element.querySelector(".chatMessage").classList.add("arrow")
        }
    }
}
const prepMsgTemplate = (time,sender,text,avatarUrl)=>{
    let clone;
    if(sender == selfUser.username) {
        clone = messageSelfTemplate.content.cloneNode(true)
    }
    else
        clone = messageTemplate.content.cloneNode(true)
    let localTime = new Date(time).toLocaleString()
    if(!avatarUrl)
        clone.querySelector(".chatSender p").innerText = sender
    else
        clone.querySelector(".chatSender").style.backgroundImage = `url('${avatarUrl}')`

    let timeSpanElem = clone.querySelector(".messageTime").cloneNode(true)
    timeSpanElem.innerText = localTime
    let msgTextElem = clone.querySelector(".messageText")
    msgTextElem.innerText = text
    msgTextElem.append(timeSpanElem)
    return clone
}
const appendChatMessage = (time, sender, text, avatarUrl) => {
    let template = prepMsgTemplate(time,sender,text,avatarUrl)
    chatElem.append(template);
    messages.push({
        time:time,
        sender:sender,
        text:text,
        element:chatElem.children().last()[0],
        styleRendered : false
    })
}
const prependChatMessage = (time, sender, text, avatarUrl) => {
    let template = prepMsgTemplate(time,sender,text,avatarUrl)
    chatElem.prepend(template);
    messages.unshift({
        time:time,
        sender:sender,
        text:text,
        element:chatElem.children().first()[0],
        styleRendered : false
    })
}


chatElem.scroll(() => { 
    const SCROLL_THRESHOLD = 50;
    if(!allMessagesLoaded) {
        if(chatElem.scrollTop() < SCROLL_THRESHOLD) {
            connection.invoke("GetMessages", firstMsg.time) //request next batch of messages before first
            batchRequested = true
        }
        else {
            batchRequested = false
        }
    }
    
});

connection.on("InitChat", (user, messages) =>{
    try {
        console.log(messages)
        selfUser = user
        if(messages.length> 0)
            firstMsg = messages[0]
        messages.forEach(msg => {
            appendChatMessage(msg.time, msg.sender, msg.text, msg.avatarUrl);
        });
        renderStyle()
        scrollTo(999999999999)
    } catch (error) {
        console.error(error)
    }
})
connection.on("ReceiveMessages", (messages) =>{
    try {
        console.debug("new batch of messages received, length - ", messages.length)
        if(messages.length> 0)
            firstMsg = messages.at(-1)
        else
            allMessagesLoaded = true //got empty messages, all messages loaded, stop requesting more
        messages.forEach(msg => {
            prependChatMessage(msg.time, msg.sender, msg.text, msg.avatarUrl);
        });
        renderStyle()
    } catch (error) {
        console.error(error)
    }
})

connection.on("ReceiveMessage", (time, sender, message, avatarUrl) => {
    try {
        appendChatMessage(time, sender, message, avatarUrl)
        renderStyle()
        if(sender === selfUser.username)
            scrollTo(999999999999)
    } catch (error) {
        console.error(error)
    }
});

const startChat = () => {
    connection.start().then(function () {
        console.debug("Chat connection established")
        connection.invoke("InitChat")
    }).catch(function (err) {
        if(initConnAttempts>0) {
            initConnAttempts-=1
            setTimeout(startChat, 1000)
        }
        else {
            setUIError("Unable to connect to chat. Make sure you are loged in and your email is confirmed.");
        }   
        return console.error(err.toString());
    });
}

startChat();


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