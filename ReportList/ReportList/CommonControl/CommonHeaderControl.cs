using System.Windows.Forms;

namespace ReportList
{
    public partial class CommonHeaderControl : UserControl
    {

        
        public CommonHeaderControl()
        {
            
             InitializeComponent();
             
        }

        private void CommonHeaderControl_Load(object sender, System.EventArgs e)
        {
            if (!this.DesignMode)
            {
                Header_lbl.Text = ReportList.Properties.Resources.APPLICATION_ENVIRONMENT_HEADER;
                
            }
        }

    }
}
