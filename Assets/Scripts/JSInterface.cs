using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices;
using UnityEngine.UI;

public class MessageData
{
    /// <summary>
    /// 接口方法类型
    /// </summary>
    public string methodNameEnum;

    /// <summary>
    /// 方法数据
    /// </summary>
    public JObject data;
}

public class SceneData
{
    public string sceneName;
}
public class HighlightDeviceID
{
    public List<string> device;
    public bool highlight;

}
public class ActiveData
{
    public List<string> device;
    public bool Active;

}
public class InitData
{
    public List<string> normalDevices;
    public List<string> warningDevices;

}
/// <summary>
/// JS通信接口
/// </summary>
public class JSInterface : MonoBehaviour
{

    /// <summary>
    /// 发送选中的监测点
    /// </summary>
    /// <param name="gateName"></param>
    /// <param name="id"></param>
    [DllImport("__Internal")]
    private static extern void sendGateID(string gateName, string id);



    /// <summary>
    /// 发送选中的监测点
    /// </summary>
    public void JSSendGateID(string gateName, string id)
    {
        Debug.Log($"Unity消息{gateName}  {id}");
#if !UNITY_EDITOR
        sendGateID(gateName, id);
#endif
    }


    //         }


    public Action<string, object> callback;

    /// <summary>
    /// JS通信入口
    /// </summary>
    /// <param name="jsJson"></param>
    public void MainInterface(string json)
    {
        Debug.Log($"JS消息：{json}");

        if (string.IsNullOrEmpty(json))
        {
            return;
        }

        try
        {
            var messageData = JsonConvert.DeserializeObject<MessageData>(json);

            switch (messageData.methodNameEnum)
            {
                case "SwitchScene":

                    {
                        var monitorData = messageData.data.ToObject<SceneData>();

                        if (monitorData != null)
                        {
                            callback?.Invoke(messageData.methodNameEnum, monitorData);
                        }
                        else
                        {
                            Debug.LogError($"切换场景数据序列化错误!!!{messageData.data}");
                        }
                    }
                    break;

                case "HightLightDevice":
                    {

                        var gateData = messageData.data.ToObject<HighlightDeviceID>();

                        if (gateData != null)
                        {
                            callback?.Invoke(messageData.methodNameEnum, gateData);
                        }
                        else
                        {
                            Debug.LogError($"高亮数据序列化错误!!!{messageData.data}");
                        }

                    }
                    break;

                case "InitData":
                    {
                        var lineChartData = messageData.data.ToObject<InitData>();

                        if (lineChartData != null)
                        {
                            callback?.Invoke(messageData.methodNameEnum, lineChartData);
                        }
                        else
                        {
                            Debug.LogError($"初始化场景数据序列化错误!!!{messageData.data}");
                        }
                    }
                    break;

                case "SetActive":
                    {
                        var activeData = messageData.data.ToObject<ActiveData>();

                        if (activeData != null)
                        {
                            callback?.Invoke(messageData.methodNameEnum, activeData);
                        }
                        else
                        {
                            Debug.LogError($"初始化场景数据序列化错误!!!{messageData.data}");
                        }
                    }
                    break;

                default:
                    {
                        UnityEngine.Debug.LogError($"指令错误{messageData.methodNameEnum}");
                    }
                    break;
            }
        }
        catch (Exception)
        {
            UnityEngine.Debug.LogError("json 解析错误");
        }
    }


}