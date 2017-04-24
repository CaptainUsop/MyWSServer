using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.IO;

namespace MyWSServer
{
    /// <summary>
    /// json操作类
    /// 对json字符串的解析
    /// 将数据封装成json格式的字符串
    /// </summary>
    class MyJsonOperator
    {
        public List<JsonData> Analyticaldata(string jsonStr)
        {
            List<JsonData> jsonList = new List<JsonData>();

            JsonTextReader reader = new JsonTextReader(new StringReader(jsonStr));
            //while (reader.Read())
            //{
            //    bool valueIsNull = true;
            //    if (reader.Value != null)
            //    {
            //        valueIsNull = false;
            //        Console.WriteLine("Name: {0}, Value: {1}", reader.TokenType, reader.Value);
            //    }
            //    else
            //    {
            //        Console.WriteLine("Token: {0}", reader.TokenType);
            //    }
                                
            //}
            jsonList = ReadJsonData(reader);
            return jsonList;
        }

        private List<JsonData> ReadJsonData(JsonTextReader reader)
        {
            List<JsonData> jsonList = null;

            JsonData jd;
            while (reader.Read())
            {
                //判断是否开始
                if (reader.TokenType == JsonToken.StartObject)//说明有一组json数据即将开始读取数据
                {
                    jsonList = new List<JsonData>();
                    continue;
                }

                if (reader.TokenType == JsonToken.EndObject)//说明一组JsonData读取结束，如果没结束继续读取，如果结束则返回该List集合
                {
                    return jsonList;
                }

                if (reader.TokenType == JsonToken.EndArray)
                {
                    return null;
                }

                if (reader.TokenType == JsonToken.PropertyName)
                {
                    jd = new JsonData();
                    jd.Name = reader.Value.ToString();
                    if (reader.Read())
                    {
                        if (reader.TokenType == JsonToken.StartArray)//判断是否包含子集
                        {
                            List<List<JsonData>> jsonListList = new List<List<JsonData>>();
                            do
                            {
                                List<JsonData> temList = ReadJsonData(reader);
                                if (temList != null)
                                {
                                    jsonListList.Add(temList);
                                }
                                
                            } 
                            while (reader.TokenType != JsonToken.EndArray);

                            jd.Value = jsonListList;
                        }
                        else//如果不包含子集 就说明是JsonData的Value值
                        {
                            jd.Value = reader.Value;
                        }
                    }
                    jsonList.Add(jd);//将读取的数据加入集合中
                }

            }
            return jsonList;

        }
    }
}