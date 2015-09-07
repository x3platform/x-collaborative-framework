/**
* form feature  : integer(数字输入框)
*
* require       : x.js, x.dom.js
*/
x.dom.features.integer = {

  /*
  * 检测整数规则
  */
  bind: function(inputName)
  {
    var input = $(document.getElementById(inputName));

    // 绑定相关事件
    input.on('keydown', function(event)
    {
      var event = x.event.getEvent(event);

      // x.debug.log(event.keyCode);

      //只允许输入一次-号
      if(this.value.indexOf('-') > -1 && (event.keyCode == 189 || event.keyCode == 109))
      {
        x.event.stopPropagation(event);
      }

      // 8：退格键、46：delete、37-40： 方向键
      // 48-57：小键盘区的数字、96-105：主键盘区的数字
      // 189、109：小键盘区和主键盘区的负号

      // 考虑小键盘上的数字键 
      // 只允许按Delete键和Backspace键
      if(!((event.keyCode >= 48 && event.keyCode <= 57) // 小键盘区的数字
          || (event.keyCode >= 96 && event.keyCode <= 105) // 主键盘区的数字
          || (event.keyCode == 189 || event.keyCode == 109) // 小键盘区和主键盘区的负号
          || (event.keyCode == 8)  // 退格
          || (event.keyCode == 46) // Del
          || (event.keyCode == 27) // ESC
          || (event.keyCode == 37) // 左
          || (event.keyCode == 39) // 右
          || (event.keyCode == 16) // Shift
          || (event.keyCode == 9)  // Tab
      ))
      {
        x.event.stopPropagation(event);
      }
    });

    input.on('keyup', function()
    {
      // 去除右边的负号
      if(this.value.length > 1 && this.value.lastIndexOf('-') == this.value.length - 1)
      {
        this.value = x.string.rtrim(this.value, '-');
      }

      if(this.value !== '' && this.value.exists(x.expressions.rules['non-integer']))
      {
        this.value = x.expressions.formatInteger(this.value, 1);
      }
    });
  }
};