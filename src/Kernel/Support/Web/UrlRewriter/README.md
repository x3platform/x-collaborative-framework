
HttpModule 和 HttpHandler 两种重写方式任选一种

<httpModules>
	<add name="RewriterModule" type="X3Platform.UrlRewriter.WindowsAuthorizationUrlRewriterModule,X3Platform" />
</httpModules>

<httpHandlers>
	<add verb="*" path="*.aspx" type="X3Platform.UrlRewriter.RewriterFactoryHandler,X3Platform" />
</httpHandlers>