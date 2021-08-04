using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Net;

/// <summary>
/// Summary description for Class1
/// </summary>
public class Smsalert
{
    private string apikey;
    private string username;
    private string password;
    private string authkey;

    public Smsalert(string username, string password)
    {
        this.username = username;
        this.password = password;
    }

    public Smsalert(string apikey)
    {
        this.apikey = apikey;
    }
    public string sendsms(string senderid, string mobileno, string text, string route)
    {
        if(apikey!=null)
            authkey="apikey="+ this.apikey;
        else
            authkey="user="+ this.username +"&pwd="+ this.password;  
        WebClient client = new WebClient();
        string apiurl = "http://www.smsalert.co.in/api/push.json?"+ authkey + "&sender=" + senderid + "&mobileno=" + mobileno + "&text=" + text + "&route=" + route;
        Stream data = client.OpenRead(apiurl);
        StreamReader reader = new StreamReader(data);
        string response = reader.ReadToEnd();
        data.Close();
        return response;
    }
}