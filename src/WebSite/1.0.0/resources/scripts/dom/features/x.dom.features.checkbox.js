/**
* form feature  : checkbox(复选框)
*
* require       : x.js, x.dom.js
*/
x.dom.features.checkbox = {

  /**
  * 绑定
  */
  bind: function(inputName)
  {
    var input = x.dom('#' + inputName);

    var callback = x.isUndefined(input.attr('x-dom-callback')) ? undefined : input.attr('x-dom-callback');

    input.on('change', function(event)
    {
      this.value = this.checked ? 1 : 0;

      // 执行回调函数
      if(callback)
      {
        x.call(callback);
      }
    });

    if(input.size() == 1)
    {
      input[0].checked = input.val() == 1 ? true : false;
    }
  }
};