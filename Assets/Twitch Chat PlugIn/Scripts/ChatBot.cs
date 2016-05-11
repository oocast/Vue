using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Text;
using System;

public class ChatBot : MonoBehaviour {
    TcpClient _tcpClient = null;
    StreamReader _reader = null;
    StreamWriter _writer = null;
    NetworkStream _netStream = null;
    string _userName, _password;
    string _chatMessagePrefix;
    readonly string _chatMessageCode = "PRIVMSG";

    /// <summary>
    /// Action interface taking the message as input
    /// </summary>
    public event System.Action<string> onChatMessageParse;

    /// <summary>
    /// Action interface taking the username and message as input
    /// </summary>
    public event System.Action<string, string> onChatUserMessageParse;

    /// <summary>
    /// The name of the twitch channel that the player stream to
    /// </summary>
    // TODO: change channel name according to player config in the build
    public string channelName;

    // Use this for initialization
    void Start ()
    {
        // Get the chatbot credential
        _userName = "vuestreamer";
        TextAsset txt = (TextAsset)Resources.Load("Credential/twitchpassword", typeof(TextAsset));
        _password = txt.text;

        if (channelName == null)
        {
            Debug.LogError("Channel name empty");
        }
        // :<username>!<username>@<username>.tmi.twitch.tv PRIVMSG #<channelname> :
        // Used to send message to chat
        _chatMessagePrefix = string.Format(":{0}!{0}@{0}.tmi.twitch.tv PRIVMSG #{1} :", _userName, channelName);

        // Connect to the IRC server for the first time, although it's named reconnect
        Reconnect();
    }

    /// <summary>
    /// Clean up variables
    /// </summary>
    void OnApplicationQuit()
    {

        if (_reader != null)
        {
            _reader.Close();
        }

        if (_writer != null)
        {
            _writer.Close();
        }

        if (_tcpClient != null)
        {
            _tcpClient.Close();
        }

    }

    /// <summary>
    /// Connect to Twitch IRC server
    /// </summary>
    void Reconnect()
    {
        _tcpClient = new TcpClient("irc.chat.twitch.tv", 6667);
        _reader = new StreamReader(_tcpClient.GetStream());
        
        _writer = new StreamWriter(_tcpClient.GetStream());
        _netStream = _tcpClient.GetStream();

        
        _writer.WriteLine("PASS " + _password + Environment.NewLine
                       + "NICK " + _userName + Environment.NewLine
                       + "USER " + _userName);
        _writer.Flush();
        Debug.Log("Login request sent");

        Membership();
        Join(channelName);
    }

    /// <summary>
    /// Call the RESTful API to get the user list in the channel. Not used because it blocks the game for about 1 second
    /// </summary>
    public void GetViewerList()
    {
        WebRequest request = WebRequest.Create("http://tmi.twitch.tv/group/user/"+channelName+"/chatters");
        request.Timeout = 100;
        WebResponse response = null;
        try
        {
            response = request.GetResponse();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            Debug.Log("Viewer list request fail");
        }

        if (response == null)
        {
            return;
        }

        var stream = response.GetResponseStream();
        byte[] buffer = new byte[1000];
        stream.Read(buffer, 0, 1000);
        Debug.Log(Encoding.Default.GetString(buffer));
    }

    /// <summary>
    /// Join the channel specified by channelName
    /// </summary>
    /// <param name="channelName"></param>
    void Join(string channelName)
    {
        _writer.WriteLine("JOIN #" + channelName);
        _writer.Flush();
        Debug.Log("Join request sent");
    }

    /// <summary>
    /// Acquire channel membership update
    /// </summary>
    void Membership()
    {
        _writer.WriteLine("CAP REQ :twitch.tv/membership");
        _writer.Flush();
        Debug.Log("Member request sent");
    }
	
	// Update is called once per frame
	void Update () {
	    if (_tcpClient == null || !_tcpClient.Connected)
        {
            Debug.Log("Reconnecting...");
            Reconnect();
        }
        
        // Incoming messages awaiting
        if (_tcpClient.Available > 0)
        {
            string[] wholeMessages = ReceiveMessage();
            foreach (string wholeMessage in wholeMessages)
            {
                Debug.Log(wholeMessage);
                string chatMessage, chatSender;
                ParseChatMessageAndSender(wholeMessage, out chatMessage, out chatSender);
                if (chatMessage != null && onChatMessageParse != null)
                {
                    
                    onChatMessageParse(chatMessage);
                }
                if (chatMessage != null && chatSender != null && onChatUserMessageParse != null)
                {
                    Debug.Log(chatSender + ": " + chatMessage);
                    onChatUserMessageParse(chatSender, chatMessage);
                }
            }
        }
    }

    public void SendChatMessage(string message)
    {
        _writer.WriteLine("" + _chatMessagePrefix + message);
        _writer.Flush();
    }

    /// <summary>
    /// Receive message from TCP client, return in an array of lines of messages
    /// </summary>
    /// <returns>an array of lines of messages</returns>
    string[] ReceiveMessage()
    {
        byte[] buffer = new byte[_tcpClient.Available];
        // First read message to buffer
        // ReadLine doesn't work because the last message doesn't end with new line sign
        int size = _netStream.Read(buffer, 0, _tcpClient.Available);
        string wholeMessage = Encoding.Default.GetString(buffer);
        // "\r\n" is the line ending of Twitch chat
        string[] wholeMessages = wholeMessage.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        return wholeMessages;
    }

    /// <summary>
    /// Parse the part of message sent by user from wrapped Twitch IRC message syntax
    /// </summary>
    /// <param name="wholeMessage">Message in Twitch IRC message syntax</param>
    /// <returns>Message sent by user</returns>
    string ParseChatMessage(string wholeMessage)
    {
        string chatMessage = null;
        if (wholeMessage.Contains(_chatMessageCode) && wholeMessage.Contains("#" + channelName))
        {
            chatMessage = wholeMessage.Substring(wholeMessage.IndexOf(":", 1) + 1);
        }
        return chatMessage;
    }

    /// <summary>
    /// Parse the part of message sent by user and user name from wrapped Twitch IRC message syntax
    /// </summary>
    /// <param name="wholeMessage">Message in Twitch IRC message syntax</param>
    /// <param name="chatMessage">Message sent by user</param>
    /// <param name="chatSender">User name</param>
    void ParseChatMessageAndSender(string wholeMessage, out string chatMessage, out string chatSender)
    {
        if (wholeMessage.Contains(_chatMessageCode) && wholeMessage.Contains("#" + channelName))
        {
            // a chat message from connected channel. Only one connected channel for one running game
            chatMessage = wholeMessage.Substring(wholeMessage.IndexOf(":", 1) + 1);
            chatSender = wholeMessage.Substring(wholeMessage.IndexOf("!") + 1, (wholeMessage.IndexOf("@") - wholeMessage.IndexOf("!") - 1));
        }
        else
        {
            // not a Chat message from connected channel
            chatMessage = null;
            chatSender = null;
        }
    }
}
