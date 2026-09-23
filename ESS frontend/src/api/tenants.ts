import type { Tenant } from "@/types";
import { authFetch } from "@/lib/auth/authFetch";
import { API_URL } from "@/api/config";

export async function getTenants(): Promise<Tenant[]> {
    const response = await authFetch(`${API_URL}/api/tenants`);

    if (!response.ok) {
        throw new Error("Failed to fetch tenants");
    }

    return response.json();
}
