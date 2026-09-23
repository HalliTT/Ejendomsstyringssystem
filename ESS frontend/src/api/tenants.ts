import type { Tenant } from "@/types";
import { authFetch } from "@/lib/auth/authFetch";

export async function getTenants(): Promise<Tenant[]> {
    const response = await authFetch("https://localhost:7119/api/tenants");

    if (!response.ok) {
        throw new Error("Failed to fetch tenants");
    }

    return response.json();
}
