window.onload = function()
{
    var options =
    {
        imageBox: '.imageBox',
        thumbBox: '.thumbBox',
        spinner: '.spinner',
        imgSrc: x.dom('#avatar_180x180').val()
    }
    var cropper = new cropbox(options);
    document.querySelector('#file').addEventListener('change', function()
    {
        var reader = new FileReader();
        reader.onload = function(e)
        {
            options.imgSrc = e.target.result;
            cropper = new cropbox(options);
        }
        reader.readAsDataURL(this.files[0]);
        this.files = [];
    })
    document.querySelector('#btnCrop').addEventListener('click', function()
    {
        var img = cropper.getDataURL();

        document.querySelector('.cropped').innerHTML = '<img src="' + img + '">';

        var outString = '<?xml version="1.0" encoding="utf-8"?>';

        outString += '<request>';
        outString += '<data><![CDATA[' + img.replace('data:image/png;base64,', '') + ']]></data>';
        outString += '</request>';

        x.net.xhr('/api/membership.account.avatar.upload.aspx', outString, {
            waitingMessage: i18n.net.waiting.saveTipText,
            popCorrectValue: 1,
            callback: function(response)
            {
                // x.page.refreshParentWindow();
                // x.page.close();
            }
        });
    })
    document.querySelector('#btnZoomIn').addEventListener('click', function()
    {
        cropper.zoomIn();
    })
    document.querySelector('#btnZoomOut').addEventListener('click', function()
    {
        cropper.zoomOut();
    })
};