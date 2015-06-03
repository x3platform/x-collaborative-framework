// -*- ecoding=utf-8 -*-
// Name     : x-client-api 
// Version  : 1.0.0 
// Author   : ruanyu@live.com
// Date     : 2015-05-25
(function(global, factory) 
{
    if (typeof module === "object" && typeof module.exports === "object") 
    {
        module.exports = global.document ?
        factory(global, true) :
        function(w) 
        {
            if (!w.document) { throw new Error("requires a window with a document"); }

            return factory(w);
        };
    } 
    else 
    {
        factory(global);
    }
} (typeof window !== "undefined" ? window : this, function(window, noGlobal) {    /**
    * 模板引擎
    * @name    template
    * @param   {String}            模板选项
    * @return  {String, Function}  渲染好的HTML字符串或者渲染方法
    */
    var template = function(options)
    {
        // content, data
        if (options.fileName)
        {
            return typeof options.context === 'string' ?
            compile(options.data, { fileName: options.fileName }) :
            renderFile(options.fileName, options.data);
        }
        else 
        {
            return compile(options.content, x.ext(options, { fileName: 'tmp-' + x.randomText.create(8) }))(options.data);
            // fileName, options.data
        } 
    };
    
    template.version = '3.0.0';
    
    /**
    * 设置全局配置
    * @name    template.config
    * @param   {String}    名称
    * @param   {Any}       值
    */
    template.config = function(name, value)
    {
        defaults[name] = value;
    };
    
    var defaults = template.defaults = {
        openTag: '{{',    // 逻辑语法开始标签
        closeTag: '}}',   // 逻辑语法结束标签
        escape: false,     // 是否编码输出变量的 HTML 字符
        cache: true,      // 是否开启缓存（依赖 options 的 fileName 字段）
        compress: false,  // 是否压缩输出
        parser: null      // 自定义语法格式器 @see: template-syntax.js
    };
    
    // 缓存
    var cacheStore = template.cache = {};
    
    /**
    * 渲染模板
    * @name    template.render
    * @param   {String}    模板
    * @param   {Object}    数据
    * @return  {String}    渲染好的字符串
    */
    template.render = function(source, options)
    {
        return compile(source, options);
    };
    
    /**
    * 渲染模板(根据模板名)
    * @name    template.render
    * @param   {String}    模板名
    * @param   {Object}    数据
    * @return  {String}    渲染好的字符串
    */
    var renderFile = template.renderFile = function(fileName, data)
    {
        var fn = template.get(fileName) || showDebugInfo({
            fileName: fileName,
            name: 'Render Error',
            message: 'Template not found'
        });
    
        return data ? fn(data) : fn;
    };
    
    /**
    * 获取编译缓存（可由外部重写此方法）
    * @param   {String}    模板名
    * @param   {Function}  编译好的函数
    */
    template.get = function(fileName)
    {
        var cache;
    
        if (cacheStore[fileName])
        {
            // 使用内存缓存
            cache = cacheStore[fileName];
        }
        else if (typeof document === 'object')
        {
            // 加载模板并编译
            var elem = x.query(fileName);
    
            if (elem)
            {
                var source = (elem.value || elem.innerHTML).replace(/^\s*|\s*$/g, '');
    
                source = template.xml.parse(x.string.trim(source));
    
                cache = compile(source, { fileName: fileName });
            }
        }
    
        return cache;
    };
    
    //var escapeMap = {
    //    "<": "&#60;",
    //    ">": "&#62;",
    //    '"': "&#34;",
    //    "'": "&#39;",
    //    "&": "&#38;"
    //};
    
    
    //var escapeFn = function(s)
    //{
    //    return escapeMap[s];
    //};
    
    //var escapeHTML = function(content)
    //{
    //    return toString(content)
    //    .replace(/&(?![\w#]+;)|[<>"']/g, escapeFn);
    //};
    
    //var isArray = Array.isArray || function(obj)
    //{
    //    return ({}).toString.call(obj) === '[object Array]';
    //};
    
    
    //var each = function(data, callback)
    //{
    //    var i, len;
    //    if (x.isArray(data))
    //    {
    //        for (i = 0, len = data.length; i < len; i++)
    //        {
    //            callback.call(data, data[i], i, data);
    //        }
    //    } 
    //    else
    //    {
    //        for (i in data)
    //        {
    //            callback.call(data, data[i], i);
    //        }
    //    }
    //};
    
    var utils = template.utils = {
    
        $helpers: {},
    
        $include: renderFile,
    
        // $string: toString,
        $string: x.string.stringify,
    
        $escape: x.encoding.html.encode,
    
        $foreach: x.each
    };
    
    /**
    * 添加模板辅助方法
    * @name    template.helper
    * @param   {String}    名称
    * @param   {Function}  方法
    */
    template.helper = function(name, helper)
    {
        helpers[name] = helper;
    };
    
    var helpers = template.helpers = utils.$helpers;
    
    /**
    * 模板错误事件（可由外部重写此方法）
    * @name    template.onerror
    * @event
    */
    template.onerror = function(ex)
    {
        var message = 'Template Error\n\n';
    
        for (var name in ex)
        {
            message += '<' + name + '>\n' + ex[name] + '\n\n';
        }
    
        x.debug.error(message);
    };
    
    
    // 模板调试器
    var showDebugInfo = function(ex)
    {
        template.onerror(ex);
    
        return function()
        {
            return '{Template Error}';
        };
    };
    template.xml = {
        /**
        * 解析 xml 字符串
        */
        parse: function(xml)
        {
            var buffer = template.xml.syntax(xml);
    
            return buffer.join('');
        },
        syntax: function(xml)
        {
            var buffer = [], originalBuffer = [];
    
            buffer = template.xml.array(xml);
    
            for (var i = 0; i < buffer.length; i++)
            {
                if (buffer[i].indexOf('</') == -1 && buffer[i].indexOf('<') < buffer[i].indexOf('>'))
                {
                    // open tag
    
                    var openTag = buffer[i];
    
                    var closeTag = template.xml.closeTag(openTag);
    
                    // console.log(openTag);
    
                    var code = openTag.match(/x-(if|else|foreach)\=\"([a-zA-Z0-9-_\=\/\' ]+)\"/);
    
                    if (x.isArray(code) && code.length == 3)
                    {
                        // 判断是否存在语法
    
                        openTag = openTag.replace(code[0], '');
    
                        if (code[1] == "foreach")
                        {
                            buffer[i] = openTag + '{{foreach ' + code[2] + '}}';
    
                            buffer = template.xml.injectionCode(buffer, i, openTag, closeTag, code[1]);
                        }
                        else if (code[1] == "if")
                        {
                            buffer[i] = '{{if ' + code[2] + '}}' + openTag;
    
                            buffer = template.xml.injectionCode(buffer, i, openTag, closeTag, code[1]);
                        }
                        else if (code[1] == "else")
                        {
                            buffer[i] = '{{else}}' + openTag;
                        }
                    }
                }
            }
    
            buffer = template.xml.array(buffer.join(''));
    
            return buffer;
        },
    
        /**
        * 将html字符转成数组
        */
        array: function()
        {
            var buffer = [], originalBuffer = [];
    
            // 分词: <
            originalBuffer = arguments[0].split(/</g);
    
            x.each(originalBuffer, function(index, node)
            {
                if (x.string.trim(node) != '' && index > 0)
                {
                    buffer[buffer.length] = '<' + node;
                }
                else
                {
                    buffer[buffer.length] = node;
                }
            });
    
            // 分词: >
            originalBuffer = buffer;
    
            buffer = [];
    
            for (var i = 0; i < originalBuffer.length; i++)
            {
                var text = x.string.trim(originalBuffer[i]);
    
                if (text != '')
                {
                    if (text.indexOf('>') == -1)
                    {
                        buffer[buffer.length] = text;
                        continue;
                    }
    
                    var nodes = text.split(/>/g);
    
                    for (var j = 0; j < nodes.length; j++)
                    {
                        if (nodes[j] != '')
                        {
                            if (nodes[j].indexOf('<') == 0)
                            {
                                buffer[buffer.length] = nodes[j] + '>';
                            }
                            else
                            {
                                buffer[buffer.length] = nodes[j];
                            }
                        }
                    }
                }
            }
    
            // 分词: {{
            originalBuffer = buffer;
    
            buffer = [];
    
            for (var i = 0; i < originalBuffer.length; i++)
            {
                var text = x.string.trim(originalBuffer[i]);
    
                if (text != '')
                {
                    if (text.indexOf('{{') == -1)
                    {
                        buffer[buffer.length] = text;
                        continue;
                    }
    
                    var nodes = text.split(/{{/g);
    
                    for (var j = 0; j < nodes.length; j++)
                    {
                        if (nodes[j] != '')
                        {
                            if (j > 0)
                            {
                                buffer[buffer.length] = '{{' + nodes[j];
                            }
                            else
                            {
                                buffer[buffer.length] = nodes[j];
                            }
                        }
                    }
                }
            }
    
            // 分词: }}
            originalBuffer = buffer;
    
            buffer = [];
    
            for (var i = 0; i < originalBuffer.length; i++)
            {
                var text = x.string.trim(originalBuffer[i]);
    
                if (text != '')
                {
                    if (text.indexOf('}}') == -1)
                    {
                        buffer[buffer.length] = text;
                        continue;
                    }
    
                    var nodes = text.split(/}}/g);
    
                    for (var j = 0; j < nodes.length; j++)
                    {
                        if (nodes[j] != '')
                        {
                            if (nodes[j].indexOf('{{') > -1)
                            {
                                buffer[buffer.length] = nodes[j] + '}}';
                            }
                            else
                            {
                                buffer[buffer.length] = nodes[j];
                            }
                        }
                    }
                }
            }
    
            // 格式化标签
            for (var i = 0; i < buffer.length; i++)
            {
                var isTag = false;
    
                if (buffer[i].indexOf('<') < buffer[i].indexOf(' />'))
                {
                    // single tag
                    isTag = true;
                }
                else if (buffer[i].indexOf('</') == -1 && buffer[i].indexOf('<') < buffer[i].indexOf('>'))
                {
                    // open tag
                    isTag = true;
                }
                else if (buffer[i].indexOf('</') < buffer[i].indexOf('>'))
                {
                    // close tag
                    isTag = true;
                }
    
                if (isTag)
                {
                    // 统一格式 小写, 去除两侧空白
                    buffer[i] = buffer[i].toLowerCase().trim();
                }
            }
    
            return buffer;
        },
        // isTag: function() { },
        /**
        * 获取结束标记
        */
        closeTag: function(openTag)
        {
            var tag = openTag.match(/(<?\w+)|(<\/?\w+)\s/);
    
            if (tag)
            {
                if (!tag[0].match(/img|input/))
                {
                    return '</' + tag[0].replace('<', '') + '>';
                }
            }
    
            return null;
        },
    
        injectionCode: function(buffer, beginIndex, openTag, closeTag, expression)
        {    
            var deep = 0, endIndex = beginIndex;
    
            // 格式化开始标签
            openTag = openTag.match(/<\w+/)[0];
    
            for (var i = beginIndex + 1; i < buffer.length; i++)
            {
                if (buffer[i] == closeTag)
                {
                    if (deep > 0)
                    {
                        deep--;
                        continue;
                    }
    
                    if (expression == 'foreach')
                    {
                        buffer[i] = '{{/' + expression + '}}' + buffer[i];
                    }
                    else if (expression == 'if')
                    {
                        buffer[i] = buffer[i] + '{{/' + expression + '}}';
                    }
                    break;
                }
                else if (buffer[i].indexOf(openTag + '>') > -1 || buffer[i].indexOf(openTag + ' ') > -1)
                {
                    deep++;
                }
            }
    
            return buffer;
        }
    }/**
    * 编译模板
    * 2012-6-6 @TooBug: define 方法名改为 compile，与 Node Express 保持一致
    * @name    template.compile
    * @param   {String}    模板字符串
    * @param   {Object}    编译选项
    *
    *      - openTag       {String}
    *      - closeTag      {String}
    *      - fileName      {String}
    *      - escape        {Boolean}
    *      - compress      {Boolean}
    *      - debug         {Boolean}
    *      - cache         {Boolean}
    *      - parser        {Function}
    *
    * @return  {Function}  渲染方法
    */
    var compile = template.compile = function(source, options)
    {
        options = x.ext(template.defaults, options || {});
    
        var fileName = options.fileName;
    
        try
        {
            var Render = compiler(source, options);
        }
        catch (ex)
        {
            ex.fileName = fileName || 'anonymous';
            ex.name = 'Syntax Error';
    
            return showDebugInfo(ex);
        }
    
        // 对编译结果进行一次包装
        function render(data)
        {
            try
            {
                return new Render(data, fileName) + '';
    
            }
            catch (ex)
            {
                // 运行时出错后自动开启调试模式重新编译
                if (!options.debug)
                {
                    options.debug = true;
                    return compile(source, options)(data);
                }
    
                return showDebugInfo(ex)();
            }
        }
    
        render.prototype = Render.prototype;
        render.toString = function()
        {
            return Render.toString();
        };
    
        if (fileName && options.cache)
        {
            cacheStore[fileName] = render;
        }
    
        return render;
    };
    
    // 静态分析模板变量
    var KEYWORDS =
    // 关键字
        'break,case,catch,continue,debugger,default,delete,do,else,false'
        + ',finally,for,function,if,in,instanceof,new,null,return,switch,this'
        + ',throw,true,try,typeof,var,void,while,with'
    
    // 保留字
        + ',abstract,boolean,byte,char,class,const,double,enum,export,extends'
        + ',final,float,goto,implements,import,int,interface,long,native'
        + ',package,private,protected,public,short,static,super,synchronized'
        + ',throws,transient,volatile'
    
    // ECMA 5 - use strict
        + ',arguments,let,yield'
    
        + ',undefined';
    
    var REMOVE_RE = /\/\*[\w\W]*?\*\/|\/\/[^\n]*\n|\/\/[^\n]*$|"(?:[^"\\]|\\[\w\W])*"|'(?:[^'\\]|\\[\w\W])*'|\s*\.\s*[$\w\.]+/g;
    var SPLIT_RE = /[^\w$]+/g;
    var KEYWORDS_RE = new RegExp(["\\b" + KEYWORDS.replace(/,/g, '\\b|\\b') + "\\b"].join('|'), 'g');
    var NUMBER_RE = /^\d[^,]*|,\d[^,]*/g;
    var BOUNDARY_RE = /^,+|,+$/g;
    
    // 获取变量
    function getVariable(code)
    {
        return code
            .replace(REMOVE_RE, '')
            .replace(SPLIT_RE, ',')
            .replace(KEYWORDS_RE, '')
            .replace(NUMBER_RE, '')
            .replace(BOUNDARY_RE, '')
            .split(/^$|,+/);
    };
    
    // 字符串转义
    function stringify(code)
    {
        return "'" + code
        // 单引号与反斜杠转义
            .replace(/('|\\)/g, '\\$1')
        // 换行符转义(windows + linux)
            .replace(/\r/g, '\\r')
            .replace(/\n/g, '\\n') + "'";
    }
    
    function compiler(source, options)
    {
        var debug = options.debug;
        var openTag = options.openTag;
        var closeTag = options.closeTag;
        var parser = options.parser;
        var compress = options.compress;
        var escape = options.escape;
    
        var line = 1;
        var uniq = { $data: 1, $fileName: 1, $utils: 1, $helpers: 1, $out: 1, $line: 1 };
    
        // 字符串数组拼接
        var isNewEngine = ''.trim; // '__proto__' in {}
        var replaces = isNewEngine
        ? ["$out='';", "$out+=", ";", "$out"]
        : ["$out=[];", "$out.push(", ");", "$out.join('')"];
    
        var concat = isNewEngine
            ? "$out+=text;return $out;"
            : "$out.push(text);";
    
        var print = "function(){"
        + "var text=''.concat.apply('',arguments);"
        + concat
        + "}";
    
        var include = "function(fileName,data){"
        + "data=data||$data;"
        + "var text=$utils.$include(fileName,data,$fileName);"
        + concat
        + "}";
    
        var headerCode = "'use strict';"
        + "var $utils=this,$helpers=$utils.$helpers,"
        + (debug ? "$line=0," : "");
    
        var mainCode = replaces[0];
    
        var footerCode = "return new String(" + replaces[3] + ");"
    
        // Html与逻辑语法分离
        x.each(source.split(openTag), function(index, code)
        {
            code = code.split(closeTag);
    
            var $0 = code[0];
            var $1 = code[1];
    
            if (code.length === 1)
            {
                // code: [html]
            
                mainCode += html($0);
            }
            else
            {
                // code: [logic, html]
             
                mainCode += logic($0);
    
                if ($1)
                {
                    mainCode += html($1);
                }
            }
        });
    
        // 输出最后代码
        var code = headerCode + mainCode + footerCode;
    
        // 调试语句
        if (debug)
        {
            code = "try{" + code + "}catch(ex){"
            + "throw {"
            + "fileName:$fileName,"
            + "name:'Render Error',"
            + "message:ex.message,"
            + "line:$line,"
            + "source:" + stringify(source)
            + ".split(/\\n/)[$line-1].replace(/^\\s+/,'')"
            + "};"
            + "}";
        }
    
        try
        {
            var Render = new Function("$data", "$fileName", code);
            Render.prototype = utils;
    
            return Render;
        }
        catch (ex)
        {
            ex.temp = "function anonymous($data,$fileName) {" + code + "}";
            throw ex;
        }
            /**
        * 处理 HTML 语句
        */
        function html(code)
        {
            // 记录行号
            line += code.split(/\n/).length - 1;
    
            // 压缩多余空白与注释
            if (compress)
            {
                code = code.replace(/\s+/g, ' ').replace(/<!--.*?-->/g, '');
            }
    
            if (code)
            {
                code = replaces[1] + stringify(code) + replaces[2] + "\n";
            }
    
            return code;
        }
    
        /**
        * 处理逻辑语句
        */
        function logic(code)
        {
            var thisLine = line;
    
            if (parser)
            {
                // 语法转换插件钩子
                code = parser(code, options);
            }
            else if (debug)
            {
                // 记录行号
                code = code.replace(/\n/g, function()
                {
                    line++;
                    return "$line=" + line + ";";
                });
            }
    
            // 输出语句. 编码: <%=value%> 不编码:<%=#value%>
            // <%=#value%> 等同 v2.0.3 之前的 <%==value%>
            if (code.indexOf('=') === 0)
            {
                var escapeSyntax = escape && !/^=[=#]/.test(code);
    
                code = code.replace(/^=[=#]?|[\s;]*$/g, '');
    
                // 对内容编码
                if (escapeSyntax)
                {
                    var name = code.replace(/\s*\([^\)]+\)/, '');
    
                    // 排除 utils.* | include | print
    
                    if (!utils[name] && !/^(include|print)$/.test(name))
                    {
                        code = "$escape(" + code + ")";
                    }
    
                    // 不编码
                }
                else
                {
                    code = "$string(" + code + ")";
                }
    
                code = replaces[1] + code + replaces[2];
            }
    
            if (debug)
            {
                code = "$line=" + thisLine + ";" + code;
            }
    
            // 提取模板中的变量名
            x.each(getVariable(code), function(index, name)
            {
                // name 值可能为空，在安卓低版本浏览器下
                if (!name || uniq[name])
                {
                    return;
                }
    
                var value;
    
                // 声明模板变量
                // 赋值优先级:
                // [include, print] > utils > helpers > data
                if (name === 'print')
                {
                    value = print;
                }
                else if (name === 'include')
                {
                    value = include;
                }
                else if (utils[name])
                {
                    value = "$utils." + name;
                }
                else if (helpers[name])
                {
                    value = "$helpers." + name;
                }
                else
                {
    
                    value = "$data." + name;
                }
    
                headerCode += name + "=" + value + ",";
                uniq[name] = true;
    
            });
    
            return code + "\n";
        }
    };
    
    
    
    // 定义模板引擎的语法
    
    var filtered = function(js, filter)
    {
        var parts = filter.split(':');
        var name = parts.shift(); // shift 方法可移除数组中的第一个元素并返回该元素。
        var args = parts.join(':') || '';
    
        if (args)
        {
            args = ', ' + args;
        }
    
        return '$helpers.' + name + '(' + js + args + ')';
    }
    
    defaults.parser = function(code, options)
    {
        // 去除左边的空格
        code = code.replace(/^\s/, '');
    
        var split = code.split(' ');
        var key = split.shift();
        var args = split.join(' ');
    
        switch (key)
        {
            case 'if':
    
                code = 'if(' + args + '){';
                break;
    
            case 'else':
    
                if (split.shift() === 'if')
                {
                    split = ' if(' + split.join(' ') + ')';
                } else
                {
                    split = '';
                }
    
                code = '}else' + split + '{';
                break;
    
            case '/if':
    
                code = '}';
                break;
    
            case 'foreach':
    
                if (split.length == 3 && split[1] == 'in')
                {
                    var object = split[2] || '$data';
                    var value = split[0] || '$value';
    
                    var param = '$index,' + value;
    
                    code = '$foreach(' + object + ',function(' + param + '){';
                }
                else if (split.length == 4 && split[2] == 'in')
                {
                    var object = split[3] || '$data';
                    var value = split[1] || '$value';
                    var index = split[0] || '$index';
    
                    var param = index + ',' + value;
    
                    code = '$foreach(' + object + ',function(' + param + '){';
                }
                else
                {
                    var object = split[3] || '$data';
                    var adverb = split[2] || 'in';
                    var value = split[1] || '$value';
                    var index = split[0] || '$index';
    
                    var param = index + ',' + value;
    
                    if (adverb !== 'in')
                    {
                        object = '[]';
                    }
    
                    code = '$foreach(' + object + ',function(' + param + '){';
                }
                break;
    
            case '/foreach':
    
                code = '});';
                break;
    
            case 'echo':
    
                code = 'print(' + args + ');';
                break;
    
            case 'print':
            case 'include':
    
                code = key + '(' + split.join(',') + ');';
                break;
    
            default:
    
                // 过滤器（辅助方法）
                // {{value | filterA:'abcd' | filterB}}
                // >>> $helpers.filterB($helpers.filterA(value, 'abcd'))
                if (args.indexOf('|') !== -1)
                {
                    var escape = options.escape;
    
                    // {{#value | link}}
                    if (code.indexOf('#') === 0)
                    {
                        code = code.substr(1);
                        escape = false;
                    }
    
                    var i = 0;
                    var array = code.split('|');
                    var len = array.length;
                    var pre = escape ? '$escape' : '$string';
                    var val = pre + '(' + array[i++] + ')';
    
                    for (; i < len; i++)
                    {
                        val = filtered(val, array[i]);
                    }
    
                    code = '=#' + val;
    
                    // 即将弃用 {{helperName value}}
                }
                else if (template.helpers[key])
                {
                    code = '=#' + key + '(' + split.join(',') + ');';
    
                    // 内容直接输出 {{value}}
                }
                else
                {
                    code = '=' + code;
                }
    
                break;
        }
    
        return code;
    };
    
    
    
    // 输出对象
        
    if (typeof define === "function" && define.amd ) {
	    // RequireJS && SeaJS
        define(function() { return template; });
    } else if (typeof module !== "undefined" && module.exports ) {
	    // Common-JS && NodeJS
        module.exports = template;
    } else {
        // Browser
        window.x.template = template;
    }

    return template;
}));