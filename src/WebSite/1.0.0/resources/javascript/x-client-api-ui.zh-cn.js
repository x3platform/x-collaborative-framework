// -*- ecoding=utf-8 -*-

/**
* @namespace animation
* @memberof x.ui
* @description 动画
*/
x.ui.animation = {
    /**
    * 动画剪辑
    * @description 动画剪辑
    * @class Clip
    * @constructor newClip
    * @memberof x.ui.animation
    * @param {object} options 选项
    */
    newClip: function(options)
    {
        // 容器对象,滑动对象,切换数量
        var clip = {
            //
            container: null,
            slider: null,
            // 场景
            scenes: [],
            count: 0,
            // 定时器
            timer: null,

            index: 0, // 当前索引
            _target: 0, // 目标值
            // tween 参数
            _t: 0,
            _b: 0,
            _c: 0,

            // 设置默认属性
            bindOptions: function(options)
            {
                this.options = {
                    vertical: true,                         // 是否垂直方向（方向不能改）
                    hange: 0,                               // 改变量
                    duration: 50,                           // 滑动持续时间
                    time: 5,                                // 滑动延时
                    auto: false,                            // 是否自动
                    cpause: 2000,                           // 停顿时间(auto为true时有效)
                    onStart: function() { },                // 开始转换时执行
                    onFinish: function() { },               // 完成转换时执行
                    tween: x.ui.animation.tween.quart.easeOut  // tween 算法
                };

                x.ext(this.options, options || {});
            },

            // 开始切换
            run: function(index)
            {
                // 修正index
                index == undefined && (index = this.index);
                index < 0 && (index = this.count - 1) || index >= this.count && (index = 0);

                // 设置参数
                this.target = -Math.abs(this.change) * (this.index = index);
                this._t = 0;
                this._b = parseInt(x.css.style(this.slider)[this.options.vertical ? "top" : "left"]);
                this._c = this.target - this._b;

                this.onStart();
                this.move();
            },

            // 移动
            move: function()
            {
                clearTimeout(this.timer);

                if (this._c && this._t < this.duration)
                {
                    // 未到达目标继续移动否则进行下一次滑动
                    this.moveTo(Math.round(this.tween(this._t++, this._b, this._c, this.duration)));
                    this.timer = setTimeout(x.invoke(this, this.move), this.time);
                }
                else
                {
                    this.moveTo(this.target);
                    this.auto && (this.timer = setTimeout(x.invoke(this, this.next), this.pause));
                }
            },

            /*#region 函数:moveTo()*/
            /**
            * 移动到
            * @method moveTo
            * @memberof x.ui.animation.newClip#
            */
            moveTo: function(i)
            {
                this.slider.style[this._css] = i + "px";
            },
            /*#endregion*/

            /*#region 函数:previous()*/
            /**
            * 上一个
            * @method previous
            * @memberof x.ui.animation.newClip#
            */
            previous: function()
            {
                this.run(--this.index);
            },
            /*#endregion*/

            /*#region 函数:next()*/
            /**
            * 下一个
            * @method next
            * @memberof x.ui.animation.newClip#
            */
            next: function()
            {
                this.run(++this.index);
            },
            /*#endregion*/

            /*#region 函数:stop()*/
            /**
            * 停止
            * @method stop
            * @memberof x.ui.animation.newClip#
            */
            stop: function()
            {
                clearTimeout(this.timer);
                this.moveTo(this.target);
            },
            /*#endregion*/

            bindScenes: function()
            {
                var that = this;

                $($(this.container).find('.x-ui-clip-scene')).each(function(index, node)
                {
                    that.scenes[index] = node;
                });

                this.count = this.scenes.length;
            },

            /*#region 函数:getScene(index)*/
            /**
            * 添加场景
            */
            getScene: function(index)
            {
                var sceneCount = this.scenes.length;

                index = x.isUndefined(index, 0);

                if (index < 0) { index = 0; }

                if (index > sceneCount - 1) { index = sceneCount - 1; }

                return this.scenes[index];
            },
            /*#endregion*/

            /*#region 函数:addScene(index)*/
            /**
            * 添加场景
            */
            addScene: function(index)
            {
                var sceneCount = this.scenes.length;

                if (sceneCount == 0) { x.debug.error('必须创建默认场景。'); }

                index = x.isUndefined(index, sceneCount - 1);

                var scene = this.getScene(index);

                // 横向
                $(scene).parent().parent().append($('<td><div class="x-ui-clip-scene" ></div></td>'));
                this.count++;

                // 重新设置容器宽度
                $(this.container).find('.x-ui-clip-scene').css({ width: this.width, height: this.height });

                // 重新绑定场景信息
                this.bindScenes();
            },
            /*#endregion*/

            /*#region 函数:addScene()*/
            /**
            * 添加场景
            */
            removeScene: function(index)
            {
                var sceneCount = this.scenes.length;

                if (sceneCount == 0) { x.debug.error('必须创建默认场景。'); }

                if (scenes.size() == 2) { x.debug.error('必须创建默认场景.'); }

                index = x.isUndefined(index, sceneCount - 1);

                var scene = this.getScene(index);

                if (scenes.size() > 0)
                {
                    // 横向
                    $(scenes[scenes.size() - 1]).parent().parent().append($('<td><div class="x-ui-clip-scene" ></div></td>'));
                    this.count++;
                }

                // 重新设置容器宽度
                $(this.container).find('.x-ui-clip-scene').css({ width: this.width, height: this.height });

                // 重新绑定场景信息
                this.bindScenes();
            },
            /*#endregion*/

            create: function(options)
            {
                this.container = x.dom.query(options.container)[0];    // 容器对象
                this.slider = x.dom.query(options.slider)[0];          // 滑动对象

                this.width = options.width;                             // 容器显示的宽度
                this.height = options.height;                           // 容器显示的高度

                this.bindOptions(options);

                $(this.container).find('.x-ui-clip-scene').css({ width: this.width, height: this.height });

                // 重新绑定场景信息
                this.bindScenes();

                this.count = Math.abs(options.count);                   // 切换数量

                this.auto = !!this.options.auto;
                this.duration = Math.abs(this.options.duration);
                this.time = Math.abs(this.options.time);
                this.pause = Math.abs(this.options.pause);
                this.tween = this.options.tween;

                this.onStart = this.options.onStart;
                this.onFinish = this.options.onFinish;

                var bvertical = !!this.options.vertical;

                // 方向
                this._css = bvertical ? "top" : "left";

                // 样式设置
                var position = x.css.style(this.container).position;
                
                position == "relative" || position == "absolute" || (this.container.style.position = "relative");

                this.container.style.overflow = "hidden";
                this.slider.style.position = "absolute";

                this.change = this.options.change ? this.options.change : this.slider[bvertical ? "offsetHeight" : "offsetWidth"] / this.count;
            }
        };

        clip.create(options);

        return clip;
    },

    /** 
    * 补间动画
    * @namespace tween
    * @memberof x.ui.animation
    */
    tween: {
        /** 
        * 线性的
        * @method linear
        * @memberof x.ui.animation.tween
        * @param {number} t: timestamp，指缓动效果开始执行到当前帧开始执行时经过的时间段，单位ms
        * @param {number} b: beginning position，起始位置
        * @param {number} c: change，要移动的距离，就是终点位置减去起始位置
        * @param {number} d: duration ，缓和效果持续的时间
        */
        linear: function(t, b, c, d)
        {
            return c * t / d + b;
        },
        /**
        * @namespace quart
        * @memberof x.ui.animation.tween
        */
        quart: {
            /** 
            * 缓慢进入
            * @method easeIn
            * @memberof x.ui.animation.tween.quart
            * @param {number} t: timestamp，指缓动效果开始执行到当前帧开始执行时经过的时间段，单位ms
            * @param {number} b: beginning position，起始位置
            * @param {number} c: change，要移动的距离，就是终点位置减去起始位置
            * @param {number} d: duration ，缓和效果持续的时间
            */
            easeIn: function(t, b, c, d)
            {
                return c * (t /= d) * t * t * t + b;
            },
            /** 
            * 缓慢退出
            * @method easeIn
            * @memberof x.ui.animation.tween.quart
            * @param {number} [t]imestamp 指缓动效果开始执行到当前帧开始执行时经过的时间段，单位ms
            * @param {number} [b]eginning position，起始位置
            * @param {number} [c]hange 要移动的距离，就是终点位置减去起始位置
            * @param {number} [d]uration 缓和效果持续的时间
            */
            easeOut: function(t, b, c, d)
            {
                return -c * ((t = t / d - 1) * t * t * t - 1) + b;
            },
            easeInOut: function(t, b, c, d)
            {
                if ((t /= d / 2) < 1) return c / 2 * t * t * t * t + b;
                return -c / 2 * ((t -= 2) * t * t * t - 2) + b;
            }
        },
        /**
        * @namespace quad
        * @memberof x.ui.animation.tween
        */
        quad: {
            easeIn: function(t, b, c, d)
            {
                return c * (t /= d) * t + b;
            },
            easeOut: function(t, b, c, d)
            {
                return -c * (t /= d) * (t - 2) + b;
            },
            easeInOut: function(t, b, c, d)
            {
                if ((t /= d / 2) < 1) return c / 2 * t * t + b;
                return -c / 2 * ((--t) * (t - 2) - 1) + b;
            }
        },
        /**
        * @namespace cubic
        * @memberof x.ui.animation.tween
        */
        cubic: {
            easeIn: function(t, b, c, d)
            {
                return c * (t /= d) * t * t + b;
            },
            easeOut: function(t, b, c, d)
            {
                return c * ((t = t / d - 1) * t * t + 1) + b;
            },
            easeInOut: function(t, b, c, d)
            {
                if ((t /= d / 2) < 1) return c / 2 * t * t * t + b;
                return c / 2 * ((t -= 2) * t * t + 2) + b;
            }
        },
        /**
        * @namespace quint
        * @memberof x.ui.animation.tween
        */
        quint: {
            easeIn: function(t, b, c, d)
            {
                return c * (t /= d) * t * t * t * t + b;
            },
            easeOut: function(t, b, c, d)
            {
                return c * ((t = t / d - 1) * t * t * t * t + 1) + b;
            },
            easeInOut: function(t, b, c, d)
            {
                if ((t /= d / 2) < 1) return c / 2 * t * t * t * t * t + b;
                return c / 2 * ((t -= 2) * t * t * t * t + 2) + b;
            }
        },
        /**
        * @namespace sine
        * @memberof x.ui.animation.tween
        */
        sine: {
            easeIn: function(t, b, c, d)
            {
                return -c * Math.cos(t / d * (Math.PI / 2)) + c + b;
            },
            easeOut: function(t, b, c, d)
            {
                return c * Math.sin(t / d * (Math.PI / 2)) + b;
            },
            easeInOut: function(t, b, c, d)
            {
                return -c / 2 * (Math.cos(Math.PI * t / d) - 1) + b;
            }
        },
        /**
        * @namespace expo
        * @memberof x.ui.animation.tween
        */
        expo: {
            easeIn: function(t, b, c, d)
            {
                return (t == 0) ? b : c * Math.pow(2, 10 * (t / d - 1)) + b;
            },
            easeOut: function(t, b, c, d)
            {
                return (t == d) ? b + c : c * (-Math.pow(2, -10 * t / d) + 1) + b;
            },
            easeInOut: function(t, b, c, d)
            {
                if (t == 0) return b;
                if (t == d) return b + c;
                if ((t /= d / 2) < 1) return c / 2 * Math.pow(2, 10 * (t - 1)) + b;
                return c / 2 * (-Math.pow(2, -10 * --t) + 2) + b;
            }
        },
        /**
        * @namespace circ
        * @memberof x.ui.animation.tween
        */
        circ: {
            easeIn: function(t, b, c, d)
            {
                return -c * (Math.sqrt(1 - (t /= d) * t) - 1) + b;
            },
            easeOut: function(t, b, c, d)
            {
                return c * Math.sqrt(1 - (t = t / d - 1) * t) + b;
            },
            easeInOut: function(t, b, c, d)
            {
                if ((t /= d / 2) < 1) return -c / 2 * (Math.sqrt(1 - t * t) - 1) + b;
                return c / 2 * (Math.sqrt(1 - (t -= 2) * t) + 1) + b;
            }
        },
        /**
        * @namespace elastic
        * @memberof x.ui.animation.tween
        */
        elastic: {
            easeIn: function(t, b, c, d, a, p)
            {
                if (t == 0) return b; if ((t /= d) == 1) return b + c; if (!p) p = d * .3;
                if (!a || a < Math.abs(c)) { a = c; var s = p / 4; }
                else var s = p / (2 * Math.PI) * Math.asin(c / a);
                return -(a * Math.pow(2, 10 * (t -= 1)) * Math.sin((t * d - s) * (2 * Math.PI) / p)) + b;
            },
            easeOut: function(t, b, c, d, a, p)
            {
                if (t == 0) return b; if ((t /= d) == 1) return b + c; if (!p) p = d * .3;
                if (!a || a < Math.abs(c)) { a = c; var s = p / 4; }
                else var s = p / (2 * Math.PI) * Math.asin(c / a);
                return (a * Math.pow(2, -10 * t) * Math.sin((t * d - s) * (2 * Math.PI) / p) + c + b);
            },
            easeInOut: function(t, b, c, d, a, p)
            {
                if (t == 0) return b; if ((t /= d / 2) == 2) return b + c; if (!p) p = d * (.3 * 1.5);
                if (!a || a < Math.abs(c)) { a = c; var s = p / 4; }
                else var s = p / (2 * Math.PI) * Math.asin(c / a);
                if (t < 1) return -.5 * (a * Math.pow(2, 10 * (t -= 1)) * Math.sin((t * d - s) * (2 * Math.PI) / p)) + b;
                return a * Math.pow(2, -10 * (t -= 1)) * Math.sin((t * d - s) * (2 * Math.PI) / p) * .5 + c + b;
            }
        },
        /**
        * @namespace back
        * @memberof x.ui.animation.tween
        */
        back: {
            easeIn: function(t, b, c, d, s)
            {
                if (s == undefined) s = 1.70158;
                return c * (t /= d) * t * ((s + 1) * t - s) + b;
            },
            easeOut: function(t, b, c, d, s)
            {
                if (s == undefined) s = 1.70158;
                return c * ((t = t / d - 1) * t * ((s + 1) * t + s) + 1) + b;
            },
            easeInOut: function(t, b, c, d, s)
            {
                if (s == undefined) s = 1.70158;
                if ((t /= d / 2) < 1) return c / 2 * (t * t * (((s *= (1.525)) + 1) * t - s)) + b;
                return c / 2 * ((t -= 2) * t * (((s *= (1.525)) + 1) * t + s) + 2) + b;
            }
        },
        /**
        * @namespace bounce
        * @memberof x.ui.animation.tween
        */
        bounce: {
            easeIn: function(t, b, c, d)
            {
                return c - x.animation.tween.bounce.easeOut(d - t, 0, c, d) + b;
            },
            easeOut: function(t, b, c, d)
            {
                if ((t /= d) < (1 / 2.75))
                {
                    return c * (7.5625 * t * t) + b;
                }
                else if (t < (2 / 2.75))
                {
                    return c * (7.5625 * (t -= (1.5 / 2.75)) * t + .75) + b;
                }
                else if (t < (2.5 / 2.75))
                {
                    return c * (7.5625 * (t -= (2.25 / 2.75)) * t + .9375) + b;
                }
                else
                {
                    return c * (7.5625 * (t -= (2.625 / 2.75)) * t + .984375) + b;
                }
            },
            easeInOut: function(t, b, c, d)
            {
                if (t < d / 2) return x.animation.tween.bounce.easeIn(t * 2, 0, c, d) * .5 + b;
                else return x.animation.tween.bounce.easeOut(t * 2 - d, 0, c, d) * .5 + c * .5 + b;
            }
        }
    }
};// -*- ecoding=utf-8 -*-

