using System;
using System.Windows.Forms;
using System.Threading;

namespace BoxIdDb
{
    internal class DefaultApplicationInitializer : ApplicationInitializer
    {
        /// <summary>
        /// instance of this class
        /// </summary>
        private static readonly DefaultApplicationInitializer instance = new DefaultApplicationInitializer();

        /// <summary>
        /// returns the current instance 
        /// </summary>
        /// <returns></returns>
        internal static DefaultApplicationInitializer GetInstance()
        {
            return instance;
        }

        public void Init()
        {
            // Set the unhandled exception mode to force all Windows Forms errors
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.ThrowException);
        }
    }
}
