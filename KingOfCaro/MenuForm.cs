using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KingOfCaro
{
    public partial class MenuForm : Form
    {
        private Form1 chessBoardForm;
        private Form2 chessBoardForm2;
        public MenuForm()
        {
            InitializeComponent();
            chessBoardForm = null;
            chessBoardForm2 = null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (chessBoardForm == null || chessBoardForm.IsDisposed)
            {
                chessBoardForm = new Form1(this);
            }
            chessBoardForm.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (chessBoardForm2 == null || chessBoardForm2.IsDisposed)
            {
                chessBoardForm2 = new Form2(this);
            }
            chessBoardForm2.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
