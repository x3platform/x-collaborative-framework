应用间的连接管理器

OAuth 2.0 验证方式介绍

http://passport.x3platform.com/api/connect.auth.authorize.aspx?clientId=52cf89ba-7db5-4e64-9c64-3c868b6e7a99&responseType=token&redirectUri=http://project.x3platform.com/sso.aspx

appKey + appKey

登录

responseType : code 
获取 authorizationCode

再通过 authorizationCode 获取 accessToken

responseType : token 
获取 accessToken
