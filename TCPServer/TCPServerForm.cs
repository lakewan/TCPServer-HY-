﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;


using System.Net;
using System.Net.Sockets;
using System.Text;

using System.Windows.Forms;
using System.Threading;
using dayLogFiles; 
using DataFormat;

using SockServer;


namespace TCPServer
{
    /// <summary>
    /// UDP服务端表单  
    /// </summary>  
   
   // public delegate void ReceiveDataHandler(SocketState state);
   // public delegate void WriteTextLogHandle(string message);
   // public delegate void OnlineChangeHandler(int onlines, EndPoint client);
   // public delegate void ErrorHandler(string error, EndPoint client);
    public delegate void TxtShowMessageDelegate(string message);



    public partial class TCPServerForm : Form
    {
        private static SockServer.AsynSocketTCPServer _tcpServer;
        private static EndPoint remoteClient;
        private static string dataPort;
        private static string dataUser;
        private static string dataPassword;

        private bool _isListening;
        // public event ReceiveDataHandler ReceivedDataEvent;
        //public event ErrorHandler ErrorEvent;
        //public event WriteTextLogHandle WriteTextLogEvent;
        private bool bRecvEncodeHex = true;
        private bool bAutoReply = false;
        private bool bSendEncodeHex = true;


        public TCPServerForm()
        {

            InitializeComponent();
            txtPort.Text = "10010";
            txtServerIP.Text = "localhost";
            txtDataIP.Text = "localhost";
            txtDataPort.Text = "3306";
            txtDataUser.Text = "root";
            txtDataPassword.Text = "111111";
            txtDB.Text = "pbsxtweb_udp";
            radRecvAscii.Checked = false;
            radRecvHex.Checked = true;
            chkAutoReply.Checked = false;
            radSendASC.Checked = false;
            radSendHex.Checked = true;
        }


        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (!_isListening)
                {
                    _tcpServer = new SockServer.AsynSocketTCPServer(IPAddress.Any, Convert.ToInt32(txtPort.Text));
                    _tcpServer.ShowMsgEvent += new ShowMessageDelegate(lsbShowMessage);
                    _tcpServer.Start();
                    _isListening = true;
                    


                }
                else
                {
                    string msg = DateTime.Now.ToString() + " 服务器" + " IP:" + txtServerIP.Text + ",Port:" + txtPort.Text + " 已绑定监听中，请 ";
                    EveryDayLog.Write(msg);
                }


            }

            catch (Exception err)
            {
                _isListening = false;
                string msg = DateTime.Now.ToString() + " 服务器" + " IP:" + txtServerIP.Text + ",Port:" + txtPort.Text + " 启动失败： " + err.Message;
                EveryDayLog.Write(msg);
            }

        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            _tcpServer.Stop();

            /*
            _isListening = false;    //停止监听实例
            string msg = DateTime.Now.ToString() + " 服务器关闭监听";
            _tcpServer.Close();
            lsbRecvMsg.Items.Add(msg);
            EveryDayLog.Write(msg);
            */

        }


        // 向ListBox中添加文本
       
        private void lsbShowMessage(string message)
        {
            if (lsbRecvMsg.InvokeRequired)
            {
                TxtShowMessageDelegate showMessageDelegate = lsbShowMessage;
                lsbRecvMsg.Invoke(showMessageDelegate, new object[] { message });
            }
            else
            {
                lsbRecvMsg.Items.Add(message);
            }
        }

        // 清空指定ListBox中的文本
        delegate void ResetTextBoxDelegate(ListBox lstbox);
        private void ResetTextBox(ListBox lstbox)
        {
            if (lstbox.InvokeRequired)
            {
                ResetTextBoxDelegate resetTextBoxDelegate = ResetTextBox;
                lstbox.Invoke(resetTextBoxDelegate, new object[] { lstbox });
            }
            else
            {
                lstbox.Items.Clear();
            }
        }
        private void lsbRecvMsg_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = this.lsbRecvMsg.IndexFromPoint(e.Location);
            if (lsbRecvMsg.SelectedItems.Count == 0)
            {

                MessageBox.Show("未选中显示项"); //执行双击事件

            }
            else
            {
                MessageBox.Show(this.lsbRecvMsg.SelectedItem.ToString()); //执行双击事件
            }
           
        }

        private void radRecvHex_CheckedChanged(object sender, EventArgs e)
        {
            bRecvEncodeHex = true;
            
        }

        private void radRecvAscii_CheckedChanged(object sender, EventArgs e)
        {
            bRecvEncodeHex = false;
        }

        private void radSendHex_CheckedChanged(object sender, EventArgs e)
        {
            bSendEncodeHex = true;
        }

        private void radSendASC_CheckedChanged(object sender, EventArgs e)
        {
            bSendEncodeHex = false;
        }

        private void chkAutoReply_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAutoReply.CheckState == CheckState.Checked)
            {
                bAutoReply = true;
            }
            else
            {
                bAutoReply = false;
            }
        }

        private void txtDataPort_TextChanged(object sender, EventArgs e)
        {
            dataPort = txtDataPort.Text;
        }

        private void txtDataUser_TextChanged(object sender, EventArgs e)
        {
            dataUser = txtDataUser.Text;
        }

        private void txtDataPassword_TextChanged(object sender, EventArgs e)
        {
            dataPassword = txtDataPassword.Text;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            lsbRecvMsg.Items.Clear() ;
        }


        private void chkCRC16_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCRC16.CheckState == CheckState.Checked)
            {
                txtSendMsg.Text = txtSendMsg.Text.Replace(" ", "") + FormatFunc.ToModbusCRC16(txtSendMsg.Text);

            }
        }

        private void btnSendto_Click(object sender, EventArgs e)
        {

        }
      

    }


 }


