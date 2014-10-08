// -*- ecoding=utf-8 -*-

var path = require('path'),
    fs = require('fs');

var ignoreFiles = ['README.md', 'change-marks.js', 'SR.strings', '.csproj'],
    ignoreDirectories = ['temp', 'bin', 'obj', 'Properties'];

// 需要修改的标记
var marks = [
{
    before: 'using CommandLine',
    after: 'using X3Platform.CommandLine'
},
{
    before: 'namespace CommandLine',
    after: 'namespace X3Platform.CommandLine'
}];

// 修改标记
changeMarks(
{
    // 目录
    path: '.',
    // 还原操作
    restore: false
});

/**
* 修改文件的标记
*/
function changeMarks(options)
{
    var files = [],
        directories = [];

    var list = fs.readdirSync(options.path);

    list.forEach(function(item)
    {
        var tmpPath = options.path + '/' + item;

        var stats = fs.statSync(tmpPath);

        if (stats.isDirectory())
        {
            directories[directories.length] = tmpPath;
        }
        else if (stats.isFile())
        {
            files[files.length] = tmpPath;
        }
    });

    files.forEach(function(file)
    {
        if (ignoreFiles.length > 0 && file.match(new RegExp('(' + ignoreFiles.join('|') + ')$', 'i')))
        {
            console.log('[ignore][file] ' + file);
            return;
        }

        var content = fs.readFileSync(file, "utf8");

        var changed = false;

        marks.forEach(function(mark)
        {
            if (options.restore)
            {
                // 逆向操作
                if (content.indexOf(mark.after) > -1)
                {
                    changed = true;
                    content = content.replace(new RegExp(mark.after, 'g'), mark.before);
                }
            }
            else
            {
                // 正向操作
                if (content.indexOf(mark.before) > -1)
                {
                    changed = true;
                    content = content.replace(new RegExp(mark.before, 'g'), mark.after);
                }
            }
        });

        if (changed)
        {
            fs.writeFile(file, content);

            console.log('[file] ' + file + ' changed');
        }
    });

    directories.forEach(function(directory)
    {
        if (ignoreDirectories.length > 0 && path.basename(directory).match('(' + ignoreDirectories.join('|') + ')$'))
        {
            console.log('[ignore][directory] ' + directory);
            return;
        }

        console.log('[directory]:' + directory);

        changeMarks(
        {
            path: directory,
            restore: options.restore
        });
    });
}