//namespace InventarioApp
//{
//    public partial class Form1 : Form
//    {
//        public Form1()
//        {
//            InitializeComponent();
//        }

//        private void webView21_Click(object sender, EventArgs e)
//        {

//        }
//    }
//}
using System;
using System.IO;
using System.Windows.Forms;

namespace InventarioApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InicializarWebView();
        }

        private async void InicializarWebView()
        {
            // Inicializar el motor de WebView2
            await webView21.EnsureCoreWebView2Async();

            // Inyectar el Bridge C# para que JavaScript pueda llamarlo
            webView21.CoreWebView2.AddHostObjectToScript("chrome", new Bridge());

            // Cargar el archivo index.html local
            string htmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "index.html");
            if (File.Exists(htmlPath))
            {
                webView21.CoreWebView2.Navigate(htmlPath);
            }
            else
            {
                MessageBox.Show("No se encontró el archivo index.html en la carpeta de ejecución.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // ===== AGREGAR ESTO EN Form1.cs después de crear el WebView =====

        // En el constructor o en el evento Load, después de configurar el WebView:

        //private async void Form1_Load(object sender, EventArgs e)
        //{ // ... tu código actual ...

        //    // Agregar esta línea para inicializar la tabla de usuarios
        //    var bridge = new Bridge();
        //    bridge.InicializarUsuarios();

        //    // ... resto del código ...

        //}
        private async void Form1_Load(object sender, EventArgs e)
        {
            // Tu código actual...

            // AGREGAR ESTA LÍNEA:
            new Bridge().InicializarUsuarios();

            // Resto del código...
        }
    }
}