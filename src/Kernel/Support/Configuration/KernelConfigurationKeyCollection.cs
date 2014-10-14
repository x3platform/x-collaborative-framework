// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================

using System;
using System.Collections;

namespace X3Platform.Configuration
{
    /// <summary>�ؼ������ü���</summary>
    [Serializable()]
    public class KernelConfigurationKeyCollection : CollectionBase
    {
        /// <summary>���ӹؼ���</summary>
        /// <param name="value">A kernel configuration key instance.</param>
        public virtual void Add(KernelConfigurationKey value)
        {
            this.InnerList.Add(value);
        }

        /// <summary>�Ƴ��ؼ���</summary>
        /// <param name="value">A kernel configuration key instance.</param>
        public virtual void Remove(KernelConfigurationKey value)
        {
            this.InnerList.Remove(value);
        }

        /// <summary>�����ؼ���</summary>
        /// <param name="name">��</param>
        public virtual bool ContainsKey(string name)
        {
            for (int i = 0; i < this.InnerList.Count; i++)
            {
                if (((KernelConfigurationKey)this.InnerList[i]).Name == name)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>���к�����</summary>
        public KernelConfigurationKey this[int index]
        {
            get { return (KernelConfigurationKey)this.InnerList[index]; }
            set { this.InnerList[index] = value; }
        }

        /// <summary>��������</summary>
        public KernelConfigurationKey this[string name]
        {
            get
            {
                for (int i = 0; i < this.InnerList.Count; i++)
                {
                    if (this.InnerList[i] == null)
                    {
                        this.InnerList.RemoveAt(i);
                        i--;
                    }
                    else
                    {
                        if (((KernelConfigurationKey)this.InnerList[i]).Name == name)
                        {
                            return (KernelConfigurationKey)this.InnerList[i];
                        }
                    }
                }

                return null;
            }
            set
            {
                for (int i = 0; i < this.InnerList.Count; i++)
                {
                    if (((KernelConfigurationKey)this.InnerList[i]).Name == name)
                    {
                        this.InnerList[i] = value;
                        break;
                    }
                }
            }
        }
    }
}
