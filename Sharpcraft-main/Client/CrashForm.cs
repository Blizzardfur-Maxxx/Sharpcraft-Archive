using Microsoft.VisualBasic;
using Microsoft.VisualBasic.Devices;
using SharpCraft.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LWCSGL.OpenGL;
using static LWCSGL.OpenGL.GL11C;
using SharpCraft.Client.Renderer;

namespace SharpCraft.Client
{
    public partial class CrashForm : Form
    {
        public CrashForm(CrashReport crashReport)
        {
            InitializeComponent();
            Icon = Client.WindowIcon;
            pbLogo.Image = Textures.GetAssetsBitmap("/gui/logo.png");
            string report = GenerateReport(crashReport);
            rtxtDetails.AppendText("\n");
            rtxtDetails.AppendText("\n");
            rtxtDetails.AppendText($"      SharpCraft has crashed!      \n");
            rtxtDetails.AppendText($"      -----------------------      \n");
            rtxtDetails.AppendText("SharpCraft has stopped running because it encountered a problem.\n");
            rtxtDetails.AppendText("\n");
            rtxtDetails.AppendText("If you wish to report this, please copy this entire text and email it to nobody@localhost.\n");
            rtxtDetails.AppendText("Please include a description of what you did when the error occured.\n");
            rtxtDetails.AppendText("\n");
            rtxtDetails.AppendText("\n");
            rtxtDetails.AppendText("\n");
            rtxtDetails.AppendText($"--- BEGIN ERROR REPORT {HashCodeToHex(rtxtDetails.Text)} --------\n");
            rtxtDetails.AppendText(report);
            rtxtDetails.AppendText($"--- END ERROR REPORT {HashCodeToHex(rtxtDetails.Text)} ----------\n");
            rtxtDetails.AppendText("\n");
            rtxtDetails.AppendText("\n");
        }

        private string HashCodeToHex(string str)
        {
            int hashCode = str.GetHashCode();
            return BitConverter
                .ToString(BitConverter.GetBytes(hashCode))
                .Replace("-", "")
                .ToLower();
        }

        private string GenerateReport(CrashReport crashReport)
        {
            string report = "";
            ComputerInfo info = new ComputerInfo();
            string osName = string.Join(' ', info.OSFullName
                .Replace("Microsoft ", "").Split(' ').Take(2)).Split('.')[0];
            string osArch = Environment.Is64BitOperatingSystem ? "arch64" : "i386";
            string osVersion = string.Join('.', info.OSVersion.Split('.').Take(2));

            try
            {
                report += $"Generated {DateTime.Now}\n";
                report += "\n";
                report += $"SharpCraft: {SharedConstants.VERSION_STRING}\n";
                report += $"OS: {osName} ({osArch}) version {osVersion}\n";
                report += $"C#: {Environment.Version}, Microsoft\n";
                if (!Display.IsCreated()) throw new Exception(
                    "No OpenGL context available.");
                report += $"OpenGL: {GLU.GetGLString(GL_RENDERER)} version " +
                    $"{GLU.GetGLString(GL_VERSION)}, {GLU.GetGLString(GL_VENDOR)}\n";
            }
            catch (Exception ex)
            {
                report += $"[failed to get system properties ({ex.Message})]\n";
            }

            report += "\n";
            report += $"{crashReport.Ex}\n";

            return report;
        }

        private void CrashForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(1);
        }
    }
}
