namespace X3Platform.Plugins.Favorite.Customize
{
    using System;
    using System.Collections.Generic;

    using X3Platform.Logging;
    using X3Platform.Plugins.Favorite.IBLL;
    using X3Platform.Plugins.Favorite.Configuration;
    using X3Platform.Spring;
    using X3Platform.Util;
    using X3Platform.Velocity;
    using X3Platform.Web.Customize;
    using X3Platform.Plugins.Favorite.Model;

    /// <summary>收藏夹部件</summary>
    public sealed class FavoriteWidget : IWidget
    {
        public void Load(string configuration)
        {

        }

        public string ParseHtml()
        {
            string widgetRuntimeId = StringHelper.ToGuid();

            VelocityContext context = new VelocityContext();

            context.Put("widgetRuntimeId", widgetRuntimeId);

            string whereClause = string.Format(" ( AccountId=#### OR AccountId=##{0}## ) ", KernelContext.Current.User.Id);

            int length = 6;

            IList<FavoriteInfo> list = FavoriteContext.Instance.FavoriteService.FindAll(whereClause, length);

            context.Put("list", list);

            return VelocityManager.Instance.Merge(context, "web/customize/widgets/favorite.vm");
        }
    }
}
