using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;

namespace WindowsFormsApp1
{
    public partial class Frm_Multimetro : Form
    {
        public Frm_Multimetro()
        {
            InitializeComponent();
            cmbBox_Portas.DataSource = SerialPort.GetPortNames();
            pictureBox1.Enabled = false;
        }

        #region Variáveis Globais
        string stringSerial;
        int contador = 1;
        Boolean clicked = false;
        #endregion

        #region Carrega Portas Serial Disponíveis
        private void cmbBox_Portas_Click_1(object sender, EventArgs e)
        {
            cmbBox_Portas.DataSource = SerialPort.GetPortNames();
        }
        #endregion

        #region Botão Conectar
        private void btn_conectar_Click(object sender, EventArgs e)
        {
            if (SPort.IsOpen)
            {
                try
                {
                    SPort.Close();
                    pnl_status.BackColor = Color.Red;
                    pictureBox1.Enabled = false;
                    lbl_status_desconectado.Visible = true;
                    lbl_status_conectado.Visible = false;
                    pictureBox1.Image = Image.FromFile(Directory.GetCurrentDirectory() + "/img1.png");
                    txt_display.Text = "";
                    btn_conectar.Text = "Conectar";
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message);
                }
            }
            else
            {
                try
                {
                    SPort.PortName = cmbBox_Portas.Text;
                    SPort.BaudRate = Convert.ToInt32(cmbBox_Taxas.Text);
                    SPort.Open();
                    pictureBox1.Enabled = true;
                    txt_display.Text = "OFF";
                    pnl_status.BackColor = Color.Green;
                    lbl_status_desconectado.Visible = false;
                    lbl_status_conectado.Visible = true;
                    btn_conectar.Text = "Desconectar";
                }
                catch (Exception error)
                {
                    if(cmbBox_Portas.Text.Equals(""))
                    {
                        MessageBox.Show("Você deve escolher uma porta serial");
                        cmbBox_Portas.Focus();
                    }
                    else if (cmbBox_Taxas.Text.Equals(""))
                    {
                        MessageBox.Show("Você deve escolher uma taxa");
                        cmbBox_Taxas.Focus();
                    }
                    else
                    {
                        MessageBox.Show(error.Message);
                    }
                }
            }
        }
        #endregion
 
        #region Fechar Form
        private void Frm_Multimetro_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                SPort.Close();
                pnl_status.BackColor = Color.Red;
                lbl_status_desconectado.Visible = true;
                lbl_status_conectado.Visible = false;
                btn_conectar.Text = "Conectar";
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }
        #endregion

        #region Receb String Serial
        private void SPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
           stringSerial = SPort.ReadLine();
           this.Invoke(new EventHandler(trataDadoRecebido));
        }
        #endregion

        #region Trata Dados Recebidos
        private void trataDadoRecebido(object sender, EventArgs e)
        {
            if(clicked == false)
            {
                txt_display.Text = "";
                if (stringSerial.Substring(0, 4).Equals("V050"))
                {
                    pictureBox1.Image = Image.FromFile(Directory.GetCurrentDirectory() + "/img2.png");
                    txt_display.Text = stringSerial.Substring(4);
                }
                else if (stringSerial.Substring(0, 4).Equals("V020"))
                {
                    pictureBox1.Image = Image.FromFile(Directory.GetCurrentDirectory() + "/V020.png");
                    txt_display.Text = stringSerial.Substring(4);
                }
                else if (stringSerial.Substring(0, 4).Equals("V005"))
                {
                    pictureBox1.Image = Image.FromFile(Directory.GetCurrentDirectory() + "/V005.png");
                    txt_display.Text = stringSerial.Substring(4);
                }

                if (stringSerial.Substring(0, 4).Equals("O010"))
                {
                    pictureBox1.Image = Image.FromFile(Directory.GetCurrentDirectory() + "/O010.png");
                    txt_display.Text = stringSerial.Substring(4);
                }
                else if (stringSerial.Substring(0, 4).Equals("O100"))
                {
                    pictureBox1.Image = Image.FromFile(Directory.GetCurrentDirectory() + "/O100.png");
                    txt_display.Text = stringSerial.Substring(4);
                }
                else if (stringSerial.Substring(0, 4).Equals("O002"))
                {
                    pictureBox1.Image = Image.FromFile(Directory.GetCurrentDirectory() + "/img4.png");
                    txt_display.Text = stringSerial.Substring(4);
                }

                if (stringSerial.Substring(0, 4).Equals("A500"))
                {
                    pictureBox1.Image = Image.FromFile(Directory.GetCurrentDirectory() + "/img3.png");
                    txt_display.Text = stringSerial.Substring(4);
                }
            }
        }
        #endregion

        #region Picture Box Click
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            contador = contador % 4 + 1;
            pictureBox1.Image = Image.FromFile(Directory.GetCurrentDirectory() + "/img" + Convert.ToString(contador) + ".png");
            SPort.Write(contador.ToString());
        }
        #endregion

        #region Timer Programa
        private void timer1_Tick(object sender, EventArgs e)
        {
            /*if(SPort.IsOpen)
            {
                SPort.Write(contador.ToString());
            }*/
        }
        #endregion

        #region Botao Sair
        private void btn_sair_Click(object sender, EventArgs e)
        {
            try
            {
                SPort.Close();
                pnl_status.BackColor = Color.Red;
                lbl_status_desconectado.Visible = true;
                lbl_status_conectado.Visible = false;
                btn_conectar.Text = "Conectar";
                this.Close();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }
        #endregion

        private void btn_hold_Click(object sender, EventArgs e)
        {
            if(clicked == false)
            {
                clicked = true;
                pictureBox1.Enabled = false;
                txt_display.Text = txt_display.Text;
                btn_conectar.Enabled = false;
            }
            else
            {
                clicked = false;
                pictureBox1.Enabled = true;
                btn_conectar.Enabled = true;
            }
        }
    }
}
