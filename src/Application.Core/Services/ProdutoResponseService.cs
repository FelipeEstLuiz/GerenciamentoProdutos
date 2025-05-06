using Application.Domain.Entities;
using Application.Domain.Interfaces.Services;
using Application.Domain.VO;

namespace Application.Core.Services;
public class ProdutoResponseService(ICategoriaCacheService categoriaCacheService) : IProdutoResponseService
{
    public async Task<ProdutoVo> MontarProdutoVoAsync(Produto produto, CancellationToken cancellationToken)
    {
        IEnumerable<CategoriaProduto> categorias = await categoriaCacheService.GetAllAsync(cancellationToken);
        return ProdutoVo.FromEntity(produto, categorias);
    }
}
