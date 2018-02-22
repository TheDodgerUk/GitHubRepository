using ConsoleLogOutput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private int m_PrevKeyCount = 0;
        private int m_PrevShownCount = 0;
        private int m_PrevSelectedTabIndex = -1;
        private Messages m_Messages;
        private TcpServer m_TcpServer;

        public MainWindow()
        {
            InitializeComponent();
            m_Messages              = new Messages();
            m_TcpServer             = new TcpServer(ref m_Messages);

            DispatcherTimer timer   = new DispatcherTimer();
            timer.Interval          = TimeSpan.FromSeconds(0.1);
            timer.Tick += CheckForNewData;
            timer.Start();


        }

        private void CheckForNewData(object sender, EventArgs e)
        {
            var lKeyList = m_Messages.m_MessageDictionaryList.Getkeys();
            if (lKeyList.Count == 0)
            {
                return;
            }

            // assign the tab name if its empty, only empty if nothing been selected before
            if (m_PrevSelectedTabIndex == -1)
            {
                m_PrevSelectedTabIndex = 0;
                m_TextBox.Text = "";
            }

            if (m_PrevKeyCount != lKeyList.Count)
            {
                m_TabControl.Items.Clear();

                for(int i = 0; i < lKeyList.Count; ++i)
                { 
                    m_TabControl.Items.Add(lKeyList[i]);
                }
            }
            
            if (m_PrevSelectedTabIndex == m_TabControl.SelectedIndex || m_PrevKeyCount == 0)
            {
                string lName = m_Messages.m_MessageDictionaryList.Getkeys()[m_PrevSelectedTabIndex];
                var lList   = m_Messages.m_MessageDictionaryList.GetList(lName);
                for (int i = m_PrevShownCount; i < lList.Count; i++)
                {
                    if (m_PrevSelectedTabIndex == 0)// ALL
                    {
                        m_ListView.Items.Add(lList[i].m_Tab.PadRight(GetLongestKeyLength()) + "\t" + lList[i].m_Time + " " + lList[i].m_Output);
                    }
                    else
                    {
                        m_ListView.Items.Add(lList[i].m_Time + " " + lList[i].m_Output);
                    }
                }
                m_PrevShownCount = lList.Count;
            }
            
            m_PrevKeyCount = lKeyList.Count;

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            m_TcpServer.StopClass();
        }

        private void m_TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            m_PrevSelectedTabIndex = m_TabControl.SelectedIndex;
            if (m_PrevSelectedTabIndex >= 0 && m_PrevSelectedTabIndex < m_Messages.m_MessageDictionaryList.Getkeys().Count)
            {
                string lName = m_Messages.m_MessageDictionaryList.Getkeys()[m_PrevSelectedTabIndex];
                var lList = m_Messages.m_MessageDictionaryList.GetList(lName);

                m_TextBox.Text = "";
                m_ListView.Items.Clear();
                foreach (Message lItems in lList)
                {
                    if (m_PrevSelectedTabIndex == 0)// ALL
                    {
                        m_ListView.Items.Add(lItems.m_Tab.PadRight(GetLongestKeyLength()) + "\t" + lItems.m_Time + " " + lItems.m_Output);
                    }
                    else
                    {
                        m_ListView.Items.Add(lItems.m_Time + " " + lItems.m_Output);
                    }
                }
                m_PrevShownCount = lList.Count;
            }
        }

        private int GetLongestKeyLength(int lPadding = 5)
        {
            var lList = m_Messages.m_MessageDictionaryList.Getkeys();
            int lLongest = 0;
            foreach(string lItem in lList)
            {
                if(lItem.Length > lLongest)
                {
                    lLongest = lItem.Length;
                }
            }
            return lLongest + lPadding;
        }

        private void m_ListView_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            m_TextBox.Text = "";
            string lName = m_Messages.m_MessageDictionaryList.Getkeys()[m_PrevSelectedTabIndex];
            var lList = m_Messages.m_MessageDictionaryList.GetList(lName);

            if (m_ListView.SelectedIndex >= 0 && m_ListView.SelectedIndex < lList.Count)
            {   // safety , sometime listBox1.SelectedIndex was -1 
                int lLength = lList[m_ListView.SelectedIndex].m_Callstack.Length;
                m_TextBox.Text = lList[m_ListView.SelectedIndex].m_Callstack;
            }
        }
    }
}
