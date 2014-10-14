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

//
// ������¼
//
// == 2010-01-01 == 
//
// �½�
//

using System;

namespace X3Platform.CacheBuffer
{
    /// <summary>���������쳣</summary>
    [Serializable]
    public class CacheBufferException : Exception
    {
        public CacheBufferException(string message)
            : base(message)
        {
        }

        public CacheBufferException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}