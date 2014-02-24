using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using KFly.Communication;
using System.IO.Ports;
 


namespace KFly
{
    public partial class KFlyConfig : Form
    {
      
        public KFlyConfig()
        {
            InitializeComponent();
         }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            //_usbHandler.WndProc(ref m);
            if (m.Msg == USBHandler.WM_DEVICECHANGE)
            {
                if (m.WParam.ToInt32() == USBHandler.DBT_DEVNODES_CHANGED)
                {
                    comportsCombo.BeginInvoke((MethodInvoker)delegate
                    {
                        reloadPorts();
                    });
                }
            }
        }

#region INIT
        private List<NumericUpDown> ch_mins;
        private List<NumericUpDown> ch_maxs;
        private List<NumericUpDown> ch_centers;
        private List<ComboBox> ch_roles;
        private List<ComboBox> ch_types;

        private void KFlyConfig_Load(object sender, EventArgs e)
        {
            InitCommunicationGUI();
            SubscribeToCommunication();

            ch_mins = new List<NumericUpDown>
            { 
                ch1_min, ch2_min, ch3_min, ch4_min, 
                ch5_min, ch6_min, ch7_min, ch8_min 
            };
            ch_maxs = new List<NumericUpDown>
            { 
                ch1_max, ch2_max, ch3_max, ch4_max, 
                ch5_max, ch6_max, ch7_max, ch8_max 
            };
            ch_centers = new List<NumericUpDown>
            { 
                ch1_center, ch2_center, ch3_center, ch4_center, 
                ch5_center, ch6_center, ch7_center, ch8_center 
            }; 
            ch_roles = new List<ComboBox>
            { 
                ch1_role, ch2_role, ch3_role, ch4_role, 
                ch5_role, ch6_role, ch7_role, ch8_role 
            };
            ch_types = new List<ComboBox>
            { 
                ch1_type, ch2_type, ch3_type, ch4_type, 
                ch5_type, ch6_type, ch7_type, ch8_type 
            }; 
            foreach (ComboBox cb in ch_roles)
            {
                cb.SelectedIndex = 0;
            }

            ch1_updRate.SelectedIndex = 0;
            ch5_updRate.SelectedIndex = 0;
            ch7_updRate.SelectedIndex = 0;
        }

        private void InitCommunicationGUI()
        {
            reloadPorts();
        }

        private void reloadPorts()
        {
            foreach (string str in SerialPort.GetPortNames())
            {
                comportsCombo.Items.Add(str);
                if (str == Properties.Settings.Default.ComPort)
                    comportsCombo.SelectedItem = str;
            }
        }

        private void SubscribeToCommunication()
        {
            //Log window
            Telemetry.Subscribe(KFlyCommandType.All, (KFlyCommand cmd) =>
            {
                infoBox.BeginInvoke((MethodInvoker)delegate
                {
                    infoBox.AppendText(cmd.ToString());
                });
            });

            //Firmware version info textbox
            Telemetry.Subscribe(KFlyCommandType.GetFirmwareVersion, (GetFirmwareVersion cmd) =>
            {
                firmwareVersion.BeginInvoke((Action)(() =>
                {
                    firmwareVersion.Text = cmd.Version;
                }));
            });

            //Bootloader version info textbox
            Telemetry.Subscribe(KFlyCommandType.GetBootloaderVersion, (GetBootLoaderVersion cmd) =>
            {
                bootloaderVersion.BeginInvoke((Action)(() =>
                {
                    bootloaderVersion.Text = cmd.Version;
                }));
            });
        }

#endregion //INIT

        private void selFirmware_Click(object sender, EventArgs e)
        {
            OpenFileDialog odf = new OpenFileDialog();
            odf.Filter = "Binary File|*.bin";

            if (odf.ShowDialog() == DialogResult.OK)
                firmwarePath.Text = odf.FileName;
        }

        private void btnCalStart_Click(object sender, EventArgs e)
        {
            btnCalCancel.Enabled = true;
            btnCalSample.Enabled = true;
            btnCalStart.Enabled = false;
        }

        private void btnCalCancel_Click(object sender, EventArgs e)
        {
            btnCalCancel.Enabled = false;
            btnCalSample.Enabled = false;
            btnCalStart.Enabled = true;
        }

