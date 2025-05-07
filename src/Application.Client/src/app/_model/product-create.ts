export interface ProductCreate {
    nome: string;
    descricao?: string | null;
    valor: number;
    quantidadeEstoque: number;
    categoria: number;    
  }