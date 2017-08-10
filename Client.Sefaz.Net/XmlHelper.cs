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
            var Ide = new StringBuilder();
            var Emitente = new StringBuilder();
            foreach (var item in GetIdentificacao().OrderBy(p=>p.Key))
                Ide.Append(item.Value); 
            foreach (var item in GetEmitente().OrderBy(p => p.Key))
                Emitente.Append(item.Value);

            var ChaveAcesso = Tags[0].Children[1].InnerText.ApenasNumeros();
            StringBuilder scope = new StringBuilder();
            scope.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            scope.Append("<nfeProc versao =\"3.10\" xmlns=\"http://www.portalfiscal.inf.br/nfe\">");
            scope.Append("  <NFe xmlns =\"http://www.portalfiscal.inf.br/nfe\">");
            scope.Append($"      <infNFe versao =\"3.10\" Id=\"NFe{ChaveAcesso}\">");
            scope.Append("          <ide>");
            scope.Append($"          {Ide.ToString()}");
            scope.Append("          </ide>");
            scope.Append("          <emit>");
            scope.Append($"          {Emitente.ToString()}");
            scope.Append("          </emit>");
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
        public Dictionary<int,string> GetIdentificacao()
        {
            var dicionario = new Dictionary<int, string>();
            var Trs = HTML.ToList()[1].GetElementsByTagName("tr"); 
            foreach (HtmlElement tr in Trs)
            {
                foreach (HtmlElement elemento in tr.GetElementsByTagName("td"))
                {
                    if (elemento.Children.Count < 2)
                        continue;

                    switch (elemento.FirstChild.InnerText)
                    {
                        case "Modelo":
                            if (!dicionario.Where(p => p.Key == 4).Any())
                                dicionario.Add(4, $"<mod>{elemento.Children[1].InnerText}</mod>");
                            break;
                        case "Série":
                            if (!dicionario.Where(p => p.Key == 5).Any())
                                dicionario.Add(5, $"<serie>{elemento.Children[1].InnerText}</serie>"); 
                            break;
                        case "Número":
                            if (!dicionario.Where(p => p.Key == 6).Any()) 
                                dicionario.Add(6, $"<nNF>{elemento.Children[1].InnerText}</nNF>"); 
                            break;
                        case "Data de Emissão":
                            if (!dicionario.Where(p => p.Key == 7).Any())
                                dicionario.Add(7, $"<dhEmi>{elemento.Children[1].InnerText}</dhEmi>"); 
                            break; 
                        case "Versão do Processo":
                            if (!dicionario.Where(p => p.Key == 20).Any())
                                dicionario.Add(20, $"<verProc>{elemento.Children[1].InnerText}</verProc>");
                            break;

                    }
                }
            } 
            return dicionario;
        }


        public Dictionary<int, string> GetEmitente()
        {
            var dicionario = new Dictionary<int, string>();
            var Trs = HTML.ToList()[1].GetElementsByTagName("tr");
            foreach (HtmlElement tr in Trs)
            {
                foreach (HtmlElement elemento in tr.GetElementsByTagName("td"))
                {
                    if (elemento.Children.Count < 2)
                        continue;

                    switch (elemento.FirstChild.InnerText)
                    {
                        case "CNPJ":
                            if (!dicionario.Where(p => p.Key == 0).Any())
                                dicionario.Add(0, $"<CNPJ>{elemento.Children[1].InnerText.ApenasNumeros()}</CNPJ>");
                            break;
                        case "Nome / Razão Social":
                            if (!dicionario.Where(p => p.Key == 1).Any())
                                dicionario.Add(1, $"<xNome>{elemento.Children[1].InnerText}</xNome>");
                            break;
                        case "Nome Fantasia":
                            if (!dicionario.Where(p => p.Key == 2).Any())
                                dicionario.Add(2, $"<xFant>{elemento.Children[1].InnerText}</xFant>"); 
                                dicionario.Add(3, $"<enderEmit>{GetEmitenteEndereco()}</enderEmit>");  
                            break;
                        case "Inscrição Estadual":
                            if (!dicionario.Where(p => p.Key == 4).Any())
                                dicionario.Add(4, $"<IE>{elemento.Children[1].InnerText}</IE>");
                            break;
                        case "Código de Regime Tributário":
                            if (!dicionario.Where(p => p.Key == 5).Any())
                                dicionario.Add(5, $"<CRT>{elemento.Children[1].InnerText.Split('-')[0].Trim().ApenasNumeros()}</CRT>");
                            break;
                    }
                }
            }
            return dicionario;
        }


        public string GetEmitenteEndereco()
        {
            var dicionario = new Dictionary<int, string>();
            var Trs = HTML.ToList()[1].GetElementsByTagName("tr");
            foreach (HtmlElement tr in Trs)
            {
                foreach (HtmlElement elemento in tr.GetElementsByTagName("td"))
                {
                    if (elemento.Children.Count < 2)
                        continue;

                    switch (elemento.FirstChild.InnerText)
                    {
                        case "Endereço":
                            if (!dicionario.Where(p => p.Key == 0).Any())
                                dicionario.Add(0, $"<xLgr>{elemento.Children[1].InnerText.Trim()}</xLgr>");
                            break; 
                        case "Bairro / Distrito":
                            if (!dicionario.Where(p => p.Key == 2).Any())
                                dicionario.Add(2, $"<xBairro>{elemento.Children[1].InnerText.Trim()}</xBairro>");
                            break;
                        case "Município":
                            if (!dicionario.Where(p => p.Key == 3).Any())
                            {
                                dicionario.Add(3, $"<cMun>{elemento.Children[1].InnerText.Split('-')[0].Trim()}</cMun>");
                                dicionario.Add(4, $"<xMun>{elemento.Children[1].InnerText.Split('-')[1].Trim()}</xMun>");
                            }
                            break;
                        case "UF":
                            if (!dicionario.Where(p => p.Key == 5).Any())
                                dicionario.Add(5, $"<UF>{elemento.Children[1].InnerText.Trim()}</UF>");
                            break;
                        case "CEP":
                            if (!dicionario.Where(p => p.Key == 6).Any())
                                dicionario.Add(6, $"<CEP>{elemento.Children[1].InnerText.Trim().ApenasNumeros()}</CEP>");
                            break;
                        case "País":
                            if (!dicionario.Where(p => p.Key == 7).Any())
                            {
                                dicionario.Add(7, $"<cPais>{elemento.Children[1].InnerText.Split('-')[0].Trim()}</cPais>");
                                dicionario.Add(8, $"<xPais>{elemento.Children[1].InnerText.Split('-')[1].Trim()}</xPais>");
                            }
                            break;
                    }
                }
            }
            return  string.Join("", dicionario.OrderBy(x=>x.Key).Select(x=> x.Value).ToArray());
        }

    }
}