/**
* @namespace drag
* @memberof x.ui
* @description 拖拽
*/
x.ui.drag = {
    /**
    * 获取可拖拽窗口
    */
    getDraggableWindow: function(options)
    {
        var name = x.getFriendlyName(location.pathname + '$' + options.targetWindowName + '$draggable');

        var draggable = {
            // 实例名称
            name: name,

            // 配置信息
            options: options,

            // 目标窗口
            targetWindow: null,

            // 目标窗口宽度
            targetWindowWidth: 0,

            // 目标窗口高度
            targetWindowHeight: 0,

            mask: 'dragListenerMask',

            //
            isDragging: false,

            pointX: '',
            pointY: '',

            currentX: '',
            currentY: '',

            // 拖拽时的容器样子
            draggingClassName: 'alpha',
            // 停止时的容器样子
            stopClassName: '',

            load: function()
            {
                if (document.getElementById(this.options.targetWindowName) === undefined)
                {
                    alert('元素【' + this.options.targetWindowName + '】未找到。');
                    return;
                }

                this.targetWindowName = this.options.targetWindowName;

                if (this.options.draggingClassName)
                {
                    this.draggingClassName = this.options.draggingClassName;
                }

                this.targetWindow = document.getElementById(this.targetWindowName);

                this.stopClassName = this.targetWindow.className;

                // 设置目标窗口的宽度
                if (typeof (this.options.targetWindowWidth) === 'undefined')
                {
                    this.targetWindowWidth = $(this.targetWindow).width();
                    this.targetWindowWidth += Number($(this.targetWindow).css('padding-left').replace('px', ''));
                    this.targetWindowWidth += Number($(this.targetWindow).css('padding-right').replace('px', ''));
                }
                else
                {
                    this.targetWindowWidth = this.options.targetWindowWidth;
                }

                // 设置目标窗口的高度
                if (typeof (this.options.targetWindowHeight) === 'undefined')
                {
                    this.targetWindowHeight = $(this.targetWindow).height();

                    this.targetWindowHeight += Number($(this.targetWindow).css('padding-top').replace('px', ''));
                    this.targetWindowHeight += Number($(this.targetWindow).css('padding-bottom').replace('px', ''));
                }
                else
                {
                    this.targetWindowHeight = this.options.targetWindowHeight;
                }

                // if (!document.getElementById(this.mask))
                // {
                // -*- IE 6 hack -*-

                //    var iframe = document.createElement('iframe');

                //    iframe.id = this.mask;

                //    iframe.frameBorder = 0;
                //    iframe.className = "hidden";

                //    document.body.appendChild(iframe);
                // }

                if (typeof (this.options.pointX) !== 'undefined')
                {
                    this.targetWindow.style.top = this.options.pointX + "px";
                }

                if (typeof (this.options.pointY) !== 'undefined')
                {
                    this.targetWindow.style.left = this.options.pointY + "px";
                }

                var self = this;

                // 拖拽
                $(document.body).bind('mousemove', function(event)
                {
                    event = (event === null) ? window.event : event;

                    if (self.isDragging)
                    {
                        // 自定义拖拽样式
                        if (self.options.draggingStyle !== 'default')
                        {
                            self.targetWindow.className = self.draggingClassName;
                        }

                        self.targetWindow.style.left = event.clientX - self.pointX + "px";
                        self.targetWindow.style.top = event.clientY - self.pointY + "px";

                        self.currentX = self.targetWindow.offsetLeft;
                        self.currentY = self.targetWindow.offsetTop;

                        if (self.currentX < 0)
                        {
                            self.targetWindow.style.left = '0px';
                        }

                        if (self.currentY < 0)
                        {
                            self.targetWindow.style.top = '0px';
                        }

                        /*
                        if (self.currentX + self.targetWindowWidth > document.body.clientWidth - 22)
                        {
                        self.targetWindow.style.left = (document.body.clientWidth - self.targetWindowWidth - 22) + "px";
                        }

                        if (self.currentY + self.targetWindowHeight > document.body.clientHeight)
                        {
                        self.targetWindow.style.top = (document.body.clientHeight - self.targetWindowHeight) + "px";
                        }
                        */
                        var range = x.page.getRange();

                        if (self.currentX + self.targetWindowWidth > range.width - 22)
                        {
                            self.targetWindow.style.left = (range.width - self.targetWindowWidth - 22) + "px";
                        }

                        if (self.currentY + self.targetWindowHeight > range.height)
                        {
                            self.targetWindow.style.top = (range.height - self.targetWindowHeight) + "px";
                        }

                        x.debug.log('document.body.scrollHeight:' + document.body.scrollHeight + " | self.targetWindow.offsetTop:" + self.targetWindow.offsetTop + ' | '
                        + 'range.width:' + range.width + " | range.height:" + range.height + " | "
                        + 'targetWindowWidth:' + self.targetWindowWidth + " | targetWindowHeight:" + self.targetWindowHeight + " | "
                        + 'left:' + self.currentX + ' | top:' + self.currentY + ' | right:' + (range.width - (self.currentX + self.targetWindowWidth)) + ' | bottom:' + (self.currentY + self.targetWindowHeight));

                        // -*- IE 6 hack -*-

                        var div = self.targetWindow;
                        var iframe = document.getElementById(self.mask);

                        div.style.display = "block";
                            
                        if (iframe)
                        {
                            iframe.style.width = div.offsetWidth;
                            iframe.style.height = div.offsetHeight;
                            iframe.style.top = div.style.top;
                            iframe.style.left = div.style.left;
                            iframe.style.zIndex = div.style.zIndex - 1;
                            iframe.style.position = 'absolute';
                            iframe.style.display = 'block';

                            iframe.className = "transparent";
                        } 
                    }
                    else
                    {
                        return true;
                    }
                });

                // 结束
                $(document.body).bind('mouseup', function(event)
                {
                    if (self.targetWindow.releaseCapture)
                    {
                        self.targetWindow.releaseCapture();
                    }
                    else if (window.releaseEvents)
                    {
                        window.releaseEvents(Event.MOUSEMOVE | Event.MOUSEUP);
                    };

                    if (self.isDragging)
                    {
                        var iframe = document.getElementById(self.mask);

                        if (iframe)
                        {
                            iframe.style.display = 'hidden';
                        }
                    }

                    self.isDragging = false;

                    return false;
                });

                // 拖拽开始
                $(this.targetWindow).bind('mousedown', function(event)
                {
                    event = (event === null) ? window.event : event;

                    // x.debug.log(event.target.id);
                    if (event.target.id === self.targetWindow.id)
                    {
                        if (self.targetWindow.setCapture)
                        {
                            self.targetWindow.setCapture();
                        }
                        else if (window.captureEvents)
                        {
                            window.captureEvents(Event.MOUSEMOVE | Event.MOUSEUP)
                        };

                        if (event.button != 2)
                        {
                            self.targetWindow.style.zIndex = 99;

                            self.pointX = event.clientX - self.targetWindow.offsetLeft;
                            self.pointY = event.clientY - self.targetWindow.offsetTop;

                            self.isDragging = true;
                        }
                    }
                });

                // 拖拽停止
                $(this.targetWindow).bind('mouseup', function(event)
                {
                    if (self.targetWindow.releaseCapture)
                    {
                        self.targetWindow.releaseCapture();

                        self.targetWindow.className = self.stopClassName;
                    }
                    else if (window.releaseEvents)
                    {
                        window.releaseEvents(Event.MOUSEMOVE | Event.MOUSEUP)

                        self.targetWindow.className = self.stopClassName;
                    }

                    self.isDragging = false;

                    return false;
                });
            }
        };

        draggable.load();

        return draggable;
    }
};// =============================================================================
//
// Copyright (c) 2010 ruanyu@live.com
//
// FileName     :x.ui.form.js
//
// Description  :
//
// Author       :RuanYu
//
// Date         :2010-01-01
//
// =============================================================================

/**
* 表单
*
* require	: x.js
*/
x.ui.form = {

    /*#region 函数:bindInputData(options)*/
    /*
    * 绊定控件的数据
    */
    bindInputData: function(options)
    {
        // options.inputName ,options multiSelection
        var input = x.dom.query(options.inputName);

        if ('[contacts],[corporation],[project]'.indexOf(options.featureName) > -1)
        {
            // 根据data标签的数据内容设置隐藏值和文本信息
            var data = input.attr('x-data');

            if (typeof (data) !== 'undefined' && data.indexOf('#') > -1)
            {
                var selectedValue = '';
                var selectedText = '';

                var list = x.string.trim(data, ',').split(',');

                for (var i = 0; i < list.length; i++)
                {
                    selectedValue += list[i].split('#')[1] + ',';
                    selectedText += list[i].split('#')[2] + ';';

                    // 单选时,只取data第一个值
                    if (options.multiSelection === 0) { break; }
                }

                selectedValue = x.string.rtrim(selectedValue, ',');
                selectedText = x.string.rtrim(selectedText, ';');

                if (options.multiSelection === 1)
                {
                    // 多选
                    input.val(data);
                }
                else
                {
                    // 单选
                    input.val(selectedValue);
                }

                input.attr('selectedText', selectedText);
            }
        }
    },
    /*#endregion*/

    /*#region 函数:checkDataStorage()*/
    /*
    * 验证客户端数据
    */
    checkDataStorage: function()
    {
        // 设置默认选项参数
        var options = x.ext({
            // 提示工具条
            tooltip: 0,
            // 提示框
            alert: 1
        }, arguments[0] || {});

        var warning = '';

        var list = x.dom('*');

        x.each(list, function(index, node)
        {
            try
            {
                if (x.dom(node).attr('custom-forms-data-required') || x.dom(node).attr('custom-forms-data-regexp'))
                {
                    warning += x.ui.form.checkDataInput(node, options.tooltip);
                }
            }
            catch (ex)
            {
                x.debug.error(ex);
            }
        });

        if (warning === '')
        {
            return false;
        }
        else
        {
            alert(warning);
            return true;
        }
    },
    /*#endregion*/

    /*#region 函数:checkDataInput(node, warnTooltip)*/
    /*
    * 验证客户端数据
    */
    checkDataInput: function(node, warnTooltip)
    {
        // 如果没有id信息，或者为空则不检测
        if (typeof (node.id) === 'undefined' || node.id === '') { return ''; }

        var warning = '';

        if (warnTooltip == 1)
        {
            x.tooltip.newWarnTooltip({ element: node.id, hide: 1 });
        }

        if ($(node).hasClass('custom-forms-data-required'))
        {
            // 数据必填项验证
            if ($(node).val().trim() === '')
            {
                var dataVerifyWarning = $(node).attr('dataVerifyWarning');

                // x.debug.log('x:' + x.page.getElementLeft(node) + ' y:' + x.page.getElementTop(node));

                if (dataVerifyWarning)
                {
                    if (warnTooltip == 1)
                    {
                        x.tooltip.newWarnTooltip({ element: node.id, message: dataVerifyWarning, hide: 0 });
                    }

                    warning += dataVerifyWarning + '\n';
                }
            }
        }

        if ($(node).hasClass('custom-forms-data-regexp'))
        {
            // 数据规则验证
            if ($(node).val().trim() !== '')
            {
                if (!x.expressions.exists({ text: $(node).val(), ignoreCase: $(node).attr('dataIgnoreCase'), regexpName: $(node).attr('dataRegExpName'), regexp: $(node).attr('dataRegExp') }))
                {
                    var dataRegExpWarning = $(node).attr('dataRegExpWarning');

                    // x.debug.log(x.page.getElementTop(node));

                    if (dataRegExpWarning)
                    {
                        if (warnTooltip == 1)
                        {
                            x.tooltip.newWarnTooltip({ element: node.id, message: dataRegExpWarning, hide: 0 });
                        }

                        warning += dataRegExpWarning + '\n';
                    }
                }
            }
        }

        return warning;
    },
    /*#endregion*/

    /*#region 函数:getDataStorage(options)*/
    /****/
    getDataStorage: function(options)
    {
        options = x.ext({
            storageType: 'JSON'
        }, options || {});

        options.storageType = options.storageType.toUpperCase();

        if (options.storageType === 'JSON')
        {
            return x.ui.form.getJsonDataStorage(options);
        }
        else
        {
            return x.ui.form.getXmlDataStorage(options);
        }
    },
    /*#endregion*/

    /*#region 函数:getJsonDataStorage(options)*/
    getJsonDataStorage: function(options)
    {
        var dataStorage = '';

        if (x.isUndefined(options.includeajaxStorageNode))
        {
            options.includeajaxStorageNode = (options.storageType === 'JSON' ? true : false);
        }

        if (options.includeajaxStorageNode)
        {
            dataStorage = '{ajaxStorage:{'
        }

        var list = x.dom('*');

        x.each(list, function(index, node)
        {
            try
            {
                if (typeof (node.id) === 'undefined' || node.id === '')
                {
                    return;
                }

                var dataType = x.dom(node).attr('dataType');

                if (dataType !== null && typeof (dataType) !== "undefined")
                {
                    switch (dataType)
                    {
                        case 'value':
                            dataStorage += '"' + node.id + '":"' + x.toSafeJSON($(node).val().trim()) + '",';
                            break;
                        case 'html':
                            dataStorage += '"' + node.id + '":"' + x.toSafeJSON($(node).html().trim()) + '",';
                            break;
                        case 'checkbox':
                            dataStorage += '"' + node.id + '":[';

                            if ($(document.getElementsByName(node.id)).size() === 0)
                            {
                                dataStorage += '],';
                                break;
                            }

                            var checkboxGroupName = node.id;

                            $(document.getElementsByName(node.id)).each(function(index, node)
                            {
                                if (checkboxGroupName === node.name && node.type.toLowerCase() === 'checkbox')
                                {
                                    dataStorage += '{"text":"' + $(node).attr('text') + '", "value":"' + x.toSafeJSON($(node).val()) + '", "checked":' + node.checked + '},';
                                }
                            });

                            if (dataStorage.substr(dataStorage.length - 1, 1) === ',')
                            {
                                dataStorage = dataStorage.substr(0, dataStorage.length - 1);
                            }

                            dataStorage += '],';

                            break;

                        case 'list':
                            dataStorage += '"' + node.id + '":[';

                            if ($(this).find('.list-item').size() === 0)
                            {
                                dataStorage += '],';
                                break;
                            }

                            $(this).find('.list-item').each(function(index, node)
                            {
                                dataStorage += '[';

                                $(this).find('.list-item-colum').each(function(index, node)
                                {
                                    if ($(node).hasClass('data-type-html'))
                                    {
                                        dataStorage += '"' + x.toSafeJSON($(node).html().trim()) + '",';
                                    }
                                    else
                                    {
                                        dataStorage += '"' + x.toSafeJSON($(node).val().trim()) + '",';
                                    }
                                });

                                if (dataStorage.substr(dataStorage.length - 1, 1) === ',')
                                {
                                    dataStorage = dataStorage.substr(0, dataStorage.length - 1);
                                }

                                dataStorage += '],';
                            });

                            if (dataStorage.substr(dataStorage.length - 1, 1) === ',')
                            {
                                dataStorage = dataStorage.substr(0, dataStorage.length - 1);
                            }

                            dataStorage += '],';

                            break;

                        case 'table':
                            dataStorage += '"' + node.id + '":[';

                            $('#' + node.id).find('tr').each(function(index, node)
                            {
                                if ($(this).find('.table-td-item').size() === 0)
                                {
                                    return;
                                }

                                dataStorage += '[';

                                $(this).find('.table-td-item').each(function(index, node)
                                {
                                    if ($(node).hasClass('data-type-html'))
                                    {
                                        dataStorage += '"' + x.toSafeJSON($(node).html().trim()) + '",';
                                    }
                                    else
                                    {
                                        dataStorage += '"' + x.toSafeJSON($(node).val().trim()) + '",';
                                    }
                                });

                                if (dataStorage.substr(dataStorage.length - 1, 1) === ',')
                                    dataStorage = dataStorage.substr(0, dataStorage.length - 1);

                                dataStorage += '],';
                            });

                            if (dataStorage.substr(dataStorage.length - 1, 1) === ',')
                                dataStorage = dataStorage.substr(0, dataStorage.length - 1);

                            dataStorage += '],';

                            break;
                        default:
                            break;
                    }
                }
            }
            catch (ex)
            {
                x.debug.error(ex);
            }
        });

        if (dataStorage.substr(dataStorage.length - 1, 1) === ',')
        {
            dataStorage = dataStorage.substr(0, dataStorage.length - 1);
        }

        if (options.includeajaxStorageNode)
        {
            dataStorage += '}}';
        }

        return dataStorage;
    },
    /*#endregion*/

    /*#region 函数:getXmlDataStorage(options)*/
    getXmlDataStorage: function(options)
    {
        var dataStorage = '';

        if (typeof (options) == 'undefined')
        {
            options = { includeajaxStorageNode: false };
        }

        if (options.includeajaxStorageNode)
        {
            outString = '<?xml version="1.0" encoding="utf-8" ?>';

            outString += '<ajaxStorage>';
        }

        var list = x.dom('*');

        x.each(list, function(index, node)
        {
            try
            {
                if (typeof (node.id) === 'undefined' || node.id === '')
                {
                    return;
                }

                var dataType = $(node).attr('dataType');

                if (dataType !== null && typeof (dataType) !== "undefined")
                {
                    switch (dataType)
                    {
                        case 'value':
                            dataStorage += '<' + node.id + '><![CDATA[' + $(node).val().trim() + ']]></' + node.id + '>';
                            break;
                        case 'html':
                            dataStorage += '<' + node.id + '><![CDATA[' + $(node).html().trim() + ']]></' + node.id + '>';
                            break;
                        case 'select':

                            if ($(node).get(0).selectedIndex !== -1)
                            {
                                dataStorage += '<' + node.id + '><![CDATA[' + x.toSafeJSON($(node).get(0)[$(node).get(0).selectedIndex].value.trim()) + ']]></' + node.id + '>';
                            }
                            else
                            {
                                dataStorage += '<' + node.id + '></' + node.id + '>';
                            }
                            break;

                        case 'checkbox':
                            var checkboxs = document.getElementsByName("check" + node.id);
                            var checkboxsResult = "";
                            for (var i = 0; i < checkboxs.length; i++)
                            {
                                if (checkboxs[i].checked)
                                {
                                    checkboxsResult += checkboxs[i].value + ';';
                                }
                            }

                            if (checkboxsResult !== '')
                            {
                                checkboxsResult = checkboxsResult.substring(0, checkboxsResult.length - 1);
                                dataStorage += '<' + node.id + '><![CDATA[' + checkboxsResult + ']]></' + node.id + '>';
                            }
                            else
                            {
                                var notSelectedDefaultValue = $(node).attr('notSelectedDefaultValue');

                                if (notSelectedDefaultValue == undefined)
                                {
                                    dataStorage += '<' + node.id + '><![CDATA[' + "" + ']]></' + node.id + '>';
                                }
                                else
                                {
                                    dataStorage += '<' + node.id + '><![CDATA[' + notSelectedDefaultValue + ']]></' + node.id + '>';
                                }
                            }
                            break;

                        default:
                            break;
                    }
                }
            }
            catch (ex)
            {
                x.debug.error(ex);
            }
        });

        if (options.includeajaxStorageNode)
        {
            dataStorage += '</ajaxStorage>';
        }

        return dataStorage;
    }
    /*#endregion*/
}// -*- ecoding=utf-8 -*-

