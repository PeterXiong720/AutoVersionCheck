using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
using AutomaticVersionCheckAPI.Models;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Serialization;
using AutomaticVersionCheckAPI.data;
using System.Text;
using System.IO;

namespace AutomaticVersionCheckAPI.Controllers
{
    [Route("api/AutoVersionCheck")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/AutoVersionCheck
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            //返回"Empty App Name!"
            return new string[] { "[ERROR]Empty App Name!" };
        }

        // GET api/AutoVersionCheck/appName
        [HttpGet("{appName}")]
        public GetResponce Get(string appName)
        {
            //创建GetResponce对象resp
            GetResponce resp = new GetResponce();
            try
            {
                //读取数据，没做数据库，json代替
                DataHelper dataHelper = new DataHelper("./data/Apps.json");
                if (dataHelper.Exists(appName))
                {
                    //搜索数据
                    foreach(Data da in dataHelper.Datas)
                    {
                        if(da.AppName == appName)
                        {
                            resp.AppName = appName;
                            resp.Version = da.Version;
                        }
                    }
                }
                else
                {
                    resp.AppName = appName;
                    resp.Version = null;
                }
            }
            catch (Exception ex)
            {
                resp.AppName = appName;
                resp.Version = null;
                //Console.WriteLine(ex);
                string FilePath = "/log/ErrorLog.txt";

                StringBuilder msg = new StringBuilder();
                msg.Append("+=======================================+ \n");
                msg.AppendFormat(" 异常发生时间： {0} \n", DateTime.Now);
                msg.AppendFormat(" 异常类型： {0} \n", ex.HResult);
                msg.AppendFormat(" 导致当前异常的 Exception 实例： {0} \n", ex.InnerException);
                msg.AppendFormat(" 导致异常的应用程序或对象的名称： {0} \n", ex.Source);
                msg.AppendFormat(" 引发异常的方法： {0} \n", ex.TargetSite);
                msg.AppendFormat(" 异常堆栈信息： {0} \n", ex.StackTrace);
                msg.AppendFormat(" 异常消息： {0} \n", ex.Message);
                msg.Append("+=======================================+");
                if (!System.IO.File.Exists(FilePath))
                {
                    if (!System.IO.Directory.Exists("./log"))
                        System.IO.Directory.CreateDirectory("./log");
                    System.IO.File.Create(FilePath);
                }
                TextWriter tw = new StreamWriter(FilePath);
                tw.WriteLine(msg.ToString());
                tw.Flush();
                tw.Close();
                tw = null;
                throw;
            }
            return resp;
        }

        // POST api/values
        [HttpPost("{mode}")]
        /*public void Post([FromBody] string value)
        {
        }*/
        public PostResponce Post(PostRequest req
            //, string mode
            )
        {
            PostResponce resp = new PostResponce();
            try
            {
                DataHelper dataHelper = new DataHelper("./data/Apps.json");
                if (!dataHelper.Exists(req.AppName))
                { 
                    dataHelper.AddData(req);
                    resp.AppName = req.AppName;
                    resp.AccessKey = req.AccessKey;
                    resp.version = req.Version;
                    resp.result = "S";
                }
                else
                {
                    if (dataHelper.SetVersion(req))
                    {
                        resp.AppName = req.AppName;
                        resp.AccessKey = "密码正确";
                        resp.version = req.Version;
                        resp.result = "S";
                    }
                    else
                    {
                        resp.AppName = req.AppName;
                        resp.AccessKey = "密码错误";
                        resp.version = null;
                        resp.result = "F";
                    }
                }
            }
            catch (Exception ex)
            {
                resp.AppName = req.AppName;
                resp.version = null;
                //Console.WriteLine(ex);
                string FilePath = "./log/ErrorLog.txt";

                StringBuilder msg = new StringBuilder();
                msg.Append("+=======================================+ \n");
                msg.AppendFormat(" 异常发生时间： {0} \n", DateTime.Now);
                msg.AppendFormat(" 异常类型： {0} \n", ex.HResult);
                msg.AppendFormat(" 导致当前异常的 Exception 实例： {0} \n", ex.InnerException);
                msg.AppendFormat(" 导致异常的应用程序或对象的名称： {0} \n", ex.Source);
                msg.AppendFormat(" 引发异常的方法： {0} \n", ex.TargetSite);
                msg.AppendFormat(" 异常堆栈信息： {0} \n", ex.StackTrace);
                msg.AppendFormat(" 异常消息： {0} \n", ex.Message);
                msg.Append("+=======================================+");
                if(!System.IO.File.Exists(FilePath))
                {
                    if(!System.IO.Directory.Exists("./log"))
                        System.IO.Directory.CreateDirectory("./log");
                    System.IO.File.Create(FilePath);
                }
                TextWriter tw = new StreamWriter(FilePath);
                tw.WriteLine(msg.ToString());
                tw.Flush();
                tw.Close();
                tw = null;
                throw;
            }
            return resp;
        }

        /*// PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }*/
    }
}
