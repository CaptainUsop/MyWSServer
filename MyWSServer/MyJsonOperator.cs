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
        /// <summary>
        /// 将JSON字符串数据解析成自定义的数据格式。
        /// </summary>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public List<JsonData> Analyticaldata(string jsonStr)
        {
            List<JsonData> jsonList = new List<JsonData>();

            JsonTextReader reader = new JsonTextReader(new StringReader(jsonStr));
            jsonList = ReadJsonData(reader);
            reader.Close();
            return jsonList;
        }

        /// <summary>
        /// 基于Newtonsoft.Json递归解析json数据，支持无限镶嵌
        /// </summary>
        /// <param name="reader">数据读取流</param>
        /// <returns>包含Json数据的集合</returns>
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