/**
* @namespace mask
* @memberof x.ui
* @description 遮罩
*/
x.ui.mask = {

    zIndex: 800,

    // 遮罩的栈
    stack: null,

    getMaskStack: function()
    {
        if(x.ui.mask.stack === null)
        {
            x.ui.mask.stack = x.newStack();
        }

        return x.ui.mask.stack;
    },

    // 默认遮罩实例
    defaultInstance: null,

    /**
    * 获取默认遮罩窗口实例
    * @method getWindow
    * @memberof x.ui.mask
    * @param {object} options 选项
    * @param {object} [instance] 当前遮罩实例
    * @returns {x.ui.mask.newMaskWapper} 遮罩对象
    */
    getWindow: function(options, instance)
    {
        if(x.isUndefined(instance))
        {
            // 获得默认遮罩实例
            if(this.defaultInstance === null)
            {
                var name = x.getFriendlyName(location.pathname + '-mask-default-instance');

                this.defaultInstance = instance = x.ui.mask.newMaskWrapper(name, options);
            }
            else
            {
                instance = this.defaultInstance;
            }
        }

        // 加载遮罩、页面结构
        instance.open().innerHTML = options.content;

        instance.resize();

        x.call(options.callback);

        return instance;
    },

    /**
    * 清空当前遮罩对象
    * @method clear
    * @memberof x.ui.mask
    */
    clear: function()
    {
        var mask = x.ui.mask.getMaskStack().peek();

        if(mask !== null)
        {
            mask.close();
        }
    },

    /**
    * 遮罩封装器
    * @class MaskWrapper 
    * @constructor newMaskWrapper
    * @memberof x.ui.mask
    */
    newMaskWrapper: function(name, options)
    {
        options = options || {};

        var maskWrapper = {
            // 实例名称
            name: 'maskWrapper',
            // 弹出窗口名称
            popupWindowName: 'maskPopupWindow',
            // 配置信息
            options: null,
            // 最大透明度
            maxOpacity: 0.4,
            // 动画周期
            maxDuration: 0.2,
            // 自动隐藏
            autoHide: 1,

            create: function(name, options)
            {
                // 初始化选项信息
                this.options = x.ext({
                    height: '100%',
                    width: '100%',
                    left: '0',
                    top: '0'
                }, options || {});

                this.name = name;
                this.popupWindowName = name + '-maskPopupWindow';

                if(this.options.url)
                {
                    this.options.content = '<div >'
                            + '<iframe border="0" frameborder="0" marginheight="0" marginwidth="0" border="0" scrolling="no" '
                            + 'style="border:none; width:' + this.options.width + '; height:' + this.options.height + ';" src="' + this.options.url + '"></iframe>'
                            + '</div>';
                }
            },

            /**
            * 显示
            * @method show
            * @memberof x.ui.mask.newMaskWrapper#
            */
            show: function()
            {
                var wrapper = document.getElementById(this.name);

                if(wrapper === null)
                {
                    $(document.body).append('<div id="' + this.name + '" style="display:none;" ></div>');

                    wrapper = document.getElementById(this.name);
                }

                if(this.autoHide === 1)
                {
                    $(wrapper).bind('click', function(event)
                    {
                        $(this).unbind('click');

                        x.event.stopPropagation(event);

                        var mask = window[this.id];

                        if(x.dom.query(mask.name).css('display') === '')
                        {
                            // x.debug.log(mask.name + '.close()');
                            mask.close();
                        }
                    });
                }

                // .x-mask-wrapper{ position: absolute; top: 0; left: 0; z-index: 90; width: 100%; height: 100%; background-color: #000; }
                // wrapper.className = 'x-mask-wrapper';

                // wrapper.style.height = x.page.getRange().height + 'px';
                // wrapper.style.width = x.page.getRange().width + 'px';

                $(wrapper).css({
                    // 'position': 'absolute',
                    'position': 'fixed',
                    'top': 0,
                    'left': 0,
                    'z-index': 90,
                    'width': '100%',
                    'height': '100%',
                    'background': 'rgba(0,0,0,100)'
                });

                if(wrapper.style.display === 'none')
                {
                    // x.debug.log('show:' + mask.name);

                    x.dom.query(this.name).css({ display: '', opacity: 0.1 });

                    x.dom.query(this.popupWindowName).css({ display: 'none' });

                    x.dom.query(this.name).fadeTo((this.maxDuration * 1000), this.maxOpacity, function()
                    {
                        var mask = window[this.id];

                        // x.debug.log(mask.popupWindowName + '.show()');

                        x.dom.query(mask.popupWindowName).css({ display: '' });
                        x.dom.query(mask.popupWindowName).slideDown('slow');
                    });
                }
            },

            /**
            * 隐藏
            * @method hide
            * @memberof x.ui.mask.newMaskWrapper#
            */
            hide: function()
            {
                if(x.dom.query(this.popupWindowName).css('display') !== 'none')
                {
                    /*
                    x.dom.query(this.popupWindowName).css({ display: 'none' });

                    x.dom.query(this.name).css({ display: '', opacity: this.maxOpacity });

                    x.dom.query(this.name).fadeOut((this.maxDuration * 1000), function()
                    {
                    x.dom.query(this.name).css({ display: 'none' });
                    });
                    */

                    // 注:取消对 IE6 的支持
                    // IE 6 支持
                    /*
                    if (document.getElementById('dragListenerMask'))
                    {
                    document.getElementById('dragListenerMask').style.display = 'none';
                    }
                    */

                    var that = this;

                    x.dom.query(this.popupWindowName).fadeOut('normal', function()
                    {
                        x.dom.query(that.name).css({ display: '', opacity: that.maxOpacity });

                        x.dom.query(that.name).fadeOut((that.maxDuration * 1000), function()
                        {
                            x.dom.query(that.name).css({ display: 'none' });
                        });
                    });
                }
            },

            /**
            * 打开
            * @method open
            * @memberof x.ui.mask.newMaskWrapper#
            */
            open: function()
            {
                // 如果之前有遮罩，则隐藏之前的遮罩内容。
                var mask = x.ui.mask.getMaskStack().peek();

                if(mask !== null && mask.name !== this.name)
                {
                    mask.hide();
                }

                if(mask === null || mask.name !== this.name)
                {
                    x.ui.mask.getMaskStack().push(this);
                }

                this.show();

                var element = document.getElementById(this.popupWindowName);

                // 弹出窗口的位置
                var pointX = this.options.left, pointY = this.options.top;

                if(element === null)
                {
                    element = document.createElement('div');

                    element.id = this.popupWindowName;

                    element.style.width = this.options.width;

                    element.style.height = this.options.height;

                    element.style.display = 'none';

                    element.style.zIndex = x.ui.mask.zIndex++;

                    $(document.body).append(element);

                    $(element).fadeIn('normal');

                    pointX = (x.page.getRange().width - $(element).width()) / 2;

                    // 设置窗口的位置
                    x.dom.fixed('#' + this.popupWindowName, pointX, pointY);
                }
                else
                {
                    element.style.zIndex = x.ui.mask.zIndex++;

                    // $(element).show();
                    $(element).fadeIn('normal');

                    pointX = (x.page.getRange().width - $(element).width()) / 2;

                    x.dom.fixed('#' + this.popupWindowName, pointX, pointY);
                }

                this.resize();

                return element;
            },

            /**
            * 重置大小
            * @method resize
            * @memberof x.ui.mask.newMaskWrapper#
            */
            resize: function()
            {
                var element = x.dom.query(this.popupWindowName);

                if(element.size() === 0) { return; }

                // 弹出窗口的位置
                var pointX = this.options.left, pointY = this.options.top;

                var width = 720;

                if(element.children().length === 0) { return; }

                // 弹出窗口宽度
                var width = element.width();
                // height = $(element.children()[0]).height();

                // 设置容器宽度
                element.css({ 'width': width + 'px' });

                pointX = (x.page.getRange().width - width) / 2;

                // 设置窗口位置
                x.dom.fixed('#' + this.popupWindowName, pointX, pointY);

                // 设置窗口可拖拽
                x.ui.drag.getDraggableWindow({
                    targetWindowName: options.draggableWindowName || this.popupWindowName,
                    targetWindowWidth: options.draggableWidth,
                    targetWindowHeight: options.draggableHeight,
                    draggingStyle: 'default'
                });
            },

            closeEvent: null,

            /**
            * 关闭
            * @method close
            * @memberof x.ui.mask.newMaskWrapper#
            */
            close: function()
            {
                x.ui.mask.getMaskStack().pop();

                this.hide();

                // 如果之前遮罩，则显示之前的遮罩内容。
                var mask = x.ui.mask.getMaskStack().peek();

                if(mask !== null)
                {
                    // x.debug.log(mask.name + '.show()');
                    mask.show();
                }

                if(this.closeEvent)
                {
                    this.closeEvent();
                }
            }
        };

        maskWrapper.create(name, options);

        window[name] = maskWrapper;

        return maskWrapper;
    }
};// -*- ecoding=utf-8 -*-

/**
* @namespace tooltip
* @memberof x.ui
* @description 工具提示
*/
x.ui.tooltip = {

    /**
    * 工具提示
    * @description 工具提示
    * @class Tooltip
    * @constructor newTooltip
    * @memberof x.ui.tooltip
    * @param {object} options 选项
    */
    newTooltip: function(options)
    {
        // 设置默认选项参数
        options = x.ext({
            // 是否跟随
            following: 1,
            // 最大透明度(0～1)
            maxOpacity: 0.85,
            // 动画周期时间
            maxDuration: 0.2,
            // 默认样式
            defaultCss: true,
            // corner
            corner: false,
            // margin: "0px",
            // padding: '6px 8px 4px 8px',
            fontSize: '12px',
            // color:'#fff',
            // backgroundColor: 'transparent', // blue:#1e90ff | red:#ff1e53
            // 增量
            deltaX: 5,
            deltaY: 5,
            zIndex: 80
        }, options || {});

        var tip = {

            create: function()
            {
                this.element = x.dom.query(options.element);

                if (typeof (options.tooltip) !== 'undefined')
                {
                    // 直接绑定元素初始化 
                }
                else if (typeof (options.tooltip) === 'undefined' && typeof (this.element.attr('tooltip')) !== 'undefined')
                {
                    // 支持元素 tooltip 属性标签初始化
                    options.message = this.element.attr('tooltip');
                    options.tooltip = this.element[0].id + '$tooltip';

                    if (x.dom.query(options.tooltip).size() === 0)
                    {
                        this.element.after('<div id="' + options.tooltip + '" class="ajax-tooltip" style="display:none;" ><span>' + options.message.replace(/\n/g, '<br />') + '</span><div><image src="/resources/images/common/tooltip_bottom.gif" /></div></div>');
                    }

                    x.dom.query(options.tooltip).find('span')[0].innerHTML = options.message.replace(/\n/g, '<br />');
                }
                else if (typeof (options.message) !== 'undefined')
                {
                    // 支持函数 message 参数初始化
                    options.tooltip = this.element[0].id + '$tooltip';

                    if (x.dom.query(options.tooltip).size() === 0)
                    {
                        this.element.after('<div id="' + options.tooltip + '" class="ajax-tooltip" style="display:none;" ><span>' + options.message.replace(/\n/g, '<br />') + '</span><div><image src="/resources/images/common/tooltip_bottom.gif" /></div></div>');
                    }

                    x.dom.query(options.tooltip).find('span')[0].innerHTML = options.message.replace(/\n/g, '<br />');
                }
                else
                {
                    // 默认初始化
                    options.tooltip = this.element[0].id + '$tooltip';

                    if (x.dom.query(options.tooltip).size() === 0)
                    {
                        this.element.after('<div id="' + options.tooltip + '" class="ajax-tooltip" style="display:none;" ><span></span><div><image src="/resources/images/common/tooltip_bottom.gif" /></div></div>');
                    }
                }

                this.tooltip = x.dom.query(options.tooltip);

                if (typeof (options.name) === 'undefined')
                {
                    this.name = this.element[0].id + '$' + this.tooltip[0].id + '$object';
                }
                else
                {
                    this.name = options.name;
                }

                // 如果存在相同名称的对象，则清除旧的对象和事件
                if (typeof (window[this.name]) !== 'undefined')
                {
                    window[this.name].unregisterEvents();
                    window[this.name] = undefined;
                }

                this.options = options;

                this.tooltip.css({
                    // margin:this.options.margin,
                    // padding:this.options.padding,
                    fontSize: this.options.fontSize,
                    // color:this.options.color,
                    // backgroundColor:this.options.backgroundColor,
                    zIndex: this.options.zIndex,
                    display: 'none'
                });

                if (this.options.corner)
                {
                    x.corners.round(this.tooltip);
                }

                // 设置光标样式
                this.element.css('cursor', 'default');

                this.element.attr('tooltipName', this.name);
                this.element.attr('tooltipElementName', this.tooltip[0].id);

                // 绑定事件
                this.registerEvents();
            },

            registerEvents: function()
            {
                if (this.following === 1)
                {
                    this.element.bind('mousemove', this.follow);
                }

                this.element.bind('mouseover', this.show);
                this.element.bind('mouseout', this.hide);
            },

            unregisterEvents: function()
            {
                this.element.unbind('mousemove');
                this.element.unbind('mouseover');
                this.element.unbind('mouseout');
            },

            getPosition: function(event)
            {
                if (this.following === 1)
                {
                    // get mouse position
                    var mouseX = x.getEventPositionX(event);
                    var mouseY = x.getEventPositionY(event);

                    // decide if wee need to switch sides for the tooltip
                    var range = x.page.getElementRange(this.tooltip[0]);
                    var elementWidth = range.width;
                    var elementHeight = range.height;

                    if ((elementWidth + mouseX) >= (x.page.getViewWidth() - this.options.deltaX))
                    {
                        // too big for X
                        mouseX = mouseX - elementWidth;
                        // apply delta to make sure that the mouse is not on the tool-tip
                        mouseX = mouseX - this.options.deltaX;
                    }
                    else
                    {
                        mouseX = mouseX + this.options.deltaX;
                    }

                    if ((elementHeight + mouseY) >= (x.page.getViewHeight() - this.options.deltaY))
                    { // too big for Y
                        mouseY = mouseY - elementHeight;
                        // apply delta to make sure that the mouse is not on the tool-tip
                        mouseY = mouseY - this.options.deltaY;
                    }
                    else
                    {
                        mouseY = mouseY + this.options.deltaY;
                    }

                    return { x: mouseX, y: mouseY };
                }
                else
                {
                    var tooltipX = x.page.getElementLeft(this.element[0]);
                    var tooltipY = x.page.getElementTop(this.element[0]) - x.page.getElementRange(this.tooltip[0]).height;

                    // x.debug.log('x:' + tooltipX + ', y:' + tooltipY + ', elementHeight:' + x.page.getElementTop(this.element[0]) + '|' + x.page.getElementRange(this.element[0]).height);

                    return { x: tooltipX, y: tooltipY };
                }
            },

            setPosition: function(x, y)
            {
                this.tooltip.css({
                    position: 'absolute',
                    top: y + "px",
                    left: x + "px",
                    zIndex: this.options.zIndex
                });
            },

            follow: function(event)
            {
                var that = window[$(this).attr('tooltipName')];

                var position = that.getPosition(event);

                that.setPosition(position.x, position.y);
            },

            show: function(event)
            {
                // x.stopEventPropagation(event);

                var that = window[$(this).attr('tooltipName')];

                var position = that.getPosition(event);

                that.setPosition(position.x, position.y);

                // apply default theme if wanted
                if (that.options.defaultCss)
                {
                    that.tooltip.css({
                        //margin:this.options.margin,
                        //padding:this.options.padding,
                        //fontSize:this.options.fontSize,
                        //color:this.options.color,
                        zIndex: that.options.zIndex
                    });
                }

                // finally show the Tooltip
                that.tooltip.css({ display: '', opacity: 0.1 });

                that.tooltip.fadeTo((that.options.maxDuration * 1000), that.options.maxOpacity, function()
                {
                    that.tooltip.css({ display: '' });
                });
            },

            /**
            * 隐藏提示框
            */
            hide: function(event)
            {
                var that = window[$(this).attr('tooltipName')];

                that.tooltip.css({ display: 'none' });

                that.tooltip.css({ display: '', opacity: that.options.maxOpacity });
                that.tooltip.fadeOut((that.options.maxDuration * 1000), function()
                {
                    that.tooltip.css({ display: 'none' });
                });
            }
        };

        tip.create();

        window[tip.name] = tip;

        return tip;
    },

    /**
    * 警告型工具提示
    * @description 警告型工具提示对象
    * @class WarnTooltip
    * @constructor newWarnTooltip
    * @memberof x.ui.tooltip
    * @param {object} options 选项
    */
    newWarnTooltip: function(options)
    {
        // 移除旧的警告工具提示条
        if (x.dom.query(options.element + '$tooltip').size() > 0)
        {
            x.dom.query(options.element + '$tooltip').remove();

            if (typeof (window[options.element + '$' + options.element + '$tooltip$object']) !== 'undefined')
            {
                window[options.element + '$' + options.element + '$tooltip$object'].unregisterEvents();
                window[options.element + '$' + options.element + '$tooltip$object'] = undefined;
            }
        }

        var warnTooltip = x.ext(x.tooltip.newTooltip(options), {

            changeEvents: function()
            {
                this.element.unbind('mousemove');
                this.element.unbind('mouseover');
                this.element.unbind('mouseout');

                this.registerEvents();
            },

            registerEvents: function()
            {
                var that = this;

                // 这段代码的效果，不支持IE6
                $(document.body).bind('scroll', function(event)
                {
                    var position = that.getPosition(event);

                    that.setPosition(position.x, position.y);
                });

                this.element.bind('blur', function()
                {
                    // x.debug.log('x.tooltip.blur()');
                    x.customForm.checkDataInput(this, 1);
                });
            },

            unregisterEvents: function()
            {
                var that = this;

                $(document.body).unbind('scroll', function(event)
                {
                    var position = that.getPosition(event);

                    that.setPosition(position.x, position.y);
                });

                this.element.unbind('blur');
            },

            warn: function()
            {
                var that = this;

                var position = {
                    x: x.page.getElementLeft(this.element[0]),
                    y: x.page.getElementTop(this.element[0]) - x.page.getElementRange(this.tooltip[0]).height
                };

                that.setPosition(position.x, position.y);

                // finally show the Tooltip
                that.tooltip.css({ display: '', opacity: 0.1 });

                that.tooltip.fadeTo((that.options.maxDuration * 1000), that.options.maxOpacity, function()
                {
                    that.tooltip.css({ display: '' });
                });
            }
        });

        // 修改绑定事件关系
        warnTooltip.changeEvents();

        if (options.hide === 0)
        {
            // 显示警告
            warnTooltip.warn();
        }

        return warnTooltip;
    }
};
// -*- ecoding=utf-8 -*-

