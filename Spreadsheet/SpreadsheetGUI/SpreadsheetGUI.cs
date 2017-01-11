using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadsheetGUI
{
    static class SpreadsheetGUI
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            SSApplicationContext appContext = SSApplicationContext.getAppContext();
            appContext.RunForm(new Form1(""));
            Application.Run(appContext);
        }
    }

    class SSApplicationContext : ApplicationContext
    {
        private int formCount = 0; //Number of open forms
        private static SSApplicationContext appContext; //Singleton ApplicationContext

        private SSApplicationContext()
        {

        }

        public static SSApplicationContext getAppContext()
        {
            if (appContext == null)
                appContext = new SSApplicationContext();
            return appContext;
        }

        public void RunForm(Form form)
        {
            formCount++;
            //when a form closes subtract 1 from formcount and check to see if it is the last form
            //if it is the last form exitThread
            form.FormClosed += (o, e) => { if (--formCount <= 0) ExitThread(); };
            form.Show();
        }
    }
}
