using Microsoft.AspNetCore.Mvc;
using Wiwi.ScheduleCenter.Common.Enums;

namespace Wiwi.ScheduleCenter.Common.Model.Result
{
    public class PageMessageOutput<T> : ListMessageOutput<T>
    {
        /// <summary>
        /// 返回数据行数
        /// </summary>
        public int total { get; set; }

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="response">数据</param>
        /// <param name="total">行数</param>
        /// <returns></returns>
        public static PageMessageOutput<T> Success(List<T> response, int total)
        {
            return Message(MessageStatus.Success, "success", response, total);
        }
        /// <summary>
        /// 返回失败
        /// </summary>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        public new static PageMessageOutput<T> Fail(string msg)
        {
            return Message(MessageStatus.Fail, msg, null, 0);
        }

        /// <summary>
        /// 返回消息
        /// </summary>
        /// <param name="status">失败/成功</param>
        /// <param name="msg">消息</param>
        /// <param name="response">数据</param>
        /// <param name="total">行数</param>
        /// <returns></returns>
        public static PageMessageOutput<T> Message(MessageStatus status, string msg, List<T> response, int total)
        {
            return new PageMessageOutput<T>() { status = status, msg = msg, response = response, total = total };
        }

    }

    public class ListMessageOutput<T> : MessageOutput
    {
        /// <summary>
        /// 返回数据集合
        /// </summary>
        public List<T> response { get; set; }


        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="response">数据</param>
        /// <returns></returns>
        public static ListMessageOutput<T> Success(List<T> response)
        {
            return Message(MessageStatus.Success, "success", response);
        }
        /// <summary>
        /// 返回失败
        /// </summary>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        public static ListMessageOutput<T> Fail(string msg)
        {
            return Message(MessageStatus.Fail, msg, null);
        }

        /// <summary>
        /// 返回消息
        /// </summary>
        /// <param name="status">失败/成功</param>
        /// <param name="msg">消息</param>
        /// <param name="response">数据</param>
        /// <returns></returns>
        public static ListMessageOutput<T> Message(MessageStatus status, string msg, List<T>? response)
        {
            return new ListMessageOutput<T>() { status = status, msg = msg, response = response };
        }
    }

    /// <summary>
    /// 通用返回信息类
    /// </summary>
    public class MessageOutput<T> : MessageOutput
    {
        /// <summary>
        /// 返回数据集合
        /// </summary>
        public T response { get; set; }


        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="response">数据</param>
        /// <returns></returns>
        public static MessageOutput<T> Success(T response)
        {
            return Message(MessageStatus.Success, "success", response);
        }
        /// <summary>
        /// 返回失败
        /// </summary>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        public static MessageOutput<T> Fail(string msg)
        {
            return Message(MessageStatus.Fail, msg, default);
        }

        /// <summary>
        /// 返回消息
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        public static MessageOutput<T> Message(MessageStatus status, string msg)
        {
            return Message(status, msg, default);
        }

        /// <summary>
        /// 返回消息
        /// </summary>
        /// <param name="status">失败/成功</param>
        /// <param name="msg">消息</param>
        /// <param name="response">数据</param>
        /// <returns></returns>
        public static MessageOutput<T> Message(MessageStatus status, string msg, T response)
        {
            return new MessageOutput<T>() { status = status, msg = msg, response = response };
        }
    }

    public class MessageOutput
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public MessageStatus status { get; set; } = MessageStatus.Success;
        /// <summary>
        /// 操作是否成功
        /// </summary>
        public bool success { get { return status == MessageStatus.Success; } }
        /// <summary>
        /// 返回信息
        /// </summary>
        public string msg { get; set; } = "success";

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        public static MessageOutput Success(string msg = "success")
        {
            return Message(MessageStatus.Success, msg);
        }
        /// <summary>
        /// 返回失败
        /// </summary>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        public static MessageOutput Fail(string msg)
        {
            return Message(MessageStatus.Fail, msg);
        }



        /// <summary>
        /// 返回消息
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        public static MessageOutput Message(MessageStatus status, string msg)
        {
            return new MessageOutput() { status = status, msg = msg };
        }

        /// <summary>
        /// 返回消息
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        public static MessageOutput Message(MessageStatus status)
        {
            return new MessageOutput() { status = status };
        }


        #region  控制器返回
        /// <summary>
        /// 控制器返回失败
        /// </summary>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        public static OkObjectResult ErrorResult(string msg)
        {
            return new OkObjectResult(Fail(msg));
        }

        /// <summary>
        /// 控制器返回成功
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns></returns>
        public static OkObjectResult Ok<T>(T data)
        {
            return new OkObjectResult(MessageOutput<T>.Success(data));
        }

        /// <summary>
        /// 控制器返回成功
        /// </summary>
        /// <returns></returns>
        public static OkObjectResult Ok()
        {
            return new OkObjectResult(Success());
        }
        #endregion
    }
}
