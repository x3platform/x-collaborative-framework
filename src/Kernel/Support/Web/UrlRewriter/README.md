
HttpModule �� HttpHandler ������д��ʽ��ѡһ��

<httpModules>
	<add name="RewriterModule" type="X3Platform.UrlRewriter.WindowsAuthorizationUrlRewriterModule,X3Platform" />
</httpModules>

<httpHandlers>
	<add verb="*" path="*.aspx" type="X3Platform.UrlRewriter.RewriterFactoryHandler,X3Platform" />
</httpHandlers>