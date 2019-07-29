using System;
using System.Windows.Forms;

namespace JigQuick
{
    public class DefaultApplicationContext : ApplicationContext
    {

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="assemblyname"></param>
        /// <param name="typename"></param>
        /// <param name="appricationname"></param>
        /// <param name="passwordCheckNeeded"></param>
        public DefaultApplicationContext(string applicationname)
        {

            //initialize the DefaultApplicationInitializer
            DefaultApplicationInitializer.GetInstance().Init();

            //login form show
            frmOmni login = new frmOmni(applicationname);
            login.Closed += new EventHandler(OnFormClosed);
            login.Show();

        }

        /// <summary>
        /// exit application on form close event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFormClosed(object sender, EventArgs e)
        {
            ExitThread();
        }
    }
}
