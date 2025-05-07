import { ProductCreate } from "./product-create";

export interface ProductEdit extends ProductCreate {
    id: number;
    dataUltimaVenda?: string | null;
    dataCadastro?: Date | null;
}