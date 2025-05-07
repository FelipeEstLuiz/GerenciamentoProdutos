export interface ProductList {
    id: number;
    nome: string;
    descricao?: string;
    valor: number;
    quantidadeEstoque: number;
    Categoria: string;
    codigoStatusProduto: string;
    dataCadastro: string;
    dataUltimaVenda?: string;
}