/**
* 窗口
* @namespace windows
* @memberof x.ui
*/
x.ui.windows = {

    /*#region 函数:newWindow(name, options)*/
    /**
    * 窗口
    * @class Window
    * @constructor newWindow
    * @memberof x.ui.windows
    * @param {string} name 名称
    * @param {object} [options] 选项<br />
    * 可选键值范围: 
    * <table class="param-options" >
    * <thead>
    * <tr><th>名称</th><th>类型</th><th class="last" >描述</th></tr>
    * </thead>
    * <tbody>
    * <tr><td class="name" >content</td><td>string</td><td>窗口内容</td></tr>
    * <tr><td class="name" >domain</td><td>string</td><td>所属的域</td></tr>
    * </tbody>
    * </table>
    
    */
    newWindow: function(name, options)
    {
        var internalWindow = {
            // 名称
            name: name,
            // 选项
            options: options,

            /*#region 函数:open()*/
            open: function()
            {
                document.getElementById(name).style.display = '';
            },
            /*#endregion*/

            /*#region 函数:close()*/
            close: function()
            {
                document.getElementById(name).style.display = 'none';
            },
            /*#endregion*/

            /*#region 函数:create()*/
            create: function()
            {
                var outString = '';

                var opts = this.options;

                outString = '<div id="' + this.name + '" style="';
                outString += 'position: fixed; display:none;'
                outString += 'z-index: ' + opts.zIndex + ';'
                outString += 'border: ' + opts.border + ';'
                outString += 'width: ' + opts.width + ';'
                outString += 'height: ' + opts.height + ';'
                outString += 'top: ' + opts.top + ';'
                outString += 'right:' + opts.right + ';'
                outString += 'bottom: ' + opts.bottom + ';'
                outString += 'left: ' + opts.left + ';'
                outString += '" >' + opts.content + '</div>';

                return outString;
            },
            /*#endregion*/

            /*#region 函数:destroy()*/
            /**
            * 销毁对象
            */
            destroy: function()
            {
                $(document.getElementById(name)).remove();
            },
            /*#endregion*/

            /*#region 函数:bindOptions(options)*/
            bindOptions: function(options)
            {
                // 设置默认选项参数
                this.options = x.ext({
                    zIndex: 999,                    // Z轴坐标
                    height: '100px',                // 高度
                    width: '100px',                 // 宽度
                    top: 'auto',                    // 上
                    right: 'auto',                  // 右
                    bottom: 'auto',                 // 下
                    left: 'auto'                    // 左
                }, options || {});
            },
            /*#endregion*/

            load: function(options)
            {
                // 验证并绑定选项信息
                this.bindOptions(options);

                // 设置重写后的创建函数
                if (!x.isUndefined(options.create))
                {
                    this.create = options.create;
                }

                // 加载遮罩和页面内容
                $(document.body).append(this.create());

                if (this.options.bindingFeature)
                {
                    x.dom.features.bind();
                }
            }
        };

        return internalWindow;
    },
    /*#endregion*/

    /*#region 函数:getWindow(name, options)*/
    /**
    * 获取窗口对象
    */
    getWindow: function(name, options)
    {
        var name = x.getFriendlyName(location.pathname + '-window-' + name);

        var internalWindow = x.ui.windows.newWindow(name, options);

        // 加载界面、数据、事件
        internalWindow.load(options);

        // 绑定到Window对象
        window[name] = internalWindow;

        return internalWindow;
    },
    /*#endregion*/

    /*#region 函数:getDialog(url, width, height, style)*/
    /**
    * 打开对话新窗口, 该窗口在屏幕居中.
    */
    getDialog: function(url, width, height, style)
    {
        // 样式参数
        // resizable        调整大小
        // location         地址栏

        if (typeof (style) === 'undefined')
        {
            style = 'resizable=1,directories=0,location=0,menubar=1,scrollbars=1,status=0,titlebar=0,toolbar=0';
            //
            //style = 'resizable=1,directories=1,location=1,menubar=1,scrollbars=1,status=1,titlebar=1,toolbar=1';
            //
            //style = 'resizable=1,directories=0,location=0,menubar=0,scrollbars=1,status=0,titlebar=0,toolbar=0';
        }

        if (typeof (width) === 'undefined')
        {
            style = "width=" + screen.availWidth + "," + style;
            var left = Math.round((screen.availWidth - width) / 2);
            style = "left=" + left + "," + style;
        }
        else
        {
            style = "width=" + width + "," + style;
            var left = Math.round((screen.availWidth - width) / 2);
            style = "left=" + left + "," + style;
        }

        if (typeof (height) === 'undefined')
        {
            style = "height=" + screen.availHeight + "," + style;
            var top = Math.round((screen.availHeight - height) / 2);
            style = "top=" + top + "," + style;
        }
        else
        {
            style = "height=" + height + "," + style;
            var top = Math.round((screen.availHeight - height) / 2);
            style = "top=" + top + "," + style;
        }

        return window.open(url, '', style);
    },
    /*#endregion*/

    /*#region 函数:getModalDialog(url, width, height, style)*/
    /**
    * 打开模态窗口, 该窗口在屏幕居中.
    */
    getModalDialog: function(url, width, height, style)
    {
        if (typeof (style) === 'undefined')
        {
            style = 'toolbar=no; location=no; directories=no; status=no; menubar=no; center=yes; help=0; resizable=yes; status=0;';
        }

        if (typeof (width) === 'undefined')
        {
            var left = Math.round((screen.availWidth - width) / 2);
            style = 'dialogWidth=' + width + 'px,left=' + left + ',' + style;
        }
        else
        {
            style = "dialogWidth=" + width + "px," + style;
            var left = Math.round((screen.availWidth - width) / 2);
            style = "left=" + left + "," + style;
        }

        if (typeof (height) === 'undefined')
        {
            var top = Math.round((screen.availHeight - height) / 2);
            style = 'dialogHeight=' + height + 'px,top=' + top + ',' + style;
        }
        else
        {
            style = "dialogHeight=" + height + "px," + style;
            var top = Math.round((screen.availHeight - height) / 2);
            style = "top=" + top + "," + style;
        }

        return window.showModalDialog(url, window, style);
    }
    /*#endregion*/
}
    // -*- ecoding=utf-8 -*-

/**
* 向导
* @namespace wizards
* @memberof x.ui
*/
x.ui.wizards = {

    newWizard: function(name, options)
    {
        // x.wizard 继承与 x.window 对象
        var wizard = x.ext(x.ui.windows.newWindow(name, options), {

            // 遮罩
            maskWrapper: null,

            /*#region 函数:open()*/
            open: function()
            {
                this.maskWrapper.open();
            },
            /*#endregion*/

            /*#region 函数:close()*/
            close: function()
            {
                this.maskWrapper.close();
            },
            /*#endregion*/

            /*#region 函数:save(e)*/
            save: function(e)
            {
                var result = this.save_callback(e);

                if(result === 0)
                {
                    this.close();
                }
            },
            /*#endregion*/

            /*#region 函数:save_callback(event)*/
            /*
            * 默认回调函数，可根据需要自行修改此函数。
            */
            save_callback: function(event)
            {
                return 0;
            },
            /*#endregion*/

            /*#region 函数:cancel()*/
            cancel: function()
            {
                if(typeof (this.cancel_callback) !== 'undefined')
                {
                    this.cancel_callback();
                }

                this.close();
            },
            /*#endregion*/

            /*#region 函数:create()*/
            create: function()
            {
                x.debug.error("必须重写向导对象的 create() 方法。");

                return "必须重写向导对象的 create() 方法。";
            },
            /*#endregion*/

            /*#region 函数:bindOptions(options)*/
            // bindOptions: function(options)
            // {
            //    x.debug.error("必须重写向导对象的 bindOptions() 方法。");
            // },
            /*#endregion*/

            /*#region 函数:load(options)*/
            load: function(options)
            {
                // 设置遮罩对象
                this.maskWrapper = options.maskWrapper || x.ui.mask.newMaskWrapper(this.name + '-maskWrapper');

                // 设置绑定选项函数
                if(typeof (options.bindOptions) !== 'undefined')
                {
                    this.bindOptions = options.bindOptions;
                }

                // 验证并绑定选项信息
                this.bindOptions(options);

                // 设置创建函数
                if(typeof (options.create) !== 'undefined')
                {
                    this.create = options.create;
                }

                // 设置创建后回调函数
                this.create_callback = options.create_callback || x.noop;

                // 设置保存后回调函数
                this.save_callback = options.save_callback || x.noop;

                // 设置取消回调函数
                this.cancel_callback = options.cancel_callback || x.noop;

                // 加载遮罩和页面内容
                x.ui.mask.getWindow({ content: this.create(), callback: this.create_callback }, this.maskWrapper);

                x.dom.features.bind();
            }
            /*#endregion*/
        });

        return wizard;
    },

    getWizard: function(name, options)
    {
        var name = x.getFriendlyName(location.pathname + '-wizard-' + name);

        var wizard = x.ui.wizards.newWizard(name, options);

        // 加载界面、数据、事件
        wizard.load(options);

        // 绑定到Window对象
        window[name] = wizard;

        return wizard;
    },

    // -------------------------------------------------------
    // 常用对话框
    // -------------------------------------------------------

    /**
    * 提示框
    */
    alert: function(options)
    {
        options.title = typeof (options.title) === 'undefined' ? '提示框' : options.title;

        options.width = typeof (options.width) === 'undefined' ? '360px' : options.width;

        x.ui.wizards.getWizard('alert', {

            bindOptions: function(options)
            {
                // 取消遮罩点击时的自动隐藏
                this.maskWrapper.autoHide = 0;
            },

            save_callback: function()
            {
                return 0;
            },

            create: function()
            {
                var outString = '';

                outString += '<div id="' + this.name + '" class="winodw-wizard-wrapper" style="width:' + options.width + '; height:auto;" >';

                outString += '<div class="winodw-wizard-toolbar" >';
                outString += '<div class="winodw-wizard-toolbar-close">';
                outString += '<a href="javascript:' + this.name + '.cancel();" class="button-text" >关闭</a>';
                outString += '</div>';
                outString += '<div class="float-left">';
                outString += '<div class="winodw-wizard-toolbar-item" style="width:200px;" ><span>' + options.title + '</span></div>';
                outString += '<div class="clear"></div>';
                outString += '</div>';
                outString += '<div class="clear"></div>';
                outString += '</div>';
                outString += '<div style="padding:10px;" >' + options.message + '</div>';
                outString += '<div class="winodw-wizard-result-container" >';
                outString += '<div class="winodw-wizard-result-item" style="float:right;" ><div class="button-2font-wrapper" style="margin:0 10px 3px 0px;" ><a href="javascript:' + this.name + '.save();" class="button-text" >确定</a></div></div>';
                outString += '<div class="clear"></div>';
                outString += '</div>';

                outString += '<div class="clear"></div>';
                outString += '</div>';

                return outString;
            }
        });

        x.stopEventPropagation();
    },

    /**
    * 确认框
    */
    confirm: function(options)
    {
        options.title = typeof (options.title) === 'undefined' ? '确认框' : options.title;

        options.width = typeof (options.width) === 'undefined' ? '360px' : options.width;

        x.ui.wizards.getWizard('confirm', {

            bindOptions: function(options)
            {
                // 取消遮罩点击时的自动隐藏
                this.maskWrapper.autoHide = 0;
            },

            save_callback: function()
            {
                if(options.callback)
                {
                    options.callback();
                }

                return 0;
            },

            create: function()
            {
                var outString = '';

                outString += '<div id="' + this.name + '" class="winodw-wizard-wrapper" style="width:' + options.width + '; height:auto;" >';

                outString += '<div class="winodw-wizard-toolbar" >';
                outString += '<div class="winodw-wizard-toolbar-close">';
                outString += '<a href="javascript:' + this.name + '.cancel();" class="button-text" >关闭</a>';
                outString += '</div>';
                outString += '<div class="float-left">';
                outString += '<div class="winodw-wizard-toolbar-item" style="width:200px;" ><span>' + options.title + '</span></div>';
                outString += '<div class="clear"></div>';
                outString += '</div>';
                outString += '<div class="clear"></div>';
                outString += '</div>';
                outString += '<div style="padding:10px;" >' + options.message + '</div>';
                outString += '<div class="winodw-wizard-result-container" >';
                outString += '<div class="winodw-wizard-result-item" style="float:right;" ><div class="button-2font-wrapper" style="margin:0 10px 3px 0px;" ><a href="javascript:' + this.name + '.cancel();" class="button-text" >取消</a></div></div>';
                outString += '<div class="winodw-wizard-result-item" style="float:right;" ><div class="button-2font-wrapper" style="margin:0 10px 3px 0px;" ><a href="javascript:' + this.name + '.save();" class="button-text" >确定</a></div></div>';
                outString += '<div class="clear"></div>';
                outString += '</div>';

                outString += '<div class="clear"></div>';
                outString += '</div>';

                return outString;
            }
        });
    },

    /**
    * 提示输入框
    */
    prompt: function(options)
    {
        options.title = typeof (options.title) === 'undefined' ? '提示输入框' : options.title;
        options.message = typeof (options.message) === 'undefined' ? '请填写相关内容' : options.message;
        options.defaultValue = typeof (options.defaultValue) === 'undefined' ? '' : options.defaultValue;

        options.width = typeof (options.width) === 'undefined' ? '360px' : options.width;

        x.ui.wizards.getWizard('confirm', {

            bindOptions: function(options)
            {
                // 取消遮罩点击时的自动隐藏
                this.maskWrapper.autoHide = 0;
            },

            save_callback: function()
            {
                if(options.callback)
                {
                    options.callback(x.dom.query(this.name + '$input').val());
                }

                return 0;
            },

            create: function()
            {
                var outString = '';

                outString += '<div id="' + this.name + '" class="winodw-wizard-wrapper" style="width:' + options.width + '; height:auto;" >';

                outString += '<div class="winodw-wizard-toolbar" >';
                outString += '<div class="winodw-wizard-toolbar-close">';
                outString += '<a href="javascript:' + this.name + '.cancel();" class="button-text" >关闭</a>';
                outString += '</div>';
                outString += '<div class="float-left">';
                outString += '<div class="winodw-wizard-toolbar-item" style="width:200px;" ><span>' + options.title + '</span></div>';
                outString += '<div class="clear"></div>';
                outString += '</div>';
                outString += '<div class="clear"></div>';
                outString += '</div>';
                outString += '<div style="padding:10px;" >';
                outString += options.message + '<br />';
                outString += '<input id="' + this.name + '$input" name="' + this.name + '$input" type="text" value="' + options.defaultValue + '" class="input-normal" style="width:200px" />';
                outString += '</div>';
                outString += '<div class="winodw-wizard-result-container" >';
                outString += '<div class="winodw-wizard-result-item" style="float:right;" ><div class="button-2font-wrapper" style="margin:0 10px 3px 0px;" ><a href="javascript:' + this.name + '.save();" class="button-text" >确定</a></div></div>';
                outString += '<div class="clear"></div>';
                outString += '</div>';

                outString += '<div class="clear"></div>';
                outString += '</div>';

                return outString;
            }
        });
    }
};// -*- ecoding=utf-8 -*-

