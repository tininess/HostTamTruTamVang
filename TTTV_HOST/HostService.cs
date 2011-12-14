using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Description;
using TTTV_Host.Contract;
using System.Threading;
using System.Diagnostics;

namespace TTTV_Host
{
    public partial class HostService : Form
    {
        public HostService()
        {
            InitializeComponent();
        }
        static TraceSource m_WcfTrace = new TraceSource("System.ServiceModel");
        public Uri Address;
        //Type contractType = typeof(ILevel2Service);
        Type contractType = typeof(ILevel2Service);
        private ServiceHost host;
        private bool IsStarted = false;
        private bool BehaviorEnable = true;
        private void button1_Click(object sender, EventArgs e)
        {
            CreateHost();
        }
        public void CreateHost()
        {
                if (IsStarted)
            {
                host.Close();
                IsStarted = false;
            }
            else
            {
                using (host)
                {
                    Address = new Uri(textBox1.Text);
                    host = new ServiceHost(typeof(Level2Service), Address);
                    ServiceMetadataBehavior behavior = new ServiceMetadataBehavior();
                    ServiceDebugBehavior debug = host.Description.Behaviors.Find<ServiceDebugBehavior>();
                    behavior.HttpGetEnabled = BehaviorEnable;
                    host.Description.Behaviors.Add(behavior);
                    // host.AddServiceEndpoint(typeof(IMetadataExchange), new BasicHttpBinding(), "MEX");
                    if (checkBox1.Checked == true)
                    {
                        host.AddServiceEndpoint(contractType, new BasicHttpBinding(), "LoginInformation");
                    }
                    else
                    {
                        host.AddServiceEndpoint(contractType, new NetMsmqBinding(), "NetMsmqBinding");
                    }
                    // if not found - add behavior with setting turned on 
                    if (debug == null)
                    {
                        host.Description.Behaviors.Add(
                             new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });
                    }
                    else
                    {
                        // make sure setting is turned ON
                        if (!debug.IncludeExceptionDetailInFaults)
                        {
                            debug.IncludeExceptionDetailInFaults = true;
                        }
                    }
                }
               
            host.Open();
            progressBar1.Value = 0;
            timer1.Start();
            lblstatus.Text = "Host Option is running......";
            button2.Enabled = true;
            button1.Enabled = false;
         
            new Thread(new ThreadStart(TestTrace)).Start();
            IsStarted = true;
       
            }
        }
        public void StopHost()
        {
            if (IsStarted)
            {
                progressBar1.Value = 0;
                timer1.Start();
                lblstatus.Text = "Host stopped!";
                host.Close();
            }
            else
            {
                lblstatus.Text = "Host is not running";
            }
           button1.Enabled = true;
           button2.Enabled = false;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                textBox1.Text = "http://localhost:8888/";
                checkBox2.Checked = false;
            }
            else
            {
                textBox1.Text = "net.msmq://localhost:8888/";
                checkBox2.Checked = true;
            }
    

        }

        private void HostService_Load(object sender, EventArgs e)
        {
            checkBox1.Checked = true;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                
                textBox1.Text = "net.msmq://localhost:8888/";
                checkBox1.Checked = false;
            }
            else
            {
                textBox1.Text = "http://localhost:8888/";
                checkBox1.Checked = true;
            }
        }
    
        public void TestTrace()
        {

            TraceSource wcfTrace = new TraceSource("System.ServiceModel");
            Guid currentActivity = Trace.CorrelationManager.ActivityId;
            Guid newActivity = Guid.NewGuid();
            Guid newActivity2 = Guid.NewGuid();

            Guid newActivity3 = Guid.NewGuid();


            wcfTrace.TraceEvent(TraceEventType.Start, 100, "[+] Method entered");


            wcfTrace.TraceEvent(TraceEventType.Information, 101, "[+] In current activity");

            wcfTrace.TraceEvent(TraceEventType.Start, 100, "[+] Still in current activity");


            wcfTrace.TraceEvent(TraceEventType.Information, 102, "[+] Still in current activity");

            wcfTrace.TraceTransfer(0, "[+] Start Level 1 activity", newActivity);



            Trace.CorrelationManager.ActivityId = newActivity;

            wcfTrace.TraceEvent(TraceEventType.Information, 103, "[+] Hello from Level 1");

            wcfTrace.TraceEvent(TraceEventType.Warning, 104, "[+] Level 1");
            wcfTrace.TraceEvent(TraceEventType.Error, 103, "[+] Level 1");
            wcfTrace.TraceEvent(TraceEventType.Critical, 105, "[+] Level 1");
            wcfTrace.TraceTransfer(0, "[+] Start Level 2 activity", newActivity2);



            Trace.CorrelationManager.ActivityId = newActivity2;


            wcfTrace.TraceEvent(TraceEventType.Information, 102, "[+] Hello from Level 2");

            wcfTrace.TraceEvent(TraceEventType.Information, 103, "[+] Level 2");

            wcfTrace.TraceTransfer(0, "[+] Jump back to level 1", newActivity);

            Trace.CorrelationManager.ActivityId = newActivity;

            wcfTrace.TraceEvent(TraceEventType.Transfer, 101, "[+] Hello from Level 1");

            wcfTrace.TraceEvent(TraceEventType.Information, 102, "[+] Level 1");

            wcfTrace.TraceTransfer(0, "[+] Jump back to current",

            currentActivity);


            Trace.CorrelationManager.ActivityId = currentActivity;

            wcfTrace.TraceEvent(TraceEventType.Information, 103, "[+] Again in current activity");

            wcfTrace.TraceEvent(TraceEventType.Stop, 101, "[+] Method exited");

        }

        private void button2_Click(object sender, EventArgs e)
        {
            StopHost();
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            progressBar1.Value += 20;
            if (progressBar1.Value >= progressBar1.Maximum)
            {
                timer1.Stop();

            }
        } 
    }
}
