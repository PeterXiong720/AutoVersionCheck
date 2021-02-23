using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using AutomaticVersionCheckAPI.Models;
using Newtonsoft.Json;

namespace AutomaticVersionCheckAPI.data
{
    public partial class DataHelper
    {
        public DataHelper(string Path)
        {
            this.path = Path;
            //读取数据
            string json = this.open();
            //如果数据不为空
            if (json != "Empty")
            { 
                //反序列化json对象至C Sharp对象List
                List<Data> lists = JsonConvert.DeserializeObject<List<Data>>(json);
                Datas = lists;
            }
            
        }

        ~DataHelper() { Datas = null; }

        /// <summary>
        /// 是否存在某个App
        /// </summary>
        /// <param name="AppName"></param>
        /// <returns></returns>
        public bool Exists(string AppName)
        {
            //搜索数据
            foreach (Data data in Datas) 
            {
                //找到数据
                if (data.AppName == AppName)
                    return true;
                else
                    continue;
            }
            return false;
        }

        /// <summary>
        /// 添加新App数据
        /// </summary>
        /// <param name="req"></param>
        public void AddData(PostRequest req)
        {
            try
            {
                //笨办法，手动复制对象
                Data da = new Data();
                da.Version = new AppVersion();
                da.AppName = req.AppName;
                da.AccessKey = req.AccessKey;
                da.Version.Latest = req.Version.Latest;
                da.Version.preview = req.Version.preview;
                Datas.Add(da);

                //序列化并写入本地文件
                string json = JsonConvert.SerializeObject(Datas);
                File.WriteAllText(this.path, json);
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        /// <summary>
        /// 更新现有App版本号
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool SetVersion(PostRequest req)
        {
            //搜索数据
            foreach(Data da in Datas)
            {
                if(req.AppName == da.AppName && req.AccessKey == da.AccessKey)
                {
                    //更新版本号
                    da.Version = req.Version;
                    string json = JsonConvert.SerializeObject(Datas);
                    File.WriteAllText(this.path, json);
                    //返回执行成功
                    return true;
                }
            }
            //默认返回
            return false;
        }

        public List<Data> Datas;
    }

    public partial class DataHelper
    {
        private string path;

        private string open()
        {
            if(File.Exists(this.path))
            {
                return File.ReadAllText(this.path);
            }
            else
            {
                File.Create(this.path);
                File.WriteAllText(this.path, "[]");
                return "Empty";
            }
        }
    }
}
