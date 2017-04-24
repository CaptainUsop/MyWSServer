using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fleck;

namespace MyWSServer
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                FleckLog.Level = LogLevel.Debug;
                var allSockets = new List<IWebSocketConnection>();
                var server = new WebSocketServer("ws://127.0.0.1:60000");
                server.Start(socket =>
                {
                    socket.OnOpen = () =>
                    {
                        Console.WriteLine("Open!");
                        foreach (string key in socket.ConnectionInfo.Headers.Keys)
                        {
                            string str = key + " " + socket.ConnectionInfo.Headers[key];
                            Console.WriteLine(str + "\n");
                        }
                        allSockets.Add(socket);
                    };
                    socket.OnClose = () =>
                    {
                        Console.WriteLine("Close!");
                        allSockets.Remove(socket);
                    };
                    socket.OnMessage = message =>
                    {
                        OnMessageInner(allSockets, message);
                    };
                });


                var input = Console.ReadLine();
                while (input != "exit")
                {
                    foreach (var socket in allSockets.ToList())
                    {
                        socket.Send(input);
                    }
                    input = Console.ReadLine();
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("[Error]:" + e.Message);
            }
        }

        private static void OnMessageInner(List<IWebSocketConnection> allSockets, string message)
        {
            Console.WriteLine(message);
            //allSockets.ToList().ForEach(s => s.Send("Echo: " + message));
            try
            {
                ViewMsg(message);
            }
            catch (Exception exp)
            {
                Console.WriteLine("Json 解析失败,"+exp.Message);
            }
        }
        private static void ViewMsg(string message)
        {
            MyJsonOperator oper = new MyJsonOperator();
            //List<JsonData> jdList = oper.Analyticaldata(JsonStringData.DataTwo);
            List<JsonData> jdList = oper.Analyticaldata(message);

            Console.WriteLine("{");
            foreach (JsonData jd in jdList)
            {
                if (jd.Value is List<List<JsonData>>)
                {
                    Console.WriteLine(" [");
                    List<List<JsonData>> llit = (List<List<JsonData>>)jd.Value;
                    foreach (List<JsonData> jdList2 in llit)
                    {
                        Console.WriteLine("     {");
                        foreach (JsonData jd2 in jdList2)
                        {
                            Console.WriteLine("         "+jd2.Name + "   :   " + jd2.Value.ToString());
                        }
                        Console.WriteLine("     }");
                    }
                    Console.WriteLine(" ]");
                }
                else if (jd.Value is List<JsonData>)
                {
                    Console.WriteLine(" {");
                    foreach (JsonData jd3 in (List<JsonData>)jd.Value)
                    {
                        Console.WriteLine("     "+jd3.Name + "   :   " + jd3.Value.ToString());
                    }
                    Console.WriteLine(" }");
                }
                else
                {
                    Console.WriteLine(" "+jd.Name + "   :   " + jd.Value.ToString());
                }
            }
            Console.WriteLine("}");
        }
    }
}