        private void chkParamsOTF_CheckedChanged(object sender, EventArgs e)
        {
            if (chkParamsOTF.Checked == true)
            {
                DialogResult result1 = MessageBox.Show("Warning!\n\nAre you sure you want to change controller gains on the fly?\n\nThis can be dangerous if you don't know what you are doing and destabalize the system, resulting in a crash and/or personal/property damage!",
                                                        "Important Question",
                                                        MessageBoxButtons.YesNo,
                                                        MessageBoxIcon.Exclamation);

                if (result1 != System.Windows.Forms.DialogResult.Yes)
                {
                    chkParamsOTF.Checked = false;
                }
            }
        }

        private void ch1_updRate_SelectedIndexChanged(object sender, EventArgs e)
        {
            string val = ch1_updRate.Items[ch1_updRate.SelectedIndex].ToString();
            ch2_updRate.Text = val;
            ch3_updRate.Text = val;
            ch4_updRate.Text = val;
        }

        private void ch5_updRate_SelectedIndexChanged(object sender, EventArgs e)
        {
            string val = ch5_updRate.Items[ch5_updRate.SelectedIndex].ToString();
            ch6_updRate.Text = val;
        }

        private void ch7_updRate_SelectedIndexChanged(object sender, EventArgs e)
        {
            string val = ch7_updRate.Items[ch7_updRate.SelectedIndex].ToString();
            ch8_updRate.Text = val;
        }

        private void connectBtn_Click(object sender, EventArgs e)
        {
            Telemetry.Link.PortName = comportsCombo.Text;
            Telemetry.Link.BaudRate = baudrateCombo.Text;
            Properties.Settings.Default.ComPort = comportsCombo.Text;
            Properties.Settings.Default.Baudrate = baudrateCombo.Text;
            Properties.Settings.Default.Save();

           // var id = Log.NewRow(Log


            if (Telemetry.Link.OpenPort())
            {
                disconnectBtn.Enabled = true;
                connectBtn.Enabled = false;
                Telemetry.SendAsync(new GetFirmwareVersion());
                Telemetry.SendAsync(new GetBootLoaderVersion());
            }
            else
            {
                infoBox.AppendText("Failed connection!");
            }
        }

        private void disconnectBtn_Click(object sender, EventArgs e)
        {
            Telemetry.Link.ClosePort();
            disconnectBtn.Enabled = false;
            connectBtn.Enabled = true;
        }


        private void ch_role_SelectedIndexChanged(object sender, EventArgs e)
        {
            var index = ch_roles.IndexOf(sender as ComboBox);
            ch_mins[index].Enabled = (ch_roles[index].SelectedIndex != 0);
            ch_maxs[index].Enabled = (ch_roles[index].SelectedIndex != 0);
            ch_centers[index].Enabled = (ch_roles[index].SelectedIndex != 0);
            ch_types[index].Enabled = (ch_roles[index].SelectedIndex != 0);
        }

        private ControllerData GetAttitudeControllerData()
        {
            ControllerData cd = new ControllerData();
            cd.Pitch.PGain = (float)attitude_pkp.Value;
            cd.Pitch.IGain = (float)attitude_pki.Value;
            cd.Pitch.ILimit = (float)attitude_pil.Value;
            cd.Roll.PGain = (float)attitude_rkp.Value;
            cd.Roll.IGain = (float)attitude_rki.Value;
            cd.Roll.ILimit = (float)attitude_ril.Value;
            cd.Yaw.PGain = (float)attitude_ykp.Value;
            cd.Yaw.IGain = (float)attitude_yki.Value;
            cd.Yaw.ILimit = (float)attitude_yil.Value;
            return cd;
        }

        private ControllerData GetRateControllerData()
        {
            ControllerData cd = new ControllerData();
            cd.Pitch.PGain = (float)rate_pkp.Value;
            cd.Pitch.IGain = (float)rate_pki.Value;
            cd.Pitch.ILimit = (float)rate_pil.Value;
            cd.Roll.PGain = (float)rate_rkp.Value;
            cd.Roll.IGain = (float)rate_rki.Value;
            cd.Roll.ILimit = (float)rate_ril.Value;
            cd.Yaw.PGain = (float)rate_ykp.Value;
            cd.Yaw.IGain = (float)rate_yki.Value;
            cd.Yaw.ILimit = (float)rate_yil.Value;
            return cd;
        }

        private void saveAttitudeControllerButton_Click(object sender, EventArgs e)
        {
         /*   _telLink.SendDataWithAck(
                new SetControllerData(KFlyCommandType.SetAttitudeControllerData, 
                    GetAttitudeControllerData()),
                100);*/
        }

  

    }
}
