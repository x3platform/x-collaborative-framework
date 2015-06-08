/**
* dom feature  : email(邮箱地址输入框)
*
* require       : x.js, x.dom.js
*/
x.dom.features.email = {

  /**
  * 绑定
  */
  bind: function(inputName)
  {
    /*
    * 检测邮箱规则
    */

    var input = x.dom('#' + inputName);

    // 绑定相关事件
    input.on('keydown', function(event)
    {
      var event = x.event.getEvent(event);

      if(this.value.indexOf('@') > -1 && event.keyCode == 50)
      {
        x.event.stopPropagation();
      }

      // 只允许@符号
      if(event.shiftKey && event.keyCode !== 50)
      {
        x.event.stopPropagation();
      }

      // 8：退格键、46：delete、37-40： 方向键
      // 48-57：小键盘区的数字、96-105：主键盘区的数字
      // 65-90：主键盘区的数字
      // 50：主键盘区的@符号
      // 110、190：小键盘区和主键盘区的小数
      // 189、109：小键盘区和主键盘区的负号

      // 考虑小键盘上的数字键 
      // 只允许按Delete键和Backspace键
      if(!((event.keyCode >= 48 && event.keyCode <= 57) // 小键盘区的数字
          || (event.keyCode >= 96 && event.keyCode <= 105) // 主键盘区的数字
          || (event.keyCode >= 65 && event.keyCode <= 90) // 主键盘区的字母
          || (event.keyCode == 110 || event.keyCode == 190) // 小键盘区和主键盘区的小数
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
        x.event.stopPropagation();
      }
    });

    input.on('blur', function()
    {
      if(this.value != '' && !x.expressions.isEmail(this.value))
      {
        x.msg('请填写正确的Email地址，例如【your-name@domain.com】。');
        this.focus();
      }
    });
  }
};