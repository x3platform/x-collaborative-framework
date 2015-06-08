// ��װ����
module.exports = function(grunt)
{
  // ��������
  grunt.initConfig(
  {
    // ��ȡ������Ϣ
    pkg: grunt.file.readJSON('package.json'),
    // �ϲ��ļ�
    concat:
    {
      options:
      {
        separator: '' //separates scripts
      },
      'WebSite-1.0.0':
      {
        files:
        {
          // �ϲ������, ��߼����ٶ�
          'src/WebSite/1.0.0/resources/javascript/x-client-api-ui.zh-cn.js': [
            'src/WebSite/1.0.0/resources/javascript/x-client-api-ui-core.zh-cn.js',
            'src/WebSite/1.0.0/resources/javascript/ui/pkg/x.ui.pkg.combobox.js',
            'src/WebSite/1.0.0/resources/javascript/ui/pkg/x.ui.pkg.tabs.js',
            'src/WebSite/1.0.0/resources/javascript/ui/pkg/x.ui.pkg.tree.js'
          ]
        }
      }
    },
    uglify:
    {
      'WebSite-1.0.0':
      {
        files:
        {
          'src/WebSite/1.0.0/resources/javascript/x-client-api-ui.zh-cn.min.js': ['src/WebSite/1.0.0/resources/javascript/x-client-api-ui.zh-cn.js']
        }
      }
    },
    less:
    {
      'WebSite-1.0.0':
      {
        files:
        {
          'src/WebSite/1.0.0/resources/styles/default/style.css': 'src/WebSite/1.0.0/resources/styles/default/style.less',
          'src/WebSite/1.0.0/resources/styles/mobile/style.css': 'src/WebSite/1.0.0/resources/styles/mobile/style.less'
        }
      }
    },

    // CSS ѹ��
    cssmin:
    {
      // WebSite 1.0.0 ��ʽ
      'WebSite-1.0.0':
      {
        files:
        {
          'src/WebSite/1.0.0/resources/styles/normalize.min.css': [
            "src/WebSite/1.0.0/resources/styles/normalize.css"
          ],
          'src/WebSite/1.0.0/resources/styles/default/style.min.css': [
            "src/WebSite/1.0.0/resources/styles/default/style.css"
          ],

          'src/WebSite/1.0.0/views/templates/themes/default/styles/styles.min.css': [
            "src/WebSite/1.0.0/views/templates/themes/default/styles/style.css",
            "src/WebSite/1.0.0/views/templates/themes/default/styles/header.css",
            "src/WebSite/1.0.0/views/templates/themes/default/styles/footer.css",
            "src/WebSite/1.0.0/views/templates/themes/default/styles/default.css",
            "src/WebSite/1.0.0/views/templates/themes/default/styles/form.css",
            "src/WebSite/1.0.0/views/templates/themes/default/styles/table.css",
            "src/WebSite/1.0.0/views/templates/themes/default/styles/text.css",
            "src/WebSite/1.0.0/views/templates/themes/default/styles/mixed.css",
            "src/WebSite/1.0.0/views/templates/themes/default/styles/window.css",

            "src/WebSite/1.0.0/views/templates/themes/default/styles/x.ui.accordion.css",
            "src/WebSite/1.0.0/views/templates/themes/default/styles/x.ui.customize.css",
            "src/WebSite/1.0.0/views/templates/themes/default/styles/x.ui.contextMenu.css",
            "src/WebSite/1.0.0/views/templates/themes/default/styles/x.ui.dialog.css",
            "src/WebSite/1.0.0/views/templates/themes/default/styles/x.ui.editor.css",
            "src/WebSite/1.0.0/views/templates/themes/default/styles/x.ui.gantt.css",
            "src/WebSite/1.0.0/views/templates/themes/default/styles/x.ui.mask.css",
            "src/WebSite/1.0.0/views/templates/themes/default/styles/x.ui.tooltip.css",
            "src/WebSite/1.0.0/views/templates/themes/default/styles/x.ui.upload.css",
            "src/WebSite/1.0.0/views/templates/themes/default/styles/x.ui.workflow.css",
            "src/WebSite/1.0.0/views/templates/themes/default/styles/x.ui.pkg.calendar.css",
            "src/WebSite/1.0.0/views/templates/themes/default/styles/x.ui.pkg.combobox.css",
            "src/WebSite/1.0.0/views/templates/themes/default/styles/x.ui.pkg.menu.css",
            "src/WebSite/1.0.0/views/templates/themes/default/styles/x.ui.pkg.tabs.css",
            "src/WebSite/1.0.0/views/templates/themes/default/styles/x.ui.pkg.tree.css"
          ]
        }
      }
    }
  });

  // Load grunt tasks from NPM packages
  require("load-grunt-tasks")(grunt);

  // �����Զ�������
  grunt.loadTasks("build/tasks");

  // �������
  grunt.loadNpmTasks('grunt-contrib-less');
  grunt.loadNpmTasks('grunt-contrib-cssmin');
  grunt.loadNpmTasks('grunt-contrib-watch');

  // Ĭ������
  grunt.registerTask('default', ['concat:WebSite-1.0.0', 'uglify:WebSite-1.0.0', 'cssmin:WebSite-1.0.0']);
};