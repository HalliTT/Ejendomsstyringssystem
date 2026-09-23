import type { Property, Properties, PropertyInput } from "@/types";
import { authFetch } from "@/lib/auth/authFetch";

export async function getProperties(): Promise<Properties[]> {
    const response = await authFetch("https://localhost:7119/api/properties");

    if(!response.ok) {
        throw new Error("Failed to fetch properties");
    }

    return response.json();
}

export async function getProperty(propertyId: string): Promise<Property> {
    const response = await authFetch(`https://localhost:7119/api/properties/${propertyId}`)

    if(!response.ok) {
        throw new Error("Failed to fetch properties");
    }

    return response.json();
}

export async function createProperty(property: PropertyInput): Promise<Property> {
    const response = await authFetch(`https://localhost:7119/api/properties`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(property),
    });

    if (!response.ok){
        throw new Error("Failed to create property");
    }

    return response.json();
}

export async function updateProperty(property: PropertyInput, propertyId: string): Promise<Property> {
    const response = await authFetch(`https://localhost:7119/api/properties/${propertyId}`, {
        method: "PUT",
        headers:{
            "Content-Type": "application/json",
        },
        body: JSON.stringify(property),
    });

    if (!response.ok){
        throw new Error("Faild to updated property");
    }

    return response.json();
}

export async function deleteProperty(propertyId: string): Promise<void> {
    const response = await authFetch(`https://localhost:7119/api/properties/${propertyId}`, {
        method: "DELETE",
    });

    if (!response.ok) {
        throw new Error("Failed to delete property");
    }
}