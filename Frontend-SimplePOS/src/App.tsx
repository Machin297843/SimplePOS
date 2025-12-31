import { useEffect, useState } from "react";
import { api } from "./api/client";

type ProductResponse = {
  id: number;
  sku: string;
  name: string;
  price: number;
  productGroupId: number;
};

export default function App() {
  const [products, setProducts] = useState<ProductResponse[]>([]);
  const [error, setError] = useState<string>("");

  useEffect(() => {
    (async () => {
      try {
        const res = await api.get<ProductResponse[]>("/products");
        setProducts(res.data);
      } catch (e: any) {
        setError(e?.message ?? "Error");
      }
    })();
  }, []);

  return (
    <div style={{ padding: 24 }}>
      <h1>SimplePOS</h1>

      {error && <p>{error}</p>}

      <ul>
        {products.map((p) => (
          <li key={p.id}>
            {p.sku} - {p.name} - {p.price}
          </li>
        ))}
      </ul>
    </div>
  );
}
