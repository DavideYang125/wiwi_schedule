namespace Wiwi.ScheduleCenter.Common
{
    public class ConstKeys
    {
        #region redisKey
        public const string SessionKey = "session:key:{0}";
        /// <summary>
        /// 
        /// </summary>
        public const string ApiScopes = "api:scopes";
        /// <summary>
        /// 
        /// </summary>
        public const string ApiRescurces = "api:rescurces";
        /// <summary>
        /// 用户登陆token
        /// </summary>
        public const string UserToken = "user:token";

        /// <summary>
        /// 用户
        /// </summary>
        public const string User = "user:info";
        /// <summary>
        /// 用户登陆错误统计
        /// </summary>
        public const string UserLoginError = "user:login:error";

        /// <summary>
        /// api操作缓存
        /// hash
        /// 子key path api路径
        /// </summary>
        public const string ApisKey = "role:apis";

        #region 消息中心

        /// <summary>
        /// 微信模板消息
        /// </summary>
        public const string WechatTemplateKey = "message:templates";

        /// <summary>
        /// 微信accesstoken
        /// </summary>
        public const string WechatAccessTokenKey = "message:accesstoken";

        /// <summary>
        /// 短信验证码缓存key
        /// </summary>
        public const string VerifyCodeKey = "message:verifycode:{0}";

        #endregion


        #region 有关设备的
        /// <summary>
        /// 网关发布订阅主题
        /// </summary>
        public const string GatewayPubSubKey = "gateway:pubsub";

        /// <summary>
        /// 网关品牌
        /// hash
        /// 子key 品牌id
        /// </summary>
        public const string GatewayBrandKey = "gateway:brand";
        /// <summary>
        /// 网关
        /// hash
        /// 子key 网关sn
        /// </summary>
        public const string GatewayKey = "gateway:sn:company:{0}";
        /// <summary>
        /// 网关
        /// hash
        /// 子key 网关id
        /// </summary>
        public const string GatewayIdKey = "gateway:id:company:{0}";
        /// <summary>
        /// 网关心跳
        /// hash
        /// 子key 网关id
        /// </summary>
        public const string GatewayHeartBeatKey = "gateway:heartbeat";
        /// <summary>
        /// 网关通道MQTT
        /// hash
        /// 子key 公司id
        /// </summary>
        public const string MqttChannelKey = "mqtt:channel:company:{0}";
        /// <summary>
        /// 设备缓存
        /// hash
        /// 子key 设备标识
        /// </summary>
        public const string EqKey = "eq:company:{0}";

        /// <summary>
        /// 设备参数缓存
        /// hash
        /// 子key 参数标识
        /// </summary>
        public const string EqParamsKey = "eq:company:{0}:params:{1}";

        /// <summary>
        /// 采集备参数缓存
        /// hash
        /// 子key 参数标识
        /// </summary>
        public const string CollectParamsKey = "collect:company:{0}:eq:{1}";

        /// <summary>
        /// 设备报警参数缓存
        /// hash
        /// 子key 参数标识
        /// </summary>
        public const string EqAlarmParamsKey = "eq:company:{0}:alarms:{1}";

        /// <summary>
        /// 采集设备缓存
        /// hash
        /// 子key 采集设备名称
        /// </summary>
        public const string CollectEquipmentKey = "collect:company:{0}:gatetwy:{1}";
        /// <summary>
        /// 采集设备和转发设备对应缓存
        /// hash
        /// 子key 采集设备名称
        /// </summary>
        public const string CollectEquipmentsKey = "collect:company:{0}:equipments";
        #endregion
        #endregion

        #region 通道消息
        /// <summary>
        /// 通道发布订阅主题
        /// </summary>
        public const string ChannelPubSubKey = "channel:pubsub";
        #endregion

        #region 实时消息
        /// <summary>
        /// 网关发布订阅主题
        /// </summary>
        public const string SignalRPubSubKey = "signalr:pubsub";
        /// <summary>
        /// SignalR会员id对应表
        /// </summary>
        public const string SignalRUserKey = "signalr:user";
        /// <summary>
        /// 公司实时数据
        /// hash 子key 类型 1Mqtt实时数据 2SignalR实时数据
        /// 值 实时数据服务编码
        /// </summary>
        public const string CompanyLiveDataKey = "livedata:company:{0}";

        /// <summary>
        /// 实时数据
        /// hash 子key 实时数据服务编
        /// 值 公司id编码
        /// </summary>
        public const string ServiceLiveDataKey = "livedata:code";

        #endregion

        #region ClaimType
        /// <summary>
        /// 登陆类型
        /// </summary>
        public const string LoginType = "login_type";
        /// <summary>
        /// 登陆Id
        /// </summary>
        public const string LoginId = "login_id";
        /// <summary>
        /// 登陆Id
        /// </summary>
        public const string UserId = "user_id";
        /// <summary>
        /// 公司Id
        /// </summary>
        public const string CompanyId = "company_id";
        /// <summary>
        /// 公司编码
        /// </summary>
        public const string CompanyCode = "company_code";
        /// <summary>
        /// 设备码
        /// </summary>
        public const string DeviceCode = "device_code";
        /// <summary>
        /// 角色Id
        /// </summary>
        public const string RoleId = "role_id";
        /// <summary>
        /// 真实姓名
        /// </summary>
        public const string RealName = "real_name";
        #endregion
    }
}
