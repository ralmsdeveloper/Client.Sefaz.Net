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

using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Linq;
using System.Collections.Generic;

namespace Client.Sefaz.Net
{
    /// <summary>
    /// Classe para gerar o XML
    /// </summary>
    public class XmlHelper
    {
        private XmlDocument NFeXML { get; set; } 
        private IEnumerable<HtmlElement> HTML { get; set; }
        /// <summary>
        /// Inicializador de Objeto
        /// </summary>
        /// <param name="html"></param>
        public XmlHelper(IEnumerable<HtmlElement> html)
        {
            this.HTML = html;
            this.NFeXML = new XmlDocument();
        }
        /// <summary>
        /// Tag NF- Principal
        /// </summary>
        /// <returns></returns>
        public XmlDocument GetNFe()
        { 
            var Tags = HTML.ToList()[1].GetElementsByTagName("td");
            var Trs = HTML.ToList()[1].GetElementsByTagName("tr");
            var TagsNF = new StringBuilder();
            var IDE = new StringBuilder();
            foreach (HtmlElement tr in Trs)
            {
                foreach (HtmlElement elemento in tr.GetElementsByTagName("td"))
                {
                    if (elemento.Children.Count < 2)
                        continue;

                    switch (elemento.FirstChild.InnerText)
                    {
                        case "Modelo":
                            IDE.Append($"<mod>{elemento.Children[1].InnerText}</mod>");
                            break;
                        case "Série":
                            IDE.Append($"<serie>{elemento.Children[1].InnerText}</serie>");
                            break;
                        case "Número":
                            IDE.Append($"<nNF>{elemento.Children[1].InnerText}</nNF>");
                            break;
                        case "Data de Emissão":
                            IDE.Append($"<dhEmi>{elemento.Children[1].InnerText}</dhEmi>");
                            break;

                    }
                } 
            }
            var ChaveAcesso = Tags[0].Children[1].InnerText.ApenasNumeros();
            StringBuilder scope = new StringBuilder();
            scope.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            scope.Append("<nfeProc versao =\"3.10\" xmlns=\"http://www.portalfiscal.inf.br/nfe\">");
            scope.Append("  <NFe xmlns =\"http://www.portalfiscal.inf.br/nfe\">");
            scope.Append($"      <infNFe versao =\"3.10\" Id=\"NFe{ChaveAcesso}\">");
            scope.Append("          <ide>");
            scope.Append($"          {IDE}");
            scope.Append("          </ide>");
            scope.Append("          <emit></emit>");
            scope.Append("          <dest>"); 
            scope.Append("          </dest>");
            scope.Append("          <autXML></autXML>");
            scope.Append("          <det></det>");
            scope.Append("          <total></total>");
            scope.Append("          <transp></transp>");
            scope.Append("          <cobr></cobr>");
            scope.Append("          <infAdic></infAdic>");
            scope.Append("      </infNFe>");
            scope.Append("      <Signature></Signature>");
            scope.Append("  </NFe>");
            scope.Append("  <protNFe></protNFe>");
            scope.Append("</nfeProc>");
            NFeXML.LoadXml(scope.ToString());
            return NFeXML;
        }

        /// <summary>
        /// Tag Identificação
        /// </summary>
        /// <returns></returns>
        public XmlDocument GetIdentificacao()
        { 
            return NFeXML;
        }
    }
}
