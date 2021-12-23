using Biblioteca.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Controllers
{

    public class UsuarioController : Controller
    {
        public IActionResult ListadeUsuarios()
        {
            Autenticacao.CheckLogin(this);
            Autenticacao.VerificaSeUsuarioEAdmin(this);
            return View(new UsuarioService().Listar());
        }

        public IActionResult RegistrarUsuarios()
        {
            Autenticacao.CheckLogin(this);
            Autenticacao.VerificaSeUsuarioEAdmin(this);
            return View();
        }

        [HttpPost]
        public IActionResult RegistrarUsuarios(Usuario novoUsuario)
        {
            novoUsuario.Senha = Criptografo.TextoCriptografado(novoUsuario.Senha);
            new UsuarioService().IncluirUsuario(novoUsuario);
            return RedirectToAction("CadastroRealizado");
        }

        public IActionResult CadastroRealizado()
        {
            return View();
        }

        public IActionResult EditarUsuario(int id)
        {
            Usuario u = new UsuarioService().BuscarporId(id);
            return View();
        }

        [HttpPost]
        public IActionResult EditarUsuario(Usuario userEditado)
        {
            new UsuarioService().EditarUsuario(userEditado);
            return RedirectToAction("ListaDeUsuarios");
        }

        public IActionResult ExcluirUsuario(int id)
        {
            return View(new UsuarioService().Listar());
        }

        [HttpPost]
        public IActionResult ExcluirUsuario(string decisao, int id)
        {
            if (decisao == "EXCLUIR")
            {
                ViewData["Mensagem"] = "Exclusão do usuário " + new UsuarioService().BuscarporId(id).Nome + " realizada com sucesso!";
                new UsuarioService().ExcluirUsuario(id);
                return View("ListaDeUsuarios", new UsuarioService().Listar());
            }
            else
            {
                ViewData["Mensagem"] = "Exclusão cancelada";
                return View("ListadeUsuarios", new UsuarioService().Listar());
            }
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult NeedAdmin()
        {
            Autenticacao.CheckLogin(this);
            return View();
        }
    }
}