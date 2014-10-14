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

namespace X3Platform.Web.UrlRewriter.Configuration
{
    /// <summary>
    /// The RewriterRuleCollection models a set of RewriterRules in the Web.config file.
    /// </summary>
    /// <remarks>
    /// The RewriterRuleCollection is expressed in XML as:
    /// <code>
    /// &lt;RewriterRule&gt;
    ///   &lt;LookFor&gt;<i>pattern to search for</i>&lt;/LookFor&gt;
    ///   &lt;SendTo&gt;<i>string to redirect to</i>&lt;/LookFor&gt;
    /// &lt;RewriterRule&gt;
    /// &lt;RewriterRule&gt;
    ///   &lt;LookFor&gt;<i>pattern to search for</i>&lt;/LookFor&gt;
    ///   &lt;SendTo&gt;<i>string to redirect to</i>&lt;/LookFor&gt;
    /// &lt;RewriterRule&gt;
    /// ...
    /// &lt;RewriterRule&gt;
    ///   &lt;LookFor&gt;<i>pattern to search for</i>&lt;/LookFor&gt;
    ///   &lt;SendTo&gt;<i>string to redirect to</i>&lt;/LookFor&gt;
    /// &lt;RewriterRule&gt;
    /// </code>
    /// </remarks>
    [Serializable()]
    public class RewriterRuleCollection : CollectionBase
    {
        /// <summary>
        /// Adds a new RewriterRule to the collection.
        /// </summary>
        /// <param name="r">A RewriterRule instance.</param>
        public virtual void Add(RewriterRule r)
        {
            this.InnerList.Add(r);
        }

        /// <summary>
        /// Gets or sets a RewriterRule at a specified ordinal index.
        /// </summary>
        public RewriterRule this[int index]
        {
            get { return (RewriterRule)this.InnerList[index]; }
            set { this.InnerList[index] = value; }
        }
    }
}
