namespace X3Platform.Storages.Strategies
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Security.Cryptography;
    #endregion

    /// <summary>根据哈希值规则存储的存储策略</summary>
    public sealed class HashCodeStorageStrategy : AbstractStorageStrategy
    {
        /// <summary></summary>
        /// <param name="storageSchema"></param>
        public HashCodeStorageStrategy(IStorageSchema storageSchema)
        {
            this.storageSchema = storageSchema;
        }

        /// <summary></summary>
        /// <param name="storageNodeType"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public override IStorageNode GetStorageNode(string storageNodeType, string index)
        {
            this.LazyLoadStorageNodes();

            int hashCode = GetHashCode(index);

            IList<IStorageNode> list = new List<IStorageNode>();

            foreach (IStorageNode storageNode in this.storageNodes)
            {
                if (storageNode.Type == storageNodeType)
                {
                    list.Add(storageNode);
                }
            }

            if (list.Count == 0)
            {
                return this.GetStorageNode();
            }
            else
            {
                return list[hashCode % list.Count];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="storageNodeType"></param>
        /// <param name="indexs"></param>
        /// <returns></returns>
        public override IStorageNode GetStorageNode(string storageNodeType, string[] indexs)
        {
            this.LazyLoadStorageNodes();

            int hashCode = GetHashCode(indexs);

            IList<IStorageNode> list = new List<IStorageNode>();

            foreach (IStorageNode storageNode in this.storageNodes)
            {
                if (storageNode.Type == storageNodeType)
                {
                    list.Add(storageNode);
                }
            }

            if (list.Count == 0)
            {
                return this.GetStorageNode();
            }
            else
            {
                return list[hashCode % list.Count];
            }
        }

        /// <summary></summary>
        /// <param name="indexs"></param>
        /// <returns></returns>
        private int GetHashCode(string[] indexs)
        {
            if (indexs.Length == 0)
            {
                return 1;
            }
            else
            {
                int hash = GetHashCode(indexs[0]);

                for (int i = 1; i < indexs.Length; i++)
                {
                    hash += GetHashCode(indexs[i]);
                }

                // 将负数转化正数
                return hash & 0x7FFFFFFF;
            }
        }

        /// <summary></summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private int GetHashCode(string index)
        {
            // 由于对象默认的GetHashCode()函数可能会因为平台或版本的不同产生不同的结果
            // 因此制作个简化的模拟函数生成一个整数共对象取模操作

            int hash = 0;

            byte[] buffer = Encoding.UTF8.GetBytes(index);

            // 将字节相加获得一个整数
            for (int i = 0; i < buffer.Length; i++)
            {
                // (hash >> 5) => hasH * (2^5) => hash * 32
                // 位运算的速度比较快 
                hash = (hash >> 5) + buffer[i];
            }

            // 将负数转化正数
            return hash & 0x7FFFFFFF;
        }
    }
}
