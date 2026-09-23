import type { RentalOption, RentalOptionFormInput, RentalOptionSummary } from "@/types";
import { authFetch } from "@/lib/auth/authFetch";
import { API_URL } from "@/api/config";

export async function getMyRentalOptions(): Promise<RentalOptionSummary[]> {
    const response = await authFetch(`${API_URL}/api/rental-options`);

    if (!response.ok) {
        throw new Error("Failed to fetch rental options");
    }

    return response.json();
}

export async function createRentalOption(input: RentalOptionFormInput, propertyId: string): Promise<RentalOption> {
    const response = await authFetch(`${API_URL}/api/rental-options`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify({ ...input, propertyId }),
    });

    if (!response.ok) {
        throw new Error("Failed to create rental option");
    }

    return response.json();
}

export async function updateRentalOption(input: RentalOptionFormInput, rentalOptionId: string): Promise<RentalOption> {
    const response = await authFetch(`${API_URL}/api/rental-options/${rentalOptionId}`, {
        method: "PUT",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(input),
    });

    if (!response.ok) {
        throw new Error("Failed to update rental option");
    }

    return response.json();
}

export async function deleteRentalOption(rentalOptionId: string): Promise<void> {
    const response = await authFetch(`${API_URL}/api/rental-options/${rentalOptionId}`, {
        method: "DELETE",
    });

    if (!response.ok) {
        throw new Error("Failed to delete rental option");
    }
}
