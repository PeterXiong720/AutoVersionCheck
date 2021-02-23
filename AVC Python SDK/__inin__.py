import requests
import json

def GetVersion(api, app_name):
    """
    Args：
    api - url, eg. http://<YourHostName>:5000/api/AutoVersionCheck/
    app_name - str
    "version":{
        "latest":"example",
        "preview":"example"
    }
    """
    result = json.loads(requests.get(api + app_name).text)
    version = result['version']
    if version == 'null':
        return "App不存在"
    return version

def Update(api, app_name, accesskey, latest_ver, preview_ver):
    """
    Args：
    api - url, eg. http://<YourHostName>:5000/api/AutoVersionCheck/
    app_name - str
    accesskey - str
    latest_ver - str
    preview_ver - str
    返回值：
    "New App添加成功",
    "%s更新成功" %(app_name),
    "未知错误"
    """
    header = {'Content-Type':'application/json'}
    
    datas = {}
    datas['version'] = {}
    datas['appname'] = app_name
    datas['accesskey'] = accesskey
    datas['version']['latest'] = latest_ver
    datas['version']['preview'] = preview_ver

    result = json.loads(requests.post(api, data=datas, headers= header).text)
    if result['result'] == "S":
        if result['accessKey'] == accesskey:
            return "New App添加成功"
        if result['accessKey'] == '密码正确':
            return "%s更新成功" %(app_name)
    if result['result'] == "F":
        if result['accessKey'] == "密码错误":
            return result['accessKey']
        else:
            return "未知错误"
    return "未知错误"

