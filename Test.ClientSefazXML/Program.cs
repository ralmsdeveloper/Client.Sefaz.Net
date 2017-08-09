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


using System; 
using System.Windows.Forms;

namespace Test.Forms
{
    static class Program
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmPrincipal());
        }
    }
}
