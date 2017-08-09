﻿/*
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
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;

namespace Client.Sefaz.Net
{
    public class Api
    {
        private WebBrowser navegador = null;
        private bool DownloadPaginaConcluido { get; set; }
        public Api()
        {
            navegador = new WebBrowser
            {
                ScriptErrorsSuppressed = true,
                AllowNavigation = true,
                AllowWebBrowserDrop = true
            };
        }

        public byte[] Captcha() => RecuperarCaptchaSefaz();

        internal byte[] RecuperarCaptchaSefaz()
        {
            DownloadPaginaConcluido = false;
            navegador.DocumentCompleted += delegate
            {
                DownloadPaginaConcluido = true;
            };
            navegador.Navigate("https://www.nfe.fazenda.gov.br/portal/consulta.aspx?tipoConsulta=completa&tipoConteudo=XbSeqxE8pl8=");
            while (!DownloadPaginaConcluido)
            {
                System.Threading.Thread.Sleep(100);
                Application.DoEvents();
            }
            return Convert.FromBase64String(navegador.Document.GetElementById("ctl00_ContentPlaceHolder1_imgCaptcha").GetAttribute("src").Replace("data:image/png;base64,", ""));
        }

        internal static IEnumerable<HtmlElement> RetornarElementoPelaClass(HtmlDocument docNavegador, string nomeClasse)
        {
            foreach (HtmlElement e in docNavegador.All)
                if (e.GetAttribute("className") == nomeClasse)
                    yield return e;
        }

        public string ConsultaToHTML(string chave, string captcha)
        {
            DownloadPaginaConcluido = false;
            navegador.Document.GetElementById("ctl00_ContentPlaceHolder1_txtCaptcha").SetAttribute("value", captcha);
            navegador.Document.GetElementById("ctl00_ContentPlaceHolder1_txtChaveAcessoCompleta").SetAttribute("value", chave);
            navegador.DocumentCompleted += delegate
            {
                DownloadPaginaConcluido = true;
            };
            navegador.Document.GetElementById("ctl00_ContentPlaceHolder1_btnConsultar").InvokeMember("click");
            while (!DownloadPaginaConcluido)
            {
                System.Threading.Thread.Sleep(100);
                Application.DoEvents();
            }
            var Elements = RetornarElementoPelaClass(navegador.Document, "indentacaoConteudo").ToList();
            return Elements[1].OuterHtml;
        }

        public string ConsultaToTags(string chave, string captcha)
        {
            DownloadPaginaConcluido = false;
            navegador.Document.GetElementById("ctl00_ContentPlaceHolder1_txtCaptcha").SetAttribute("value", captcha);
            navegador.Document.GetElementById("ctl00_ContentPlaceHolder1_txtChaveAcessoCompleta").SetAttribute("value", chave);
            navegador.DocumentCompleted += delegate
            {
                DownloadPaginaConcluido = true;
            };
            navegador.Document.GetElementById("ctl00_ContentPlaceHolder1_btnConsultar").InvokeMember("click");
            while (!DownloadPaginaConcluido)
            {
                System.Threading.Thread.Sleep(100);
                Application.DoEvents();
            }
            var Elements = RetornarElementoPelaClass(navegador.Document, "indentacaoConteudo");
            if (Elements == null)
                return "Erro na consulta";

            var Tags = Elements.ToList()[1].GetElementsByTagName("td");
            StringBuilder TagsNF = new StringBuilder();
            foreach (HtmlElement tag in Tags)
            {
                if (tag.Children.Count == 1)
                    TagsNF.Append($"---> <b>{tag.Children[0].InnerText}:</b>&nbsp;<br>");

                if (tag.Children.Count > 1)
                    TagsNF.Append($"<b>{tag.Children[0].InnerText}:</b>&nbsp;{tag.Children[1].InnerText}<br>");
            }


            return TagsNF.ToString();
        }

    }
}
