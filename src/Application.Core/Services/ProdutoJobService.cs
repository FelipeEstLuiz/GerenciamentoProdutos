using Application.Domain.Interfaces.Repositories;

namespace Application.Core.Services;

public class ProdutoJobService(IProdutoRepository produtoRepository)
{
    public async Task AtualizarStatusProdutos()
        => await produtoRepository.UpdateStatusForOldProductsAsync(CancellationToken.None);
}
