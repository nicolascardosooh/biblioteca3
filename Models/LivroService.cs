using System.Linq;
using System.Collections.Generic;
using System.Collections;

namespace Biblioteca.Models
{
    public class LivroService
    {
        public void Inserir(Livro  livro)
        {
            using (BibliotecaContext bc = new BibliotecaContext())
            {
                bc.Livros.Add(livro);
                bc.SaveChanges();
            }
        }

        public void Atualizar(Livro livro)
        {
            using (BibliotecaContext bc = new BibliotecaContext())
            {
                 Livro livroAntigo = bc.Livros.Find(livro.Id);
            
                livroAntigo.Autor = livro.Autor;
                
                livroAntigo.Titulo = livro.Titulo;
                
                livroAntigo.Ano = livro.Ano;

            
                bc.SaveChanges();
            }
        }

        public ICollection<Livro> ListarTodos(FiltrosLivros filtro = null)
        {
            using (BibliotecaContext bc = new BibliotecaContext())
            {
                IQueryable<Livro> query;

                if (filtro != null)
                {
                    //definindo dinamicamente a filtragem
                    switch (filtro.TipoFiltro)
                    {
                        case "Autor":
                            query = bc.Livros.Where(l => l.Autor.Contains(filtro.Filtro));
                            break;

                        case "Titulo":
                            query = bc.Livros.Where(l => l.Titulo.Contains(filtro.Filtro));
                            break;

                        default:
                            query = bc.Livros;
                            break;
                    }
                }
                else
                {
                    // caso filtro não tenha sido informado
                    query = bc.Livros;
                }

                //ordenação padrão
                return query.OrderBy(l => l.Titulo).ToList();
            }
        }

        public ICollection<Livro> ListarDisponiveis()
        {
            using (BibliotecaContext bc = new BibliotecaContext())
            {
                //busca os livros onde o id não está entre os ids de livro em empréstimo
                // utiliza uma subconsulta
                return
                    bc.Livros
                    .Where(l => !(bc.Emprestimos.Where(e => e.Devolvido == false).Select(e => e.LivroId).Contains(l.Id)))
                    .ToList();
            }
        }

        public Livro ObterPorId(int id)
        {
            using (BibliotecaContext bc = new BibliotecaContext())
            {
                return bc.Livros.Find(id);
            }
        }
    }
}