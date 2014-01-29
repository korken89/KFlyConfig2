using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KFly_Config_2._0
{
    public partial class KFlyConfig : Form
    {
        public KFlyConfig()
        {
            InitializeComponent();
        }

        private void selFirmware_Click(object sender, EventArgs e)
        {
            OpenFileDialog odf = new OpenFileDialog();
            odf.Filter = "Binary File|*.bin";

            if (odf.ShowDialog() == DialogResult.OK)
                firmwarePath.Text = odf.FileName;
        }

        private void KFlyConfig_Load(object sender, EventArgs e)
        {
            baudrateCombo.SelectedIndex = 0;

            ch1_role.SelectedIndex = 0;
            ch2_role.SelectedIndex = 0;
            ch3_role.SelectedIndex = 0;
            ch4_role.SelectedIndex = 0;
            ch5_role.SelectedIndex = 0;
            ch6_role.SelectedIndex = 0;
            ch7_role.SelectedIndex = 0;
            ch8_role.SelectedIndex = 0;

            ch1_updRate.SelectedIndex = 0;
            ch5_updRate.SelectedIndex = 0;
            ch7_updRate.SelectedIndex = 0;
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

        private void ch1_role_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ch1_role.SelectedIndex == 0)
            {
                ch1_min.Enabled = false;
                ch1_center.Enabled = false;
                ch1_max.Enabled = false;
                ch1_type.Enabled = false;
            }
            else
            {
                ch1_min.Enabled = true;
                ch1_center.Enabled = true;
                ch1_max.Enabled = true;
                ch1_type.Enabled = true;
            }
        }

        private void ch2_role_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ch2_role.SelectedIndex == 0)
            {
                ch2_min.Enabled = false;
                ch2_center.Enabled = false;
                ch2_max.Enabled = false;
                ch2_type.Enabled = false;
            }
            else
            {
                ch2_min.Enabled = true;
                ch2_center.Enabled = true;
                ch2_max.Enabled = true;
                ch2_type.Enabled = true;
            }
        }

        private void ch3_role_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ch3_role.SelectedIndex == 0)
            {
                ch3_min.Enabled = false;
                ch3_center.Enabled = false;
                ch3_max.Enabled = false;
                ch3_type.Enabled = false;
            }
            else
            {
                ch3_min.Enabled = true;
                ch3_center.Enabled = true;
                ch3_max.Enabled = true;
                ch3_type.Enabled = true;
            }
        }

        private void ch4_role_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ch4_role.SelectedIndex == 0)
            {
                ch4_min.Enabled = false;
                ch4_center.Enabled = false;
                ch4_max.Enabled = false;
                ch4_type.Enabled = false;
            }
            else
            {
                ch4_min.Enabled = true;
                ch4_center.Enabled = true;
                ch4_max.Enabled = true;
                ch4_type.Enabled = true;
            }
        }

        private void ch5_role_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ch5_role.SelectedIndex == 0)
            {
                ch5_min.Enabled = false;
                ch5_center.Enabled = false;
                ch5_max.Enabled = false;
                ch5_type.Enabled = false;
            }
            else
            {
                ch5_min.Enabled = true;
                ch5_center.Enabled = true;
                ch5_max.Enabled = true;
                ch5_type.Enabled = true;
            }
        }

        private void ch6_role_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ch6_role.SelectedIndex == 0)
            {
                ch6_min.Enabled = false;
                ch6_center.Enabled = false;
                ch6_max.Enabled = false;
                ch6_type.Enabled = false;
            }
            else
            {
                ch6_min.Enabled = true;
                ch6_center.Enabled = true;
                ch6_max.Enabled = true;
                ch6_type.Enabled = true;
            }

        }

        private void ch7_role_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ch7_role.SelectedIndex == 0)
            {
                ch7_min.Enabled = false;
                ch7_center.Enabled = false;
                ch7_max.Enabled = false;
                ch7_type.Enabled = false;
            }
            else
            {
                ch7_min.Enabled = true;
                ch7_center.Enabled = true;
                ch7_max.Enabled = true;
                ch7_type.Enabled = true;
            }
        }

        private void ch8_role_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ch8_role.SelectedIndex == 0)
            {
                ch8_min.Enabled = false;
                ch8_center.Enabled = false;
                ch8_max.Enabled = false;
                ch8_type.Enabled = false;
            }
            else
            {
                ch8_min.Enabled = true;
                ch8_center.Enabled = true;
                ch8_max.Enabled = true;
                ch8_type.Enabled = true;
            }
        }
    }
}
