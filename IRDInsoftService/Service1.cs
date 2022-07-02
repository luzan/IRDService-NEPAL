using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace IRDInsoftService
{
    public partial class InsoftIRD : ServiceBase
    {
        public InsoftIRD()
        {
            InitializeComponent();
        }
        public void OnDebug()
        {
            OnStart(null);
        }
        protected override void OnStart(string[] args)
        {
            if (CheckInterent.CheckForInternetConnection())
            {
                IRDDAta irdData = new IRDDAta();
                irdData.Prepare(null);
            }
            else
            {
                Console.WriteLine("No Interent Connection. Service couldn't be started.");
            }
            
        }

        protected override void OnStop()
        {
        }
    }
}