/**
* @namespace dialogs
* @memberof x.ui
* @description 页面对话框
*/
x.ui.dialogs = {

    // 默认选项
    defaults: {
        id: '',
        title: '标题',
        content: 'text:内容',
        width: "280",
        height: "140",
        titleClass: "box-title",
        closeID: "",
        triggerID: "",
        // 对话框边框颜色 #E9F3FD eeeeee
        // boxBdColor: "#eeeeee",
        boxBdOpacity: "1",
        // 对话框包围边框颜色 #A6C9E1
        boxWrapBdColor: "#cccccc",
        windowBgColor: "#000000",
        windowBgOpacity: "0.5",
        time: "",
        drag: "",
        dragBoxOpacity: "1",
        showTitle: true,
        showBoxbg: true,
        showbg: false,
        offsets: "",
        button: "",
        callback: function() { },
        fns: function() { }
    },

    // 列表
    list: [],

    /**
    * 对话框
    * @description 对话框
    * @class Dialog
    * @constructor newDialog
    * @memberof x.ui.dialogs
    * @param {object} options 选项
    */
    newDialog: function(options)
    {
        // options = x.ext({}, x.ui.dialogs.defaults, options || {});
        options = x.ext({}, x.ui.dialogs.defaults, options || {});

        var dialog = {
            id: null,
            container: null,

            //            getID: function()
            //            {
            //                return thisID = BOXID.boxID;
            //            },

            //构造弹出层
            show: function()
            {
                var $titleHeight = options.showTitle != true ? 1 : 33,
				$borderHeight = options.showTitle != true ? 0 : 10;
                $boxDialogHeight = options.button != "" ? 45 : 0;
                $boxDialogBorder = $boxDialogHeight == "0" ? "0" : "1";
                var $width = parseInt(options.width) > 1000 ? 1000 : parseInt(options.width),
				$height = parseInt(options.height) > 550 ? 550 : parseInt(options.height);
                var $boxDom = "<div id=\"" + options.id + "\" class=\"x-ui-dialogs\">";
                $boxDom += "<div class=\"boxWrap\">";
                $boxDom += "<div class=\"box-title\"><h3></h3><span class=\"box-close-btn\">关闭</span></div>";
                $boxDom += "<div class=\"boxContent\"></div>";
                $boxDom += "<div class=\"box-dialog\"></div>";
                $boxDom += "</div>";
                $boxDom += "<div class=\"box-bd\"></div>";
                $boxDom += "<iframe src=\"about:blank\" style=\"position:absolute;left:0;top:0;filter:alpha(opacity=0);opacity:0;scrolling=no;z-index:10714\"></iframe>";
                $boxDom += "</div>";
                $($boxDom).appendTo("body");
                var $box = $("#" + options.id);
                $box.css({
                    position: "relative",
                    width: $width + 12 + "px",
                    height: $height + $titleHeight + $borderHeight + $boxDialogHeight + 1 + "px",
                    zIndex: "891208"
                });
                var $iframe = $("iframe", $box);
                $iframe.css({
                    width: $width + 12 + "px",
                    height: $height + $titleHeight + $borderHeight + $boxDialogHeight + 1 + "px"
                });
                var $boxWrap = $(".boxWrap", $box);
                $boxWrap.css({
                    position: "relative",
                    top: "5px",
                    margin: "0 auto",
                    width: $width + "px",
                    height: $height + $titleHeight + $boxDialogHeight + 1 + "px",
                    overflow: "hidden",
                    zIndex: "20590"
                });
                
                var $boxContent = $(".boxContent", $box);
                $boxContent.css({
                    position: "relative",
                    width: $width + "px",
                    height: $height + "px",
                    padding: "0",
                    borderWidth: "1px",
                    borderStyle: "solid",
                    borderColor: options.boxWrapBdColor,
                    overflow: "auto",
                    backgroundColor: "#fff"
                });
                var $boxDialog = $(".box-dialog", $box);
                $boxDialog.css({
                    width: $width + "px",
                    height: $boxDialogHeight + "px",
                    borderWidth: $boxDialogBorder + "px",
                    borderStyle: "solid",
                    borderColor: options.boxWrapBdColor,
                    borderTop: "none",
                    textAlign: "right"
                });
                var $boxBg = $(".box-bd", $box);
                $boxBg.css({
                    position: "absolute",
                    width: $width + 12 + "px",
                    height: $height + $titleHeight + $borderHeight + $boxDialogHeight + 1 + "px",
                    left: "0",
                    top: "0",
                    opacity: options.boxBdOpacity,
                    background: options.boxBdColor,
                    zIndex: "10715"
                });
                var $title = $(".box-title>h3", $box);
                $title.html(options.title);
                $title.parent().css({
                    position: "relative",
                    width: $width + "px",
                    borderColor: options.boxWrapBdColor
                });
                if (options.titleClass != "")
                {
                    $title.parent().addClass(options.titleClass);
                    $title.parent().find("span").hover(function()
                    {
                        $(this).addClass("hover");
                    }, function()
                    {
                        $(this).removeClass("hover");
                    });
                };
                if (options.showTitle != true) { $(".box-title", $box).remove(); }
                if (options.showBoxbg != true)
                {
                    $(".box-bd", $box).remove();
                    $box.css({
                        width: $width + 2 + "px",
                        height: $height + $titleHeight + $boxDialogHeight + 1 + "px"
                    });
                    $boxWrap.css({ left: "0", top: "0" });
                };
                // 定位弹出层
                var TOP = -1;
                this.getDomPosition();
                var $location = options.offsets;
                var $wrap = $("<div id=\"" + options.id + "parent\"></div>");
                var est = x.browser.ie6 ? (options.triggerID != "" ? 0 : document.documentElement.scrollTop) : "";
                if (options.offsets == "" || options.offsets.constructor == String)
                {
                    switch ($location)
                    {
                        case ("left-top"):      //左上角
                            $location = { left: "0px", top: "0px" + est };
                            TOP = 0;
                            break;
                        case ("left-bottom"):   //左下角
                            $location = { left: "0px", bottom: "0px" };
                            break;
                        case ("right-top"):     //右上角
                            $location = { right: "0px", top: "0px" + est };
                            TOP = 0;
                            break;
                        case ("right-bottom"):  //右下角
                            $location = { right: "0px", bottom: "0px" };
                            break;
                        case ("middle-top"):    //居中置顶
                            $location = { left: "50%", marginLeft: -parseInt($box.width() / 2) + "px", top: "0px" + est };
                            TOP = 0;
                            break;
                        case ("middle-bottom"): //居中置低
                            $location = { left: "50%", marginLeft: -parseInt($box.width() / 2) + "px", bottom: "0px" };
                            break;
                        case ("left-middle"):   //左边居中
                            $location = { left: "0px", top: "50%" + est, marginTop: -parseInt($box.height() / 2) + "px" + est };
                            TOP = $getPageSize[1] / 2 - $box.height() / 2;
                            break;
                        case ("right-middle"):  //右边居中
                            $location = { right: "0px", top: "50%" + est, marginTop: -parseInt($box.height() / 2) + "px" + est };
                            TOP = $getPageSize[1] / 2 - $box.height() / 2;
                            break;
                        default:                //默认为居中
                            $location = { left: "50%", marginLeft: -parseInt($box.width() / 2) + "px", top: "50%" + est, marginTop: -parseInt($box.height() / 2) + "px" + est };
                            TOP = $getPageSize[1] / 2 - $box.height() / 2;
                            break;
                    };
                }
                else
                {
                    var str = $location.top;
                    $location.top = $location.top + est;
                    if (typeof (str) != 'undefined')
                    {
                        str = str.replace("px", "");
                        TOP = str;
                    };
                };

                if (options.triggerID != "")
                {
                    var $offset = $("#" + options.triggerID).offset();
                    var triggerID_W = $("#" + options.triggerID).outerWidth(), triggerID_H = $("#" + options.triggerID).outerHeight();
                    var triggerID_Left = $offset.left, triggerID_Top = $offset.top;
                    var vL = $location.left, vT = $location.top;
                    if (typeof (vL) != 'undefined' || typeof (vT) != 'undefined')
                    {
                        vL = parseInt(vL.replace("px", ""));
                        vT = parseInt(vT.replace("px", ""));
                    };
                    var left = vL >= 0 ? parseInt(vL) + triggerID_Left : parseInt(vL) + triggerID_Left - $getPageSize[2];
                    var top = vT >= 0 ? parseInt(vT) + triggerID_Top : parseInt(vT) + triggerID_Top - $getPageSize[3];
                    $location = { left: left + "px", top: top + "px" };
                };
                if (x.browser.ie6)
                {
                    if (options.triggerID == "")
                    {
                        if (TOP >= 0)
                        {
                            this.addStyle(".ui_fixed_" + options.id + "{width:100%;height:100%;position:absolute;left:expression(documentElement.scrollLeft+documentElement.clientWidth-this.offsetWidth);top:expression(documentElement.scrollTop+" + TOP + ")}");
                            $wrap = $("<div class=\"" + options.id + "IE6FIXED\" id=\"" + options.id + "parent\"></div>");
                            $box.appendTo($wrap);
                            $("body").append($wrap);
                            $("." + options.id + "IE6FIXED").css($location).css({
                                position: "absolute",
                                width: $width + 12 + "px",
                                height: $height + $titleHeight + $borderHeight + $boxDialogHeight + 1 + "px",
                                zIndex: "891208"
                            }).addClass("ui_fixed_" + options.id);
                        } else
                        {
                            this.addStyle(".ui_fixed2_" + options.id + "{width:100%;height:100%;position:absolute;left:expression(documentElement.scrollLeft+documentElement.clientWidth-this.offsetWidth);top:expression(documentElement.scrollTop+documentElement.clientHeight-this.offsetHeight)}");
                            $wrap = $("<div class=\"" + options.id + "IE6FIXED\"  id=\"" + options.id + "parent\"></div>");
                            $box.appendTo($wrap);
                            $("body").append($wrap);
                            $("." + options.id + "IE6FIXED").css($location).css({
                                position: "absolute",
                                width: $width + 12 + "px",
                                height: $height + $titleHeight + $borderHeight + $boxDialogHeight + 1 + "px",
                                zIndex: "891208"
                            }).addClass("ui_fixed2_" + options.id);
                        };
                        $("body").css("background-attachment", "fixed").css("background-image", "url(n1othing.txt)");
                    }
                    else
                    {
                        $wrap.css({
                            position: "absolute",
                            left: left + "px",
                            top: top + "px",
                            width: $width + 12 + "px",
                            height: $height + $titleHeight + $borderHeight + $boxDialogHeight + 1 + "px",
                            zIndex: "891208"
                        });
                    };
                }
                else
                {
                    $wrap.css($location).css({
                        position: "fixed",
                        width: $width + 12 + "px",
                        height: $height + $titleHeight + $borderHeight + $boxDialogHeight + 1 + "px",
                        zIndex: "891208"
                    });
                    if (options.triggerID != "") { $wrap.css({ position: "absolute" }) };
                    $("body").append($wrap);
                    $box.appendTo($wrap);
                };
            },

            // 装载弹出层内容
            setContent: function()
            {
                var $box = $("#" + options.id);
                var $width = parseInt(options.width) > 1000 ? 1000 : parseInt(options.width),
				$height = parseInt(options.height) > 550 ? 550 : parseInt(options.height);
                var $contentID = $(".boxContent", $box);
                $contentType = options.content.substring(0, options.content.indexOf(":"));
                $content = options.content.substring(options.content.indexOf(":") + 1, options.content.length);
                $.ajaxSetup({ global: false });
                switch ($contentType)
                {
                    case "text":
                        $contentID.html($content);
                        break;
                    case "id":
                        $("#" + $content).children().appendTo($contentID);
                        break;
                    case "img":
                        $.ajax({
                            beforeSend: function()
                            {
                                $contentID.html("<p class='boxLoading'>loading...</p>");
                            },
                            error: function()
                            {
                                $contentID.html("<p class='boxError'>加载数据出错...</p>");
                            },
                            success: function(html)
                            {
                                $contentID.html("<img src=" + $content + " alt='' />");
                            }
                        });
                        break;
                    case "swf":
                        $.ajax({
                            beforeSend: function()
                            {
                                $contentID.html("<p class='boxLoading'>loading...</p>");
                            },
                            error: function()
                            {
                                $contentID.html("<p class='boxError'>加载数据出错...</p>");
                            },
                            success: function(html)
                            {
                                $contentID.html("<div id='" + options.id + "swf'><h1>Alternative content</h1><p><a href=\"http://www.adobe.com/go/getflashplayer\"><img src=\"http://www.adobe.com/images/shared/download_buttons/get_flash_player.gif\" alt=\"Get Adobe Flash player\" /></a></p></div><script type=\"text/javascript\" src=\"swfobject.js\" ></script><script type=\"text/javascript\">swfobject.embedSWF('" + $content + "', '" + options.id + "swf', '" + $width + "', '" + $height + "', '9.0.0', 'expressInstall.swf');</script>");
                                $("#" + options.id + "swf").css({
                                    position: "absolute",
                                    left: "0",
                                    top: "0",
                                    textAlign: "center"
                                });
                            }
                        });
                        break;
                    case "url":
                        var contentDate = $content.split("?");
                        $.ajax({
                            beforeSend: function()
                            {
                                $contentID.html("<p class='boxLoading'>loading...</p>");
                            },
                            type: contentDate[0],
                            url: contentDate[1],
                            data: contentDate[2],
                            error: function()
                            {
                                $contentID.html("<p class='boxError'><em></em><span>加载数据出错...</span></p>");
                            },
                            success: function(html)
                            {
                                $contentID.html(html);
                            }
                        });
                        break;
                    case "iframe":
                        $contentID.css({ overflowY: "hidden" });
                        $.ajax({
                            beforeSend: function()
                            {
                                $contentID.html("<p class='boxLoading'>loading...</p>");
                            },
                            error: function()
                            {
                                $contentID.html("<p class='boxError'>加载数据出错...</p>");
                            },
                            success: function(html)
                            {
                                $contentID.html("<iframe src=\"" + $content + "\" width=\"100%\" height=\"" + parseInt(options.height) + "px\" scrolling=\"auto\" frameborder=\"0\" marginheight=\"0\" marginwidth=\"0\"></iframe>");
                            }
                        });
                };
            },

            //对话模式
            ask: function()
            {
                var $box = $("#" + options.id);
                $boxDialog = $(".box-dialog", $box);
                if (options.button != "")
                {
                    var map = {}, answerStrings = [];

                    if (options.button instanceof Array)
                    {
                        for (var i = 0; i < options.button.length; i++)
                        {
                            map[options.button[i]] = options.button[i];
                            answerStrings.push(options.button[i]);
                        };
                    }
                    else
                    {
                        for (var k in options.button)
                        {
                            map[options.button[k]] = k;
                            answerStrings.push(options.button[k]);
                        };
                    };

                    $boxDialog.html($.map(answerStrings, function(v)
                    {
                        return "<input class='dialogBtn' type='button'  value='" + v + "' />";
                    }).join(' '));

                    $(".dialogBtn", $boxDialog).hover(function()
                    {
                        $(this).addClass("hover");
                    }, function()
                    {
                        $(this).removeClass("hover");
                    }).click(function()
                    {
                        var $this = this;
                        if (options.callback != "" && $.isFunction(options.callback))
                        {
                            //设置回调函数返回值很简单，就是回调函数名后加括号括住的返回值就可以了。
                            options.callback(map[$this.value]);
                        };
                        this.remove();
                    });
                };
            },
            // 获取要吸附的ID的left和top值并重新计算弹出层left和top值
            getDomPosition: function()
            {
                var $box = $("#" + options.id);
                var cw = document.documentElement.clientWidth, ch = document.documentElement.clientHeight;
                var sw = $box.outerWidth(), sh = $box.outerHeight();
                var $soffset = $box.offset(), sl = $soffset.left, st = $soffset.top;
                $getPageSize = new Array();
                $getPageSize.push(cw, ch, sw, sh, sl, st);
            },
            //设置窗口的zIndex
            setZIndex: function()
            {
                x.ui.dialogs.list.push(document.getElementById(options.id + "parent")); //存储窗口到数组

                // var event = "mousedown" || "click";
                var target = x.query('#' + this.id + 'parent');

                x.event.add(target, 'click', function()
                {
                    for (var i = 0; i < x.ui.dialogs.list.length; i++)
                    {
                        x.ui.dialogs.list[i].style.zIndex = 870618;
                    };

                    target.style.zIndex = 891208;
                });
            },

            // 写入CSS样式
            addStyle: function(s)
            {
                var T = this.style;
                if (!T)
                {
                    T = this.style = document.createElement('style');
                    T.setAttribute('type', 'text/css');
                    document.getElementsByTagName('head')[0].appendChild(T);
                };
                T.styleSheet && (T.styleSheet.cssText += s) || T.appendChild(document.createTextNode(s));
            },

            // 绑定拖拽
            drag: function()
            {
                var $moveX = 0, $moveY = 0,
				drag = false;
                var $ID = $("#" + options.id);
                $Handle = $("." + options.drag, $ID);
                $Handle.mouseover(function()
                {
                    if (options.triggerID != "")
                    {
                        $(this).css("cursor", "default");
                    } else
                    {
                        $(this).css("cursor", "move");
                    };
                });
                $Handle.mousedown(function(e)
                {
                    drag = options.triggerID != "" ? false : true;
                    if (options.dragBoxOpacity)
                    {
                        if (options.boxBdOpacity != "1")
                        {
                            $ID.children("div").css("opacity", options.dragBoxOpacity);
                            $ID.children("div.box-bd").css("opacity", options.box - bdOpacity);
                        } else
                        {
                            $ID.children("div").css("opacity", options.dragBoxOpacity);
                        };
                    };
                    e = window.event ? window.event : e;
                    var ID = document.getElementById(options.id);
                    $moveX = e.clientX - ID.offsetLeft;
                    $moveY = e.clientY - ID.offsetTop;
                    $(document).mousemove(function(e)
                    {
                        if (drag)
                        {
                            e = window.event ? window.event : e;
                            window.getSelection ? window.getSelection().removeAllRanges() : document.selection.empty();
                            var x = e.clientX - $moveX;
                            var y = e.clientY - $moveY;
                            $(ID).css({
                                left: x,
                                top: y
                            });
                        };
                    });
                    $(document).mouseup(function()
                    {
                        drag = false;
                        if (options.dragBoxOpacity)
                        {
                            if (options.boxBdOpacity != "1")
                            {
                                $ID.children("div").css("opacity", "1");
                                $ID.children("div.box-bd").css("opacity", options.box - bdOpacity);
                            } else
                            {
                                $ID.children("div").css("opacity", "1");
                            };
                        };
                    });
                });
            },
            //关闭弹出层
            remove: function()
            {
                var $box = $("#" + this.id);
                var $boxbg = $("#x-ui-dialogs-window-bg");
                if ($box != null || $boxbg != null)
                {
                    var $contentID = $(".boxContent", $box);
                    $contentType = this.content.substring(0, this.content.indexOf(":"));
                    $content = this.content.substring(this.content.indexOf(":") + 1, this.content.length);
                    if ($contentType == "id")
                    {
                        $contentID.children().appendTo($("#" + $content));
                        $box.parent().removeAttr("style").remove();
                        $boxbg.animate({ opacity: "0" }, 500, function() { $(this).remove(); });
                        $("body").css("background", "#fff");
                    }
                    else
                    {
                        $box.parent().removeAttr("style").remove();
                        $boxbg.animate({ opacity: "0" }, 500, function() { $(this).remove(); });
                        $("body").css("background", "#fff");
                    };
                };
            },

            //健盘事件，当按Esc的时候关闭弹出层
            keydown: function()
            {
                document.onkeydown = function(e)
                {
                    e = e || event;
                    if (e.keyCode == 27)
                    {
                        this.remove();
                    };
                };
            },

            /*#region 函数:create(options)*/
            /**
            * 创建对象实例
            */
            create: function(options)
            {
                var me = this;

                // 初始化选项信息
                this.options = options;

                if (this.options.id == '')
                {
                    this.id = this.options.id = x.randomText.create(10);
                }
                else
                {
                    this.id = this.options.id;
                }

                this.content = options.content;

                // BOXID = options;

                if (x.query("#" + this.id) != null)
                {
                    x.msg("创建弹出层失败！窗口“" + options.id + "”已存在！");
                    return false;
                };

                this.show();

                // 设置内容
                this.setContent();

                var $box = $("#" + this.id);

                this.box = x.query("#" + this.id);

                x.dom.on(x.query(".box-close-btn", this.box), "click", function()
                {
                    me.remove();
                });

                x.css.style(this.box, { zIndex: "870618" });

                if (options.closeID != "")
                {
                    x.dom.on(x.query("#" + options.closeID, this.box), "click", function()
                    {
                        me.remove();
                    });
                };

                if (options.time != "")
                {
                    setTimeout(function() { me.remove(); }, options.time);
                };

                if (options.showbg != "" && options.showbg == true)
                {
                    x.dom.append(document.body, '<div id="x-ui-dialogs-window-bg" style="position:absolute;background:' + options.windowBgColor + ';filter:alpha(opacity='+ options.windowBgOpacity +');opacity:'+ options.windowBgOpacity +';width:100%;left:0;top:0;z-index:870618"><iframe src="about:blank" style="width=100%;height:' + $(document).height() + 'px;filter:alpha(opacity=0);opacity:0;scrolling=no;z-index:870610"></iframe></div>');

                    // var $boxBgDom = "<div id=\"x-ui-dialogs-window-bg\" style=\"position:absolute;background:" + options.windowBgColor + ";filter:alpha(opacity=0);opacity:0;width:100%;left:0;top:0;z-index:870618\"><iframe src=\"about:blank\" style=\"width=100%;height:" + $(document).height() + "px;filter:alpha(opacity=0);opacity:0;scrolling=no;z-index:870610\"></iframe></div>";
                    // $($boxBgDom).appendTo("body").animate({ opacity: options.windowBgOpacity }, 200);
                };
                if (options.drag != "")
                {
                    this.drag();
                };
                if (options.fns != "" && $.isFunction(options.fns))
                {
                    options.fns.call(this);
                };

                if (options.button != "")
                {
                    this.ask();
                };

                this.keydown();

                this.setZIndex();

                if (options.showbg != true)
                {
                    // $("#" + options.id).addClass("shadow");
                    x.css.add(this.container, 'shadow');
                };

                // $("#" + options.id).die().live("mouseenter", function()
                // {
                //    BOXID = o;
                // });
            }
            /*#endregion*/

        }

        dialog.create(options);

        return dialog;
    }
};
// -*- ecoding=utf-8 -*-

