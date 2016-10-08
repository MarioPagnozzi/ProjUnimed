using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SisUnimed
{
    public static class GerenciadorDeRedirecionamento
    {
        public static ItemDePagina ObterPaginaPorUrl(string url)
        {
            ItemDePagina item = null;

            /* Aqui você pesquisa na entidade pela descrição, passando o parâmetro url. */
            /* Este é o ponto mais importante da lógica, que é onde você vai pesquisar o item de acordo com as suas regras de negócio. */
            /* Depois você monta um objeto ItemDePagina (no caso, item) e o devolve. */

            return item;
        }
    }
}