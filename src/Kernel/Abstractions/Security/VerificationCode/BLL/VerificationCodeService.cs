namespace X3Platform.Security.VerificationCode.BLL
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;

    using Common.Logging;

    using X3Platform.CacheBuffer;
    using X3Platform.Data;
    using X3Platform.Spring;

    using X3Platform.Security.VerificationCode.Configuration;
    using X3Platform.Security.VerificationCode.IBLL;
    using X3Platform.Security.VerificationCode.IDAL;
    using X3Platform.DigitalNumber;
    using X3Platform.Util;
    #endregion

    /// <summary>权限服务</summary>
    public class VerificationCodeService : IVerificationCodeService
    {
        /// <summary>数据提供器</summary>
        private IVerificationCodeProvider provider = null;

        /// <summary>缓存存储</summary>
        private IDictionary<string, VerificationCodeInfo> dictionary = new Dictionary<string, VerificationCodeInfo>();

        private DateTime actionTime = DateTime.Now;

        #region 构造函数:VerificationCodeService()
        /// <summary>构造函数</summary>
        public VerificationCodeService()
        {
            // 创建对象构建器(Spring.NET)
            string springObjectFile = VerificationCodeConfigurationView.Instance.Configuration.Keys["SpringObjectFile"].Value;

            SpringObjectBuilder objectBuilder = SpringObjectBuilder.Create(VerificationCodeConfiguration.ApplicationName, springObjectFile);

            this.provider = objectBuilder.GetObject<IVerificationCodeProvider>(typeof(IVerificationCodeProvider));
        }
        #endregion

        //-------------------------------------------------------
        // 保存 删除
        //-------------------------------------------------------

        #region 函数:Save(VerificationCodeInfo param)
        /// <summary>保存记录</summary>
        /// <param name="param"> 实例<see cref="VerificationCodeInfo"/>详细信息</param>
        /// <returns>VerificationCodeInfo 实例详细信息</returns>
        public VerificationCodeInfo Save(VerificationCodeInfo param)
        {
            return this.provider.Save(param);
        }
        #endregion

        #region 属性:Delete(string id)
        /// <summary>删除记录</summary>
        /// <param name="id">标识</param>
        public void Delete(string id)
        {
            this.provider.Delete(id);
        }
        #endregion

        //-------------------------------------------------------
        // 查询
        //-------------------------------------------------------

        #region 函数:FindOne(string objectType, string objectValue, string validationType)
        /// <summary>查询某条记录</summary>
        /// <param name="objectType">对象类型</param>
        /// <param name="objectValue">对象的值</param>
        /// <param name="validationType">验证方式</param>
        /// <returns>返回一个<see cref="VerificationCodeInfo"/>实例的详细信息</returns>
        public VerificationCodeInfo FindOne(string objectType, string objectValue, string validationType)
        {
            return this.provider.FindOne(objectType, objectValue, validationType);
        }
        #endregion

        #region 属性:FindAll(DataQuery query)
        /// <summary>查询所有相关记录</summary>
        /// <param name="query">数据查询参数</param>
        /// <returns>返回所有<see cref="VerificationCodeInfo"/>实例的详细信息</returns>
        public IList<VerificationCodeInfo> FindAll(DataQuery query)
        {
            return this.provider.FindAll(query);
        }
        #endregion

        //-------------------------------------------------------
        // 自定义功能
        //-------------------------------------------------------

        #region 属性:Query(int startIndex, int pageSize, DataQuery query, out int rowCount)
        /// <summary>分页函数</summary>
        /// <param name="startIndex">开始行索引数,由0开始统计</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="query">数据查询参数</param>
        /// <param name="rowCount">行数</param>
        /// <returns>返回一个<see cref="VerificationCodeInfo"/>列表实例</returns> 
        public IList<VerificationCodeInfo> GetPaging(int startIndex, int pageSize, DataQuery query, out int rowCount)
        {
            return this.provider.GetPaging(startIndex, pageSize, query, out rowCount);
        }
        #endregion

        #region 函数:IsExist(string id)
        /// <summary>查询是否存在相关的记录.</summary>
        /// <param name="id">标识</param>
        /// <returns>布尔值</returns>
        public bool IsExist(string id)
        {
            return this.provider.IsExist(id);
        }
        #endregion

        #region 函数:Create(string objectType, string objectValue, string validationType)
        /// <summary>创建新的验证码</summary>
        /// <param name="objectType">对象类型</param>
        /// <param name="objectValue">对象的值</param>
        /// <param name="validationType">验证方式</param>
        /// <returns>验证码对象</returns>
        public VerificationCodeInfo Create(string objectType, string objectValue, string validationType)
        {
            return this.Create(objectType, objectValue, validationType, string.Empty, 6);
        }
        #endregion

        #region 函数:Create(string objectType, string objectValue, string validationType, string ip)
        /// <summary>创建新的验证码</summary>
        /// <param name="objectType">对象类型</param>
        /// <param name="objectValue">对象的值</param>
        /// <param name="validationType">验证方式</param>
        /// <param name="ip">IP 地址</param>
        /// <returns>验证码对象</returns>
        public VerificationCodeInfo Create(string objectType, string objectValue, string validationType, string ip)
        {
            return this.Create(objectType, objectValue, validationType, ip, 6);
        }
        #endregion

        #region 函数:Create(string objectType, string objectValue, string validationType, string ip, int length)
        /// <summary>创建新的验证码</summary>
        /// <param name="objectType">对象类型</param>
        /// <param name="objectValue">对象的值</param>
        /// <param name="validationType">验证方式</param>
        /// <param name="length">验证码长度</param>
        /// <returns>验证码对象</returns>
        public VerificationCodeInfo Create(string objectType, string objectValue, string validationType, int length)
        {
            return this.Create(objectType, objectValue, validationType, string.Empty, length);
        }
        #endregion

        #region 函数:Create(string objectType, string objectValue, string validationType, string ip, int length)
        /// <summary>创建新的验证码</summary>
        /// <param name="objectType">对象类型</param>
        /// <param name="objectValue">对象的值</param>
        /// <param name="validationType">验证方式</param>
        /// <param name="ip">IP 地址</param>
        /// <param name="length">验证码长度</param>
        /// <returns>验证码对象</returns>
        public VerificationCodeInfo Create(string objectType, string objectValue, string validationType, string ip, int length)
        {
            VerificationCodeInfo param = new VerificationCodeInfo();

            // datetime(17) + randomtext(6) = 唯一标识长度(23) 
            param.Id = string.Concat(DateTime.Now.ToString("yyyyMMddHHmmssfff"), StringHelper.ToRandom("0123456789", 6));
            param.IP = ip;
            param.ObjectType = objectType;
            param.ObjectValue = objectValue;
            param.Code = StringHelper.ToRandom("0123456789", length);
            param.ValidationType = validationType;
            param.CreatedDate = DateTime.Now;

            return this.provider.Save(param);
        }
        #endregion

        #region 函数:Validate(string objectType, string objectValue, string validationType, string code)
        /// <summary>校验验证码</summary>
        /// <param name="objectType">对象类型</param>
        /// <param name="objectValue">对象的值</param>
        /// <param name="validationType">验证方式</param>
        /// <param name="code">验证码</param>
        /// <returns>布尔值</returns>
        public bool Validate(string objectType, string objectValue, string validationType, string code)
        {
            return this.Validate(objectType, objectValue, validationType, code, 0);
        }
        #endregion

        #region 函数:Validate(string objectType, string objectValue, string validationType, string code, int availableMinutes)
        /// <summary>校验验证码</summary>
        /// <param name="objectType">对象类型</param>
        /// <param name="objectValue">对象的值</param>
        /// <param name="validationType">验证方式</param>
        /// <param name="code">验证码</param>
        /// <param name="availableMinutes">有效分钟数</param>
        /// <returns>布尔值</returns>
        public bool Validate(string objectType, string objectValue, string validationType, string code, int availableMinutes)
        {
            VerificationCodeInfo param = this.FindOne(objectType, objectValue, validationType);

            // 判断对象是否存在
            if (param == null)
            {
                return false;
            }
            else
            {
                // 校验时间有效性
                if (availableMinutes > 0 && param.CreatedDate.AddMinutes(availableMinutes) < DateTime.Now)
                {
                    return false;
                }

                // 校验验证码
                return (param.Code == code);
            }
        }
        #endregion
    }
}
