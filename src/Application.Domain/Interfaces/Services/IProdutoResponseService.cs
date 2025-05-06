using Application.Domain.Entities;
using Application.Domain.VO;

namespace Application.Domain.Interfaces.Services;

public interface IProdutoResponseService
{
    Task<ProdutoVo> MontarProdutoVoAsync(Produto produto, CancellationToken cancellationToken);
}
