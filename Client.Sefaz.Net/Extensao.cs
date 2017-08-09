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

using System.Collections.Generic; 
using System.Windows.Forms;

namespace Client.Sefaz.Net
{
    public static class HtmlExtensao
    {
        public static IEnumerable<HtmlElement> RetornarElementoPelaClasse(this HtmlDocument docNavegador, string nomeClasse)
        {
            foreach (HtmlElement e in docNavegador.All)
                if (e.GetAttribute("className") == nomeClasse)
                    yield return e;
        }

        public static string ApenasNumeros(this string str)
        {
            var somenteNumeros = new System.Text.RegularExpressions.Regex(@"[^\d]");
            return somenteNumeros.Replace(str, "");
        }
    }


}