/**
* @namespace util
* @memberof x.ui
* @description 常用工具类库
*/
x.ui.util = {
    /**
    * 设置 Flash 元素
    * @method setFlash
    * @memberof x.ui.util
    * @param {string} selector 选择器或者元素对象
    * @param {string} url Falsh 的地址
    * @param {number} widht Falsh 的宽度
    * @param {number} height Falsh 的高度
    * @param {string} [summary] Falsh 内容概述
    */
    setFlash: function(selector, url, width, height, summary)
    {
        var outString = '';

        var element = x.query(selector);

        summary = x.isUndefined(summary, '');

        outString += '<object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" '
			+ 'codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=11" '
			+ 'width="' + width + '" height="' + height + '" title="' + summary + '"> '
			+ '<param name="movie" value="' + url + '" />'
			+ '<param name="quality" value="high" />'
			+ '<embed src="' + url + '" quality="high" '
			+ 'pluginspage="http://www.macromedia.com/go/getflashplayer" '
			+ 'type="application/x-shockwave-flash" '
			+ 'width="' + width + '" height="' + height + '"></embed>'
			+ '</object>';

        element.innerHTML = outString;
    },

    /**
    * 在页面上打开QQ链接
    * @method setQQ
    * @memberof x.ui.util
    * @param {string} selector 选择器或者元素对象
    * @param {number} qq QQ 号码
    * @param {string} site 来源的站点信息
    */
    setQQ: function(selector, qq, site)
    {
        var outString = '';

        var element = x.query(selector);

        if (!(qq === '' || qq === '0'))
        {
            outString += '<a target="_blank" href="tencent://message/?uin=';
            outString += qq + '&site=' + ((site) ? site : 'localhost');
            outString += '&menu=yes" alt="QQ:' + qq + '" >' + qq + "</a>";
        }

        element.innerHTML = outString;
    }
};
// -*- ecoding=utf-8 -*-

/**
* Combobox
*
* require	: x.js
*/
x.ui.pkg.combobox = {

  // 默认配置
  defaults: {
    /** 样式名称默认前缀 */
    classNamePrefix: 'x-ui-pkg-combobox'
  },

  // 缓存
  caches: {},

  newCombobox: function(name, containerName, targetViewName, targetValueName, options)
  {
    options = x.ext({}, x.ui.pkg.combobox.defaults, options || {});

    //
    // options : {selectedValue:'selectedValue',comboboxType:'static',list[{text:'text1',value:'value1'},{text:'text2',value:'value2'}]}
    // options : {selectedValue:'selectedValue',comboboxType:'dynamic',url:'/fff/ffe.aspx'}
    //

    var combobox = {
      // 对象名称
      name: name,
      // 选项
      options: options,
      // 容器对象
      container: x.dom.query(containerName),
      // 目标视图对象
      targetViewObject: x.dom.query(targetViewName),
      // 目标值对象
      targetValueObject: x.dom.query(targetValueName),
      // 内部容器对象
      innerContainer: null,
      // 选择的文本
      selectedText: '',
      // 选择的值
      selectedValue: '',
      // 默认空数据源显示的文本信息
      defaultEmptyText: '##请配置数据源##',

      lockObject: false,

      /*#region 函数:getValue(text, value)*/
      /*
      * 获取值
      */
      getValue: function(text, value, disableCallback)
      {
        if(text === this.defaultEmptyText)
        {
          this.targetViewObject.val('');
          this.targetValueObject.val('');
        }

        this.targetViewObject.val(text);
        this.targetValueObject.val(value);
        this.targetValueObject.attr('selectedText', text);

        this.selectedText = text;
        this.selectedValue = value;

        // 设置验证组件
        if(this.targetViewObject.hasClass('custom-forms-data-required') || this.targetViewObject.hasClass('custom-forms-data-regexp'))
        {
          // x.customForm.checkDataInput(this.targetViewObject[0], 1);
        }

        this.close();

        if(!disableCallback)
        {
          x.call(options.callback);
        }
      },
      /*#endregion*/

      /*#region 函数:create()*/
      /*
      * 创建
      */
      create: function()
      {
        var width = this.targetViewObject.width();

        if(typeof (this.options.widthOffset) !== 'undefined')
        {
          width = width + Number(this.options.widthOffset);
        }

        this.container.hide();

        this.container.html('<div id="' + this.name + '-innerContainer" style="display: none;" ></div>');

        this.innerContainer = x.dom.query(this.name + '-innerContainer');

        this.innerContainer.attr('class', options.classNamePrefix);

        if(this.options.comboboxType === 'static')
        {
          var list = this.options.list;

          if(typeof (list) === 'undefined')
          {
            list = this.options.list = [{ text: this.defaultEmptyText, value: '' }];
          }

          for(var i = 0;i < list.length;i++)
          {
            if(list[i].value === this.targetValueObject.val())
            {
              // 创建时, 不运行回调函数.
              this.getValue(list[i].text, list[i].value, true);
              break;
            }
          }
        }
        else if(this.options.comboboxType === 'dynamic' && this.options.ajaxPreload > 0)
        {
          // 强制转换为数字型
          this.options.ajaxPreload = Number(this.options.ajaxPreload);

          this.preload();
        }
      },
      /*#endregion*/

      /*#region 函数:open()*/
      /*
      * 打开
      */
      open: function()
      {
        if(!this.lockObject)
        {
          this.lockObject = true;

          this.selectedValue = this.targetValueObject.val();

          // 当输入框在隐藏模式下初始化是, 代码获取不到元素宽度, 因此在此处重新设置宽度
          var width = this.targetViewObject.width();
          /*
          if(x.browser.ie)
          {
              // IE
              this.container.css({
                  'vertical-align': 'baseline',
                  'width': width + 'px',
                  'margin-top': (this.options.topOffset - 1) + 'px'
              });
          }
          else if(x.browser.webkit)
          {
              // Chrome
              this.container.css({
                  'position': 'absolute',
                  'width': width + 'px',
                  'top': ((x.page.getElementAbsoluteTop(this.targetViewObject[0]) + x.page.getElementRange(this.targetViewObject[0]).height) - 1) + 'px' 
              });
          }
          else
          {
              // Firefox
              this.container.css({
                  'vertical-align': 'baseline',
                  'width': width + 'px',
                  'height': (Number(this.options.topOffset) < 0 ? -Number(this.options.topOffset) : 0) + 'px',
                  'margin-top': this.options.topOffset + 'px'
              });
          }
          */
          // combobox.container.show();
          this.container.css({ 'display': '' });

          // this.innerContainer.css({ 'width': width + 'px' });

          this.bind();

          // 取消验证组件
          this.targetValueObject.unbind('blur');
          this.targetViewObject.unbind('blur');
        }
      },
      /*#endregion*/

      /*#region 函数:close()*/
      /*
      * 关闭
      */
      close: function()
      {
        // 设置验证组件
        if(this.lockObject && (this.targetViewObject.hasClass('custom-forms-data-required') || this.targetViewObject.hasClass('custom-forms-data-regexp')))
        {
          x.customForm.checkDataInput(this.targetViewObject[0], 1);
        }

        // this.container.slideToggle("slow"); 
        this.lockObject = false;

        // var that = this;

        // this.innerContainer.slideUp("slow", function()
        // {
        //    that.innerContainer.hide();
        // });

        this.innerContainer.css({ 'display': 'none' });
      },
      /*#endregion*/

      /*#region 函数:preload()*/
      /*
      * Ajax方式预加载，可以提高显示速度。
      */
      preload: function()
      {
        // 预加载人员选择列表，加快显示速度。
        var outString = '<?xml version="1.0" encoding="utf-8" ?>';

        outString += '<ajaxStorage>';
        outString += '<action><![CDATA[' + ((typeof (this.options.ajaxMethod) === 'undefined') ? 'getCombobox' : this.options.ajaxMethod) + ']]></action>';
        outString += '<combobox><![CDATA[' + this.name + ']]></combobox>';
        outString += '<selectedValue><![CDATA[' + x.util.htmlEncode(this.targetValueObject.val()) + ']]></selectedValue>';
        if(typeof (this.options.comboboxEmptyItemText) !== 'undefined' && this.options.comboboxEmptyItemText.trim() !== '')
        {
          // 空白选项 全部 
          outString += '<emptyItemText><![CDATA[' + this.options.comboboxEmptyItemText.trim() + ']]></emptyItemText>';
        }
        outString += '<whereClause><![CDATA[' + this.options.whereClause.replace('{0}', this.targetViewObject.val()).replace(/\\/g, '') + ']]></whereClause>';
        outString += '<length>0</length>';
        outString += '</ajaxStorage>';

        x.net.xhr(combobox.options.url, outString, {
          callback: function(response)
          {
            var result = x.toJSON(response);

            var list = result.ajaxStorage;

            var combobox = window[result.combobox];

            if(list.length >= combobox.options.ajaxPreload)
            {
              combobox.targetViewObject.val(list[combobox.options.ajaxPreload - 1].text);
              combobox.targetValueObject.val(list[combobox.options.ajaxPreload - 1].value);

              combobox.selectedText = list[combobox.options.ajaxPreload - 1].text;
              combobox.selectedValue = list[combobox.options.ajaxPreload - 1].value;
            }
          }
        });
      },
      /*#endregion*/

      /*#region 函数:bind()*/
      /*
      * 绑定数据
      */
      bind: function()
      {
        if(this.options.comboboxType === 'static')
        {
          this.show(this.options.show, this.options.list);

          var html = this.innerContainer.html();

          if(html !== '' && html !== '<ul class="' + options.classNamePrefix + '-ul" ></ul>')
          {
            // this.innerContainer.slideDown('slow');
            this.innerContainer.css({ 'display': '' });
          }
        }
        else if(this.options.comboboxType === 'dynamic')
        {
          var outString = '<?xml version="1.0" encoding="utf-8" ?>';

          outString += '<request>';
          outString += '<action><![CDATA[' + ((typeof (this.options.ajaxMethod) === 'undefined') ? 'getCombobox' : this.options.ajaxMethod) + ']]></action>';
          outString += '<combobox><![CDATA[' + this.name + ']]></combobox>';
          outString += '<selectedValue><![CDATA[' + this.targetValueObject.val() + ']]></selectedValue>';
          if(typeof (this.options.comboboxEmptyItemText) !== 'undefined' && this.options.comboboxEmptyItemText.trim() !== '')
          {
            // 空白选项 全部 
            outString += '<emptyItemText><![CDATA[' + this.options.comboboxEmptyItemText.trim() + ']]></emptyItemText>';
          }
          outString += '<whereClause><![CDATA[' + this.options.whereClause.replace('{0}', this.targetViewObject.val()).replace(/\\/g, '') + ']]></whereClause>';
          outString += '<length>0</length>';
          outString += '</request>';

          x.net.xhr(this.options.url, outString, {
            callback: function(response)
            {
              var result = x.toJSON(response);

              var list = result.data;

              if(list.length > 0)
              {
                var combobox = window[result.combobox];

                combobox.show(combobox.options.show, list);

                var html = combobox.innerContainer.html();

                if(html !== '' && html !== '<ul></ul>')
                {
                  // combobox.innerContainer.slideDown('slow');
                  combobox.innerContainer.css({ 'display': '' });
                }
              }
            }
          });
        }
      },
      /*#endregion*/

      /*#region 函数:show(type, list)*/
      show: function(type, list)
      {
        switch(type)
        {
          case 'textAndDescription':
            this.showText(list);
            break;

          case 'text':
            this.showText(list);
            break;

          default:
            this.showText(list);
            break;
        }
      },
      /*#endregion*/

      /*#region 函数:showText(list)*/
      /*
      * 显示
      */
      showText: function(list)
      {
        var outString = '';

        var classNamePrefix = options.classNamePrefix;

        outString += '<ul>';

        for(var i = 0;i < list.length;i++)
        {
          if(list[i] != undefined)
          {
            outString += '<li>';
            // outString += '<div class="' + classNamePrefix + '-list-text"><a href="javascript:window[\'' + this.name + '\'].getValue(\'' + list[i].text + '\',\'' + list[i].value + '\');" >' + this.setTextBlod(list[i].text, this.selectedText) + '</a></div>';
            outString += '<a href="javascript:window[\'' + this.name + '\'].getValue(\'' + list[i].text + '\',\'' + list[i].value + '\');" >' + this.setTextBlod(list[i].text, this.selectedText) + '</a>';
            outString += '</li>';
          }
        }

        outString += '</ul>';

        this.innerContainer.html(outString);

        if(list.length > 6)
        {
          $('.' + classNamePrefix + '-ul').css({
            'height': '132px',
            'overflow': 'auto'
          });
        }
      },
      /*#endregion*/

      /*#region 函数:showTextAndDescription(list)*/
      showTextAndDescription: function(list)
      {
        var outString = '';

        var classNamePrefix = options.classNamePrefix;

        outString += '<ul class="' + classNamePrefix + '-ul" >';

        for(var i = 0;i < list.length;i++)
        {
          if(list[i] != undefined)
          {
            outString += '<li><a href="javascript:window[\'' + this.name + '\'].getValue(\'' + list[i].text + '\',\'' + list[i].value + '\');" >';
            outString += '<div class="' + classNamePrefix + '-description">' + list[i].description + '</div>';
            outString += '<div class="' + classNamePrefix + '-text">' + this.setTextBlod(list[i].text, this.selectedText) + '</div>';
            //outString += '<div class="clear"></div>';
            outString += '</a></li>';
          }
        }

        outString += '</ul>';

        this.innerContainer.html(outString);

        if(list.length > 6)
        {
          $('.' + classNamePrefix + '-ul').css({ 'height': '132px', 'overflow': 'auto' });
        }
      },
      /*#endregion*/

      /*#region 函数:setTextBlod(text, selectedText)*/
      setTextBlod: function(text, selectedText)
      {
        return text.replace(selectedText, '<strong>' + selectedText + '</strong>');
      }
      /*#endregion*/
    };

    combobox.create();

    return combobox;
  }
};// -*- ecoding=utf-8 -*-

