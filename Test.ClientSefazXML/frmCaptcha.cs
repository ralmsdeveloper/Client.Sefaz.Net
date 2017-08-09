/*
                    #####   Client.Sefaz.Net   #####

   Este arquivo é um software livre; você pode redistribuí-lo e / ou
modificá-lo sob os termos da GNU Lesser General Public License conforme 
publicada pela Free Software Foundation; ou a versão 2.1 da Licença, ou 
(a seu critério) qualquer versão posterior.
   Esta biblioteca é distribuída na esperança de que será útil, mas SEM 
QUALQUER GARANTIA; mesmo sem a garantia implícita de COMERCIALIZAÇÃO ou 
ADEQUAÇÃO A UM DETERMINADO PURPOSE.See do GNU
Lesser General Public License para mais detalhes.
Você deve ter recebido uma cópia da GNU General Public License
juntamente com esta. Se não, veja <http://www.gnu.org/licenses/>  

               Direitos Autorais Reservados©  2017 
                    (Rafael Almeida) - Ralms 
                        www.ralms.net
                       ralms@ralms.net  

*/
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Test.Forms
{
    public partial class frmCaptcha : Form
    {
        public string DadosPagina = string.Empty;
        private Client.Sefaz.Net.Api Api = null;
        private string _Chave = string.Empty;
        public frmCaptcha(string chave)
        {
            InitializeComponent();
            Api = new Client.Sefaz.Net.Api(); 
            using (var ms = new MemoryStream(Api.Captcha()))
            {
                pictureBox1.Image = Image.FromStream(ms);
            }
            _Chave = chave;
        }

        private void txtCaptcha_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Enter)
            {
                DadosPagina = Api.ConsultaToTags(_Chave, txtCaptcha.Text);
                Close();
            }
        }
    }
}
