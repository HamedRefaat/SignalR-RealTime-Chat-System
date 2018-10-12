// declare tha chathb 
var chatHub
$(document).ready(function () {
    //assign the chat hub to our chathub class
    chatHub = $.connection.chatHub;


    //subscripe on the onNewUserConnected event so when a user connect the hub the assigend function will fire
    // that's the use of the SignalR js proxy
    chatHub.client.onNewUserConnected = function (userid, Name) {
        // what to do when a new user connect the chat app ?
        //ofcourse notifiy all user that the use is online now so thy can start a real time chat with him
        toastr.success(Name + " Is online Now");

        //what happend if the recent connected user is a new user just registerd now. So he is not in the user list

        if ($('#' + userid + '-1').length)         // is user in the user list
        {
            // just change his status to make him online
          
            $("#" + userid + "").css("color", "green").css("font-size", "xx-large");
            $("#chattab").text(Name + " is Online Now");

        }
        else {
            // a new user register while other users are online so we have to append the user name to the ul
            toastr.success("Say Hello to " + Name + "who just registerd Now");

            var item = ' <li id="' + userid + '-1" style="margin-left:-44px;"> <h3 id = "' + Name + '"  style = "display:inline-block;" > <a href="/chat/index/' + userid + '#chat">' + Name + '</a></h3> <span id="' + userid + '" style="color: green; font-size : xx-large">•</span></li >';

            $("#userlist").append(item);

        }


    }
    // if user connect to the chat while there are othe user online, so when he connect
    // he received a callback message from server tell'm who is online now
    chatHub.client.sendonlinuser = function (userid) {
        $("#" + userid + "").css("color", "green").css("font-size", "xx-large");

        
    }
    // when user logout or close the browser
    // the server send a message to all connected to tell that a user is offline

    chatHub.client.onUserDisconnected = function (connectedid, userId, userDisplayName) {

        var st = "offline";
        toastr.warning(userDisplayName + " Is " + st);
        $("#" + userId + "").css("color", "gray").css("font-size", "xx-large");

        $("#chattab").text(userDisplayName + " is Online Offline");

    }

    // register on the sendprivatemessage method so when some one send a message to other one
    //the recevier knows that he got an new message from the sender
    chatHub.client.sendPrivateMessage = function (Name, userId, gender, message) {
        // we have 2 senarioes here
        //1- the recevier user already open the chat windwo the the sender so in this case we append the message in the reveiver chat window
        //2- the revever dosent open the sender user chat windw so just notify him that he got a private message


        var inchatuser = $("#inChatuser").val();

        var d = new Date();
        var UserInChat = $("#inChatuser").val();
       // if the receiver open the sender chat windwo, hust append the message
        if (UserInChat == userId) {
            var tt = "Am";
            var hr = d.getHours();
            if (hr > 12) { hr = hr - 12; tt = "PM" }
            var hrr = hr + "";
            
            if (hr < 10) {
                hrr = "0" + hr;
            }

            
            var min = d.getMinutes();
            var minn = min + "";
            if (min < 10) { minn = "0" + min }

            var currntdate = "Today at " + hrr + ":" + minn + " " + tt;

            var toappend = '<p id="friend" style="color:green; text-align:left;"><div class="row"><div class="col-4" style="min-width: 140px; max-width :140px;"><img src="/Content/Images/' + gender + '-1.png" title="' + Name + '"</div><div class="col-8"><h3 style="margin-left: 20px;"> ' + Name + ' </h3><strong style="margin-left: 10px;">' + currntdate + '</strong</div></div><div class="row"><div class="col-12" style="margin-left: 20px;"><strong style="color:green; ">' + message + '</strong></div></div></p>';
            $('#chat').append(toappend);
            $('#chat').scrollTop($('#chat')[0].scrollHeight);
            $('#noconvers').hide();

        }
        else {
            // the receiver open othe chat window or dosn't open any, so just notify him
            // that he got a private message from sender and play a notification sound
            // to let him know if he minimize the browser windw
            toastr.success("You received a new message from your friend " + Name);
            $("#" + userId + "").css("color", "red").css("font-size", "xx-large");

            $("#" + userId + "-1").effect("shake", { direction: "right", times: 10, distance: 5 }, 1000);

            $('#notificationsound')[0].play();
        }
    }


    // start the hub to enable clients to register on events
    $.connection.hub.start().done(function () {

        var userid = $("#currentUserId").val();
        //   alert("started");
        var userDisplayName = $("#currentUserDisplayName").val();
        var userGender = $("#currentUserGender").val();
        chatHub.server.connect(userid, userDisplayName, userGender);
    });

});

