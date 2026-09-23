import type { Unit, UnitInput } from "@/types";
import { authFetch } from "@/lib/auth/authFetch";
import { API_URL } from "@/api/config";

export async function getUnit(unitId: string): Promise<Unit> {
    const response = await authFetch(`${API_URL}/api/units/${unitId}`)

    if(!response.ok) {
        throw new Error("Failed to fetch units");
    }

    return response.json();
}

export async function createUnit(unit: UnitInput, propertyId: string): Promise<Unit> {
    const response = await authFetch(`${API_URL}/api/units`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify({ ...unit, propertyId }),
    });

    if (!response.ok){
        throw new Error("Failed to create unit");
    }

    return response.json();
}

export async function updateUnit(unit: UnitInput, unitId: string): Promise<Unit> {
    const response = await authFetch(`${API_URL}/api/units/${unitId}`, {
        method: "PUT",
        headers:{
            "Content-Type": "application/json",
        },
        body: JSON.stringify(unit),
    });

    if (!response.ok){
        throw new Error("Faild to updated unit");
    }

    return response.json();
}

export async function deleteUnit(unitId: string): Promise<void> {
    const response = await authFetch(`${API_URL}/api/units/${unitId}`, {
        method: "DELETE",
    });

    if (!response.ok) {
        throw new Error("Failed to delete unit");
    }
}