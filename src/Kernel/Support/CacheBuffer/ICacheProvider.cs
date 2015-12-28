using System;
using System.Collections.Generic;

namespace X3Platform.CacheBuffer
{
    public interface ICacheProvider
    {
        #region 属性:索引
        /// <summary>索引</summary>
        object this[string key] { get; set; }
        #endregion

        #region 函数:Contains(string key)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Contains(string key);
        #endregion

        #region 函数:Read(string key)
        ///<summary>读取缓存记录</summary>
        ///<param name="key">标识</param>
        ///<returns>返回缓存对象的详细信息</returns>
        object Read(string key);
        #endregion

        #region 函数:Write(string key, object value)
        ///<summary>写入缓存记录</summary>
        ///<param name="key">标志</param>
        ///<param name="value">RuanYu.CacheBuffer.Model.TaskInfo Id号</param>
        void Write(string key, object value);
        #endregion

        #region 函数:Write(string key, object value, int minutes)
        void Write(string key, object value, int minutes);
        #endregion

        #region 函数:Delete(string key)
        ///<summary>查询某条记录</summary>
        ///<param name="Id">RuanYu.CacheBuffer.Model.TaskInfo Id号</param>
        ///<returns>返回一个 RuanYu.CacheBuffer.Model.TaskInfo 实例的详细信息</returns>
        void Delete(string key);
        #endregion

        #region 函数:Clear()
        ///<summary>清空数据</summary>
        void Clear();
        #endregion
    }
}