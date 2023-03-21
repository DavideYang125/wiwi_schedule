using System.ComponentModel;
using System.Runtime.Serialization;

namespace Wiwi.ScheduleCenter.Common.Enums
{
    public enum Sort
    {
        /// <summary>
        /// 正序
        /// </summary>
        Asc,
        /// <summary>
        /// 倒序
        /// </summary>
        Desc,
    }

    /// <summary>
    /// 消息状态
    /// </summary>
    public enum MessageStatus
    {
        /// <summary>
        /// 成功
        /// </summary>
        [Description("成功")]
        Success = 200,
        /// <summary>
        /// 失败 一般应用错误
        /// </summary>
        [Description("失败")]
        Fail = 300,
        /// <summary>
        /// 授权错误
        /// </summary>
        [Description("授权错误")]
        UnAuthorization = 401,
        /// <summary>
        /// 刷新token失败
        /// </summary>
        [Description("刷新token失败")]
        RefreshToken = 402,
        /// <summary>
        /// 异常错误
        /// </summary>
        [Description("异常错误")]
        Error = 500
    }

    /// <summary>
    /// 状态
    /// </summary>
    public enum Status
    {
        /// <summary>
        ///未知状态
        /// </summary>
        UnKnown = 0,
        /// <summary>
        /// 禁用
        /// </summary>
        Disabled = 1,
        /// <summary>
        /// 启用
        /// </summary>
        Enabled = 2,
    }
    /// <summary>
    /// 性别
    /// </summary>
    public enum Gender
    {
        /// <summary>
        ///保密
        /// </summary>
        UnKnown = 0,
        /// <summary>
        /// 女性
        /// </summary>
        Female = 1,
        /// <summary>
        /// 男性
        /// </summary>
        Male = 2,

    }

    public enum MqttStatus
    {
        /// <summary>
        /// 未连接
        /// </summary>
        Closed = 0,
        /// <summary>
        /// 重连中
        /// </summary>
        TryConnect = 1,
        /// <summary>
        /// 运行中
        /// </summary>
        Runing = 2
    }

    /// <summary>
    /// 数据类型
    /// </summary>
    public enum DataType
    {
        /// <summary>
        /// 未定义
        /// </summary>
        [Description("未定义")]
        [EnumMember(Value = "Undefined")]
        Undefined = -1,
        /// <summary>
        /// Bool
        /// </summary>
        [EnumMember(Value = "Bool")]
        [Description("Bool")]
        Bool = 0,
        /// <summary>
        /// Byte
        /// </summary>
        [EnumMember(Value = "Byte")]
        [Description("Byte")]
        Byte = 1,
        /// <summary>
        /// UShort
        /// </summary>
        [EnumMember(Value = "UShort")]
        [Description("UShort")]
        UShort = 2,
        /// <summary>
        /// Short
        /// </summary>
        [EnumMember(Value = "Short")]
        [Description("Short")]
        Short = 3,
        /// <summary>
        /// UInt
        /// </summary>
        [EnumMember(Value = "UInt")]
        [Description("UInt")]
        UInt = 4,
        /// <summary>
        /// Int
        /// </summary>
        [EnumMember(Value = "Int")]
        [Description("Int")]
        Int = 5,
        /// <summary>
        /// ULong
        /// </summary>
        [EnumMember(Value = "ULong")]
        [Description("ULong")]
        ULong = 6,
        /// <summary>
        /// Long
        /// </summary>
        [EnumMember(Value = "Long")]
        [Description("Long")]
        Long = 7,
        /// <summary>
        /// Float
        /// </summary>
        [EnumMember(Value = "Float")]
        [Description("Float")]
        Float = 8,
        /// <summary>
        /// Double
        /// </summary>
        [EnumMember(Value = "Double")]
        [Description("Double")]
        Double = 9,
        /// <summary>
        /// String
        /// </summary>
        [EnumMember(Value = "String")]
        [Description("String")]
        String = 10,
        /// <summary>
        /// Enum
        /// </summary>
        [EnumMember(Value = "Enum")]
        [Description("Enum")]
        Enum = 11,
        /// <summary>
        /// DateTime
        /// </summary>
        [EnumMember(Value = "DateTime")]
        [Description("DateTime")]
        DateTime = 12
    }
    /// <summary>
    /// SignalR消息类型
    /// </summary>
    public enum SignalRMessageType
    {

        #region 网关消息
        /// <summary>
        /// 网关采集消息
        /// </summary>
        GatewayCollectData = 10,
        /// <summary>
        /// 网关预警消息
        /// </summary>
        GatewayAlarmData = 11,
        /// <summary>
        /// 网关上行数据
        /// </summary>
        GatewayOperateDevice = 20,
        /// <summary>
        /// 网关消息监控
        /// </summary>
        GatewayWatchDevice = 21,
        #endregion

        #region 实时数据
        LiveData = 100,
        AlarmData = 110
        #endregion
    }
    /// <summary>
    /// SignalR消息类型
    /// </summary>
    public enum SignalRMessageRange
    {

        /// <summary>
        /// 全网消息
        /// </summary>
        All = 0,
        /// <summary>
        /// 公司消息
        /// </summary>
        Company = 1,
        /// <summary>
        /// 个人
        /// </summary>
        Person = 2,
    }

    /// <summary>
    /// 实时数据类型
    /// </summary>
    public enum LiveDataType
    {
        /// <summary>
        /// 未定义
        /// </summary>
        [Description("未定义")]
        Undefined = 0,
        /// <summary>
        /// Mqtt实时数据
        /// </summary>
        [Description("Mqtt实时数据")]
        MqttServce = 1,
        /// <summary>
        /// SignalR实时数据
        /// </summary>
        [Description("SignalR实时数据")]
        SignalRService
    }

    #region message

    /// <summary>
    /// 邮件类型
    /// </summary>
    public enum MailTypeEnum
    {
        [Description("验证码")] VerifyCode = 1,
        [Description("通知类消息")] Template = 2,
    }

    /// <summary>
    /// 短信模板类型
    /// </summary>
    public enum SmsTemplateTypeEnum
    {
        [Description("验证码")] VerifyCode = 1,
        [Description("通用")] Common = 2,
    }

    #endregion
}
