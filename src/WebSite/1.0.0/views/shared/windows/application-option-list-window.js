x.register('x.ui.windows');

x.ui.windows.newApplicationOptionListWindow = function(name, options)
{
    return x.ext(x.ui.windows.newWindow(name, options), {

        /*#region 函数:bindOptions(options)*/
        bindOptions: function(options)
        {
            this.applicationId = options.applicationId;

            this.tableHeader = options.tableHeader;
            this.tableContainer = options.tableContainer;
            this.tableFooter = options.tableFooter;
        },
        /*#endregion*/

        /*#region 函数:getObjectsView(me, list, maxCount)*/
        /*
        * 创建对象列表的视图
        */
        getObjectsView: function(me, list, maxCount)
        {
            var outString = '';

            var counter = 0;

            var classNameValue = '';

            outString += '<div class="table-freeze-body">';
            outString += '<table class="table table-striped">';
            outString += '<tbody>';

            x.each(list, function(index, node)
            {
                var name = node.name.replace(/./g, '-');

                outString += '<tr id="' + name + '" >';
                outString += '<td id="' + name + '-label-view" style="width:200px" ><a href="javascript:' + me.name + '.toggle(\'' + name + '\');" >' + node.label + '</a></td>';
                outString += '<td id="' + name + '-value-view" style="word-break:break-all;" >' + node.value + '</td>';
                outString += '<td id="' + name + '-status-view" style="width:60px" >' + x.app.setColorStatusView(node.status) + '</td>';
                outString += '<td style="width:100px" >' + node.modifiedDateView + '</td>';
                outString += '<td style="width:40px" ><a href="javascript:' + me.name + '.toggle(\'' + name + '\');" title="编辑" ><i class="fa fa-pencil-square-o"></i></a></td>';
                outString += '</tr>';

                outString += '<tr id="' + name + '-detail" class="' + classNameValue + '" style="display:none" >';
                outString += '<td colspan="5" >';
                outString += me.getObjectView(me, node);
                outString += '</td>';
                outString += '</tr>';

                counter++;
            });

            // 补全

            while(counter < maxCount)
            {
                outString += '<tr >';
                outString += '<td colspan="6" >&nbsp;</td>';
                outString += '</tr>';

                counter++;
            }

            outString += '</tbody>';
            outString += '</table>';
            outString += '</div>';

            return outString;
        },
        /*#endregion*/

        /*#region 函数:getObjectView(me, param)*/
        /*
        * 创建单个对象的视图
        */
        getObjectView: function(me, param)
        {
            var outString = '';

            var name = param.name.replace(/./g, '-');

            outString += '<table class="table table-borderless" >';

            outString += '<tr>';
            outString += '<td class="table-body-text" ><span class="required-text" >显示名称<span></td>';
            outString += '<td class="table-body-input" >' + param.label + '</td>';
            outString += '</tr>';

            outString += '<tr>';
            outString += '<td class="table-body-text" ><span class="required-text" >名称<span></td>';
            outString += '<td class="table-body-input" >';
            outString += '<input id="' + name + '-applicationId" name="' + name + '-applicationId" type="hidden" value="' + x.isUndefined(param.applicationId, '') + '" x-dom-data-type="value" /> ';
            outString += '<input id="' + name + '-label" name="' + name + '-label" type="hidden" value="' + x.isUndefined(param.label, '') + '" x-dom-data-type="value" /> ';
            outString += '<input id="' + name + '-description" name="' + name + '-description" type="hidden" x-dom-data-type="value" value="' + x.isUndefined(param.description, '') + '" /> ';
            outString += '<input id="' + name + '-isInternal" name="' + name + '-isInternal" type="hidden" x-dom-data-type="value" value="' + x.isUndefined(param.isInternal, '0') + '" /> ';
            outString += '<input id="' + name + '-orderId" name="' + name + '-orderId" type="hidden" x-dom-data-type="value" value="' + x.isUndefined(param.orderId, '') + '" /> ';
            outString += '<input id="' + name + '-remark" name="' + name + '-remark" type="hidden" x-dom-data-type="value" value="' + x.isUndefined(param.remark, '') + '" /> ';
            outString += '<input id="' + name + '-name" name="' + name + '-name" type="text" x-dom-data-type="value" x-dom-forms-data-required="1" x-dom-data-verify-warning="必须填写【名称】。" class="form-control" style="width:420px;" value="' + x.isUndefined(param.name, '') + '" ' + ((param.name === '') ? '' : ' readonly="readonly" ') + '/>';
            outString += '</td>';
            outString += '</tr>';

            outString += '<tr>';
            outString += '<td class="table-body-text" ><span class="required-text" >值<span></td>';
            outString += '<td class="table-body-input" >';
            outString += '<textarea id="' + name + '-value" name="' + name + '-value" type="text" x-dom-data-type="value" class="form-control" style="width:420px;height:40px;" >' + x.isUndefined(param.value, '') + '</textarea>';
            if(param.description != '')
            {
                outString += '<div class="alert alert-info" style="width:420px; margin-top:8px; margin-bottom:0;" >';
                outString += '<span >(*)' + x.isUndefined(param.description, '') + '</span>';
                outString += '</div>';
            }
            outString += '</td>';
            outString += '</tr>';

            outString += '<tr>';
            outString += '<td class="table-body-text" >启用</td>';
            outString += '<td class="table-body-input" ><input id="' + name + '-status" name="' + name + '-status" type="checkbox" x-dom-data-type="value" x-dom-feature="checkbox" value="' + x.isUndefined(param.status, '1') + '" /></td>';
            outString += '</tr>';

            outString += '<tr>';
            outString += '<td class="table-body-text" ></td>';
            outString += '<td class="table-body-input" >';
            outString += '<button class="btn btn-default" onclick="' + me.name + '.save(\'' + name + '\');" style="margin-right:10px;" >确定</button>';
            outString += '<button class="btn btn-default" onclick="' + me.name + '.toggle(\'' + name + '\');" >取消</button>';
            outString += '</td>';
            outString += '</tr>';

            outString += '</table>';

            return outString;
        },
        /*#endregion*/

        /*#region 函数:findAll()*/
        /*
        * 查询
        */
        findAll: function()
        {
            var whereClause = ' T.ApplicationId =  ##' + this.applicationId + '##  ORDER BY OrderId, Name ';

            var outString = '<?xml version="1.0" encoding="utf-8" ?>';

            outString += '<request>';
            outString += '<whereClause>' + whereClause + '</whereClause>';
            outString += '</request>';

            var me = this;

            x.net.xhr('/api/application.option.findAll.aspx', outString, {
                waitingType: 'mini',
                waitingMessage: '正在查询数据，请稍后......',
                callback: function(response)
                {
                    var result = x.toJSON(response);

                    var containerHtml = me.getObjectsView(me, result.data, 100);

                    me.tableContainer.html(containerHtml);

                    x.dom.features.bind();

                    masterpage.resize();
                }
            });
        },
        /*#endregion*/

        /*#region 函数:toggle(name)*/
        /*
        * 查看单个记录的信息
        */
        toggle: function(name)
        {
            if($('#' + name).css('display') === 'none')
            {
                $('#' + name).css({ 'display': '' });
                $('#' + name + '-detail').css({ 'display': 'none' });
            }
            else
            {
                $('#' + name).css({ 'display': 'none' });
                $('#' + name + '-detail').css({ 'display': '' });
            }

            masterpage.resize();
        },
        /*#endregion*/

        /*#region 函数:checkObject(name)*/
        /*
        * 检测对象的必填数据
        */
        checkObject: function(name)
        {
            if($(name + '-name').val() == '')
            {
                alert('必须填写【名称】。');
                return false;
            }

            if($(name + '-value').val() == '')
            {
                alert('必须填写【值】。');
                return false;
            }

            return true;
        },
        /*#endregion*/

        /*#region 函数:save(name)*/
        save: function(name)
        {
            // 1.数据检测
            if(this.checkObject(name))
            {
                // 2.发送数据
                var outString = '<?xml version="1.0" encoding="utf-8" ?>';

                outString += '<request>';
                outString += '<applicationId><![CDATA[' + $('#' + name + '-applicationId').val() + ']]></applicationId>';
                outString += '<name><![CDATA[' + $('#' + name + '-name').val() + ']]></name>';
                outString += '<label><![CDATA[' + $('#' + name + '-label').val() + ']]></label>';
                outString += '<description><![CDATA[' + $('#' + name + '-description').val() + ']]></description>';
                outString += '<value><![CDATA[' + $('#' + name + '-value').val() + ']]></value>';
                outString += '<isInternal><![CDATA[' + $('#' + name + '-isInternal').val() + ']]></isInternal>';
                outString += '<orderId><![CDATA[' + $('#' + name + '-orderId').val() + ']]></orderId>';
                outString += '<status><![CDATA[' + $('#' + name + '-status').val() + ']]></status>';
                outString += '<remark><![CDATA[' + $('#' + name + '-remark').val() + ']]></remark>';
                outString += '</request>';

                var me = this;

                x.net.xhr('/api/application.option.save.aspx', outString, {
                    // popResultValue: 1,
                    callback: function(response)
                    {
                        $('#' + name + '-value-view').html($('#' + name + '-value').val());
                        $('#' + name + '-status-view').html(x.app.setColorStatusView($('#' + name + '-status').val()));

                        me.toggle(name);
                    }
                });
            }
        },
        /*#endregion*/

        /*#region 函数:create()*/
        /*
        * 页面加载事件
        */
        create: function()
        {
            this.findAll();
        }
        /*#endregion*/
    });
};

x.ui.windows.getApplicationOptionListWindow = function(name, options)
{
    var name = x.getFriendlyName(location.pathname + '-window-' + name);

    var internalWindow = x.ui.windows.newApplicationOptionListWindow(name, options);

    // 加载界面、数据、事件
    internalWindow.load(options);

    // 绑定到Window对象
    window[name] = internalWindow;

    return internalWindow;
};
