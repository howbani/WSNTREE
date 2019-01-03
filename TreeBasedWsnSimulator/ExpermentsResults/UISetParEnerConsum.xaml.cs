using System;
using System.Windows;
using TreeBasedWsnSimulator.Properties;
using TreeBasedWsnSimulator.ui;

namespace TreeBasedWsnSimulator.ExpermentsResults 
{
    /// <summary>
    /// Interaction logic for UISetParEnerConsum.xaml
    /// </summary>
    public partial class UISetParEnerConsum : Window
    {
        MainWindow _MainWindow;
        public UISetParEnerConsum(MainWindow __MainWindow_)
        {
            InitializeComponent();
            _MainWindow = __MainWindow_;

            for (int i = 20; i <= 1000; i = i + 20)
            {
                comb_simuTime.Items.Add(i);

            }
            comb_simuTime.Text = "300";

            comb_packet_rate.Items.Add("0.001");
            comb_packet_rate.Items.Add("0.01");
            comb_packet_rate.Items.Add("0.1");
            comb_packet_rate.Items.Add("0.5");
            for (int i = 1; i <= 5; i++)
            {
                comb_packet_rate.Items.Add(i);
            }

            comb_packet_rate.Text = "0.1";

        }


        private void btn_ok_Click(object sender, RoutedEventArgs e)
        {
            //PublicParamerters.UpdateORReveryXseconds = Convert.ToInt16(comp_update_EDC_EVER.Text);
          //  Settings.Default.DrawPacketsLines = Convert.ToBoolean(chk_drawrouts.IsChecked);
          //  Settings.Default.KeepLogs = Convert.ToBoolean(chk_save_logs.IsChecked);
            Settings.Default.StopeWhenFirstNodeDeid = Convert.ToBoolean(chk_stope_when_first_node_deis.IsChecked);

            if (!Settings.Default.StopeWhenFirstNodeDeid)
            {
                int stime = Convert.ToInt16(comb_simuTime.Text);
                double packper = Convert.ToDouble(comb_packet_rate.Text);
                _MainWindow.stopSimlationWhen = stime;
                _MainWindow.SendPackectPerSecond(packper);
              //  _MainWindow.RandomDeplayment(0);
                _MainWindow.TimerCounter.Start();
            }
            else
            {
                int stime = 100000000;
                double packper = Convert.ToDouble(comb_packet_rate.Text);
                _MainWindow.stopSimlationWhen = stime;
                _MainWindow.SendPackectPerSecond(packper);
              //  _MainWindow.RandomDeplayment(0);
                _MainWindow.TimerCounter.Start();
            }

            this.Close();

        }


        private void chk_stope_when_first_node_deis_Checked(object sender, RoutedEventArgs e)
        {
            comb_simuTime.IsEnabled = false;
        }

        private void chk_stope_when_first_node_deis_Unchecked(object sender, RoutedEventArgs e)
        {
            comb_simuTime.IsEnabled = true;
        }






    }
}
