using System.ComponentModel.DataAnnotations.Schema;

namespace Wiwi.ScheduleCenter.Core.Domain.Base
{
    public abstract class EntityWithOperatorBase : EntityBase
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        [Column("create_time")]
        public DateTime CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 创建人
        /// </summary>
        [Column("creater")]
        public string Creater { get; set; }

        /// <summary>
        /// 创建人id
        /// </summary>
        [Column("create_id")]
        public string CreaterId { get; set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        [Column("last_modify_time")]
        public DateTime LastModifyTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 最后修改人
        /// </summary>
        [Column("last_modifier")]
        public string LastModifier { get; set; }

        /// <summary>
        /// 最后修改人id
        /// </summary>
        [Column("last_modifier_id")]
        public string LastModifierId { get; set; }

        #region 实体操作

        /// <summary>
        /// 添加创建人信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        public virtual void SetCreater(string userId, string userName)
        {
            Creater = userName;
            CreaterId = userId;
            CreateTime = DateTime.Now;
            SetModifier(userId, userName);
        }

        /// <summary>
        /// 添加修改人信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        public virtual void SetModifier(string userId, string userName)
        {
            LastModifierId = userId;
            LastModifier = userName;
            LastModifyTime = DateTime.Now;
        }

        #endregion
    }
}