/**
* @namespace tabs
* @memberof x.ui.pkg
* @description 页签
*/
x.ui.pkg.tabs = {

  /**
  * -*- 修改自DOMtab, 具体参考以下信息. -*-
  *
  * DOMtab Version 3.1415927
  * Updated March the First 2006
  * written by Christian Heilmann
  * check blog for updates: http://www.wait-till-i.com
  */
  defaults: {
    /** 样式名称前缀 */
    classNamePrefix: 'x-ui-pkg-tabs',
    /** 页签特性是否已加载标识属性名称 */
    tabLoadedAttributeName: 'x-ui-pkg-tabs-loaded',
    tabWrapperClass: 'x-ui-pkg-tabs-wrapper', // class to trigger tabbing
    tabMenuClass: 'x-ui-pkg-tabs-menu', // class of the menus
    tabContainerClass: 'x-ui-pkg-tabs-container', //  class to containers
    tabContainerHeadHiddenClass: 'x-ui-pkg-tabs-container-head-hidden', //  class to containers
    activeClass: 'active', // 当前链接样式
    contentElements: 'div', // elements to loop through
    backToLinks: /#top/, // pattern to check "back to top" links
    printId: 'windowTabsPrintWiew', // id of the print all link
    showAllLinkText: '显示全部内容', // text for the print all link
    prevNextIndicator: 'tabsprevnext', // class to trigger prev and next links
    prevNextClass: 'x-ui-pkg-tabs-prevnext', // class of the prev and next list
    // 上一页链接文本
    prevLabel: '上一页',
    // 下一页链接文本
    nextLabel: '下一页',
    // 上一页默认样式
    prevClass: 'prev',
    // 下一页默认样式                    
    nextClass: 'next'
  },

  /**
  * 页签
  * @description 页签
  * @class Tabs
  * @constructor newTabs
  * @memberof x.ui.pkg.tabs
  * @param {object} options 选项
  */
  newTabs: function(options)
  {
    var options = x.ext({}, x.ui.pkg.tabs.defaults, options || {});

    var tabs = {
      /*
      * 检测地址
      */
      //    checkUrl: function()
      //    {
      //        var id, cur;
      //        var loc = window.location.toString();

      //        loc = /#/.test(loc) ? loc.match(/#(\w.+)/)[1] : '';

      //        if (loc === '') { return; }

      //        var elm = document.getElementById(loc);

      //        if (!elm) { return; }

      //        var parentMenu = elm.parentNode.parentNode.parentNode;

      //        parentMenu.currentSection = loc;

      //        // 链接地址中带有锚点信息时, 不隐藏菜单.
      //        // parentMenu.getElementsByTagName(x.ui.pkg.tabs.contentElements)[0].style.display = 'none';

      //        x.css.remove(parentMenu.getElementsByTagName('a')[0].parentNode, options.activeClass);

      //        var links = parentMenu.getElementsByTagName('a');
      //        for (var i = 0; i < links.length; i++)
      //        {
      //            if (!links[i].getAttribute('href')) { continue; }
      //            if (!/#/.test(links[i].getAttribute('href').toString())) { continue; }

      //            id = links[i].href.match(/#(\w.+)/)[1];

      //            if (id === loc)
      //            {
      //                cur = links[i].parentNode.parentNode;
      //                x.css.add(links[i].parentNode, options.activeClass);
      //                break;
      //            }
      //        }

      //        this.changeTab(elm, 1);
      //        // elm.focus();
      //        cur.currentLink = links[i];
      //        cur.currentSection = loc;
      //    },

      /**
      * 显示全部
      */
      showAll: function(event)
      {
        x.query('#' + options.printId).parentNode.removeChild(x.query('#' + options.printId));

        var tempElement = document.getElementsByTagName('div');

        for(var i = 0;i < tempElement.length;i++)
        {
          if(!x.css.check(tempElement[i], options.tabWrapperClass))
          {
            continue;
          }

          var sec = tempElement[i].getElementsByTagName(x.ui.pkg.tabs.contentElements);

          for(var j = 0;j < sec.length;j++)
          {
            sec[j].style.display = 'block';
          }
        }

        // menu ul    
        tempElement = document.getElementsByTagName('ul');

        for(i = 0;i < tempElement.length;i++)
        {
          if(!x.css.check(tempElement[i], options.prevNextClass)) { continue; }
          tempElement[i].parentNode.removeChild(tempElement[i]);
          i--;
        }

        // title h2
        tempElement = document.getElementsByTagName('h2');

        for(i = 0;i < tempElement.length;i++)
        {
          if(x.css.check(tempElement[i], options.tabContainerHeadHiddenClass))
          {
            x.css.remove(tempElement[i], options.tabContainerHeadHiddenClass);
          }
        }

        x.event.stopPropagation(e);
      },

      /*
      *
      */
      addPrevNext: function(menu)
      {
        var temp;

        // 源代码: var sections = menu.getElementsByTagName(x.ui.pkg.tabs.contentElements);
        var sections = $('.' + options.tabContainerClass);

        for(var i = 0;i < sections.length;i++)
        {
          temp = x.ui.pkg.tabs.createPrevNext();

          if(i === 0)
          {
            temp.removeChild(temp.getElementsByTagName('li')[0]);
          }

          if(i === sections.length - 1)
          {
            temp.removeChild(temp.getElementsByTagName('li')[1]);
          }

          temp.i = i; // h4xx0r!
          temp.menu = menu;
          sections[i].appendChild(temp);
        }
      },

      /*
      *
      */
      removeBackLinks: function(menu)
      {
        var links = menu.getElementsByTagName('a');
        for(var i = 0;i < links.length;i++)
        {
          if(!options.backToLinks.test(links[i].href)) { continue; }
          links[i].parentNode.removeChild(links[i]);
          i--;
        }
      },

      /*
      * 创建Tabs菜单
      */
      createTabMenu: function(elements)
      {
        var me = this;

        var id;

        var menu = elements.getElementsByTagName('ul');

        var thismenu;

        for(var i = 0;i < menu.length;i++)
        {
          if(x.css.check(menu[i], options.tabMenuClass))
          {
            var thismenu = menu[i];
            break;
          }
        }

        if(!thismenu) { return; }

        thismenu.currentSection = '';
        thismenu.currentLink = '';

        var links = thismenu.getElementsByTagName('a');

        for(i = 0;i < links.length;i++)
        {
          if(!/#/.test(links[i].getAttribute('href').toString())) { continue; }

          id = links[i].href.match(/#(\w.+)/)[1];

          if(document.getElementById(id))
          {
            x.event.add(links[i], 'click', function(event) { me.showTab(event) }, false);

            // 源码: links[i].onclick = function () { alert($(this)[0].href ); return false; } // safari hack
            links[i].onclick = function()
            {
              x.call(options.bind);
              return false;
            };

            this.changeTab(document.getElementById(id), 0);
          }
        }

        if(links[0].href.match(/#(\w.+)/) != null)
        {
          id = links[0].href.match(/#(\w.+)/)[1];

          if(document.getElementById(id))
          {
            this.changeTab(document.getElementById(id), 1);
            thismenu.currentSection = id;
            thismenu.currentLink = links[0];
            x.css.add(links[0].parentNode, options.activeClass);
          }
        }
      },

      createPrevNext: function()
      {
        var me = this;

        // this would be so much easier with innerHTML, darn you standards fetish!
        var temp = document.createElement('ul');
        temp.className = options.prevNextClass;
        temp.appendChild(document.createElement('li'));
        temp.getElementsByTagName('li')[0].appendChild(document.createElement('a'));
        temp.getElementsByTagName('a')[0].setAttribute('href', '#');
        temp.getElementsByTagName('a')[0].innerHTML = options.prevLabel;
        temp.getElementsByTagName('li')[0].className = options.prevClass;
        temp.appendChild(document.createElement('li'));
        temp.getElementsByTagName('li')[1].appendChild(document.createElement('a'));
        temp.getElementsByTagName('a')[1].setAttribute('href', '#');
        temp.getElementsByTagName('a')[1].innerHTML = options.nextLabel;
        temp.getElementsByTagName('li')[1].className = options.nextClass;

        x.event.add(temp.getElementsByTagName('a')[0], 'click', function(event) { me.navTabs(event) }, false);
        x.event.add(temp.getElementsByTagName('a')[1], 'click', function(event) { me.navTabs(event) }, false);

        // safari fix
        temp.getElementsByTagName('a')[0].onclick = function() { return false; };
        temp.getElementsByTagName('a')[1].onclick = function() { return false; };

        return temp;
      },

      /*
      *
      */
      navTabs: function(event)
      {
        // x.debug.log('x.ui.pkg.tabs.navTabs(e)');

        var li = x.ui.pkg.tabs.getTarget(event);
        var menu = li.parentNode.parentNode.menu;
        var count = li.parentNode.parentNode.i;

        // 源代码: var sections = menu.getElementsByTagName(x.ui.pkg.tabs.contentElements);
        var sections = $('.' + options.tabContainerClass);

        // x.debug.log(x.ui.pkg.tabs.tabContainerClass);

        var links = menu.getElementsByTagName('a');
        var otherCount = (li.parentNode.className === options.prevClass) ? count - 1 : count + 1;

        sections[count].style.display = 'none';
        x.css.remove(links[count].parentNode, options.activeClass);

        sections[otherCount].style.display = 'block';
        x.css.add(links[otherCount].parentNode, options.activeClass);

        var parent = links[count].parentNode.parentNode;
        parent.currentLink = links[otherCount];
        parent.currentSection = links[otherCount].href.match(/#(\w.+)/)[1];

        x.event.stopPropagation(e);
      },

      /*
      * 更改Tab状态
      */
      changeTab: function(element, status)
      {
        do
        {
          element = element.parentNode;
        }
        while(element.nodeName.toLowerCase() != options.contentElements);

        element.style.display = (status === 0) ? 'none' : 'block';
      },

      /*
      *
      */
      showTab: function(event)
      {
        // x.debug.log('x.ui.pkg.tabs.showTab(e)');

        var tabWrapperTarget = this.getTabWrapperTarget(event);

        // title h2 hidden
        var titleTarget = document.getElementsByTagName('h2');

        for(i = 0;i < titleTarget.length;i++)
        {
          if(!x.css.check(titleTarget[i], options.tabContainerHeadHiddenClass))
          {
            x.css.add(titleTarget[i], options.tabContainerHeadHiddenClass);
          }
        }

        // container hidden
        var sections = x.queryAll('.' + options.tabContainerClass, tabWrapperTarget);

        for(var i = 0;i < sections.length;i++)
        {
          sections[i].style.display = 'none';
        }

        // set target

        var target = this.getTarget(event);

        // x.debug.log('e.target.id:' + target.id);

        if(target.parentNode.parentNode.currentSection !== '')
        {
          this.changeTab(document.getElementById(target.parentNode.parentNode.currentSection), 0);
          x.css.remove(target.parentNode.parentNode.currentLink.parentNode, options.activeClass);
        }

        var id = target.href.match(/#(\w.+)/)[1];

        target.parentNode.parentNode.currentSection = id;
        target.parentNode.parentNode.currentLink = target;
        x.css.add(target.parentNode, options.activeClass);
        this.changeTab(document.getElementById(id), 1);

        document.getElementById(id).focus();
        x.event.stopPropagation(event);
      },

      /*
      * helper methods
      */
      getTabWrapperTarget: function(event, tabWrapperClass)
      {
        var target = x.event.getTarget(event);

        if(!target) { return false; }

        var count = 0;

        while(target.nodeName.toLowerCase() != 'div' && target.className == tabWrapperClass)
        {
          target = target.parentNode;

          if(count++ > 5)
          {
            return false;
          }
        }
        return target;
      },

      getTarget: function(event)
      {
        var target = x.event.getTarget(event);

        if(!target) { return false; }

        if(target.nodeName.toLowerCase() != 'a') { target = target.parentNode; }

        return target;
      },

      create: function(options)
      {
        var tempElements = document.getElementsByTagName('div');

        for(var i = 0;i < tempElements.length;i++)
        {
          if(!x.css.check(tempElements[i], options.tabWrapperClass))
          {
            continue;
          }

          if(x.dom.attr(tempElements[i], options.tabLoadedAttributeName) != '1')
          {
            if(tempElements[i].id == '')
            {
              tempElements[i].id = x.randomText.create(8);
            }

            // x.debug.log('name:' + tempElements[i].id);

            this.createTabMenu(tempElements[i]);

            this.removeBackLinks(tempElements[i]);

            if(x.css.check(tempElements[i], options.prevNextIndicator))
            {
              this.addPrevNext(tempElements[i]);
            }

            // x.ui.pkg.tabs.checkUrl();

            // 加载完毕后,加个tabsLoaded标识,避免重复加载效果.
            x.dom.attr(tempElements[i], options.tabLoadedAttributeName, '1');
          }
        }

        //
        if(x.query('#' + options.printId) && !x.query('#' + options.printId).getElementsByTagName('a')[0])
        {
          var newlink = document.createElement('a');
          newlink.setAttribute('href', '#');
          x.event.add(newlink, 'click', this.showAll, false);
          newlink.onclick = function() { return false; }; // safari hack
          newlink.appendChild(document.createTextNode(x.ui.pkg.tabs.showAllLinkText));
          document.getElementById(options.printId).appendChild(newlink);
        }
      }
    };

    tabs.create(options);

    return tabs;
  }
};
// -*- ecoding=utf-8 -*-

/**
* @namespace tree
* @memberof x.ui.pkg
* @description 树
*/
x.ui.pkg.tree = {

  // 修改自dTree, 具体参考以下信息. -*-
  // dTree - http://www.destroydrop.com/javascripts/tree/

  /**
  * 默认配置信息
  */
  defaults: {
    /** 样式名称默认前缀 */
    classNamePrefix: 'x-ui-pkg-tree',
    /** 图片路径 */
    imagePathRoot: '/resources/images/tree/',
    target: null,
    folderLinks: true,
    useSelection: true,
    useCookies: true,
    // 显示线
    useLines: true,
    uselementIcons: true,
    useStatusText: false,
    // 关闭同级的其他节点
    closeSameLevel: true,
    inOrder: false,
    ajaxMode: false

  },

  // 缓存
  cache: {},

  /**
  * 树视图
  * @description 树视图
  * @class TreeView
  * @constructor newTreeView
  * @memberof x.ui.pkg.tree
  * @param {object} options 选项
  */
  newTreeView: function(options)
  {
    var options = x.ext({}, x.ui.pkg.tree.defaults, options || {});

    // 图片路径
    var imagePath = options.imagePathRoot;

    var treeView =
    {
      // 配置
      options: null,

      icon: {
        root: imagePath + 'tree_icon.gif',
        folder: imagePath + 'folder.gif',
        folderOpen: imagePath + 'folderOpen.gif',
        node: imagePath + 'leaf.gif',
        empty: imagePath + 'empty.gif',
        line: imagePath + 'line.gif',
        join: imagePath + 'join.gif',
        joinBottom: imagePath + 'joinbottom.gif',
        plus: imagePath + 'plus.gif',
        plusBottom: imagePath + 'plusbottom.gif',
        minus: imagePath + 'minus.gif',
        minusBottom: imagePath + 'minusbottom.gif',
        nlPlus: imagePath + 'nolines_plus.gif',
        nlMinus: imagePath + 'nolines_minus.gif'
      },

      ajaxLoadUrl: '',

      /**
      * 设置Ajax模式开关
      */
      setAjaxMode: function(mode)
      {
        this.options.ajaxMode = mode;
        this.options.useCookies = false;
      },

      load: function(url, async, xml)
      {
        this.ajaxLoadUrl = url;

        this.ajaxLoadXml = xml;

        var outString = this.ajaxLoadXml;

        outString = outString.replace('{tree}', this.name);
        outString = outString.replace('{parentId}', 0);

        x.net.xhr(this.ajaxLoadUrl, outString,
            {
              async: async,
              callback: this.load_callback
            });
      },

      post: function(url, async, xml)
      {
        this.ajaxLoadUrl = url;

        x.net.xhr(this.ajaxLoadUrl, xml,
        {
          type: 'POST',
          async: async,
          data: { tree: this.name, parentId: 0 },
          callback: this.load_callback
        });
      },

      load_callback: function(response)
      {
        var result = x.toJSON(response);

        var tree = x.ui.pkg.tree.cache[result.data.tree];

        var parentId = result.data.parentId;

        var childNodes = result.data.childNodes;

        tree.bindChildNodes(childNodes);
      },

      bindChildNodes: function(childNodes)
      {
        for(var i = 0;i < childNodes.length;i++)
        {
          this.add({
            id: childNodes[i].id,
            parentId: childNodes[i].parentId,
            name: childNodes[i].name,
            url: childNodes[i].url,
            title: childNodes[i].title,
            target: childNodes[i].target,
            icon: (typeof (childNodes[i].icon) == 'undefined' ? '' : childNodes[i].icon),
            iconOpen: (typeof (childNodes[i].iconOpen) == 'undefined' ? '' : childNodes[i].iconOpen),
            open: (typeof (childNodes[i].open) == 'undefined' ? false : (childNodes[i].open == 'true' ? true : false)),
            hasChildren: (typeof (childNodes[i].hasChildren) == 'undefined' ? true : (childNodes[i].hasChildren == 'true' ? true : false)),
            ajaxLoading: (typeof (childNodes[i].ajaxLoading) == 'undefined' ? true : (childNodes[i].ajaxLoading == 'true' ? true : false))
          });

          if(!x.isUndefined(childNodes[i].childNodes) && childNodes[i].childNodes.length > 0)
          {
            this.bindChildNodes(childNodes[i].childNodes);
          }
        }
      },

      /*#region 函数:add(options)*/
      /**
      * 添加新的节点到节点数组
      * @method add
      * @memberof x.ui.pkg.tree.newTreeView#
      * @param {string} name 名称
      */
      add: function(options)
      {
        this.nodes[this.nodes.length] = x.ui.pkg.tree.newTreeNode(options);

        // ajaxMode
        if(this.options.ajaxMode && options.hasChildren)
        {
          this.nodes[this.nodes.length - 1].hasChildren = options.hasChildren;
          this.nodes[this.nodes.length - 1].ajaxLoading = options.ajaxLoading;
        }
      },
      /*#endregion*/

      /*#region 函数:openAll()*/
      /**
      * 打开所有的节点
      * @method openAll
      * @memberof x.ui.pkg.tree.newTreeView#
      */
      openAll: function()
      {
        this.toggleAll(true);
      },
      /*#endregion*/

      /*#region 函数:closeAll()*/
      /**
      * 关闭所有的节点
      * @method closeAll
      * @memberof x.ui.pkg.tree.newTreeView#
      */
      closeAll: function()
      {
        this.toggleAll(false);
      },
      /*#endregion*/

      /*#region 函数:toString()*/
      /**
      * 输出树的 html 格式文本
      * @method toString
      * @memberof x.ui.pkg.tree.newTreeView#
      */
      toString: function()
      {
        var outString = '<div id="' + this.name + '" class="' + options.classNamePrefix + '">\n';

        if(document.getElementById)
        {
          if(this.options.useCookies)
          {
            this.selectedNode = this.getSelected();
          }

          outString += this.addNode(this.root);
        }
        else
        {
          outString += 'Browser not supported.';
        }

        outString += '</div>';

        if(!this.selectedFound)
        {
          this.selectedNode = null;
        }

        this.completed = true;

        return outString;
      },
      /*#endregion*/

      // Creates the tree structure

      addNode: function(parentNode)
      {
        var outString = '';

        var i = 0;

        if(this.options.inOrder)
          i = parentNode._atIndex;

        for(i;i < this.nodes.length;i++)
        {
          if(this.nodes[i].parentId == parentNode.id)
          {
            var childNode = this.nodes[i];

            childNode._parent = parentNode;

            childNode._atIndex = i;

            this.setProperty(childNode);

            if(!childNode.target && this.options.target)
              childNode.target = this.options.target;

            if(childNode.hasChildren && !childNode._isSelectedOpen && this.options.useCookies)
              childNode._isSelectedOpen = this.isOpen(childNode.id);

            if(!this.options.folderLinks && childNode.hasChildren)
              childNode.url = null;

            if(this.options.useSelection && childNode.id == this.selectedNode && !this.selectedFound)
            {
              childNode._isSelected = true;

              this.selectedNode = i;

              this.selectedFound = true;
            }

            outString += this.node(childNode, i);

            if(childNode._isLastNode)
              break;
          }

        }
        return outString;
      },

      // Creates the node icon, url and text

      node: function(node, nodeId)
      {
        var outString = '<div class="x-ui-pkg-tree-node" >' + this.indent(node, nodeId);

        if(this.options.uselementIcons)
        {
          if(!node.icon)
          {
            node.icon = (this.root.id == node.parentId) ? this.icon.root : ((node.hasChildren) ? this.icon.folder : this.icon.node);
          }

          if(!node.iconOpen)
          {
            node.iconOpen = (node.hasChildren) ? this.icon.folderOpen : this.icon.node;
          }

          if(this.root.id == node.parentId)
          {

            node.icon = this.icon.root;

            node.iconOpen = this.icon.root;

          }

          outString += '<img id="i' + this.name + nodeId + '" src="' + ((node._isSelectedOpen) ? node.iconOpen : node.icon) + '" alt="" />';

        }

        if(node.url)
        {
          outString += '<a id="s' + this.name + nodeId + '" class="' + ((this.options.useSelection) ? ((node._isSelected ? 'nodeSel' : 'node')) : 'node') + '" href="' + node.url + '"';

          if(node.title) outString += ' title="' + node.title + '"';

          if(node.target) outString += ' target="' + node.target + '"';

          if(this.options.useStatusText) outString += ' onmouseover="window.status=\'' + node.name + '\';return true;" onmouseout="window.status=\'\';return true;" ';

          if(this.options.useSelection && ((node.hasChildren && this.options.folderLinks) || !node.hasChildren))
          {
            outString += ' onclick="javascript:x.ui.pkg.tree.cache[\'' + this.name + '\'].select(' + nodeId + ');"';
          }

          outString += '>';

        }
        else if((!this.options.folderLinks || !node.url) && node.hasChildren && node.parentId != this.root.id)
        {
          outString += '<a href="javascript:x.ui.pkg.tree.cache[\'' + this.name + '\'].toggle(' + nodeId + ');" class="node" >';
        }

        outString += node.name;

        if(node.url || ((!this.options.folderLinks || !node.url) && node.hasChildren)) outString += '</a>';

        outString += '</div>';

        if(node.hasChildren)
        {

          outString += '<div id="d' + this.name + nodeId + '" class="clip" style="display:' + ((this.root.id == node.parentId || node._isSelectedOpen) ? 'block' : 'none') + ';">';

          outString += this.addNode(node);

          outString += '</div>';

        }

        this.arrayIndent.pop();

        return outString;

      },

      // Adds the empty and line icons

      /**
      * 缩进
      */
      indent: function(node, nodeId)
      {
        var outString = '';

        if(this.root.id != node.parentId)
        {

          for(var n = 0;n < this.arrayIndent.length;n++)
          {
            outString += '<img src="' + ((this.arrayIndent[n] == 1 && this.options.useLines) ? this.icon.line : this.icon.empty) + '" alt="" />';
          }

          (node._isLastNode) ? this.arrayIndent.push(0) : this.arrayIndent.push(1);

          if(node.hasChildren)
          {

            outString += '<a href="javascript:x.ui.pkg.tree.cache[\'' + this.name + '\'].toggle(' + nodeId + ');"><img id="j' + this.name + nodeId + '" src="';

            if(!this.options.useLines)
            {
              outString += (node._isSelectedOpen) ? this.icon.nlMinus : this.icon.nlPlus;
            }
            else
            {
              outString += ((node._isSelectedOpen) ? ((node._isLastNode && this.options.useLines) ? this.icon.minusBottom : this.icon.minus) : ((node._isLastNode && this.options.useLines) ? this.icon.plusBottom : this.icon.plus));
            }

            outString += '" alt="" /></a>';

          } else outString += '<img src="' + ((this.options.useLines) ? ((node._isLastNode) ? this.icon.joinBottom : this.icon.join) : this.icon.empty) + '" alt="" />';

        }

        return outString;

      },

      // Checks if a node has any children and if it is the last sibling

      /** 
      * 设置节点属性
      * @method setProperty
      * @memberof x.ui.pkg.tree.newTreeView#
      * @param {x.ui.tree.newTreeNode} node 树节点
      */
      setProperty: function(node)
      {
        var lastId;

        for(var i = 0;i < this.nodes.length;i++)
        {
          if(this.nodes[i].parentId == node.id)
          {
            // 判断节点是否有子节点
            node.hasChildren = true;
          }

          if(this.nodes[i].parentId == node.parentId)
          {
            lastId = this.nodes[i].id;
          }
        }

        node._isLastNode = (lastId == node.id) ? true : false;

        // x.debug.log(node);
      },

      /*#region 函数:getSelectedNodeId()*/
      /** 
      * 获取当前选择的节点标识
      * @method getSelectedNodeId
      * @memberof x.ui.pkg.tree.newTreeView#
      */
      getSelectedNodeId: function()
      {
        return this.selectedNodeId;
      },
      /*#endregion*/

      /*#region 函数:getSelectedNode()*/
      /** 
      * 获取当前选择的节点对象
      * @method getSelectedNode
      * @memberof x.ui.pkg.tree.newTreeView#
      */
      getSelectedNode: function()
      {
        return (this.selectedNodeId == '') ? null : this.nodes[this.selectedNodeId];
      },
      /*#endregion*/

      /*#region 函数:getNodeByNodeId(nodeId)*/
      /** 
      * 根据节点标识, 获取节点对象
      * @method getSelectedNode
      * @memberof x.ui.pkg.tree.newTreeView#
      * @param {string} nodeId 节点标识
      */
      getNodeByNodeId: function(nodeId)
      {
        for(var i = 0;i < this.nodes.length;i++)
        {
          if(this.nodes[i].id == nodeId)
          {
            return this.nodes[i];
          }
        }

        return null;
      },
      /*#endregion*/

      /*#region 函数:removeNode(nodeId)*/
      /** 
      * 移除一个节点
      * @method removeNode
      * @memberof x.ui.pkg.tree.newTreeView#
      * @param {string} nodeId 节点标识
      */
      removeNode: function(nodeId)
      {
        var index = -1;

        for(var i = 0;i < this.nodes.length;i++)
        {
          if(this.nodes[i].id == nodeId)
          {
            index = i;
          }
        }

        if(index != -1)
        {
          for(var i = index + 1;i < this.nodes.length;i++)
          {
            var nodeTemp = this.nodes[i];

            this.nodes[i] = null;

            this.nodes[i - 1] = nodeTemp;
          }

          this.nodes.pop();
        }
      },
      /*#endregion*/

      /*#region 函数:getSelected()*/
      /** 
      * 获取当前选着的节点
      * @method getSelected
      * @memberof x.ui.pkg.tree.newTreeView#
      */
      getSelected: function()
      {
        var selectedNode = x.cookies.query('cs' + this.name);

        return (selectedNode) ? selectedNode : null;
      },
      /*#endregion*/

      // Highlights the selected node
      // s -> select

      select: function(id)
      {
        if(!this.options.useSelection) return;

        var childNode = this.nodes[id];

        if(childNode.hasChildren && !this.options.folderLinks) return;

        if(this.selectedNode != id)
        {

          if(this.selectedNode || this.selectedNode == 0)
          {
            eOld = document.getElementById("s" + this.name + this.selectedNode);

            if(eOld != null)
            {
              eOld.className = "node";
            }
          }

          eNew = document.getElementById("s" + this.name + id);

          eNew.className = "nodeSel";

          this.selectedNode = id;

          // jiqiliang nodearr 下标
          this.selectedNodeId = id;

          if(this.options.useCookies) x.cookies.add('cs' + this.name, childNode.id);

        }
      },

      /**
      * 打开或关闭
      */
      toggle: function(index)
      {
        if(this.nodes[index].ajaxLoading)
        {
          this.nodes[index].ajaxLoading = false;

          var outString = this.ajaxLoadXml;

          outString = outString.replace('{tree}', this.name);
          outString = outString.replace('{parentId}', this.nodes[index].id);

          x.net.xhr(this.ajaxLoadUrl, outString,
              {
                async: false,
                callback: this.toggle_callback
              });
        }
        else
        {
          var childNode = this.nodes[index];

          this.setStatus(!childNode._isSelectedOpen, index, childNode._isLastNode);

          childNode._isSelectedOpen = !childNode._isSelectedOpen;

          if(this.options.closeSameLevel)
          {
            this.closeLevel(childNode);
          }

          if(this.options.useCookies)
          {
            this.updateCookie();
          }
        }
      },

      toggle_callback: function(response)
      {
        var result = x.toJSON(response);

        var tree = x.ui.pkg.tree.cache[result.data.tree];

        var parentId = result.data.parentId;

        var childNodes = result.data.childNodes;

        var index = -1;

        var hasChildren = false;

        for(var i = 0;i < tree.nodes.length;i++)
        {
          if(tree.nodes[i].id == parentId)
          {
            index = i;

            hasChildren = (childNodes.length > 0) ? true : false;

            tree.nodes[i].hasChildren = hasChildren;
            tree.nodes[i].icon = (tree.nodes[i].hasChildren) ? tree.icon.folder : tree.icon.node;

            break;
          }
        }

        tree.bindChildNodes(childNodes);

        $(document.getElementById(tree.name)).parent().html(tree.toString());

        if(index > -1 && hasChildren)
        {
          tree.toggle(index);
        }
      },

      /*#region 函数:toggleAll(status)*/
      /**
      * 展开或关闭全部节点
      * @method toggleAll
      * @memberof x.ui.pkg.tree.newTreeView#
      * @param {string} status 状态 true 张开
      */
      toggleAll: function(status)
      {

        for(var i = 0;i < this.nodes.length;i++)
        {
          if(this.nodes[i].hasChildren && this.nodes[i].parentId != this.root.id)
          {
            this.setStatus(status, i, this.nodes[i]._isLastNode)

            this.nodes[i]._isSelectedOpen = status;
          }
        }

        if(this.options.useCookies)
        {
          this.updateCookie();
        }
      },
      /*#endregion*/

      // Opens the tree to a specific node

      openTo: function(nId, bSelect, bFirst)
      {
        if(!bFirst)
        {
          for(var n = 0;n < this.nodes.length;n++)
          {
            if(this.nodes[n].id == nId)
            {
              nId = n;
              break;
            }
          }
        }

        var childNode = this.nodes[nId];

        if(childNode.parentId == this.root.id || !childNode._parent) { return; }

        childNode._isSelectedOpen = true;

        childNode._isSelected = bSelect;

        if(this.completed && childNode.hasChildren) this.setStatus(true, childNode._atIndex, childNode._isLastNode);

        if(this.completed && bSelect) this.select(childNode._atIndex);

        else if(bSelect) this._sn = childNode._atIndex;

        this.openTo(childNode._parent._atIndex, false, true);

      },

      // Closes all nodes on the same level as certain node
      //
      closeLevel: function(node)
      {
        for(var i = 0;i < this.nodes.length;i++)
        {
          if(this.nodes[i].parentId == node.parentId && this.nodes[i].id != node.id && this.nodes[i].hasChildren)
          {
            this.setStatus(false, i, this.nodes[i]._isLastNode);

            this.nodes[i]._isSelectedOpen = false;

            this.closeAllChildren(this.nodes[i]);
          }
        }
      },

      /*#region 函数:closeAllChildren(node)*/
      /**
      * 关闭所有的节点信息
      * @method closeAllChildren
      * @memberof x.ui.pkg.tree.newTreeView#
      * @param {string} name 名称
      */
      closeAllChildren: function(node)
      {
        for(var i = 0;i < this.nodes.length;i++)
        {
          if(this.nodes[i].parentId == node.id && this.nodes[i].hasChildren)
          {
            if(this.nodes[i]._isSelectedOpen) this.setStatus(false, i, this.nodes[i]._isLastNode);

            this.nodes[i]._isSelectedOpen = false;

            this.closeAllChildren(this.nodes[i]);
          }
        }
      },
      /*#endregion*/

      // Change the status of a node(open or closed)

      /*#region 函数:setStatus(status, index, bottom)*/
      /**
      * 设置状态
      * @method setStatus
      * @memberof x.ui.pkg.tree.newTreeView#
      * @param {string} status
      * @param {string} index
      * @param {string} bottom
      */
      setStatus: function(status, index, bottom)
      {
        var elementDiv = document.getElementById('d' + this.name + index);

        var elementJoin = document.getElementById('j' + this.name + index);

        if(this.options.uselementIcons)
        {
          elementIcon = document.getElementById('i' + this.name + index);

          elementIcon.src = (status) ? this.nodes[index].iconOpen : this.nodes[index].icon;
        }

        elementJoin.src = (this.options.useLines) ?

    ((status) ? ((bottom) ? this.icon.minusBottom : this.icon.minus) : ((bottom) ? this.icon.plusBottom : this.icon.plus)) :

    ((status) ? this.icon.nlMinus : this.icon.nlPlus);

        elementDiv.style.display = (status) ? 'block' : 'none';

      },
      /*#endregion*/

      /*
      // [Cookie] Clears a cookie
    
      clearCookie: function()
      {
    
      var now = new Date();
    
      var yesterday = new Date(now.getTime() - 1000 * 60 * 60 * 24);
    
      this.setCookie('co' + this.name, 'cookieValue', yesterday);
    
      this.setCookie('cs' + this.name, 'cookieValue', yesterday);
    
      },
          
      // [Cookie] Sets value in a cookie
    
      setCookie: function(cookieName, cookieValue, expires, path, domain, secure)
      {
    
      document.cookie =
    
      escape(cookieName) + '=' + escape(cookieValue)
    
      + (expires ? '; expires=' + expires.toGMTString() : '')
    
      + (path ? '; path=' + path : '')
    
      + (domain ? '; domain=' + domain : '')
    
      + (secure ? '; secure' : '');
    
      },
    
      // [Cookie] Gets a value from a cookie
    
      getCookie: function(cookieName)
      {
    
      var cookieValue = '';
    
      var posName = document.cookie.indexOf(escape(cookieName) + '=');
    
      if (posName != -1)
      {
    
      var posValue = posName + (escape(cookieName) + '=').length;
    
      var endPos = document.cookie.indexOf(';', posValue);
    
      if (endPos != -1) cookieValue = unescape(document.cookie.substring(posValue, endPos));
    
      else cookieValue = unescape(document.cookie.substring(posValue));
    
      }
    
      return (cookieValue);
    
      },
      */

      // [Cookie] Returns ids of open nodes as a string

      updateCookie: function()
      {
        var outString = '';

        for(var n = 0;n < this.nodes.length;n++)
        {
          if(this.nodes[n]._isSelectedOpen && this.nodes[n].parentId != this.root.id)
          {
            if(outString) outString += '.';

            outString += this.nodes[n].id;
          }

        }

        x.cookies.add('co' + this.name, outString);
      },

      // [Cookie] Checks if a node id is in a cookie

      /*#region 函数:isOpen(id)*/
      /**
      * 判断节点是否开启, 需要设置选项 useCookies = true
      * @method isOpen
      * @memberof x.ui.pkg.tree.newTreeView#
      * @param {string} id 节点标识
      */
      isOpen: function(id)
      {
        var list = x.cookies.query('co' + this.name).split('.');

        for(var i = 0;i < list.length;i++)
        {
          if(list[i] == id) return true;
        }

        return false;
      },
      /*#endregion*/

      /*#region 函数:create(options)*/
      /**
      * 初始化树对象
      * @method create
      * @memberof x.ui.pkg.tree.newTreeView#
      * @param {object} options 选项
      */
      create: function(options)
      {
        this.options = options;

        this.name = options.name;

        this.nodes = [];

        this.arrayIndent = [];

        this.root = x.ui.pkg.tree.newTreeNode({ id: -1 });

        this.selectedNode = null;

        this.selectedFound = false;

        this.selectedNodeId = 0;

        this.completed = false;

        this.icon = {
          root: options.imagePathRoot + 'tree_icon.gif',
          folder: options.imagePathRoot + 'folder.gif',
          folderOpen: options.imagePathRoot + 'folderOpen.gif',
          node: options.imagePathRoot + 'leaf.gif',
          empty: options.imagePathRoot + 'empty.gif',
          line: options.imagePathRoot + 'line.gif',
          join: options.imagePathRoot + 'join.gif',
          joinBottom: options.imagePathRoot + 'joinbottom.gif',
          plus: options.imagePathRoot + 'plus.gif',
          plusBottom: options.imagePathRoot + 'plusbottom.gif',
          minus: options.imagePathRoot + 'minus.gif',
          minusBottom: options.imagePathRoot + 'minusbottom.gif',
          nlPlus: options.imagePathRoot + 'nolines_plus.gif',
          nlMinus: options.imagePathRoot + 'nolines_minus.gif'
        };
      }
      /*#endregion*/
    };

    treeView.create(options);

    // 存为全局变量
    x.ui.pkg.tree.cache[options.name] = treeView;

    return treeView;
  },

  /*#region 类:newTreeNode(options)*/
  /**
  * 树节点
  * @description 树节点
  * @class TreeNode
  * @constructor newTreeNode
  * @memberof x.ui.tree
  * @param {object} options 选项<br />
  * 可选键值范围: 
  * <table class="param-options" >
  * <thead>
  * <tr><th>名称</th><th>类型</th><th class="last" >描述</th></tr>
  * </thead>
  * <tbody>
  * <tr><td class="name" >id</td><td>string</td><td>节点标识</td></tr> 
  * <tr><td class="name" >parentId</td><td>string</td><td>父级节点标识</td></tr>
  * <tr><td class="name" >name</td><td>string</td><td>名称</td></tr>
  * <tr><td class="name" >url</td><td>string</td><td>链接地址</td></tr>
  * </tbody>
  * </table>
  */
  newTreeNode: function(options)
  {
    return x.ext({
      id: '',
      parentId: '-1',
      name: '',
      url: '',
      title: '',
      target: '_self',
      icon: '',
      iconOpen: '',
      hasChildren: false,
      ajaxLoading: false,
      _isSelectedOpen: false,
      _isSelected: false,
      _isLastNode: false,
      _atIndex: 0,
      _parent: undefined
    }, options || {});
  }
  /*#endregion*/
}
