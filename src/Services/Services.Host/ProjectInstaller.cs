using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;

namespace X3Platform.Services
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }
    }
}