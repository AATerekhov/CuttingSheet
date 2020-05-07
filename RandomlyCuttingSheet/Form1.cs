using System;
using System.Windows.Forms;

namespace RandomlyCuttingSheet
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        
        private void button5_Click(object sender, EventArgs e)
        {
            Functions.CreatingCuttingFCNR();
        }
    }
}
