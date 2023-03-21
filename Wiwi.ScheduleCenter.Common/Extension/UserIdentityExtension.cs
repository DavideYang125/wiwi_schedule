using System.Security.Claims;
using Newtonsoft.Json;
using Wiwi.ScheduleCenter.Common.Configs;

namespace Wiwi.ScheduleCenter.Common.Extension
{
    public static class UserIdentityExtension
    {
        /// <summary>
        /// 获得用户的Name
        /// </summary>
        /// <returns></returns>
        public static string GetName(this ClaimsPrincipal @this)
        {
            return @this?.Identity?.Name;
        }
        /// <summary>
        /// 获得公司编码
        /// </summary>
        /// <returns></returns>
        public static string GetCompanyCode(this ClaimsPrincipal @this)
        {
            return GetClaim(@this, ConstKeys.CompanyCode);
        }
        /// <summary>
        /// 会员Id
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static int GetUserId(this ClaimsPrincipal @this)
        {
            int.TryParse(GetClaim(@this, ConstKeys.UserId), out var userId);
            return userId;
        }
        /// <summary>
        /// 获取会员
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static User GetUser(this ClaimsPrincipal @this)
        {
            var userId = GetUserId(@this);
            if (userId > 0 && RedisHelper.HExists(ConstKeys.User, userId.ToString()))
            {
                return RedisHelper.HGet<User>(ConstKeys.User, userId.ToString());
            }
            return default;
        }

        /// <summary>
        /// 公司Id
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static int GetCompanyId(this ClaimsPrincipal @this)
        {
            int.TryParse(GetClaim(@this, ConstKeys.CompanyId), out var companyId);
            return companyId;
        }
        /// <summary>
        /// 角色Id
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static List<int> GetRoleId(this ClaimsPrincipal @this)
        {
            var rolyIds = GetClaim(@this, ConstKeys.RoleId);
            if (!string.IsNullOrEmpty(rolyIds))
            {
                return JsonConvert.DeserializeObject<List<int>>(rolyIds);
            }
            return new List<int>();
        }

        /// <summary>
        /// 获取ClaimValue
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static string GetClaim(this ClaimsPrincipal @this, string ClaimType)
        {
            return @this?.Claims?.FirstOrDefault(it => it.Type == ClaimType)?.Value;
        }


    }
}
