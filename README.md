# Client.Sefaz.Net
Api de Download de XML (NF-e Versão 3.10 / 4.00)
'''
Api = new Client.Sefaz.Net.Api(); 
using (var ms = new MemoryStream(Api.Captcha()))
{
    pictureBox1.Image = Image.FromStream(ms);
}
'''

'''
var DadosPagina = Api.ConsultaToXml(_Chave, txtCaptcha.Text);
'''

![alt text](http://ralms.net/img_git/Consulta.png)

![alt text](http://ralms.net/img_git/RetornoXMl.png)
