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
    #endregion

    /// <summary>权限服务</summary>
    public class VerificationCodeService : IVerificationCodeService
    {
        /// <summary>日志记录器</summary>
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>配置</summary>
        private VerificationCodeConfiguration configuration = null;

        /// <summary>数据提供器</summary>
        private IVerificationCodeProvider provider = null;

        /// <summary>缓存存储</summary>
        private IDictionary<string, VerificationCodeInfo> dictionary = new Dictionary<string, VerificationCodeInfo>();

        private DateTime actionTime = DateTime.Now;

        #region 构造函数:VerificationCodeService()
        /// <summary>构造函数</summary>
        public VerificationCodeService()
        {
            this.configuration = VerificationCodeConfigurationView.Instance.Configuration;

            // 创建对象构建器(Spring.NET)
            string springObjectFile = this.configuration.Keys["SpringObjectFile"].Value;

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
            VerificationCodeInfo param = new VerificationCodeInfo();

            param.Id = "";
            param.ObjectType = "";
            param.ObjectValue = "";
            param.Code = "";
            param.ValidationType = "";
            param.CreateDate = DateTime.Now;

            return this.provider.Save(param);
        }
        #endregion
    }
}
